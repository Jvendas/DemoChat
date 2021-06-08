using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace DemoChat
{
    public partial class FormLogin : Form
    {
        //Varivavel dos formulario chat
        private FormChat chatForm;
        private const int SALTSIZE = 8;
        private const int NUMBER_OF_ITERATIONS = 50000;


        public FormLogin()
        {
            InitializeComponent();
            MessageBox.Show("Hello, I'm Doc.Octopus and this is DemoChat. Welcome!");
        }

               private bool VerifyLogin(string username, string password)
        {
            SqlConnection conn = null;
            try
            {
                // Configurar ligação à Base de Dados 
                conn = new SqlConnection();
                conn.ConnectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\Karol\Documents\TESP_PSI\1ano\2semestre\topicosSeguranca\trabalhoGrupo\DemoChat\ChatServer\Database1.mdf';Integrated Security=True");
                //conn.ConnectionString = Properties.Settings.Default.connectionString;

                // Abrir ligação à Base de Dados
                conn.Open();

                // Declaração do comando SQL
                String sql = "SELECT * FROM Users WHERE Username = @username";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;

                // Declaração dos parâmetros do comando SQL
                SqlParameter param = new SqlParameter("@username", username);

                // Introduzir valor ao parâmentro registado no comando SQL
                cmd.Parameters.Add(param);

                // Associar ligação à Base de Dados ao comando a ser executado
                cmd.Connection = conn;

                // Executar comando SQL
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("Erro de acesso ao utilizador");
                }

                // Ler resultado da pesquisa
                reader.Read();

                // Obter Hash (password + salt)
                byte[] saltedPasswordHashStored = (byte[])reader["SaltedPasswordHash"];

                // Obter salt
                byte[] saltStored = (byte[])reader["Salt"];

                conn.Close();

                //byte[] pass = Encoding.UTF8.GetBytes(password);

                byte[] hash = GenerateSaltedHash(password, saltStored);

                return saltedPasswordHashStored.SequenceEqual(hash);
            }
            catch
            {
                MessageBox.Show("Ocorreu um erro");
                return false;
            }
        }

        private void Register(string username, byte[] saltedPasswordHash, byte[] salt)
        {
            SqlConnection conn = null;
            try
            {

                // Configurar ligação à Base de Dados
                conn = new SqlConnection();
                conn.ConnectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\Karol\Documents\TESP_PSI\1ano\2semestre\topicosSeguranca\trabalhoGrupo\DemoChat\ChatServer\Database1.mdf';Integrated Security=True");
                //conn.ConnectionString = Properties.Settings.Default.connectionString;

                // Abrir ligação à Base de Dados
                conn.Open();

                // Declaração dos parâmetros do comando SQL
                SqlParameter paramUsername = new SqlParameter("@username", username);
                SqlParameter paramPassHash = new SqlParameter("@saltedPasswordHash", saltedPasswordHash);
                SqlParameter paramSalt = new SqlParameter("@salt", salt);

                // Declaração do comando SQL
                String sql = "INSERT INTO Users (Username, SaltedPasswordHash, Salt) VALUES (@username,@saltedPasswordHash,@salt)";

                // Prepara comando SQL para ser executado na Base de Dados
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Introduzir valores aos parâmentros registados no comando SQL
                cmd.Parameters.Add(paramUsername);
                cmd.Parameters.Add(paramPassHash);
                cmd.Parameters.Add(paramSalt);

                // Executar comando SQL
                int lines = cmd.ExecuteNonQuery();

                // Fechar ligação
                conn.Close();
                if (lines == 0)
                {
                    // Se forem devolvidas 0 linhas alteradas então o não foi executado com sucesso
                    throw new Exception("Erro ao inserir o utilizador");
                }
                MessageBox.Show("Registado com sucesso!");
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao inserir o utilizador:" + e.Message);
            }
        }

                private static byte[] GenerateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return buff;
        }

        private static byte[] GenerateSaltedHash(string plainText, byte[] salt)
        {
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(plainText, salt, NUMBER_OF_ITERATIONS);
            return rfc2898.GetBytes(32);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            String pass = tbPassword.Text;

            string username = tbUserName.Text;

            byte[] salt = GenerateSalt(SALTSIZE);

            byte[] hash = GenerateSaltedHash(pass, salt);

            Register(username, hash, salt);
        }

        private void btnLogin_Click(object sender, EventArgs e)
            
        {

            //Ao fazer login abre o janela do chat e esconde a janela do login
            chatForm = new FormChat(this);
            chatForm.Show();
            this.Hide();
       
            String password = tbPassword.Text;
            String username = tbUserName.Text;

            if (VerifyLogin(username, password))
            {
                MessageBox.Show("Utilizador Valido");
            }
            else
            {
                MessageBox.Show("Utilizador Invalido");
            }


        }

        private void btnGenerateSaltedHash_Click(object sender, EventArgs e)
        {
            String password = tbPassword.Text;

            byte[] salt = GenerateSalt(SALTSIZE);
            byte[] hash = GenerateSaltedHash(password, salt);

            tbSaltedHash.Text = Convert.ToBase64String(hash);
            tbSalt.Text = Convert.ToBase64String(salt);

            tbSizePass.Text = (hash.Length * 8).ToString();
            tbSizeSalt.Text = (salt.Length * 8).ToString();
        } 

         private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
    }
        }