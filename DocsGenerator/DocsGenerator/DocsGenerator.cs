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
        private static string htmlPath;
        private static string pdfPath;
        public static void GenerateDocs(string gitUrl, string outputPath)
        {
            string tmpPath = Path.GetTempPath();
            gitPath = tmpPath + @"DocsGenerator\gitdownloads\";
            htmlPath = tmpPath + @"DocsGenerator\html\";
            pdfPath = tmpPath + @"DocsGenerator\pdf\";


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

        }

        
        // TODO: move to separate class?
        private static bool GetGitDirectories(string gitUrl, string outputPath)
        {
            string result = Repository.Clone(gitUrl, outputPath);
            if (String.IsNullOrEmpty(result)) return false;
            else return true;
        }

        
    }
}
