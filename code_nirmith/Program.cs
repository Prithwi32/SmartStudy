using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using SmartStudyPlanner.Forms;
using SmartStudyPlanner.Managers;

namespace SmartStudyPlanner
{
    /// <summary>
    /// Main entry point for the Smart Study Planner application
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            // Load data asynchronously at startup
            try
            {
                LoadApplicationData();
                
                // Run the main dashboard form
                Application.Run(new DashboardForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Application startup error: {ex.Message}", 
                              "Startup Error", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads application data at startup
        /// </summary>
        private static void LoadApplicationData()
        {
            try
            {
                var taskManager = TaskManager.Instance;
                var subjectManager = SubjectManager.Instance;

                // Load data synchronously on startup
                Task.Run(async () =>
                {
                    await subjectManager.LoadDataAsync();
                    await taskManager.LoadDataAsync();
                }).Wait();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading application data: {ex.Message}\n\nThe application will start with empty data.", 
                              "Data Load Error", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Warning);
            }
        }
    }
}
