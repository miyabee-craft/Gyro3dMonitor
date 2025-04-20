using OpenCvSharp;
using SurfaceAnalyzer;
using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;

namespace Gyro3dMonitor
{
    public partial class Form_Main : Form
    {
        // ビューフォームと共有するパラメータ
        public float[] materialRGBA = new float[4]; // マテリアルカラー
        public float[] lightingRGBA = new float[4]; // ライティングカラー
        public float[] backcolorRGBA = new float[4];// 背景色
        public bool priorityColor = true;           // true: MaterialColor, false: LightingColor
        public bool stateMonitor = false;           // true: Online, false: Offline
        public bool backcolorMode = false;          // true: RGB, false: Texture
        public string textureFile = "";             // テクスチャファイル
        public int rotationMethod = 0;              // 0: Quaternion, 1: Euler angles
        public PolygonModel? polygon = null;        // ポリゴンデータ
        public bool skydomeEnable = true;           // 青空ドーム有無
        public bool meshgroundEnable = true;        // メッシュ床有無
        public bool checkgroundEnable = true;       // 市松模様床有無
        public bool shadowEnable = true;            // 影有無
        public bool coordinateAxesEnable = true;    // 座標軸有無
        public float groundHight = -0.0f;           // 床高さ
        public float skydomeRadius = 250.0f;        // 青空ドーム半径
        public mcViewport viewport = new();         // ビューポートインスタンス
        //---------------------------------------------------------------------------------------

        private Viewer? viewer = null;
        private System.Drawing.Point[] selectedPoints = new System.Drawing.Point[]
        {
            new(-1, -1),
            new(-1, -1),
            new(-1, -1),
        };
        private int selectedX = -1, selectedY = -1;     // パレット上に丸を描画する座標
        private Color BackgroundColor, MaterialColor, LightingColor;
        private string modelPath;
        private string texturePath;
        private string imagePath;
        private System.Windows.Forms.Timer timer;
        private int timeout = 30;                       // ジャイロセンサ通信タイムアウト（秒）

