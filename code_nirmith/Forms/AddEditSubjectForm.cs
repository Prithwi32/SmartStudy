using System;
using System.Drawing;
using System.Windows.Forms;
using SmartStudyPlanner.Managers;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Forms
{
    public partial class AddEditSubjectForm : Form
    {
        private SubjectManager subjectManager;
        private Subject currentSubject;
        private bool isEditMode;

        // UI Controls
        private Panel headerPanel;
        private Label lblTitle;
        
        private Label lblCode;
        private TextBox txtCode;
        
        private Label lblName;
        private TextBox txtName;
        
        private Label lblTeacher;
        private TextBox txtTeacher;
        
        private Button btnSave;
        private Button btnCancel;

        public AddEditSubjectForm(Subject subject = null)
        {
            InitializeComponent();
            subjectManager = SubjectManager.Instance;
            currentSubject = subject;
            isEditMode = subject != null;
            
            SetupUI();
            
            if (isEditMode)
            {
                LoadSubjectData();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.ClientSize = new Size(500, 400);
            this.Text = "Add/Edit Subject";
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            // Header Panel
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(30, 64, 175)
            };

            lblTitle = new Label
            {
                Text = isEditMode ? "✏ Edit Subject" : "➕ Add New Subject",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 22)
            };
            headerPanel.Controls.Add(lblTitle);
            this.Controls.Add(headerPanel);

            // Subject Code
            lblCode = new Label
            {
                Text = "Subject Code:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(30, 100),
                AutoSize = true
            };

            txtCode = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(30, 130),
                Size = new Size(430, 30),
                MaxLength = 20
            };

            // Subject Name
            lblName = new Label
            {
                Text = "Subject Name:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(30, 180),
                AutoSize = true
            };

            txtName = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(30, 210),
                Size = new Size(430, 30),
                MaxLength = 100
            };

            // Teacher Name
            lblTeacher = new Label
            {
                Text = "Teacher Name:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(30, 260),
                AutoSize = true
            };

            txtTeacher = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(30, 290),
                Size = new Size(430, 30),
                MaxLength = 100
            };

            // Buttons
            btnSave = new Button
            {
                Text = "💾 Save",
                Location = new Point(30, 340),
                Size = new Size(200, 45),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "✕ Cancel",
                Location = new Point(260, 340),
                Size = new Size(200, 45),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] 
            { 
                lblCode, txtCode,
                lblName, txtName,
                lblTeacher, txtTeacher,
                btnSave, btnCancel
            });
        }

        private void LoadSubjectData()
        {
            if (currentSubject != null)
            {
                txtCode.Text = currentSubject.Code;
                txtName.Text = currentSubject.Name;
                txtTeacher.Text = currentSubject.TeacherName;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtCode.Text))
                {
                    MessageBox.Show("Subject Code is required.", "Validation Error", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCode.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Subject Name is required.", "Validation Error", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTeacher.Text))
                {
                    MessageBox.Show("Teacher Name is required.", "Validation Error", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTeacher.Focus();
                    return;
                }

                if (isEditMode)
                {
                    // Update existing subject
                    currentSubject.Code = txtCode.Text.Trim();
                    currentSubject.Name = txtName.Text.Trim();
                    currentSubject.TeacherName = txtTeacher.Text.Trim();

                    subjectManager.UpdateSubject(currentSubject);
                    MessageBox.Show("Subject updated successfully!", "Success", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Add new subject
                    var newSubject = new Subject(
                        txtName.Text.Trim(),
                        txtCode.Text.Trim(),
                        txtTeacher.Text.Trim()
                    );

                    subjectManager.AddSubject(newSubject);
                    MessageBox.Show("Subject added successfully!", "Success", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving subject: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
