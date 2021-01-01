using Bloom.DataObjects;
using DLL.FileLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bloom.Models
{
    class CFileEditModel : AModel
    {
        /// <summary>
        /// ファイル属性文字列変換テーブル
        /// </summary>
        private readonly Dictionary<FileAttributes, string> str_attributes = new Dictionary<FileAttributes, string>()
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

        public CFileEditModel()
        {
            /* NOP */
        }

        /// <summary>
        /// ファイル情報を取得
        /// </summary>
        /// <returns>ファイル情報</returns>
        /// <remarks>例外発生時、nullを返却<remarks>
        public SFileInfo? GetFileInfo(string filePath)
        {
            FileInfomationManager info = new FileInfomationManager(filePath);

            //ガード処理
            if (!info.IsValidInfomation()) return null;

            SFileInfo fileInfo = new SFileInfo()
            {
                Name = info.Name,
                FullName = info.FullName,
                Size = (long)info.Size,
                CreateTime = (DateTime)info.CreateTime,
                UpdateTime = (DateTime)info.UpdateTime,
                Owner = info.Owner,
                OriginalName = info.OriginalName,
                Attributes = this.str_attributes[(FileAttributes)info.Attributes]
            };
            return fileInfo;
        }
    }
}
