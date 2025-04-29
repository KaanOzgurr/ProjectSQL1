// frmRegister.cs dosyanızın tam içeriği (ProjectSQL1 projesi için)
// btnRegister'a basıldığında başarılı kayıt sonrası frmMain'e Yönlendirme Yapacak Şekilde Güncellendi

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; 
using System.Configuration;


namespace ProjectSQL1
{
    public partial class frmRegister : Form
    {

        public frmRegister()
        {
            InitializeComponent();
        }


        private void frmRegister_Load(object sender, EventArgs e)
        {

        }


        private void btnRegister_Click(object sender, EventArgs e)
        {

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;


            string role = "User";


            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (password != confirmPassword)
            {
                MessageBox.Show("Girilen şifreler eşleşmiyor. Lütfen kontrol edin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDBConnection"].ConnectionString;

            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                string insertQuery = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@Username", username);

                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role); 

                    try
                    {

                        connection.Open();


                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Yeni kullanıcı başarıyla kaydedildi.", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            this.Hide();


                            frmMain mainForm = new frmMain(username, role);
                            mainForm.Show();



                        }
                        else
                        {

                            MessageBox.Show("Kullanıcı kaydedilirken beklenmeyen bir durum oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (SqlException ex)
                    {

                        if (ex.Number == 2627)
                        {
                            MessageBox.Show("Bu kullanıcı adı zaten kullanılıyor. Lütfen başka bir kullanıcı adı deneyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {

                            MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {



        }
    }
}
