using System;

namespace SmartStudyPlanner.Models
{
    public class Subject
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique identifier for the subject
        public string Name { get; set; }
        public string Code { get; set; }
        public string TeacherName { get; set; }

        public Subject() { } // Parameterless constructor for deserialization

        public Subject(string name, string code, string teacherName)
        {
            Name = name;
            Code = code;
            TeacherName = teacherName;
        }

        public override string ToString()
        {
            return $"{Name} ({Code}) - {TeacherName}";
        }
    }
}
