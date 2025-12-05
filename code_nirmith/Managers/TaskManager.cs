using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Managers
{
    /// <summary>
    /// Manages study task operations including CRUD, filtering, and persistence
    /// </summary>
    public class TaskManager
    {
        private List<StudyTask> _tasks;
        private readonly string _dataFilePath;
        private static TaskManager _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets the singleton instance of TaskManager
        /// </summary>
        public static TaskManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TaskManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private TaskManager()
        {
            _tasks = new List<StudyTask>();
            
            // Set data file path in Data folder
            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            
            _dataFilePath = Path.Combine(dataFolder, "tasks.json");
        }

        /// <summary>
        /// Adds a new task
        /// </summary>
        /// <param name="task">Task to add</param>
        /// <exception cref="ArgumentNullException">Thrown when task is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when task is invalid</exception>
        public void AddTask(StudyTask task)
        {
            try
            {
                if (task == null)
                    throw new ArgumentNullException(nameof(task), "Task cannot be null");

                if (!task.IsValid())
                    throw new InvalidOperationException("Task data is invalid. Title and Subject are required.");

                _tasks.Add(task);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to add task: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing task
        /// </summary>
        /// <param name="task">Task with updated data</param>
        /// <exception cref="ArgumentNullException">Thrown when task is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when task is not found or invalid</exception>
        public void UpdateTask(StudyTask task)
        {
            try
            {
                if (task == null)
                    throw new ArgumentNullException(nameof(task), "Task cannot be null");

                if (!task.IsValid())
                    throw new InvalidOperationException("Task data is invalid. Title and Subject are required.");

                var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
                if (existingTask == null)
                    throw new InvalidOperationException("Task not found.");

                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.Subject = task.Subject;
                existingTask.DueDate = task.DueDate;
                existingTask.Priority = task.Priority;
                existingTask.Status = task.Status;
                existingTask.TaskType = task.TaskType;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update task: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a task by ID
        /// </summary>
        /// <param name="id">ID of the task to delete</param>
        /// <returns>True if deleted, false if not found</returns>
        public bool DeleteTask(Guid id)
        {
            try
            {
                var task = _tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    _tasks.Remove(task);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete task: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <returns>List of all tasks</returns>
        public List<StudyTask> GetAllTasks()
        {
            return new List<StudyTask>(_tasks);
        }

        /// <summary>
        /// Gets a task by ID
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>Task if found, null otherwise</returns>
        public StudyTask GetTaskById(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Gets tasks due today
        /// </summary>
        /// <returns>List of tasks due today</returns>
        public List<StudyTask> GetTodayTasks()
        {
            return _tasks.Where(t => t.IsDueToday()).OrderBy(t => t.Priority).ToList();
        }

        /// <summary>
        /// Gets upcoming tasks (next 7 days)
        /// </summary>
        /// <returns>List of upcoming tasks</returns>
        public List<StudyTask> GetUpcomingTasks()
        {
            return _tasks.Where(t => t.IsUpcoming())
                        .OrderBy(t => t.DueDate)
                        .ThenBy(t => t.Priority)
                        .ToList();
        }

        /// <summary>
        /// Gets overdue tasks
        /// </summary>
        /// <returns>List of overdue tasks</returns>
        public List<StudyTask> GetOverdueTasks()
        {
            return _tasks.Where(t => t.IsOverdue())
                        .OrderBy(t => t.DueDate)
                        .ThenBy(t => t.Priority)
                        .ToList();
        }

        /// <summary>
        /// Gets completed tasks
        /// </summary>
        /// <returns>List of completed tasks</returns>
        public List<StudyTask> GetCompletedTasks()
        {
            return _tasks.Where(t => t.Status == StudyTaskStatus.Completed)
                        .OrderByDescending(t => t.DueDate)
                        .ToList();
        }

        /// <summary>
        /// Filters tasks based on criteria
        /// </summary>
        /// <param name="status">Filter by status (null for all)</param>
        /// <param name="subject">Filter by subject (null for all)</param>
        /// <param name="priority">Filter by priority (null for all)</param>
        /// <param name="taskType">Filter by task type (null for all)</param>
        /// <param name="searchText">Search in title (null for no search)</param>
        /// <returns>Filtered list of tasks</returns>
        public List<StudyTask> FilterTasks(string status = null, string subject = null, 
                                           string priority = null, string taskType = null, 
                                           string searchText = null)
        {
            var filtered = _tasks.AsEnumerable();

            if (!string.IsNullOrEmpty(status))
                filtered = filtered.Where(t => t.Status == status);

            if (!string.IsNullOrEmpty(subject))
                filtered = filtered.Where(t => t.Subject == subject);

            if (!string.IsNullOrEmpty(priority))
                filtered = filtered.Where(t => t.Priority == priority);

            if (!string.IsNullOrEmpty(taskType))
                filtered = filtered.Where(t => t.TaskType == taskType);

            if (!string.IsNullOrEmpty(searchText))
                filtered = filtered.Where(t => t.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);

            return filtered.OrderBy(t => t.DueDate).ToList();
        }

        /// <summary>
        /// Gets task statistics
        /// </summary>
        /// <returns>Dictionary with task counts</returns>
        public Dictionary<string, int> GetTaskStatistics()
        {
            return new Dictionary<string, int>
            {
                { "Total", _tasks.Count },
                { "Completed", _tasks.Count(t => t.Status == StudyTaskStatus.Completed) },
                { "Pending", _tasks.Count(t => t.Status == StudyTaskStatus.Pending) },
                { "InProgress", _tasks.Count(t => t.Status == StudyTaskStatus.InProgress) },
                { "Overdue", _tasks.Count(t => t.IsOverdue()) },
                { "DueToday", _tasks.Count(t => t.IsDueToday()) },
                { "Upcoming", _tasks.Count(t => t.IsUpcoming()) }
            };
        }

        /// <summary>
        /// Generates a text report for tasks
        /// </summary>
        /// <param name="startDate">Report start date</param>
        /// <param name="endDate">Report end date</param>
        /// <returns>Report as string</returns>
        public string GenerateReport(DateTime startDate, DateTime endDate)
        {
            var reportTasks = _tasks.Where(t => t.DueDate >= startDate && t.DueDate <= endDate).ToList();
            
            var sb = new StringBuilder();
            sb.AppendLine("==============================================");
            sb.AppendLine("      SMART STUDY PLANNER - REPORT");
            sb.AppendLine("==============================================");
            sb.AppendLine($"Report Period: {startDate:dd/MM/yyyy} to {endDate:dd/MM/yyyy}");
            sb.AppendLine($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine("==============================================\n");

            // Summary
            var completedCount = reportTasks.Count(t => t.Status == StudyTaskStatus.Completed);
            var pendingCount = reportTasks.Count(t => t.Status == StudyTaskStatus.Pending);
            var inProgressCount = reportTasks.Count(t => t.Status == StudyTaskStatus.InProgress);
            var overdueCount = reportTasks.Count(t => t.IsOverdue());

            sb.AppendLine("SUMMARY:");
            sb.AppendLine($"  Total Tasks: {reportTasks.Count}");
            sb.AppendLine($"  Completed: {completedCount}");
            sb.AppendLine($"  Pending: {pendingCount}");
            sb.AppendLine($"  In Progress: {inProgressCount}");
            sb.AppendLine($"  Overdue: {overdueCount}");
            
            if (reportTasks.Count > 0)
            {
                double completionRate = (double)completedCount / reportTasks.Count * 100;
                sb.AppendLine($"  Completion Rate: {completionRate:F2}%");
            }
            
            sb.AppendLine("\n==============================================\n");

            // Tasks by Status
            sb.AppendLine("COMPLETED TASKS:");
            var completed = reportTasks.Where(t => t.Status == StudyTaskStatus.Completed).OrderBy(t => t.DueDate);
            if (completed.Any())
            {
                foreach (var task in completed)
                {
                    sb.AppendLine($"  • {task.Title} ({task.Subject}) - Due: {task.DueDate:dd/MM/yyyy}");
                }
            }
            else
            {
                sb.AppendLine("  No completed tasks in this period.");
            }

            sb.AppendLine("\nPENDING TASKS:");
            var pending = reportTasks.Where(t => t.Status == StudyTaskStatus.Pending).OrderBy(t => t.DueDate);
            if (pending.Any())
            {
                foreach (var task in pending)
                {
                    sb.AppendLine($"  • {task.Title} ({task.Subject}) - Due: {task.DueDate:dd/MM/yyyy} - Priority: {task.Priority}");
                }
            }
            else
            {
                sb.AppendLine("  No pending tasks in this period.");
            }

            sb.AppendLine("\nOVERDUE TASKS:");
            var overdue = reportTasks.Where(t => t.IsOverdue()).OrderBy(t => t.DueDate);
            if (overdue.Any())
            {
                foreach (var task in overdue)
                {
                    sb.AppendLine($"  • {task.Title} ({task.Subject}) - Due: {task.DueDate:dd/MM/yyyy} - Priority: {task.Priority}");
                }
            }
            else
            {
                sb.AppendLine("  No overdue tasks in this period.");
            }

            sb.AppendLine("\n==============================================");
            sb.AppendLine("             END OF REPORT");
            sb.AppendLine("==============================================");

            return sb.ToString();
        }

        /// <summary>
        /// Saves report to a text file
        /// </summary>
        /// <param name="reportContent">Report content</param>
        /// <param name="fileName">File name (without path)</param>
        /// <returns>Full path to saved file</returns>
        public string SaveReportToFile(string reportContent, string fileName)
        {
            try
            {
                string reportsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                if (!Directory.Exists(reportsFolder))
                {
                    Directory.CreateDirectory(reportsFolder);
                }

                string filePath = Path.Combine(reportsFolder, fileName);
                File.WriteAllText(filePath, reportContent);
                return filePath;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves all tasks to JSON file
        /// </summary>
        /// <returns>Async task</returns>
        public async Task SaveDataAsync()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(_tasks, options);
                await File.WriteAllTextAsync(_dataFilePath, jsonString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save tasks data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads tasks from JSON file
        /// </summary>
        /// <returns>Async task</returns>
        public async Task LoadDataAsync()
        {
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    string jsonString = await File.ReadAllTextAsync(_dataFilePath);
                    var loadedTasks = JsonSerializer.Deserialize<List<StudyTask>>(jsonString);
                    
                    if (loadedTasks != null)
                    {
                        _tasks = loadedTasks;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load tasks data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clears all tasks (for testing purposes)
        /// </summary>
        public void ClearAll()
        {
            _tasks.Clear();
        }
    }
}
