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

namespace form1
{
    //formcadastro
    public partial class Form2 : Form
    {
        // Conexão com o banco de dados MySQL
        private MySqlConnection conexao;
        private string banco = "datasource=localhost;username=root;password=;database=playlogin";
        public Form2()
        {
            InitializeComponent();
        }

        // Evento chamado ao marcar/desmarcar a caixa de senha visível
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar=checkBox1.Checked ? '\0':'*';
        }

        // Evento chamado ao clicar no botão de cadastro

        private void button1_Click(object sender, EventArgs e)
        {
            string senha = textBox2.Text;
            try
            {
                conexao = new MySqlConnection(banco);
                conexao.Open();
                // Verifica se o e-mail já foi utilizado
                string queryemail = "SELECT COUNT(*) FROM login WHERE email = @email";
                MySqlCommand comandochecar = new MySqlCommand(queryemail, conexao);
                comandochecar.Parameters.AddWithValue("@email", textBox3.Text);
                int count = Convert.ToInt32(comandochecar.ExecuteScalar());
                // Se o e-mail já foi utilizado, exibe uma mensagem
                if (count > 0)
                {
                    MessageBox.Show("E-mail já utilizado.");
                }
                else
                {
                    // Verifica se a senha tem pelo menos 8 caracteres
                    if (senha.Length < 8)
                    {
                        MessageBox.Show("Digite pelo menos 8 caracteres para a senha.");
                    }
                    else
                    {
                        // Gera o hash da senha
                        //string salt = gerarsalt(64);
                        string hash = GerarHash(senha);
                        // Insere os dados no banco de dados
                        string queryInsert = "INSERT into login (nome, email, senha) VALUES (@nome, @email, @senha)";
                        MySqlCommand commandInsert = new MySqlCommand(queryInsert, conexao);
                        commandInsert.Parameters.AddWithValue("@nome", textBox1.Text);
                        commandInsert.Parameters.AddWithValue("@email", textBox3.Text);
                        commandInsert.Parameters.AddWithValue("@senha", hash);

                        commandInsert.ExecuteNonQuery();
                        // Exibe mensagem de conta criada e abre o formulário de início
                        Frminicio form = new Frminicio();
                        form.Show();
                        this.Hide();
                        MessageBox.Show("Conta criada");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
        //metodo para cripritografar as senhas com salt
        /*public string gerarsalt(int size)
        {
            /*using (var crypto = new RNGCryptoServiceProvider())
            {
                var buff = new byte[size];
                crypto.GetBytes(buff);

                return Convert.ToBase64String(buff);
            }
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            string saltBase64 = Convert.ToBase64String(salt);
            // agora é possivel armazenar o salt no banco
            return saltBase64;
        }
        /*
        //metodo para gerar hash com salt
        /*public string gerarhash(string senha, string salt)
        {

            // Convertendo o salt de Base64 para bytes
            byte[] salt2 = Convert.FromBase64String(salt);

            using (SHA256 sha256 = SHA256.Create())
            {
                // Concatenar a senha com o salt antes de calcular o hash
                byte[] saltedPassword = Encoding.UTF8.GetBytes(senha).Concat(salt2).ToArray();

                // Calcular o hash da senha com o salt
                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                // Converte os bytes do hash para uma string hexadecimal
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    stringBuilder.Append(hashedBytes[i].ToString("X2"));
                }

                return stringBuilder.ToString();
            }
        }
        */
        public string GerarHash(string senha)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convertendo a senha para bytes antes de calcular o hash
                byte[] passwordBytes = Encoding.UTF8.GetBytes(senha);

                // Calcular o hash da senha
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

                // Converte os bytes do hash para uma string hexadecimal
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    stringBuilder.Append(hashedBytes[i].ToString("X2"));
                }

                return stringBuilder.ToString();
            }
        }
        // Evento chamado ao pressionar Enter no campo de nome
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                textBox3.Focus();
                e.Handled = true;
            }
        }
        // Evento chamado ao pressionar Enter no campo de e-mail
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                textBox2.Focus();
                e.Handled = true;
            }
        }
    }
}
