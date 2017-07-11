using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            using (StreamReader toc = new StreamReader(dirPath + "TOC.md"))
            {
                string line;
                Regex regex = new Regex(@"# \[^[a-zA-Z0-9]*\]\(^[a-zA-Z0-9]*\)$");
                while ((line = toc.ReadLine()) != null)
                {
                    if (!line.StartsWith("#")) continue;
                    Match match = regex.Match(line);
                    DocumentsWrapper doc = new DocumentsWrapper();
                    doc.Title = match.Groups[0].Value;
                    doc.fileName = match.Groups[1].Value;
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
                    docsList.Add(doc);
                }
            }
            return docsList;
        }

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
            throw new NotImplementedException();
        }

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
    }
}
