using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator
{
    public class DocumentsWrapper
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
        /// <summary>
        /// Generates a path for a html file from GitPath. Path also includes the correct file name
        /// with a .html extension
        /// </summary>
        /// <param name="setInternally">If true, given value is also stored to HtmlPath property.</param>
        /// <returns>Path for html file.</returns>
        public string GenerateHtmlPath(bool setInternally = false)
        {
            if (String.IsNullOrEmpty(GitPath)) return string.Empty;
            int i = GitPath.LastIndexOf('\\');
            if (!IsDirectory)
            {
                string shortFileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                if (setInternally)
                {
                    HtmlPath = GitPath.Substring(0, i + 1) + shortFileName + ".html";
                }
                return GitPath.Substring(0, i + 1) + shortFileName + ".html";
            }
            else
            {
                if (setInternally)
                    HtmlPath = GitPath.Substring(0, i + 1) + fileName + ".html";
                return GitPath.Substring(0, i + 1) + fileName + ".html";
            }
        }

        public string GeneratePdfPath(bool setInternally = false)
        {
            if (String.IsNullOrEmpty(GitPath)) return string.Empty;
            int i = GitPath.LastIndexOf('\\');
            if (!IsDirectory)
            {
                string shortFileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                if (setInternally)
                {
                    PdfPath = GitPath.Substring(0, i + 1) + shortFileName + ".pdf";
                }
                return GitPath.Substring(0, i + 1) + shortFileName + ".pdf";
            }
            else
            {
                if (setInternally)
                {
                    PdfPath = GitPath.Substring(0, i + 1) + fileName + ".pdf";
                }
                return GitPath.Substring(0, i + 1) + fileName + ".pdf";
            }
        }
    }
}
