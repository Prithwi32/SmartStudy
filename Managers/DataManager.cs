using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Managers
{
    public static class DataManager
    {
        private static readonly string DataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string SubjectsFilePath = Path.Combine(DataDirectory, "subjects.json");
        private static readonly string TasksFilePath = Path.Combine(DataDirectory, "tasks.json");

        static DataManager()
        {
            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
            }
        }

        public static async Task<List<Subject>> LoadSubjectsAsync()
        {
            return await LoadDataAsync<List<Subject>>(SubjectsFilePath) ?? new List<Subject>();
        }

        public static async Task SaveSubjectsAsync(List<Subject> subjects)
        {
            await SaveDataAsync(subjects, SubjectsFilePath);
        }

        public static async Task<List<StudyTask>> LoadTasksAsync()
        {
            return await LoadDataAsync<List<StudyTask>>(TasksFilePath) ?? new List<StudyTask>();
        }

        public static async Task SaveTasksAsync(List<StudyTask> tasks)
        {
            await SaveDataAsync(tasks, TasksFilePath);
        }

        private static async Task<T> LoadDataAsync<T>(string filePath) where T : new()
        {
            if (!File.Exists(filePath))
            {
                return new T();
            }

            try
            {
                string jsonString = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., to a file or console)
                Console.WriteLine($"Error loading data from {filePath}: {ex.Message}");
                // Optionally, re-throw or return a default value
                return new T();
            }
        }

        private static async Task SaveDataAsync<T>(T data, string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(data, options);
                await File.WriteAllTextAsync(filePath, jsonString);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error saving data to {filePath}: {ex.Message}");
            }
        }
    }
}
