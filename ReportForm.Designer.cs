namespace TrelloSys
{
    partial class ReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cmbBoards = new System.Windows.Forms.ComboBox();
            this.lblBoard = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxStatistics = new System.Windows.Forms.GroupBox();
            this.lblStatisticsSummary = new System.Windows.Forms.Label();
            this.dgvStatistics = new System.Windows.Forms.DataGridView();
            this.groupBoxOverdue = new System.Windows.Forms.GroupBox();
            this.lblOverdueSummary = new System.Windows.Forms.Label();
            this.dgvOverdueCards = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).BeginInit();
            this.groupBoxOverdue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOverdueCards)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(121)))), ((int)(((byte)(191)))));
            this.panelTop.Controls.Add(this.btnClose);
            this.panelTop.Controls.Add(this.btnExport);
            this.panelTop.Controls.Add(this.btnRefresh);
            this.panelTop.Controls.Add(this.cmbBoards);
            this.panelTop.Controls.Add(this.lblBoard);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 80);
            this.panelTop.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1095, 40);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(995, 40);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(90, 30);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Xuất CSV";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(0)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(895, 40);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cmbBoards
            // 
            this.cmbBoards.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoards.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbBoards.FormattingEnabled = true;
            this.cmbBoards.Location = new System.Drawing.Point(95, 42);
            this.cmbBoards.Name = "cmbBoards";
            this.cmbBoards.Size = new System.Drawing.Size(400, 25);
            this.cmbBoards.TabIndex = 2;
            this.cmbBoards.SelectedIndexChanged += new System.EventHandler(this.cmbBoards_SelectedIndexChanged);
            // 
            // lblBoard
            // 
            this.lblBoard.AutoSize = true;
            this.lblBoard.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBoard.ForeColor = System.Drawing.Color.White;
            this.lblBoard.Location = new System.Drawing.Point(15, 45);
            this.lblBoard.Name = "lblBoard";
            this.lblBoard.Size = new System.Drawing.Size(53, 19);
            this.lblBoard.TabIndex = 1;
            this.lblBoard.Text = "Board:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(404, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "BÁO CÁO THỐNG KÊ VÀ QUÁ HẠN";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 80);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxStatistics);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxOverdue);
            this.splitContainer1.Size = new System.Drawing.Size(1200, 620);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBoxStatistics
            // 
            this.groupBoxStatistics.Controls.Add(this.lblStatisticsSummary);
            this.groupBoxStatistics.Controls.Add(this.dgvStatistics);
            this.groupBoxStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxStatistics.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxStatistics.Location = new System.Drawing.Point(0, 0);
            this.groupBoxStatistics.Name = "groupBoxStatistics";
            this.groupBoxStatistics.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxStatistics.Size = new System.Drawing.Size(1200, 300);
            this.groupBoxStatistics.TabIndex = 0;
            this.groupBoxStatistics.TabStop = false;
            this.groupBoxStatistics.Text = "📊 Thống kê năng suất làm việc (sp_ProjectStatistics)";
            // 
            // lblStatisticsSummary
            // 
            this.lblStatisticsSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatisticsSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblStatisticsSummary.Location = new System.Drawing.Point(10, 265);
            this.lblStatisticsSummary.Name = "lblStatisticsSummary";
            this.lblStatisticsSummary.Size = new System.Drawing.Size(1180, 25);
            this.lblStatisticsSummary.TabIndex = 1;
            this.lblStatisticsSummary.Text = "Tổng số thành viên: 0";
            this.lblStatisticsSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvStatistics
            // 
            this.dgvStatistics.AllowUserToAddRows = false;
            this.dgvStatistics.AllowUserToDeleteRows = false;
            this.dgvStatistics.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStatistics.BackgroundColor = System.Drawing.Color.White;
            this.dgvStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStatistics.Location = new System.Drawing.Point(10, 26);
            this.dgvStatistics.Name = "dgvStatistics";
            this.dgvStatistics.ReadOnly = true;
            this.dgvStatistics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStatistics.Size = new System.Drawing.Size(1180, 264);
            this.dgvStatistics.TabIndex = 0;
            // 
            // groupBoxOverdue
            // 
            this.groupBoxOverdue.Controls.Add(this.lblOverdueSummary);
            this.groupBoxOverdue.Controls.Add(this.dgvOverdueCards);
            this.groupBoxOverdue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxOverdue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxOverdue.Location = new System.Drawing.Point(0, 0);
            this.groupBoxOverdue.Name = "groupBoxOverdue";
            this.groupBoxOverdue.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxOverdue.Size = new System.Drawing.Size(1200, 316);
            this.groupBoxOverdue.TabIndex = 0;
            this.groupBoxOverdue.TabStop = false;
            this.groupBoxOverdue.Text = "⚠️ Danh sách Card quá hạn (fn_GetOverdueDays)";
            // 
            // lblOverdueSummary
            // 
            this.lblOverdueSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblOverdueSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblOverdueSummary.ForeColor = System.Drawing.Color.Red;
            this.lblOverdueSummary.Location = new System.Drawing.Point(10, 281);
            this.lblOverdueSummary.Name = "lblOverdueSummary";
            this.lblOverdueSummary.Size = new System.Drawing.Size(1180, 25);
            this.lblOverdueSummary.TabIndex = 1;
            this.lblOverdueSummary.Text = "Tổng số card quá hạn: 0";
            this.lblOverdueSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvOverdueCards
            // 
            this.dgvOverdueCards.AllowUserToAddRows = false;
            this.dgvOverdueCards.AllowUserToDeleteRows = false;
            this.dgvOverdueCards.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOverdueCards.BackgroundColor = System.Drawing.Color.White;
            this.dgvOverdueCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOverdueCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOverdueCards.Location = new System.Drawing.Point(10, 26);
            this.dgvOverdueCards.Name = "dgvOverdueCards";
            this.dgvOverdueCards.ReadOnly = true;
            this.dgvOverdueCards.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOverdueCards.Size = new System.Drawing.Size(1180, 280);
            this.dgvOverdueCards.TabIndex = 0;
            this.dgvOverdueCards.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOverdueCards_CellDoubleClick);
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Báo cáo Thống kê & Quá hạn - Assignment 2";
            this.Load += new System.EventHandler(this.ReportForm_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxStatistics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).EndInit();
            this.groupBoxOverdue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOverdueCards)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBoard;
        private System.Windows.Forms.ComboBox cmbBoards;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxStatistics;
        private System.Windows.Forms.DataGridView dgvStatistics;
        private System.Windows.Forms.Label lblStatisticsSummary;
        private System.Windows.Forms.GroupBox groupBoxOverdue;
        private System.Windows.Forms.DataGridView dgvOverdueCards;
        private System.Windows.Forms.Label lblOverdueSummary;
    }
}