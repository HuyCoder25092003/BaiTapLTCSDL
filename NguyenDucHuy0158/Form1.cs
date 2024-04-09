using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
namespace NguyenDucHuy0158
{
    public partial class Form1 : Form
    {
        string chuoiKN;
        SqlConnection conn;
        string maSP = "1";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chuoiKN = ConfigurationManager.ConnectionStrings["cnstr"].ConnectionString;
            conn = new SqlConnection(chuoiKN);

            gVSanPham.DataSource = LayDSSP();

            cbLoaiSP.DataSource = LayDSLSP();
            cbLoaiSP.DisplayMember = "CategoryName";
            cbLoaiSP.ValueMember = "CategoryID";

            cbNCC.DataSource = LayDNCC();
            cbNCC.DisplayMember = "CompanyName";
            cbNCC.ValueMember = "SupplierID";
        }

        private DataTable LayDSSP()
        {
            string query = "select * from Products order by ProductID desc";
            SqlDataAdapter da = new SqlDataAdapter(query,conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        private DataTable LayDSLSP()
        {
            string query = "select CategoryID, CategoryName from Categories";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        private DataTable LayDNCC()
        {
            string query = "select SupplierID, CompanyName from Suppliers";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                string query = String.Format("insert into Products(ProductName,SupplierID,CategoryID,UnitPrice,UnitsInStock)" +
                    "values(N'{0}',{1},{2},{3},{4})",
                    txtTenSP.Text, int.Parse(cbNCC.SelectedValue.ToString()),int.Parse(cbLoaiSP.SelectedValue.ToString()),
                    Convert.ToDecimal(txtDonGia.Text),int.Parse(txtSoLuong.Text));
                MessageBox.Show(query);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();

                //Update again gVSanPham
                gVSanPham.DataSource = null;
                gVSanPham.DataSource = LayDSSP();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void gVSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            maSP = gVSanPham.Rows[e.RowIndex].Cells["ProductID"].Value.ToString();
            txtTenSP.Text = gVSanPham.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
            txtSoLuong.Text = gVSanPham.Rows[e.RowIndex].Cells["UnitsInStock"].Value.ToString();
            txtDonGia.Text = gVSanPham.Rows[e.RowIndex].Cells["UnitPrice"].Value.ToString();
            cbLoaiSP.SelectedValue = gVSanPham.Rows[e.RowIndex].Cells["CategoryID"].Value.ToString();
            cbNCC.SelectedValue = gVSanPham.Rows[e.RowIndex].Cells["SupplierID"].Value.ToString();
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                string query = String.Format("update Products " + "Set ProductName=N'{0}', CategoryID={1}, UnitPrice={2}, UnitsInStock={3}, SupplierID={4}" +
                    "Where ProductID={5}", txtTenSP.Text, int.Parse(cbLoaiSP.SelectedValue.ToString()),
                    Convert.ToDecimal(txtDonGia.Text), int.Parse(txtSoLuong.Text), int.Parse(cbNCC.SelectedValue.ToString()),maSP);
                MessageBox.Show(query);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();

                //Update again gVSanPham
                gVSanPham.DataSource = null;
                gVSanPham.DataSource = LayDSSP();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int maSP1 = int.Parse(maSP);
                string query = String.Format("DELETE FROM Products Where ProductID={0}", maSP1);
                MessageBox.Show(query);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();

                //Update again gVSanPham
                gVSanPham.DataSource = null;
                gVSanPham.DataSource = LayDSSP();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
