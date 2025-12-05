using System;
using System.Drawing;
using System.Windows.Forms;
using SmartStudyPlanner.Managers;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Forms
{
    public partial class AddEditTaskForm : Form
    {
        private TaskManager taskManager;
        private SubjectManager subjectManager;
        private StudyTask currentTask;
        private bool isEditMode;

        // UI Controls
        private Panel headerPanel;
        private Label lblTitle;
        
        private Label lblTaskTitle;
        private TextBox txtTaskTitle;
        
        private Label lblDescription;
        private TextBox txtDescription;
        
        private Label lblSubject;
        private ComboBox cmbSubject;
        
        private Label lblDueDate;
        private DateTimePicker dtpDueDate;
        
        private Label lblPriority;
        private ComboBox cmbPriority;
        
        private Label lblStatus;
        private ComboBox cmbStatus;
        
        private Label lblTaskType;
        private ComboBox cmbTaskType;
        
        private Button btnSave;
        private Button btnCancel;

        public AddEditTaskForm(StudyTask task = null)
        {
            InitializeComponent();
            taskManager = TaskManager.Instance;
            subjectManager = SubjectManager.Instance;
            currentTask = task;
            isEditMode = task != null;
            
            SetupUI();
            LoadSubjects();
            
            if (isEditMode)
            {
                LoadTaskData();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.ClientSize = new Size(600, 650);
            this.Text = "Add/Edit Task";
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
                BackColor = Color.FromArgb(46, 204, 113)
            };

            lblTitle = new Label
            {
                Text = isEditMode ? "✏ Edit Task" : "➕ Add New Task",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 22)
            };
            headerPanel.Controls.Add(lblTitle);
            this.Controls.Add(headerPanel);

            int labelX = 30;
            int controlX = 180;
            int controlWidth = 380;
            int startY = 90;
            int spacing = 70;

            // Task Title
            lblTaskTitle = new Label
            {
                Text = "Task Title:",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(labelX, startY),
                Size = new Size(140, 25)
            };

            txtTaskTitle = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(controlX, startY - 3),
                Size = new Size(controlWidth, 30),
                MaxLength = 200
            };

            // Description
            lblDescription = new Label
            {
                Text = "Description:",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(labelX, startY + spacing),
                Size = new Size(140, 25)
            };

            txtDescription = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(controlX, startY + spacing - 3),
                Size = new Size(controlWidth, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                MaxLength = 1000
            };

            // Subject
            lblSubject = new Label
            {
                Text = "Subject:",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(labelX, startY + spacing * 2 + 10),
                Size = new Size(140, 25)
            };

            cmbSubject = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(controlX, startY + spacing * 2 + 7),
                Size = new Size(controlWidth, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Due Date
            lblDueDate = new Label
            {
                Text = "Due Date:",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(labelX, startY + spacing * 3 + 10),
                Size = new Size(140, 25)
            };

            dtpDueDate = new DateTimePicker
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(controlX, startY + spacing * 3 + 7),
                Size = new Size(controlWidth, 30),
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Now.Date
            };

            // Priority
            lblPriority = new Label
            {
                Text = "Priority:",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(labelX, startY + spacing * 4 + 10),
                Size = new Size(140, 25)
            };

            cmbPriority = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(controlX, startY + spacing * 4 + 7),
                Size = new Size(controlWidth, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPriority.Items.AddRange(TaskPriority.GetAll());
            cmbPriority.SelectedIndex = 1; // Medium

            // Status
            lblStatus = new Label
            {
                Text = "Status:",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(labelX, startY + spacing * 5 + 10),
                Size = new Size(140, 25)
            };

            cmbStatus = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(controlX, startY + spacing * 5 + 7),
                Size = new Size(controlWidth, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(StudyTaskStatus.GetAll());
            cmbStatus.SelectedIndex = 0; // Pending

            // Task Type
            lblTaskType = new Label
            {
                Text = "Task Type:",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(labelX, startY + spacing * 6 + 10),
                Size = new Size(140, 25)
            };

            cmbTaskType = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(controlX, startY + spacing * 6 + 7),
                Size = new Size(controlWidth, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTaskType.Items.AddRange(TaskType.GetAll());
            cmbTaskType.SelectedIndex = 0; // Assignment

            // Buttons
            btnSave = new Button
            {
                Text = "💾 Save",
                Location = new Point(controlX, startY + spacing * 7 + 20),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "✕ Cancel",
                Location = new Point(controlX + 190, startY + spacing * 7 + 20),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] 
            { 
                lblTaskTitle, txtTaskTitle,
                lblDescription, txtDescription,
                lblSubject, cmbSubject,
                lblDueDate, dtpDueDate,
                lblPriority, cmbPriority,
                lblStatus, cmbStatus,
                lblTaskType, cmbTaskType,
                btnSave, btnCancel
            });
        }

        private void LoadSubjects()
        {
            cmbSubject.Items.Clear();
            var subjects = subjectManager.GetSubjectNames();
            
            if (subjects.Length == 0)
            {
                MessageBox.Show("No subjects found. Please add subjects first.", "Warning", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSubject.Items.Add("No Subjects Available");
                cmbSubject.SelectedIndex = 0;
                cmbSubject.Enabled = false;
            }
            else
            {
                cmbSubject.Items.AddRange(subjects);
                cmbSubject.SelectedIndex = 0;
            }
        }

        private void LoadTaskData()
        {
            if (currentTask != null)
            {
                txtTaskTitle.Text = currentTask.Title;
                txtDescription.Text = currentTask.Description;
                
                // Set subject
                int subjectIndex = cmbSubject.FindStringExact(currentTask.Subject);
                if (subjectIndex >= 0)
                    cmbSubject.SelectedIndex = subjectIndex;

                dtpDueDate.Value = currentTask.DueDate;
                
                cmbPriority.SelectedItem = currentTask.Priority;
                cmbStatus.SelectedItem = currentTask.Status;
                cmbTaskType.SelectedItem = currentTask.TaskType;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtTaskTitle.Text))
                {
                    MessageBox.Show("Task Title is required.", "Validation Error", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTaskTitle.Focus();
                    return;
                }

                if (cmbSubject.SelectedItem == null || cmbSubject.Text == "No Subjects Available")
                {
                    MessageBox.Show("Please select a valid subject.", "Validation Error", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dtpDueDate.Value.Date < DateTime.Now.Date)
                {
                    var result = MessageBox.Show("The due date is in the past. Do you want to continue?", 
                                                "Confirm", 
                                                MessageBoxButtons.YesNo, 
                                                MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                        return;
                }

                if (isEditMode)
                {
                    // Update existing task
                    currentTask.Title = txtTaskTitle.Text.Trim();
                    currentTask.Description = txtDescription.Text.Trim();
                    currentTask.Subject = cmbSubject.SelectedItem.ToString();
                    currentTask.DueDate = dtpDueDate.Value.Date;
                    currentTask.Priority = cmbPriority.SelectedItem.ToString();
                    currentTask.Status = cmbStatus.SelectedItem.ToString();
                    currentTask.TaskType = cmbTaskType.SelectedItem.ToString();

                    taskManager.UpdateTask(currentTask);
                    MessageBox.Show("Task updated successfully!", "Success", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Add new task
                    var newTask = new StudyTask(
                        txtTaskTitle.Text.Trim(),
                        txtDescription.Text.Trim(),
                        cmbSubject.SelectedItem.ToString(),
                        dtpDueDate.Value.Date,
                        cmbPriority.SelectedItem.ToString(),
                        cmbStatus.SelectedItem.ToString(),
                        cmbTaskType.SelectedItem.ToString()
                    );

                    taskManager.AddTask(newTask);
                    MessageBox.Show("Task added successfully!", "Success", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving task: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
