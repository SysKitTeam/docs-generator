using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator
{
    class HtmlToPdfParser
    {
        public static bool GeneratePdf(ref List<DocumentsWrapper> docsList, string outputPath)
        {
            // First, generate multiple pdfs from all the html files
            generateIndividualPdfs(ref docsList);
            // Second, gather all generated pdfs to a single one:
            return true;
        }

        private static bool generateIndividualPdfs(ref List<DocumentsWrapper> docsList)
        {
            for(int i = 0; i < docsList.Count; i++)
            {
                DocumentsWrapper doc = docsList[i];
                if (doc.IsDirectory)
                {
                    List<DocumentsWrapper> newDocsList = doc.SubDocuments;
                    if (!generateIndividualPdfs(ref newDocsList)) return false;
                    doc.SubDocuments = newDocsList;
                }
                else
                {
                    if (!toPdf(ref doc)) return false;
                }
            }
            return true;
        }

        private static bool toPdf(ref DocumentsWrapper doc)
        {
            throw new NotImplementedException();
        }
    }
}
