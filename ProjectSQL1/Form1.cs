// frmLogin.cs dosyanızın tam içeriği (ProjectSQL1 projesi için)

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
    public partial class frmLogin : Form  
    {
        
        public frmLogin()
        {
            InitializeComponent(); 
        }

        
        private void frmLogin_Load(object sender, EventArgs e)
        {
            
        }

        
        private void btnLogin_Click(object sender, EventArgs e)
        {
          

            
            string username = txtUsername.Text.Trim(); 
            string password = txtPassword.Text;       

           
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifreyi girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

          

           
            string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDBConnection"].ConnectionString;

            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                string selectQuery = "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password); 

                    try
                    {
                       
                        connection.Open();

                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            if (reader.Read()) 
                            {
                                
                                MessageBox.Show("Giriş başarılı!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                
                                string role = reader["Role"].ToString(); 

                               
                                this.Hide();
                                // Ana formu oluştur ve göster
                                // frmMain mainForm = new frmMain(username, role); // Ana forma kullanıcı adı ve rolü gönderebilirsiniz
                                // mainForm.Show();

                                
                            }
                            else
                            {
                                
                                MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                
                                txtPassword.Clear();
                            }
                        } 

                    }
                    catch (Exception ex)
                    {
                        
                        MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } 
            } 

        }

        
        private void btnRegister_Click(object sender, EventArgs e)
        {
            
             
        }
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            frmRegister registerForm = new frmRegister();
            registerForm.Show();
        }
    }
}
