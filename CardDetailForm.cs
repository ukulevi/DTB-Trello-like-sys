using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TrelloSys;

namespace TrelloSys
{
    public partial class CardDetailForm : Form
    {
        // State variable: null indicates 'Add New', an integer indicates 'Edit'
        private int? _cardID = null;
        private int _listID = 1; // Default List ID (e.g., 'To Do' list)

        public CardDetailForm()
        {
            InitializeComponent();
            SetupForm();
        }

        public CardDetailForm(int cardID)
        {
            InitializeComponent();
            _cardID = cardID;
            SetupForm();
            LoadCardData();
        }

        private void SetupForm()
        {
            // Avoid null pointer if items are missing
            if (cbPriority.Items.Count > 0) cbPriority.SelectedIndex = 1;

            if (_cardID == null)
            {
                btnDelete.Visible = false;
                this.Text = "Add New Card";
            }
            else
            {
                btnDelete.Visible = true;
                this.Text = "Edit Card #" + _cardID;
            }
        }

        private void LoadCardData()
        {
            try
            {
                DatabaseHelper db = new DatabaseHelper();
                string query = "SELECT * FROM Card WHERE CardID = " + _cardID;
                DataTable dt = db.ExecuteQuery(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtTitle.Text = row["Title"].ToString();
                    txtDescription.Text = row["Description"].ToString();

                    string dbPriority = row["Priority"].ToString();
                    if (cbPriority.Items.Contains(dbPriority))
                        cbPriority.SelectedItem = dbPriority;

                    if (row["DueDate"] != DBNull.Value)
                        dtpDueDate.Value = Convert.ToDateTime(row["DueDate"]);

                    if (row["IsCompleted"] != DBNull.Value)
                        chkCompleted.Checked = Convert.ToBoolean(row["IsCompleted"]);

                    btnSave.Text = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string desc = txtDescription.Text.Trim();
            string priority = cbPriority.SelectedItem?.ToString() ?? "medium";

            // FIXED: This was cut off in your previous code
            DateTime dueDate = dtpDueDate.Value;
            bool isCompleted = chkCompleted.Checked;

            DatabaseHelper db = new DatabaseHelper();
            SqlParameter[] parameters;
            string procedureName;

            if (_cardID == null) // INSERT
            {
                procedureName = "sp_InsertCard";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@ListID", _listID),
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Description", desc),
                    new SqlParameter("@Priority", priority),
                    new SqlParameter("@CreatedBy", 1),
                    new SqlParameter("@DueDate", dueDate)
                };
            }
            else // UPDATE
            {
                procedureName = "sp_UpdateCard";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@CardID", _cardID),
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Description", desc),
                    new SqlParameter("@Priority", priority),
                    new SqlParameter("@DueDate", dueDate),
                    new SqlParameter("@IsCompleted", isCompleted)
                };
            }

            if (db.ExecuteNonQuery(procedureName, parameters))
            {
                string msg = _cardID == null ? "Card added successfully!" : "Card updated successfully!";
                MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this card?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DatabaseHelper db = new DatabaseHelper();
                SqlParameter[] p = { new SqlParameter("@CardID", _cardID) };

                if (db.ExecuteNonQuery("sp_DeleteCard", p))
                {
                    MessageBox.Show("Card deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }
    }
}