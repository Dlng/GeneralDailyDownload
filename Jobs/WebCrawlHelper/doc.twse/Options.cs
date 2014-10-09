using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doc.twse
{
    public partial class Options : Form
    {
        string documentType;
        public string DocumentType
        {
            get { return documentType; }
        }

        List<string> companyIdList;
        public List<string> CompanyIdList
        {
            get { return companyIdList; }
        }

        List<string> monthsList;
        public List<string> MonthsList
        {
            get { return monthsList; }
        }

        int year;
        public int Year
        {
            get { return year; }
        }

        string downloadFolder;
        public string DownloadFolder
        {
            get { return downloadFolder; }
        }

        public Options(Dictionary<string, string> CompanyList)
        {
            InitializeComponent();

            radioButtonProspectus.Checked = true;

            companyIdList = new List<string>();
            monthsList = new List<string>();

            string selectedcompany = Properties.Settings.Default.SelectedCompany;
            foreach (string id in CompanyList.Keys)
            {
                string name = string.Format("{0}-{1}", id, CompanyList[id]);
                int idx = checkedListBoxCompany.Items.Add(name);

                if (!string.IsNullOrEmpty(selectedcompany) && selectedcompany.IndexOf(id) > -1)
                {
                    checkedListBoxCompany.SetItemChecked(idx, true);
                }
            }

            //checked last month
            DateTime dt = DateTime.Now.AddMonths(-1);
            checkedListBoxMonths.SetItemChecked(dt.Month - 1, true);

            textBoxYear.Text = dt.Year.ToString();

            textBoxDirectory.Text = Properties.Settings.Default.DownloadFolder;

        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            if (radioButtonProspectus.Checked)
                documentType = "E";
            else
                documentType = "D";

            string selectedCompany = "";
            companyIdList.Clear();
            foreach (object itemChecked in checkedListBoxCompany.CheckedItems)
            {
                string[] id = itemChecked.ToString().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                companyIdList.Add(id[0]);
                selectedCompany += id[0] + ";";
            }
            Properties.Settings.Default.DownloadFolder = textBoxDirectory.Text;


            monthsList.Clear();
            foreach (int itemIdx in checkedListBoxMonths.CheckedIndices)
            {
                int month = itemIdx + 1;
                monthsList.Add(month < 10 ? "0" + month.ToString() : month.ToString());
            }

            int y;
            if (!int.TryParse(textBoxYear.Text, out y))
            {
                textBoxYear.Focus();
                textBoxYear.SelectAll();
                MessageBox.Show("Invalid Year.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                year = y;
            }

            if (string.IsNullOrEmpty(textBoxDirectory.Text))
            {
                textBoxDirectory.Focus();
                textBoxDirectory.SelectAll();
                MessageBox.Show("Please fill up Download To.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                downloadFolder = textBoxDirectory.Text;
            }

            Properties.Settings.Default.DownloadFolder = textBoxDirectory.Text;
            Properties.Settings.Default.SelectedCompany = selectedCompany;
            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonBrowseDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == DialogResult.OK)
            {
                textBoxDirectory.Text = fb.SelectedPath;
            }
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // Options
        //    // 
        //    this.ClientSize = new System.Drawing.Size(545, 485);
        //    this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //    this.Name = "Options";
        //    this.Text = "Download Options";
        //    this.ResumeLayout(false);

        //}
    }
}
