using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Managers
{
    public class TaskManager
    {
        private List<StudyTask> _tasks;

        public TaskManager()
        {
            _tasks = new List<StudyTask>();
        }

        public async Task LoadTasksAsync()
        {
            _tasks = await DataManager.LoadTasksAsync();
        }

        public async Task SaveTasksAsync()
        {
            await DataManager.SaveTasksAsync(_tasks);
        }

        public List<StudyTask> GetTasks()
        {
            return _tasks.OrderBy(t => t.DueDate).ToList();
        }

        public StudyTask GetTaskById(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void AddTask(StudyTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Task cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ArgumentException("Task Title cannot be empty.");
            }
            if (task.DueDate == DateTime.MinValue)
            {
                throw new ArgumentException("Task Due Date cannot be empty.");
            }

            _tasks.Add(task);
        }

        public void UpdateTask(StudyTask updatedTask)
        {
            if (updatedTask == null)
            {
                throw new ArgumentNullException(nameof(updatedTask), "Updated task cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(updatedTask.Title))
            {
                throw new ArgumentException("Task Title cannot be empty.");
            }
            if (updatedTask.DueDate == DateTime.MinValue)
            {
                throw new ArgumentException("Task Due Date cannot be empty.");
            }

            var existingTask = _tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with ID {updatedTask.Id} not found.");
            }

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.SubjectId = updatedTask.SubjectId;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.Priority = updatedTask.Priority;
            existingTask.Status = updatedTask.Status;
            existingTask.TaskType = updatedTask.TaskType;
        }

        public void DeleteTask(Guid taskId)
        {
            var taskToRemove = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToRemove == null)
            {
                throw new KeyNotFoundException($"Task with ID {taskId} not found.");
            }
            _tasks.Remove(taskToRemove);
        }

        public List<StudyTask> GetTodayTasks()
        {
            return _tasks.Where(t => t.DueDate.Date == DateTime.Today.Date && t.Status != Status.Completed)
                         .OrderBy(t => t.DueDate)
                         .ToList();
        }

        public List<StudyTask> GetUpcomingTasks()
        {
            DateTime sevenDaysFromNow = DateTime.Today.AddDays(7);
            return _tasks.Where(t => t.DueDate.Date > DateTime.Today.Date && t.DueDate.Date <= sevenDaysFromNow.Date && t.Status != Status.Completed)
                         .OrderBy(t => t.DueDate)
                         .ToList();
        }

        public List<StudyTask> GetOverdueTasks()
        {
            return _tasks.Where(t => t.DueDate.Date < DateTime.Today.Date && t.Status != Status.Completed)
                         .OrderBy(t => t.DueDate)
                         .ToList();
        }

        public string GenerateSummaryReport(SubjectManager subjectManager)
        {
            var report = new System.Text.StringBuilder();
            report.AppendLine("--- Smart Study Planner Summary Report ---");
            report.AppendLine($"Generated On: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine("----------------------------------------");
            report.AppendLine();

            report.AppendLine("Task Overview:");
            report.AppendLine($"Total Tasks: {_tasks.Count}");
            report.AppendLine($"Completed Tasks: {_tasks.Count(t => t.Status == Status.Completed)}");
            report.AppendLine($"Pending Tasks: {_tasks.Count(t => t.Status == Status.Pending || t.Status == Status.InProgress)}");
            report.AppendLine($"Overdue Tasks: {_tasks.Count(t => t.DueDate.Date < DateTime.Today.Date && t.Status != Status.Completed)}");
            report.AppendLine();

            report.AppendLine("Productivity Summary (Completed Tasks by Type):");
            var completedTasksByType = _tasks.Where(t => t.Status == Status.Completed)
                                             .GroupBy(t => t.TaskType)
                                             .ToDictionary(g => g.Key, g => g.Count());
            foreach (var entry in completedTasksByType)
            {
                report.AppendLine($"- {entry.Key}: {entry.Value}");
            }
            if (!completedTasksByType.Any())
            {
                report.AppendLine("- No completed tasks yet.");
            }
            report.AppendLine();

            report.AppendLine("Tasks by Subject:");
            var tasksBySubject = _tasks.GroupBy(t => t.SubjectId)
                                       .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var entry in tasksBySubject)
            {
                var subject = subjectManager.GetSubjectById(entry.Key);
                string subjectName = subject != null ? subject.Name : "Unknown Subject";
                report.AppendLine($"- {subjectName}: {entry.Value.Count} tasks");
                foreach (var task in entry.Value.OrderBy(t => t.DueDate))
                {
                    report.AppendLine($"  - [{task.Status}] {task.Title} (Due: {task.DueDate:yyyy-MM-dd}, Priority: {task.Priority})");
                }
            }
            if (!tasksBySubject.Any())
            {
                report.AppendLine("- No tasks assigned to subjects yet.");
            }
            report.AppendLine();

            report.AppendLine("----------------------------------------");
            return report.ToString();
        }
    }
}
