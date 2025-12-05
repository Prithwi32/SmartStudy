using System;

namespace SmartStudyPlanner.Models
{
    /// <summary>
    /// Represents a study task with all its properties
    /// </summary>
    public class StudyTask
    {
        /// <summary>
        /// Gets or sets the unique identifier for the task
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the task
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the task
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the subject name this task belongs to
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the due date for the task
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the priority level (Low, Medium, High)
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// Gets or sets the status (Pending, InProgress, Completed)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the task type (Assignment, Test, Project, Revision)
        /// </summary>
        public string TaskType { get; set; }

        /// <summary>
        /// Gets or sets the date when the task was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public StudyTask()
        {
            Id = Guid.NewGuid();
            Title = string.Empty;
            Description = string.Empty;
            Subject = string.Empty;
            DueDate = DateTime.Now.AddDays(1);
            Priority = "Medium";
            Status = "Pending";
            TaskType = "Assignment";
            CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public StudyTask(string title, string description, string subject, DateTime dueDate, 
                        string priority, string status, string taskType)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? string.Empty;
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            DueDate = dueDate;
            Priority = priority ?? "Medium";
            Status = status ?? "Pending";
            TaskType = taskType ?? "Assignment";
            CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// Validates the task data
        /// </summary>
        /// <returns>True if valid, otherwise false</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Title) &&
                   !string.IsNullOrWhiteSpace(Subject) &&
                   DueDate > DateTime.MinValue;
        }

        /// <summary>
        /// Checks if the task is overdue
        /// </summary>
        /// <returns>True if overdue, otherwise false</returns>
        public bool IsOverdue()
        {
            return Status != "Completed" && DueDate.Date < DateTime.Now.Date;
        }

        /// <summary>
        /// Checks if the task is due today
        /// </summary>
        /// <returns>True if due today, otherwise false</returns>
        public bool IsDueToday()
        {
            return Status != "Completed" && DueDate.Date == DateTime.Now.Date;
        }

        /// <summary>
        /// Checks if the task is upcoming (within next 7 days, including today)
        /// </summary>
        /// <returns>True if upcoming, otherwise false</returns>
        public bool IsUpcoming()
        {
            return Status != "Completed" && 
                   DueDate.Date >= DateTime.Now.Date && 
                   DueDate.Date <= DateTime.Now.Date.AddDays(7);
        }

        public override string ToString()
        {
            return $"{Title} - {Subject} (Due: {DueDate:dd/MM/yyyy})";
        }
    }

    /// <summary>
    /// Enum for task priorities
    /// </summary>
    public static class TaskPriority
    {
        public const string Low = "Low";
        public const string Medium = "Medium";
        public const string High = "High";

        public static string[] GetAll()
        {
            return new[] { Low, Medium, High };
        }
    }

    /// <summary>
    /// Enum for task statuses
    /// </summary>
    public static class StudyTaskStatus
    {
        public const string Pending = "Pending";
        public const string InProgress = "InProgress";
        public const string Completed = "Completed";

        public static string[] GetAll()
        {
            return new[] { Pending, InProgress, Completed };
        }
    }

    /// <summary>
    /// Enum for task types
    /// </summary>
    public static class TaskType
    {
        public const string Assignment = "Assignment";
        public const string Test = "Test";
        public const string Project = "Project";
        public const string Revision = "Revision";

        public static string[] GetAll()
        {
            return new[] { Assignment, Test, Project, Revision };
        }
    }
}
