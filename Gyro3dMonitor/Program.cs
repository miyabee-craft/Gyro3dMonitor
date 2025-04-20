using System.Globalization;

namespace Gyro3dMonitor
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 言語選択ダイアログを表示
            string selectedLanguage = "En"; // デフォルト
            using (var langForm = new LanguageSelectionForm())
            {
                if (langForm.ShowDialog() == DialogResult.OK)
                {
                    if(langForm.SelectedLanguage != null)
                    {
                        selectedLanguage = langForm.SelectedLanguage;
                    }
                }
            }

            // 選択した言語を適用
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);

            Application.Run(new Form_Main());
        }
    }
}