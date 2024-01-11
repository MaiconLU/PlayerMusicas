using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
namespace form1
{
    public partial class Form3 : Form
    {
        Thread x;
        MySqlConnection conexao = new MySqlConnection();
        string db = "datasource=localhost;username=root;password=;database=playlogin";


        public Form3()
        {
            InitializeComponent();

        }

       //habilitar a visualização do campo senha 

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = checkBox2.Checked ? '\0' : '*';
        }
        //botão para recuperar a senha 
        private void button1_Click(object sender, EventArgs e)
        {

            string verificarp = textBox1.Text;
            string senha = textBox3.Text;
            //verificar senhas digitadas
            if (textBox2.Text == senha)
            {
                if (UsuarioEXiste(db, verificarp))
                {
                    try
                    {
                        //conexao = new MySqlConnection(db);
                        using (MySqlConnection conexao = new MySqlConnection(db))
                        {
                            //query para trocar a senha.
                            string query = "update login set senha = @senha where nome=@nome";
                            //MySqlCommand cmd = new MySqlCommand(query, conexao);
                            using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                            {
                                if (senha.Length < 8)
                                {
                                    MessageBox.Show("digite os caracters minimos");
                                }
                                else
                                {
                                    conexao.Open();
                                    cmd.Parameters.AddWithValue("@nome", textBox1.Text);
                                    cmd.Parameters.AddWithValue("@senha", senha);
                                    /*int rows =*/
                                    cmd.ExecuteReader();
                                    //if (rows > 0)
                                    //{
                                    Frminicio form = new Frminicio();
                                    form.Show();
                                    this.Hide();
                                    MessageBox.Show("Senha alterada");
                                    //}
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Erro ao atualizar senha: " + ex.Message);

                    }
                }
                else
                {
                    MessageBox.Show("Usuario não encontrado");
                }
                
            }
            else
            {
                MessageBox.Show("senhas não coicidem");
            }

        }
        //Método para verificar se o usario ja está cadastrado
        public bool UsuarioEXiste(string db, string verificarp)
        {
            bool usuarioexiste = false;
            try
            {
                conexao = new MySqlConnection(db);
                string query = "select count(*) from login where nome=@nome";
                MySqlCommand cmd = new MySqlCommand(query, conexao);
                conexao.Open();
                cmd.Parameters.AddWithValue("@nome", textBox1.Text);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                usuarioexiste = count > 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao verificar usuário: " + ex.Message);
            }

            // Adicione um log aqui para verificar o valor de usuarioExiste antes de retornar.
            
            finally
            {
                conexao.Close();
                MessageBox.Show("UsuarioExiste: " + usuarioexiste);
            }
            return usuarioexiste;
        }
        //botão que leva a o forminicial

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            x = new Thread(abrir);
            x.SetApartmentState(ApartmentState.STA);
            x.Start();
        }
        private void abrir(object obj)
        {

            Application.Run(new Frminicio());
        }

        private void gradient21_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    }


