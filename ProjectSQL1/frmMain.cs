// frmMain.cs dosyanızın tam içeriği (ProjectSQL1 projesi için)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ProjectSQL1 
{
    public partial class frmMain : Form 
    {
        
        private string loggedInUsername;
        private string userRole; 

        
        public frmMain(string username, string role)
        {
            InitializeComponent(); 

          
            this.loggedInUsername = username;
            this.userRole = role;

            
            lblWelcome.Text = "Hoş Geldiniz, " + loggedInUsername + "!"; // Veya "Welcome, " + loggedInUsername + "!";

           
        }

      
        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }

        
        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            
            frmEmployee employeeForm = new frmEmployee();
            employeeForm.Show();

            
        }

        
        private void btnLogout_Click(object sender, EventArgs e)
        {
            

            
            this.Hide();
           
            frmLogin loginForm = new frmLogin();
           
            loginForm.Show();

            
        }

      
    }
}
