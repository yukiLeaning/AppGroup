using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DLL.ConvertLib;

namespace DLL.Test
{
    [TestClass]
    public class ConvertLibTest
    {
        [TestMethod]
        public void Convert()
        {
            string pdfFilePath = @"C:\tmp\拘辱蹂躙.pdf";
            string outputDirectoryPath = @"C:\tmp\";

            Assert.IsTrue(PDFConvert.ConvertPDF2Image(pdfFilePath, outputDirectoryPath));
        }
    }
}
