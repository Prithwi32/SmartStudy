using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using SmartStudyPlanner.Managers;

namespace SmartStudyPlanner.Forms
{
    public partial class ReportForm : Form
    {
        private TaskManager taskManager;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        // UI Controls
        private Panel headerPanel;
        private Label lblTitle;

        private GroupBox grpReportType;
        private RadioButton rbWeekly;
        private RadioButton rbMonthly;
        private RadioButton rbCustom;

        private GroupBox grpDateRange;
        private Label lblStartDate;
        private DateTimePicker dtpStartDate;
        private Label lblEndDate;
        private DateTimePicker dtpEndDate;

        private Button btnGenerateReport;
        private Button btnSaveReport;

        private GroupBox grpReportPreview;
        private TextBox txtReportPreview;

        private string currentReportContent;

        public ReportForm()
        {
            InitializeComponent();
            taskManager = TaskManager.Instance;
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.ClientSize = new Size(1000, 750);
            this.Text = "Reports";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;

            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            // Modern Header Panel with Gradient
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                Padding = new Padding(30, 25, 30, 25)
            };
            headerPanel.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    headerPanel.ClientRectangle,
                    Color.FromArgb(139, 92, 246),
                    Color.FromArgb(124, 58, 237),
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillRectangle(brush, headerPanel.ClientRectangle);
                }
            };

            lblTitle = new Label
            {
                Text = "📊 Reports & Analytics",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(30, 32)
            };
            headerPanel.Controls.Add(lblTitle);

            this.Controls.Add(headerPanel);

            // Modern Report Type Group
            grpReportType = new GroupBox
            {
                Text = "Report Type",
                Location = new Point(30, 120),
                Size = new Size(940, 90),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                BackColor = Color.White
            };
            grpReportType.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, grpReportType.Width, grpReportType.Height, 15, 15));

            rbWeekly = new RadioButton
            {
                Text = "📅 Weekly Report (Last 7 Days)",
                Location = new Point(30, 40),
                Size = new Size(270, 35),
                Font = new Font("Segoe UI", 11),
                Checked = true
            };
            rbWeekly.CheckedChanged += ReportType_CheckedChanged;

            rbMonthly = new RadioButton
            {
                Text = "📆 Monthly Report (Last 30 Days)",
                Location = new Point(330, 40),
                Size = new Size(280, 35),
                Font = new Font("Segoe UI", 11)
            };
            rbMonthly.CheckedChanged += ReportType_CheckedChanged;

            rbCustom = new RadioButton
            {
                Text = "⚙️ Custom Date Range",
                Location = new Point(640, 40),
                Size = new Size(230, 35),
                Font = new Font("Segoe UI", 11)
            };
            rbCustom.CheckedChanged += ReportType_CheckedChanged;

            grpReportType.Controls.AddRange(new Control[] { rbWeekly, rbMonthly, rbCustom });
            this.Controls.Add(grpReportType);

            // Modern Date Range Group
            grpDateRange = new GroupBox
            {
                Text = "Custom Date Range",
                Location = new Point(30, 225),
                Size = new Size(940, 90),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                BackColor = Color.White,
                Enabled = false
            };
            grpDateRange.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, grpDateRange.Width, grpDateRange.Height, 15, 15));

            lblStartDate = new Label
            {
                Text = "📅 Start Date:",
                Location = new Point(40, 42),
                Size = new Size(110, 30),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            dtpStartDate = new DateTimePicker
            {
                Location = new Point(160, 40),
                Size = new Size(280, 35),
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 11)
            };

            lblEndDate = new Label
            {
                Text = "📅 End Date:",
                Location = new Point(490, 42),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            dtpEndDate = new DateTimePicker
            {
                Location = new Point(600, 40),
                Size = new Size(280, 35),
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 11),
                Value = DateTime.Now
            };

            grpDateRange.Controls.AddRange(new Control[] { lblStartDate, dtpStartDate, lblEndDate, dtpEndDate });
            this.Controls.Add(grpDateRange);

            // Modern Buttons
            btnGenerateReport = new Button
            {
                Text = "📄 Generate Report",
                Location = new Point(30, 335),
                Size = new Size(220, 55),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGenerateReport.FlatAppearance.BorderSize = 0;
            btnGenerateReport.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnGenerateReport.Width, btnGenerateReport.Height, 12, 12));
            btnGenerateReport.Click += BtnGenerateReport_Click;

            btnSaveReport = new Button
            {
                Text = "💾 Save Report",
                Location = new Point(270, 335),
                Size = new Size(220, 55),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnSaveReport.FlatAppearance.BorderSize = 0;
            btnSaveReport.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSaveReport.Width, btnSaveReport.Height, 12, 12));
            btnSaveReport.Click += BtnSaveReport_Click;

            this.Controls.AddRange(new Control[] { btnGenerateReport, btnSaveReport });

            // Modern Report Preview Group
            grpReportPreview = new GroupBox
            {
                Text = "Report Preview",
                Location = new Point(30, 410),
                Size = new Size(940, this.ClientSize.Height - 430),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            grpReportPreview.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, grpReportPreview.Width, grpReportPreview.Height, 15, 15));

            txtReportPreview = new TextBox
            {
                Location = new Point(20, 40),
                Size = new Size(900, grpReportPreview.Height - 60),
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                ReadOnly = true,
                Font = new Font("Consolas", 10),
                BackColor = Color.FromArgb(248, 250, 252),
                ForeColor = Color.FromArgb(30, 41, 59),
                WordWrap = false,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            grpReportPreview.Controls.Add(txtReportPreview);
            this.Controls.Add(grpReportPreview);
        }

        private void ReportType_CheckedChanged(object sender, EventArgs e)
        {
            grpDateRange.Enabled = rbCustom.Checked;
            
            if (rbWeekly.Checked)
            {
                dtpStartDate.Value = DateTime.Now.AddDays(-7);
                dtpEndDate.Value = DateTime.Now;
            }
            else if (rbMonthly.Checked)
            {
                dtpStartDate.Value = DateTime.Now.AddDays(-30);
                dtpEndDate.Value = DateTime.Now;
            }
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate;
                DateTime endDate;

                if (rbWeekly.Checked)
                {
                    startDate = DateTime.Now.AddDays(-7).Date;
                    endDate = DateTime.Now.Date;
                }
                else if (rbMonthly.Checked)
                {
                    startDate = DateTime.Now.AddDays(-30).Date;
                    endDate = DateTime.Now.Date;
                }
                else // Custom
                {
                    startDate = dtpStartDate.Value.Date;
                    endDate = dtpEndDate.Value.Date;

                    if (startDate > endDate)
                    {
                        MessageBox.Show("Start date cannot be after end date.", "Validation Error", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Generate report
                currentReportContent = taskManager.GenerateReport(startDate, endDate);
                txtReportPreview.Text = currentReportContent;
                btnSaveReport.Enabled = true;

                MessageBox.Show("Report generated successfully!", "Success", 
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentReportContent))
            {
                MessageBox.Show("Please generate a report first.", "No Report", 
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string fileName = $"StudyPlannerReport_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string filePath = taskManager.SaveReportToFile(currentReportContent, fileName);

                MessageBox.Show($"Report saved successfully!\n\nLocation: {filePath}", "Success", 
                              MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Ask if user wants to open the file
                var result = MessageBox.Show("Do you want to open the report file?", "Open Report", 
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("notepad.exe", filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving report: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
