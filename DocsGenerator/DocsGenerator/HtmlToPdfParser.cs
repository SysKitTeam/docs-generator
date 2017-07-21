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
        public bool GeneratePdf(List<DocumentsWrapper> docsList, string outputPath, string tmpDirPath, string documentTitle)
        {
            string tmpFile = tmpDirPath + "ALL.html";
            copyCSSfile(tmpDirPath);
            generateSingleHtmlFile(docsList, tmpFile);
            toPdf(tmpFile, outputPath, documentTitle);
            return true;
        }

        public void generateSingleHtmlFile(List<DocumentsWrapper> docsList, string outputFile)
        {
            using (StreamWriter writer = new StreamWriter(outputFile, false, Encoding.UTF8))
            {
                writer.WriteLine("<head>");
                writer.WriteLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"./style.css\" >");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                recursiveDocumentWriter(docsList, writer, 1);
                writer.WriteLine("</body>");
            }
            
        }

        private void copyCSSfile(string outputPath)
        {
            File.Copy(".\\style.css", outputPath + "style.css");
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

        private bool toPdf(string inputHtmlPath, string outputPdfPath, string documentTitle)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "wkhtmltopdf.exe";
            startInfo.Arguments = "--header-left " + documentTitle + " " +
                                  "--header-right [page]/[topage] " +
                                  "--header-line " +
                                  "--header-spacing 2 " +
                                  "toc " + 
                                  inputHtmlPath + " " + 
                                  outputPdfPath;
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
