# Smart Study Planner

This is a C# Windows Forms application designed to help students manage their subjects and study tasks efficiently.

## Features

- **Subject Management**: Add, edit, delete subjects with Name, Code, and TeacherName.
- **Task Management**: Add, edit, delete tasks with Title, Description, Subject, DueDate, Priority, Status, and TaskType.
- **Dashboard**: View today's, upcoming, and overdue tasks, along with summary counts.
- **Filtering & Searching**: Filter tasks by Status, Subject, Priority, Task Type, and search by title.
- **Color Coding**: Tasks in the DataGridView are color-coded based on their due date and status.
  - Red: Overdue
  - Yellow: Due Today
  - Green: Completed
  - White: Upcoming
- **Reports**: Generate weekly/monthly summary reports in TXT format.
- **Data Persistence**: Data is automatically loaded at startup and saved on application close using JSON files.

## Project Structure

- `Models`: Contains `Subject` and `StudyTask` classes.
- `Managers`: Contains `SubjectManager`, `TaskManager`, and `DataManager` (for JSON persistence).
- `Forms`: Contains all Windows Forms for the application (MainForm, SubjectForm, TaskForm, AddEditTaskForm, AddEditSubjectForm).
- `Data`: Stores `subjects.json` and `tasks.json`.
- `Reports`: Stores generated reports.

## How to Run

1.  **Prerequisites**:
    *   .NET Framework (usually comes with Visual Studio).
    *   Visual Studio (recommended IDE).

2.  **Open the Solution**:
    *   Open the `SmartStudyPlanner.sln` file in Visual Studio.

3.  **Build the Project**:
    *   In Visual Studio, go to `Build` > `Build Solution` (or press `Ctrl+Shift+B`).

4.  **Run the Application**:
    *   Start the application by pressing `F5` or clicking the `Start` button in Visual Studio.

## Data Storage

The application uses JSON files (`subjects.json` and `tasks.json`) located in the `Data` folder for data persistence. These files are automatically created if they don't exist.

## Contributing

Feel free to fork the repository, make improvements, and submit pull requests.

## License

This project is open-source and available under the MIT License.
