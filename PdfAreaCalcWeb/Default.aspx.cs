// Erik Koch-Nommensen 2018-01-14
using PdfAreaCalcCommon;
using System;
using System.Text;
using System.Web.Configuration;
using System.Web.UI.WebControls;

public partial class PdfAreaCalcPage : System.Web.UI.Page
{
    protected bool UseGrid { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UseGridCheckBox.Checked = UseGrid = (WebConfigurationManager.AppSettings["UseGrid"]) == "true";
            DirectoryTextBox.Text = WebConfigurationManager.AppSettings["defaultPdfDirectory"];
        }
        CurrentDtLabel.Text = DateTime.Now.ToString();
        UseGrid = UseGridCheckBox.Checked;
    }

    protected void InvokeButton_Click(object sender, EventArgs e)
    {
        try
        {
            Tuple<bool, string> prepResult = CheckPathsAndPrepareArgs(out int areaType, out string directory, out string fileMask);

            if (!prepResult.Item1)
            {
                ResultTextBox.Visible = true;
                ResultTextBox.Text = prepResult.Item2;
                return;
            }

            if (UseGrid)
            {
                ResultTable.Visible = true;
                ResultTextBox.Visible = false;
                GetAreasData(directory, (PdfAreaType)areaType, fileMask);
            }
            else
            {
                ResultTextBox.Text = string.Empty;
                ResultTable.Visible = false;
                ResultTextBox.Visible = true;
                ResultTextBox.Text = GetAreasText(directory, (PdfAreaType)areaType, fileMask);
            }
        }
        catch (Exception ex)
        {
            ResultTextBox.Text = "Ett fel inträffade: " + ex.Message;
        }
    }

    protected string GetAreasText(string directory, PdfAreaType areaType, string fileMask)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        string[] fileNames = System.IO.Directory.GetFiles(directory,
            searchPattern: string.IsNullOrEmpty(fileMask) ? @"*.pdf" : fileMask);

        double sumArea = 0.0;

        StringBuilder builder = new StringBuilder();
        foreach (string fileName in fileNames)
        {
            double fileArea = PdfAreaCalcCommon.PdfAreaCalc.GetFileAreaM2(fileName, areaType);
            sumArea += fileArea;
            builder.Append(string.Format("{0}: File: {1},\t Area: {2:0.00} m2\r\n",
                areaType.ToString(), fileName.Replace(directory + @"\", ""), fileArea));
        }
        builder.Append(string.Format("Directory {0},\t Sum Area: {1:0.00} m2\r\n", directory, sumArea));

        watch.Stop();
        builder.Append(string.Format("{0} files,\t used {1} millisecs\r\n", fileNames.Length, watch.ElapsedMilliseconds));

        return builder.ToString();
    }

    protected void GetAreasData(string directory, PdfAreaType areaType, string fileMask)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        ResultTable.Rows.Clear();

        string[] fileNames = System.IO.Directory.GetFiles(directory,
            searchPattern: string.IsNullOrEmpty(fileMask) ? @"*.pdf" : fileMask);
        double sumArea = 0.0;
        Int64 sumFileSize = 0;

        // Header row
        ResultTable.Controls.Add(AddRowCells(
            new string[] { "Areatyp", "Fil", "Area m2", "Size KB", "Modifierad" }, bold: true));

        foreach (string fileName in fileNames)
        {
            double fileArea = PdfAreaCalcCommon.PdfAreaCalc.GetFileAreaM2(fileName, areaType);
            DateTime modified = System.IO.File.GetLastWriteTime(fileName);
            System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
            Int64 fileSize = fi.Length;

            TableRow tRow = AddRowCells(new string[] {
                areaType.ToString(),
                fileName.Replace(directory + @"\", ""),
                fileArea.ToString("#.##"),
                (fileSize/1000.0).ToString("0.00"),
                modified.ToString("yyyy-MM-dd") });

            ResultTable.Controls.Add(tRow);

            sumArea += fileArea;
            sumFileSize += fileSize;
        }

        watch.Stop();

        // Totals row
        ResultTable.Controls.Add(AddRowCells(new string[] { "",
            string.Format("Total {0} fil(er),&nbsp&nbsp&nbsp&nbsp förbrukade {1} ms ",
            fileNames.Length, watch.ElapsedMilliseconds),
            sumArea.ToString("0.00"),
            (sumFileSize/1000.0).ToString("0.00"),"" }, true));

    }

    private TableRow AddRowCells(string[] items, bool bold = false)
    {
        TableRow tRow = new TableRow();
        foreach (string item in items)
        {
            TableCell tCell = new TableCell { Text = "&nbsp" + item + "&nbsp" };
            if (double.TryParse(item, out double temp))
                tCell.HorizontalAlign = HorizontalAlign.Right;
            tRow.Cells.Add(tCell);
        }
        tRow.Font.Bold = bold;
        return tRow;
    }

    private Tuple<bool, string> CheckPathsAndPrepareArgs(out int areaType,
        out string directory, out string fileMask)
    {
        if (!int.TryParse(AreaTypeDropDownList.SelectedValue, out areaType))
            areaType = (int)PdfAreaType.MediaBox;

        directory = DirectoryTextBox.Text.Trim();
        fileMask = FileMaskTextBox.Text.Trim();

        bool okSoFar = System.IO.Directory.Exists(directory);
        if (okSoFar)
        {
            string[] fileNames = System.IO.Directory.GetFiles(directory,
                string.IsNullOrEmpty(fileMask) ? @"*.pdf" : fileMask);
            okSoFar = fileNames.Length > 0;
        }
        if (okSoFar)
            return Tuple.Create(true, "OK");
        else
            return Tuple.Create(false, "Inga filer hittades !");
    }

//    protected void CopyGridButton_Click(object sender, EventArgs e)
//    {
//        StringBuilder builder = new StringBuilder();
//        foreach (TableRow row in ResultTable.Rows)
//        {
//            foreach (TableCell cell in row.Cells)
//            {
//                builder.Append(cell.Text + "\t");
//            }
//            builder.Append("\r\n");
//        }
//        System.Windows.Clipboard.SetText(builder.ToString());
//    }
}