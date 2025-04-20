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
        // �r���[�t�H�[���Ƌ��L����p�����[�^
        public float[] materialRGBA = new float[4]; // �}�e���A���J���[
        public float[] lightingRGBA = new float[4]; // ���C�e�B���O�J���[
        public float[] backcolorRGBA = new float[4];// �w�i�F
        public bool priorityColor = true;           // true: MaterialColor, false: LightingColor
        public bool stateMonitor = false;           // true: Online, false: Offline
        public bool backcolorMode = false;          // true: RGB, false: Texture
        public string textureFile = "";             // �e�N�X�`���t�@�C��
        public int rotationMethod = 0;              // 0: Quaternion, 1: Euler angles
        public PolygonModel? polygon = null;        // �|���S���f�[�^
        public bool skydomeEnable = true;           // ��h�[���L��
        public bool meshgroundEnable = true;        // ���b�V�����L��
        public bool checkgroundEnable = true;       // �s���͗l���L��
        public bool shadowEnable = true;            // �e�L��
        public bool coordinateAxesEnable = true;    // ���W���L��
        public float groundHight = -0.0f;           // ������
        public float skydomeRadius = 250.0f;        // ��h�[�����a
        public mcViewport viewport = new();         // �r���[�|�[�g�C���X�^���X
        //---------------------------------------------------------------------------------------

        private Viewer? viewer = null;
        private System.Drawing.Point[] selectedPoints = new System.Drawing.Point[]
        {
            new(-1, -1),
            new(-1, -1),
            new(-1, -1),
        };
        private int selectedX = -1, selectedY = -1;     // �p���b�g��Ɋۂ�`�悷����W
        private Color BackgroundColor, MaterialColor, LightingColor;
        private string modelPath;
        private string texturePath;
        private string imagePath;
        private System.Windows.Forms.Timer timer;
        private int timeout = 30;                       // �W���C���Z���T�ʐM�^�C���A�E�g�i�b�j

        public Form_Main()
        {
            InitializeComponent();
            ApplyLocalization();
            // �r���[�|�[�g�ݒ�
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

            // �r���[�|�[�g�ݒ�̃C�x���g�o�^
            numericUpDown_Width.ValueChanged += NumericUpDown_ViewportValueChanged;
            numericUpDown_Hight.ValueChanged += NumericUpDown_ViewportValueChanged;
            numericUpDown_FOV.ValueChanged += NumericUpDown_ViewportValueChanged;
            numericUpDown_near.ValueChanged += NumericUpDown_ViewportValueChanged;
            numericUpDown_far.ValueChanged += NumericUpDown_ViewportValueChanged;

            // �}�e���A���J���[�ݒ�̃C�x���g�o�^
            numericUpDown_Red.ValueChanged += NumericUpDown_MaterialColorValueChanged;
            numericUpDown_Green.ValueChanged += NumericUpDown_MaterialColorValueChanged;
            numericUpDown_Blue.ValueChanged += NumericUpDown_MaterialColorValueChanged;
            numericUpDown_Alfa.ValueChanged += NumericUpDown_MaterialColorValueChanged;

            // ���C�e�B���O�J���[�ݒ�̃C�x���g�o�^
            numericUpDown_lightR.ValueChanged += NumericUpDown_LightingColorValueChanged;
            numericUpDown_lightG.ValueChanged += NumericUpDown_LightingColorValueChanged;
            numericUpDown_lightB.ValueChanged += NumericUpDown_LightingColorValueChanged;
            numericUpDown_lightA.ValueChanged += NumericUpDown_LightingColorValueChanged;

            // �w�i�F�ݒ�̃C�x���g�o�^
            numericUpDown_backcolorR.ValueChanged += NumericUpDown_BackgroundColorValueChanged;
            numericUpDown_backcolorG.ValueChanged += NumericUpDown_BackgroundColorValueChanged;
            numericUpDown_backcolorB.ValueChanged += NumericUpDown_BackgroundColorValueChanged;
            numericUpDown_backcolorA.ValueChanged += NumericUpDown_BackgroundColorValueChanged;

            // �f�t�H���g�t�@�C���p�X���v���p�e�B����ݒ肷��
            modelPath = Properties.Settings.Default.modelPath;
            texturePath = Properties.Settings.Default.texturePath;
            imagePath = Properties.Settings.Default.imagePath;

            comboBox_RotationMethod.SelectedIndex = rotationMethod;
            // �^�C�}�[�̐ݒ�i0.1�b���ƂɎ��s�j
            timer = new();
            timer.Interval = 100; // 1000ms = 1�b
            timer.Tick += Timer_Tick; // �C�x���g�n���h���o�^
            timer.Start(); // �^�C�}�[�J�n
        }
        /// <summary>
        /// ���[�J���C�Y
        /// �I����������ɑ΂��郊�\�[�X��R�Â���
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

            // �R���{�{�b�N�X�̗v�f
            string[] items = Resources.Resource.comboBox_RotationMethod_Items.Split(';');
            comboBox_RotationMethod.Items.AddRange(items);
        }
        /// <summary>
        /// �^�C�}�[����
        /// �r���[�����̕ύX�����C���t�H�[���ɔ��f���邽�߂̃^�C�}�[
        /// �r���[���X�V�̂��߃C���^�[�o����0.1�b�Ƃ���
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            // �r���[���t�H�[���̕ύX�𔽉f����ꍇ
            if (viewer == null || viewer.IsDisposed == true)
            {
                comboBox_RotationMethod.Enabled = true;
            }
            if (viewport != null && viewport.VP != null)
            {
                // �r���[�|�[�g���̍X�V
                textBox_vpX.Text = $"{viewport.VP[0]:0.000}";
                textBox_vpY.Text = $"{viewport.VP[1]:0.000}";
                textBox_vpZ.Text = $"{viewport.VP[2]:0.000}";
                numericUpDown_Width.Value = viewport.Width;
                numericUpDown_Hight.Value = viewport.Hight;
            }
            if (viewer != null && viewer.sensorMonitor != null)
            {
                // �W���C�����j�^�J�n�{�^���̍X�V����
                switch (viewer.sensorMonitor.State)
                {
                    case GyroSensorMonitor.lineState.OFFLINE:
                        stateMonitor = false;
                        button_MonitorOnOff.Text = Resources.Resource.button_MonitorOnOff;  // "�W���C�����j�^�J�n";
                        break;
                    case GyroSensorMonitor.lineState.ONLINE_WAIT:
                    case GyroSensorMonitor.lineState.ONLINE_RECIEVE:
                        stateMonitor = true;
                        button_MonitorOnOff.Text = Resources.Resource.button_MonitorOff;    // "�W���C�����j�^���f";
                        break;
                }
            }
        }
        /// <summary>
        /// ���C���t�H�[���̃��[�h����
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void Form_Main_Load(object sender, EventArgs e)
        {
            // �t�H�[����ʂ̏�����
            backcolorMode = true;
            radioButton_MaterialColor.Checked = priorityColor;
            SetMaterialColor();
            SetLightingColor();
            SetBackgroundColor();
            // �v���r���[�G���A�̔w�i�F�ύX
            panel_MatPreview.BackColor = MaterialColor;
            panel_LigPreview.BackColor = LightingColor;
            panel_BacPreview.BackColor = BackgroundColor;

            // �I�u�W�F�N�g�\���̏�����
            checkBox_SkyDome.Checked = skydomeEnable;
            checkBox_MeshGround.Checked = meshgroundEnable;
            checkBox_CheckGround.Checked = checkgroundEnable;
            checkBox_shadowEnable.Checked = shadowEnable;
            checkBox_CoordinateAxes.Checked = coordinateAxesEnable;
            numericUpDown_groundHight.Value = (int)groundHight;
            numericUpDown_groundHight.Enabled = false;
            numericUpDown_skydomeRadius.Value = (int)skydomeRadius;

            // �W���C���Z���T�ʐM�̏�����
            numericUpDown_Timeout.Value = timeout;
        }
        /// <summary>
        /// ���f���̓ǂݍ��݃{�^������
        /// �Ώۃf�[�^��STL�t�@�C��
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void Button_LoadModel_Click(object sender, EventArgs e)
        {
            // STL�`�����f���t�@�C���̑I��
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = Resources.Resource.msgMain_1; // "���f���t�@�C���̓ǂݍ���";
                dlg.Filter = Resources.Resource.msgMain_2; // "STL���f�� (*.stl)|*.stl";
                dlg.DefaultExt = "stl";
                dlg.FileName = modelPath;
                dlg.InitialDirectory = modelPath;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // ���f���̓ǂݍ���
                    string filePath = dlg.FileName;
                    if (File.Exists(filePath))
                    {
                        if (viewer != null)
                        {
                            // ���Ƀ��f���������Ă���̂ŉ������
                            viewer.Dispose();
                            viewer = null;
                            button_DisplayModel.Text = Resources.Resource.button_DisplayModel;  // "�r���[�̕\��";
                        }
                        // STL�t�@�C���̓ǂݏo��
                        polygon = SurfaceAnalyzer.LoadData.LoadSTL(filePath, true);

                        // �t�@�C���p�X���v���p�e�B�Ƃ��ĕۑ�����
                        // �ۑ���FC:\Users\<���[�U�A�J�E���g��>\AppData\Local\Gyro3dMonitor
                        Properties.Settings.Default.modelPath = Path.GetDirectoryName(filePath);
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        MessageBox.Show($"{Resources.Resource.msgMain_3}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);�@//"�t�@�C����������܂���B"
                    }
                }
            }
        }
        /// <summary>
        /// �e�N�X�`���̉摜�t�@�C����I������{�^������
        /// �Ώۃt�@�C���Fjpg
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void Button_SelectBitmap_Click(object sender, EventArgs e)
        {
            // �r�b�g�}�b�v�̑I��
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = Resources.Resource.msgMain_4;   //"�摜�t�@�C���̓ǂݍ���";
                dlg.Filter = Resources.Resource.msgMain_5;  //"�摜�t�@�C�� (*.jpg)|*.jpg";
                dlg.DefaultExt = "jpg";
                dlg.FileName = texturePath;
                dlg.InitialDirectory = texturePath;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // �摜�t�@�C���̑I��
                    string filePath = dlg.FileName;
                    if (File.Exists(filePath))
                    {
                        if (viewer != null && viewer.sensorMonitor != null)
                        {
                            // ���Ƀ��f���������Ă���̂ŉ������
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

                        // �t�@�C���p�X���v���p�e�B�Ƃ��ĕۑ�����
                        // �ۑ���FC:\Users\<���[�U�A�J�E���g��>\AppData\Local\Gyro3dMonitor
                        Properties.Settings.Default.texturePath = Path.GetDirectoryName(filePath);
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        MessageBox.Show($"{Resources.Resource.msgMain_6}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);�@//"�t�@�C����������܂���B"
                    }
                }
            }
        }
        /// <summary>
        /// �L���v�`���[�ƃL���v�`���[�̕ۑ��{�^���̏���
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void Button_SaveImage_Click(object sender, EventArgs e)
        {
            if (viewer == null || viewer.IsDisposed)
            {
                MessageBox.Show($"{Resources.Resource.msgMain_7}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);�@//"�L���v�`���[�摜������܂���B"
                return;
            }
            try
            {
                // viewer�̃L���v�`���[�̎擾
                using (Mat? mat = viewer.GetMat())
                {
                    if (mat == null) throw new ArgumentException($"{Resources.Resource.msgMain_8}"); //"�摜�f�[�^������܂���B"

                    // OpenGL�̍��W�n��OpenCV�̍��W�n���قȂ邽�߁A�摜���㉺���]
                    Cv2.Flip(mat, mat, FlipMode.X);

                    // �L���v�`���[�̕\��
                    Cv2.ImShow("mat", mat);

                    DialogResult result = MessageBox.Show($"{Resources.Resource.msgMain_9}", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);�@//"�L���v�`���[�摹��ۑ����܂����H"
                    if (result == DialogResult.Yes)
                    {
                        // �L���v�`���[�摜�̕ۑ�
                        using (SaveFileDialog dlg = new SaveFileDialog())
                        {
                            dlg.Filter = Resources.Resource.msgMain_10; // "PNG�摜 (*.png)|*.png|JPEG�摜 (*.jpg)|*.jpg";
                            dlg.DefaultExt = "png";
                            dlg.FileName = imagePath + DateTime.Now.ToString(("yyyyMMdd_HHmmss")) + "_screenshot.png";
                            dlg.InitialDirectory = imagePath;

                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                string filePath = dlg.FileName;
                                string ext = System.IO.Path.GetExtension(filePath).ToLower();
                                var encodingParams = (ext == ".jpg") ? new int[] { (int)ImwriteFlags.JpegQuality, 90 } : new int[] { (int)ImwriteFlags.PngCompression, 3 };

                                Cv2.ImWrite(filePath + ext, mat * 256, encodingParams);
                                // �t�@�C���p�X���v���p�e�B�Ƃ��ĕۑ�����
                                // �ۑ���FC:\Users\<���[�U�A�J�E���g��>\AppData\Local\Gyro3dMonitor
                                Properties.Settings.Default.imagePath = Path.GetDirectoryName(filePath);
                                Properties.Settings.Default.Save();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Resources.Resource.msgMain_11} {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error); //"�G���[���������܂���: "
            }
        }
        /// <summary>
        /// �r���[�t�H�[�����J���ă��f����`�悷��{�^������
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void Button_DisplayModel_Click(object sender, EventArgs e)
        {
            if (polygon == null)
            {
                MessageBox.Show($"{Resources.Resource.msgMain_12}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error); //"���f�����ǂݍ��܂�Ă��܂���B"
                return;
            }
            if (viewer == null || viewer.IsDisposed)
            {
                viewer = new Viewer(this);
                button_DisplayModel.Text = Resources.Resource.button_DisplayModelUpdate;    // "�r���[�̍X�V";
                comboBox_RotationMethod.Enabled = false;
            }
            // �����`��ݒ�
            SetMaterialColor();
            SetLightingColor();
            SetBackgroundColor();
            viewer.Show();
            // ���f���̕`��
            viewer.Update();
        }
        /// <summary>
        /// �W���C�����j�^�J�n/���f�{�^������
        /// �J�n��A�{�^�����͒��f�ɕύX�i�g�O���ύX�j
        /// �J�n���ɒʐM���J�n����B���f���ɒʐM���L�����Z������B
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void Button_MonitorOnOff_Click(object sender, EventArgs e)
        {
            // �ʐM�|�[�g�ԍ����擾����
            int port = (int)numericUpDown_Port.Value;
            timeout = (int)numericUpDown_Timeout.Value;

            if (!stateMonitor)
            {
                // �W���C�����j�^�J�n
                if (viewer != null && !viewer.IsDisposed)
                {
                    viewer.StartGyroSensorServer(port, timeout);
                    viewer.Update();
                    stateMonitor = true; // �I�����C��
                    button_MonitorOnOff.Text = Resources.Resource.button_MonitorOff;    // "�W���C�����j�^���f";
                }
                else
                {
                    MessageBox.Show($"{Resources.Resource.msgMain_13}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error); //"�r���[���\������Ă��܂���B"
                    return;
                }
            }
            else
            {
                // �W���C�����j�^���f
                if (viewer != null && !viewer.IsDisposed && viewer.sensorMonitor != null)
                {
                    // ���j�^���L�����Z������
                    if (viewer.sensorMonitor.cancelToken != null)
                    {
                        viewer.sensorMonitor.cancelToken.Cancel();
                    }
                }
                stateMonitor = false; //�I�t���C��
                button_MonitorOnOff.Text = Resources.Resource.button_MonitorOnOff;  // "�W���C�����j�^�J�n";
            }
        }
        /// <summary>
        /// �}�e���A���F���X�V
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
        /// ���C�e�B���O�F���X�V
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
        /// �w�i�F���X�V
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
        /// ���C���t�H�[�������{�^������
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void Button_Close_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
        /// <summary>
        /// ���W�I�{�^������
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void RadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            // �`�F�b�N���ꂽ���W�I�{�^���𔻒�
            RadioButton? selectedRadio = sender as RadioButton;
            if (selectedRadio != null && selectedRadio.Checked)
            {
                // ���ׂẴ��W�I�{�^���𖳌�������
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

                //�`�F�b�N���ꂽ���W�I�{�^���̖��̂ɂ�蔻�肷��
                switch (selectedRadio.Tag)
                {
                    case "1": // "�}�e���A���J���[:":
                        // �D��F true: �}�e���A��, false: ���C�e�B���O
                        priorityColor = true;
                        // �}�e���A���J���[
                        numericUpDown_Red.Enabled = true;
                        numericUpDown_Green.Enabled = true;
                        numericUpDown_Blue.Enabled = true;
                        numericUpDown_Alfa.Enabled = true;
                        break;
                    case "2": // "���C�e�B���O�J���[:":
                        priorityColor = false;
                        // ���C�e�B���O�J���[
                        numericUpDown_lightR.Enabled = true;
                        numericUpDown_lightG.Enabled = true;
                        numericUpDown_lightB.Enabled = true;
                        numericUpDown_lightA.Enabled = true;
                        break;
                    case "3": // "�w�i�F:":
                        // �w�i�F true: RGB�w��, false: Texture
                        backcolorMode = true;
                        // �w�i�FRGB
                        numericUpDown_backcolorR.Enabled = true;
                        numericUpDown_backcolorG.Enabled = true;
                        numericUpDown_backcolorB.Enabled = true;
                        numericUpDown_backcolorA.Enabled = true;
                        break;
                    case "4": // "�e�N�X�`���[:":
                        // �w�i�FTexture
                        backcolorMode = false;
                        textBox_BitmapFileName.Enabled = true;
                        button_SelectBitmap.Enabled = true;
                        break;
                }
                // �ۂ��ĕ`��
                pictureBox_Palette.Invalidate();
                if (viewer != null)
                {
                    viewer.Update();
                }
            }
        }
        /// <summary>
        /// �p���b�g����}�E�X�N���b�N�������̏���
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void PictureBox_Palette_MouseClick(object sender, MouseEventArgs e)
        {
            // �N���b�N���ꂽ�ʒu�̐F���擾
            Bitmap bitmap = (Bitmap)pictureBox_Palette.Image;

            Color selectedColor = bitmap.GetPixel(e.X * bitmap.Width / pictureBox_Palette.Width,
                                                  e.Y * bitmap.Height / pictureBox_Palette.Height);
            // �I���ʒu��ۑ�
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

            // RGB �l��\��
            if (radioButton_MaterialColor.Checked)
            {
                // �v���r���[�G���A�̔w�i�F�ύX
                panel_MatPreview.BackColor = selectedColor;

                numericUpDown_Red.Value = selectedColor.R;
                numericUpDown_Green.Value = selectedColor.G;
                numericUpDown_Blue.Value = selectedColor.B;
                SetMaterialColor();
            }
            else if (radioButton_LightingColor.Checked)
            {
                // �v���r���[�G���A�̔w�i�F�ύX
                panel_LigPreview.BackColor = selectedColor;

                numericUpDown_lightR.Value = selectedColor.R;
                numericUpDown_lightG.Value = selectedColor.G;
                numericUpDown_lightB.Value = selectedColor.B;
                SetLightingColor();
            }
            else if (radioButton_RGBColor.Checked)
            {
                // �v���r���[�G���A�̔w�i�F�ύX
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
            // �ۂ��ĕ`��
            pictureBox_Palette.Invalidate();
        }
        /// <summary>
        /// �}�E�X�N���b�N�����_�Ɋۂ�`�悷��
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
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
        /// �J���[�p���b�g�̃J���[�e���v���[�g��؂�ւ���{�^������
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
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
        /// ���[�e�[�V�������@�̑I��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_RotationMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            rotationMethod = comboBox_RotationMethod.SelectedIndex;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// ��h�[����ON/OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_SkyDome_CheckedChanged(object sender, EventArgs e)
        {
            skydomeEnable = checkBox_SkyDome.Checked;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// ���b�V���O�����h��ONOFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_MeshGround_CheckedChanged(object sender, EventArgs e)
        {
            meshgroundEnable = checkBox_MeshGround.Checked;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// �s���͗l����ON/OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CheckGround_CheckedChanged(object sender, EventArgs e)
        {
            checkgroundEnable = checkBox_CheckGround.Checked;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// �e��ON/OFF
        /// </summary>
        /// <param name="sender">�I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�̒ǉ����</param>
        private void CheckBox_shadowEnable_CheckedChanged(object sender, EventArgs e)
        {
            shadowEnable = checkBox_shadowEnable.Checked;
            if (shadowEnable)
            {
                // �e�̕`�掞�͏������̓[���Œ�ŕҏW�s��
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
        /// ���W����ON/OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CoordinateAxes_CheckedChanged(object sender, EventArgs e)
        {
            coordinateAxesEnable = checkBox_CoordinateAxes.Checked;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// �������̐ݒ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_groundHight_ValueChanged(object sender, EventArgs e)
        {
            groundHight = (float)numericUpDown_groundHight.Value;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// ��h�[�����a�̐ݒ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_skydomeRadius_ValueChanged(object sender, EventArgs e)
        {
            skydomeRadius = (float)numericUpDown_skydomeRadius.Value;
            if (viewer != null) viewer.Update();
        }
        /// <summary>
        /// �r���[�{�[�g�̐ݒ�
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
        /// �}�e���A���J���[�̍X�V
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
        /// ���C�e�B���O�J���[�̍X�V
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
        /// �w�i�F�̍X�V
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
    /// �r���[�|�[�g�N���X��`
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
        /// �r���[�|�[�g�̃R���X�g���N�^
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
