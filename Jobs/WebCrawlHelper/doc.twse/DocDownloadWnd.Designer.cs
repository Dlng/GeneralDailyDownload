namespace doc.twse
{
    partial class DocDownloadWnd
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocDownloadWnd));
            this.toolStripBar = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDownload = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpenFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExport = new System.Windows.Forms.ToolStripButton();
            this.listViewDocuments = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.toolStripBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripBar
            // 
            this.toolStripBar.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDownload,
            this.toolStripButtonOpenFolder,
            this.toolStripButtonExport});
            this.toolStripBar.Location = new System.Drawing.Point(0, 0);
            this.toolStripBar.Name = "toolStripBar";
            this.toolStripBar.Size = new System.Drawing.Size(748, 25);
            this.toolStripBar.TabIndex = 0;
            this.toolStripBar.Text = "toolStrip1";
            // 
            // toolStripButtonDownload
            // 
            this.toolStripButtonDownload.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDownload.Image")));
            this.toolStripButtonDownload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDownload.Name = "toolStripButtonDownload";
            this.toolStripButtonDownload.Size = new System.Drawing.Size(92, 22);
            this.toolStripButtonDownload.Text = "Download...";
            this.toolStripButtonDownload.Click += new System.EventHandler(this.toolStripButtonDownload_Click);
            // 
            // toolStripButtonOpenFolder
            // 
            this.toolStripButtonOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpenFolder.Image")));
            this.toolStripButtonOpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenFolder.Name = "toolStripButtonOpenFolder";
            this.toolStripButtonOpenFolder.Size = new System.Drawing.Size(99, 22);
            this.toolStripButtonOpenFolder.Text = "Open Folder";
            this.toolStripButtonOpenFolder.Click += new System.EventHandler(this.toolStripButtonOpenFolder_Click);
            // 
            // toolStripButtonExport
            // 
            this.toolStripButtonExport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExport.Image")));
            this.toolStripButtonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExport.Name = "toolStripButtonExport";
            this.toolStripButtonExport.Size = new System.Drawing.Size(91, 22);
            this.toolStripButtonExport.Text = "Export List";
            this.toolStripButtonExport.Click += new System.EventHandler(this.toolStripButtonExport_Click);
            // 
            // listViewDocuments
            // 
            this.listViewDocuments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader8});
            this.listViewDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDocuments.FullRowSelect = true;
            this.listViewDocuments.GridLines = true;
            this.listViewDocuments.Location = new System.Drawing.Point(0, 25);
            this.listViewDocuments.Name = "listViewDocuments";
            this.listViewDocuments.Size = new System.Drawing.Size(748, 492);
            this.listViewDocuments.TabIndex = 1;
            this.listViewDocuments.UseCompatibleStateImageBehavior = false;
            this.listViewDocuments.View = System.Windows.Forms.View.Details;
            this.listViewDocuments.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listViewDocuments_RetrieveVirtualItem);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "    #";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "File Name";
            this.columnHeader2.Width = 180;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Fund Name";
            this.columnHeader3.Width = 180;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Status";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Type";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Company";
            this.columnHeader6.Width = 120;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "URL";
            this.columnHeader8.Width = 480;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // DocDownloadWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 517);
            this.Controls.Add(this.listViewDocuments);
            this.Controls.Add(this.toolStripBar);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DocDownloadWnd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "doc.twse - download helper";
            this.toolStripBar.ResumeLayout(false);
            this.toolStripBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripBar;
        private System.Windows.Forms.ToolStripButton toolStripButtonDownload;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenFolder;
        private System.Windows.Forms.ToolStripButton toolStripButtonExport;
        private System.Windows.Forms.ListView listViewDocuments;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Timer timerRefresh;
    }
}