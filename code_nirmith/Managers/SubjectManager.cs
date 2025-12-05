using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Managers
{
    /// <summary>
    /// Manages subject data operations including CRUD and persistence
    /// </summary>
    public class SubjectManager
    {
        private List<Subject> _subjects;
        private readonly string _dataFilePath;
        private static SubjectManager _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets the singleton instance of SubjectManager
        /// </summary>
        public static SubjectManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SubjectManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SubjectManager()
        {
            _subjects = new List<Subject>();
            
            // Set data file path in Data folder
            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            
            _dataFilePath = Path.Combine(dataFolder, "subjects.json");
        }

        /// <summary>
        /// Adds a new subject
        /// </summary>
        /// <param name="subject">Subject to add</param>
        /// <exception cref="ArgumentNullException">Thrown when subject is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when subject is invalid or code already exists</exception>
        public void AddSubject(Subject subject)
        {
            try
            {
                if (subject == null)
                    throw new ArgumentNullException(nameof(subject), "Subject cannot be null");

                if (!subject.IsValid())
                    throw new InvalidOperationException("Subject data is invalid. All fields are required.");

                // Check for duplicate code
                if (_subjects.Any(s => s.Code.Equals(subject.Code, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException($"A subject with code '{subject.Code}' already exists.");

                _subjects.Add(subject);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to add subject: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing subject
        /// </summary>
        /// <param name="subject">Subject with updated data</param>
        /// <exception cref="ArgumentNullException">Thrown when subject is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when subject is not found or invalid</exception>
        public void UpdateSubject(Subject subject)
        {
            try
            {
                if (subject == null)
                    throw new ArgumentNullException(nameof(subject), "Subject cannot be null");

                if (!subject.IsValid())
                    throw new InvalidOperationException("Subject data is invalid. All fields are required.");

                var existingSubject = _subjects.FirstOrDefault(s => s.Id == subject.Id);
                if (existingSubject == null)
                    throw new InvalidOperationException("Subject not found.");

                // Check for duplicate code (excluding current subject)
                if (_subjects.Any(s => s.Id != subject.Id && 
                                      s.Code.Equals(subject.Code, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException($"Another subject with code '{subject.Code}' already exists.");

                existingSubject.Name = subject.Name;
                existingSubject.Code = subject.Code;
                existingSubject.TeacherName = subject.TeacherName;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update subject: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a subject by ID
        /// </summary>
        /// <param name="id">ID of the subject to delete</param>
        /// <returns>True if deleted, false if not found</returns>
        public bool DeleteSubject(Guid id)
        {
            try
            {
                var subject = _subjects.FirstOrDefault(s => s.Id == id);
                if (subject != null)
                {
                    _subjects.Remove(subject);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete subject: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all subjects
        /// </summary>
        /// <returns>List of all subjects</returns>
        public List<Subject> GetAllSubjects()
        {
            return new List<Subject>(_subjects);
        }

        /// <summary>
        /// Gets a subject by ID
        /// </summary>
        /// <param name="id">Subject ID</param>
        /// <returns>Subject if found, null otherwise</returns>
        public Subject GetSubjectById(Guid id)
        {
            return _subjects.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Gets subject names for dropdown lists
        /// </summary>
        /// <returns>Array of subject names</returns>
        public string[] GetSubjectNames()
        {
            return _subjects.Select(s => s.ToString()).OrderBy(n => n).ToArray();
        }

        /// <summary>
        /// Saves all subjects to JSON file
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

                string jsonString = JsonSerializer.Serialize(_subjects, options);
                await File.WriteAllTextAsync(_dataFilePath, jsonString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save subjects data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads subjects from JSON file
        /// </summary>
        /// <returns>Async task</returns>
        public async Task LoadDataAsync()
        {
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    string jsonString = await File.ReadAllTextAsync(_dataFilePath);
                    var loadedSubjects = JsonSerializer.Deserialize<List<Subject>>(jsonString);
                    
                    if (loadedSubjects != null)
                    {
                        _subjects = loadedSubjects;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load subjects data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clears all subjects (for testing purposes)
        /// </summary>
        public void ClearAll()
        {
            _subjects.Clear();
        }
    }
}
