namespace DaveTheMonitor.TMModCreator
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            MainForm form = new MainForm();
            Application.Run(form);
        }
    }
}