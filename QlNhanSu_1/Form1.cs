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

namespace QlNhanSu_1
{
    public partial class frmNhanVien : Form
    {
        string st = @"Data Source=CANH-DHQN\SQLEXPRESS;Initial Catalog=QLNhanSu;Integrated Security=True";
        SqlConnection cn;
        SqlDataAdapter da;
        DataSet ds;
        SqlCommandBuilder builder;
        public frmNhanVien()
        {
            InitializeComponent();

            Load += new EventHandler(Form_Load);//Sự kiện Load của Form
            btnThem.Click += new EventHandler(Them);
            btnSua.Click += new EventHandler(Sua);
            btnXoa.Click += new EventHandler(Xoa);

            btnLamMoi.Click += new EventHandler(LamMoi);
            btnThongKe.Click += new EventHandler(ThongKePhongBan);
            txtTimKiem.Click += new EventHandler(TenNV_Click);
            btnTimKiem.Click += new EventHandler(TimKiem);
            dataGridView.CellClick += new DataGridViewCellEventHandler(Data_Click);
        }

        private void Data_Click(object sender, EventArgs e)
        {
            int i = dataGridView.CurrentCell.RowIndex;
            txtMaNV.Text = dataGridView.Rows[i].Cells[0].Value.ToString();
            txtHoTen.Text = dataGridView.Rows[i].Cells[1].Value.ToString();
            dtpNgaySinh.Text = dataGridView.Rows[i].Cells[2].Value.ToString();
            string gt = dataGridView.Rows[i].Cells[3].Value.ToString();
            if (gt.Equals("True"))
                rdNam.Checked = true;
            else
                rdNu.Checked = true;
            
            txtSoDT.Text = dataGridView.Rows[i].Cells[4].Value.ToString();
            txtHSL.Text = dataGridView.Rows[i].Cells[5].Value.ToString();
            cboTenPhong.SelectedValue = dataGridView.Rows[i].Cells[6].Value.ToString();
            cboChucVu.SelectedValue = dataGridView.Rows[i].Cells[7].Value.ToString();


        }


        #region Load dữ liệu
        //Phương thức load dữ liệu lên ô Combobox Phòng ban
        public void loadPB()
        {
            string sql = "SELECT * FROM DMPHONG";
            //Khởi tạo DataAdapter
            da = new SqlDataAdapter(sql, cn);
            //Đổ dữ liệu lên Dataset
            da.Fill(ds, "PhongBan");

            //Lấy dữ liệu từ Dataset đổ lên Combobox
            cboTenPhong.DataSource = ds.Tables["PhongBan"];
            cboTenPhong.DisplayMember = "TenPhong";
            cboTenPhong.ValueMember = "MaPhong";
            da.Dispose();
        }
        //Phương thức load dữ liệu lên ô Combobox Chức vụ
        public void loadChucVu()
        {
            string sql = "SELECT * FROM CHUCVU ";
            //Khởi tạo DataAdapter
            da = new SqlDataAdapter(sql, cn);
            //Đổ dữ liệu lên Dataset
            da.Fill(ds, "ChucVu");

            //Lấy dữ liệu từ Dataset đổ lên Combobox
            cboChucVu.DataSource = ds.Tables["ChucVu"];
            cboChucVu.DisplayMember = "TenChucVu";
            cboChucVu.ValueMember = "MaChucVu";
            da.Dispose();
        }
        //Phương thức load dữ liệu lên DataGridView danh sách nv
        public void loadDSNV()
        {
            string sql = "SELECT * FROM DSNV";
            //Khởi tạo DataAdapter
            da = new SqlDataAdapter(sql, cn);
            //Đổ dữ liệu lên Dataset
            da.Fill(ds, "NhanVien");

            //Lấy dữ liệu từ Dataset đổ lên DataGridView
            dataGridView.DataSource = ds.Tables["NhanVien"];
            txtTongNV.Text = (dataGridView.Rows.Count-1).ToString();
        }
        #endregion

        #region xác định tính hợp lệ của dữ liệu
        public bool isNumber(string value)
        {
            bool ktra;
            float result;
            ktra = float.TryParse(value, out result);
            return ktra;                
        }

