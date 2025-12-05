using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SmartStudyPlanner.Managers;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Forms
{
    public partial class MainForm : Form
    {
        private readonly SubjectManager _subjectManager;
        private readonly TaskManager _taskManager;

        public MainForm()
        {
            InitializeComponent();
            _subjectManager = new SubjectManager();
            _taskManager = new TaskManager();
            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {n                await _subjectManager.LoadSubjectsAsync();
                await _taskManager.LoadTasksAsync();
                RefreshDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                await _subjectManager.SaveSubjectsAsync();
                await _taskManager.SaveTasksAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshDashboard()
        {
            // Clear existing data
            dgvTodayTasks.DataSource = null;
            dgvUpcomingTasks.DataSource = null;
            dgvOverdueTasks.DataSource = null;

            // Load and display Today's Tasks
            var todayTasks = _taskManager.GetTodayTasks().Select(t => new
            {
                t.Title,
                Subject = _subjectManager.GetSubjectById(t.SubjectId)?.Name ?? "Unknown",
                t.DueDate,
                t.Priority,
                t.Status
            }).ToList();
            dgvTodayTasks.DataSource = todayTasks;
            if (dgvTodayTasks.Columns.Contains("Id")) dgvTodayTasks.Columns["Id"].Visible = false;


            // Load and display Upcoming Tasks
            var upcomingTasks = _taskManager.GetUpcomingTasks().Select(t => new
            {
                t.Title,
                Subject = _subjectManager.GetSubjectById(t.SubjectId)?.Name ?? "Unknown",
                t.DueDate,
                t.Priority,
                t.Status
            }).ToList();
            dgvUpcomingTasks.DataSource = upcomingTasks;
            if (dgvUpcomingTasks.Columns.Contains("Id")) dgvUpcomingTasks.Columns["Id"].Visible = false;


            // Load and display Overdue Tasks
            var overdueTasks = _taskManager.GetOverdueTasks().Select(t => new
            {
                t.Title,
                Subject = _subjectManager.GetSubjectById(t.SubjectId)?.Name ?? "Unknown",
                t.DueDate,
                t.Priority,
                t.Status
            }).ToList();
            dgvOverdueTasks.DataSource = overdueTasks;
            if (dgvOverdueTasks.Columns.Contains("Id")) dgvOverdueTasks.Columns["Id"].Visible = false;


            // Update Summary Counts
            lblTotalTasksCount.Text = _taskManager.GetTasks().Count.ToString();
            lblCompletedTasksCount.Text = _taskManager.GetTasks().Count(t => t.Status == Status.Completed).ToString();
            lblPendingTasksCount.Text = _taskManager.GetTasks().Count(t => t.Status == Status.Pending || t.Status == Status.InProgress).ToString();
        }

        private void dgvTasks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv != null && e.RowIndex >= 0 && dgv.Rows[e.RowIndex].DataBoundItem != null)
            {
                dynamic rowData = dgv.Rows[e.RowIndex].DataBoundItem;
                DateTime dueDate = rowData.DueDate;
                Status status = rowData.Status;

                if (status == Status.Completed)
                {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen; // Green for Completed
                }
                else if (dueDate.Date < DateTime.Today.Date)
                {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral; // Red for Overdue
                }
                else if (dueDate.Date == DateTime.Today.Date)
                {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow; // Yellow for Due Today
                }
                else
                {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White; // White for Upcoming
                }
            }
        }

        private void manageSubjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var subjectForm = new SubjectForm(_subjectManager);
            subjectForm.ShowDialog();
            RefreshDashboard(); // Refresh dashboard after subject changes
        }

        private void manageTasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var taskForm = new TaskForm(_taskManager, _subjectManager);
            taskForm.ShowDialog();
            RefreshDashboard(); // Refresh dashboard after task changes
        }

        private void generateReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string reportContent = _taskManager.GenerateSummaryReport(_subjectManager);
                string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                if (!Directory.Exists(reportsDirectory))
                {
                    Directory.CreateDirectory(reportsDirectory);
                }
                string fileName = $"StudyReport_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string filePath = Path.Combine(reportsDirectory, fileName);

                File.WriteAllText(filePath, reportContent);
                MessageBox.Show($"Report generated successfully at:\n{filePath}", "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}