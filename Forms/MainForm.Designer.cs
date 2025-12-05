namespace SmartStudyPlanner.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.managementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageSubjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageTasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelDashboard = new System.Windows.Forms.Panel();
            this.groupBoxOverdueTasks = new System.Windows.Forms.GroupBox();
            this.dgvOverdueTasks = new System.Windows.Forms.DataGridView();
            this.groupBoxUpcomingTasks = new System.Windows.Forms.GroupBox();
            this.dgvUpcomingTasks = new System.Windows.Forms.DataGridView();
            this.groupBoxTodayTasks = new System.Windows.Forms.GroupBox();
            this.dgvTodayTasks = new System.Windows.Forms.DataGridView();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblPendingTasksCount = new System.Windows.Forms.Label();
            this.lblCompletedTasksCount = new System.Windows.Forms.Label();
            this.lblTotalTasksCount = new System.Windows.Forms.Label();
            this.lblPendingTasks = new System.Windows.Forms.Label();
            this.lblCompletedTasks = new System.Windows.Forms.Label();
            this.lblTotalTasks = new System.Windows.Forms.Label();
            this.lblDashboardTitle = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panelDashboard.SuspendLayout();
            this.groupBoxOverdueTasks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOverdueTasks)).BeginInit();
            this.groupBoxUpcomingTasks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpcomingTasks)).BeginInit();
            this.groupBoxTodayTasks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTodayTasks)).BeginInit();
            this.panelSummary.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip1
            //
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.managementToolStripMenuItem,
            this.reportsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1000, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            //
            // managementToolStripMenuItem
            //
            this.managementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageSubjectsToolStripMenuItem,
            this.manageTasksToolStripMenuItem});
            this.managementToolStripMenuItem.Name = "managementToolStripMenuItem";
            this.managementToolStripMenuItem.Size = new System.Drawing.Size(109, 24);
            this.managementToolStripMenuItem.Text = "Management";
            //
            // manageSubjectsToolStripMenuItem
            //
            this.manageSubjectsToolStripMenuItem.Name = "manageSubjectsToolStripMenuItem";
            this.manageSubjectsToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.manageSubjectsToolStripMenuItem.Text = "Manage Subjects";
            this.manageSubjectsToolStripMenuItem.Click += new System.EventHandler(this.manageSubjectsToolStripMenuItem_Click);
            //
            // manageTasksToolStripMenuItem
            //
            this.manageTasksToolStripMenuItem.Name = "manageTasksToolStripMenuItem";
            this.manageTasksToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.manageTasksToolStripMenuItem.Text = "Manage Tasks";
            this.manageTasksToolStripMenuItem.Click += new System.EventHandler(this.manageTasksToolStripMenuItem_Click);
            //
            // reportsToolStripMenuItem
            //
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateReportToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.reportsToolStripMenuItem.Text = "Reports";
            //
            // generateReportToolStripMenuItem
            //
            this.generateReportToolStripMenuItem.Name = "generateReportToolStripMenuItem";
            this.generateReportToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.generateReportToolStripMenuItem.Text = "Generate Report";
            this.generateReportToolStripMenuItem.Click += new System.EventHandler(this.generateReportToolStripMenuItem_Click);
            //
            // panelDashboard
            //
            this.panelDashboard.Controls.Add(this.groupBoxOverdueTasks);
            this.panelDashboard.Controls.Add(this.groupBoxUpcomingTasks);
            this.panelDashboard.Controls.Add(this.groupBoxTodayTasks);
            this.panelDashboard.Controls.Add(this.panelSummary);
            this.panelDashboard.Controls.Add(this.lblDashboardTitle);
            this.panelDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDashboard.Location = new System.Drawing.Point(0, 28);
            this.panelDashboard.Name = "panelDashboard";
            this.panelDashboard.Size = new System.Drawing.Size(1000, 672);
            this.panelDashboard.TabIndex = 1;
            //
            // groupBoxOverdueTasks
            //
            this.groupBoxOverdueTasks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOverdueTasks.Controls.Add(this.dgvOverdueTasks);
            this.groupBoxOverdueTasks.Location = new System.Drawing.Point(12, 460);
            this.groupBoxOverdueTasks.Name = "groupBoxOverdueTasks";
            this.groupBoxOverdueTasks.Size = new System.Drawing.Size(976, 200);
            this.groupBoxOverdueTasks.TabIndex = 4;
            this.groupBoxOverdueTasks.TabStop = false;
            this.groupBoxOverdueTasks.Text = "Overdue Tasks";
            //
            // dgvOverdueTasks
            //
            this.dgvOverdueTasks.AllowUserToAddRows = false;
            this.dgvOverdueTasks.AllowUserToDeleteRows = false;
            this.dgvOverdueTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOverdueTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOverdueTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOverdueTasks.Location = new System.Drawing.Point(3, 18);
            this.dgvOverdueTasks.Name = "dgvOverdueTasks";
            this.dgvOverdueTasks.ReadOnly = true;
            this.dgvOverdueTasks.RowHeadersWidth = 51;
            this.dgvOverdueTasks.RowTemplate.Height = 24;
            this.dgvOverdueTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOverdueTasks.Size = new System.Drawing.Size(970, 179);
            this.dgvOverdueTasks.TabIndex = 0;
            this.dgvOverdueTasks.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvTasks_CellFormatting);
            //
            // groupBoxUpcomingTasks
            //
            this.groupBoxUpcomingTasks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUpcomingTasks.Controls.Add(this.dgvUpcomingTasks);
            this.groupBoxUpcomingTasks.Location = new System.Drawing.Point(12, 250);
            this.groupBoxUpcomingTasks.Name = "groupBoxUpcomingTasks";
            this.groupBoxUpcomingTasks.Size = new System.Drawing.Size(976, 200);
            this.groupBoxUpcomingTasks.TabIndex = 3;
            this.groupBoxUpcomingTasks.TabStop = false;
            this.groupBoxUpcomingTasks.Text = "Upcoming Tasks (Next 7 Days)";
            //
            // dgvUpcomingTasks
            //
            this.dgvUpcomingTasks.AllowUserToAddRows = false;
            this.dgvUpcomingTasks.AllowUserToDeleteRows = false;
            this.dgvUpcomingTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUpcomingTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUpcomingTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUpcomingTasks.Location = new System.Drawing.Point(3, 18);
            this.dgvUpcomingTasks.Name = "dgvUpcomingTasks";
            this.dgvUpcomingTasks.ReadOnly = true;
            this.dgvUpcomingTasks.RowHeadersWidth = 51;
            this.dgvUpcomingTasks.RowTemplate.Height = 24;
            this.dgvUpcomingTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUpcomingTasks.Size = new System.Drawing.Size(970, 179);
            this.dgvUpcomingTasks.TabIndex = 0;
            this.dgvUpcomingTasks.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvTasks_CellFormatting);
            //
            // groupBoxTodayTasks
            //
            this.groupBoxTodayTasks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTodayTasks.Controls.Add(this.dgvTodayTasks);
            this.groupBoxTodayTasks.Location = new System.Drawing.Point(12, 44);
            this.groupBoxTodayTasks.Name = "groupBoxTodayTasks";
            this.groupBoxTodayTasks.Size = new System.Drawing.Size(976, 200);
            this.groupBoxTodayTasks.TabIndex = 2;
            this.groupBoxTodayTasks.TabStop = false;
            this.groupBoxTodayTasks.Text = "Today's Tasks";
            //
            // dgvTodayTasks
            //
            this.dgvTodayTasks.AllowUserToAddRows = false;
            this.dgvTodayTasks.AllowUserToDeleteRows = false;
            this.dgvTodayTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTodayTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTodayTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTodayTasks.Location = new System.Drawing.Point(3, 18);
            this.dgvTodayTasks.Name = "dgvTodayTasks";
            this.dgvTodayTasks.ReadOnly = true;
            this.dgvTodayTasks.RowHeadersWidth = 51;
            this.dgvTodayTasks.RowTemplate.Height = 24;
            this.dgvTodayTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTodayTasks.Size = new System.Drawing.Size(970, 179);
            this.dgvTodayTasks.TabIndex = 0;
            this.dgvTodayTasks.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvTasks_CellFormatting);
            //
            // panelSummary
            //
            this.panelSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSummary.Controls.Add(this.lblPendingTasksCount);
            this.panelSummary.Controls.Add(this.lblCompletedTasksCount);
            this.panelSummary.Controls.Add(this.lblTotalTasksCount);
            this.panelSummary.Controls.Add(this.lblPendingTasks);
            this.panelSummary.Controls.Add(this.lblCompletedTasks);
            this.panelSummary.Controls.Add(this.lblTotalTasks);
            this.panelSummary.Location = new System.Drawing.Point(788, 0);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(200, 100);
            this.panelSummary.TabIndex = 1;
            this.panelSummary.Visible = false; // Initially hidden, can be shown if needed
            //
            // lblPendingTasksCount
            //
            this.lblPendingTasksCount.AutoSize = true;
            this.lblPendingTasksCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPendingTasksCount.Location = new System.Drawing.Point(130, 60);
            this.lblPendingTasksCount.Name = "lblPendingTasksCount";
            this.lblPendingTasksCount.Size = new System.Drawing.Size(17, 17);
            this.lblPendingTasksCount.TabIndex = 5;
            this.lblPendingTasksCount.Text = "0";
            //
            // lblCompletedTasksCount
            //
            this.lblCompletedTasksCount.AutoSize = true;
            this.lblCompletedTasksCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompletedTasksCount.Location = new System.Drawing.Point(130, 35);
            this.lblCompletedTasksCount.Name = "lblCompletedTasksCount";
            this.lblCompletedTasksCount.Size = new System.Drawing.Size(17, 17);
            this.lblCompletedTasksCount.TabIndex = 4;
            this.lblCompletedTasksCount.Text = "0";
            //
            // lblTotalTasksCount
            //
            this.lblTotalTasksCount.AutoSize = true;
            this.lblTotalTasksCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTasksCount.Location = new System.Drawing.Point(130, 10);
            this.lblTotalTasksCount.Name = "lblTotalTasksCount";
            this.lblTotalTasksCount.Size = new System.Drawing.Size(17, 17);
            this.lblTotalTasksCount.TabIndex = 3;
            this.lblTotalTasksCount.Text = "0";
            //
            // lblPendingTasks
            //
            this.lblPendingTasks.AutoSize = true;
            this.lblPendingTasks.Location = new System.Drawing.Point(10, 60);
            this.lblPendingTasks.Name = "lblPendingTasks";
            this.lblPendingTasks.Size = new System.Drawing.Size(106, 17);
            this.lblPendingTasks.TabIndex = 2;
            this.lblPendingTasks.Text = "Pending Tasks:";
            //
            // lblCompletedTasks
            //
            this.lblCompletedTasks.AutoSize = true;
            this.lblCompletedTasks.Location = new System.Drawing.Point(10, 35);
            this.lblCompletedTasks.Name = "lblCompletedTasks";
            this.lblCompletedTasks.Size = new System.Drawing.Size(120, 17);
            this.lblCompletedTasks.TabIndex = 1;
            this.lblCompletedTasks.Text = "Completed Tasks:";
            //
            // lblTotalTasks
            //
            this.lblTotalTasks.AutoSize = true;
            this.lblTotalTasks.Location = new System.Drawing.Point(10, 10);
            this.lblTotalTasks.Name = "lblTotalTasks";
            this.lblTotalTasks.Size = new System.Drawing.Size(85, 17);
            this.lblTotalTasks.TabIndex = 0;
            this.lblTotalTasks.Text = "Total Tasks:";
            //
            // lblDashboardTitle
            //
            this.lblDashboardTitle.AutoSize = true;
            this.lblDashboardTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDashboardTitle.Location = new System.Drawing.Point(12, 9);
            this.lblDashboardTitle.Name = "lblDashboardTitle";
            this.lblDashboardTitle.Size = new System.Drawing.Size(154, 32);
            this.lblDashboardTitle.TabIndex = 0;
            this.lblDashboardTitle.Text = "Dashboard";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.panelDashboard);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smart Study Planner";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelDashboard.ResumeLayout(false);
            this.panelDashboard.PerformLayout();
            this.groupBoxOverdueTasks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOverdueTasks)).EndInit();
            this.groupBoxUpcomingTasks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpcomingTasks)).EndInit();
            this.groupBoxTodayTasks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTodayTasks)).EndInit();
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem managementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageSubjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageTasksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateReportToolStripMenuItem;
        private System.Windows.Forms.Panel panelDashboard;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblPendingTasksCount;
        private System.Windows.Forms.Label lblCompletedTasksCount;
        private System.Windows.Forms.Label lblTotalTasksCount;
        private System.Windows.Forms.Label lblPendingTasks;
        private System.Windows.Forms.Label lblCompletedTasks;
        private System.Windows.Forms.Label lblTotalTasks;
        private System.Windows.Forms.GroupBox groupBoxTodayTasks;
        private System.Windows.Forms.DataGridView dgvTodayTasks;
        private System.Windows.Forms.GroupBox groupBoxOverdueTasks;
        private System.Windows.Forms.DataGridView dgvOverdueTasks;
        private System.Windows.Forms.GroupBox groupBoxUpcomingTasks;
        private System.Windows.Forms.DataGridView dgvUpcomingTasks;
    }
}
