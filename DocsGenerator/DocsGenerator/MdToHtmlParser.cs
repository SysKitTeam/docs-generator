using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator
{
    class MdToHtmlParser
    {
        public static bool parseAllFiles(ref List<DocumentsWrapper> docsList)
        {
            foreach(DocumentsWrapper doc in docsList)
            {
                if (!doc.IsDirectory)
                {
                    editMdFile(doc.GitPath);
                    parseFileToHtml(doc);
                }
                else
                {
                    parseRecursive(doc);
                }
            }
        }

        public static void editMdFile(string path)
        {

        }

        private static void parseRecursive(DocumentsWrapper doc)
        {

        }

        private static void parseFileToHtml(DocumentsWrapper doc)
        {

        }
    }
}
