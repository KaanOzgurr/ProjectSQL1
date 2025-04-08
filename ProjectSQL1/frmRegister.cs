using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ProjectSQL1
{
    public partial class frmRegister : Form
    {
        private string connectionString = "Host=localhost;Port=5432;Database=EmployeeManagement;Username=postgres;Password=sifre;";
        public frmRegister()
        {
            InitializeComponent();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtConfirmPassword.Text == txtConfirmPassword.Text)
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string sql = "INSERT INTO Users (Username, Password) VALUES (@username, crypt(@password, gen_salt('bf')));";
                        using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@username", txtUsername.Text);
                            command.Parameters.AddWithValue("@password", txtConfirmPassword.Text);

                            command.ExecuteNonQuery();

                            MessageBox.Show("Kayıt başarılı.");

                            frmLogin loginForm = new frmLogin();
                            loginForm.Show();
                            this.Hide();
                        }

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanı bağlantısı başarısız: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Şifreler eşleşmiyor.");
            }
        }
    }
}
