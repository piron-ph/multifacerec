namespace MultiFaceRec
{
    partial class Report_LB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.crystalReportViewer01 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.CrystalReport11 = new MultiFaceRec.CrystalReport1();
            this.SuspendLayout();
            // 
            // crystalReportViewer01
            // 
            this.crystalReportViewer01.ActiveViewIndex = 0;
            this.crystalReportViewer01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer01.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer01.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer01.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer01.Name = "crystalReportViewer01";
            this.crystalReportViewer01.ReportSource = this.CrystalReport11;
            this.crystalReportViewer01.Size = new System.Drawing.Size(712, 474);
            this.crystalReportViewer01.TabIndex = 0;
            // 
            // Report_LB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 474);
            this.Controls.Add(this.crystalReportViewer01);
            this.Name = "Report_LB";
            this.Text = "Report_LB";
            this.Load += new System.EventHandler(this.Report_LB_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer01;
        private CrystalReport1 CrystalReport11;
    }
}