using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppGroup_UnitTest.UtilityLib
{
    [TestClass]
    public class FFOLib_Test
    {
        private string testFolderPath = @"C:\testTmp";
        private string testFilePath = @"C:\testTmp\testFile.test";

        [TestInitialize]
        public void SetUp()
        {
            // テストディレクトリ作成
            Directory.CreateDirectory(testFolderPath);
            // テストファイル作成
            File.Create(testFilePath);
        }

        [TestCleanup]
        public void CleanUp()
        {
            // テストファイルが残っていた場合、削除する
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
            // テストディレクトリが残っていた場合、削除する
            if (Directory.Exists(testFolderPath))
            {
                Directory.Delete(testFolderPath);
            }
        }

        [TestMethod]
        public void DelteFile_Test()
        {
        }
    }
}
