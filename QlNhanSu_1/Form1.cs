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
        public frmNhanVien()
        {
            InitializeComponent();

            Load += new EventHandler(Form_Load);//Sự kiện Load của Form
            btnThem.Click += new EventHandler(Them);
            btnSua.Click += new EventHandler(Sua);
            btnXoa.Click += new EventHandler(Xoa);

            btnLamMoi.Click += new EventHandler(LamMoi);
            btnThongKe.Click += new EventHandler(ThongKePhongBan);
            txtTimKiem.Click += new EventHandler(TenNV);
            btnTimKiem.Click += new EventHandler(TimKiem);
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
        }
        #endregion

        private void Form_Load(object sender, EventArgs e)
        {
            cn = new SqlConnection(st);
            ds = new DataSet();
            loadChucVu();
            loadPB();
            loadDSNV();
        }

        private void TimKiem(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT * FROM DSNV WHERE HoTen Like N'%{0}'",txtTimKiem.Text.Trim());
            da = new SqlDataAdapter(sql, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView.DataSource = dt;
        }

        private void TenNV(object sender, EventArgs e)
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

        private void Xoa(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Sua(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Them(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

       
        
    }
}
