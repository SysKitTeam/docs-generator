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

        public static void editMdFile(string path)
        {

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

        }
    }
}
