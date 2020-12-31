using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DLL.FileLib
{
    public class FileInfomationManager
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// ファイル名(パス込み)
        /// </summary>
        public string FullName { get; private set; }
        /// <summary>
        /// ファイルサイズ
        /// </summary>
        public long? Size { get; private set; }
        /// <summary>
        /// 作成日
        /// </summary>
        public DateTime? CreateTime { get; private set; }
        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime? UpdateTime { get; private set; }
        /// <summary>
        /// 所有者
        /// </summary>
        public string Owner { get; private set; }
        /// <summary>
        /// 作成時のファイル名
        /// </summary>
        public string OriginalName { get; private set; }
        /// <summary>
        /// ファイル属性
        /// </summary>
        public string Attributes { get; private set; }
        /// <summary>
        /// ファイル属性文字列変換テーブル
        /// </summary>
        private Dictionary<FileAttributes, string> str_attributes = new Dictionary<FileAttributes, string>()
        {
            { FileAttributes.ReadOnly           ,"読み取り専用" },
            { FileAttributes.Hidden             ,"隠しファイル" },
            { FileAttributes.System             ,"システムファイル" },
            { FileAttributes.Directory          ,"ディレクトリ" },
            { FileAttributes.Archive            ,"アーカイブ" },
            { FileAttributes.Device             ,"デバイスファイル" },
            { FileAttributes.Normal             ,"標準ファイル" },
            { FileAttributes.Temporary          ,"一時ファイル" },
            { FileAttributes.SparseFile         ,"スパースファイル" },
            { FileAttributes.ReparsePoint       ,"リパースポイント" },
            { FileAttributes.Compressed         ,"圧縮ファイル" },
            { FileAttributes.Offline            ,"オフライン" },
            { FileAttributes.NotContentIndexed  ,"インデックス未付属" },
            { FileAttributes.Encrypted          ,"暗号化ファイル" },
            { FileAttributes.IntegrityStream    ,"データ整合性ストリーム" },
            { FileAttributes.NoScrubData        ,"データ整合性未チェック" }
        };

        /// <summary>
        /// 禁止
        /// </summary>
        private FileInfomationManager() { /*NOP*/ }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~FileInfomationManager() { }

        /// <summary>
        /// メンバを全て初期化する
        /// </summary>
        private void InitializeMember()
        {
            this.Name = null;
            this.FullName = null;
            this.Size = null;
            this.CreateTime = null;
            this.UpdateTime = null;
            this.Owner = null;
            this.OriginalName = null;
            this.Attributes = null;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込むファイルパス</param>
        public FileInfomationManager(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException();
                }

                FileInfo info = new FileInfo(filePath);
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);
                FileAttributes attributes = File.GetAttributes(filePath);
                this.Name = info.Name;
                this.FullName = info.FullName;
                this.Size = info.Length;
                this.CreateTime = info.CreationTime;
                this.UpdateTime = info.LastWriteTime;
                this.Owner = versionInfo.LegalCopyright;
                this.OriginalName = versionInfo.OriginalFilename;
                this.Attributes = this.str_attributes[info.Attributes];
            }
            catch
            {
                this.InitializeMember();
                return;
            }
        }

        /// <summary>
        /// 有効なファイル情報か否か
        /// </summary>
        /// <returns>有効か否か</returns>
        public bool IsValidInfomation()
        {
            bool result = 
                (this.Name is string) &&
                (this.FullName is string) &&
                (this.Size is long) &&
                (this.CreateTime is DateTime) &&
                (this.UpdateTime is DateTime) &&
                (this.Attributes is string);
            
            return result;
        }
    }
}
