using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Managers
{
    public class SubjectManager
    {
        private List<Subject> _subjects;

        public SubjectManager()
        {
            _subjects = new List<Subject>();
        }

        public async Task LoadSubjectsAsync()
        {
            _subjects = await DataManager.LoadSubjectsAsync();
        }

        public async Task SaveSubjectsAsync()
        {
            await DataManager.SaveSubjectsAsync(_subjects);
        }

        public List<Subject> GetSubjects()
        {
            return _subjects.OrderBy(s => s.Name).ToList();
        }

        public Subject GetSubjectById(Guid id)
        {
            return _subjects.FirstOrDefault(s => s.Id == id);
        }

        public void AddSubject(Subject subject)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject), "Subject cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(subject.Name) || string.IsNullOrWhiteSpace(subject.Code))
            {
                throw new ArgumentException("Subject Name and Code cannot be empty.");
            }
            if (_subjects.Any(s => s.Name.Equals(subject.Name, StringComparison.OrdinalIgnoreCase) || s.Code.Equals(subject.Code, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A subject with the same name or code already exists.");
            }

            _subjects.Add(subject);
        }

        public void UpdateSubject(Subject updatedSubject)
        {
            if (updatedSubject == null)
            {
                throw new ArgumentNullException(nameof(updatedSubject), "Updated subject cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(updatedSubject.Name) || string.IsNullOrWhiteSpace(updatedSubject.Code))
            {
                throw new ArgumentException("Subject Name and Code cannot be empty.");
            }

            var existingSubject = _subjects.FirstOrDefault(s => s.Id == updatedSubject.Id);
            if (existingSubject == null)
            {
                throw new KeyNotFoundException($"Subject with ID {updatedSubject.Id} not found.");
            }

            // Check for duplicate name/code excluding the current subject being updated
            if (_subjects.Any(s => s.Id != updatedSubject.Id &&
                                  (s.Name.Equals(updatedSubject.Name, StringComparison.OrdinalIgnoreCase) ||
                                   s.Code.Equals(updatedSubject.Code, StringComparison.OrdinalIgnoreCase))))
            {
                throw new InvalidOperationException("Another subject with the same name or code already exists.");
            }

            existingSubject.Name = updatedSubject.Name;
            existingSubject.Code = updatedSubject.Code;
            existingSubject.TeacherName = updatedSubject.TeacherName;
        }

        public void DeleteSubject(Guid subjectId)
        {
            var subjectToRemove = _subjects.FirstOrDefault(s => s.Id == subjectId);
            if (subjectToRemove == null)
            {
                throw new KeyNotFoundException($"Subject with ID {subjectId} not found.");
            }
            _subjects.Remove(subjectToRemove);
        }
    }
}
