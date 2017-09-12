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
        public bool GeneratePdf(List<DocumentsWrapper> docsList, string outputPath, string tmpDirPath, string documentTitle, string version)
        {
            var tmpFile = tmpDirPath + "ALL.html";
            copyCSSfile(tmpDirPath);

            var headerPath = copyHeaderFile(tmpDirPath);
            var footerPath = copyFooterFile(tmpDirPath);
            var coverPath = copyAndEditCoverFile(tmpDirPath, documentTitle, version);
            var fakeCoverPath = copyFakeCoverFile(tmpDirPath);
            var tocXslPath = copyTocXslFile(tmpDirPath);

            GenerateSingleHtmlFile(docsList, tmpFile);

            return toPdf(tmpFile, outputPath, headerPath, footerPath, coverPath, fakeCoverPath, tocXslPath, tmpDirPath);
        }

        /// <summary>
        /// Generates a single html file from all the previously generated html files. Also adds reference to style.css.
        /// </summary>
        /// <param name="docsList">List (tree) of document wrappers</param>
        /// <param name="outputFile">Full output path (with file name and extension).</param>
        public void GenerateSingleHtmlFile(List<DocumentsWrapper> docsList, string outputFile)
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
        /// Recursive function used by GenerateSingleHtmlFile method.
        /// </summary>
        /// <param name="docsList">List of document wrappers</param>
        /// <param name="writer">Writer linked to the html file that is being created</param>
        /// <param name="level">Depth of recursion</param>
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
        /// Creates a copy of project style.css file to the given output path.
        /// </summary>
        /// <param name="outputPath">Destination directory path.</param>
        /// <returns>Full file path of the new style.css file.</returns>
        private string copyCSSfile(string outputPath)
        {
            File.Copy(".\\style.css", outputPath + "style.css", true);
            return outputPath + "style.css";
        }

        /// <summary>
        /// Creates a copy of project header.html file to the given output path.
        /// </summary>
        /// <param name="outputPath">Destination directory path.</param>
        /// <returns>Full file path of the new header.html file.</returns>
        private string copyHeaderFile(string outputPath)
        {
            File.Copy(".\\header.html", outputPath + "header.html", true);
            File.Copy(".\\Images\\header.png", outputPath + "header.png", true);
            return outputPath + "header.html";
        }

        /// <summary>
        /// Creates a copy of project footer.html file to the given output path.
        /// </summary>
        /// <param name="outputPath">Destination directory path.</param>
        /// <returns>Full file path of the new footer.html file.</returns>
        private string copyFooterFile(string outputPath)
        {
            File.Copy(".\\footer.html", outputPath + "footer.html", true);
            File.Copy(".\\Images\\footer.png", outputPath + "footer.png", true);
            return outputPath + "footer.html";
        }

        private string copyTocXslFile(string outputPath)
        {
            File.Copy(".\\TOCStyle.xsl", outputPath + "TOCStyle.xsl", true);
            return outputPath + "TOCStyle.xsl";
        }

        private string copyFakeCoverFile(string outputPath)
        {
            File.Copy(".\\fakeCover.html", outputPath + "fakeCover.html", true);
            return outputPath + "fakeCover.html";
        }

        /// <summary>
        /// Creates a copy of project cover.html file with document data to the given output path.
        /// </summary>
        /// <param name="outputPath">Destination directory path.</param>
        /// <param name="documentTitle">Title of the document that will be put in the cover file.</param>
        /// <param name="dateTime">Date parameter that will be put in the cover file.</param>
        /// <returns>Full file path of the new cover.html file.</returns>
        private string copyAndEditCoverFile(string outputPath, string documentTitle, string version)
        {
            using (StreamReader reader = new StreamReader(".\\cover.html"))
            using (StreamWriter writer = new StreamWriter(outputPath + "cover.html"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("_title_"))
                    {
                        line = line.Replace("_title_", documentTitle);
                    }
                    if (line.Contains("_subtitle_"))
                    {
                        line = line.Replace("_subtitle_", $"Documentation for version {version}.");
                    }
                    writer.WriteLine(line);
                }
            }

            File.Copy(".\\Images\\cover.png", outputPath + "cover.png", true);
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
                while ((line = reader.ReadLine()) != null)
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
            string line = "<h" + level + ">" + doc.TitleNumber + " " + doc.Title + "</h" + level + ">";
            writer.WriteLine(line);
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
        private bool toPdf(string inputHtmlPath, string outputPdfPath, string headerPath, string footerPath, string coverPath, string fakeCoverPath, string tocXslPath, string tmpDirPath)
        {
            var contentPdfPath = tmpDirPath + "content.pdf";
            var coverPdfPath = tmpDirPath + "cover.pdf";
            var tmpPdf = tmpDirPath + "temp.pdf";
            var bookmarksOutput = tmpDirPath + "bookmark.info";

            createContentPdf(inputHtmlPath, contentPdfPath, headerPath, footerPath, fakeCoverPath, tocXslPath);
            createCoverPdf(coverPath, coverPdfPath);
            replaceCover(coverPdfPath, contentPdfPath, outputPdfPath, tmpPdf, bookmarksOutput);

            return true;
        }

        private bool replaceCover(string coverPdfPath, string contentPdfPath, string outPutPdfPath, string tempPdfPath, string bookmarkOutputPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = ".\\pdfkt\\pdftk.exe";
            startInfo.Arguments = "A=" + contentPdfPath + " " +
                                  "B=" + coverPdfPath + " " +
                                  "cat B1 A2-end output " + tempPdfPath;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();

            startInfo = new ProcessStartInfo();
            startInfo.FileName = ".\\pdfkt\\pdftk.exe";
            startInfo.Arguments = contentPdfPath + " " +
                                  "dump_data output " + bookmarkOutputPath;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();

            startInfo = new ProcessStartInfo();
            startInfo.FileName = ".\\pdfkt\\pdftk.exe";
            startInfo.Arguments = tempPdfPath + " " +
                                  "update_info " + bookmarkOutputPath + " "
                                  + "output " + outPutPdfPath;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();

            return true;
        }

        private bool createCoverPdf(string inputHtmlPath, string outputPdfPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "wkhtmltopdf.exe";
            startInfo.Arguments = "-B 0 -L 0 -R 0 -T 0 " +
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

        private bool createContentPdf(string inputHtmlPath, string outputPdfPath, string headerPath, string footerPath, string fakeCoverPath, string tocXslPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "wkhtmltopdf.exe";
            startInfo.Arguments = "--header-html " + headerPath + " " +
                                  "--margin-left 0  --margin-right 0 " +
                                  "--footer-html " + footerPath + " " +
                                  " cover " + fakeCoverPath + " " +
                                  "toc " +
                                  "--xsl-style-sheet " + tocXslPath + " " +
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
