using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UtilityLib
{
    /// <summary>
    /// ファイル操作系ライブラリ
    /// </summary>
    public static class FFOLib
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool GetFiles(string targetFolderPath, ref string[] outFilePaths, string[] extensions = null)
        {
            bool result = true;
            try
            {
                IEnumerable<string> tmpFiles = Directory.EnumerateFiles(targetFolderPath);
                List<string> files = new List<string>();
                foreach (var file in tmpFiles)
                {
                    string targetExtension = Path.GetExtension(file);
                    if ((extensions == null) || (extensions.Length == 0) || (extensions.Contains(targetExtension)))
                    {
                        files.Add(file);
                    }
                }
                outFilePaths = Convert2Array<string>(files);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static T[] Convert2Array<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }
            return enumerable as T[] ?? enumerable.ToArray();
        }

        /// <summary>
        /// ファイルか否かを確認
        /// </summary>
        /// <param name="filePath">確認対象のファイルパス</param>
        /// <returns>ファイル：true</returns>
        public static bool IsFile(string filePath)
        {
            bool result = false;
            if (File.Exists(filePath))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// フォルダか否かを確認
        /// </summary>
        /// <param name="filePath">確認対象のフォルダパス</param>
        /// <returns>フォルダ：true</returns>
        public static bool IsFolder(string folderPath)
        {
            bool result = false;
            if (Directory.Exists(folderPath))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 強制的にファイルを削除する
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns></returns>
        public static bool DeleteFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            bool result = false;
            FileInfo file = new FileInfo(filePath);
            try
            {
                if((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    file.Attributes = FileAttributes.Normal;
                }
                file.Delete();
                result = true;
            }
            catch
            {
                result = false;
            }
            
            return result;
        }
        
        /// <summary>
        /// 強制的にフォルダを削除する
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static bool DeleteFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                return false;
            }

            bool result = false;
            try
            {
                Directory.Delete(folderPath);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
