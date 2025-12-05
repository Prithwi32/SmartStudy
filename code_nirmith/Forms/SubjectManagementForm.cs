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
    public partial class SubjectManagementForm : Form
    {
        private SubjectManager subjectManager;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        // UI Controls
        private Panel headerPanel;
        private Label lblTitle;
        private Button btnAddSubject;

        private DataGridView dgvSubjects;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        public SubjectManagementForm()
        {
            InitializeComponent();
            subjectManager = SubjectManager.Instance;
            SetupUI();
            LoadSubjects();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.ClientSize = new Size(1100, 700);
            this.Text = "Subject Management";
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
                Text = "📖 Subject Management",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(30, 32)
            };
            headerPanel.Controls.Add(lblTitle);

            this.Controls.Add(headerPanel);

            // Modern Button Panel
            var buttonPanel = new Panel
            {
                Location = new Point(30, 120),
                Size = new Size(this.ClientSize.Width - 60, 60),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            btnAddSubject = new Button
            {
                Text = "➕ Add Subject",
                Location = new Point(0, 8),
                Size = new Size(160, 50),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddSubject.FlatAppearance.BorderSize = 0;
            btnAddSubject.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAddSubject.Width, btnAddSubject.Height, 12, 12));
            btnAddSubject.Click += BtnAddSubject_Click;

            btnEdit = new Button
            {
                Text = "✏ Edit",
                Location = new Point(175, 8),
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
                Location = new Point(330, 8),
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
                Location = new Point(485, 8),
                Size = new Size(140, 50),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRefresh.Width, btnRefresh.Height, 12, 12));
            btnRefresh.Click += (s, e) => LoadSubjects();

            buttonPanel.Controls.AddRange(new Control[] { btnAddSubject, btnEdit, btnDelete, btnRefresh });
            this.Controls.Add(buttonPanel);

            // Modern DataGridView
            dgvSubjects = new DataGridView
            {
                Location = new Point(30, 200),
                Size = new Size(this.ClientSize.Width - 60, this.ClientSize.Height - 220),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 11),
                MultiSelect = false,
                RowTemplate = { Height = 45 },
                EnableHeadersVisualStyles = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Add columns with emojis
            dgvSubjects.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "Id", 
                HeaderText = "ID", 
                Visible = false 
            });
            dgvSubjects.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "Code", 
                HeaderText = "📋 Subject Code"
            });
            dgvSubjects.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "Name", 
                HeaderText = "📚 Subject Name"
            });
            dgvSubjects.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "TeacherName", 
                HeaderText = "👨‍🏫 Teacher Name"
            });

            // Style headers with deep blue gradient theme
            dgvSubjects.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 64, 175);
            dgvSubjects.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSubjects.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dgvSubjects.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            dgvSubjects.ColumnHeadersHeight = 55;

            // Modern row styling
            dgvSubjects.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(219, 234, 254);
            dgvSubjects.DefaultCellStyle.SelectionBackColor = Color.FromArgb(29, 78, 216);
            dgvSubjects.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvSubjects.DefaultCellStyle.Padding = new Padding(8, 5, 8, 5);

            this.Controls.Add(dgvSubjects);
        }

        private void LoadSubjects()
        {
            try
            {
                dgvSubjects.Rows.Clear();
                var subjects = subjectManager.GetAllSubjects();

                foreach (var subject in subjects.OrderBy(s => s.Code))
                {
                    dgvSubjects.Rows.Add(subject.Id, subject.Code, subject.Name, subject.TeacherName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddSubject_Click(object sender, EventArgs e)
        {
            var addForm = new AddEditSubjectForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadSubjects();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a subject to edit.", "No Selection", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedRow = dgvSubjects.SelectedRows[0];
                var subjectId = (Guid)selectedRow.Cells["Id"].Value;
                var subject = subjectManager.GetSubjectById(subjectId);

                if (subject != null)
                {
                    var editForm = new AddEditSubjectForm(subject);
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadSubjects();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing subject: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a subject to delete.", "No Selection", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this subject?", 
                                        "Confirm Delete", 
                                        MessageBoxButtons.YesNo, 
                                        MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var selectedRow = dgvSubjects.SelectedRows[0];
                    var subjectId = (Guid)selectedRow.Cells["Id"].Value;

                    if (subjectManager.DeleteSubject(subjectId))
                    {
                        MessageBox.Show("Subject deleted successfully!", "Success", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSubjects();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete subject.", "Error", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting subject: {ex.Message}", "Error", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
