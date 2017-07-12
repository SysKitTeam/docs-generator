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
                }
                else
                {
                    parseRecursive(ref doc);
                }
            }
            return true;
        }

        /// <summary>
        /// Prepares md file for conversion to html. This method will overwrite the old md file.
        /// </summary>
        /// <param name="path">Path of the md file to be edited.</param>
        public static void editMdFile(string path)
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

        private static void parseRecursive(ref DocumentsWrapper doc)
        {
            if (!doc.IsDirectory)
            {
                editMdFile(doc.GitPath);
                parseFileToHtml(ref doc);
            }
            else
            {
                parseRecursive(ref doc);
            }
        }

        private static void parseFileToHtml(ref DocumentsWrapper doc)
        {
            using (var reader = new System.IO.StreamReader(doc.GitPath))
            using (var writer = new System.IO.StreamWriter(doc.GenerateHtmlPath()))
            {
                CommonMark.CommonMarkConverter.Convert(reader, writer);
            }
        }
    }
}
