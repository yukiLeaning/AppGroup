using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Linq;

namespace DLL.LogLib
{
    public class LogManager
    {
        private const string logConfigFileName = @"LogConfig.xml";
        //TODO削除
        private const string delimiter = @"\";
        private const string logDirectoryName = "Log";
        private const string applicationLogFileName = "Application.log";
        //定値
        private static int frameCount = 1; //StackFrame数(1ならば直接呼び出したメソッド)
        //メンバ変数
        private static string logDirectoryPath;
        private static string applicationLogFilePath;
        
        internal enum LogLevel_Enum : int
        {
            Infomation  = 0,
            Error       = 1,
            Warning     = 2,
            Debug       = 3,
            Verbose     = 4
        }

        internal struct LogInfo
        {
            readonly string FileName;
            readonly string DirectoryPath;
            readonly int Level;

            internal LogInfo(string fileName, string directoryPath, int level)
            {
                FileName = fileName;
                DirectoryPath = directoryPath;
                Level = level;
            }
        }

        private static Dictionary<LogLevel_Enum, string> dictionary = new Dictionary<LogLevel_Enum, string>()
        {
            {LogLevel_Enum.Infomation,  "[INF]"},
            {LogLevel_Enum.Error,       "[ERR]"},
            {LogLevel_Enum.Warning,     "[WAN]"},
            {LogLevel_Enum.Debug,       "[DEB]"},
            {LogLevel_Enum.Verbose,     "[VEB]"},
        };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LogManager()
        {
            logDirectoryPath = CreatePath(Environment.CurrentDirectory, logDirectoryName);
            applicationLogFilePath = CreatePath(logDirectoryPath, applicationLogFileName);
            try
            {
                //Logフォルダの作成
                if (!Directory.Exists(logDirectoryPath))
                {
                    Directory.CreateDirectory(logDirectoryPath);
                }

                //Logファイルを作成
                using (FileStream fileStream = new FileStream(applicationLogFilePath, FileMode.OpenOrCreate))
                {
                    byte[] byteLog = Encoding.Unicode.GetBytes(CreateStartLog());
                    fileStream.Write(byteLog, 0, byteLog.Length);
                }
            }
            catch
            {
                /* Not throw exception */
                //LogManagerの初期化に失敗した場合、救いようがないため、以降の処理は行わない
                return;
            }
        }

        /// <summary>
        /// InfomationレベルでApplicationログを書き込む
        /// </summary>
        /// <param name="log">出力ログ</param>
        public static void InfomationWriteApplicationLog(string log, bool isStack = false)
        {
            if (isStack)
            {
                StackFrame stackFrame = new StackFrame(frameCount);
                AddStackFrame(ref log, stackFrame);
            }
            WriteApplicationLog(log, LogLevel_Enum.Infomation);
        }

        /// <summary>
        /// ErrorレベルでApplicationログを書き込む
        /// </summary>
        /// <param name="log">出力ログ</param>
        public static void ErrorWriteApplicationLog(string log, bool isStack = false)
        {
            if (isStack)
            {
                StackFrame stackFrame = new StackFrame(frameCount);
                AddStackFrame(ref log, stackFrame);
            }
            WriteApplicationLog(log, LogLevel_Enum.Error);
        }

        /// <summary>
        /// DebugレベルでApplicationログを書き込む
        /// </summary>
        /// <param name="log">出力ログ</param>
        public static void DebugWriteApplicationLog(string log, bool isStack = false)
        {
            if (isStack)
            {
                StackFrame stackFrame = new StackFrame(frameCount);
                AddStackFrame(ref log, stackFrame);
            }
            WriteApplicationLog(log, LogLevel_Enum.Debug);
        }

        /// <summary>
        /// WarningレベルでApplicationログを書き込む
        /// </summary>
        /// <param name="log">出力ログ</param>
        public static void WarningWriteApplicationLog(string log, bool isStack = false)
        {
            if (isStack)
            {
                StackFrame stackFrame = new StackFrame(frameCount);
                AddStackFrame(ref log, stackFrame);
            }
            WriteApplicationLog(log, LogLevel_Enum.Warning);
        }

        /// <summary>
        /// VerboseレベルでApplicationログを書き込む
        /// </summary>
        /// <param name="log">出力ログ</param>
        public static void VerboseWriteApplicationLog(string log, bool isStack = false)
        {
            if (isStack)
            {
                StackFrame stackFrame = new StackFrame(frameCount);
                AddStackFrame(ref log, stackFrame);
            }
            WriteApplicationLog(log, LogLevel_Enum.Verbose);
        }

        /// <summary>
        /// Inコールスタックを書き込む
        /// </summary>
        public static void WriteCallStack_In()
        {
            StackFrame stackFrame = new StackFrame(frameCount);
            string log = "In";
            AddStackFrame(ref log, stackFrame);

            WriteApplicationLog(log, LogLevel_Enum.Verbose);
        }

        /// <summary>
        /// Outコールスタックを書き込む
        /// </summary>
        public static void WriteCallStack_Out()
        {
            StackFrame stackFrame = new StackFrame(frameCount);
            string log = "Out";
            AddStackFrame(ref log, stackFrame);

            WriteApplicationLog(log, LogLevel_Enum.Verbose);
        }
        
