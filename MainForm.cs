using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using TrelloSys;

namespace TrelloSys
{
    public partial class MainForm : Form
    {
        public MainForm() { InitializeComponent(); }

        private void MainForm_Load(object sender, EventArgs e) { LoadData(); }

        private void LoadData()
        {
            DatabaseHelper db = new DatabaseHelper();

            // 1. Lấy dữ liệu từ ô Tìm kiếm
            string searchKeyword = txtSearch.Text.Trim();
            object searchParam = string.IsNullOrEmpty(searchKeyword) ? (object)DBNull.Value : searchKeyword;

            // 2. Lấy dữ liệu từ ô Sắp xếp
            // Nếu chưa chọn gì thì mặc định là Priority
            string sortBy = cbSort.SelectedItem != null ? cbSort.SelectedItem.ToString() : "Priority";

            // 3. Gọi Procedure với đầy đủ 3 tham số
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@BoardID", 1), // Giả lập Board 1
        new SqlParameter("@SearchKeyword", searchParam),
        new SqlParameter("@SortBy", sortBy)
            };

            dgvCards.DataSource = db.ExecuteQuery("sp_GetBoardData", parameters);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Gọi Form 1
            CardDetailForm f = new CardDetailForm();
            f.ShowDialog();
            LoadData(); // Refresh lưới
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            // Gọi Form 3
            ReportForm f = new ReportForm(1); // Truyền BoardID
            f.ShowDialog();
        }

        private void dgvCards_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dgvCards.Rows[e.RowIndex].Cells["CardID"].Value);
                // Gọi Form để sửa
                CardDetailForm f = new CardDetailForm(id);
                f.ShowDialog();
                LoadData();
            }
        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvCards_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Mở form sửa
                int id = Convert.ToInt32(dgvCards.Rows[e.RowIndex].Cells["CardID"].Value);
                CardDetailForm f = new CardDetailForm(id);
                f.ShowDialog();
                LoadData();
            }
        }
    }
}