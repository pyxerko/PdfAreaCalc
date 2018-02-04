// PdfAreaCalc 2018-02-04
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace PdfAreaCalcNs
{
    public enum PdfAreaType
    {
        BleedBox = 0,
        CropBox, // 1
        MediaBox,// 2
        TrimBox  // 3
    }

    /// <summary>
    /// Summary description for class PdfAreaCalc
    /// </summary>
    public static class PdfAreaCalc
    {
        /// <summary>
        /// Computes printing area of a pdf file
        /// </summary>
        /// <param name="fileNameWithPath"> </param>
        /// <param name="areaType"></param>
        /// <returns></returns>
        public static double GetFileAreaM2(string fileNameWithPath, PdfAreaType areaType)
        {
            using (StreamReader reader = File.OpenText(fileNameWithPath))
            {
                if (reader.EndOfStream)
                    return 0.0;

                return GetFileAreaM2(reader.ReadToEnd(), areaType.ToString());
            }
        }

        private static double GetFileAreaM2(string fileContent, string areaType)
        {
            const string numberpattern = @"\s*\[\s*(?<x0>\d*\.?\d*)\.?\d*\s+(?<y0>\d*\.?\d*)\s+(?<x1>\d*\.?\d*)\s+(?<y1>\d*\.?\d*\s*)\]";
            string pattern = "/" + areaType + numberpattern;
            double areaM2 = 0.0;

            Match match = Regex.Match(fileContent, pattern);
            while (match.Success)
            {
                if (match.Groups.Count >= 5)
                {
                    // match.Groups[0].Value == "/MediaBox\[1.0...2.1..2.2...3.3.]

                    // match.Groups[1].Value == "<x0>.........
                    double x0 = ConvertPtsToMeters(match.Groups[1].Value);

                    // match.Groups[2].Value == "....<y0>......
                    double y0 = ConvertPtsToMeters(match.Groups[2].Value);

                    // match.Groups[3].Value == "........<x1>.....
                    double x1 = ConvertPtsToMeters(match.Groups[3].Value);

                    // match.Groups[4].Value == "............<y1>.
                    double y1 = ConvertPtsToMeters(match.Groups[4].Value);

                    areaM2 += (x1 - x0) * (y1 - y0);
                }

                match = match.NextMatch();
            }

            return areaM2;
        }

        /// <summary>
        /// Converts Pts to m
        /// </summary>
        /// <param name="input" e.g "10.3"></param>
        /// <returns></returns>
        private static double ConvertPtsToMeters(string input)
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
    }
}
