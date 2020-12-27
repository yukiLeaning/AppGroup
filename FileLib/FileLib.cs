using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLib
{
    public class FileLib
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
        public long Size { get; private set; }
        /// <summary>
        /// 作成日
        /// </summary>
        public DateTime CreateTime { get; private set; }
        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime UpdateTime { get; private set; }
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
        public FileAttributes attributes { get; private set; }
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
        private FileLib() { /*NOP*/ }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込むファイルパス</param>
        public FileLib(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"[{filePath}]は見つかりません");
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
            this.attributes = info.Attributes;
        }
    }
}
