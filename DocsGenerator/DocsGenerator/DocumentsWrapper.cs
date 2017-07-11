using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator
{
    class DocumentsWrapper
    {
        public DocumentsWrapper()
        {
            SubDocuments = new List<DocumentsWrapper>();
        }
        public string Title { get; set; }
        public string fileName { get; set; }
        public string GitPath { get; set; }
        public string PdfPath { get; set; }
        public string HtmlPath { get; set; }
        public bool IsDirectory { get; set; }
        public List<DocumentsWrapper> SubDocuments { get; set; }
    }
}
