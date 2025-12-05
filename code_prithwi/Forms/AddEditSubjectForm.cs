using System;
using System.Windows.Forms;
using SmartStudyPlanner.Models;

namespace SmartStudyPlanner.Forms
{
    public partial class AddEditSubjectForm : Form
    {
        public Subject Subject { get; private set; }
        private bool _isEditMode;

        public AddEditSubjectForm()
        {
            InitializeComponent();
            _isEditMode = false;
            this.Text = "Add New Subject";
        }

        public AddEditSubjectForm(Subject subject) : this()
        {
            Subject = subject;
            _isEditMode = true;
            this.Text = "Edit Subject";
            LoadSubjectData();
        }

        private void LoadSubjectData()
        {
            if (Subject != null)
            {
                txtName.Text = Subject.Name;
                txtCode.Text = Subject.Code;
                txtTeacherName.Text = Subject.TeacherName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Subject Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtCode.Text))
                {
                    MessageBox.Show("Subject Code cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCode.Focus();
                    return;
                }

                if (_isEditMode)
                {
                    Subject.Name = txtName.Text.Trim();
                    Subject.Code = txtCode.Text.Trim();
                    Subject.TeacherName = txtTeacherName.Text.Trim();
                }
                else
                {
                    Subject = new Subject(txtName.Text.Trim(), txtCode.Text.Trim(), txtTeacherName.Text.Trim());
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the subject: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
