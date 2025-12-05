using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartStudyPlanner.Managers;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Forms
{
    public partial class TaskForm : Form
    {
        private readonly TaskManager _taskManager;
        private readonly SubjectManager _subjectManager;
        private List<StudyTask> _currentTasks; // To hold tasks after filtering/searching

        public TaskForm(TaskManager taskManager, SubjectManager subjectManager)
        {
            InitializeComponent();
            _taskManager = taskManager;
            _subjectManager = subjectManager;
            InitializeFilterComboBoxes();
            LoadTasks();
        }

        private void InitializeFilterComboBoxes()
        {
            // Subject Filter
            var subjects = _subjectManager.GetSubjects();
            subjects.Insert(0, new Subject { Id = Guid.Empty, Name = "All Subjects" }); // Add "All" option
            cmbFilterSubject.DataSource = subjects;
            cmbFilterSubject.DisplayMember = "Name";
            cmbFilterSubject.ValueMember = "Id";

            // Status Filter
            var statuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
            cmbFilterStatus.DataSource = new List<string> { "All Statuses" }.Concat(statuses.Select(s => s.ToString())).ToList();
            cmbFilterStatus.SelectedIndex = 0;

            // Priority Filter
            var priorities = Enum.GetValues(typeof(Priority)).Cast<Priority>().ToList();
            cmbFilterPriority.DataSource = new List<string> { "All Priorities" }.Concat(priorities.Select(p => p.ToString())).ToList();
            cmbFilterPriority.SelectedIndex = 0;

            // Task Type Filter
            var taskTypes = Enum.GetValues(typeof(TaskType)).Cast<TaskType>().ToList();
            cmbFilterTaskType.DataSource = new List<string> { "All Task Types" }.Concat(taskTypes.Select(tt => tt.ToString())).ToList();
            cmbFilterTaskType.SelectedIndex = 0;
        }

        private void LoadTasks()
        {
            try
            {
                _currentTasks = _taskManager.GetTasks();
                ApplyFiltersAndSearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFiltersAndSearch()
        {
            IEnumerable<StudyTask> filteredTasks = _taskManager.GetTasks();

            // Filter by Subject
            if (cmbFilterSubject.SelectedValue is Guid selectedSubjectId && selectedSubjectId != Guid.Empty)
            {
                filteredTasks = filteredTasks.Where(t => t.SubjectId == selectedSubjectId);
            }

            // Filter by Status
            if (cmbFilterStatus.SelectedItem is string selectedStatus && selectedStatus != "All Statuses")
            {
                filteredTasks = filteredTasks.Where(t => t.Status.ToString() == selectedStatus);
            }

            // Filter by Priority
            if (cmbFilterPriority.SelectedItem is string selectedPriority && selectedPriority != "All Priorities")
            {
                filteredTasks = filteredTasks.Where(t => t.Priority.ToString() == selectedPriority);
            }

            // Filter by Task Type
            if (cmbFilterTaskType.SelectedItem is string selectedTaskType && selectedTaskType != "All Task Types")
            {
                filteredTasks = filteredTasks.Where(t => t.TaskType.ToString() == selectedTaskType);
            }

            // Search by Title
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string searchTerm = txtSearch.Text.Trim().ToLower();
                filteredTasks = filteredTasks.Where(t => t.Title.ToLower().Contains(searchTerm));
            }

            // Project tasks to an anonymous type or DTO to include Subject Name
            var tasksToDisplay = filteredTasks.Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                SubjectName = _subjectManager.GetSubjectById(t.SubjectId)?.Name ?? "Unknown",
                t.DueDate,
                t.Priority,
                t.Status,
                t.TaskType
            }).ToList();

            dgvTasks.DataSource = tasksToDisplay;

            // Hide the Id column
            if (dgvTasks.Columns.Contains("Id"))
            {
                dgvTasks.Columns["Id"].Visible = false;
            }
        }

        private void dgvTasks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTasks.Rows[e.RowIndex].DataBoundItem is StudyTask task)
            {
                DateTime dueDate = task.DueDate;
                Status status = task.Status;

                if (status == Status.Completed)
                {
                    dgvTasks.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen; // Green for Completed
                }
                else if (dueDate.Date < DateTime.Today.Date)
                {
                    dgvTasks.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral; // Red for Overdue
                }
                else if (dueDate.Date == DateTime.Today.Date)
                {
                    dgvTasks.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow; // Yellow for Due Today
                }
                else
                {
                    dgvTasks.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White; // White for Upcoming
                }
            }
        }


        private void btnAddTask_Click(object sender, EventArgs e)
        {
            using (var addEditForm = new AddEditTaskForm(_subjectManager))
            {
                if (addEditForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _taskManager.AddTask(addEditForm.Task);
                        _taskManager.SaveTasksAsync(); // Save changes
                        LoadTasks(); // Refresh DataGridView
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEditTask_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count > 0)
            {
                // Get the actual StudyTask object from the manager using the Id
                // The DataBoundItem is an anonymous type, so we need to get the Id and then the original object
                dynamic selectedRowData = dgvTasks.SelectedRows[0].DataBoundItem;
                Guid taskId = selectedRowData.Id;
                var selectedTask = _taskManager.GetTaskById(taskId);

                if (selectedTask != null)
                {
                    // Create a copy to edit, so original isn't modified if user cancels
                    var taskToEdit = new StudyTask
                    {
                        Id = selectedTask.Id,
                        Title = selectedTask.Title,
                        Description = selectedTask.Description,
                        SubjectId = selectedTask.SubjectId,
                        DueDate = selectedTask.DueDate,
                        Priority = selectedTask.Priority,
                        Status = selectedTask.Status,
                        TaskType = selectedTask.TaskType
                    };

                    using (var addEditForm = new AddEditTaskForm(taskToEdit, _subjectManager))
                    {
                        if (addEditForm.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                _taskManager.UpdateTask(addEditForm.Task);
                                _taskManager.SaveTasksAsync(); // Save changes
                                LoadTasks(); // Refresh DataGridView
                            }
                            catch (ArgumentException ex)
                            {
                                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            catch (KeyNotFoundException ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error updating task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a task to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteTask_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count > 0)
            {
                dynamic selectedRowData = dgvTasks.SelectedRows[0].DataBoundItem;
                Guid taskId = selectedRowData.Id;
                string taskTitle = selectedRowData.Title;

                var confirmResult = MessageBox.Show($"Are you sure you want to delete task '{taskTitle}'?",
                                                    "Confirm Delete",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        _taskManager.DeleteTask(taskId);
                        _taskManager.SaveTasksAsync(); // Save changes
                        LoadTasks(); // Refresh DataGridView
                    }
                    catch (KeyNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a task to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFiltersAndSearch();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFiltersAndSearch();
        }
    }
}
