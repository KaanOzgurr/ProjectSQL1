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
using System.Xml.Linq;

namespace ProjectSQL1
{
    public partial class frmEmployee : Form
    {
        private string connectionString = "Host=localhost;Port=5432;Database=EmployeeManagement;Username=postgres;Password=sifre;"; 
        private int? selectedEmployeeIdForEdit = null; 
        private bool isEditMode = false; 

        public frmEmployee()
        {
            InitializeComponent();
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT employeeid, name, position, salary FROM employees;";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            dgvEmployees.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvEmployees.Rows.Add(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetFloat(3));
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Çalışan verileri yüklenirken bir hata oluştu: " + ex.Message);
                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string position = txtPosition.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(position) || string.IsNullOrEmpty(txtSalary.Text.Trim()))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!float.TryParse(txtSalary.Text.Trim(), out float salary))
            {
                MessageBox.Show("Geçersiz maaş formatı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "INSERT INTO employees (name, position, salary) VALUES (@name, @position, @salary)";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@position", position);
                        command.Parameters.AddWithValue("@salary", salary);

                        int affectedRows = command.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Çalışan başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEmployeeData();
                            ClearInputFields(); 
                        }
                        else
                        {
                            MessageBox.Show("Çalışan eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                if (dgvEmployees.SelectedRows[0].Cells["employeeid"].Value != null)
                {
                    int employeeId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["employeeid"].Value);

                    if (!isEditMode)
                    {
                        
                        selectedEmployeeIdForEdit = employeeId;
                        txtName.Text = dgvEmployees.SelectedRows[0].Cells["name"].Value.ToString();
                        txtPosition.Text = dgvEmployees.SelectedRows[0].Cells["position"].Value.ToString();
                        txtSalary.Text = dgvEmployees.SelectedRows[0].Cells["salary"].Value.ToString();

                        isEditMode = true;
                        btnEdit.Text = "Kaydet";
                        btnAdd.Enabled = false; 
                        btnDelete.Enabled = false; 
                    }
                    else
                    {
                        
                        string name = txtName.Text.Trim();
                        string position = txtPosition.Text.Trim();

                        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(position) || string.IsNullOrEmpty(txtSalary.Text.Trim()))
                        {
                            MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (!float.TryParse(txtSalary.Text.Trim(), out float salary))
                        {
                            MessageBox.Show("Geçersiz maaş formatı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                string sql = "UPDATE employees SET name = @name, position = @position, salary = @salary WHERE employeeid = @employeeid";
                                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                                {
                                    command.Parameters.AddWithValue("@name", name);
                                    command.Parameters.AddWithValue("@position", position);
                                    command.Parameters.AddWithValue("@salary", salary);
                                    command.Parameters.AddWithValue("@employeeid", selectedEmployeeIdForEdit.Value);

                                    int affectedRows = command.ExecuteNonQuery();

                                    if (affectedRows > 0)
                                    {
                                        MessageBox.Show("Çalışan bilgileri başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        LoadEmployeeData(); 
                                        ClearInputFields(); 
                                        isEditMode = false; 
                                        btnEdit.Text = "Düzenle";
                                        btnAdd.Enabled = true; 
                                        btnDelete.Enabled = true; 
                                        selectedEmployeeIdForEdit = null;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Çalışan bilgileri güncellenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Düzenlemek için geçerli bir çalışan seçilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek için bir çalışan seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                if (dgvEmployees.SelectedRows[0].Cells["employeeid"].Value != null)
                {
                    int employeeId = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["employeeid"].Value);

                    DialogResult result = MessageBox.Show("Seçili çalışanı silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                string sql = "DELETE FROM employees WHERE employeeid = @employeeid";
                                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                                {
                                    command.Parameters.AddWithValue("@employeeid", employeeId);

                                    int affectedRows = command.ExecuteNonQuery();

                                    if (affectedRows > 0)
                                    {
                                        MessageBox.Show("Çalışan başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        LoadEmployeeData(); 
                                        ClearInputFields(); 
                                    }
                                    else
                                    {
                                        MessageBox.Show("Çalışan silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Silmek için geçerli bir çalışan seçilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir çalışan seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearInputFields()
        {
            txtName.Clear();
            txtPosition.Clear();
            txtSalary.Clear();
            isEditMode = false; 
            btnEdit.Text = "Düzenle"; 
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            selectedEmployeeIdForEdit = null;
        }
    }
}

