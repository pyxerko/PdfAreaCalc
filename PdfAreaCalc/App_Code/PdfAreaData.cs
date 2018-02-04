using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PdfAreaData
/// </summary>
public class PdfAreaData
{
    public static DataTable GetTable()
    {
        // Here we create a DataTable with four columns.
        DataTable table = new DataTable();
        table.Columns.Add("FileName", typeof(string));
        table.Columns.Add("AreaM2", typeof(string));
        table.Columns.Add("DateTime", typeof(DateTime));

        return table;
    }
}