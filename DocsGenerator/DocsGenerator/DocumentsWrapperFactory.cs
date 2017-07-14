using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace DocsGenerator
{
    class DocumentsWrapperFactory
    {
        public static List<DocumentsWrapper> GenerateDocumentsWrapperListFromPath(string rootDir)
        {
            List<DocumentsWrapper> docsList = new List<DocumentsWrapper>();
            if (File.Exists(rootDir + "TOC.md"))
            {
                docsList = parseTOC(rootDir);
            }
            else
            {
                generateAlphabeticalDocsList(rootDir);
            }
            for (int i = 0; i < docsList.Count; i++)
            {
                DocumentsWrapper currentDoc = docsList[i];
                if (currentDoc.IsDirectory)
                {
                    docsFillRecursive(ref currentDoc);
                }
            }
            return docsList;
        }

        private static void docsFillRecursive(ref DocumentsWrapper docsDir)
        {
            // Check for TOC:
            if (File.Exists(docsDir.GitPath + "TOC.md"))
            {
                docsDir.SubDocuments = parseTOC(docsDir.GitPath);
            }
            else
            {
                docsDir.SubDocuments = generateAlphabeticalDocsList(docsDir.GitPath);
            }

            // Check for more directories, if found continue recursive
            for (int i = 0; i < docsDir.SubDocuments.Count; i++)
            {
                DocumentsWrapper currentDoc = docsDir.SubDocuments[i];
                if (currentDoc.IsDirectory)
                {
                    docsFillRecursive(ref currentDoc);
                }
            }
        }

        private static List<DocumentsWrapper> parseTOC(string dirPath)
        {
            List<DocumentsWrapper> docsList = new List<DocumentsWrapper>();
            // structureList sluzi za podrsku vise razina naslova (odredjeno po broju '#' u TOC.md)
            // tu se cuvaju zadnji naslovi za svaku razinu
            List<DocumentsWrapper> structureList = new List<DocumentsWrapper>();
            using (StreamReader toc = new StreamReader(dirPath + "TOC.md"))
            {
                string line;
                
                while ((line = toc.ReadLine()) != null)
                {
                    if (!line.StartsWith("#")) continue;
                    
                    

                    DocumentsWrapper doc = new DocumentsWrapper();
                    string title, fileName;
                    getTitleAndFilenameFromString(line, out title, out fileName);
                    doc.Title = title;
                    doc.fileName = fileName;
                    if (fileName.EndsWith(".md"))
                    {
                        // Check if it is a directory or a file:
                        if (File.Exists(dirPath + doc.fileName))
                        {
                            doc.GitPath = dirPath + doc.fileName;
                            doc.IsDirectory = false;
                        }
                        else
                        {
                            int indexOfDotmd = doc.fileName.LastIndexOf('.');
                            doc.fileName = doc.fileName.Remove(indexOfDotmd) + @"\";
                            doc.IsDirectory = true;
                            doc.GitPath = dirPath + doc.fileName;
                        }
                    }
                    int hashcount = countHashes(line);
                    if (hashcount == structureList.Count) // no changes in level, just add doc where needed and replace last one in structureList
                    {
                        if (hashcount > 1) structureList[hashcount - 2].SubDocuments.Add(doc);
                        else docsList.Add(doc);
                        structureList[hashcount - 1] = doc;
                    } else if (hashcount > structureList.Count()) // one level added
                    {
                        structureList[hashcount - 2].SubDocuments.Add(doc);
                        structureList.Add(doc);
                    }
                    else // one or more levels removed
                    {
                        structureList.RemoveRange(hashcount, structureList.Count - (hashcount - 1));
                        if (hashcount > 1)
                        {
                            structureList[hashcount - 2].SubDocuments.Add(doc);
                        } else
                        {
                            docsList.Add(doc);
                        }
                        structureList[hashcount - 1] = doc;
                    }
                }
            }
            return docsList;
        }

        /// <summary>
        /// Generates a list of <typeparamref name="DocumentsWrapper"/>s ordered alphabetically (both files and directories in the same list)
        /// </summary>
        /// <param name="dirPath"> Path of the directory. </param>
        /// <returns> List of <typeparamref name="DocumentsWrapper"/>s. </returns>
        private static List<DocumentsWrapper> generateAlphabeticalDocsList(string dirPath)
        {
            List<DocumentsWrapper> docsList = new List<DocumentsWrapper>();
            IEnumerable<string> allFiles = Directory.EnumerateFileSystemEntries(dirPath).OrderBy(s => s);
            foreach (string currentPath in allFiles)
            {
                if (File.Exists(currentPath))
                {
                    DocumentsWrapper currentDoc = new DocumentsWrapper();
                    currentDoc.Title = getTitleFromFile(currentPath);
                    currentDoc.GitPath = currentPath;
                    currentDoc.IsDirectory = false;
                    docsList.Add(currentDoc);
                }
                else
                {
                    DocumentsWrapper currentDoc = new DocumentsWrapper();
                    currentDoc.GitPath = currentPath;
                    currentDoc.IsDirectory = true;
                    docsList.Add(currentDoc);
                }
            }
            return docsList;
        }

        /// <summary>
        /// Reads the whole file to find a line witch gives a title.
        /// </summary>
        /// <param name="filePath"> Path of the file. </param>
        /// <returns> Title if found, null otherwise. </returns>
        private static string getTitleFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return string.Empty;
            StreamReader sr = new StreamReader(filePath);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("title:"))
                {
                    return line.Split(':')[1].Trim();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Parses given string to get title and file name from it. If no fileName is given in the string,
        /// then it is generated from the given title as a directory name.
        /// </summary>
        /// <param name="input"> String to be parsed. </param>
        /// <param name="title"> Where title will be stored. </param>
        /// <param name="fileName"> Where filename will be stored. </param>
        private static void getTitleAndFilenameFromString(string input, out string title, out string fileName)
        {
            if (input.Contains("("))
            {
                string[] parts = input.Split(']');
                title = parts[0].Substring(3);
                fileName = parts[1].Replace(")", "").Substring(1);
            } else
            {
                title = input.Substring(countHashes(input));
                fileName = title.ToLower().Replace(' ', '-') + "\\";
            }
            
        }

        /// <summary>
        /// Counts the amount of hashes on the begining of the given string
        /// </summary>
        /// <param name="input">String to be counted.</param>
        /// <returns>Number of hashes on the begining of the string.</returns>
        private static int countHashes(string input)
        {
            int i = 0;
            while (input[i].Equals('#'))
            {
                i++;
            }
            return i;
        }
    }
}
