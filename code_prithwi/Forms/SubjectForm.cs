using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SmartStudyPlanner.Managers;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Forms
{
    public partial class SubjectForm : Form
    {
        private readonly SubjectManager _subjectManager;

        public SubjectForm(SubjectManager subjectManager)
        {
            InitializeComponent();
            _subjectManager = subjectManager;
            LoadSubjects();
        }

        private void LoadSubjects()
        {
            try
            {
                dgvSubjects.DataSource = _subjectManager.GetSubjects();
                // Hide the Id column as it's an internal GUID
                if (dgvSubjects.Columns.Contains("Id"))
                {
                    dgvSubjects.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            using (var addEditForm = new AddEditSubjectForm())
            {
                if (addEditForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _subjectManager.AddSubject(addEditForm.Subject);
                        _subjectManager.SaveSubjectsAsync(); // Save changes
                        LoadSubjects(); // Refresh DataGridView
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding subject: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEditSubject_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count > 0)
            {
                var selectedSubject = dgvSubjects.SelectedRows[0].DataBoundItem as Subject;
                if (selectedSubject != null)
                {
                    // Create a copy to edit, so original isn't modified if user cancels
                    var subjectToEdit = new Subject
                    {
                        Id = selectedSubject.Id,
                        Name = selectedSubject.Name,
                        Code = selectedSubject.Code,
                        TeacherName = selectedSubject.TeacherName
                    };

                    using (var addEditForm = new AddEditSubjectForm(subjectToEdit))
                    {
                        if (addEditForm.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                _subjectManager.UpdateSubject(addEditForm.Subject);
                                _subjectManager.SaveSubjectsAsync(); // Save changes
                                LoadSubjects(); // Refresh DataGridView
                            }
                            catch (InvalidOperationException ex)
                            {
                                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            catch (KeyNotFoundException ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error updating subject: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a subject to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteSubject_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.SelectedRows.Count > 0)
            {
                var selectedSubject = dgvSubjects.SelectedRows[0].DataBoundItem as Subject;
                if (selectedSubject != null)
                {
                    var confirmResult = MessageBox.Show($"Are you sure you want to delete subject '{selectedSubject.Name}'?",
                                                        "Confirm Delete",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question);
                    if (confirmResult == DialogResult.Yes)
                    {
                        try
                        {
                            _subjectManager.DeleteSubject(selectedSubject.Id);
                            _subjectManager.SaveSubjectsAsync(); // Save changes
                            LoadSubjects(); // Refresh DataGridView
                        }
                        catch (KeyNotFoundException ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting subject: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a subject to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
