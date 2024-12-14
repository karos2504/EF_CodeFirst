using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EF.Models;

namespace EF
{
    public partial class frmStudentManagement : Form
    {
        StudentContextDB context = new StudentContextDB();

        public frmStudentManagement()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            {
                try
                {
                    StudentContextDB context = new StudentContextDB();
                    List<Faculty> listFalcultys = context.Faculties.ToList();
                    List<Student> listStudent = context.Students.ToList();
                    FillFalcultyCombobox(listFalcultys);
                    BindGrid(listStudent);
                }
                catch (Exception ex)
                { 
                    MessageBox.Show(ex.Message); 
                }
            }
        }

        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore;

            }
        }

        private void ResetInputs()
        {
            txtStudentID.Clear();
            txtFullName.Clear();
            txtAverageScore.Clear();
            cmbFaculty.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtStudentID.Text.Length != 10)
            {
                MessageBox.Show("Mã số sinh viên phải có 10 kí tự!");
                return;
            }

            try
            {
                Student newStudent = new Student
                {
                    StudentID = txtStudentID.Text,
                    FullName = txtFullName.Text,
                    FacultyID = (int)cmbFaculty.SelectedValue,
                    AverageScore = float.Parse(txtAverageScore.Text)
                };

                context.Students.Add(newStudent);
                context.SaveChanges();
                MessageBox.Show("Thêm mới dữ liệu thành công!");

                ResetInputs();
                BindGrid(context.Students.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var student = context.Students.FirstOrDefault(s => s.StudentID == txtStudentID.Text);
            if (student == null)
            {
                MessageBox.Show("Không tìm thấy MSSV cần sửa!");
                return;
            }

            try
            {
                student.FullName = txtFullName.Text;
                student.FacultyID = (int)cmbFaculty.SelectedValue;
                student.AverageScore = float.Parse(txtAverageScore.Text);

                context.SaveChanges();
                MessageBox.Show("Cập nhật dữ liệu thành công!");
                
                ResetInputs();
                BindGrid(context.Students.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudent.Rows[e.RowIndex];

                txtStudentID.Text = row.Cells[0].Value?.ToString();
                txtFullName.Text = row.Cells[1].Value?.ToString();
                cmbFaculty.Text = row.Cells[2].Value?.ToString();
                txtAverageScore.Text = row.Cells[3].Value?.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var student = context.Students.FirstOrDefault(s => s.StudentID == txtStudentID.Text);
            if (student == null)
            {
                MessageBox.Show("Không tìm thấy MSSV cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    context.Students.Remove(student);
                    context.SaveChanges();
                    MessageBox.Show("Xóa sinh viên thành công!");

                    ResetInputs();  
                    BindGrid(context.Students.ToList());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }
    }
}