namespace Gyro3dMonitor
{
    partial class LanguageSelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguageSelectionForm));
            comboBox_LanguageList = new ComboBox();
            label_LanguageSelection = new Label();
            button_OK = new Button();
            SuspendLayout();
            // 
            // comboBox_LanguageList
            // 
            comboBox_LanguageList.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_LanguageList.FormattingEnabled = true;
            comboBox_LanguageList.Location = new Point(30, 33);
            comboBox_LanguageList.Name = "comboBox_LanguageList";
            comboBox_LanguageList.Size = new Size(171, 23);
            comboBox_LanguageList.TabIndex = 0;
            // 
            // label_LanguageSelection
            // 
            label_LanguageSelection.AutoSize = true;
            label_LanguageSelection.Location = new Point(30, 15);
            label_LanguageSelection.Name = "label_LanguageSelection";
            label_LanguageSelection.Size = new Size(171, 15);
            label_LanguageSelection.TabIndex = 1;
            label_LanguageSelection.Text = "言語選択 (Language selection):";
            // 
            // button_OK
            // 
            button_OK.Location = new Point(74, 62);
            button_OK.Name = "button_OK";
            button_OK.Size = new Size(75, 23);
            button_OK.TabIndex = 2;
            button_OK.Text = "OK";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Click += Button_OK_Click;
            // 
            // LanguageSelectionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(238, 98);
            Controls.Add(button_OK);
            Controls.Add(label_LanguageSelection);
            Controls.Add(comboBox_LanguageList);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LanguageSelectionForm";
            Text = "Miyabee-Craft  3DV Dashboard";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox_LanguageList;
        private Label label_LanguageSelection;
        private Button button_OK;
    }
}