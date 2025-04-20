using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gyro3dMonitor
{
    /// <summary>
    /// 表示言語選択フォームのクラス定義
    /// </summary>
    public partial class LanguageSelectionForm : Form
    {
        public string? SelectedLanguage { get; private set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LanguageSelectionForm()
        {
            InitializeComponent();
            comboBox_LanguageList.Items.Add("日本語( Japanese )");
            comboBox_LanguageList.Items.Add("英語( English )");
            comboBox_LanguageList.Items.Add("韓国語( Korean )");
            comboBox_LanguageList.Items.Add("スペイン語語( Español )");
            comboBox_LanguageList.SelectedIndex = 0; // デフォルト: 日本語
        }
        /// <summary>
        /// OKボタンの処理
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベントの追加情報</param>
        private void Button_OK_Click(object sender, EventArgs e)
        {
            // 選択した言語を取得
            if (comboBox_LanguageList.SelectedIndex == 1)
                SelectedLanguage = "En";
            else if (comboBox_LanguageList.SelectedIndex == 0)
                SelectedLanguage = "Ja";
            else if (comboBox_LanguageList.SelectedIndex == 2)
                SelectedLanguage = "Ko";
            else
                SelectedLanguage = "Es";
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
