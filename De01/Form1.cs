using De01.Model;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De01
{
    public partial class Form1 : Form
    {
        private DBContextSV dBContext;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dBContext = new DBContextSV();
            List<SinhVien> sinhViens = dBContext.SinhVien.ToList();
            List<Lop> lops = dBContext.Lop.ToList();
            rpvSinhVien.Visible = false;
            dgvSinhVien.Visible = true;
            FillDataToDGV(sinhViens);
            FillDataToCBB(lops);
            ResetInformation();
            this.rpvSinhVien.RefreshReport();
        }
        private void ResetInformation()
        {
            txtHoTen.Text = "";
            txtMSV.Text = "";
            cbbLop.Text = "Chọn Lớp";
            dtpNgaySinh.Value = DateTime.Now;
        }

        private void FillDataToDGV(List<SinhVien> sinhViens)
        {
            foreach (var item in sinhViens)
            {
                int i = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[i].Cells[0].Value = item.MaSV;
                dgvSinhVien.Rows[i].Cells[1].Value = item.HoTenSV;
                dgvSinhVien.Rows[i].Cells[2].Value = item.NgaySinh;
                dgvSinhVien.Rows[i].Cells[3].Value = item.Lop.TenLop;
            }
        }

        private void FillDataToCBB(List<Lop> lops)
        {
            cbbLop.DataSource = lops;
            cbbLop.DisplayMember = "TenLop";
            cbbLop.ValueMember = "MaLop";
        }
        private int GetSelectedRow(string msv)
        {
            for (int i = 0; i < dgvSinhVien.Rows.Count; i++)
            {
                if (dgvSinhVien.Rows[i].Cells[0].Value != null)
                {
                    if (dgvSinhVien.Rows[i].Cells[0].Value.ToString() == msv)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMSV.Text == "" || txtHoTen.Text == "")
                    throw new Exception("Bạn chưa nhập đủ thông tin!");

                int selectedRow = GetSelectedRow(txtMSV.Text);
                if (selectedRow == -1)
                {
                    selectedRow = dgvSinhVien.Rows.Add();
                    Insert(selectedRow);
                    MessageBox.Show("Thêm sinh viên mới thành công!", "Thông báo", MessageBoxButtons.OK);
                    ResetInformation();
                }
                else 
                {
                    MessageBox.Show("Sinh Viên Đã Tồn Tại !!!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Insert(int selectedRow)
        {
            dgvSinhVien.Rows[selectedRow].Cells[0].Value = txtMSV.Text;
            dgvSinhVien.Rows[selectedRow].Cells[1].Value = txtHoTen.Text;
            dgvSinhVien.Rows[selectedRow].Cells[2].Value = dtpNgaySinh.Value;
            dgvSinhVien.Rows[selectedRow].Cells[3].Value = cbbLop.Text;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedRow = GetSelectedRow(txtMSV.Text);
                if (selectedRow == -1)
                {
                    throw new Exception("Chương trình không tìm thấy sinh viên cần xóa!");
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Bạn có muốn xóa thông tin ?", "Thông báo", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        dgvSinhVien.Rows.RemoveAt(selectedRow);
                        MessageBox.Show("Xóa thông tin thành công!", "Thông báo", MessageBoxButtons.OK);
                        ResetInformation();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMSV.Text == "" || txtHoTen.Text == "")
                    throw new Exception("Bạn chưa nhập đủ thông tin!");

                int selectedRow = GetSelectedRow(txtMSV.Text);
                if (selectedRow != -1)
                {
                    Insert(selectedRow);
                    MessageBox.Show("Sửa Thông Tin sinh viên Thành Công!", "Thông báo", MessageBoxButtons.OK);
                    ResetInformation();
                }
                else
                {
                    MessageBox.Show("Sinh Viên Không Tồn Tại !!!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void bntThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn THOÁT Không ?", "Thông báo", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvSinhVien.Rows[e.RowIndex].Cells[e.ColumnIndex] != null)
                {
                    dgvSinhVien.CurrentRow.Selected = true;
                    txtMSV.Text = dgvSinhVien.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                    txtHoTen.Text = dgvSinhVien.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                    dtpNgaySinh.Text = dgvSinhVien.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
                    cbbLop.SelectedIndex = cbbLop.FindString(dgvSinhVien.Rows[e.RowIndex].Cells[3].FormattedValue.ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Dòng bạn chọn không đúng, vui lòng chọn lại", "Thông Báo");
            }
        }

        private void bntReport_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn Xuất Report Không ?", "Thông báo", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpvSinhVien.Visible = true;
                dgvSinhVien.Visible = false;
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;

                List<SinhVien> sinhViens = dBContext.SinhVien.ToList();

                rpvSinhVien.LocalReport.ReportPath = "./Report/R_DSSinhVien.rdlc";
                ReportDataSource reportDataSource = new ReportDataSource("DataSet_DSSinhVien", sinhViens);
                rpvSinhVien.LocalReport.DataSources.Clear();
                rpvSinhVien.LocalReport.DataSources.Add(reportDataSource);
                rpvSinhVien.RefreshReport();
            }
        }
    }
}
