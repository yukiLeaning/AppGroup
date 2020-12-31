using Bloom.DataObjects;
using DLL.FileLib;
using System;

namespace Bloom.Models
{
    class CFileEditModel : AModel
    {
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
                Attributes = info.Attributes
            };
            return fileInfo;
        }
    }
}
