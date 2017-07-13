using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator
{
    class HtmlToPdfParser
    {
        public static bool GeneratePdf(List<DocumentsWrapper> docsList, string outputPath)
        {
            string tmpFile = Path.GetTempFileName();
            generateSingleHtmlFile(docsList, tmpFile);
            toPdf(tmpFile, outputPath);
            return true;
        }

        public static void generateSingleHtmlFile(List<DocumentsWrapper> docsList, string outputFile)
        {
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                recursiveDocumentWriter(docsList, writer);
            }
            
        }

        private static void appendText(string inputFile, StreamWriter outputWriter)
        {
            using (StreamReader reader = new StreamReader(inputFile))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    outputWriter.WriteLine(line);
                }
            }
        }

        private static void recursiveDocumentWriter(List<DocumentsWrapper> docsList, StreamWriter writer)
        {
            foreach (DocumentsWrapper doc in docsList)
            {
                if (!doc.IsDirectory)
                {
                    appendText(doc.HtmlPath, writer);
                }
                else
                {
                    recursiveDocumentWriter(doc.SubDocuments, writer);
                }
            }
        }

        private static bool toPdf(string inputHtmlPath, string outputPdfPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "wkhtmltopdf.exe";
            startInfo.Arguments = "toc " + inputHtmlPath + " " + outputPdfPath;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();

            return true;
        }
    }
}
