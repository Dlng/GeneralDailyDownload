namespace doc.twse
{
    partial class Options
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButtonProspectus = new System.Windows.Forms.RadioButton();
            this.radioButtonAnnualReport = new System.Windows.Forms.RadioButton();
            this.checkedListBoxCompany = new System.Windows.Forms.CheckedListBox();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.checkedListBoxMonths = new System.Windows.Forms.CheckedListBox();
            this.textBoxDirectory = new System.Windows.Forms.TextBox();
            this.buttonBrowseDir = new System.Windows.Forms.Button();
            this.buttonGo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Document Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Company List:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(76, 219);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Year:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(26, 244);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Months Filter:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 393);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Download To:";
            // 
            // radioButtonProspectus
            // 
            this.radioButtonProspectus.AutoSize = true;
            this.radioButtonProspectus.Location = new System.Drawing.Point(120, 22);
            this.radioButtonProspectus.Name = "radioButtonProspectus";
            this.radioButtonProspectus.Size = new System.Drawing.Size(92, 20);
            this.radioButtonProspectus.TabIndex = 1;
            this.radioButtonProspectus.TabStop = true;
            this.radioButtonProspectus.Text = "Prospectus";
            this.radioButtonProspectus.UseVisualStyleBackColor = true;
            // 
            // radioButtonAnnualReport
            // 
            this.radioButtonAnnualReport.AutoSize = true;
            this.radioButtonAnnualReport.Location = new System.Drawing.Point(120, 44);
            this.radioButtonAnnualReport.Name = "radioButtonAnnualReport";
            this.radioButtonAnnualReport.Size = new System.Drawing.Size(108, 20);
            this.radioButtonAnnualReport.TabIndex = 1;
            this.radioButtonAnnualReport.TabStop = true;
            this.radioButtonAnnualReport.Text = "Annual Report";
            this.radioButtonAnnualReport.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxCompany
            // 
            this.checkedListBoxCompany.FormattingEnabled = true;
            this.checkedListBoxCompany.Location = new System.Drawing.Point(120, 70);
            this.checkedListBoxCompany.Name = "checkedListBoxCompany";
            this.checkedListBoxCompany.Size = new System.Drawing.Size(251, 140);
            this.checkedListBoxCompany.TabIndex = 2;
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(120, 216);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(100, 22);
            this.textBoxYear.TabIndex = 3;
            // 
            // checkedListBoxMonths
            // 
            this.checkedListBoxMonths.FormattingEnabled = true;
            this.checkedListBoxMonths.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.checkedListBoxMonths.Location = new System.Drawing.Point(120, 244);
            this.checkedListBoxMonths.Name = "checkedListBoxMonths";
            this.checkedListBoxMonths.Size = new System.Drawing.Size(251, 140);
            this.checkedListBoxMonths.TabIndex = 4;
            // 
            // textBoxDirectory
            // 
            this.textBoxDirectory.Location = new System.Drawing.Point(120, 390);
            this.textBoxDirectory.Name = "textBoxDirectory";
            this.textBoxDirectory.Size = new System.Drawing.Size(215, 22);
            this.textBoxDirectory.TabIndex = 5;
            // 
            // buttonBrowseDir
            // 
            this.buttonBrowseDir.Location = new System.Drawing.Point(341, 390);
            this.buttonBrowseDir.Name = "buttonBrowseDir";
            this.buttonBrowseDir.Size = new System.Drawing.Size(30, 23);
            this.buttonBrowseDir.TabIndex = 6;
            this.buttonBrowseDir.Text = "...";
            this.buttonBrowseDir.UseVisualStyleBackColor = true;
            this.buttonBrowseDir.Click += new System.EventHandler(this.buttonBrowseDir_Click);
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(120, 421);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(150, 31);
            this.buttonGo.TabIndex = 7;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // Options2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 465);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.buttonBrowseDir);
            this.Controls.Add(this.textBoxDirectory);
            this.Controls.Add(this.checkedListBoxMonths);
            this.Controls.Add(this.textBoxYear);
            this.Controls.Add(this.checkedListBoxCompany);
            this.Controls.Add(this.radioButtonAnnualReport);
            this.Controls.Add(this.radioButtonProspectus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Download Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButtonProspectus;
        private System.Windows.Forms.RadioButton radioButtonAnnualReport;
        private System.Windows.Forms.CheckedListBox checkedListBoxCompany;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.CheckedListBox checkedListBoxMonths;
        private System.Windows.Forms.TextBox textBoxDirectory;
        private System.Windows.Forms.Button buttonBrowseDir;
        private System.Windows.Forms.Button buttonGo;
    }
}