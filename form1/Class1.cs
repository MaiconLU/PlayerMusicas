using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace form1
{
    public static class FormManager3
    {
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Abrir o form desejado
            Application.Run(new Frminicio()); 
        }
    }
}

