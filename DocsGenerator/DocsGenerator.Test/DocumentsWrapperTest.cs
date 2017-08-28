using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocsGenerator.Test
{
    [TestClass]
    public class DocumentsWrapperTest
    {
        [TestMethod]
        public void GenerateHtmlPath()
        {
            DocumentsWrapper doc = new DocumentsWrapper();
            doc.GitPath = @"C:\Test\Filename.md";
            doc.IsDirectory = false;
            doc.fileName = "Filename.md";
            string generatedHtmlPath = doc.GenerateHtmlPath();
            Assert.AreEqual(@"C:\Test\Filename.html", generatedHtmlPath);
        }
    }
}
