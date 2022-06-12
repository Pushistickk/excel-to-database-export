using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace excel_to_database
{
    internal static class Program
    {
        public static DatabaseConnection DBC = new DatabaseConnection();
        public static vdkContext database = new vdkContext();
        public static Recorder recorder = new Recorder();
        public static List<DataGridView> dataGrids = new List<DataGridView>();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if(!DBC.TestConnection())
            {
                MessageBox.Show("Нет подключения");
                Application.Exit();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}