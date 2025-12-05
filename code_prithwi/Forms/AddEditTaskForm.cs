using System;
using System.Linq;
using System.Windows.Forms;
using SmartStudyPlanner.Managers;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Forms
{
    public partial class AddEditTaskForm : Form
    {
        public StudyTask Task { get; private set; }
        private bool _isEditMode;
        private readonly SubjectManager _subjectManager;

        public AddEditTaskForm(SubjectManager subjectManager)
        {
            InitializeComponent();
            _subjectManager = subjectManager;
            _isEditMode = false;
            this.Text = "Add New Task";
            InitializeComboBoxes();
        }

        public AddEditTaskForm(StudyTask task, SubjectManager subjectManager) : this(subjectManager)
        {
            Task = task;
            _isEditMode = true;
            this.Text = "Edit Task";
            LoadTaskData();
        }

        private void InitializeComboBoxes()
        {
            // Populate Subject ComboBox
            cmbSubject.DataSource = _subjectManager.GetSubjects();
            cmbSubject.DisplayMember = "Name";
            cmbSubject.ValueMember = "Id";

            // Populate Priority ComboBox
            cmbPriority.DataSource = Enum.GetValues(typeof(Priority));
            cmbPriority.SelectedItem = Priority.Medium; // Default

            // Populate Status ComboBox
            cmbStatus.DataSource = Enum.GetValues(typeof(Status));
            cmbStatus.SelectedItem = Status.Pending; // Default

            // Populate TaskType ComboBox
            cmbTaskType.DataSource = Enum.GetValues(typeof(TaskType));
            cmbTaskType.SelectedItem = TaskType.Assignment; // Default
        }

        private void LoadTaskData()
        {
            if (Task != null)
            {
                txtTitle.Text = Task.Title;
                txtDescription.Text = Task.Description;
                dtpDueDate.Value = Task.DueDate;
                cmbPriority.SelectedItem = Task.Priority;
                cmbStatus.SelectedItem = Task.Status;
                cmbTaskType.SelectedItem = Task.TaskType;

                // Select the correct subject
                cmbSubject.SelectedValue = Task.SubjectId;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Task Title cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTitle.Focus();
                    return;
                }
                if (cmbSubject.SelectedValue == null)
                {
                    MessageBox.Show("Please select a Subject.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbSubject.Focus();
                    return;
                }

                if (_isEditMode)
                {
                    Task.Title = txtTitle.Text.Trim();
                    Task.Description = txtDescription.Text.Trim();
                    Task.SubjectId = (Guid)cmbSubject.SelectedValue;
                    Task.DueDate = dtpDueDate.Value;
                    Task.Priority = (Priority)cmbPriority.SelectedItem;
                    Task.Status = (Status)cmbStatus.SelectedItem;
                    Task.TaskType = (TaskType)cmbTaskType.SelectedItem;
                }
                else
                {
                    Task = new StudyTask(
                        txtTitle.Text.Trim(),
                        txtDescription.Text.Trim(),
                        (Guid)cmbSubject.SelectedValue,
                        dtpDueDate.Value,
                        (Priority)cmbPriority.SelectedItem,
                        (Status)cmbStatus.SelectedItem,
                        (TaskType)cmbTaskType.SelectedItem
                    );
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
