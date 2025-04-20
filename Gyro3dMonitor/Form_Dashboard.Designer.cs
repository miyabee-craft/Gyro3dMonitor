
using System.Windows.Forms;

namespace Gyro3dMonitor
{
    partial class Form_Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            button_LoadModel = new Button();
            button_DisplayModel = new Button();
            button_SaveImage = new Button();
            numericUpDown_Red = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            numericUpDown_Green = new NumericUpDown();
            label3 = new Label();
            numericUpDown_Blue = new NumericUpDown();
            label4 = new Label();
            numericUpDown_Alfa = new NumericUpDown();
            button_Close = new Button();
            label7 = new Label();
            numericUpDown_lightA = new NumericUpDown();
            label8 = new Label();
            numericUpDown_lightB = new NumericUpDown();
            label9 = new Label();
            numericUpDown_lightG = new NumericUpDown();
            label10 = new Label();
            numericUpDown_lightR = new NumericUpDown();
            label11 = new Label();
            radioButton_MaterialColor = new RadioButton();
            radioButton_LightingColor = new RadioButton();
            button_MonitorOnOff = new Button();
            label_Port = new Label();
            numericUpDown_Port = new NumericUpDown();
            label6 = new Label();
            numericUpDown_backcolorA = new NumericUpDown();
            label12 = new Label();
            numericUpDown_backcolorB = new NumericUpDown();
            label13 = new Label();
            numericUpDown_backcolorG = new NumericUpDown();
            label14 = new Label();
            numericUpDown_backcolorR = new NumericUpDown();
            textBox_BitmapFileName = new TextBox();
            button_SelectBitmap = new Button();
            pictureBox_Palette = new PictureBox();
            label_ColorPalette = new Label();
            panel_MatPreview = new Panel();
            label_MaterialColor = new Label();
            panel_LigPreview = new Panel();
            label_LightingColor = new Label();
            panel_BacPreview = new Panel();
            label_BackgroundColor = new Label();
            radioButton_RGBColor = new RadioButton();
            radioButton_Texture = new RadioButton();
            panel1 = new Panel();
            radioButton_Light = new RadioButton();
            radioButton_Dark = new RadioButton();
            comboBox_RotationMethod = new ComboBox();
            label_RotationMethod = new Label();
            checkBox_SkyDome = new CheckBox();
            checkBox_MeshGround = new CheckBox();
            checkBox_CheckGround = new CheckBox();
            label_Hight = new Label();
            numericUpDown_groundHight = new NumericUpDown();
            label_Radius = new Label();
            numericUpDown_skydomeRadius = new NumericUpDown();
            label_WH = new Label();
            label_near = new Label();
            label_far = new Label();
            label_FOV = new Label();
            pictureBox1 = new PictureBox();
            numericUpDown_Width = new NumericUpDown();
            numericUpDown_FOV = new NumericUpDown();
            numericUpDown_near = new NumericUpDown();
            numericUpDown_far = new NumericUpDown();
            label_VP = new Label();
            numericUpDown_Hight = new NumericUpDown();
            label_ViewportSettings = new Label();
            textBox_vpX = new TextBox();
            textBox_vpY = new TextBox();
            textBox_vpZ = new TextBox();
            checkBox_shadowEnable = new CheckBox();
            checkBox_CoordinateAxes = new CheckBox();
            label_Timeout = new Label();
            numericUpDown_Timeout = new NumericUpDown();
            label_GyroOperation = new Label();
            label_ViewerOperation = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Red).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Green).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Blue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Alfa).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_lightA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_lightB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_lightG).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_lightR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Port).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_backcolorA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_backcolorB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_backcolorG).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_backcolorR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Palette).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_groundHight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_skydomeRadius).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Width).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_FOV).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_near).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_far).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Hight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Timeout).BeginInit();
            SuspendLayout();
            // 
            // button_LoadModel
            // 
            button_LoadModel.Location = new Point(370, 76);
            button_LoadModel.Name = "button_LoadModel";
            button_LoadModel.Size = new Size(127, 44);
            button_LoadModel.TabIndex = 2;
            button_LoadModel.Text = "STLモデルの読込み";
            button_LoadModel.UseVisualStyleBackColor = true;
            button_LoadModel.Click += Button_LoadModel_Click;
            // 
            // button_DisplayModel
            // 
            button_DisplayModel.Location = new Point(370, 123);
            button_DisplayModel.Name = "button_DisplayModel";
            button_DisplayModel.Size = new Size(127, 44);
            button_DisplayModel.TabIndex = 3;
            button_DisplayModel.Text = "ビューの表示";
            button_DisplayModel.UseVisualStyleBackColor = true;
            button_DisplayModel.Click += Button_DisplayModel_Click;
            // 
            // button_SaveImage
            // 
            button_SaveImage.Location = new Point(370, 170);
            button_SaveImage.Name = "button_SaveImage";
            button_SaveImage.Size = new Size(127, 44);
            button_SaveImage.TabIndex = 4;
            button_SaveImage.Text = "キャプチャ・保存";
            button_SaveImage.UseVisualStyleBackColor = true;
            button_SaveImage.Click += Button_SaveImage_Click;
            // 
            // numericUpDown_Red
            // 
            numericUpDown_Red.Location = new Point(277, 29);
            numericUpDown_Red.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_Red.Name = "numericUpDown_Red";
            numericUpDown_Red.Size = new Size(64, 23);
            numericUpDown_Red.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(255, 33);
            label1.Name = "label1";
            label1.Size = new Size(17, 15);
            label1.TabIndex = 6;
            label1.Text = "R:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(255, 57);
            label2.Name = "label2";
            label2.Size = new Size(18, 15);
            label2.TabIndex = 8;
            label2.Text = "G:";
            // 
            // numericUpDown_Green
            // 
            numericUpDown_Green.Location = new Point(277, 54);
            numericUpDown_Green.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_Green.Name = "numericUpDown_Green";
            numericUpDown_Green.Size = new Size(64, 23);
            numericUpDown_Green.TabIndex = 7;
            numericUpDown_Green.Value = new decimal(new int[] { 150, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(255, 83);
            label3.Name = "label3";
            label3.Size = new Size(17, 15);
            label3.TabIndex = 10;
            label3.Text = "B:";
            // 
            // numericUpDown_Blue
            // 
            numericUpDown_Blue.Location = new Point(277, 79);
            numericUpDown_Blue.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_Blue.Name = "numericUpDown_Blue";
            numericUpDown_Blue.Size = new Size(64, 23);
            numericUpDown_Blue.TabIndex = 9;
            numericUpDown_Blue.Value = new decimal(new int[] { 140, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(255, 108);
            label4.Name = "label4";
            label4.Size = new Size(18, 15);
            label4.TabIndex = 12;
            label4.Text = "A:";
            // 
            // numericUpDown_Alfa
            // 
            numericUpDown_Alfa.Location = new Point(277, 104);
            numericUpDown_Alfa.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_Alfa.Name = "numericUpDown_Alfa";
            numericUpDown_Alfa.Size = new Size(64, 23);
            numericUpDown_Alfa.TabIndex = 11;
            numericUpDown_Alfa.Value = new decimal(new int[] { 255, 0, 0, 0 });
            // 
            // button_Close
            // 
            button_Close.Location = new Point(413, 625);
            button_Close.Name = "button_Close";
            button_Close.Size = new Size(75, 23);
            button_Close.TabIndex = 14;
            button_Close.Text = "閉じる";
            button_Close.UseVisualStyleBackColor = true;
            button_Close.Click += Button_Close_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(255, 237);
            label7.Name = "label7";
            label7.Size = new Size(18, 15);
            label7.TabIndex = 22;
            label7.Text = "A:";
            // 
            // numericUpDown_lightA
            // 
            numericUpDown_lightA.Enabled = false;
            numericUpDown_lightA.Location = new Point(277, 233);
            numericUpDown_lightA.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_lightA.Name = "numericUpDown_lightA";
            numericUpDown_lightA.Size = new Size(64, 23);
            numericUpDown_lightA.TabIndex = 21;
            numericUpDown_lightA.Value = new decimal(new int[] { 255, 0, 0, 0 });
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(255, 212);
            label8.Name = "label8";
            label8.Size = new Size(17, 15);
            label8.TabIndex = 20;
            label8.Text = "B:";
            // 
            // numericUpDown_lightB
            // 
            numericUpDown_lightB.Enabled = false;
            numericUpDown_lightB.Location = new Point(277, 208);
            numericUpDown_lightB.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_lightB.Name = "numericUpDown_lightB";
            numericUpDown_lightB.Size = new Size(64, 23);
            numericUpDown_lightB.TabIndex = 19;
            numericUpDown_lightB.Value = new decimal(new int[] { 150, 0, 0, 0 });
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(255, 187);
            label9.Name = "label9";
            label9.Size = new Size(18, 15);
            label9.TabIndex = 18;
            label9.Text = "G:";
            // 
            // numericUpDown_lightG
            // 
            numericUpDown_lightG.Enabled = false;
            numericUpDown_lightG.Location = new Point(277, 183);
            numericUpDown_lightG.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_lightG.Name = "numericUpDown_lightG";
            numericUpDown_lightG.Size = new Size(64, 23);
            numericUpDown_lightG.TabIndex = 17;
            numericUpDown_lightG.Value = new decimal(new int[] { 150, 0, 0, 0 });
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(255, 162);
            label10.Name = "label10";
            label10.Size = new Size(17, 15);
            label10.TabIndex = 16;
            label10.Text = "R:";
            // 
            // numericUpDown_lightR
            // 
            numericUpDown_lightR.Enabled = false;
            numericUpDown_lightR.Location = new Point(277, 158);
            numericUpDown_lightR.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_lightR.Name = "numericUpDown_lightR";
            numericUpDown_lightR.Size = new Size(64, 23);
            numericUpDown_lightR.TabIndex = 15;
            numericUpDown_lightR.Value = new decimal(new int[] { 150, 0, 0, 0 });
            // 
            // label11
            // 
            label11.BorderStyle = BorderStyle.Fixed3D;
            label11.Location = new Point(3, 618);
            label11.Name = "label11";
            label11.Size = new Size(500, 1);
            label11.TabIndex = 24;
            // 
            // radioButton_MaterialColor
            // 
            radioButton_MaterialColor.AutoSize = true;
            radioButton_MaterialColor.Checked = true;
            radioButton_MaterialColor.Location = new Point(249, 5);
            radioButton_MaterialColor.Name = "radioButton_MaterialColor";
            radioButton_MaterialColor.Size = new Size(95, 19);
            radioButton_MaterialColor.TabIndex = 26;
            radioButton_MaterialColor.TabStop = true;
            radioButton_MaterialColor.Tag = "1";
            radioButton_MaterialColor.Text = "マテリアルカラー";
            radioButton_MaterialColor.UseVisualStyleBackColor = true;
            // 
            // radioButton_LightingColor
            // 
            radioButton_LightingColor.AutoSize = true;
            radioButton_LightingColor.Location = new Point(249, 135);
            radioButton_LightingColor.Name = "radioButton_LightingColor";
            radioButton_LightingColor.Size = new Size(101, 19);
            radioButton_LightingColor.TabIndex = 27;
            radioButton_LightingColor.Tag = "2";
            radioButton_LightingColor.Text = "ライティングカラー";
            radioButton_LightingColor.UseVisualStyleBackColor = true;
            // 
            // button_MonitorOnOff
            // 
            button_MonitorOnOff.Location = new Point(370, 305);
            button_MonitorOnOff.Name = "button_MonitorOnOff";
            button_MonitorOnOff.Size = new Size(127, 44);
            button_MonitorOnOff.TabIndex = 28;
            button_MonitorOnOff.Text = "ジャイロモニタ開始";
            button_MonitorOnOff.UseVisualStyleBackColor = true;
            button_MonitorOnOff.Click += Button_MonitorOnOff_Click;
            // 
            // label_Port
            // 
            label_Port.AutoSize = true;
            label_Port.Location = new Point(370, 251);
            label_Port.Name = "label_Port";
            label_Port.Size = new Size(60, 15);
            label_Port.TabIndex = 29;
            label_Port.Text = "通信ポート:";
            // 
            // numericUpDown_Port
            // 
            numericUpDown_Port.Location = new Point(432, 247);
            numericUpDown_Port.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numericUpDown_Port.Minimum = new decimal(new int[] { 49152, 0, 0, 0 });
            numericUpDown_Port.Name = "numericUpDown_Port";
            numericUpDown_Port.Size = new Size(63, 23);
            numericUpDown_Port.TabIndex = 30;
            numericUpDown_Port.TextAlign = HorizontalAlignment.Right;
            numericUpDown_Port.Value = new decimal(new int[] { 60001, 0, 0, 0 });
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(255, 365);
            label6.Name = "label6";
            label6.Size = new Size(18, 15);
            label6.TabIndex = 38;
            label6.Text = "A:";
            // 
            // numericUpDown_backcolorA
            // 
            numericUpDown_backcolorA.Enabled = false;
            numericUpDown_backcolorA.Location = new Point(277, 361);
            numericUpDown_backcolorA.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_backcolorA.Name = "numericUpDown_backcolorA";
            numericUpDown_backcolorA.Size = new Size(64, 23);
            numericUpDown_backcolorA.TabIndex = 37;
            numericUpDown_backcolorA.Value = new decimal(new int[] { 255, 0, 0, 0 });
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(255, 340);
            label12.Name = "label12";
            label12.Size = new Size(17, 15);
            label12.TabIndex = 36;
            label12.Text = "B:";
            // 
            // numericUpDown_backcolorB
            // 
            numericUpDown_backcolorB.Enabled = false;
            numericUpDown_backcolorB.Location = new Point(277, 336);
            numericUpDown_backcolorB.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_backcolorB.Name = "numericUpDown_backcolorB";
            numericUpDown_backcolorB.Size = new Size(64, 23);
            numericUpDown_backcolorB.TabIndex = 35;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(255, 315);
            label13.Name = "label13";
            label13.Size = new Size(18, 15);
            label13.TabIndex = 34;
            label13.Text = "G:";
            // 
            // numericUpDown_backcolorG
            // 
            numericUpDown_backcolorG.Enabled = false;
            numericUpDown_backcolorG.Location = new Point(277, 311);
            numericUpDown_backcolorG.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_backcolorG.Name = "numericUpDown_backcolorG";
            numericUpDown_backcolorG.Size = new Size(64, 23);
            numericUpDown_backcolorG.TabIndex = 33;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(255, 290);
            label14.Name = "label14";
            label14.Size = new Size(17, 15);
            label14.TabIndex = 32;
            label14.Text = "R:";
            // 
            // numericUpDown_backcolorR
            // 
            numericUpDown_backcolorR.Enabled = false;
            numericUpDown_backcolorR.Location = new Point(277, 286);
            numericUpDown_backcolorR.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numericUpDown_backcolorR.Name = "numericUpDown_backcolorR";
            numericUpDown_backcolorR.Size = new Size(64, 23);
            numericUpDown_backcolorR.TabIndex = 31;
            // 
            // textBox_BitmapFileName
            // 
            textBox_BitmapFileName.Enabled = false;
            textBox_BitmapFileName.Location = new Point(246, 410);
            textBox_BitmapFileName.Name = "textBox_BitmapFileName";
            textBox_BitmapFileName.Size = new Size(90, 23);
            textBox_BitmapFileName.TabIndex = 40;
            // 
            // button_SelectBitmap
            // 
            button_SelectBitmap.Enabled = false;
            button_SelectBitmap.Location = new Point(337, 410);
            button_SelectBitmap.Name = "button_SelectBitmap";
            button_SelectBitmap.Size = new Size(24, 24);
            button_SelectBitmap.TabIndex = 41;
            button_SelectBitmap.Text = "...";
            button_SelectBitmap.UseVisualStyleBackColor = true;
            button_SelectBitmap.Click += Button_SelectBitmap_Click;
            // 
            // pictureBox_Palette
            // 
            pictureBox_Palette.Image = Properties.Resources.pallete255x255;
            pictureBox_Palette.Location = new Point(2, 26);
            pictureBox_Palette.Name = "pictureBox_Palette";
            pictureBox_Palette.Size = new Size(240, 240);
            pictureBox_Palette.TabIndex = 42;
            pictureBox_Palette.TabStop = false;
            pictureBox_Palette.Paint += PictureBox_Palette_Paint;
            pictureBox_Palette.MouseClick += PictureBox_Palette_MouseClick;
            // 
            // label_ColorPalette
            // 
            label_ColorPalette.AutoSize = true;
            label_ColorPalette.Location = new Point(2, 5);
            label_ColorPalette.Name = "label_ColorPalette";
            label_ColorPalette.Size = new Size(70, 15);
            label_ColorPalette.TabIndex = 43;
            label_ColorPalette.Text = "カラーパレット:";
            // 
            // panel_MatPreview
            // 
            panel_MatPreview.Location = new Point(2, 284);
            panel_MatPreview.Name = "panel_MatPreview";
            panel_MatPreview.Size = new Size(75, 54);
            panel_MatPreview.TabIndex = 44;
            // 
            // label_MaterialColor
            // 
            label_MaterialColor.AutoSize = true;
            label_MaterialColor.Location = new Point(2, 268);
            label_MaterialColor.Name = "label_MaterialColor";
            label_MaterialColor.Size = new Size(55, 15);
            label_MaterialColor.TabIndex = 45;
            label_MaterialColor.Text = "マテリアル:";
            // 
            // panel_LigPreview
            // 
            panel_LigPreview.Location = new Point(83, 284);
            panel_LigPreview.Name = "panel_LigPreview";
            panel_LigPreview.Size = new Size(75, 54);
            panel_LigPreview.TabIndex = 45;
            // 
            // label_LightingColor
            // 
            label_LightingColor.AutoSize = true;
            label_LightingColor.Location = new Point(84, 268);
            label_LightingColor.Name = "label_LightingColor";
            label_LightingColor.Size = new Size(61, 15);
            label_LightingColor.TabIndex = 46;
            label_LightingColor.Text = "ライティング:";
            // 
            // panel_BacPreview
            // 
            panel_BacPreview.Location = new Point(164, 284);
            panel_BacPreview.Name = "panel_BacPreview";
            panel_BacPreview.Size = new Size(75, 54);
            panel_BacPreview.TabIndex = 47;
            // 
            // label_BackgroundColor
            // 
            label_BackgroundColor.AutoSize = true;
            label_BackgroundColor.Location = new Point(164, 268);
            label_BackgroundColor.Name = "label_BackgroundColor";
            label_BackgroundColor.Size = new Size(34, 15);
            label_BackgroundColor.TabIndex = 48;
            label_BackgroundColor.Text = "背景:";
            // 
            // radioButton_RGBColor
            // 
            radioButton_RGBColor.AutoSize = true;
            radioButton_RGBColor.Location = new Point(249, 262);
            radioButton_RGBColor.Name = "radioButton_RGBColor";
            radioButton_RGBColor.Size = new Size(61, 19);
            radioButton_RGBColor.TabIndex = 0;
            radioButton_RGBColor.Tag = "3";
            radioButton_RGBColor.Text = "背景色";
            radioButton_RGBColor.UseVisualStyleBackColor = true;
            // 
            // radioButton_Texture
            // 
            radioButton_Texture.AutoSize = true;
            radioButton_Texture.Location = new Point(249, 387);
            radioButton_Texture.Name = "radioButton_Texture";
            radioButton_Texture.Size = new Size(77, 19);
            radioButton_Texture.TabIndex = 1;
            radioButton_Texture.Tag = "4";
            radioButton_Texture.Text = "テクスチャー";
            radioButton_Texture.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(radioButton_Light);
            panel1.Controls.Add(radioButton_Dark);
            panel1.Location = new Point(100, -2);
            panel1.Name = "panel1";
            panel1.Size = new Size(137, 28);
            panel1.TabIndex = 49;
            // 
            // radioButton_Light
            // 
            radioButton_Light.AutoSize = true;
            radioButton_Light.Location = new Point(75, 6);
            radioButton_Light.Name = "radioButton_Light";
            radioButton_Light.Size = new Size(50, 19);
            radioButton_Light.TabIndex = 1;
            radioButton_Light.TabStop = true;
            radioButton_Light.Text = "ライト";
            radioButton_Light.UseVisualStyleBackColor = true;
            // 
            // radioButton_Dark
            // 
            radioButton_Dark.AutoSize = true;
            radioButton_Dark.Checked = true;
            radioButton_Dark.Location = new Point(9, 6);
            radioButton_Dark.Name = "radioButton_Dark";
            radioButton_Dark.Size = new Size(51, 19);
            radioButton_Dark.TabIndex = 0;
            radioButton_Dark.TabStop = true;
            radioButton_Dark.Text = "ダーク";
            radioButton_Dark.UseVisualStyleBackColor = true;
            radioButton_Dark.CheckedChanged += RadioButton_Dark_CheckedChanged;
            // 
            // comboBox_RotationMethod
            // 
            comboBox_RotationMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_RotationMethod.FormattingEnabled = true;
            comboBox_RotationMethod.Location = new Point(370, 47);
            comboBox_RotationMethod.Name = "comboBox_RotationMethod";
            comboBox_RotationMethod.Size = new Size(127, 23);
            comboBox_RotationMethod.TabIndex = 50;
            comboBox_RotationMethod.SelectedIndexChanged += ComboBox_RotationMethod_SelectedIndexChanged;
            // 
            // label_RotationMethod
            // 
            label_RotationMethod.AutoSize = true;
            label_RotationMethod.Location = new Point(369, 29);
            label_RotationMethod.Name = "label_RotationMethod";
            label_RotationMethod.Size = new Size(46, 15);
            label_RotationMethod.TabIndex = 51;
            label_RotationMethod.Text = "回転法:";
            // 
            // checkBox_SkyDome
            // 
            checkBox_SkyDome.AutoSize = true;
            checkBox_SkyDome.Location = new Point(8, 342);
            checkBox_SkyDome.Name = "checkBox_SkyDome";
            checkBox_SkyDome.Size = new Size(103, 19);
            checkBox_SkyDome.TabIndex = 52;
            checkBox_SkyDome.Text = "青空ドーム---->";
            checkBox_SkyDome.UseVisualStyleBackColor = true;
            checkBox_SkyDome.CheckedChanged += CheckBox_SkyDome_CheckedChanged;
            // 
            // checkBox_MeshGround
            // 
            checkBox_MeshGround.AutoSize = true;
            checkBox_MeshGround.Location = new Point(8, 361);
            checkBox_MeshGround.Name = "checkBox_MeshGround";
            checkBox_MeshGround.Size = new Size(94, 19);
            checkBox_MeshGround.TabIndex = 53;
            checkBox_MeshGround.Text = "メッシュグランド";
            checkBox_MeshGround.UseVisualStyleBackColor = true;
            checkBox_MeshGround.CheckedChanged += CheckBox_MeshGround_CheckedChanged;
            // 
            // checkBox_CheckGround
            // 
            checkBox_CheckGround.AutoSize = true;
            checkBox_CheckGround.Location = new Point(8, 380);
            checkBox_CheckGround.Name = "checkBox_CheckGround";
            checkBox_CheckGround.Size = new Size(86, 19);
            checkBox_CheckGround.TabIndex = 54;
            checkBox_CheckGround.Text = "市松模様床";
            checkBox_CheckGround.UseVisualStyleBackColor = true;
            checkBox_CheckGround.CheckedChanged += CheckBox_CheckGround_CheckedChanged;
            // 
            // label_Hight
            // 
            label_Hight.AutoSize = true;
            label_Hight.Location = new Point(133, 372);
            label_Hight.Name = "label_Hight";
            label_Hight.Size = new Size(42, 15);
            label_Hight.TabIndex = 56;
            label_Hight.Text = "］高さ:";
            // 
            // numericUpDown_groundHight
            // 
            numericUpDown_groundHight.Location = new Point(184, 368);
            numericUpDown_groundHight.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDown_groundHight.Minimum = new decimal(new int[] { 200, 0, 0, int.MinValue });
            numericUpDown_groundHight.Name = "numericUpDown_groundHight";
            numericUpDown_groundHight.Size = new Size(53, 23);
            numericUpDown_groundHight.TabIndex = 57;
            numericUpDown_groundHight.ValueChanged += NumericUpDown_groundHight_ValueChanged;
            // 
            // label_Radius
            // 
            label_Radius.AutoSize = true;
            label_Radius.Location = new Point(141, 345);
            label_Radius.Name = "label_Radius";
            label_Radius.Size = new Size(34, 15);
            label_Radius.TabIndex = 58;
            label_Radius.Text = "半径:";
            // 
            // numericUpDown_skydomeRadius
            // 
            numericUpDown_skydomeRadius.Location = new Point(184, 341);
            numericUpDown_skydomeRadius.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDown_skydomeRadius.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_skydomeRadius.Name = "numericUpDown_skydomeRadius";
            numericUpDown_skydomeRadius.Size = new Size(53, 23);
            numericUpDown_skydomeRadius.TabIndex = 59;
            numericUpDown_skydomeRadius.TabStop = false;
            numericUpDown_skydomeRadius.Value = new decimal(new int[] { 200, 0, 0, 0 });
            numericUpDown_skydomeRadius.ValueChanged += NumericUpDown_skydomeRadius_ValueChanged;
            // 
            // label_WH
            // 
            label_WH.AutoSize = true;
            label_WH.Location = new Point(253, 493);
            label_WH.Name = "label_WH";
            label_WH.Size = new Size(35, 15);
            label_WH.TabIndex = 60;
            label_WH.Text = "W, H:";
            // 
            // label_near
            // 
            label_near.AutoSize = true;
            label_near.Location = new Point(255, 545);
            label_near.Name = "label_near";
            label_near.Size = new Size(33, 15);
            label_near.TabIndex = 61;
            label_near.Text = "near:";
            // 
            // label_far
            // 
            label_far.AutoSize = true;
            label_far.Location = new Point(264, 571);
            label_far.Name = "label_far";
            label_far.Size = new Size(24, 15);
            label_far.TabIndex = 62;
            label_far.Text = "far:";
            // 
            // label_FOV
            // 
            label_FOV.AutoSize = true;
            label_FOV.Location = new Point(256, 519);
            label_FOV.Name = "label_FOV";
            label_FOV.Size = new Size(32, 15);
            label_FOV.TabIndex = 63;
            label_FOV.Text = "FOV:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.viewport;
            pictureBox1.Location = new Point(13, 442);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(210, 165);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 64;
            pictureBox1.TabStop = false;
            // 
            // numericUpDown_Width
            // 
            numericUpDown_Width.Location = new Point(290, 489);
            numericUpDown_Width.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numericUpDown_Width.Name = "numericUpDown_Width";
            numericUpDown_Width.Size = new Size(65, 23);
            numericUpDown_Width.TabIndex = 65;
            // 
            // numericUpDown_FOV
            // 
            numericUpDown_FOV.Location = new Point(290, 515);
            numericUpDown_FOV.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            numericUpDown_FOV.Name = "numericUpDown_FOV";
            numericUpDown_FOV.Size = new Size(65, 23);
            numericUpDown_FOV.TabIndex = 65;
            // 
            // numericUpDown_near
            // 
            numericUpDown_near.Location = new Point(290, 541);
            numericUpDown_near.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDown_near.Name = "numericUpDown_near";
            numericUpDown_near.Size = new Size(65, 23);
            numericUpDown_near.TabIndex = 65;
            // 
            // numericUpDown_far
            // 
            numericUpDown_far.Location = new Point(290, 568);
            numericUpDown_far.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDown_far.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown_far.Name = "numericUpDown_far";
            numericUpDown_far.Size = new Size(65, 23);
            numericUpDown_far.TabIndex = 65;
            numericUpDown_far.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label_VP
            // 
            label_VP.AutoSize = true;
            label_VP.Location = new Point(239, 467);
            label_VP.Name = "label_VP";
            label_VP.Size = new Size(49, 15);
            label_VP.TabIndex = 60;
            label_VP.Text = "VP x,y,z:";
            // 
            // numericUpDown_Hight
            // 
            numericUpDown_Hight.Location = new Point(361, 489);
            numericUpDown_Hight.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numericUpDown_Hight.Name = "numericUpDown_Hight";
            numericUpDown_Hight.Size = new Size(65, 23);
            numericUpDown_Hight.TabIndex = 65;
            // 
            // label_ViewportSettings
            // 
            label_ViewportSettings.AutoSize = true;
            label_ViewportSettings.Location = new Point(232, 445);
            label_ViewportSettings.Name = "label_ViewportSettings";
            label_ViewportSettings.Size = new Size(154, 15);
            label_ViewportSettings.TabIndex = 66;
            label_ViewportSettings.Text = "【 ビューポート設定(VPは自動) 】";
            // 
            // textBox_vpX
            // 
            textBox_vpX.Location = new Point(290, 463);
            textBox_vpX.Name = "textBox_vpX";
            textBox_vpX.ReadOnly = true;
            textBox_vpX.Size = new Size(65, 23);
            textBox_vpX.TabIndex = 67;
            // 
            // textBox_vpY
            // 
            textBox_vpY.Location = new Point(358, 463);
            textBox_vpY.Name = "textBox_vpY";
            textBox_vpY.ReadOnly = true;
            textBox_vpY.Size = new Size(65, 23);
            textBox_vpY.TabIndex = 67;
            // 
            // textBox_vpZ
            // 
            textBox_vpZ.Location = new Point(427, 463);
            textBox_vpZ.Name = "textBox_vpZ";
            textBox_vpZ.ReadOnly = true;
            textBox_vpZ.Size = new Size(65, 23);
            textBox_vpZ.TabIndex = 67;
            // 
            // checkBox_shadowEnable
            // 
            checkBox_shadowEnable.AutoSize = true;
            checkBox_shadowEnable.Location = new Point(8, 399);
            checkBox_shadowEnable.Name = "checkBox_shadowEnable";
            checkBox_shadowEnable.Size = new Size(137, 19);
            checkBox_shadowEnable.TabIndex = 68;
            checkBox_shadowEnable.Text = "影（床高さゼロ固定）";
            checkBox_shadowEnable.UseVisualStyleBackColor = true;
            checkBox_shadowEnable.CheckedChanged += CheckBox_shadowEnable_CheckedChanged;
            // 
            // checkBox_CoordinateAxes
            // 
            checkBox_CoordinateAxes.AutoSize = true;
            checkBox_CoordinateAxes.Location = new Point(8, 418);
            checkBox_CoordinateAxes.Name = "checkBox_CoordinateAxes";
            checkBox_CoordinateAxes.Size = new Size(62, 19);
            checkBox_CoordinateAxes.TabIndex = 69;
            checkBox_CoordinateAxes.Text = "座標軸";
            checkBox_CoordinateAxes.UseVisualStyleBackColor = true;
            checkBox_CoordinateAxes.CheckedChanged += CheckBox_CoordinateAxes_CheckedChanged;
            // 
            // label_Timeout
            // 
            label_Timeout.AutoSize = true;
            label_Timeout.Location = new Point(370, 280);
            label_Timeout.Name = "label_Timeout";
            label_Timeout.Size = new Size(83, 15);
            label_Timeout.TabIndex = 70;
            label_Timeout.Text = "タイムアウト(秒):";
            // 
            // numericUpDown_Timeout
            // 
            numericUpDown_Timeout.Location = new Point(452, 276);
            numericUpDown_Timeout.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            numericUpDown_Timeout.Name = "numericUpDown_Timeout";
            numericUpDown_Timeout.Size = new Size(43, 23);
            numericUpDown_Timeout.TabIndex = 71;
            numericUpDown_Timeout.TextAlign = HorizontalAlignment.Right;
            numericUpDown_Timeout.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // label_GyroOperation
            // 
            label_GyroOperation.AutoSize = true;
            label_GyroOperation.Location = new Point(352, 229);
            label_GyroOperation.Name = "label_GyroOperation";
            label_GyroOperation.Size = new Size(85, 15);
            label_GyroOperation.TabIndex = 72;
            label_GyroOperation.Text = "【 ジャイロ操作 】";
            // 
            // label_ViewerOperation
            // 
            label_ViewerOperation.AutoSize = true;
            label_ViewerOperation.Location = new Point(352, 6);
            label_ViewerOperation.Name = "label_ViewerOperation";
            label_ViewerOperation.Size = new Size(81, 15);
            label_ViewerOperation.TabIndex = 75;
            label_ViewerOperation.Text = "【 ビューワ操作 】";
            // 
            // Form_Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(505, 656);
            Controls.Add(label_ViewerOperation);
            Controls.Add(label_GyroOperation);
            Controls.Add(numericUpDown_Timeout);
            Controls.Add(label_Timeout);
            Controls.Add(checkBox_CoordinateAxes);
            Controls.Add(checkBox_shadowEnable);
            Controls.Add(textBox_vpZ);
            Controls.Add(textBox_vpY);
            Controls.Add(textBox_vpX);
            Controls.Add(label_ViewportSettings);
            Controls.Add(numericUpDown_far);
            Controls.Add(numericUpDown_near);
            Controls.Add(numericUpDown_FOV);
            Controls.Add(numericUpDown_Hight);
            Controls.Add(numericUpDown_Width);
            Controls.Add(pictureBox1);
            Controls.Add(label_FOV);
            Controls.Add(label_far);
            Controls.Add(label_near);
            Controls.Add(label_VP);
            Controls.Add(label_WH);
            Controls.Add(numericUpDown_skydomeRadius);
            Controls.Add(label_Radius);
            Controls.Add(numericUpDown_groundHight);
            Controls.Add(label_Hight);
            Controls.Add(checkBox_CheckGround);
            Controls.Add(checkBox_MeshGround);
            Controls.Add(checkBox_SkyDome);
            Controls.Add(label_RotationMethod);
            Controls.Add(comboBox_RotationMethod);
            Controls.Add(panel1);
            Controls.Add(radioButton_Texture);
            Controls.Add(radioButton_RGBColor);
            Controls.Add(label_BackgroundColor);
            Controls.Add(panel_BacPreview);
            Controls.Add(label_LightingColor);
            Controls.Add(panel_LigPreview);
            Controls.Add(label_MaterialColor);
            Controls.Add(panel_MatPreview);
            Controls.Add(label_ColorPalette);
            Controls.Add(pictureBox_Palette);
            Controls.Add(button_SelectBitmap);
            Controls.Add(textBox_BitmapFileName);
            Controls.Add(label6);
            Controls.Add(numericUpDown_backcolorA);
            Controls.Add(label12);
            Controls.Add(numericUpDown_backcolorB);
            Controls.Add(label13);
            Controls.Add(numericUpDown_backcolorG);
            Controls.Add(label14);
            Controls.Add(numericUpDown_backcolorR);
            Controls.Add(numericUpDown_Port);
            Controls.Add(label_Port);
            Controls.Add(button_MonitorOnOff);
            Controls.Add(radioButton_LightingColor);
            Controls.Add(radioButton_MaterialColor);
            Controls.Add(label11);
            Controls.Add(label7);
            Controls.Add(numericUpDown_lightA);
            Controls.Add(label8);
            Controls.Add(numericUpDown_lightB);
            Controls.Add(label9);
            Controls.Add(numericUpDown_lightG);
            Controls.Add(label10);
            Controls.Add(numericUpDown_lightR);
            Controls.Add(button_Close);
            Controls.Add(label4);
            Controls.Add(numericUpDown_Alfa);
            Controls.Add(label3);
            Controls.Add(numericUpDown_Blue);
            Controls.Add(label2);
            Controls.Add(numericUpDown_Green);
            Controls.Add(label1);
            Controls.Add(numericUpDown_Red);
            Controls.Add(button_SaveImage);
            Controls.Add(button_DisplayModel);
            Controls.Add(button_LoadModel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form_Main";
            Text = "Miyabee-Craft 3DV Dashboard";
            Load += Form_Main_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Red).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Green).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Blue).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Alfa).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_lightA).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_lightB).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_lightG).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_lightR).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Port).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_backcolorA).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_backcolorB).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_backcolorG).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_backcolorR).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Palette).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_groundHight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_skydomeRadius).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Width).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_FOV).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_near).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_far).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Hight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_Timeout).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button_LoadModel;
        private Button button_DisplayModel;
        private Button button_SaveImage;
        private NumericUpDown numericUpDown_Red;
        private Label label1;
        private Label label2;
        private NumericUpDown numericUpDown_Green;
        private Label label3;
        private NumericUpDown numericUpDown_Blue;
        private Label label4;
        private NumericUpDown numericUpDown_Alfa;
        private Button button_Close;
        private Label label7;
        private NumericUpDown numericUpDown_lightA;
        private Label label8;
        private NumericUpDown numericUpDown_lightB;
        private Label label9;
        private NumericUpDown numericUpDown_lightG;
        private Label label10;
        private NumericUpDown numericUpDown_lightR;
        private Label label11;
        private RadioButton radioButton_MaterialColor;
        private RadioButton radioButton_LightingColor;
        private Button button_MonitorOnOff;
        private Label label_Port;
        private NumericUpDown numericUpDown_Port;
        private Label label6;
        private NumericUpDown numericUpDown_backcolorA;
        private Label label12;
        private NumericUpDown numericUpDown_backcolorB;
        private Label label13;
        private NumericUpDown numericUpDown_backcolorG;
        private Label label14;
        private NumericUpDown numericUpDown_backcolorR;
        private TextBox textBox_BitmapFileName;
        private Button button_SelectBitmap;
        private PictureBox pictureBox_Palette;
        private Label label_ColorPalette;
        private Panel panel_MatPreview;
        private Label label_MaterialColor;
        private Panel panel_LigPreview;
        private Label label_LightingColor;
        private Panel panel_BacPreview;
        private Label label_BackgroundColor;
        private RadioButton radioButton_RGBColor;
        private RadioButton radioButton_Texture;
        private Panel panel1;
        private RadioButton radioButton_Dark;
        private RadioButton radioButton_Light;
        private ComboBox comboBox_RotationMethod;
        private Label label_RotationMethod;
        private CheckBox checkBox_SkyDome;
        private CheckBox checkBox_MeshGround;
        private CheckBox checkBox_CheckGround;
        private Label label_Hight;
        private NumericUpDown numericUpDown_groundHight;
        private Label label_Radius;
        private NumericUpDown numericUpDown_skydomeRadius;
        private Label label_WH;
        private Label label_near;
        private Label label_far;
        private Label label_FOV;
        private PictureBox pictureBox1;
        private NumericUpDown numericUpDown_Width;
        private NumericUpDown numericUpDown_FOV;
        private NumericUpDown numericUpDown_near;
        private NumericUpDown numericUpDown_far;
        private Label label_VP;
        private NumericUpDown numericUpDown_Hight;
        private Label label_ViewportSettings;
        private TextBox textBox_vpX;
        private TextBox textBox_vpY;
        private TextBox textBox_vpZ;
        private CheckBox checkBox_shadowEnable;
        private CheckBox checkBox_CoordinateAxes;
        private Label label_Timeout;
        private NumericUpDown numericUpDown_Timeout;
        private Label label_GyroOperation;
        private Label label_ViewerOperation;
    }
}
