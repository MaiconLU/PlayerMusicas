using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace form1
{
    public partial class Frminicio : Form
    {
         Thread x;
        public Frminicio()
        {
            InitializeComponent();
        }
        //botão login abre o Formlogin
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            x = new Thread(abrir2);
            x.SetApartmentState(ApartmentState.STA);
            x.Start();
        }
        //botão login abre o Form2(cadastro)
        private void button2_Click_1(object sender, EventArgs e)
        {
            
           this.Close();
        x = new Thread(abrir);
        x.SetApartmentState(ApartmentState.STA);
            x.Start();
        }
    private void abrir(object obj)
    {
            
       Application.Run(new Formlogin());
    }
    private void abrir2(object obj)
    {
       Application.Run(new Form2());
    }


}
}
