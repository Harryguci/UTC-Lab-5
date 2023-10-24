using System.Data;

namespace UTC_LAB_5
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(Local);Initial Catalog=MonThi;User ID=sa;Password=123456");
        SqlCommand cmd;
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }
        void Load_Data()
        {
            cmd = con.CreateCommand();
            cmd.CommandText = "select * from monhoc";
            da.SelectCommand = cmd;
            dt.Clear();
            da.Fill(dt);
            dgv.DataSource = dt;
            dgv.Columns["mamon"].HeaderText = "Mã môn";
            dgv.Columns["mamon"].Width = 100;
            dgv.Columns["tenmon"].HeaderText = "Tên môn";
            dgv.Columns["tenmon"].Width = 100;
            dgv.Columns["sotin"].HeaderText = "Số tín chỉ";
            dgv.Columns["sotin"].Width = 100;
            dgv.Columns["diemthi"].HeaderText = "Điểm thi";
            dgv.Columns["diemthi"].Width = 100;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            con.Open();
            Load_Data();
            con.Close();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtDiemThi.Text = string.Empty;
            txtMaMon.Text = string.Empty;
            txtSoTin.Text = string.Empty;
            txtTenMon.Text = string.Empty;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaMon.Text != string.Empty && txtTenMon.Text != string.Empty
                && txtSoTin.Text != string.Empty && txtDiemThi.Text != string.Empty)
            {
                con.Open();
                using (SqlCommand command = con.CreateCommand())
                {
                    int sotin = Convert.ToInt32(txtSoTin.Text);
                    double diemthi = Convert.ToDouble(txtDiemThi.Text);

                    command.CommandText = "SELECT COUNT(*) FROM monhoc WHERE mamon = @mamon";
                    command.Parameters.AddWithValue("@mamon", txtMaMon.Text);
                    int duplicateCount = (int)command.ExecuteScalar();

                    if (duplicateCount > 0)
                    {
                        MessageBox.Show("Mã môn đã tồn tại. Vui lòng chọn mã môn khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO monhoc (mamon, tenmon, sotin, diemthi) VALUES (@mamon, @tenmon, @sotin, @diemthi)";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@mamon", txtMaMon.Text);
                        command.Parameters.AddWithValue("@tenmon", txtTenMon.Text);
                        command.Parameters.AddWithValue("@sotin", sotin);
                        command.Parameters.AddWithValue("@diemthi", diemthi);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Thêm dữ liệu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Load_Data();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng điền đủ các trường thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            con.Close();
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaMon.Text != string.Empty && txtTenMon.Text != string.Empty
                && txtSoTin.Text != string.Empty && txtDiemThi.Text != string.Empty)
            {
                if (MessageBox.Show("Bạn có chắc chắn xoá?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cmd = con.CreateCommand();
                    int mamon = Convert.ToInt32(txtMaMon.Text);
                    cmd.CommandText = "DELETE FROM monhoc WHERE mamon = @mamon";
                    cmd.Parameters.AddWithValue("@mamon", mamon);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Xoá dữ liệu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Load_Data();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn các trường thông tin muốn xoá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgv.Rows[e.RowIndex];
                txtMaMon.Text = selectedRow.Cells[0].Value.ToString();
                txtTenMon.Text = selectedRow.Cells[1].Value.ToString();
                txtSoTin.Text = selectedRow.Cells[2].Value.ToString();
                txtDiemThi.Text = selectedRow.Cells[3].Value.ToString();
            }
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            con.Open();
            using (SqlCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT SUM(diemthi * sotin) FROM monhoc";
                double TongDiem = Convert.ToDouble(command.ExecuteScalar());

                command.CommandText = "SELECT SUM(sotin) FROM monhoc";
                int TongSoTin = Convert.ToInt32(command.ExecuteScalar());

                double DTB = TongDiem / TongSoTin;

                MessageBox.Show($"Điểm trung bình: {DTB}", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            con.Close();
        }
    }
}
}