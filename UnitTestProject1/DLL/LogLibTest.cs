using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace DLL.Test
{
    [TestClass]
    public class LogManagerTest
    {
        string logDirectoryPath = Environment.CurrentDirectory + @"\Log";
        string applicationFilePath = Environment.CurrentDirectory + @"\Log\Application.log";
        
        [TestInitialize]
        public void Initialize()
        {
            if (Directory.Exists(logDirectoryPath))
            {
                //過去に生成したLogフォルダがある場合は削除しておく
                Directory.Delete(logDirectoryPath, true);
            }
            LogManager logManager = new LogManager();
        }

        [TestMethod]
        public void LogManager_Normal01()
        {
            //Logフォルダの存在確認
            Assert.IsTrue(Directory.Exists(logDirectoryPath));
            //Application.logの存在確認
            Assert.IsTrue(File.Exists(applicationFilePath));
        }

        [TestMethod]
        public void WriteApplicationLog_Normal01()
        {
            LogManager.WriteCallStack_In();
            LogManager.InfomationWriteApplicationLog("Info test");
            LogManager.InfomationWriteApplicationLog("Info test", true);
            LogManager.ErrorWriteApplicationLog("Error test");
            LogManager.DebugWriteApplicationLog("Debug test");
            LogManager.WarningWriteApplicationLog("Warning test");
            LogManager.VerboseWriteApplicationLog("Verbose test");
            LogManager.WriteCallStack_Out();
        }
    }
}
