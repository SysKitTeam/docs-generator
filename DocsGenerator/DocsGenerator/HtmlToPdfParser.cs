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
        /// <summary>
        /// Generates a single pdf file with default header, footer and cover, along with the default css file.
        /// </summary>
        /// <param name="docsList">Recursive list of documents that will be in the pdf document.</param>
        /// <param name="outputPath">Full path of the pdf to be created.</param>
        /// <param name="tmpDirPath">Path of a temporary directory for mid-calculation file storage.</param>
        /// <param name="documentTitle">Title of the document to be created.</param>
        /// <returns>True if pdf generation was successfull, false otherwise.</returns>
        public bool GeneratePdf(List<DocumentsWrapper> docsList, string outputPath, string tmpDirPath, string documentTitle)
        {
            string tmpFile = tmpDirPath + "ALL.html";

            copyCSSfile(tmpDirPath);
            string headerPath = copyHeaderFile(tmpDirPath);
            string footerPath = copyFooterFile(tmpDirPath);
            string coverPath = copyAndEditCoverFile(tmpDirPath, documentTitle, DateTime.Now);

            generateSingleHtmlFile(docsList, tmpFile);
            
            return toPdf(tmpFile, outputPath, headerPath, footerPath, coverPath); ;
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

        /// <summary>
        /// Creates a copy of project style.css file to the given output path.
        /// </summary>
        /// <param name="outputPath">Destination directory path.</param>
        /// <returns>Full file path of the new style.css file.</returns>
        private string copyCSSfile(string outputPath)
        {
            File.Copy(".\\style.css", outputPath + "style.css");
            return outputPath + "style.css";
        }

        /// <summary>
        /// Creates a copy of project header.html file to the given output path.
        /// </summary>
        /// <param name="outputPath">Destination directory path.</param>
        /// <returns>Full file path of the new header.html file.</returns>
        private string copyHeaderFile(string outputPath)
        {
            File.Copy(".\\header.html", outputPath + "header.html");
            return outputPath + "header.html";
        }

        /// <summary>
        /// Creates a copy of project footer.html file to the given output path.
        /// </summary>
        /// <param name="outputPath">Destination directory path.</param>
        /// <returns>Full file path of the new footer.html file.</returns>
        private string copyFooterFile(string outputPath)
        {
            File.Copy(".\\footer.html", outputPath + "footer.html");
            return outputPath + "footer.html";
        }

        /// <summary>
        /// Creates a copy of project cover.html file with document data to the given output path.
        /// </summary>
        /// <param name="outputPath">Destination directory path.</param>
        /// <param name="documentTitle">Title of the document that will be put in the cover file.</param>
        /// <param name="dateTime">Date parameter that will be put in the cover file.</param>
        /// <returns>Full file path of the new cover.html file.</returns>
        private string copyAndEditCoverFile(string outputPath, string documentTitle, DateTime dateTime)
        {
            using (StreamReader reader = new StreamReader(".\\cover.html"))
            using (StreamWriter writer = new StreamWriter(outputPath + "cover.html"))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("_title_"))
                    {
                        line = line.Replace("_title_", documentTitle);
                    }
                    if (line.Contains("_date_"))
                    {
                        line = line.Replace("_date_", dateTime.ToShortDateString());
                    }
                    writer.WriteLine(line);
                }
            }
                
            return outputPath + "cover.html";
        }

        /// <summary>
        /// Copies all contents of the given file to the outputWriter stream
        /// </summary>
        /// <param name="inputFile">Path of the file that will be copied.</param>
        /// <param name="outputWriter">Output stream for copying contents.</param>
        private void appendText(string inputFile, StreamWriter outputWriter)
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

        /// <summary>
        /// Writes a header html line to the output streamWriter with a document title taken from the given document.
        /// This is used for directories (titles with no content).
        /// </summary>
        /// <param name="doc">Document from which the title will be taken.</param>
        /// <param name="writer">Output stream where title will be written.</param>
        /// <param name="level">Header level.</param>
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
                    appendText(doc.HtmlPath, writer);
                }
                else
                {
                    appendDirectoryTitle(doc, writer, level);
                    recursiveDocumentWriter(doc.SubDocuments, writer, level + 1);
                }
            }
        }

        /// <summary>
        /// Generates a pdf using wkhtmltopdf with given content, header, footer and cover files to the given output path.
        /// </summary>
        /// <param name="inputHtmlPath">Path of the html file with contents.</param>
        /// <param name="outputPdfPath">Path where pdf file will be created.</param>
        /// <param name="headerPath">Path of the header html file.</param>
        /// <param name="footerPath">Path of the footer html file.</param>
        /// <param name="coverPath">Path of the cover html file.</param>
        /// <returns>True if pdf generation was successfull, false otherwise.</returns>
        private bool toPdf(string inputHtmlPath, string outputPdfPath, string headerPath, string footerPath, string coverPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "wkhtmltopdf.exe";
            startInfo.Arguments = "--header-html " + headerPath + " " +
                                  "--header-spacing " + footerPath + " " +
                                  "cover " + coverPath + " " +
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
