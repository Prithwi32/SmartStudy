using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Linq;
using SmartStudyPlanner.Managers;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Forms
{
    public partial class DashboardForm : Form
    {
        private TaskManager taskManager;
        private SubjectManager subjectManager;

        // UI Controls
        private Panel headerPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnSubjects;
        private Button btnTasks;
        private Button btnReports;
        private Button btnRefresh;

        private Panel statsPanel;
        private Panel cardTotalTasks;
        private Panel cardCompletedTasks;
        private Panel cardPendingTasks;
        private Panel cardOverdueTasks;

        private Panel filterPanel;
        private Label lblFilter;
        private ComboBox cmbFilter;
        private TextBox txtSearch;

        private Panel tasksPanel;
        private DataGridView dgvAllTasks;

        public DashboardForm()
        {
            InitializeComponent();
            taskManager = TaskManager.Instance;
            subjectManager = SubjectManager.Instance;
            
            SetupUI();
            this.Load += DashboardForm_Load;
            LoadDashboardData();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            // Position buttons after form is fully loaded and sized
            PositionHeaderButtons();
        }

        private void PositionHeaderButtons()
        {
            if (headerPanel == null) return;
            
            int buttonY = 35;
            int buttonSpacing = 10;
            int buttonWidth = 130;
            int rightMargin = 30;
            
            btnRefresh.Location = new Point(headerPanel.Width - rightMargin - buttonWidth, buttonY);
            btnReports.Location = new Point(btnRefresh.Left - buttonSpacing - buttonWidth, buttonY);
            btnTasks.Location = new Point(btnReports.Left - buttonSpacing - buttonWidth, buttonY);
            btnSubjects.Location = new Point(btnTasks.Left - buttonSpacing - buttonWidth, buttonY);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.ClientSize = new Size(1400, 900);
            this.Text = "Smart Study Planner - Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.FormClosing += DashboardForm_FormClosing;
            this.MinimumSize = new Size(1400, 900);
            this.Resize += DashboardForm_Resize;

            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            // Modern Header Panel with Gradient
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                Padding = new Padding(30, 20, 30, 20)
            };
            headerPanel.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    headerPanel.ClientRectangle,
                    Color.FromArgb(99, 102, 241),  // Indigo
                    Color.FromArgb(139, 92, 246),  // Purple
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
                }
            };

            lblTitle = new Label
            {
                Text = "📚 Smart Study Planner",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(30, 25)
            };

            lblSubtitle = new Label
            {
                Text = "Manage your study schedule efficiently",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(230, 230, 250),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(30, 75)
            };

            // Modern Navigation Buttons - create them first
            btnSubjects = CreateModernButton("📖 Subjects", 0, 35, Color.FromArgb(236, 72, 153));
            btnSubjects.Click += BtnSubjects_Click;
            btnSubjects.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            
            btnTasks = CreateModernButton("✓ Tasks", 0, 35, Color.FromArgb(251, 191, 36));
            btnTasks.Click += BtnTasks_Click;
            btnTasks.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            
            btnReports = CreateModernButton("📊 Reports", 0, 35, Color.FromArgb(59, 130, 246));
            btnReports.Click += BtnReports_Click;
            btnReports.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            
            btnRefresh = CreateModernButton("🔄 Refresh", 0, 35, Color.FromArgb(16, 185, 129));
            btnRefresh.Click += BtnRefresh_Click;
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            headerPanel.Controls.AddRange(new Control[] { lblTitle, lblSubtitle, btnSubjects, btnTasks, btnReports, btnRefresh });
            this.Controls.Add(headerPanel);

            // Modern Stats Panel with Cards
            statsPanel = new Panel
            {
                Location = new Point(30, 140),
                Size = new Size(this.ClientSize.Width - 60, 140),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            cardTotalTasks = CreateStatCard("📊 Total Tasks", "0", 0, Color.FromArgb(99, 102, 241), "lblTotalValue");
            cardCompletedTasks = CreateStatCard("✅ Completed", "0", 340, Color.FromArgb(16, 185, 129), "lblCompletedValue");
            cardPendingTasks = CreateStatCard("⏳ Pending", "0", 680, Color.FromArgb(251, 146, 60), "lblPendingValue");
            cardOverdueTasks = CreateStatCard("⚠️ Overdue", "0", 1020, Color.FromArgb(239, 68, 68), "lblOverdueValue");

            statsPanel.Controls.AddRange(new Control[] { cardTotalTasks, cardCompletedTasks, cardPendingTasks, cardOverdueTasks });
            this.Controls.Add(statsPanel);

            // Modern Filter Panel
            filterPanel = new Panel
            {
                Location = new Point(30, 300),
                Size = new Size(this.ClientSize.Width - 60, 70),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            filterPanel.Paint += (s, e) => DrawRoundedPanel(e.Graphics, filterPanel, 15, Color.White);

            lblFilter = new Label
            {
                Text = "🔍 Filter:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(15, 23),
                AutoSize = true
            };

            cmbFilter = new ComboBox
            {
                Location = new Point(110, 20),
                Size = new Size(380, 35),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbFilter.Items.AddRange(new string[] 
            { 
                "📅 Today's Tasks", 
                "⏰ Upcoming (Next 7 Days)", 
                "⚠️ Overdue Tasks", 
                "✅ Completed Tasks", 
                "📋 All Tasks" 
            });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += CmbFilter_SelectedIndexChanged;

            txtSearch = new TextBox
            {
                Location = new Point(510, 20),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(100, 116, 139),
                Text = "🔎 Search tasks..."
            };
            txtSearch.GotFocus += (s, e) => { if (txtSearch.Text == "🔎 Search tasks...") txtSearch.Text = ""; };
            txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) txtSearch.Text = "🔎 Search tasks..."; };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            filterPanel.Controls.AddRange(new Control[] { lblFilter, cmbFilter, txtSearch });
            this.Controls.Add(filterPanel);

            // Modern Tasks Panel
            tasksPanel = new Panel
            {
                Location = new Point(30, 390),
                Size = new Size(this.ClientSize.Width - 60, this.ClientSize.Height - 420),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            tasksPanel.Paint += (s, e) => DrawRoundedPanel(e.Graphics, tasksPanel, 15, Color.White);
            
            dgvAllTasks = CreateModernDataGridView();
            dgvAllTasks.Location = new Point(20, 20);
            dgvAllTasks.Size = new Size(tasksPanel.Width - 40, tasksPanel.Height - 40);
            dgvAllTasks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tasksPanel.Controls.Add(dgvAllTasks);
            this.Controls.Add(tasksPanel);
        }

        private Button CreateModernButton(string text, int x, int y, Color color)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(130, 45),
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(color, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(color, 0.1f);
            
            // Rounded corners
            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 10, 10));
            
            return btn;
        }

        private Panel CreateStatCard(string caption, string value, int x, Color color, string valueName)
        {
            var card = new Panel
            {
                Location = new Point(x, 0),
                Size = new Size(320, 130),
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            
            // Add shadow effect through paint event
            card.Paint += (s, e) =>
            {
                DrawRoundedPanel(e.Graphics, card, 15, Color.White);
                
                // Draw colored left border
                using (SolidBrush brush = new SolidBrush(color))
                {
                    e.Graphics.FillRectangle(brush, new Rectangle(0, 0, 6, card.Height));
                }
            };

            var lblIcon = new Label
            {
                Text = caption.Split(' ')[0], // Get emoji
                Font = new Font("Segoe UI", 32),
                Location = new Point(20, 25),
                Size = new Size(70, 70),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblCaption = new Label
            {
                Text = string.Join(" ", caption.Split(' ').Skip(1)),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(100, 30),
                AutoSize = true
            };

            var lblValue = new Label
            {
                Text = value,
                Name = valueName,
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(100, 55),
                AutoSize = true
            };

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(248, 250, 252);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            card.Controls.AddRange(new Control[] { lblIcon, lblCaption, lblValue });
            return card;
        }

        private DataGridView CreateModernDataGridView()
        {
            var dgv = new DataGridView
            {
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(226, 232, 240),
                ColumnHeadersHeight = 45,
                RowTemplate = { Height = 40 },
                EnableHeadersVisualStyles = false
            };

            // Modern header style
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252),
                ForeColor = Color.FromArgb(51, 65, 85),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(30, 64, 175),
                Padding = new Padding(10, 0, 0, 0)
            };

            // Add columns
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "📝 Task", Width = 300 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Subject", HeaderText = "📚 Subject", Width = 180 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "DueDate", HeaderText = "📅 Due Date", Width = 150 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Priority", HeaderText = "⚡ Priority", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "✓ Status", Width = 150 });

            return dgv;
        }

        private void DrawRoundedPanel(Graphics graphics, Panel panel, int radius, Color bgColor)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            using (GraphicsPath path = GetRoundedRectPath(new Rectangle(0, 0, panel.Width, panel.Height), radius))
            {
                using (SolidBrush brush = new SolidBrush(bgColor))
                {
                    graphics.FillPath(brush, path);
                }
                
                // Add subtle shadow
                using (Pen shadowPen = new Pen(Color.FromArgb(30, 0, 0, 0), 2))
                {
                    graphics.DrawPath(shadowPen, path);
                }
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            
            return path;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

        private void LoadDashboardData()
        {
            try
            {
                var stats = taskManager.GetTaskStatistics();

                // Update statistics - find value labels in each card
                UpdateStatValue(cardTotalTasks, "lblTotalValue", stats["Total"].ToString());
                UpdateStatValue(cardCompletedTasks, "lblCompletedValue", stats["Completed"].ToString());
                UpdateStatValue(cardPendingTasks, "lblPendingValue", stats["Pending"].ToString());
                UpdateStatValue(cardOverdueTasks, "lblOverdueValue", stats["Overdue"].ToString());

                // Load tasks based on filter
                LoadFilteredTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatValue(Panel card, string valueName, string newValue)
        {
            foreach (Control control in card.Controls)
            {
                if (control is Label lbl && lbl.Name == valueName)
                {
                    lbl.Text = newValue;
                    break;
                }
            }
        }

        private void CmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilteredTasks();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text != "🔎 Search tasks..." && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                SearchTasks(txtSearch.Text);
            }
            else
            {
                LoadFilteredTasks();
            }
        }

        private void SearchTasks(string searchText)
        {
            try
            {
                dgvAllTasks.Rows.Clear();
                var allTasks = taskManager.GetAllTasks();
                var filteredTasks = allTasks.Where(t => 
                    t.Title.ToLower().Contains(searchText.ToLower()) ||
                    t.Subject.ToLower().Contains(searchText.ToLower()) ||
                    t.Description.ToLower().Contains(searchText.ToLower())
                ).ToList();

                DisplayTasks(filteredTasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching tasks: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFilteredTasks()
        {
            try
            {
                dgvAllTasks.Rows.Clear();
                List<StudyTask> filteredTasks = new List<StudyTask>();

                switch (cmbFilter.SelectedIndex)
                {
                    case 0: // Today's Tasks
                        filteredTasks = taskManager.GetTodayTasks();
                        break;
                    case 1: // Upcoming Tasks (Next 7 Days)
                        filteredTasks = taskManager.GetUpcomingTasks();
                        break;
                    case 2: // Overdue Tasks
                        filteredTasks = taskManager.GetOverdueTasks();
                        break;
                    case 3: // Completed Tasks
                        filteredTasks = taskManager.GetCompletedTasks();
                        break;
                    case 4: // All Tasks
                        filteredTasks = taskManager.GetAllTasks();
                        break;
                }

                DisplayTasks(filteredTasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading filtered tasks: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayTasks(List<StudyTask> tasks)
        {
            foreach (var task in tasks)
            {
                int rowIndex = dgvAllTasks.Rows.Add(
                    task.Title, 
                    task.Subject, 
                    task.DueDate.ToString("dd/MM/yyyy"), 
                    task.Priority,
                    task.Status
                );

                // Modern color coding with softer colors
                if (task.Status == "Completed")
                {
                    dgvAllTasks.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(220, 252, 231);
                    dgvAllTasks.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(22, 163, 74);
                }
                else if (task.IsOverdue())
                {
                    dgvAllTasks.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
                    dgvAllTasks.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                }
                else if (task.IsDueToday())
                {
                    dgvAllTasks.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
                    dgvAllTasks.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(217, 119, 6);
                }
                else
                {
                    dgvAllTasks.Rows[rowIndex].DefaultCellStyle.BackColor = Color.White;
                    dgvAllTasks.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
                }
            }
        }

        private void BtnSubjects_Click(object sender, EventArgs e)
        {
            var subjectForm = new SubjectManagementForm();
            subjectForm.ShowDialog();
            LoadDashboardData(); // Refresh after closing
        }

        private void BtnTasks_Click(object sender, EventArgs e)
        {
            var taskForm = new TaskManagementForm();
            taskForm.ShowDialog();
            LoadDashboardData(); // Refresh after closing
        }

        private void BtnReports_Click(object sender, EventArgs e)
        {
            var reportForm = new ReportForm();
            reportForm.ShowDialog();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
            MessageBox.Show("✅ Dashboard refreshed successfully!", "Success", 
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DashboardForm_Resize(object sender, EventArgs e)
        {
            // Reposition buttons on resize
            PositionHeaderButtons();
            
            // Reposition stat cards to distribute evenly
            if (statsPanel != null && cardTotalTasks != null)
            {
                int panelWidth = statsPanel.Width;
                int cardWidth = 320;
                int totalGap = panelWidth - (cardWidth * 4);
                int gap = totalGap / 5;

                cardTotalTasks.Location = new Point(gap, 0);
                cardCompletedTasks.Location = new Point(gap * 2 + cardWidth, 0);
                cardPendingTasks.Location = new Point(gap * 3 + cardWidth * 2, 0);
                cardOverdueTasks.Location = new Point(gap * 4 + cardWidth * 3, 0);
            }
        }

        private async void DashboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Auto-save data when closing
                await taskManager.SaveDataAsync();
                await subjectManager.SaveDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
