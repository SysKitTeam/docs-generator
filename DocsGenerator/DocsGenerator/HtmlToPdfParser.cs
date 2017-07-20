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
        public bool GeneratePdf(List<DocumentsWrapper> docsList, string outputPath, string tmpDirPath)
        {
            string tmpFile = tmpDirPath + "ALL.html";
            generateSingleHtmlFile(docsList, tmpFile);
            toPdf(tmpFile, outputPath);
            return true;
        }

        public void generateSingleHtmlFile(List<DocumentsWrapper> docsList, string outputFile)
        {
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.WriteLine("<font face=\"Helvetica\" size=\"6\">");
                recursiveDocumentWriter(docsList, writer, 1);
                writer.WriteLine("</font>");
            }
            
        }

        private void appendText(string inputFile, StreamWriter outputWriter, int level)
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

        private void appendDirectoryTitle(DocumentsWrapper doc, StreamWriter writer, int level)
        {
            string line = "<h" + level + "><font face=\"Arial\" size=\"10\" color=\"#eca11d\">" + doc.Title + "</font></h" + level + ">";
            writer.WriteLine(line);
        }

        private void recursiveDocumentWriter(List<DocumentsWrapper> docsList, StreamWriter writer, int level)
        {
            foreach (DocumentsWrapper doc in docsList)
            {
                if (!doc.IsDirectory)
                {
                    appendText(doc.HtmlPath, writer, level);
                }
                else
                {
                    appendDirectoryTitle(doc, writer, level);
                    recursiveDocumentWriter(doc.SubDocuments, writer, level + 1);
                }
            }
        }

        private bool toPdf(string inputHtmlPath, string outputPdfPath)
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
