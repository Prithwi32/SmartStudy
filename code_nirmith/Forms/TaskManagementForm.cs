using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using SmartStudyPlanner.Managers;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Forms
{
    public partial class TaskManagementForm : Form
    {
        private TaskManager taskManager;
        private SubjectManager subjectManager;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        // UI Controls
        private Panel headerPanel;
        private Label lblTitle;
        private Button btnAddTask;

        private GroupBox grpFilters;
        private Label lblFilterStatus;
        private ComboBox cmbFilterStatus;
        private Label lblFilterSubject;
        private ComboBox cmbFilterSubject;
        private Label lblFilterPriority;
        private ComboBox cmbFilterPriority;
        private Label lblFilterType;
        private ComboBox cmbFilterType;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnApplyFilter;
        private Button btnClearFilter;

        private DataGridView dgvTasks;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        public TaskManagementForm()
        {
            InitializeComponent();
            taskManager = TaskManager.Instance;
            subjectManager = SubjectManager.Instance;
            
            SetupUI();
            LoadTasks();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.ClientSize = new Size(1300, 750);
            this.Text = "Task Management";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;

            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            // Modern Header Panel with Gradient
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                Padding = new Padding(30, 25, 30, 25)
            };
            headerPanel.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    headerPanel.ClientRectangle,
                    Color.FromArgb(30, 64, 175),
                    Color.FromArgb(29, 78, 216),
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
                }
            };

            lblTitle = new Label
            {
                Text = "✓ Task Management",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(30, 32)
            };
            headerPanel.Controls.Add(lblTitle);

            this.Controls.Add(headerPanel);

            // Modern Filter Panel
            grpFilters = new GroupBox
            {
                Text = "",
                Location = new Point(30, 120),
                Size = new Size(this.ClientSize.Width - 60, 200),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.White,
                Padding = new Padding(20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            grpFilters.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, grpFilters.Width, grpFilters.Height, 15, 15));

            var titleLabel = new Label
            {
                Text = "🔍 Filters & Search",
                Location = new Point(20, 15),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = true
            };
            grpFilters.Controls.Add(titleLabel);

            // Filter controls with modern styling
            int labelY = 55;
            int controlY = 80;
            int bottomRow = 130;
            int spacing = 240;

            lblFilterStatus = new Label { Text = "Status:", Location = new Point(20, labelY), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            cmbFilterStatus = new ComboBox { Location = new Point(20, controlY), Size = new Size(200, 35), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbFilterStatus.Items.AddRange(new object[] { "All", StudyTaskStatus.Pending, StudyTaskStatus.InProgress, StudyTaskStatus.Completed });
            cmbFilterStatus.SelectedIndex = 0;

            lblFilterSubject = new Label { Text = "Subject:", Location = new Point(20 + spacing, labelY), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            cmbFilterSubject = new ComboBox { Location = new Point(20 + spacing, controlY), Size = new Size(200, 35), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            lblFilterPriority = new Label { Text = "Priority:", Location = new Point(20 + spacing * 2, labelY), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            cmbFilterPriority = new ComboBox { Location = new Point(20 + spacing * 2, controlY), Size = new Size(200, 35), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbFilterPriority.Items.AddRange(new object[] { "All", TaskPriority.Low, TaskPriority.Medium, TaskPriority.High });
            cmbFilterPriority.SelectedIndex = 0;

            lblFilterType = new Label { Text = "Type:", Location = new Point(20 + spacing * 3, labelY), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            cmbFilterType = new ComboBox { Location = new Point(20 + spacing * 3, controlY), Size = new Size(200, 35), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cmbFilterType.Items.AddRange(new object[] { "All", TaskType.Assignment, TaskType.Test, TaskType.Project, TaskType.Revision });
            cmbFilterType.SelectedIndex = 0;

            lblSearch = new Label { Text = "🔎 Search keyword:", Location = new Point(20, bottomRow), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };
            txtSearch = new TextBox { Location = new Point(20, bottomRow + 25), Size = new Size(640, 35), Font = new Font("Segoe UI", 11), BorderStyle = BorderStyle.FixedSingle };

            btnApplyFilter = new Button
            {
                Text = "🔎 Apply Filter",
                Location = new Point(680, bottomRow + 24),
                Size = new Size(140, 37),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnApplyFilter.FlatAppearance.BorderSize = 0;
            btnApplyFilter.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnApplyFilter.Width, btnApplyFilter.Height, 10, 10));
            btnApplyFilter.Click += BtnApplyFilter_Click;

            btnClearFilter = new Button
            {
                Text = "✕ Clear",
                Location = new Point(835, bottomRow + 24),
                Size = new Size(120, 37),
                BackColor = Color.FromArgb(148, 163, 184),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClearFilter.FlatAppearance.BorderSize = 0;
            btnClearFilter.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClearFilter.Width, btnClearFilter.Height, 10, 10));
            btnClearFilter.Click += BtnClearFilter_Click;

            grpFilters.Controls.AddRange(new Control[] 
            { 
                lblFilterStatus, cmbFilterStatus,
                lblFilterSubject, cmbFilterSubject,
                lblFilterPriority, cmbFilterPriority,
                lblFilterType, cmbFilterType,
                lblSearch, txtSearch,
                btnApplyFilter, btnClearFilter
            });

            this.Controls.Add(grpFilters);

            // Modern Button Panel
            var buttonPanel = new Panel
            {
                Location = new Point(30, 335),
                Size = new Size(this.ClientSize.Width - 60, 60),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            btnAddTask = new Button
            {
                Text = "➕ New Task",
                Location = new Point(0, 8),
                Size = new Size(150, 50),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddTask.FlatAppearance.BorderSize = 0;
            btnAddTask.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAddTask.Width, btnAddTask.Height, 12, 12));
            btnAddTask.Click += BtnAddTask_Click;

            btnEdit = new Button
            {
                Text = "✏ Edit",
                Location = new Point(165, 8),
                Size = new Size(140, 50),
                BackColor = Color.FromArgb(251, 191, 36),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnEdit.Width, btnEdit.Height, 12, 12));
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "🗑 Delete",
                Location = new Point(320, 8),
                Size = new Size(140, 50),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDelete.Width, btnDelete.Height, 12, 12));
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = new Button
            {
                Text = "🔄 Refresh",
                Location = new Point(475, 8),
                Size = new Size(140, 50),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRefresh.Width, btnRefresh.Height, 12, 12));
            btnRefresh.Click += (s, e) => LoadTasks();

            buttonPanel.Controls.AddRange(new Control[] { btnAddTask, btnEdit, btnDelete, btnRefresh });
            this.Controls.Add(buttonPanel);

            // Modern DataGridView
            dgvTasks = new DataGridView
            {
                Location = new Point(30, 410),
                Size = new Size(this.ClientSize.Width - 60, this.ClientSize.Height - 430),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                MultiSelect = false,
                RowTemplate = { Height = 40 },
                EnableHeadersVisualStyles = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Add columns with emojis
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Visible = false });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "📝 Title", Width = 200 });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Subject", HeaderText = "📚 Subject", Width = 120 });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "DueDate", HeaderText = "📅 Due Date", Width = 100 });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Priority", HeaderText = "⚡ Priority", Width = 90 });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "📊 Status", Width = 110 });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "TaskType", HeaderText = "📂 Type", Width = 100 });
            dgvTasks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "📄 Description", Width = 300 });

            // Style headers with deep blue gradient theme
            dgvTasks.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 64, 175);
            dgvTasks.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTasks.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvTasks.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            dgvTasks.ColumnHeadersHeight = 50;

            this.Controls.Add(dgvTasks);

            // Load subject filter
            LoadSubjectFilter();
        }

        private void LoadSubjectFilter()
        {
            cmbFilterSubject.Items.Clear();
            cmbFilterSubject.Items.Add("All");
            var subjects = subjectManager.GetSubjectNames();
            cmbFilterSubject.Items.AddRange(subjects);
            cmbFilterSubject.SelectedIndex = 0;
        }

        private void LoadTasks(System.Collections.Generic.List<StudyTask> tasks = null)
        {
            try
            {
                dgvTasks.Rows.Clear();
                
                if (tasks == null)
                {
                    tasks = taskManager.GetAllTasks();
                }

                foreach (var task in tasks.OrderBy(t => t.DueDate))
                {
                    int rowIndex = dgvTasks.Rows.Add(
                        task.Id,
                        task.Title,
                        task.Subject,
                        task.DueDate.ToString("dd/MM/yyyy"),
                        task.Priority,
                        task.Status,
                        task.TaskType,
                        task.Description
                    );

                    // Enhanced color coding with better contrast
                    var row = dgvTasks.Rows[rowIndex];
                    
                    if (task.Status == StudyTaskStatus.Completed)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(187, 247, 208);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(22, 101, 52);
                        row.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    }
                    else if (task.IsOverdue())
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(254, 202, 202);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(153, 27, 27);
                        row.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    }
                    else if (task.IsDueToday())
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(254, 240, 138);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(113, 63, 18);
                        row.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
                    }
                    
                    // Add padding for better readability
                    row.DefaultCellStyle.Padding = new Padding(8, 5, 8, 5);

                    // Priority color indicator and emoji prefix
                    try
                    {
                        var priorityCell = row.Cells["Priority"];
                        var priorityText = task.Priority ?? TaskPriority.Medium;
                        string displayText = priorityText;
                        Color cellBack = Color.White;
                        Color cellFore = Color.FromArgb(30, 41, 59);

                        if (string.Equals(priorityText, TaskPriority.Low, StringComparison.OrdinalIgnoreCase))
                        {
                            displayText = "🟢 " + priorityText;
                            cellBack = Color.FromArgb(220, 252, 231);
                            cellFore = Color.FromArgb(6, 95, 70);
                        }
                        else if (string.Equals(priorityText, TaskPriority.Medium, StringComparison.OrdinalIgnoreCase))
                        {
                            displayText = "🟡 " + priorityText;
                            cellBack = Color.FromArgb(255, 247, 200);
                            cellFore = Color.FromArgb(133, 77, 14);
                        }
                        else if (string.Equals(priorityText, TaskPriority.High, StringComparison.OrdinalIgnoreCase))
                        {
                            displayText = "🔴 " + priorityText;
                            cellBack = Color.FromArgb(254, 226, 226);
                            cellFore = Color.FromArgb(153, 27, 27);
                        }

                        // Update displayed value
                        priorityCell.Value = displayText;

                        // Apply colored badge only when task is not completed or overdue (so status coloring stays prominent)
                        if (task.Status != StudyTaskStatus.Completed && !task.IsOverdue())
                        {
                            priorityCell.Style.BackColor = cellBack;
                            priorityCell.Style.ForeColor = cellFore;
                            priorityCell.Style.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                            priorityCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                    catch
                    {
                        // ignore styling errors to avoid breaking task loading
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnApplyFilter_Click(object sender, EventArgs e)
        {
            try
            {
                string status = cmbFilterStatus.SelectedItem?.ToString() == "All" ? null : cmbFilterStatus.SelectedItem?.ToString();
                string subject = cmbFilterSubject.SelectedItem?.ToString() == "All" ? null : cmbFilterSubject.SelectedItem?.ToString();
                string priority = cmbFilterPriority.SelectedItem?.ToString() == "All" ? null : cmbFilterPriority.SelectedItem?.ToString();
                string taskType = cmbFilterType.SelectedItem?.ToString() == "All" ? null : cmbFilterType.SelectedItem?.ToString();
                string searchText = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text;

                var filteredTasks = taskManager.FilterTasks(status, subject, priority, taskType, searchText);
                LoadTasks(filteredTasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filter: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClearFilter_Click(object sender, EventArgs e)
        {
            cmbFilterStatus.SelectedIndex = 0;
            cmbFilterSubject.SelectedIndex = 0;
            cmbFilterPriority.SelectedIndex = 0;
            cmbFilterType.SelectedIndex = 0;
            txtSearch.Clear();
            LoadTasks();
        }

        private void BtnAddTask_Click(object sender, EventArgs e)
        {
            var addForm = new AddEditTaskForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadTasks();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a task to edit.", "No Selection", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedRow = dgvTasks.SelectedRows[0];
                var taskId = (Guid)selectedRow.Cells["Id"].Value;
                var task = taskManager.GetTaskById(taskId);

                if (task != null)
                {
                    var editForm = new AddEditTaskForm(task);
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadTasks();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing task: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a task to delete.", "No Selection", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this task?", 
                                        "Confirm Delete", 
                                        MessageBoxButtons.YesNo, 
                                        MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var selectedRow = dgvTasks.SelectedRows[0];
                    var taskId = (Guid)selectedRow.Cells["Id"].Value;

                    if (taskManager.DeleteTask(taskId))
                    {
                        MessageBox.Show("Task deleted successfully!", "Success", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTasks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete task.", "Error", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting task: {ex.Message}", "Error", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
