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
using System.Security.Cryptography;
using System.Threading;


namespace form1
{
    public partial class Formlogin : Form
    {
        Thread x;
        private MySqlConnection conexao;
        private string banco = "datasource=localhost;username=root;password=;database=playlogin";

        public Formlogin()
        {
            InitializeComponent();
        }
         int tentativas = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            // Método chamado ao clicar no botão de login
            login();
        }
        private void login() { 
            try
            {
                // Abre a conexão com o banco de dados
                conexao = new MySqlConnection(banco);
                string query = "SELECT nome,email,senha FROM login WHERE (nome=@nome OR email=@email )  ";
                conexao.Open();
                MySqlCommand command = new MySqlCommand(query, conexao);
                command.Parameters.AddWithValue("@nome", textBox3.Text);
                command.Parameters.AddWithValue("@email", textBox3.Text);
                //command.Parameters.AddWithValue("@senha", textBox2.Text);
                MySqlDataReader reader =
                command.ExecuteReader();


                if (reader.Read())
                {
                    //acesso a metodos do form2(cadastro)
                    Form2 form2 = new Form2();
                    /* Senha e salt armazenados no banco
                    string saltarmazenado = reader["salt"].ToString();
                    
                    byte[] saltBytes = Convert.FromBase64String(saltarmazenado);
                    string saltBase64 = Convert.ToBase64String(saltBytes);
                    */
                    string senhaArmazenada = reader["senha"].ToString();
                    // Calcular o hash da senha inserida pelo usuário usando o mesmo salt
                    string senhaInserida = textBox2.Text;
                    string hashInserido = form2.GerarHash(senhaInserida);



                    /* Exiba mensagens de log para depuração
                   MessageBox.Show("Senha inserida (hash): " + hashInserido);
                    MessageBox.Show("Senha armazenada: " + senhaArmazenada);*/
                    if (senhaInserida.Length < 8)
                    {
                        MessageBox.Show("Digite pelo menos 8 caracteres para a senha.");
                    }
                    else if (senhaArmazenada == hashInserido)
                    // Comparar o hash inserido com o hash armazenado
                    {
                        // Login bem-sucedido
                        reader.Close();
                        Form4 form4 = new Form4();
                        form4.Show();
                        this.Hide();
                    }
                    else if (senhaInserida == "")
                    {
                        // Login falhou (senha em branco)
                        MessageBox.Show("senha invalida.");
                    }

                    else
                    {
                        if (senhaArmazenada != hashInserido && senhaInserida.Length>=8)
                        {
                         
                                tentativas++;
                                if (tentativas == 3)
                                {
                                    button2.Visible = true;
                                    label1.Visible = true;
                                MessageBox.Show("Número máximo de tentativas alcançado. Use o botão de recuperação de senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                textBox2.Enabled = false; // Desabilita o campo de senha após o limite de tentativas
                                    tentativas = 0; // Reinicializa o número de tentativas
                                    return;
                                }
                                // Login falhou
                            else
                            {
                                MessageBox.Show("Usuário inválido. Tentativas restantes: " + (3 - tentativas));
                            }
                            textBox2.Text = ""; //apaga o que foi digitado no campo senha
                            textBox2.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
    
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            x = new Thread(abrir);
            x.SetApartmentState(ApartmentState.STA);
            x.Start();
        }
        private void abrir(object obj)
        {
            //Abre o formulário Frminicio em uma nova thread
            Application.Run(new Frminicio());
        }
        private void abrir2(object obj)
        {
            // Abre o formulário Form3 em uma nova thread
            Application.Run(new Form3());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Método chamado ao clicar no botão para fechar o formulário
            this.Close();
            x = new Thread(abrir2);
            x.SetApartmentState(ApartmentState.STA);
            x.Start();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Evento chamado ao pressionar Enter no campo de nome/email
            if (e.KeyChar == 13)
            {
                textBox2.Focus();
                e.Handled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Evento chamado ao marcar/desmarcar a caixa de senha visível
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }
    }
}

