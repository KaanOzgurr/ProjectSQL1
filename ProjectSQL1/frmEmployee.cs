// frmEmployee.cs dosyanızın tam içeriği (ProjectSQL1 projesi için)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // SQL Server bağlantısı ve komutları için GEREKLİ
using System.Configuration; // App.config'ten bağlantı dizesini okumak için GEREKLİ

// Eğer App.config bağlantısını farklı bir sınıfta yönetiyorsanız, o sınıfın namespace'ini buraya ekleyin.
// Örneğin: using ProjectSQL1.DatabaseHelper;

namespace ProjectSQL1 // << Projenizin namespace'i (ProjectSQL1)
{
    public partial class frmEmployee : Form // << Form sınıfı tanımı
    {
        // Constructor: Form ilk oluşturulduğunda çalışır
        public frmEmployee()
        {
            InitializeComponent(); // Formun görsel kontrollerini ayarlar, OLMALI.
        }

        // frmEmployee formu yüklendiğinde çalışan olay metodu
        // Form açıldığında çalışan listesini otomatik olarak yüklemek için burayı kullanırız.
        private void frmEmployee_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde çalışanları DataGridView'e yükle
            LoadEmployeeData(); // Bu metot aşağıda tanımlanmıştır.
        }

        // Veritabanındaki çalışanları DataGridView'e yükleyen metot
        private void LoadEmployeeData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDBConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SELECT komutu ile tüm çalışanları getir
                // EmployeeID, Name, Position, Salary sütunlarını seçiyoruz
                string selectQuery = "SELECT EmployeeID, Name, Position, Salary FROM Employees";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    try
                    {
                        connection.Open(); // Bağlantıyı aç
                        DataTable dataTable = new DataTable(); // Verileri tutmak için DataTable oluştur
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command); // Veri adaptörünü oluştur
                        dataAdapter.Fill(dataTable); // Verileri DataTable'a doldur

                        // DataGridView'in kaynağını doldurulmuş DataTable olarak ayarla
                        // Bu, DataGridView'in verileri göstermesini sağlar.
                        dgvEmployees.DataSource = dataTable;

                        // DataGridView sütun başlıklarını düzenleme (isteğe bağlı ama iyi uygulama)
                        // Eğer veritabanı sütun adları farklıysa veya başlıkları değiştirmek isterseniz.
                        // dgvEmployees.Columns["EmployeeID"].HeaderText = "ID";
                        // dgvEmployees.Columns["Name"].HeaderText = "Ad Soyad";
                        // dgvEmployees.Columns["Position"].HeaderText = "Pozisyon";
                        // dgvEmployees.Columns["Salary"].HeaderText = "Maaş";
                        // dgvEmployees.Columns["EmployeeID"].Visible = false; // Eğer ID sütununu gizlemek isterseniz

                    }
                    catch (Exception ex)
                    {
                        // Veritabanı hatası durumunda
                        MessageBox.Show("Çalışanlar yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } 
            } 
        } 


        // Çalışan Ekle butonunun Click olayı
        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            string name = txtName.Text.Trim();
            string position = txtPosition.Text.Trim();
            string salaryText = txtSalary.Text.Trim();

            // Temel Doğrulama
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(position) || string.IsNullOrEmpty(salaryText))
            {
                MessageBox.Show("Lütfen tüm çalışan bilgilerini doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            // Maaş alanının sayısal olup olmadığını kontrol et
            // TryParse metodu, dönüşüm başarılı olursa true döner ve değeri 'salary' değişkenine atar.
            if (!float.TryParse(salaryText, out float salary))
            {
                MessageBox.Show("Lütfen maaş için geçerli bir sayı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSalary.Clear(); // Maaş alanını temizle
                txtSalary.Focus(); // Maaş alanına odaklan
                return; // Geçersiz maaşsa işlemi durdur
            }

            // === Veritabanına Ekleme İşlemi ===

            string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDBConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                string insertQuery = "INSERT INTO Employees (Name, Position, Salary) VALUES (@Name, @Position, @Salary)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Parametre değerlerini ata
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Position", position);
                    command.Parameters.AddWithValue("@Salary", salary); // Float değişkenini parametre olarak ekle

                    try
                    {
                        connection.Open(); // Bağlantıyı aç
                        int rowsAffected = command.ExecuteNonQuery(); 

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Çalışan başarıyla eklendi.", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            

                            
                            txtName.Clear();
                            txtPosition.Clear();
                            txtSalary.Clear();

                            
                            LoadEmployeeData(); 
                        }
                        else
                        {
                           
                            MessageBox.Show("Çalışan eklenirken beklenmeyen bir durum oluştu.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } 
            } 
        } 


      
        private void btnEdit_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("Düzenle butonu tıklandı. Düzenleme mantığı henüz eklenmedi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            
        }


        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("Sil butonu tıklandı. Silme mantığı henüz eklenmedi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

           
        }


        // DataGridView'de bir hücreye tıklandığında (isteğe bağlı olarak düzenleme için kullanılabilir)
        // private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        // {
        //     // Eğer düzenleme için DataGridView satırına tıklama özelliğini implement edecekseniz
        //     // Seçili satırdaki veriyi TextBox'lara doldurma mantığı buraya gelebilir.
        //     if (e.RowIndex >= 0) // Başlık satırına tıklanmadıysa
        //     {
        //         DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];
        //         // TextBox'ları doldur
        //         // txtName.Text = row.Cells["Name"].Value.ToString();
        //         // txtPosition.Text = row.Cells["Position"].Value.ToString();
        //         // txtSalary.Text = row.Cells["Salary"].Value.ToString();
        //         // Seçili çalışanın EmployeeID'sini bir değişkende saklayın (güncelleme/silme için gerekli olacak)
        //         // int selectedEmployeeId = Convert.ToInt32(row.Cells["EmployeeID"].Value);
        //     }
        // }


        
    } 
} 

