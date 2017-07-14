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
        
        public static bool parseAllFiles(ref List<DocumentsWrapper> docsList)
        {
            
            for( int i = 0; i < docsList.Count; i++)
            {
                DocumentsWrapper doc = docsList[i];
                if (!doc.IsDirectory)
                {
                    editMdFile(doc.GitPath);
                    parseFileToHtml(ref doc);
                    htmlFilePostProcess(doc.HtmlPath, 1);
                }
                else
                {
                    parseRecursive(ref doc, 2);
                }
            }
            return true;
        }

        /// <summary>
        /// Prepares md file for conversion to html. This method will overwrite the old md file.
        /// </summary>
        /// <param name="path">Path of the md file to be edited.</param>
        private static void editMdFile(string path)
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
                            writer.WriteLine(line.Substring(6));
                        }
                    }
                    else // other:
                    {
                        writer.WriteLine(line);
                    }
                }
            }

            File.Delete(path);
            File.Move(tempFile, path);
        }

        private static void htmlFilePostProcess(string path, int level)
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
                bool firstHeader = true;
                while((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("<h2>"))
                    {
                        if (firstHeader)
                        {
                            line.Replace("<h2>", titleHeaderTag);
                        } else
                        {
                            line.Replace("<h2>", otherHeaderTag);
                        }
                    } else if (line.Contains("</h2>"))
                    {
                        if (firstHeader)
                        {
                            line.Replace("</h2>", titleHeaderEndTag);
                            firstHeader = false;
                        } else
                        {
                            line.Replace("</h2>", otherHeaderEndTag);
                        }
                    }
                    writer.WriteLine(line);
                }
            }

            File.Delete(path);
            File.Move(tempFile, path);
        }

        private static void parseRecursive(ref DocumentsWrapper doc, int level)
        {
            if (!doc.IsDirectory)
            {
                editMdFile(doc.GitPath);
                parseFileToHtml(ref doc);
                htmlFilePostProcess(doc.HtmlPath, level);
            }
            else
            {
                for (int i = 0; i < doc.SubDocuments.Count; i++)
                {
                    DocumentsWrapper subDoc = doc.SubDocuments[i];
                    parseRecursive(ref subDoc, level + 1);
                }
            }
        }

        private static void parseFileToHtml(ref DocumentsWrapper doc)
        {
            using (var reader = new System.IO.StreamReader(doc.GitPath))
            using (var writer = new System.IO.StreamWriter(doc.GenerateHtmlPath(true)))
            {
                CommonMark.CommonMarkConverter.Convert(reader, writer);
            }
        }
    }
}
