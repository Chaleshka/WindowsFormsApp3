using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DatesAndWeeks";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(path + "/Dates.ini"))
                File.Create(path + "/Dates.ini");
            if (!File.Exists(path + "/Weeks.ini"))
                File.Create(path + "/Weeks.ini");
            Application.Run(new Form1());
        }
    }
}
