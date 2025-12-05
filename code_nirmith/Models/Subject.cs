using System;

namespace SmartStudyPlanner.Models
{
    /// <summary>
    /// Represents a subject/course in the study planner
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// Gets or sets the unique identifier for the subject
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the subject
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the subject code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the teacher's name
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Subject()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Code = string.Empty;
            TeacherName = string.Empty;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public Subject(string name, string code, string teacherName)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Code = code ?? throw new ArgumentNullException(nameof(code));
            TeacherName = teacherName ?? throw new ArgumentNullException(nameof(teacherName));
        }

        /// <summary>
        /// Validates the subject data
        /// </summary>
        /// <returns>True if valid, otherwise false</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Code) &&
                   !string.IsNullOrWhiteSpace(TeacherName);
        }

        public override string ToString()
        {
            return $"{Code} - {Name}";
        }
    }
}
