using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Gyro3dMonitor
{
    /// <summary>
    /// ジャイロセンサーモニタークラス定義
    /// </summary>
    public class GyroSensorMonitor
    {
        public float angleX { get; protected set; }
        public float angleY { get; protected set; }
        public float angleZ { get; protected set; }
        

        public enum lineState { OFFLINE, ONLINE_WAIT, ONLINE_RECIEVE, ONLINE_CANCEL, ONLINE_TIMEOUT }; // 通信状態定義
        private lineState _onlineState; // 通信状態
        public lineState State
        {
            get => _onlineState;
            protected set
            {
                _onlineState = value;
                OnChangedState(EventArgs.Empty);
            }

        }
        public event EventHandler? ChangedState;
        protected virtual void OnChangedState(EventArgs e)
        {
            ChangedState?.Invoke(this, e);
        }
        public CancellationTokenSource? cancelToken = null;
        private TcpListener? server = null;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GyroSensorMonitor()
        {
            cancelToken = new CancellationTokenSource();
        }
        /// <summary>
        /// ジャイロセンサーの通信サーバー
        /// </summary>
        /// <param name="port">通信ポート</param>
        /// <param name="timeout">通信タイムアウト(秒)</param>
        /// <returns></returns>
        public async Task StartServer(int port, int timeout)
        {
            // 既存のキャンセルトークンをリセット
            cancelToken?.Dispose(); // 以前のトークンを解放
            cancelToken = new CancellationTokenSource();

            // 指定タイムアウト後にキャンセル
            using CancellationTokenSource cts = new CancellationTokenSource();
            TimeSpan timespan = TimeSpan.FromSeconds(timeout);

            try
            {
                // 通信サーバーの生存確認
                if (server != null)
                {
                    server.Stop();
                    server.Dispose();
                    server = null;
                }
                // 指定ポートに対してTCPリスナー開始
                server = new TcpListener(IPAddress.Any, port);
                server.Start();

                State = lineState.ONLINE_WAIT;
                while (true)
                {
                    if (cancelToken.Token.IsCancellationRequested)
                    {
                        State = lineState.OFFLINE;
                        break;
                    }
                    // 非同期でタイムアウト付き受信待ち
                    TcpClient? client = await AcceptClientWithTimeout(server, timespan, cancelToken.Token);
                    if (client == null)
                    {
                        State = (cancelToken.Token.IsCancellationRequested) ? lineState.ONLINE_CANCEL : lineState.ONLINE_TIMEOUT;
                        break;
                    }
                    else
                    {
                        // 非同期で受信処理
                        _ = ProcessClientAsync(client, cancelToken.Token); // クライアントごとに非同期処理
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show($"{Resources.Resource.msgView_3} {ex.Message}", "GyroSensorMonitor", MessageBoxButtons.OK, MessageBoxIcon.Information); //"通信を中断しました。 {ex.Message}"
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                MessageBox.Show($"{Resources.Resource.msgView_4} {ex.Message}", "GyroSensorMonitor", MessageBoxButtons.OK, MessageBoxIcon.Information); //"通信を中断されました。 {ex.Message}"
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Resources.Resource.msgView_5} {ex.Message}", "GyroSensorMonitor", MessageBoxButtons.OK, MessageBoxIcon.Information);  //"ソケットエラー:"
            }
            finally
            {
                State = lineState.OFFLINE;
                server?.Stop();
                server?.Dispose();
            }
        }
        /// <summary>
        /// タイムアウト付き受信処理
        /// </summary>
        /// <param name="server">通信サーバー</param>
        /// <param name="timeout">通信タイムアウト(秒)</param>
        /// <param name="token">キャンセルトークン</param>
        /// <returns>通信クライアント</returns>
        private async Task<TcpClient?> AcceptClientWithTimeout(TcpListener server, TimeSpan timeout, CancellationToken token)
        {
            Task<TcpClient> acceptTask = server.AcceptTcpClientAsync();
            Task delayTask = Task.Delay(timeout, token); // タイムアウト待機タスク

            Task completedTask = await Task.WhenAny(acceptTask, delayTask);

            if (completedTask == acceptTask)
            {
                return acceptTask.Result; // クライアントを返す
            }

            return null; // タイムアウト発生時は null を返す
        }

        /// <summary>
        /// ジャイロセンサーの受信したデータの処理
        /// </summary>
        /// <param name="client">通信クライアント</param>
        /// <param name="token">キャンセルトークン</param>
        /// <returns>タスク</returns>
        private async Task ProcessClientAsync(TcpClient client, CancellationToken token)
        {
            using (client)
            {
                try
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] buffer = new byte[1024];
                        while (true)
                        {
                            if (token.IsCancellationRequested)
                            {
                                State = lineState.OFFLINE;
                                break;
                            }
                            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                            if (bytesRead == 0) break;

                            string jsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            var sensorData = JsonSerializer.Deserialize<SensorData>(jsonData);

                            if (sensorData == null) continue;
                            // OpenGL座標系の場合
                            //angleX = sensorData.angleX;
                            //angleY = sensorData.angleY;
                            //angleZ = sensorData.angleZ;
                            // Raspberry Pi(Z軸)の座標系をOpenGL(Y軸)への座標系変更
                            angleX = -sensorData.angleX;
                            angleY = sensorData.angleZ;
                            angleZ = sensorData.angleY;
                            State = lineState.ONLINE_RECIEVE;
                        }
                    }
                }
                catch (Exception)
                {
                    // 上位に例外を返す
                    throw;
                }
            }
        }
        /// <summary>
        /// 通信サーバーの停止と解放処理
        /// </summary>
        public void StopServer()
        {
            if (cancelToken != null) cancelToken.Cancel();
            server?.Stop();
            server?.Dispose();
            server = null;
        }
    }
    /// <summary>
    /// ジャイロセンサーの受信データクラス
    /// </summary>
    public class SensorData
    {
        public float angleX { get; set; }
        public float angleY { get; set; }
        public float angleZ { get; set; }
    }
}
