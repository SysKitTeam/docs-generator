using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator
{
    class MdToHtmlParser
    {
        
        public bool parseAllFiles(ref List<DocumentsWrapper> docsList)
        {
            
            for( int i = 0; i < docsList.Count; i++)
            {
                DocumentsWrapper doc = docsList[i];
                if (!doc.IsDirectory)
                {
                    editMdFile(doc.GitPath, (i + 1) + ".");
                    parseFileToHtml(ref doc);
                    htmlFilePostProcess(doc.HtmlPath, 1);
                }
                else
                {
                    parseRecursive(ref doc, 2, (i + 1) + ".");
                }
            }
            return true;
        }

        /// <summary>
        /// Prepares md file for conversion to html. This method will overwrite the old md file.
        /// </summary>
        /// <param name="path">Path of the md file to be edited.</param>
        private void editMdFile(string path, string numberTitle)
        {
            string tempFile = Path.GetTempFileName();

            using (var reader = new StreamReader(path))
            using (var writer = new StreamWriter(tempFile))
            {
                int headerLinesCount = 0;
                bool inHeader = false;
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    // header:
                    if (line.StartsWith("---"))
                    {
                        headerLinesCount++;
                        if (headerLinesCount < 2)
                        {
                            inHeader = true;
                        } else
                        {
                            inHeader = false;
                        }
                        writer.WriteLine(line);
                    }
                    else if (inHeader)
                    {
                        if (line.StartsWith("title:"))
                        {
                            writer.WriteLine(numberTitle + " " + line.Substring(6));
                        }
                    }
                    else // other:
                    {
                        // because a header and a line starting with '##' are both translated to <h2> using wkhtmltopdf,
                        // all lines begining with at least two ## will be appended to an aditional '#' so there will be no conflicts
                        if (line.StartsWith("##"))
                        {
                            line = "#" + line;
                        }
                        writer.WriteLine(line);
                    }
                }
            }

            File.Delete(path);
            File.Move(tempFile, path);
        }

        private void htmlFilePostProcess(string path, int level)
        {
            string titleHeaderTag = "<h" + level + ">";
            string otherHeaderTag = "<h" + (level + 1) + ">";
            string titleHeaderEndTag = "</h" + level + ">";
            string otherHeaderEndTag = "</h" + (level + 1) + ">";
            string tempFile = Path.GetTempFileName();

            using (StreamReader reader = new StreamReader(path))
            using (StreamWriter writer = new StreamWriter(tempFile))
            {
                string line;
                //bool firstHeader = true;
                while((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("<h2>") && line.Contains("</h2>"))
                    {
                        line = line.Replace("<h2>", titleHeaderTag);
                        line = line.Replace("</h2>", titleHeaderEndTag);
                    }
                    else if (line.Contains("<h3>") && line.Contains("</h3>"))   // headers greater than level 3 are replaced with <p> because
                    {                                                           // wkhtmltopdf puts all the headers in the table of contents
                        line = line.Replace("<h3>", "<p>");
                        line = line.Replace("</h3>", "</p>");
                    }
                    else if (line.Contains("<h4>") && line.Contains("</h4>"))
                    {
                        line = line.Replace("<h4>", "<p>");
                        line = line.Replace("</h4>", "</p>");
                    } else if (line.Contains("<h5>") && line.Contains("</h5>"))
                    {
                        line = line.Replace("<h5>", "<p>");
                        line = line.Replace("</h5>", "</p>");
                    }
                    if (line.Contains("<img src"))
                    {
                        line = line.Replace("#img", "./gitdownloads/_assets");
                    }
                    writer.WriteLine(line);
                }
            }

            File.Delete(path);
            File.Move(tempFile, path);
        }

        private void parseRecursive(ref DocumentsWrapper doc, int level, string numberTitle)
        {
            if (!doc.IsDirectory)
            {
                editMdFile(doc.GitPath, numberTitle);
                parseFileToHtml(ref doc);
                htmlFilePostProcess(doc.HtmlPath, level);
            }
            else
            {
                doc.TitleNumber = numberTitle;
                for (int i = 0; i < doc.SubDocuments.Count; i++)
                {
                    DocumentsWrapper subDoc = doc.SubDocuments[i];
                    parseRecursive(ref subDoc, level + 1, numberTitle + (i + 1) + ".");
                }
            }
        }

        private void parseFileToHtml(ref DocumentsWrapper doc)
        {
            using (var reader = new System.IO.StreamReader(doc.GitPath))
            using (var writer = new System.IO.StreamWriter(doc.GenerateHtmlPath(true)))
            {
                CommonMark.CommonMarkConverter.Convert(reader, writer);
            }
        }
    }
}
