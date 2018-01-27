// Erik Koch-Nommensen 2018-01-14
using System;
using System.Configuration;
using System.Text;
using System.Web.Configuration;
using static PdfAreaNs.PdfAreaCalc;

public partial class PdfAreaCalc_PdfAreaCalc : System.Web.UI.Page
{
    private string DefaultPdfDirectory
    {
        get
        {
            return WebConfigurationManager.AppSettings["defaultPdfDirectory"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DirectoryTextBox.Text = DefaultPdfDirectory;
        }
    }

    protected void InvokeButton_Click(object sender, EventArgs e)
    {
        try
        {
            ResultTextBox.Text = string.Empty;

            if (!int.TryParse(AreaTypeDropDownList.SelectedValue, out int areaType))
                areaType = (int)PdfAreaType.MediaBox;

            string result = GetAreasText(DirectoryTextBox.Text, (PdfAreaType)areaType);

            if (result != string.Empty)
            {
                ResultTextBox.Text = result;
            }
            else
            {
                ResultTextBox.Text = "Inga filer hittades, eller inga areor";
            }

        }
        catch (Exception ex)
        {
            ResultTextBox.Text = "Ett fel inträffade: " + ex.Message;
        }
    }

    protected string GetAreasText(string directory, PdfAreaType areaType)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        string[] fileNames = System.IO.Directory.GetFiles(directory, @"*.pdf");
        double sumArea = 0.0;
        StringBuilder builder = new StringBuilder();

        foreach (string fileName in fileNames)
        {
            double fileArea = GetFileAreaM2(fileName, areaType);
            sumArea += fileArea;
            builder.Append(string.Format("{0}: File: {1},\t Area: {2:0.00} m2\r\n",
                GetAreaTypeString(areaType), fileName.Replace(directory + @"\", ""), fileArea));
        }
        builder.Append(string.Format("Directory {0},\t Sum Area: {1:0.00} m2\r\n", directory, sumArea));

        watch.Stop();
        builder.Append(string.Format("{0} files,\t used {1} millisecs\r\n", fileNames.Length, watch.ElapsedMilliseconds));

        return builder.ToString();
    }
}