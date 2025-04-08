using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;


namespace ProjectSQL1
{
    public partial class frmLogin : Form
    {
        private string connectionString = "Host=localhost;Port=5432;Database=EmployeeManagement;Username=postgres;Password=sifre;";

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT Password FROM Users WHERE Username = @username;";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", txtUsername.Text);

                        string storedHashedPassword = (string)command.ExecuteScalar();

                        if (storedHashedPassword != null && BCrypt.Net.BCrypt.Verify(txtPassword.Text, storedHashedPassword))
                        {
                            
                            frmMain mainForm = new frmMain();
                            mainForm.ShowDialog();
                            this.Hide();
                        }
                        else
                        {
                            
                            MessageBox.Show("Kullanıcı adı veya şifre yanlış.");
                        }
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanı bağlantısı başarısız: " + ex.Message);
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                frmRegister registerForm = new frmRegister();
                registerForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt formu açılırken bir hata oluştu: " + ex.Message);
            }
        }
    }
}
