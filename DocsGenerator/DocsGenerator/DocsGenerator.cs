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
        private string gitPath;
        private List<DocumentsWrapper> unprocessedDocuments;
        public void GenerateDocs(string gitUrl, string outputPath, string branchName, string tmpPath = null)
        {
            if (string.IsNullOrEmpty(tmpPath))
            {
                tmpPath = Path.GetTempPath();
            }
            gitPath = tmpPath + @"DocsGenerator\gitdownloads\";
            clearOldData(tmpPath + @"DocsGenerator\");
            // Step 1: Fetch files

            if (!GetGitDirectories(gitUrl, gitPath, branchName))
            {
                throw new Exception("Error fetching files. Given URL: " + gitUrl);
            }

            // Step 2: Parse all .md files to html format (with editing)
            DocumentsWrapperFactory docFactory = new DocumentsWrapperFactory();
            List<DocumentsWrapper> docsList = docFactory.GenerateDocumentsWrapperListFromPath(gitPath);
            unprocessedDocuments = docFactory.UnprocessedDocuments;
            MdToHtmlParser mdToHtmlParser = new MdToHtmlParser();
            if (!mdToHtmlParser.parseAllFiles(ref docsList))
            {
                throw new Exception("Something went wrong with parsing md to html.");
            }

            // Step 3: Generate single pdf from given files
            HtmlToPdfParser htmlToPdfParser = new HtmlToPdfParser();
            string title = getDocumentTitle(gitPath);
            if (!htmlToPdfParser.GeneratePdf(docsList, outputPath, tmpPath + @"DocsGenerator\", title))
            {
                throw new Exception("Something went wrong with parsing html to pdf.");
            }

            if (!File.Exists(outputPath))
            {
                throw new Exception("Error using wkhtml to create pdf document.");
            }


        }

        
        // TODO: move to separate class?
        private bool GetGitDirectories(string gitUrl, string outputPath, string branchName)
        {
            if (!Repository.IsValid(gitUrl)) throw new Exception("Given url is not a valid GitHub repository.");
            string result;
            if (string.IsNullOrEmpty(branchName))
            {
                result = Repository.Clone(gitUrl, outputPath);
            } else
            {
                CloneOptions options = new CloneOptions();
                options.BranchName = branchName;
                result = Repository.Clone(gitUrl, outputPath, options);
            }
            if (String.IsNullOrEmpty(result)) return false;
            else return true;
        }

        private void clearOldData(string path)
        {
            if (Directory.Exists(path))
            {
                DeleteDirectory(path);
            }
        }

        public bool HasUnprocessedDocuments()
        {
            if (unprocessedDocuments == null) return false;
            if (unprocessedDocuments.Count > 1) return true;
            else return false;
        }

        public List<string> getUnprocessedDocumentsDetails()
        {
            if (!HasUnprocessedDocuments()) return new List<string>();
            List<string> details = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach(DocumentsWrapper doc in unprocessedDocuments)
            {
                if (!string.IsNullOrEmpty(doc.Title)) sb.Append("Title: " + doc.Title);
                if (!string.IsNullOrEmpty(doc.GitPath)) sb.Append(" Path: " + doc.GitPath);
                else if (!string.IsNullOrEmpty(doc.fileName)) sb.Append(" File name: " + doc.fileName);
                if (sb.Length < 1) sb.Append("Unknown file.");
                details.Add(sb.ToString());
                sb.Clear();
            }
            return details;
        }

        private string getDocumentTitle(string rootpath)
        {
            if (File.Exists(rootpath + "README.md"))
            {
                using (StreamReader reader = new StreamReader(rootpath + "README.md"))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("# "))
                        {
                            return line.Substring(2);
                        }
                    }
                }
            }
            if (File.Exists(rootpath + "index.md"))
            {
                using (StreamReader reader = new StreamReader(rootpath + "index.md"))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("title: "))
                        {
                            return line.Substring(7);
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, true);
        }

    }
}
