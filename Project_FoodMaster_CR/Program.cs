using appFoodMaster_CR.Layer.UI;
using appFoodMaster_CR.Layer.UI.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_FoodMaster_CR
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmLogin frmLogin = new frmLogin();

            frmLogin.ShowDialog();

            if (frmLogin.DialogResult == DialogResult.OK)
            {
                Application.Run(new frmMenu());
            }
        }
    }
}
