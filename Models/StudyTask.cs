using System;

namespace SmartStudyPlanner.Models
{
    public class StudyTask
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique identifier for the task
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid SubjectId { get; set; } // Link to Subject by Id
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public TaskType TaskType { get; set; }

        public StudyTask() { } // Parameterless constructor for deserialization

        public StudyTask(string title, string description, Guid subjectId, DateTime dueDate, Priority priority, Status status, TaskType taskType)
        {
            Title = title;
            Description = description;
            SubjectId = subjectId;
            DueDate = dueDate;
            Priority = priority;
            Status = status;
            TaskType = taskType;
        }
    }

    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public enum Status
    {
        Pending,
        InProgress,
        Completed
    }

    public enum TaskType
    {
        Assignment,
        Test,
        Project,
        Revision
    }
}
