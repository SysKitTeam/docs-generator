using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocsGenerator
{
    public class DocsGenerator
    {
        string gitPath;
        string pdfPath;
        public void GenerateDocs(string gitUrl, string outpuPath)
        {
            string tmpPath = Path.GetTempPath();
            gitPath = tmpPath + @"DocsGenerator\gitdownloads\";
            pdfPath = tmpPath + @"DocsGenerator\pdf\";
            
            if (!GetGitDirectories(gitUrl, gitPath))
            {
                Console.WriteLine("Error fetching git files.");
                return;
            }

            List<DocumentsWrapper> docsList = getDocumentsWrapperList(gitPath);


        }

        private List<DocumentsWrapper> getDocumentsWrapperList(string rootDir)
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
            for(int i = 0; i < docsList.Count; i++)
            {
                DocumentsWrapper currentDoc = docsList[i];
                if (currentDoc.IsDirectory)
                {
                    docsFillRecursive(ref currentDoc);
                }
            }
        }

        private List<DocumentsWrapper> parseTOC(string dirPath)
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

        private List<DocumentsWrapper> generateAlphabeticalDocsList(string dirPath)
        {
            IEnumerable<string> allFiles = Directory.EnumerateFileSystemEntries(dirPath);
            foreach(string currentPath in allFiles)
            {
                if (File.Exists(currentPath);
            }
            throw new NotImplementedException();
        }

        private void docsFillRecursive(ref DocumentsWrapper docsDir)
        {
            // Check for TOC:
            if(File.Exists(docsDir.GitPath + "TOC.md"))
            {
                docsDir.SubDocuments = parseTOC(docsDir.GitPath);
            } else
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
        
        private bool GetGitDirectories(string gitUrl, string outputPath)
        {
            string result = Repository.Clone(gitUrl, outputPath);
            if (String.IsNullOrEmpty(result)) return false;
            else return true;
        }

        
    }
}
