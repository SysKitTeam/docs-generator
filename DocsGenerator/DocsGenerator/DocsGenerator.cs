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
        private static string gitPath;
        public static void GenerateDocs(string gitUrl, string outputPath)
        {
            string tmpPath = Path.GetTempPath();
            gitPath = tmpPath + @"DocsGenerator\gitdownloads\";
            clearOldData(tmpPath + @"DocsGenerator\");
            try
            {
                // Step 1: Fetch files
                Console.WriteLine("Fetching files from GitHub...");
                if (!GetGitDirectories(gitUrl, gitPath))
                {
                    Console.WriteLine("Error fetching git files.");
                    return;
                }
                Console.WriteLine("Files fetched.\n");

                // Step 2: Parse all .md files to html format (with editing)
                Console.WriteLine("Parsing .md to html format...");
                List<DocumentsWrapper> docsList = DocumentsWrapperFactory.GenerateDocumentsWrapperListFromPath(gitPath);
                if (!MdToHtmlParser.parseAllFiles(ref docsList))
                {
                    throw new Exception("Something went wrong with parsing md to html.");
                }
                Console.WriteLine("Html files created.");

                // Step 3: Generate single pdf from given files
                Console.WriteLine("Generating pdf...");
                if (!HtmlToPdfParser.GeneratePdf(docsList, outputPath))
                {
                    throw new Exception("Something went wrong with parsing html to pdf.");
                }
                Console.WriteLine("");
                if (Directory.Exists(tmpPath + @"DocsGenerator\"))
                {
                    Directory.Delete(tmpPath + @"DocsGenerator\");
                }
            } finally
            {
                if (Directory.Exists(tmpPath + @"DocsGenerator\"))
                {
                    DeleteDirectory(tmpPath + @"DocsGenerator\");
                }
            }
            

        }

        
        // TODO: move to separate class?
        private static bool GetGitDirectories(string gitUrl, string outputPath)
        {
            string result = Repository.Clone(gitUrl, outputPath);
            if (String.IsNullOrEmpty(result)) return false;
            else return true;
        }

        private static void clearOldData(string path)
        {
            if (Directory.Exists(path))
            {
                DeleteDirectory(path);
            }
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

            Directory.Delete(target_dir, false);
        }

    }
}