        public Form_Main()
        {
            InitializeComponent();
            ApplyLocalization();
            // ビューポート設定
            if (viewport != null && viewport.VP != null)
            {
                textBox_vpX.Text = $"{viewport.VP[0]:0.000}";
                textBox_vpY.Text = $"{viewport.VP[1]:0.000}";
                textBox_vpZ.Text = $"{viewport.VP[2]:0.000}";
                numericUpDown_FOV.Value = (int)viewport.FOV;
                numericUpDown_Width.Value = viewport.Width;
                numericUpDown_Hight.Value = viewport.Hight;
                numericUpDown_near.Value = (int)viewport.near;
                numericUpDown_far.Value = (int)viewport.far;
            }
            radioButton_MaterialColor.CheckedChanged += RadioButton_CheckedChanged;
            radioButton_LightingColor.CheckedChanged += RadioButton_CheckedChanged;
            radioButton_RGBColor.CheckedChanged += RadioButton_CheckedChanged;
            radioButton_Texture.CheckedChanged += RadioButton_CheckedChanged;

            // ビューポート設定のイベント登録
            numericUpDown_Width.ValueChanged += NumericUpDown_ViewportValueChanged;
            numericUpDown_Hight.ValueChanged += NumericUpDown_ViewportValueChanged;
            numericUpDown_FOV.ValueChanged += NumericUpDown_ViewportValueChanged;
            numericUpDown_near.ValueChanged += NumericUpDown_ViewportValueChanged;
            numericUpDown_far.ValueChanged += NumericUpDown_ViewportValueChanged;

            // マテリアルカラー設定のイベント登録
            numericUpDown_Red.ValueChanged += NumericUpDown_MaterialColorValueChanged;
            numericUpDown_Green.ValueChanged += NumericUpDown_MaterialColorValueChanged;
            numericUpDown_Blue.ValueChanged += NumericUpDown_MaterialColorValueChanged;
            numericUpDown_Alfa.ValueChanged += NumericUpDown_MaterialColorValueChanged;

            // ライティングカラー設定のイベント登録
            numericUpDown_lightR.ValueChanged += NumericUpDown_LightingColorValueChanged;
            numericUpDown_lightG.ValueChanged += NumericUpDown_LightingColorValueChanged;
            numericUpDown_lightB.ValueChanged += NumericUpDown_LightingColorValueChanged;
            numericUpDown_lightA.ValueChanged += NumericUpDown_LightingColorValueChanged;

            // 背景色設定のイベント登録
            numericUpDown_backcolorR.ValueChanged += NumericUpDown_BackgroundColorValueChanged;
            numericUpDown_backcolorG.ValueChanged += NumericUpDown_BackgroundColorValueChanged;
            numericUpDown_backcolorB.ValueChanged += NumericUpDown_BackgroundColorValueChanged;
            numericUpDown_backcolorA.ValueChanged += NumericUpDown_BackgroundColorValueChanged;

            // デフォルトファイルパスをプロパティから設定する
            modelPath = Properties.Settings.Default.modelPath;
            texturePath = Properties.Settings.Default.texturePath;
            imagePath = Properties.Settings.Default.imagePath;

            comboBox_RotationMethod.SelectedIndex = rotationMethod;
            // タイマーの設定（0.1秒ごとに実行）
            timer = new();
            timer.Interval = 100; // 1000ms = 1秒
            timer.Tick += Timer_Tick; // イベントハンドラ登録
            timer.Start(); // タイマー開始
        }
        /// <summary>
        /// ローカライズ
        /// 選択した言語に対するリソースを紐づける
        /// </summary>
        private void ApplyLocalization()
        {
            button_LoadModel.Text = Resources.Resource.button_LoadModel;
            button_DisplayModel.Text = Resources.Resource.button_DisplayModel;
            button_SaveImage.Text = Resources.Resource.button_SaveImage;
            button_Close.Text = Resources.Resource.button_Close;
            radioButton_MaterialColor.Text = Resources.Resource.radioButton_MaterialColor;
            radioButton_LightingColor.Text = Resources.Resource.radioButton_LightingColor;
            button_MonitorOnOff.Text = Resources.Resource.button_MonitorOnOff;
            label_Port.Text = Resources.Resource.label_Port;
            button_SelectBitmap.Text = Resources.Resource.button_SelectBitmap;
            label_ColorPalette.Text = Resources.Resource.label_ColorPalette;
            label_MaterialColor.Text = Resources.Resource.label_MaterialColor;
            label_LightingColor.Text = Resources.Resource.label_LightingColor;
            label_BackgroundColor.Text = Resources.Resource.label_BackgroundColor;
            radioButton_RGBColor.Text = Resources.Resource.radioButton_RGBColor;
            radioButton_Texture.Text = Resources.Resource.radioButton_Texture;
            radioButton_Light.Text = Resources.Resource.radioButton_Light;
            radioButton_Dark.Text = Resources.Resource.radioButton_Dark;
            label_RotationMethod.Text = Resources.Resource.label_RotationMethod;
            checkBox_SkyDome.Text = Resources.Resource.checkBox_SkyDome;
            checkBox_MeshGround.Text = Resources.Resource.checkBox_MeshGround;
            checkBox_CheckGround.Text = Resources.Resource.checkBox_CheckGround;
            label_Hight.Text = Resources.Resource.label_Hight;
            label_Radius.Text = Resources.Resource.label_Radius;
            label_WH.Text = Resources.Resource.label_WH;
            label_near.Text = Resources.Resource.label_near;
            label_far.Text = Resources.Resource.label_far;
            label_FOV.Text = Resources.Resource.label_FOV;
            label_VP.Text = Resources.Resource.label_VP;
            label_ViewportSettings.Text = Resources.Resource.label_ViewportSettings;
            checkBox_shadowEnable.Text = Resources.Resource.checkBox_shadowEnable;
            checkBox_CoordinateAxes.Text = Resources.Resource.checkBox_CoordinateAxes;
            label_Timeout.Text = Resources.Resource.label_Timeout;
            label_GyroOperation.Text = Resources.Resource.label_GyroOperation;
            label_ViewerOperation.Text = Resources.Resource.label_ViewerOperation;

            // コンボボックスの要素
            string[] items = Resources.Resource.comboBox_RotationMethod_Items.Split(';');
            comboBox_RotationMethod.Items.AddRange(items);
        }
        /// <summary>
        /// タイマー処理
        /// ビューワ側の変更をメインフォームに反映するためのタイマー
        /// ビュー情報更新のためインターバルは0.1秒とする
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            // ビューワフォームの変更を反映する場合
            if (viewer == null || viewer.IsDisposed == true)
            {
                comboBox_RotationMethod.Enabled = true;
            }
            if (viewport != null && viewport.VP != null)
            {
                // ビューポート情報の更新
                textBox_vpX.Text = $"{viewport.VP[0]:0.000}";
                textBox_vpY.Text = $"{viewport.VP[1]:0.000}";
                textBox_vpZ.Text = $"{viewport.VP[2]:0.000}";
                numericUpDown_Width.Value = viewport.Width;
                numericUpDown_Hight.Value = viewport.Hight;
            }
            if (viewer != null && viewer.sensorMonitor != null)
            {
                // ジャイロモニタ開始ボタンの更新処理
                switch (viewer.sensorMonitor.State)
                {
                    case GyroSensorMonitor.lineState.OFFLINE:
                        stateMonitor = false;
                        button_MonitorOnOff.Text = Resources.Resource.button_MonitorOnOff;  // "ジャイロモニタ開始";
                        break;
                    case GyroSensorMonitor.lineState.ONLINE_WAIT:
                    case GyroSensorMonitor.lineState.ONLINE_RECIEVE:
                        stateMonitor = true;
                        button_MonitorOnOff.Text = Resources.Resource.button_MonitorOff;    // "ジャイロモニタ中断";
                        break;
                }
            }
        }
        /// <summary>
        /// メインフォームのロード処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Form_Main_Load(object sender, EventArgs e)
        {
            // フォーム画面の初期化
            backcolorMode = true;
            radioButton_MaterialColor.Checked = priorityColor;
            SetMaterialColor();
            SetLightingColor();
            SetBackgroundColor();
            // プレビューエリアの背景色変更
            panel_MatPreview.BackColor = MaterialColor;
            panel_LigPreview.BackColor = LightingColor;
            panel_BacPreview.BackColor = BackgroundColor;

            // オブジェクト表示の初期化
            checkBox_SkyDome.Checked = skydomeEnable;
            checkBox_MeshGround.Checked = meshgroundEnable;
            checkBox_CheckGround.Checked = checkgroundEnable;
            checkBox_shadowEnable.Checked = shadowEnable;
            checkBox_CoordinateAxes.Checked = coordinateAxesEnable;
            numericUpDown_groundHight.Value = (int)groundHight;
            numericUpDown_groundHight.Enabled = false;
            numericUpDown_skydomeRadius.Value = (int)skydomeRadius;

            // ジャイロセンサ通信の初期化
            numericUpDown_Timeout.Value = timeout;
        }
        /// <summary>
        /// モデルの読み込みボタン処理
        /// 対象データはSTLファイル
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Button_LoadModel_Click(object sender, EventArgs e)
        {
            // STL形式モデルファイルの選択
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = Resources.Resource.msgMain_1; // "モデルファイルの読み込み";
                dlg.Filter = Resources.Resource.msgMain_2; // "STLモデル (*.stl)|*.stl";
                dlg.DefaultExt = "stl";
                dlg.FileName = modelPath;
                dlg.InitialDirectory = modelPath;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // モデルの読み込み
                    string filePath = dlg.FileName;
                    if (File.Exists(filePath))
                    {
                        if (viewer != null)
                        {
                            // 既にモデルを持っているので解放する
                            viewer.Dispose();
                            viewer = null;
                            button_DisplayModel.Text = Resources.Resource.button_DisplayModel;  // "ビューの表示";
                        }
                        // STLファイルの読み出し
                        polygon = SurfaceAnalyzer.LoadData.LoadSTL(filePath, true);

                        // ファイルパスをプロパティとして保存する
                        // 保存先：C:\Users\<ユーザアカウント名>\AppData\Local\Gyro3dMonitor
                        Properties.Settings.Default.modelPath = Path.GetDirectoryName(filePath);
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        MessageBox.Show($"{Resources.Resource.msgMain_3}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);　//"ファイルが見つかりません。"
                    }
                }
            }
        }
        /// <summary>
        /// テクスチャの画像ファイルを選択するボタン処理
        /// 対象ファイル：jpg
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Button_SelectBitmap_Click(object sender, EventArgs e)
        {
            // ビットマップの選択
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = Resources.Resource.msgMain_4;   //"画像ファイルの読み込み";
                dlg.Filter = Resources.Resource.msgMain_5;  //"画像ファイル (*.jpg)|*.jpg";
                dlg.DefaultExt = "jpg";
                dlg.FileName = texturePath;
                dlg.InitialDirectory = texturePath;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // 画像ファイルの選択
                    string filePath = dlg.FileName;
                    if (File.Exists(filePath))
                    {
                        if (viewer != null && viewer.sensorMonitor != null)
                        {
                            // 既にモデルを持っているので解放する
                            if ((viewer.sensorMonitor.State == GyroSensorMonitor.lineState.ONLINE_RECIEVE
                                || viewer.sensorMonitor.State == GyroSensorMonitor.lineState.ONLINE_WAIT)
                                && viewer.cancelToken != null)
                                {
                                viewer.cancelToken.Cancel();
                            }
                            viewer.Close();
                            viewer.Dispose();
                            viewer = null;
                        }
                        textureFile = filePath;
                        textBox_BitmapFileName.Text = filePath;
                        textBox_BitmapFileName.Select(textBox_BitmapFileName.Text.Length, 0);

                        // ファイルパスをプロパティとして保存する
                        // 保存先：C:\Users\<ユーザアカウント名>\AppData\Local\Gyro3dMonitor
                        Properties.Settings.Default.texturePath = Path.GetDirectoryName(filePath);
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        MessageBox.Show($"{Resources.Resource.msgMain_6}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);　//"ファイルが見つかりません。"
                    }
                }
            }
        }
        /// <summary>
        /// キャプチャーとキャプチャーの保存ボタンの処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Button_SaveImage_Click(object sender, EventArgs e)
        {
            if (viewer == null || viewer.IsDisposed)
            {
                MessageBox.Show($"{Resources.Resource.msgMain_7}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);　//"キャプチャー画像がありません。"
                return;
            }
            try
            {
                // viewerのキャプチャーの取得
                using (Mat? mat = viewer.GetMat())
                {
                    if (mat == null) throw new ArgumentException($"{Resources.Resource.msgMain_8}"); //"画像データがありません。"

                    // OpenGLの座標系とOpenCVの座標系が異なるため、画像を上下反転
                    Cv2.Flip(mat, mat, FlipMode.X);

                    // キャプチャーの表示
                    Cv2.ImShow("mat", mat);

                    DialogResult result = MessageBox.Show($"{Resources.Resource.msgMain_9}", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);　//"キャプチャー画損を保存しますか？"
                    if (result == DialogResult.Yes)
                    {
                        // キャプチャー画像の保存
                        using (SaveFileDialog dlg = new SaveFileDialog())
                        {
                            dlg.Filter = Resources.Resource.msgMain_10; // "PNG画像 (*.png)|*.png|JPEG画像 (*.jpg)|*.jpg";
                            dlg.DefaultExt = "png";
                            dlg.FileName = imagePath + DateTime.Now.ToString(("yyyyMMdd_HHmmss")) + "_screenshot.png";
                            dlg.InitialDirectory = imagePath;

                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                string filePath = dlg.FileName;
                                string ext = System.IO.Path.GetExtension(filePath).ToLower();
                                var encodingParams = (ext == ".jpg") ? new int[] { (int)ImwriteFlags.JpegQuality, 90 } : new int[] { (int)ImwriteFlags.PngCompression, 3 };

                                Cv2.ImWrite(filePath + ext, mat * 256, encodingParams);
                                // ファイルパスをプロパティとして保存する
                                // 保存先：C:\Users\<ユーザアカウント名>\AppData\Local\Gyro3dMonitor
                                Properties.Settings.Default.imagePath = Path.GetDirectoryName(filePath);
                                Properties.Settings.Default.Save();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Resources.Resource.msgMain_11} {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error); //"エラーが発生しました: "
            }
        }
        /// <summary>
        /// ビューフォームを開いてモデルを描画するボタン処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Button_DisplayModel_Click(object sender, EventArgs e)
        {
            if (polygon == null)
            {
                MessageBox.Show($"{Resources.Resource.msgMain_12}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error); //"モデルが読み込まれていません。"
                return;
            }
            if (viewer == null || viewer.IsDisposed)
            {
                viewer = new Viewer(this);
                button_DisplayModel.Text = Resources.Resource.button_DisplayModelUpdate;    // "ビューの更新";
                comboBox_RotationMethod.Enabled = false;
            }
            // 初期描画設定
            SetMaterialColor();
            SetLightingColor();
            SetBackgroundColor();
            viewer.Show();
            // モデルの描画
            viewer.Update();
        }
        /// <summary>
        /// ジャイロモニタ開始/中断ボタン処理
        /// 開始後、ボタン名は中断に変更（トグル変更）
        /// 開始時に通信を開始する。中断時に通信をキャンセルする。
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Button_MonitorOnOff_Click(object sender, EventArgs e)
        {
            // 通信ポート番号を取得する
            int port = (int)numericUpDown_Port.Value;
            timeout = (int)numericUpDown_Timeout.Value;

            if (!stateMonitor)
            {
                // ジャイロモニタ開始
                if (viewer != null && !viewer.IsDisposed)
                {
                    viewer.StartGyroSensorServer(port, timeout);
                    viewer.Update();
                    stateMonitor = true; // オンライン
                    button_MonitorOnOff.Text = Resources.Resource.button_MonitorOff;    // "ジャイロモニタ中断";
                }
                else
                {
                    MessageBox.Show($"{Resources.Resource.msgMain_13}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error); //"ビューが表示されていません。"
                    return;
                }
            }
            else
            {
                // ジャイロモニタ中断
                if (viewer != null && !viewer.IsDisposed && viewer.sensorMonitor != null)
                {
                    // モニタをキャンセルする
                    if (viewer.sensorMonitor.cancelToken != null)
                    {
                        viewer.sensorMonitor.cancelToken.Cancel();
                    }
                }
                stateMonitor = false; //オフライン
                button_MonitorOnOff.Text = Resources.Resource.button_MonitorOnOff;  // "ジャイロモニタ開始";
            }
        }
        /// <summary>
        /// マテリアル色を更新
        /// </summary>
        private void SetMaterialColor()
        {
            materialRGBA[0] = ((float)numericUpDown_Red.Value);
            materialRGBA[1] = ((float)numericUpDown_Green.Value);
            materialRGBA[2] = ((float)numericUpDown_Blue.Value);
            materialRGBA[3] = ((float)numericUpDown_Alfa.Value);
            MaterialColor = Color.FromArgb((int)numericUpDown_Alfa.Value, (int)numericUpDown_Red.Value, (int)numericUpDown_Green.Value, (int)numericUpDown_Blue.Value);
        }
        /// <summary>
        /// ライティング色を更新
        /// </summary>
        private void SetLightingColor()
        {
            lightingRGBA[0] = ((float)numericUpDown_lightR.Value);
            lightingRGBA[1] = ((float)numericUpDown_lightG.Value);
            lightingRGBA[2] = ((float)numericUpDown_lightB.Value);
            lightingRGBA[3] = ((float)numericUpDown_lightA.Value);
            LightingColor = Color.FromArgb((int)numericUpDown_lightA.Value, (int)numericUpDown_lightR.Value, (int)numericUpDown_lightG.Value, (int)numericUpDown_lightB.Value);
        }
        /// <summary>
        /// 背景色を更新
        /// </summary>
        private void SetBackgroundColor()
        {
            backcolorRGBA[0] = ((float)numericUpDown_backcolorR.Value);
            backcolorRGBA[1] = ((float)numericUpDown_backcolorG.Value);
            backcolorRGBA[2] = ((float)numericUpDown_backcolorB.Value);
            backcolorRGBA[3] = ((float)numericUpDown_backcolorA.Value);
            BackgroundColor = Color.FromArgb((int)numericUpDown_backcolorA.Value, (int)numericUpDown_backcolorR.Value, (int)numericUpDown_backcolorG.Value, (int)numericUpDown_backcolorB.Value);
        }
        /// <summary>
        /// メインフォームを閉じるボタン処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Button_Close_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
        /// <summary>
        /// ラジオボタン処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            // チェックされたラジオボタンを判定
            RadioButton? selectedRadio = sender as RadioButton;
            if (selectedRadio != null && selectedRadio.Checked)
            {
                // すべてのラジオボタンを無効化する
                numericUpDown_Red.Enabled = false;
                numericUpDown_Green.Enabled = false;
                numericUpDown_Blue.Enabled = false;
                numericUpDown_Alfa.Enabled = false;

                numericUpDown_lightR.Enabled = false;
                numericUpDown_lightG.Enabled = false;
                numericUpDown_lightB.Enabled = false;
                numericUpDown_lightA.Enabled = false;

                numericUpDown_backcolorR.Enabled = false;
                numericUpDown_backcolorG.Enabled = false;
                numericUpDown_backcolorB.Enabled = false;
                numericUpDown_backcolorA.Enabled = false;

                textBox_BitmapFileName.Enabled = false;
                button_SelectBitmap.Enabled = false;

                //チェックされたラジオボタンの名称により判定する
                switch (selectedRadio.Tag)
                {
                    case "1": // "マテリアルカラー:":
                        // 優先色 true: マテリアル, false: ライティング
                        priorityColor = true;
                        // マテリアルカラー
                        numericUpDown_Red.Enabled = true;
                        numericUpDown_Green.Enabled = true;
                        numericUpDown_Blue.Enabled = true;
                        numericUpDown_Alfa.Enabled = true;
                        break;
                    case "2": // "ライティングカラー:":
                        priorityColor = false;
                        // ライティングカラー
                        numericUpDown_lightR.Enabled = true;
                        numericUpDown_lightG.Enabled = true;
                        numericUpDown_lightB.Enabled = true;
                        numericUpDown_lightA.Enabled = true;
                        break;
                    case "3": // "背景色:":
                        // 背景色 true: RGB指定, false: Texture
                        backcolorMode = true;
                        // 背景色RGB
                        numericUpDown_backcolorR.Enabled = true;
                        numericUpDown_backcolorG.Enabled = true;
                        numericUpDown_backcolorB.Enabled = true;
                        numericUpDown_backcolorA.Enabled = true;
                        break;
                    case "4": // "テクスチャー:":
                        // 背景色Texture
                        backcolorMode = false;
                        textBox_BitmapFileName.Enabled = true;
                        button_SelectBitmap.Enabled = true;
                        break;
                }
                // 丸を再描画
                pictureBox_Palette.Invalidate();
                if (viewer != null)
                {
                    viewer.Update();
                }
            }
        }
        /// <summary>
        /// パレット上をマウスクリックした時の処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void PictureBox_Palette_MouseClick(object sender, MouseEventArgs e)
        {
            // クリックされた位置の色を取得
            Bitmap bitmap = (Bitmap)pictureBox_Palette.Image;

            Color selectedColor = bitmap.GetPixel(e.X * bitmap.Width / pictureBox_Palette.Width,
                                                  e.Y * bitmap.Height / pictureBox_Palette.Height);
            // 選択位置を保存
            if (radioButton_MaterialColor.Checked)
            {
                selectedPoints[0].X = e.X;
                selectedPoints[0].Y = e.Y;
            }
            else if (radioButton_LightingColor.Checked)
            {
                selectedPoints[1].X = e.X;
                selectedPoints[1].Y = e.Y;
            }
            else if (radioButton_RGBColor.Checked)
            {
                selectedPoints[2].X = e.X;
                selectedPoints[2].Y = e.Y;
            }

            // RGB 値を表示
            if (radioButton_MaterialColor.Checked)
            {
                // プレビューエリアの背景色変更
                panel_MatPreview.BackColor = selectedColor;

                numericUpDown_Red.Value = selectedColor.R;
                numericUpDown_Green.Value = selectedColor.G;
                numericUpDown_Blue.Value = selectedColor.B;
                SetMaterialColor();
            }
            else if (radioButton_LightingColor.Checked)
            {
                // プレビューエリアの背景色変更
                panel_LigPreview.BackColor = selectedColor;

                numericUpDown_lightR.Value = selectedColor.R;
                numericUpDown_lightG.Value = selectedColor.G;
                numericUpDown_lightB.Value = selectedColor.B;
                SetLightingColor();
            }
            else if (radioButton_RGBColor.Checked)
            {
                // プレビューエリアの背景色変更
                panel_BacPreview.BackColor = selectedColor;

                numericUpDown_backcolorR.Value = selectedColor.R;
                numericUpDown_backcolorG.Value = selectedColor.G;
                numericUpDown_backcolorB.Value = selectedColor.B;
                SetBackgroundColor();
            }
            if (viewer != null)
            {
                viewer.Update();
            }
            // 丸を再描画
            pictureBox_Palette.Invalidate();
        }
        /// <summary>
        /// マウスクリックした点に丸を描画する
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void PictureBox_Palette_Paint(object sender, PaintEventArgs e)
        {
            if (radioButton_MaterialColor.Checked)
            {
                selectedX = selectedPoints[0].X;
                selectedY = selectedPoints[0].Y;
            }
            else if (radioButton_LightingColor.Checked)
            {
                selectedX = selectedPoints[1].X;
                selectedY = selectedPoints[1].Y;
            }
            else if (radioButton_RGBColor.Checked)
            {
                selectedX = selectedPoints[2].X;
                selectedY = selectedPoints[2].Y;
            }
            else
            {
                selectedX = -1;
                selectedY = -1;
            }
            if (selectedX >= 0 && selectedY >= 0)
            {
                using (Pen pen = new(Color.White, 1))
                {
                    e.Graphics.DrawEllipse(pen, selectedX - 5, selectedY - 5, 10, 10);
                }
            }
        }
        /// <summary>
        /// カラーパレットのカラーテンプレートを切り替えるボタン処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void RadioButton_Dark_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Dark.Checked)
            {
                pictureBox_Palette.Image = Properties.Resources.pallete255x255;
            }
            else
            {
                pictureBox_Palette.Image = Properties.Resources.pallete_light_255x255;
            }
        }
        /// <summary>
        /// ローテーション方法の選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_RotationMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            rotationMethod = comboBox_RotationMethod.SelectedIndex;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// 青空ドームのON/OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_SkyDome_CheckedChanged(object sender, EventArgs e)
        {
            skydomeEnable = checkBox_SkyDome.Checked;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// メッシュグランドのONOFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_MeshGround_CheckedChanged(object sender, EventArgs e)
        {
            meshgroundEnable = checkBox_MeshGround.Checked;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// 市松模様床のON/OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CheckGround_CheckedChanged(object sender, EventArgs e)
        {
            checkgroundEnable = checkBox_CheckGround.Checked;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// 影のON/OFF
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void CheckBox_shadowEnable_CheckedChanged(object sender, EventArgs e)
        {
            shadowEnable = checkBox_shadowEnable.Checked;
            if (shadowEnable)
            {
                // 影の描画時は床高さはゼロ固定で編集不可
                numericUpDown_groundHight.Value = 0;
                numericUpDown_groundHight.Enabled = false;
            }
            else
            {
                numericUpDown_groundHight.Enabled = true;
            }
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// 座標軸のON/OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CoordinateAxes_CheckedChanged(object sender, EventArgs e)
        {
            coordinateAxesEnable = checkBox_CoordinateAxes.Checked;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// 床高さの設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_groundHight_ValueChanged(object sender, EventArgs e)
        {
            groundHight = (float)numericUpDown_groundHight.Value;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// 青空ドーム半径の設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_skydomeRadius_ValueChanged(object sender, EventArgs e)
        {
            skydomeRadius = (float)numericUpDown_skydomeRadius.Value;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// ビューボートの設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_ViewportValueChanged(object? sender, EventArgs e)
        {
            viewport.Width = (int)numericUpDown_Width.Value;
            viewport.Hight = (int)numericUpDown_Hight.Value;
            viewport.FOV = (int)numericUpDown_FOV.Value;
            viewport.near = (int)numericUpDown_near.Value;
            viewport.far = (int)numericUpDown_far.Value;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// マテリアルカラーの更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_MaterialColorValueChanged(object? sender, EventArgs e)
        {
            materialRGBA[0] = (int)numericUpDown_Red.Value;
            materialRGBA[1] = (int)numericUpDown_Green.Value;
            materialRGBA[2] = (int)numericUpDown_Blue.Value;
            materialRGBA[3] = (int)numericUpDown_Alfa.Value;
            panel_MatPreview.BackColor = Color.FromArgb((int)materialRGBA[3], (int)materialRGBA[0], (int)materialRGBA[1], (int)materialRGBA[2]);
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// ライティングカラーの更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_LightingColorValueChanged(object? sender, EventArgs e)
        {
            lightingRGBA[0] = (float)numericUpDown_lightR.Value;
            lightingRGBA[1] = (float)numericUpDown_lightG.Value;
            lightingRGBA[2] = (float)numericUpDown_lightB.Value;
            lightingRGBA[3] = (float)numericUpDown_lightA.Value;
            panel_LigPreview.BackColor = Color.FromArgb((int)lightingRGBA[3], (int)lightingRGBA[0], (int)lightingRGBA[1], (int)lightingRGBA[2]);
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// 背景色の更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_BackgroundColorValueChanged(object? sender, EventArgs e)
        {
            backcolorRGBA[0] = (float)numericUpDown_backcolorR.Value;
            backcolorRGBA[1] = (float)numericUpDown_backcolorG.Value;
            backcolorRGBA[2] = (float)numericUpDown_backcolorB.Value;
            backcolorRGBA[3] = (float)numericUpDown_backcolorA.Value;
            panel_BacPreview.BackColor = Color.FromArgb((int)backcolorRGBA[3], (int)backcolorRGBA[0], (int)backcolorRGBA[1], (int)backcolorRGBA[2]);
            if (viewer != null) viewer.Update();
        }
    }
    /// <summary>
    /// ビューポートクラス定義
    /// </summary>
    public class mcViewport
    {
        public float[]? VP { get; set; }
        public float FOV { get; set; }
        public int Width { get; set; }
        public int Hight { get; set; }
        public float near { get; set; }
        public float far { get; set; }
        /// <summary>
        /// ビューポートのコンストラクタ
        /// </summary>
        public mcViewport()
        {
            VP = [0f, 0f, 0f];
            FOV = 60.0f;
            Width = 695;
            Hight = 695;
            near = 1.0f;
            far = 500.0f;
        }
    }
}
