using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;

using doc.twse.DownloadPageService;
using Microsoft.Office.Interop.Excel;

namespace doc.twse
{
    public partial class DocDownloadWnd : Form
    {
        Dictionary<string, string> _documentTypeList = new Dictionary<string, string>();
        Dictionary<string, string> _companyList = new Dictionary<string, string>();

        DownloadTaskHandler _downloadingHandler;

        public DocDownloadWnd()
        {
            InitializeComponent();

            _downloadingHandler = new DownloadTaskHandler(this);

            _companyList = new Dictionary<string, string>();
            string companies = Properties.Settings.Default.CompanyList;

            string[] companylist = companies.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string comp in companylist)
            {
                string[] company = comp.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (company.Length == 2)
                {
                    _companyList.Add(company[0], company[1]);
                }
            }

            _documentTypeList = new Dictionary<string, string>();
            _documentTypeList.Add("E", "Prospectus");
            _documentTypeList.Add("D", "Annual Report");

            listViewDocuments.VirtualMode = true;
            listViewDocuments.VirtualListSize = 0;

            timerRefresh.Interval = 7500;
            timerRefresh.Enabled = false;
            this.Width = 1024;

        }

        private void toolStripButtonDownload_Click(object sender, EventArgs e)
        {
            Options optionWnd = new Options(_companyList);
            if (optionWnd.ShowDialog(this) == DialogResult.OK)
            {
                _downloadingHandler.Clear();
                listViewDocuments.VirtualListSize = 0;
                this.Refresh();

                int Year = optionWnd.Year;
                string docType = optionWnd.DocumentType;

                if (!Directory.Exists(optionWnd.DownloadFolder))
                    Directory.CreateDirectory(optionWnd.DownloadFolder);
                _downloadingHandler.DownloadPath = optionWnd.DownloadFolder;

                string url = "http://doc.twse.com.tw/server-java/t57sb01?step=1&colorchg=1&co_id={0}&year={1}&month=&mtype={2}&";

                Cursor oldCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                timerRefresh.Enabled = true;

                int taskId = 0;
                foreach (string company in optionWnd.CompanyIdList)
                {
                    int year = Year - 1911;
                    string pageUrl = string.Format(url, company, year, docType);

                    DocPageTask task = new DocPageTask(taskId, docType, pageUrl, company, year,
                                                        optionWnd.MonthsList, _downloadingHandler.DownloadPath, _downloadingHandler);
                    if (task.ScanDocument() > 0)
                    {
                        listViewDocuments.VirtualListSize = _downloadingHandler.DocumentsList.Count;
                        this.Refresh();
                    }
                }

                _downloadingHandler.TryStart();

                this.Cursor = oldCursor;
            }
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            this.Refresh();

            if (_downloadingHandler != null)
            {
                listViewDocuments.VirtualListSize = _downloadingHandler.DocumentsList.Count;
                if (_downloadingHandler.IsAllDone())
                {
                    timerRefresh.Enabled = false;
                    MessageBox.Show(this, "All documents downloaded.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void listViewDocuments_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (_downloadingHandler.DocumentsList.Count > 0)
            {
                DocDownloadTask task = _downloadingHandler.DocumentsList[e.ItemIndex];
                int nNumber = e.ItemIndex + 1;
                ListViewItem lv = new ListViewItem(nNumber.ToString());
                lv.SubItems.Add(task.DocId);
                lv.SubItems.Add(task.FundName);
                if (task.Status == DocDownloadStatus.Downloading)
                    lv.SubItems.Add(task.Percentage);
                else
                    lv.SubItems.Add(task.Status.ToString());
                lv.SubItems.Add(_documentTypeList[task.DocType]);
                lv.SubItems.Add(string.Format("{0}-{1}", task.CompanyId, _companyList[task.CompanyId]));
                //if (task.DownloadFailedTimes > 0)
                //    lv.SubItems.Add(task.DownloadFailedTimes.ToString());
                //else
                //    lv.SubItems.Add("");
                lv.SubItems.Add(task.DocUrl);

                e.Item = lv;
            }
        }

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDlg = new SaveFileDialog();
            fileDlg.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            fileDlg.FilterIndex = 1;
            fileDlg.RestoreDirectory = true;

            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                string filepath = fileDlg.FileName;

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                try
                {
                    app.Visible = true;
                    Workbook wBook = app.Workbooks.Add(true);
                    Worksheet wSheet = wBook.Worksheets[1] as Worksheet;

                    wSheet.Cells[1, 1] = "Document Name";
                    wSheet.Cells[1, 2] = "Document Type";
                    wSheet.Cells[1, 3] = "Fund Name";

                    int Row = 2;
                    foreach (DocDownloadTask task in _downloadingHandler.DocumentsList)
                    {
                        wSheet.Cells[Row, 1] = task.DocId;
                        wSheet.Cells[Row, 2] = _documentTypeList[task.DocType];
                        wSheet.Cells[Row, 3] = task.FundName;
                        Row++;
                    }

                    app.DisplayAlerts = false;
                    app.AlertBeforeOverwriting = false;
                    wBook.Saved = true;
                    wBook.SaveAs(filepath);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripButtonOpenFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_downloadingHandler.DownloadPath))
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                psi.Arguments = "/e," + _downloadingHandler.DownloadPath;
                System.Diagnostics.Process.Start(psi);
            }
        }

    }
}
