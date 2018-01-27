using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace PdfAreaNs
{
    /// <summary>
    /// Summary description for class PdfAreaCalc
    /// </summary>
    public static class PdfAreaCalc
    {
        public enum PdfAreaType
        {
            BleedBox = 0,
            CropBox, // 1
            MediaBox,// 2
            TrimBox  // 3
        }

        public static string GetAreaTypeString(PdfAreaType areaType)
        {
            switch (areaType)
            {
                case PdfAreaType.BleedBox:
                    return "/BleedBox";
                case PdfAreaType.CropBox:
                    return "/CropBox";
                case PdfAreaType.TrimBox:
                    return "/TrimBox";
                default:
                    return "/MediaBox";
            }
        }

        /// <summary>
        /// Converts Pts to m
        /// </summary>
        /// <param name="input" e.g "10.3"></param>
        /// <returns></returns>
        private static double ConvertPts2Meters(string input)
        {
            // Pdf files uses coordinate in points i.e 1/72 inch, 
            //    some times as floating point, some times as integers.
            // When floating point: always w decimal separator point, not decimal comma i.e 10.03, NOT 10,03
            if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                //			72 pts/inch 2.54 cm/100cm -> m
                return value / 72.0 * (2.54 / 100.0);
            }

            return 0.0;
        }

        private static double GetFileAreaM2(string fileName, string areaType)
        {
            const string numberPattern = @"\s*\[\s*(?<X0>\d*\.?\d*)\.?\d*\s+(?<Y0>\d*\.?\d*)\s+(?<X1>\d*\.?\d*)\s+(?<Y1>\d*\.?\d*\s*)\]";
            string pattern = areaType + numberPattern;
            double areaM2 = 0.0;

            using (StreamReader reader = File.OpenText(fileName))
            {
                if (reader.EndOfStream)
                    return 0.0;

                string s = reader.ReadToEnd();

                Match match = Regex.Match(s, pattern);
                while (match.Success)
                {
                    if (match.Groups.Count == 5)
                    {
                        // match.Groups[0].Value == "/MediaBox\[.....

                        // match.Groups[1].Value == ".....<x0>.....
                        double x0 = ConvertPts2Meters(match.Groups[1].Value);

                        // match.Groups[2].Value == ".....<y0>.....
                        double y0 = ConvertPts2Meters(match.Groups[2].Value);

                        // match.Groups[3].Value == ".....<x1>.....
                        double x1 = ConvertPts2Meters(match.Groups[3].Value);

                        // match.Groups[4].Value == ".....<y1>.....
                        double y1 = ConvertPts2Meters(match.Groups[4].Value);

                        areaM2 += (x1 - x0) * (y1 - y0);
                    }

                    match = match.NextMatch();
                }
            }

            return areaM2;
        }

        /// <summary>
        /// Computes printing area of a pdf file
        /// </summary>
        /// <param name="fileName"> </param>
        /// <param name="areaType"></param>
        /// <returns></returns>
        public static double GetFileAreaM2(string fileName, PdfAreaType areaType)
        {
            return GetFileAreaM2(fileName, GetAreaTypeString(areaType));
        }

        //static void dret()
        //{
        //    DataTable dt = new DataTable();
        //    DataRow dr;
        //}
    }
}
