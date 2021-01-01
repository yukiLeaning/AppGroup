using System;
using System.Diagnostics;
using System.IO;

namespace DLL.FileLib
{
    public class FileInfomationManager
    {
        private FileInfo fileInfo;
        private FileVersionInfo versionInfo;

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
        public DateTime? CreateTime
        {
            get { return _CreateTime; }
            private set
            {
                if (_CreateTime == value) return;
                _CreateTime = value;
                this.fileInfo.CreationTime = (DateTime)_CreateTime;
            }
        }
        private DateTime? _CreateTime;
        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime? UpdateTime 
        {
            get { return _UpdateTime; }
            private set
            {
                if (_UpdateTime == value) return;
                _UpdateTime = value;
                this.fileInfo.CreationTime = (DateTime)_UpdateTime;
            }
        }
        private DateTime? _UpdateTime;
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
        public FileAttributes? Attributes
        {
            get { return _Attributes; }
            private set
            {
                if (_Attributes == value) return;
                _Attributes = value;

            }
        }
        private FileAttributes? _Attributes;
        
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

                this.fileInfo = new FileInfo(filePath);
                this.versionInfo = FileVersionInfo.GetVersionInfo(filePath);
                FileAttributes attributes = File.GetAttributes(filePath);
                this.Name = this.fileInfo.Name;
                this.FullName = this.fileInfo.FullName;
                this.Size = this.fileInfo.Length;
                this.CreateTime = this.fileInfo.CreationTime;
                this.UpdateTime = this.fileInfo.LastWriteTime;
                this.Owner = this.versionInfo.LegalCopyright;
                this.OriginalName = this.versionInfo.OriginalFilename;
                this.Attributes = this.fileInfo.Attributes;
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
                (this.Attributes is FileAttributes);
            
            return result;
        }

        /// <summary>
        /// 作成日を設定
        /// </summary>
        /// <param name="createTime">作成日</param>
        /// <returns>成否</returns>
        public bool SetCreateTime(DateTime createTime)
        {
            bool result = true;
            try
            {
                this.CreateTime = createTime;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 更新日を設定
        /// </summary>
        /// <param name="createTime">更新日</param>
        /// <returns>成否</returns>
        public bool SetUpdateTime(DateTime updateTime)
        {
            bool result = true;
            try
            {
                this.UpdateTime = updateTime;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// ファイル属性を設定
        /// </summary>
        /// <param name="fileAttributes">ファイル属性</param>
        /// <returns>成否</returns>
        public bool SetFileAttributes(FileAttributes fileAttributes)
        {
            bool result = true;
            try
            {
                this.Attributes = fileAttributes;
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