        //ファイル／フォルダパスを生成
        private string CreatePath(params string[] args)
        {
            string path = string.Empty;
            for (int i=0; i<args.Length; i++)
            {
                path += args[i];
                if(args.Length != (i + 1))
                {
                    path += delimiter;
                }
            }

            return path;
        }
        
        //日付を付与
        private static bool AddDateTime(ref string logStr)
        {
            bool result = true;
            try
            {
                DateTime nowTime = DateTime.Now;
                string dateTimeStr = nowTime.ToString("[yyyy/MM/dd HH:mm:ss]");
                logStr += dateTimeStr;
            }
            catch
            {
                result = false;
            }

            return result;
        }
        
        //ログレベルを付与
        private static bool AddLogLevel(ref string logStr, LogLevel_Enum logLevel)
        {
            bool result = true;
            try
            {
                string logLevelStr = string.Empty;
                result = dictionary.TryGetValue(logLevel, out logLevelStr);
                logStr += logLevelStr;
            }
            catch
            {
                result = false;
            }

            return result;
        }
        
        //呼び出し元情報を付与
        private static bool AddStackFrame(ref string logStr, StackFrame stackFrame)
        {
            bool result = true;
            try
            {
                string stackFrameStr = string.Empty;
                //呼び出し元のメソッド名を取得
                stackFrameStr += " (method=[" + stackFrame.GetMethod().Name + "()],";
                //呼び出し元のクラス名を取得
                stackFrameStr += "class=[" + stackFrame.GetMethod().ReflectedType.FullName + "])";
                logStr += stackFrameStr;
            }
            catch
            {
                result = false;
            }

            return result;
        }
        
        //Applicationログを書き込む
        private static void WriteApplicationLog(string log, LogLevel_Enum logLevel = LogLevel_Enum.Infomation)
        {
            string writeValue = "\n";
            //Logヘッダー情報を書き込む
            AddDateTime(ref writeValue);
            AddLogLevel(ref writeValue, logLevel);

            //Log情報を出力
            writeValue += log;

            using (FileStream fileStream = new FileStream(applicationLogFilePath, FileMode.Append))
            {
                byte[] byteLog = Encoding.Unicode.GetBytes(writeValue);
                fileStream.Write(byteLog, 0, byteLog.Length);
            }
        }
        
        //起動時ログを作成
        private string CreateStartLog()
        {
            string result = string.Empty;

            result += "**********************************************************************************************" + "\n";
            result += "                                                                                              " + "\n";
            result += "                                       App Group Start                                        " + "\n";
            result += "                                                                                              " + "\n";
            result += "**********************************************************************************************" + "\n";

            return result;
        }

        //コンフィグファイルを作成
        private void CreateConfigFile(string configFilePath)
        {
            using (FileStream fileStream = File.Create(configFilePath))
            {
                string xmlValue = string.Empty;
                xmlValue += "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "\n";
                xmlValue += "<Log>" + "\n";
                xmlValue += "    <ApplicationLog>" + "\n";
                xmlValue += "        <DirectoryPath>C:\\tmp\\</DirectoryPath>" + "\n";
                xmlValue += "        <KeyName>Application</KeyName>" + "\n";
                xmlValue += "        <FileName>Application.log</FileName>" + "\n";
                xmlValue += "        <Level>0</Level>" + "\n";
                xmlValue += "    </ApplicationLog>" + "\n";
                xmlValue += "</Log>" + "\n";

                byte[] byteStr = Encoding.Unicode.GetBytes(xmlValue);
                fileStream.Write(byteStr, 0, byteStr.Length);
            }
        }

        //Xml形式のコンフィグファイルを読み込む
        private void LoadConfigFile()
        {
            //コンフィグファイルの読み込み
            string logconfigFilePath = CreatePath(Environment.CurrentDirectory, logConfigFileName);
            if (!File.Exists(logconfigFilePath))
            {
                //コンフィグファイルがなければデフォルト値で作成する
                CreateConfigFile(logconfigFilePath);
            }

            //xmlファイルを取得
            XElement xml = XElement.Load(logconfigFilePath);
            //Log情報のタグ内の情報を取得する
            IEnumerable<XElement> infos = from item in xml.Elements("Log") select item;
            Dictionary<string, LogInfo> logInfos = new Dictionary<string, LogInfo>();
            foreach (XElement info in infos)
            {
                try
                {
                    string _keyName = info.Element("KeyName").Value;
                    string _fileName = info.Element("FileName").Value;
                    string _directoryPath = info.Element("DirectoryPath").Value;
                    int _logLevel = int.Parse(info.Element("Level").Value);
                    LogInfo _logInfo = new LogInfo(_fileName, _directoryPath, _logLevel);
                    logInfos.Add(_keyName, _logInfo);
                }
                catch
                {
                    continue;
                }
            }
        }

        //コンフィグファイルからApplicationログのファイルパスを取得
        private string GetApplicationLogFilePath()
        {
            throw new NotImplementedException();
        }

        //コンフィグファイルからログレベルを取得
        private LogLevel_Enum GetLogLevel()
        {
            throw new NotImplementedException();
        }
    }
}
