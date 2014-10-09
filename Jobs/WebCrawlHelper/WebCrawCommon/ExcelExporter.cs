using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using OfficeOpenXml;
using System.Windows.Forms;
using System.IO;


namespace WebCrawCommon
{
    public class ExcelExporter
    {
        public ExcelExporter()
        {
        }

        public bool Export(DataTable dt, string fullPath, string sheetName = null)
        {

            FileInfo file = new FileInfo(fullPath);
            ExcelPackage excel = new ExcelPackage(file);
            ExcelWorksheet sheet = excel.Workbook.Worksheets.Add(sheetName);

            sheet.Cells.Style.Font.Name = "Verdana";
            sheet.Cells.Style.Font.Size = 10;
            sheet.Cells.Style.Font.Bold = false;
            sheet.Row(1).Height *= 2;

            int columnIndex = 1;
            foreach (DataColumn column in dt.Columns)
            {
                //sheet.Column(columnIndex).Width = column.ColumnName.Length * 400;
                sheet.Cells[1, columnIndex].Value = column.ColumnName;
                sheet.Cells[1, columnIndex].Style.Font.Bold = true;
                sheet.Cells[1, columnIndex].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;

                columnIndex++;
            }

            int rowIndex = 2;
            foreach (DataRow dataRow in dt.Rows)
            {
                //DateTime changedOn = dataRow.Field<DateTime>("LastChangedOn");
                //if (changedOn > ScanedOn)
                {
                    int colIndex = 1;
                    foreach (DataColumn column in dt.Columns)
                    {
                        object dbVal = dataRow[column];

                        string columnType = column.DataType.Name.ToLower();
                        if (columnType == "datetime")
                        {
                            //sheet.Cells[rowIndex, colIndex].Value = Common.ConvertDateToString(Convert.ToDateTime(dbVal));
                        }
                        else if (columnType == "boolean")
                            sheet.Cells[rowIndex, colIndex].Value = Convert.ToBoolean(dbVal) ? "Yes" : "No";
                        else
                            sheet.Cells[rowIndex, colIndex].Value = dbVal;

                        switch (column.ColumnName)
                        {
                            case "DocType":
                                //sheet.Cells[rowIndex, colIndex].Value = dataRow.IsNull("DocType") ? "" : ((DocType)dataRow.Field<int>("DocType")).ToString();
                                break;
                            case "LastChange":
                                //sheet.Cells[rowIndex, colIndex].Value = (ElementStatus)(Int16)dataRow["LastChange"];
                                break;
                        }

                        colIndex++;
                    }
                    rowIndex++;
                }
            }

            //MemoryStream ms = new MemoryStream();
            //excel.SaveAs(ms);

            excel.Save();
            //return fullPath;
            return false;
        }

        public bool ExportGrid(DataGridView grid, string fullPath, string sheetName = null)
        {
            return false;
        }

        public bool Export(DataSet ds, string fullPath)
        {
            return false;
        }

    }
}
