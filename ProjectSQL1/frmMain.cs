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
// using System.Data.SqlClient; // frmMain'de doğrudan veritabanı işlemi yapmıyorsak gerekli değil
// using System.Configuration; // frmMain'de doğrudan App.config'ten okuma yapmıyorsak gerekli değil

namespace ProjectSQL1 // << Projenizin namespace'i (ProjectSQL1)
{
    public partial class frmMain : Form // << Form sınıfı tanımı
    {
        // Giriş yapan kullanıcının bilgilerini saklamak için alanlar
        private string loggedInUsername;
        private string userRole; // Eğer role-based authentication implement edecekseniz rol bilgisini saklayabilirsiniz

        // Constructor: Giriş yapan kullanıcının adı ve rolü Login formundan bu constructor'a gönderilecek
        public frmMain(string username, string role)
        {
            InitializeComponent(); 

          
            this.loggedInUsername = username;
            this.userRole = role;

            
            lblWelcome.Text = "Hoş Geldiniz, " + loggedInUsername + "!"; // Veya "Welcome, " + loggedInUsername + "!";

           
        }

        // frmMain formu yüklendiğinde çalışan olay metodu (kullanılmıyorsa silebilirsiniz)
        private void frmMain_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde yapılacak ek işlemler (şimdilik boş)
            // Örneğin: Yetkilendirme kontrolü yaparak bazı butonları gizleme (constructor'da da yapılabilir).
        }

        // Çalışanları Yönet butonunun Click olayı
        // Bu buton, frmEmployee formunu açacaktır.
        // Butonun adı tasarımda 'btnManageEmployees' olmalı.
        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            // frmEmployee formunu oluştur ve göster
            frmEmployee employeeForm = new frmEmployee();
            employeeForm.Show();

            // İsteğe bağlı: frmMain formunu gizleyebilirsiniz
            // this.Hide();
        }

        // Çıkış butonunun Click olayı
        // Bu buton, uygulamadan çıkış yapmayı veya Login formuna dönmeyi sağlar.
        // Butonun adı tasarımda 'btnLogout' olmalı.
        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Uygulamadan tamamen çıkış yapma:
            // Application.Exit(); // Tüm pencereleri kapatır ve uygulamadan çıkar.

            // Login formuna geri dönme (Daha yaygın bir senaryo):
            // Önce mevcut frmMain formunu gizle
            this.Hide();
            // Yeni bir Login formu oluştur
            frmLogin loginForm = new frmLogin();
            // Login formunu göster
            loginForm.Show();

            
        }

      
    }
}
