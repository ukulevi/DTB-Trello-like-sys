using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;

namespace TrelloSys
{
    /// <summary>
    /// ReportForm - Assignment 2, Mục 3.3
    /// Hiển thị thống kê năng suất và danh sách Card quá hạn
    /// Student: Nguyen Ngoc Kim (2352661)
    /// </summary>
    public partial class ReportForm : Form
    {
        private string connectionString;
        private int currentBoardID;

        public ReportForm()
        {
            InitializeComponent();
            // Lấy connection string từ App.config
            connectionString = ConfigurationManager.ConnectionStrings["TrelloTaskManagementDB"].ConnectionString;
        }

        public ReportForm(int boardID) : this()
        {
            currentBoardID = boardID;
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Load danh sách Boards vào ComboBox
                LoadBoardsComboBox();

                // Set default board nếu có
                if (currentBoardID > 0)
                {
                    cmbBoards.SelectedValue = currentBoardID;
                }

                // Load initial data
                if (cmbBoards.SelectedValue != null)
                {
                    LoadStatistics();
                    LoadOverdueCards();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi tạo form: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load danh sách Boards vào ComboBox
        /// </summary>
        private void LoadBoardsComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT b.BoardID, b.Name, w.Name AS WorkspaceName
            FROM Board b
            INNER JOIN Workspace w ON b.WorkspaceID = w.WorkspaceID
            WHERE b.IsClosed = 0
            ORDER BY w.Name, b.Name";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Thêm cột hiển thị
                dt.Columns.Add("DisplayName", typeof(string), "WorkspaceName + ' - ' + Name");

                // Cài đặt cột giá trị trước
                cmbBoards.DisplayMember = "DisplayName";
                cmbBoards.ValueMember = "BoardID";

                // Gán dữ liệu sau (Để tránh sự kiện chạy khi chưa biết ID ở đâu)
                cmbBoards.DataSource = dt;
            }
        }

        /// <summary>
        /// Load thống kê năng suất từ sp_ProjectStatistics
        /// Mục 2.3 - Query 2 với GROUP BY, HAVING, Aggregate Functions
        /// </summary>
        private void LoadStatistics()
        {
            // Nếu giá trị là null HOẶC đang là dòng dữ liệu (DataRowView) -> Dừng ngay
            if (cmbBoards.SelectedValue == null || cmbBoards.SelectedValue is DataRowView)
                return;

            int boardID = Convert.ToInt32(cmbBoards.SelectedValue);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ProjectStatistics", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BoardID", boardID);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        conn.Open();
                        adapter.Fill(dt);
                        conn.Close();

