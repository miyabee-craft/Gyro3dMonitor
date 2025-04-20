using OpenTK;
using OpenTK.Graphics.OpenGL;
using SurfaceAnalyzer;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Gyro3dMonitor
{
    /// <summary>
    /// ビューワフォームクラス定義
    /// </summary>
    public partial class Viewer : Form
    {
        // 親フォーム
        private Form_Main? control = null;

        // ステイタスストリップに閉じるボタンを配置するインスタンス
        private ToolStripButton toolStripButton;

        // glControlのインスタンス
        private GLControl? glControl = null;
        private int textureID = -1;
        private bool quaternionEnable = true;
        private Quaternion rotation = Quaternion.Identity;

        // カメラ回転
        private float zoom = 0.20f;         //拡大率
        private Vector3 eye;                //カメラ視線ベクトル
        private Vector3 eyeUp;              //カメラ視線の上ベクトル
        private bool isCameraRotating;      //カメラの回転状態
        private Vector2 current, previous;  //マウス現在点、前回点
        private double rotateX = 1.0,
                       rotateY = 0.0,
                       rotateZ = 0.0;       //カメラの回転による移動（オイラー角）
        private float thetaU = 0.0f,
                      thetaW = 0.0f;        //カメラの回転による移動（クォータニオン）
        private float theta = 0;
        private float phi = 0;

        // モデルポリゴンデータ
        private PolygonModel? Polygon = null;

        public CancellationTokenSource? cancelToken = null; // 通信キャンセルトークン
        private int startTick, elapsed;                     // 受信待ち時間
        public System.Windows.Forms.Timer? timer = null;    // 受信待ちタイマー

        public GyroSensorMonitor? sensorMonitor = null;

        /// <summary>
        /// ビューワフォームのコンストラクタ
        /// </summary>
        /// <param name="parent">メインフォームのオブジェクト</param>
        public Viewer(Form_Main parent)
        {
            control = parent;
            InitializeComponent();

            // ステイタスストリップに閉じるボタンを配置するインスタンス生成
            toolStripButton = new ToolStripButton();
            toolStripButton.Text = Resources.Resource.button_Close;     // "閉じる"
            toolStripButton.Click += ToolStripButton_Click;             // クリックイベント設定
            statusStrip1.Items.Add(toolStripButton);

            //ビューサイズ
            this.ClientSize = new Size(control.viewport.Width - 16, control.viewport.Hight - 39);

            //回転変換方式の選択
            quaternionEnable = (control.rotationMethod == 0) ? true : false;

            AddglControl();
            // 親フォームの右端に子フォームを配置
            this.StartPosition = FormStartPosition.Manual;
            this.Left = control.Right - 15; // 親フォームの右端に配置
            this.Top = control.Top; // 親フォームの上端と揃える
            this.Polygon = control.polygon; // 読み込んだポリゴンデータ

            sensorMonitor = new();

            // タイマーの設定（1秒ごとに実行）
            timer = new();
            timer.Interval = 10;      // 1000ms = 1秒
            timer.Tick += Timer_Tick;   // イベントハンドラ登録

            // イベント登録
            sensorMonitor.ChangedState += SensorMonitor_ChangedState;
        }
        /// <summary>
        /// ビューワフォームのロード
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Viewer_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel_StateMonitor.Text = Resources.Resource.StateMonitor_Offline;   // "オフライン";
        }
        /// <summary>
        /// glControlコントロールをフォームに追加
        /// </summary>
        private void AddglControl()
        {
            // フォームレイアウト更新の一時停止
            SuspendLayout();

            //GLControlの初期化
            glControl = new GLControl
            {
                Name = "Miyabee-Craft 3D Model Viewer",
                Size = new Size(this.Width, this.Height),
                Location = new System.Drawing.Point(0, 0),
                Dock = DockStyle.Fill
            };
            // ビューを最背面に移動（UIコントロールを前面に移動）
            glControl.SendToBack();

            //イベントハンドラ
            glControl.Load += new EventHandler(glControl_Load);
            glControl.Resize += new EventHandler(Viewer_Resize);
            glControl.MouseDown += new MouseEventHandler(Viewer_MouseDown);
            glControl.MouseUp += new MouseEventHandler(Viewer_MouseUp);
            if (quaternionEnable)
            {
                glControl.MouseMove += new MouseEventHandler(Viewer_MouseMoveByQuaternion);
                glControl.MouseWheel += new MouseEventHandler(Viewer_MouseWheelByQuaternion);
            }
            else
            {
                glControl.MouseMove += new MouseEventHandler(Viewer_MouseMoveByEuler);
                glControl.MouseWheel += new MouseEventHandler(Viewer_MouseWheelByEuler);
            }

            // Viewフォームに追加
            Controls.Add(glControl);

            // フォームレイアウト更新の再開
            ResumeLayout(false);
        }
        /// <summary>
        /// glControlコントロールのロード
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void glControl_Load(object? sender, EventArgs e)
        {
            if (glControl == null || control == null) return;

            GL.Enable(EnableCap.DepthTest);             // Zバッファを有効にする
            SetupViewport();                            // ビューポートの設定
            //ライティングとマテリアルの反射色の設定
            if (!control.priorityColor)
            {
                SetupLighting();
            }

            SetupBackground();
            SetupCamera();
        }
        /// <summary>
        /// ビューポートの設定 
        /// </summary>
        private void SetupViewport()
        {
            if (control == null || glControl == null) return;

            int width = glControl.Size.Width;
            int hight = glControl.Size.Height;

            GL.Viewport(0, 0, width, hight);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            // 透視投影行列の作成
            // fov(視野角）：45°（小：ズームイン効果大、大：広角効果大）
            // aspect ratio：画面サイズより算出
            // near（手前クリッピング面）： 1.0より手前のオブジェクトは表示しない
            // far（奥のクリッピング面）：256.0より奥のオブジェクトは表示しない
            // near,farはオブジェクトのサイズやオブジェクトを配置する広さによって決める。
            float fov = control.viewport.FOV / 360.0f * (float)Math.PI;
            float aspect = (float)width / (float)hight;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, control.viewport.near, control.viewport.far);
            GL.LoadMatrix(ref perspective);
        }
        /// <summary>
        /// 背景の設定 
        /// </summary>
        private void SetupBackground()
        {
            if (glControl == null || control == null) return;

            // OpenGLのコンテキストをglControlにバインドする
            glControl.MakeCurrent();
            if (control.backcolorMode)
            {
                // 背景色 RGB指定
                GL.ClearColor(control.backcolorRGBA[0] / 255,
                              control.backcolorRGBA[1] / 255,
                              control.backcolorRGBA[2] / 255,
                              control.backcolorRGBA[3] / 255);
            }
            else
            {
                GL.ClearColor(System.Drawing.Color.Black);  // 背景色を設定
                // 背景色 Texture
                if (textureID < 0 && !string.IsNullOrEmpty(control.textureFile))
                {
                    // テクスチャの読込み
                    textureID = LoadTexture(control.textureFile);
                    GL.Enable(EnableCap.Texture2D);         // 2Dテクスチャを有効化
                    GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Modulate);
                }
            }
        }
        /// <summary>
        /// ライティングの設定
        /// </summary>
        private void SetupLighting()
        {
            if (control == null) return;

            // ライティングの有効化
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.AmbientAndDiffuse);

            //ライティング設定
            float[] rgba = new float[4];
            for (int i = 0; i < rgba.Length; i++)
            {
                rgba[i] = control.lightingRGBA[i];
                rgba[i] /= 255;
            }
            float[] lightColor = { rgba[0], rgba[1], rgba[2], rgba[3] };
            // 上からのライティングポイントの定義
            float[] lightPos0 = { 0.0f, 10000.0f, 0.0f, 0.0f };
            float[] lightPos2 = { 0.0f, 0.0f, 10000.0f, 0.0f };

            // ライティングの設定
            GL.Light(LightName.Light0, LightParameter.Position, lightPos0);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightColor);
        }
        /// <summary>
        /// 初期カメラ設定 
        /// </summary>
        private void SetupCamera()
        {
            if (control == null || control.viewport.VP == null || Polygon == null) return;

            // 視点位置はポリゴンデータから自動設定する
            Vector3 vec_rotate = new((float)rotateX, (float)rotateY, (float)rotateZ);
            Vector3 center = new(N2TK(Polygon.GravityPoint()));
            eye = center + vec_rotate * center.LengthFast / zoom;
            Matrix4 modelView = Matrix4.LookAt(eye, center, Vector3.UnitY);
            control.viewport.VP[0] = eye.X;
            control.viewport.VP[1] = eye.Y;
            control.viewport.VP[2] = eye.Z;

            // 表示設定
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelView);
        }
        /// <summary>
        /// 影の描画
        /// </summary>
        /// <param name="lightPos">光源の位置</param>
        /// <param name="groundPlane">床の平面</param>
        private void DrawShadow(Vector3 lightPos, Vector4 groundPlane)
        {
            // 光源の位置
            Vector4 lightPosition = new(lightPos, 0.0f);

            // シャドウ投影行列を取得
            Matrix4 shadowMatrix = GetShadowMatrix(lightPosition, groundPlane);

            // 影が真っ黒となり床の模様が消えてしまう不自然さを回避するためにαブレンディングを行う
            // αブレンドとは、ソース画像（影）と背景画像（床）のピクセルごとに透過度（α値）に基づいて合成する処理
            // α値は0～255で完全透明から完全不透明までを表す。
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            // 影の描画前にライティングを無効化
            GL.Disable(EnableCap.Lighting);
            GL.PolygonOffset(-1.0f, -1.0f);
            GL.Enable(EnableCap.PolygonOffsetFill);
            // 影の描画
            GL.PushMatrix();
            GL.MultMatrix(ref shadowMatrix);
            GL.Color4(0.0f, 0.0f, 0.0f, 0.5f); // 半透明の影の色を設定
            DrawShadowPolygons();

            GL.PopMatrix();
            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.Lighting);
            GL.Disable(EnableCap.PolygonOffsetFill);
        }

        /// <summary>
        /// 影の投影行列の取得
        /// Note: a Lx + b Ly + c Lz > 0 must be satisfied (normal (a,b,c) directs to light side
        /// </summary>
        /// <param name="lightPos">光源の位置</param>
        /// <param name="groundPlane">床の平面</param>
        /// <returns>4ｘ4行列</returns>
        private Matrix4 GetShadowMatrix(Vector4 lightPos, Vector4 groundPlane)
        {
            float ip; // inner product

            //ip = a * Lx + b * Ly + c * Lz + d * Lw;  /* N^T L */
            ip = Vector4.Dot(lightPos, groundPlane);
            // projection matrix = N^T L I - L N^T
            Matrix4 shadowMatrix = new(
                -groundPlane.X * lightPos.X + ip, -groundPlane.Y * lightPos.X, -groundPlane.Z * lightPos.X, -groundPlane.W * lightPos.X,
                -groundPlane.X * lightPos.Y, -groundPlane.Y * lightPos.Y + ip, -groundPlane.Z * lightPos.Y, -groundPlane.W * lightPos.Y,
                -groundPlane.X * lightPos.Z, -groundPlane.Y * lightPos.Z, -groundPlane.Z * lightPos.Z + ip, -groundPlane.W * lightPos.Z,
                -groundPlane.X * lightPos.W, -groundPlane.Y * lightPos.W, -groundPlane.Z * lightPos.W, -groundPlane.W * lightPos.W + ip
            );
            return shadowMatrix;
        }
        /// <summary>
        /// 指定したウィンドウ座標 (x, y) のピクセルの色を取得する。
        /// </summary>
        /// <param name="x">取得するピクセルの X 座標（ウィンドウ座標）</param>
        /// <param name="y">取得するピクセルの Y 座標（ウィンドウ座標）</param>
        /// <returns>取得した色（Color 型）</returns>
        public Color GetPixelColor(int x, int y)
        {
            byte[] pixel = new byte[4]; // RGBA 各1バイト

            GL.ReadPixels(x, y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, pixel);

            return Color.FromArgb(pixel[3], pixel[0], pixel[1], pixel[2]);
        }
        /// <summary>
        /// 描画の更新
        /// </summary>
        public new void Update()
        {
            if (Polygon == null) return;
            SetupLighting();
            SetupBackground();
            Render(quaternionEnable);
            DisplayState();
        }
        /// <summary>
        /// レンダー処理
        /// </summary>
        /// <param name="mode"></param>
        public void Render(bool mode)
        {
            if (mode == true)
            {
                // クォータニオン
                RenderByQuaternion();
            }
            else
            {
                // オイラー角
                RenderByEuler();
            }
        }
        /// <summary>
        /// レンダー（描画）（オイラー角による回転方式）
        /// </summary>
        public void RenderByEuler()
        {
            if (glControl == null || control == null || Polygon == null) return;

            //// カメラ設定
            Vector3 vec_rotate = new Vector3((float)rotateX, (float)rotateY, (float)rotateZ);
            Vector3 center = new Vector3(N2TK(Polygon.GravityPoint()));
            eye = center + vec_rotate * center.LengthFast / zoom;
            Matrix4 modelView = Matrix4.LookAt(eye, center, Vector3.UnitY);

            // 表示設定
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelView);

            // バッファのクリア
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // アンチエイリアシングを有効化 
            GL.Enable(EnableCap.Multisample);

            // 透明度アルファブレンドを有効化
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // オブジェクトの外側（見える側）のみ描画するようにカリングファイスを有効化する
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.AutoNormal);
            GL.Enable(EnableCap.Normalize);

            if (!control.backcolorMode && textureID > 0)
            {
                // 白色にしてテクスチャの色を再現する
                GL.Color3(1.0f, 1.0f, 1.0f);
                // テクスチャ対象を設定する
                GL.BindTexture(TextureTarget.Texture2D, textureID);
                // 画面全体を覆う四角形（クワッド）を描画
                GL.Begin(PrimitiveType.Quads);
                {
                    // YZ平面にテクスチャを描画
                    GL.TexCoord2(0.0, 0.0); GL.Vertex3(-50.0, 100.0, -150.0);   // 左下
                    GL.TexCoord2(1.0, 0.0); GL.Vertex3(-50.0, 100.0, 150.0);    // 右下
                    GL.TexCoord2(1.0, 1.0); GL.Vertex3(-50.0, -100.0, 150.0);   // 右上
                    GL.TexCoord2(0.0, 1.0); GL.Vertex3(-50.0, -100.0, -150.0);  // 左上
                }
                GL.End();
                // テクスチャ設定対象を無効にする
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            GL.PushMatrix();  // ワールド座標系を保存
            {
                if (control.skydomeEnable)
                {
                    // スカイドームは内側（見える側）のためカリングフェイスを無効化する
                    GL.Disable(EnableCap.CullFace);
                    {
                        DrawSkyDome(200, 200, control.skydomeRadius);
                    }
                    GL.Enable(EnableCap.CullFace);
                }
                // メッシュ床の描画
                if (control.meshgroundEnable) DrawMeshGround(200.0f, 10, control.groundHight);
                // 市松模様床の描画
                if (control.checkgroundEnable) DrawCheckPattern(200.0f, 10, control.groundHight);

                // ジャイロ情報をオブジェクトに反映---------------------------
                // 1. 平行移動（オブジェクトを原点に移動）
                GL.Translate(center);

                // 2. 回転の適用(ャイロ情報を反映)
                if(sensorMonitor != null)
                {
                    GL.Rotate(sensorMonitor.angleX, 1.0f, 0.0f, 0.0f);
                    GL.Rotate(sensorMonitor.angleY, 0.0f, 1.0f, 0.0f);
                    GL.Rotate(sensorMonitor.angleZ, 0.0f, 0.0f, 1.0f);
                    toolStripStatusLabel_Angle.Text = $"Roll, Pitch, Yaw: {-sensorMonitor.angleX,+6:F3} ,  {sensorMonitor.angleZ,+6:F3} ,  {sensorMonitor.angleY,+6:F3}";
                }
                // 3. 元の位置に戻す
                GL.Translate(-center);
                //-----------------------------------------------------------

                // オブジェクトの描画
                DrawPolygons();
                // 影の描画
                //float[] lightPos0 = new float[3];
                //GL.GetLight(LightName.Light0, LightParameter.Position, lightPos0);
                Vector3 lightPos = new(0, 200, 0);
                Vector4 groundPlane = new(0, 1, 0, -control.groundHight);
                if (control.shadowEnable) DrawShadow(lightPos, groundPlane);
            }
            GL.PopMatrix();  // ワールド座標系を復元
            // ワールド座標系の座標軸を常に最上面に表示
            if (control.coordinateAxesEnable) CoordinateAxes();

            // バッファの入れ替え
            glControl.SwapBuffers();
        }
        /// <summary>
        /// レンダー（描画）（クォータニオンによる回転方式）
        /// </summary>
        public void RenderByQuaternion()
        {
            if (glControl == null || control == null || Polygon == null) return;

            // バッファのクリア
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // アンチエイリアシングを有効化 
            GL.Enable(EnableCap.Multisample);

            // 透明度アルファブレンドを有効化
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // オブジェクトの外側（見える側）のみ描画するようにカリングファイスを有効化する
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.AutoNormal);
            GL.Enable(EnableCap.Normalize);

            if (!control.backcolorMode && textureID > 0)
            {
                // 白色にしてテクスチャの色を再現する
                GL.Color3(1.0f, 1.0f, 1.0f);
                // テクスチャ対象を設定する
                GL.BindTexture(TextureTarget.Texture2D, textureID);
                // 画面全体を覆う四角形（クワッド）を描画
                GL.Begin(PrimitiveType.Quads);
                {
                    // YZ平面にテクスチャを描画
                    GL.TexCoord2(0.0, 0.0); GL.Vertex3(-50.0, 100.0, -150.0);   // 左下
                    GL.TexCoord2(1.0, 0.0); GL.Vertex3(-50.0, 100.0, 150.0);    // 右下
                    GL.TexCoord2(1.0, 1.0); GL.Vertex3(-50.0, -100.0, 150.0);   // 右上
                    GL.TexCoord2(0.0, 1.0); GL.Vertex3(-50.0, -100.0, -150.0);  // 左上
                }
                GL.End();
                // テクスチャ設定対象を無効にする
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            GL.PushMatrix();  // ワールド座標系を保存
            {
                if (control.skydomeEnable)
                {
                    // スカイドームは内側（見える側）のためカリングフェイスを無効化する
                    GL.Disable(EnableCap.CullFace);
                    {
                        DrawSkyDome(200, 200, control.skydomeRadius);
                    }
                    GL.Enable(EnableCap.CullFace);
                }
                // メッシュ床の描画
                if (control.meshgroundEnable) DrawMeshGround(200.0f, 10, control.groundHight);
                // 市松模様床の描画
                if (control.checkgroundEnable) DrawCheckPattern(200.0f, 10, control.groundHight);

                // ジャイロ情報をオブジェクトに反映---------------------------
                // 1. 平行移動（オブジェクトを原点に移動）
                Vector3 center = new(N2TK(Polygon.GravityPoint()));
                GL.Translate(center);

                // 2. 回転の適用(ジャイロ情報を反映)
                if(sensorMonitor != null)
                {
                    GL.Rotate(sensorMonitor.angleX, 1.0f, 0.0f, 0.0f);
                    GL.Rotate(sensorMonitor.angleY, 0.0f, 1.0f, 0.0f);
                    GL.Rotate(sensorMonitor.angleZ, 0.0f, 0.0f, 1.0f);
                    toolStripStatusLabel_Angle.Text = $"Roll, Pitch, Yaw: {-sensorMonitor.angleX,+6:F3} ,  {sensorMonitor.angleZ,+6:F3} ,  {sensorMonitor.angleY,+6:F3}";
                }
                // 3. 元の位置に戻す
                GL.Translate(-center);
                //-----------------------------------------------------------

                // オブジェクトの描画
                DrawPolygons();
                // 影の描画
                //float[] lightPos0 = new float[3];
                //GL.GetLight(LightName.Light0, LightParameter.Position, lightPos0);
                Vector3 lightPos = new(0, 200, 0);
                Vector4 groundPlane = new(0, 1, 0, -control.groundHight);
                if (control.shadowEnable) DrawShadow(lightPos, groundPlane);

            }
            GL.PopMatrix();  // ワールド座標系を復元
            // ワールド座標系の座標軸を常に最上面に表示
            if (control.coordinateAxesEnable) CoordinateAxes();

            // バッファの入れ替え
            glControl.SwapBuffers();
        }
        /// <summary>
        /// ジャイロセンサーモニター更新タイマー
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            Update();
        }
        /// <summary>
        /// モニターのオンライン状態イベント処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void SensorMonitor_ChangedState(object? sender, EventArgs e)
        {
            if(sensorMonitor == null) return;
            //オンライン状態の表示
            switch (sensorMonitor.State)
            {
                case GyroSensorMonitor.lineState.OFFLINE:
                    toolStripStatusLabel_StateMonitor.Text = Resources.Resource.StateMonitor_Offline;   // "オフライン";
                    toolStripStatusLabel_StateMonitor.ForeColor = Color.White;
                    toolStripStatusLabel_StateMonitor.BackColor = Color.Red;
                    break;
                case GyroSensorMonitor.lineState.ONLINE_WAIT:
                    toolStripStatusLabel_StateMonitor.ForeColor = Color.Black;
                    toolStripStatusLabel_StateMonitor.BackColor = Color.Yellow;
                    break;
                case GyroSensorMonitor.lineState.ONLINE_RECIEVE:
                    toolStripStatusLabel_StateMonitor.Text = Resources.Resource.StateMonitor_Receiving; // "受信中";
                    toolStripStatusLabel_StateMonitor.ForeColor = Color.White;
                    toolStripStatusLabel_StateMonitor.BackColor = Color.Green;
                    break;
                case GyroSensorMonitor.lineState.ONLINE_CANCEL:
                    toolStripStatusLabel_StateMonitor.Text = Resources.Resource.StateMonitor_Cancel; // "キャンセル";
                    toolStripStatusLabel_StateMonitor.ForeColor = Color.White;
                    toolStripStatusLabel_StateMonitor.BackColor = Color.DeepPink;
                    MessageBox.Show($"{Resources.Resource.msgView_1}", "GyroSensorMonitor", MessageBoxButtons.OK, MessageBoxIcon.Error); //"ジャイロモニタを中断しました。"
                    break;
                case GyroSensorMonitor.lineState.ONLINE_TIMEOUT:
                    toolStripStatusLabel_StateMonitor.Text = Resources.Resource.StateMonitor_Timeout; // "タイムアウト"
                    toolStripStatusLabel_StateMonitor.ForeColor = Color.White;
                    toolStripStatusLabel_StateMonitor.BackColor = Color.Violet;
                     MessageBox.Show($"{Resources.Resource.msgView_2}", "GyroSensorMonitor", MessageBoxButtons.OK, MessageBoxIcon.Error); //"ジャイロセンサを受信できませんでした(タイムアウト)。"
                    break;
            }
        }
        /// <summary>
        /// 状態の表示更新
        /// </summary>
        private void DisplayState()
        {
            //Eyeの表示
            toolStripStatusLabel_Eye.Text = $"Eye: {eye.X,+6:F3}, {eye.Y,+6:F3}, {eye.Z,+6:F3}";
            //ズームの表示
            toolStripStatusLabel_Zoom.Text = $" Zoom: {zoom:F3}";
            if (quaternionEnable)
            {
                //カメラ回転角の表示（クォータニオン）
                toolStripStatusLabel_Rotate.Text = $" Rotation(θu, θw): {thetaU,+6:F3}, {thetaW,+6:F3}";
            }
            else
            {
                //カメラ回転角の表示（オイラー角）
                toolStripStatusLabel_Rotate.Text = $" Rotation(X, Y, Z): {rotateX,+6:F3}, {rotateY,+6:F3}, {rotateZ,+6:F3}";
            }

            //受信待ち状態表示
            if(sensorMonitor != null && sensorMonitor.State == GyroSensorMonitor.lineState.ONLINE_WAIT)
            {
                elapsed = (Environment.TickCount - startTick) / 1000;     // 秒
                toolStripStatusLabel_StateMonitor.Text = $"{Resources.Resource.StateMonitor_Waiting}({elapsed}{Resources.Resource.StateMonitor_Receiving_Unit})"; // "受信待ち"
                toolStripStatusLabel_StateMonitor.ForeColor = Color.Black;
                toolStripStatusLabel_StateMonitor.BackColor = Color.Yellow;
            }
        }
        /// <summary>
        /// テクスチャ画像のロード
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns></returns>
        public int LoadTexture(string path)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bitmap = new(path);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                              ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0,
                          OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            // テクスチャフィルタリング設定
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            return id;
        }
        /// <summary>
        /// ワールド座標系の座標軸を常に最上面に表示
        /// </summary>
        private void CoordinateAxes()
        {
            // 深度テストを無効化（座標軸を最前面に描画するため）
            GL.Disable(EnableCap.DepthTest);
            GL.DepthMask(false);

            GL.LineWidth(2.0f); // 軸の太さを設定

            GL.Begin(PrimitiveType.Lines);
            {
                // X軸（赤）
                GL.Color3(1.0f, 0.0f, 0.0f);
                GL.Vertex3(0.0f, 0.0f, 0.0f);
                GL.Vertex3(1.0f, 0.0f, 0.0f);

                // Y軸（緑）
                GL.Color3(0.0f, 1.0f, 0.0f);
                GL.Vertex3(0.0f, 0.0f, 0.0f);
                GL.Vertex3(0.0f, 1.0f, 0.0f);

                // Z軸（青）
                GL.Color3(0.0f, 0.0f, 1.0f);
                GL.Vertex3(0.0f, 0.0f, 0.0f);
                GL.Vertex3(0.0f, 0.0f, 1.0f);
            }
            GL.End();
            GL.Begin(PrimitiveType.Polygon);
            {
                // Y座標軸の矢印
                GL.Color3(0.0f, 1.0f, 0.0f);
                GL.Vertex3(0.0f, 1.3f, 0.0f);
                GL.Vertex3(-0.1f, 1.0f, -0.1f);
                GL.Vertex3(0.1f, 1.0f, -0.1f);

                GL.Vertex3(0.0f, 1.3f, 0.0f);
                GL.Vertex3(0.1f, 1.0f, -0.1f);
                GL.Vertex3(0.1f, 1.0f, 0.1f);

                GL.Vertex3(0.0f, 1.3f, 0.0f);
                GL.Vertex3(0.1f, 1.0f, 0.1f);
                GL.Vertex3(-0.1f, 1.0f, 0.1f);

                GL.Vertex3(0.0f, 1.3f, 0.0f);
                GL.Vertex3(-0.1f, 1.0f, 0.1f);
                GL.Vertex3(-0.1f, 1.0f, -0.1f);
            }
            GL.End();
            GL.Begin(PrimitiveType.Polygon);
            {
                // X座標軸の矢印
                GL.Color3(1.0f, 0.0f, 0.0f);
                GL.Vertex3(1.3f, 0.0f, 0.0f);
                GL.Vertex3(1.0f, -0.1f, -0.1f);
                GL.Vertex3(1.0f, 0.1f, -0.1f);

                GL.Vertex3(1.3f, 0.0f, 0.0f);
                GL.Vertex3(1.0f, 0.1f, -0.1f);
                GL.Vertex3(1.0f, 0.1f, 0.1f);

                GL.Vertex3(1.3f, 0.0f, 0.0f);
                GL.Vertex3(1.0f, 0.1f, 0.1f);
                GL.Vertex3(1.0f, -0.1f, 0.1f);

                GL.Vertex3(1.3f, 0.0f, 0.0f);
                GL.Vertex3(1.0f, -0.1f, 0.1f);
                GL.Vertex3(1.0f, -0.1f, -0.1f);
            }
            GL.End();
            GL.Begin(PrimitiveType.Polygon);
            {
                // Z座標軸の矢印
                GL.Color3(0.0f, 0.0f, 1.0f);
                GL.Vertex3(0.0f, 0.0f, 1.3f);
                GL.Vertex3(-0.1f, -0.1f, 1.0f);
                GL.Vertex3(0.1f, -0.1f, 1.0f);

                GL.Vertex3(0.0f, 0.0f, 1.3f);
                GL.Vertex3(0.1f, -0.1f, 1.0f);
                GL.Vertex3(0.1f, 0.1f, 1.0f);

                GL.Vertex3(0.0f, 0.0f, 1.3f);
                GL.Vertex3(0.1f, 0.1f, 1.0f);
                GL.Vertex3(-0.1f, 0.1f, 1.0f);

                GL.Vertex3(0.0f, 0.0f, 1.3f);
                GL.Vertex3(-0.1f, 0.1f, 1.0f);
                GL.Vertex3(-0.1f, -0.1f, 1.0f);
            }
            GL.End();

            // 深度テストを再び有効化
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
        }
        /// <summary>
        /// 青空ドームの描画
        /// </summary>
        /// <param name="slices">経度方法の分割数</param>
        /// <param name="stacks">緯度方法の分割数</param>
        /// <param name="radius">半径</param>
        private void DrawSkyDome(int slices, int stacks, float radius)
        {
            for (int i = 0; i < stacks; i++)  // 球面の描画
            {
                double lat0 = Math.PI * (0.5 + (double)(i) / stacks);       // 緯度（開始）
                double lat1 = Math.PI * (0.5 + (double)(i + 1) / stacks);   // 緯度（次のステップ）
                float y0 = (float)(Math.Sin(lat0) * radius);
                float y1 = (float)(Math.Sin(lat1) * radius);
                float r0 = (float)(Math.Cos(lat0) * radius);
                float r1 = (float)(Math.Cos(lat1) * radius);

                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j <= slices; j++)
                {
                    double lng = 2 * Math.PI * (double)(j) / slices;　// 経度
                    float x = (float)(Math.Cos(lng));
                    float z = (float)(Math.Sin(lng));

                    // グラデーション計算
                    float gradientFactor = (float)(i + 1) / stacks; // 0.0（頂点）→ 1.0（底）

                    float red = 0.1f + 0.9f * gradientFactor; // 群青（0.1）→ 白（1.0）
                    float green = 0.1f + 0.9f * gradientFactor; // 群青（0.1）→ 白（1.0）
                    float blue = 0.6f + 0.4f * gradientFactor; // 群青（0.6）→ 白（1.0）

                    GL.Color3(red, green, blue);

                    GL.Vertex3(x * r1, y1, z * r1);
                    GL.Vertex3(x * r0, y0, z * r0);
                }
                GL.End();
            }
        }
        /// <summary>
        /// メッシュグランドの描画
        /// </summary>
        /// <param name="size">大きさ</param>
        /// <param name="divisions">メッシュの数</param>
        /// <param name="hight">高さ</param>
        private void DrawMeshGround(float size, int divisions, float hight)
        {
            float step = size / divisions;

            GL.Color3(0.4f, 0.4f, 0.4f); // ラインカラー（薄いグレー）
            GL.Begin(PrimitiveType.Lines);

            // X方向のライン
            for (int i = -divisions; i <= divisions; i++)
            {
                float pos = i * step;
                GL.Vertex3(pos, hight, -size);
                GL.Vertex3(pos, hight, size);
            }

            // Z方向のライン
            for (int i = -divisions; i <= divisions; i++)
            {
                float pos = i * step;
                GL.Vertex3(-size, hight, pos);
                GL.Vertex3(size, hight, pos);
            }

            GL.End();
        }
        /// <summary>
        /// 市松模様床の描画
        /// </summary>
        /// <param name="size">大きさ</param>
        /// <param name="divisions">メッシュの数</param>
        /// <param name="hight">高さ</param>
        private void DrawCheckPattern(float size, int divisions, float hight)
        {
            float cellSize = size / divisions;  // 1マスの大きさ

            GL.Begin(PrimitiveType.Quads);
            for (int i = -divisions; i <= divisions; i++)
            {
                for (int j = -divisions; j <= divisions; j++)
                {
                    float x = i * cellSize;
                    float z = j * cellSize;

                    // 市松模様の色設定（偶数：黒、奇数：白）
                    if ((i + j) % 2 == 0)
                        GL.Color3(0.2f, 0.2f, 0.2f); // 黒 (ダークグレー)
                    else
                        GL.Color3(0.8f, 0.8f, 0.8f); // 白 (ライトグレー)

                    // 四角形（タイル）の描画
                    GL.Vertex3(x, hight, z);
                    GL.Vertex3(x, hight, z + cellSize);
                    GL.Vertex3(x + cellSize, hight, z + cellSize);
                    GL.Vertex3(x + cellSize, hight, z);
                }
            }
            GL.End();
        }
        /// <summary>
        /// 詠み込んだポリゴンの描画
        /// </summary>
        private void DrawPolygons()
        {
            if (Polygon == null) return;

            //描画
            GL.Begin(PrimitiveType.Triangles);
            {
                //ファセット（三角形）を描画
                for (int l = 0; l < Polygon.Faces.Count; l++)
                {
                    // ファセットに対する法線ベクトル
                    var normal = Polygon.Faces[l].Normal();
                    // 光源リスト（3つの光源）
                    List<Vector3> lightSources = new()
                    {
                        // Y軸が上の場合
                        new Vector3(1.0f, 1.0f, 1.0f),      // 光源1（右上）
                        new Vector3(-1.0f, 1.0f, -0.5f),    // 光源2（左上）
                        new Vector3(-1.0f, -1.0f, -1.0f),   // 光源3（左下）
                        new Vector3(0.7f, -0.5f, 0.3f),     // 光源3（左下）
                    };
                    // 法線ベクトルからのファセット面の反射色の算出
                    OpenTK.Vector3 vcr3 = N2TK(normal);
                    Vector4 color = ComputeReflectedColor(vcr3, lightSources);

                    GL.Color4(color);  // 三角形の色を法線に基づいて設定
                    GL.Normal3(vcr3);
                    // 三角形ポリゴンを反時計回りにする
                    GL.Vertex3(N2TK(Polygon.Faces[l].Vertices[0].P));
                    GL.Vertex3(N2TK(Polygon.Faces[l].Vertices[2].P));
                    GL.Vertex3(N2TK(Polygon.Faces[l].Vertices[1].P));
                }
            }
            GL.End();
        }
        /// <summary>
        /// 壁の描画
        /// </summary>
        private void DrawShadowPolygons()
        {
            if (Polygon == null) return;

            //描画
            GL.Begin(PrimitiveType.Triangles);
            {
                //ファセット（三角形）を描画
                for (int i = 0; i < Polygon.Faces.Count; i++)
                {
                    // ファセットに対する法線ベクトル
                    var normal = Polygon.Faces[i].Normal();
                    OpenTK.Vector3 vcr3 = N2TK(normal);
                    GL.Normal3(vcr3);
                    GL.Vertex3(N2TK(Polygon.Faces[i].Vertices[0].P));
                    GL.Vertex3(N2TK(Polygon.Faces[i].Vertices[2].P));
                    GL.Vertex3(N2TK(Polygon.Faces[i].Vertices[1].P));
                }
            }
            GL.End();
        }
        /// <summary>
        /// Numerics.Vector3をOpenTK.Vector3に変換
        /// </summary>
        /// <param name="vec3">Numerics.Vector3型ベクトル</param>
        /// <returns>OpenTK.Vector3型ベクトル</returns>
        private static OpenTK.Vector3 N2TK(System.Numerics.Vector3 vec3) => new Vector3(vec3.X, vec3.Z, vec3.Y);
        /// <summary>
        /// 法線ベクトルに対する反射色の算出
        /// </summary>
        /// <param name="normal">法線ベクトル</param>
        /// <param name="lightSources">光源リスト</param>
        /// <returns>4ｘ4行列</returns>
        private Vector4 ComputeReflectedColor(Vector3 normal, List<Vector3> lightSources)
        {
            // 基本色
            float[] rgba = { 255f, 255f, 255f, 255f };

            for (int i = 0; i < rgba.Length; i++)
            {
                rgba[i] = (control == null) ? rgba[i] : control.materialRGBA[i];
                rgba[i] /= 255;
            }
            Vector4 baseColor = new Vector4(rgba[0], rgba[1], rgba[2], rgba[3]);

            // 初期値（黒 / 光が当たらない場合）
            Vector4 finalColor = Vector4.Zero;

            // 各光源に対して拡散光を計算
            foreach (var lightDir in lightSources)
            {
                Vector3 lightDirNormalized = lightDir.Normalized(); // 光源の方向を正規化
                float diffuseFactor = Math.Max(Vector3.Dot(normal, lightDirNormalized), 0.3f); // 拡散光計算

                finalColor += baseColor * diffuseFactor; // 拡散光の影響を加算
            }

            // RGBの値を (0.0～1.0) の範囲にクランプ
            finalColor.X = Math.Min(finalColor.X, 1.0f);
            finalColor.Y = Math.Min(finalColor.Y, 1.0f);
            finalColor.Z = Math.Min(finalColor.Z, 1.0f);

            return finalColor;
        }
        /// <summary>
        /// キャプチャー画像の取得
        /// </summary>
        /// <returns>キャプチャー画像またはnull</returns>
        public OpenCvSharp.Mat? GetMat()
        {
            if (glControl == null) return null;

            int width = glControl.Width;
            int height = glControl.Height;

            float[] floatArr = new float[width * height * 3];
            OpenCvSharp.Mat ret = new OpenCvSharp.Mat(height, width, OpenCvSharp.MatType.CV_32FC3);

            // dataBufferへの画像の読み込み
            IntPtr dataBuffer = Marshal.AllocHGlobal(width * height * 12);
            GL.ReadBuffer(ReadBufferMode.Front);
            GL.ReadPixels(0, 0, width, height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.Float, dataBuffer);

            // imgへの読み込み
            Marshal.Copy(dataBuffer, floatArr, 0, floatArr.Length);

            // opencvsharp.Matへの変換
            Marshal.Copy(floatArr, 0, ret.Data, floatArr.Length);

            // 破棄
            Marshal.FreeHGlobal(dataBuffer);

            return ret;
        }
        /// <summary>
        /// マウス右ボタンダウンイベント処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Viewer_MouseDown(object? sender, MouseEventArgs e)
        {
            // 右ボタンが押された場合
            if (e.Button == MouseButtons.Right)
            {
                //押されている間、カメラ回転を有効
                isCameraRotating = true;
                current = new Vector2(e.X, e.Y);
            }
            Update();
        }
        /// <summary>
        /// マウス右ボタンアップイベント処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Viewer_MouseUp(object? sender, MouseEventArgs e)
        {
            //右ボタンを戻した場合
            if (e.Button == MouseButtons.Right)
            {
                //押下解除によりカメラ回転を不可
                isCameraRotating = false;
                previous = Vector2.Zero;
            }
            Update();
        }
        /// <summary>
        /// マウス移動イベント処理（オイラー角による回転）
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Viewer_MouseMoveByEuler(object? sender, MouseEventArgs e)
        {
            // カメラが回転状態の場合
            if (isCameraRotating)
            {
                previous = current;
                current = new Vector2(e.X, e.Y);
                Vector2 delta = current - previous;
                delta /= (float)Math.Sqrt(this.Width * this.Width + this.Height * this.Height);
                float length = delta.Length; ;

                if (length > 0.0)
                {
                    theta += delta.X * 10;
                    double limit = Math.Sin(phi + delta.Y * 10);
                    if (limit > -0.98f && limit < 0.98f)
                    {
                        phi += delta.Y * 10;
                        // Y軸が上の場合
                        rotateX = Math.Cos(theta) * Math.Cos(phi);
                        rotateY = limit;
                        rotateZ = Math.Sin(theta) * Math.Cos(phi);
                    }
                }
                Update();
            }
        }
        /// <summary>
        /// 参考：https://www.natural-science.or.jp/article/20120416225426.php
        /// マウス移動イベント処理（クォータニオンによる回転）
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">オブジェクト拡張情報</param>
        private void Viewer_MouseMoveByQuaternion(object? sender, MouseEventArgs e)
        {
            if (control == null || control.viewport.VP == null || Polygon == null) return;

            // カメラが回転状態の場合
            if (isCameraRotating)
            {
                // 取得したビュー行列を Camera にセット
                Vector3 center = new(N2TK(Polygon.GravityPoint()));

                // OpenGL から現在のビュー行列を取得
                Matrix4 currentViewMatrix;
                GL.GetFloat(GetPName.ModelviewMatrix, out currentViewMatrix);

                // カメラ情報を取得する
                Camera camera = new Camera(currentViewMatrix);
                Vector3 cameraPos = camera.GetCameraPosition();
                Vector3 cameraUp = camera.GetUpVector();

                Vector3 u = new(cameraUp);                  // cameraUp のコピー
                u.Normalize();                              // uの正規化
                Vector3 w = Vector3.Cross(cameraPos, u);    // cameraPos × u(=cameraUp) 外積の演算
                w.Normalize();                              // wの正規化

                // マウスクリック位置から移動量(delta)算出
                previous = current;
                current = new Vector2(e.X, e.Y);
                Vector2 delta = current - previous;

                // マウス移動量を回転角度に変換、応答性の係数で調整
                thetaU = -delta.X / 80;     // u軸の回転角度
                thetaW = delta.Y / 80;      // w軸の回転角度

                // 現在のカメラ視線位置をu軸にtheta_u回転させる
                eye = QuaternionRotation(thetaU, u, cameraPos);
                // さらにカメラ視線位置をw軸にtheta_w回転させる
                eye = QuaternionRotation(thetaW, w, eye);
                // 最後にカメラ視線の上ベクトルもw軸にtheta_w回転させる
                eyeUp = QuaternionRotation(thetaW, w, cameraUp);
                Matrix4 modelView = Matrix4.LookAt(eye, center, eyeUp);
                control.viewport.VP[0] = eye.X;
                control.viewport.VP[1] = eye.Y;
                control.viewport.VP[2] = eye.Z;

                // 表示設定
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelView);
                Update();
            }
        }
        /// <summary>
        /// 以下の手順で 任意の回転軸 u に対してベクトル v を theta 回転させる
        /// 1. 回転軸 u を単位ベクトルにする回転クォータニオン Q を作成
        /// 2. ベクトル v をクォータニオン形式(vx, vy, vz, 0) にする
        /// 3. 回転 Q を適用（Q* P * Q⁻¹ を計算）
        /// 4. 回転後のベクトルを取得
        /// </summary>
        /// <param name="theta">回転角シータ</param>
        /// <param name="u">任意の回転軸</param>
        /// <param name="v">回転対象のベクトル</param>
        /// <returns>回転後のベクトル</returns>
        public Vector3 QuaternionRotation(float theta, Vector3 u, Vector3 v)
        {
            // 回転軸を単位ベクトル化
            u.Normalize();

            // クォータニオン Q = (axis * sin(theta/2), cos(theta/2))
            float halfTheta = theta / 2.0f;
            Quaternion Q = new(
                u.X * MathF.Sin(halfTheta),
                u.Y * MathF.Sin(halfTheta),
                u.Z * MathF.Sin(halfTheta),
                MathF.Cos(halfTheta)
            );

            // Q の逆クォータニオン (共役)
            Quaternion Q_inv = Q.Inverted();

            // v をクォータニオン (vx, vy, vz, 0) として定義
            Quaternion P = new Quaternion(v.X, v.Y, v.Z, 0);

            // 回転後のクォータニオン S = Q * P * Q⁻¹
            Quaternion S = Q * P * Q_inv;

            // 回転後のベクトル (x, y, z) を取得
            return new Vector3(S.X, S.Y, S.Z);
        }
        /// <summary>
        /// マウスホイールイベント処理（クォータニオンの場合）
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Viewer_MouseWheelByQuaternion(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            // マウスホイール値は 120 or -120が返るため120で割っている
            float dz = e.Delta * SystemInformation.MouseWheelScrollLines / 120;

            Matrix4 currentViewMatrix;
            GL.GetFloat(GetPName.ModelviewMatrix, out currentViewMatrix);

            // カメラ情報を取得して、ズーム処理する
            Camera camera = new Camera(currentViewMatrix);
            zoom = camera.Zoom(dz);
            Update();
        }
        /// <summary>
        /// マウスホイールイベント処理（オイラー角の場合）
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Viewer_MouseWheelByEuler(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            float delta = e.Delta;

            zoom *= (float)Math.Pow(1.001, delta);

            //拡大、縮小の制限
            if (zoom > 5.0f)
                zoom = 5.0f;
            if (zoom < 0.02f)
                zoom = 0.02f;

            Update();
        }
        /// <summary>
        /// 画面サイズ変更イベント処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Viewer_Resize(object? sender, EventArgs e)
        {
            if (glControl == null || control == null) return;
            SetupViewport();
            if (!control.priorityColor)
            {
                SetupLighting();
            }
            SetupBackground();
            SetupCamera();
            Update();
            control.viewport.Width = this.Width;
            control.viewport.Hight = this.Height;
        }
        /// <summary>
        /// ジャイロセンサーの通信サーバー
        /// </summary>
        /// <param name="port">通信ポート番号</param>
        /// <param name="timeout">通信タイムアウト(秒)</param>
        public async void StartGyroSensorServer(int port, int timeout)
        {
            if (timer == null || sensorMonitor == null) return;
            // 現在Tickを取得
            startTick = Environment.TickCount;
            // タイマー開始
            timer.Start();
            await sensorMonitor.StartServer(port, timeout);
        }
        /// <summary>
        /// ビューワを閉じるときの処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(sensorMonitor != null) sensorMonitor.StopServer();
            if (glControl != null)
            {
                glControl.Dispose();
                glControl = null;
            }
        }
        /// <summary>
        /// 閉じるボタンの処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void ToolStripButton_Click(object? sender, EventArgs e)
        {
            if(sensorMonitor != null) sensorMonitor.StopServer();
            this.Close();
        }
    }
}
