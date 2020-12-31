using System;
using System.IO;

namespace Bloom.DataObjects
{
    public struct SFileInfo
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ファイル名(パス込み)
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// ファイルサイズ
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 作成日
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 所有者
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// 作成時のファイル名
        /// </summary>
        public string OriginalName { get; set; }
        /// <summary>
        /// ファイル属性
        /// </summary>
        public string Attributes { get; set; }

        public SFileInfo(SFileInfo fileInfo)
        {
            this.Name = fileInfo.Name;
            this.FullName = fileInfo.FullName;
            this.Size = fileInfo.Size;
            this.CreateTime = fileInfo.CreateTime;
            this.UpdateTime = fileInfo.UpdateTime;
            this.Owner = fileInfo.Owner;
            this.OriginalName = fileInfo.OriginalName;
            this.Attributes = fileInfo.Attributes;
        }

        public SFileInfo(
            string name,
            string fullName,
            long size,
            DateTime createTime,
            DateTime updateTime,
            string owner,
            string originalName,
            string attributes)
        {
            this.Name = name;
            this.FullName = fullName;
            this.Size = size;
            this.CreateTime = createTime;
            this.UpdateTime = updateTime;
            this.Owner = owner;
            this.OriginalName = originalName;
            this.Attributes = attributes;
        }
    }
}
