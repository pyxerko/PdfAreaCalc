using PdfAreaCalcCommon;
using System.Collections.Generic;
using System.IO;

namespace PdfAreraCalcConsoleApp
{
    class Program
    {
        private static PdfAreaType areaType = PdfAreaType.MediaBox;

        static void Main(string[] args)
        {
            List<string> allFileNames = new List<string>();

            foreach (string arg in args)
            {
                if (arg.StartsWith("/") || arg.StartsWith("/-"))
                {
                    DecodeOneArg(arg);
                }
                else
                {
                    allFileNames.AddRange( GetFileNamesFromOneArg(arg));
                }
            }

            foreach(string fileName in allFileNames)
            {
                HandleOneFile(fileName, areaType);
            }
        }

        private static void HandleOneFile(string fileName, PdfAreaType areaType)
        {
            double area = PdfAreaCalc.GetFileAreaM2(fileName, areaType);
            System.Console.WriteLine("{0}\t{1}", fileName, area.ToString("#.##"));
        }

        private static List<string> GetFileNamesFromOneArg(string pathArg)
        {
            List<string> result = new List<string>();

            if (File.Exists(pathArg))
                result.Add(pathArg);
            else if (Directory.Exists(pathArg))
            {
                string[] fileNames = Directory.GetFiles(pathArg, @"*.pdf");
                foreach (string fileName in fileNames)
                    result.Add(fileName);
            }
            return result;
        }

        private static void DecodeOneArg(string arg)
        {
            switch (arg.TrimStart().Substring(1,1).ToUpper())
            {
                case "B":
                    areaType = PdfAreaType.BleedBox;
                    break;
                case "C":
                    areaType = PdfAreaType.CropBox;
                    break;
                case "T":
                    areaType = PdfAreaType.TrimBox;
                    break;
                default:
                    areaType = PdfAreaType.MediaBox;
                    break;
            }
        }
    }
}
