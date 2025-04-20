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

            // ����I���_�C�A���O��\��
            string selectedLanguage = "En"; // �f�t�H���g
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

            // �I�����������K�p
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);

            Application.Run(new Form_Main());
        }
    }
}