        //Kiểm tra sự trùng lặp của khóa chính
        public bool KtraMaNV(string ma)
        {
            bool kt = false;
            DataTable dt = ds.Tables["NhanVien"];
            foreach (DataRow r in dt.Rows)
                if (r[0].Equals(ma))
                {
                    kt = true;
                    break;
                }
            return kt;     
        }
        
        #endregion
        private void Form_Load(object sender, EventArgs e)
        {
            cn = new SqlConnection(st);
            ds = new DataSet();
            loadChucVu();
            loadPB();
            loadDSNV();
            builder = new SqlCommandBuilder(da);
        }

        private void TimKiem(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT * FROM DSNV WHERE HoTen Like N'%{0}'",txtTimKiem.Text.Trim());
            da = new SqlDataAdapter(sql, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView.DataSource = dt;
        }

        private void TenNV_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
        }

        private void ThongKePhongBan(object sender, EventArgs e)
        {
            string sql = @"SELECT B.TenPhong, count(A.MaPhong) as SoLuong  " +
                    "FROM DSNV as A, DMPHONG as B " +
                    "WHERE A.MaPhong = B.MaPhong " +
                    "GROUP BY B.TenPhong";
            da = new SqlDataAdapter(sql, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView.DataSource = dt;
        }

        private void LamMoi(object sender, EventArgs e)
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtHoTen.Focus();
            dtpNgaySinh.Text = DateTime.Now.ToString();
            if (rdNam.Checked == false)
                rdNam.Checked = true;
            txtHSL.Clear();
            txtSoDT.Clear();
            cboTenPhong.SelectedIndex = 0;
            cboChucVu.SelectedIndex = 0;

            dataGridView.DataSource = ds.Tables["NhanVien"];
        }

        //Xóa 1 nhân viên được chọn trên DataGridView
        private void Xoa(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                int i = dataGridView.CurrentCell.RowIndex;//Chỉ số dòng được chọn
                try
                {
                    DataTable dt = ds.Tables["NhanVien"];
                    //Xóa trên DataSet
                    dt.Rows[i].Delete();
                    //Cập nhật từ Dataset xuống Database
                    da.Update(ds, "NhanVien");
                    MessageBox.Show("Xóa thành công");
                } catch (Exception ex)
                {
                    MessageBox.Show("Xóa không thành công!");
                }
                   
            }    
        }

        private void Sua(object sender, EventArgs e)
        {
                DataTable dt = ds.Tables["NhanVien"];
                int ma = int.Parse(txtMaNV.Text);
                string dk =string.Format("MaNV='{0}'",ma);
                DataRow [] rows = dt.Select(dk);
                foreach (DataRow r in rows)
                {
                    r[1] = txtHoTen.Text;
                    r[2] = dtpNgaySinh.Value.ToString();
                    r[3] = rdNam.Checked == true ? true : false;
                    r[4] = txtSoDT.Text;
                    r[5] = Math.Round(float.Parse(txtHSL.Text), 2);
                    r[6] = cboTenPhong.SelectedValue.ToString();
                    r[7] = cboChucVu.SelectedValue.ToString();
                    //Cập nhật dữ liệu từ Dataset xuống Database
                    da.Update(ds, "NhanVien");
                }    
                MessageBox.Show("Sửa thành công!");
                
        }

        private void Them(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(txtHoTen.Text)) && (isNumber(txtHSL.Text)))
            {
                //Thêm trên Dataset
                DataTable dt = ds.Tables["NhanVien"];
                DataRow r = dt.NewRow(); //Thêm 1 dòng trống
                r[1] = txtHoTen.Text;
                r[2] = dtpNgaySinh.Value.ToString();
                r[3] = rdNam.Checked == true ? true : false;
                r[4] = txtSoDT.Text;
                r[5] = Math.Round(float.Parse(txtHSL.Text),2);
                r[6] = cboTenPhong.SelectedValue.ToString();
                r[7] = cboChucVu.SelectedValue.ToString();
                dt.Rows.Add(r);

                //Cập nhật dữ liệu từ Dataset xuống Database
                da.Update(ds, "NhanVien");
                txtTongNV.Text = dt.Rows.Count.ToString();
                MessageBox.Show("Thêm thành công!");
            }
            else
                MessageBox.Show("Thêm chưa thành công!");
        }

       
        
    }
}
