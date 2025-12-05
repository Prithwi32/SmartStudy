namespace SmartStudyPlanner.Forms
{
    partial class TaskForm
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
            this.dgvTasks = new System.Windows.Forms.DataGridView();
            this.btnAddTask = new System.Windows.Forms.Button();
            this.btnEditTask = new System.Windows.Forms.Button();
            this.btnDeleteTask = new System.Windows.Forms.Button();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.cmbFilterTaskType = new System.Windows.Forms.ComboBox();
            this.lblFilterTaskType = new System.Windows.Forms.Label();
            this.cmbFilterPriority = new System.Windows.Forms.ComboBox();
            this.lblFilterPriority = new System.Windows.Forms.Label();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.cmbFilterSubject = new System.Windows.Forms.ComboBox();
            this.lblFilterSubject = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            this.panelFilters.SuspendLayout();
            this.SuspendLayout();
            //
            // dgvTasks
            //
            this.dgvTasks.AllowUserToAddRows = false;
            this.dgvTasks.AllowUserToDeleteRows = false;
            this.dgvTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTasks.Location = new System.Drawing.Point(12, 100);
            this.dgvTasks.MultiSelect = false;
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.ReadOnly = true;
            this.dgvTasks.RowHeadersWidth = 51;
            this.dgvTasks.RowTemplate.Height = 24;
            this.dgvTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTasks.Size = new System.Drawing.Size(776, 300);
            this.dgvTasks.TabIndex = 0;
            this.dgvTasks.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvTasks_CellFormatting);
            //
            // btnAddTask
            //
            this.btnAddTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddTask.Location = new System.Drawing.Point(12, 415);
            this.btnAddTask.Name = "btnAddTask";
            this.btnAddTask.Size = new System.Drawing.Size(100, 38);
            this.btnAddTask.TabIndex = 1;
            this.btnAddTask.Text = "Add New";
            this.btnAddTask.UseVisualStyleBackColor = true;
            this.btnAddTask.Click += new System.EventHandler(this.btnAddTask_Click);
            //
            // btnEditTask
            //
            this.btnEditTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditTask.Location = new System.Drawing.Point(118, 415);
            this.btnEditTask.Name = "btnEditTask";
            this.btnEditTask.Size = new System.Drawing.Size(100, 38);
            this.btnEditTask.TabIndex = 2;
            this.btnEditTask.Text = "Edit Selected";
            this.btnEditTask.UseVisualStyleBackColor = true;
            this.btnEditTask.Click += new System.EventHandler(this.btnEditTask_Click);
            //
            // btnDeleteTask
            //
            this.btnDeleteTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteTask.Location = new System.Drawing.Point(224, 415);
            this.btnDeleteTask.Name = "btnDeleteTask";
            this.btnDeleteTask.Size = new System.Drawing.Size(100, 38);
            this.btnDeleteTask.TabIndex = 3;
            this.btnDeleteTask.Text = "Delete Selected";
            this.btnDeleteTask.UseVisualStyleBackColor = true;
            this.btnDeleteTask.Click += new System.EventHandler(this.btnDeleteTask_Click);
            //
            // panelFilters
            //
            this.panelFilters.Controls.Add(this.txtSearch);
            this.panelFilters.Controls.Add(this.lblSearch);
            this.panelFilters.Controls.Add(this.cmbFilterTaskType);
            this.panelFilters.Controls.Add(this.lblFilterTaskType);
            this.panelFilters.Controls.Add(this.cmbFilterPriority);
            this.panelFilters.Controls.Add(this.lblFilterPriority);
            this.panelFilters.Controls.Add(this.cmbFilterStatus);
            this.panelFilters.Controls.Add(this.lblFilterStatus);
            this.panelFilters.Controls.Add(this.cmbFilterSubject);
            this.panelFilters.Controls.Add(this.lblFilterSubject);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 0);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(800, 94);
            this.panelFilters.TabIndex = 4;
            //
            // txtSearch
            //
            this.txtSearch.Location = new System.Drawing.Point(550, 55);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 22);
            this.txtSearch.TabIndex = 9;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(490, 58);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(57, 17);
            this.lblSearch.TabIndex = 8;
            this.lblSearch.Text = "Search:";
            //
            // cmbFilterTaskType
            //
            this.cmbFilterTaskType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterTaskType.FormattingEnabled = true;
            this.cmbFilterTaskType.Location = new System.Drawing.Point(350, 55);
            this.cmbFilterTaskType.Name = "cmbFilterTaskType";
            this.cmbFilterTaskType.Size = new System.Drawing.Size(121, 24);
            this.cmbFilterTaskType.TabIndex = 7;
            this.cmbFilterTaskType.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            //
            // lblFilterTaskType
            //
            this.lblFilterTaskType.AutoSize = true;
            this.lblFilterTaskType.Location = new System.Drawing.Point(270, 58);
            this.lblFilterTaskType.Name = "lblFilterTaskType";
            this.lblFilterTaskType.Size = new System.Drawing.Size(77, 17);
            this.lblFilterTaskType.TabIndex = 6;
            this.lblFilterTaskType.Text = "Task Type:";
            //
            // cmbFilterPriority
            //
            this.cmbFilterPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterPriority.FormattingEnabled = true;
            this.cmbFilterPriority.Location = new System.Drawing.Point(120, 55);
            this.cmbFilterPriority.Name = "cmbFilterPriority";
            this.cmbFilterPriority.Size = new System.Drawing.Size(121, 24);
            this.cmbFilterPriority.TabIndex = 5;
            this.cmbFilterPriority.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            //
            // lblFilterPriority
            //
            this.lblFilterPriority.AutoSize = true;
            this.lblFilterPriority.Location = new System.Drawing.Point(50, 58);
            this.lblFilterPriority.Name = "lblFilterPriority";
            this.lblFilterPriority.Size = new System.Drawing.Size(56, 17);
            this.lblFilterPriority.TabIndex = 4;
            this.lblFilterPriority.Text = "Priority:";
            //
            // cmbFilterStatus
            //
            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.FormattingEnabled = true;
            this.cmbFilterStatus.Location = new System.Drawing.Point(350, 15);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(121, 24);
            this.cmbFilterStatus.TabIndex = 3;
            this.cmbFilterStatus.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            //
            // lblFilterStatus
            //
            this.lblFilterStatus.AutoSize = true;
            this.lblFilterStatus.Location = new System.Drawing.Point(270, 18);
            this.lblFilterStatus.Name = "lblFilterStatus";
            this.lblFilterStatus.Size = new System.Drawing.Size(52, 17);
            this.lblFilterStatus.TabIndex = 2;
            this.lblFilterStatus.Text = "Status:";
            //
            // cmbFilterSubject
            //
            this.cmbFilterSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterSubject.FormattingEnabled = true;
            this.cmbFilterSubject.Location = new System.Drawing.Point(120, 15);
            this.cmbFilterSubject.Name = "cmbFilterSubject";
            this.cmbFilterSubject.Size = new System.Drawing.Size(121, 24);
            this.cmbFilterSubject.TabIndex = 1;
            this.cmbFilterSubject.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            //
            // lblFilterSubject
            //
            this.lblFilterSubject.AutoSize = true;
            this.lblFilterSubject.Location = new System.Drawing.Point(50, 18);
            this.lblFilterSubject.Name = "lblFilterSubject";
            this.lblFilterSubject.Size = new System.Drawing.Size(59, 17);
            this.lblFilterSubject.TabIndex = 0;
            this.lblFilterSubject.Text = "Subject:";
            //
            // TaskForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 465);
            this.Controls.Add(this.panelFilters);
            this.Controls.Add(this.btnDeleteTask);
            this.Controls.Add(this.btnEditTask);
            this.Controls.Add(this.btnAddTask);
            this.Controls.Add(this.dgvTasks);
            this.Name = "TaskForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Task Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.Button btnAddTask;
        private System.Windows.Forms.Button btnEditTask;
        private System.Windows.Forms.Button btnDeleteTask;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.ComboBox cmbFilterSubject;
        private System.Windows.Forms.Label lblFilterSubject;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.ComboBox cmbFilterPriority;
        private System.Windows.Forms.Label lblFilterPriority;
        private System.Windows.Forms.ComboBox cmbFilterTaskType;
        private System.Windows.Forms.Label lblFilterTaskType;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
    }
}
