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
        string htmlPath;
        string pdfPath;
        public void GenerateDocs(string gitUrl, string outpuPath)
        {
            string tmpPath = Path.GetTempPath();
            gitPath = tmpPath + @"DocsGenerator\gitdownloads\";
            htmlPath = tmpPath + @"DocsGenerator\html\";
            pdfPath = tmpPath + @"DocsGenerator\pdf\";
            
            if (!GetGitDirectories(gitUrl, gitPath))
            {
                Console.WriteLine("Error fetching git files.");
                return;
            }

            List<DocumentsWrapper> docsList = DocumentsWrapperFactory.GenerateDocumentsWrapperListFromPath(gitPath);
            if (!MdToHtmlParser.parseAllFiles(ref docsList))
            {
                throw new Exception("Something went wrong with parsing md to html conversion.");
            }


        }

        
        // TODO: move to separate class?
        private bool GetGitDirectories(string gitUrl, string outputPath)
        {
            string result = Repository.Clone(gitUrl, outputPath);
            if (String.IsNullOrEmpty(result)) return false;
            else return true;
        }

        
    }
}