                        dgvStatistics.DataSource = dt;
                        FormatStatisticsGrid();
                        lblStatisticsSummary.Text = $"Tổng số thành viên: {dt.Rows.Count}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thống kê: {ex.Message}");
            }
        }

        /// <summary>
        /// Load danh sách Cards quá hạn
        /// Sử dụng function fn_GetOverdueDays trong query
        /// </summary>
        private void LoadOverdueCards()
        {
            if (cmbBoards.SelectedValue == null || cmbBoards.SelectedValue is DataRowView)
                return;

            int boardID = Convert.ToInt32(cmbBoards.SelectedValue);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetOverdueCards", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BoardID", boardID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        conn.Open();
                        adapter.Fill(dt);
                        conn.Close();

                        // Bind to DataGridView
                        dgvOverdueCards.DataSource = dt;

                        // Format columns
                        FormatOverdueGrid();

                        // Update summary
                        int totalOverdue = dt.Rows.Count;
                        int criticalCount = dt.AsEnumerable()
                            .Count(row => row.Field<int>("OverdueDays") > 7);

                        lblOverdueSummary.Text = $"Tổng số card quá hạn: {totalOverdue} " +
                            $"(Nghiêm trọng >7 ngày: {criticalCount})";

                        // Highlight critical items
                        HighlightCriticalOverdue();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách quá hạn: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Format DataGridView cho Statistics
        /// </summary>
        private void FormatStatisticsGrid()
        {
            // Nếu không có cột nào thì thoát luôn
            if (dgvStatistics.Columns.Count == 0) return;

            // --- CẤU HÌNH CỘT (Có kiểm tra tồn tại để tránh Crash) ---

            // 1. UserID
            if (dgvStatistics.Columns.Contains("UserID"))
            {
                dgvStatistics.Columns["UserID"].HeaderText = "ID";
                dgvStatistics.Columns["UserID"].Width = 50;
            }

            // 2. FullName
            if (dgvStatistics.Columns.Contains("FullName"))
            {
                dgvStatistics.Columns["FullName"].HeaderText = "Họ và Tên";
                dgvStatistics.Columns["FullName"].Width = 150;
            }

            // 3. Email
            if (dgvStatistics.Columns.Contains("Email"))
            {
                dgvStatistics.Columns["Email"].HeaderText = "Email";
                dgvStatistics.Columns["Email"].Width = 200;
            }

            // 4. TotalCards
            if (dgvStatistics.Columns.Contains("TotalCards"))
            {
                dgvStatistics.Columns["TotalCards"].HeaderText = "Tổng Cards";
                dgvStatistics.Columns["TotalCards"].Width = 80;
            }

            // 5. CompletedCards
            if (dgvStatistics.Columns.Contains("CompletedCards"))
            {
                dgvStatistics.Columns["CompletedCards"].HeaderText = "Đã hoàn thành";
                dgvStatistics.Columns["CompletedCards"].Width = 100;
            }

            // 6. PendingCards
            if (dgvStatistics.Columns.Contains("PendingCards"))
            {
                dgvStatistics.Columns["PendingCards"].HeaderText = "Đang làm";
                dgvStatistics.Columns["PendingCards"].Width = 80;
            }

            // 7. OverdueCards
            if (dgvStatistics.Columns.Contains("OverdueCards"))
            {
                dgvStatistics.Columns["OverdueCards"].HeaderText = "Quá hạn";
                dgvStatistics.Columns["OverdueCards"].Width = 80;
            }

            // 8. AvgCompletionDays
            if (dgvStatistics.Columns.Contains("AvgCompletionDays"))
            {
                dgvStatistics.Columns["AvgCompletionDays"].HeaderText = "Trung bình (ngày)";
                dgvStatistics.Columns["AvgCompletionDays"].Width = 120;
                dgvStatistics.Columns["AvgCompletionDays"].DefaultCellStyle.Format = "N1";
            }

            // 9. LastCardCreated
            if (dgvStatistics.Columns.Contains("LastCardCreated"))
            {
                dgvStatistics.Columns["LastCardCreated"].HeaderText = "Card mới nhất";
                dgvStatistics.Columns["LastCardCreated"].Width = 120;
                dgvStatistics.Columns["LastCardCreated"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            // 10. CompletionRate
            if (dgvStatistics.Columns.Contains("CompletionRate"))
            {
                dgvStatistics.Columns["CompletionRate"].HeaderText = "Tỷ lệ hoàn thành (%)";
                dgvStatistics.Columns["CompletionRate"].Width = 120;
                dgvStatistics.Columns["CompletionRate"].DefaultCellStyle.Format = "N2";
            }

            // --- CẤU HÌNH CHUNG ---
            dgvStatistics.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvStatistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        /// <summary>
        /// Format DataGridView cho Overdue Cards
        /// </summary>
        private void FormatOverdueGrid()
        {
            if (dgvOverdueCards.Columns.Count == 0) return;

            // --- CẤU HÌNH CỘT AN TOÀN (Có kiểm tra Contains) ---

            if (dgvOverdueCards.Columns.Contains("CardID"))
            {
                dgvOverdueCards.Columns["CardID"].HeaderText = "ID";
                dgvOverdueCards.Columns["CardID"].Width = 50;
            }

            if (dgvOverdueCards.Columns.Contains("Title"))
            {
                dgvOverdueCards.Columns["Title"].HeaderText = "Tiêu đề";
                dgvOverdueCards.Columns["Title"].Width = 200;
            }

            if (dgvOverdueCards.Columns.Contains("Description"))
            {
                dgvOverdueCards.Columns["Description"].HeaderText = "Mô tả";
                dgvOverdueCards.Columns["Description"].Width = 250;
            }

            if (dgvOverdueCards.Columns.Contains("DueDate"))
            {
                dgvOverdueCards.Columns["DueDate"].HeaderText = "Hạn chót";
                dgvOverdueCards.Columns["DueDate"].Width = 100;
                dgvOverdueCards.Columns["DueDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            if (dgvOverdueCards.Columns.Contains("Priority"))
            {
                dgvOverdueCards.Columns["Priority"].HeaderText = "Độ ưu tiên";
                dgvOverdueCards.Columns["Priority"].Width = 80;
            }

            if (dgvOverdueCards.Columns.Contains("ListName"))
            {
                dgvOverdueCards.Columns["ListName"].HeaderText = "Danh sách";
                dgvOverdueCards.Columns["ListName"].Width = 100;
            }

            if (dgvOverdueCards.Columns.Contains("OverdueDays"))
            {
                dgvOverdueCards.Columns["OverdueDays"].HeaderText = "Quá hạn (ngày)";
                dgvOverdueCards.Columns["OverdueDays"].Width = 100;
            }

            if (dgvOverdueCards.Columns.Contains("ProgressPercent"))
            {
                dgvOverdueCards.Columns["ProgressPercent"].HeaderText = "Tiến độ (%)";
                dgvOverdueCards.Columns["ProgressPercent"].Width = 80;
            }

            if (dgvOverdueCards.Columns.Contains("AssignedUsers"))
            {
                dgvOverdueCards.Columns["AssignedUsers"].HeaderText = "Người phụ trách";
                dgvOverdueCards.Columns["AssignedUsers"].Width = 200;
            }

            dgvOverdueCards.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvOverdueCards.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        /// <summary>
        /// Highlight các card quá hạn nghiêm trọng (>7 ngày)
        /// </summary>
        private void HighlightCriticalOverdue()
        {
            foreach (DataGridViewRow row in dgvOverdueCards.Rows)
            {
                if (row.Cells["OverdueDays"].Value != null &&
                    row.Cells["OverdueDays"].Value != DBNull.Value)
                {
                    int overdueDays = Convert.ToInt32(row.Cells["OverdueDays"].Value);

                    if (overdueDays > 14)
                    {
                        // Critical: > 14 days - Red background
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                    }
                    else if (overdueDays > 7)
                    {
                        // Warning: > 7 days - Yellow background
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkOrange;
                    }
                }
            }
        }

        /// <summary>
        /// Event handler khi chọn Board khác
        /// </summary>
        private void cmbBoards_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBoards.SelectedValue != null)
            {
                LoadStatistics();
                LoadOverdueCards();
            }
        }

        /// <summary>
        /// Refresh button click
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStatistics();
            LoadOverdueCards();
            MessageBox.Show("Dữ liệu đã được làm mới!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Export to Excel (Optional feature)
        /// </summary>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    FilterIndex = 1,
                    FileName = $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportDataGridViewToCSV(dgvStatistics, saveDialog.FileName);
                    MessageBox.Show("Xuất báo cáo thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất báo cáo: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Helper method to export DataGridView to CSV
        /// </summary>
        private void ExportDataGridViewToCSV(DataGridView dgv, string filePath)
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Write headers
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    writer.Write(dgv.Columns[i].HeaderText);
                    if (i < dgv.Columns.Count - 1)
                        writer.Write(",");
                }
                writer.WriteLine();

                // Write data rows
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            object value = row.Cells[i].Value;
                            writer.Write(value != null ? value.ToString().Replace(",", ";") : "");
                            if (i < dgv.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                }
            }
        }

        /// <summary>
        /// View card details when double-clicking
        /// </summary>
        private void dgvOverdueCards_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int cardID = Convert.ToInt32(dgvOverdueCards.Rows[e.RowIndex].Cells["CardID"].Value);
                string title = dgvOverdueCards.Rows[e.RowIndex].Cells["Title"].Value.ToString();
                int overdueDays = Convert.ToInt32(dgvOverdueCards.Rows[e.RowIndex].Cells["OverdueDays"].Value);

                MessageBox.Show(
                    $"Card ID: {cardID}\n" +
                    $"Tiêu đề: {title}\n" +
                    $"Quá hạn: {overdueDays} ngày\n\n" +
                    $"Vui lòng cập nhật tiến độ hoặc điều chỉnh deadline!",
                    "Chi tiết Card",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Close button
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}