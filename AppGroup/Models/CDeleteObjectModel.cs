using AppGroup.Properties;
using System;
using System.Collections.Generic;
using UtilityLib;
using LogLib;

namespace AppGroup.Models
{
    class CDeleteObjectModel : AModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CDeleteObjectModel() 
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectPaths"></param>
        /// <returns></returns>
        public void DeleteObject(string[] objectPaths)
        {
            LogManager.WriteCallStack_In();

            if (objectPaths.Length.Equals(0))
            {
                throw new ArgumentException(LangResource_jp.PARAMETER_EMPTY);
            }

            List<string> errorObjects = new List<string>();
            foreach(var objectPath in objectPaths)
            {
                bool ret = false;
                if (FFOLib.IsFile(objectPath))
                {
                    ret = FFOLib.DeleteFile(objectPath);
                }
                else if (FFOLib.IsFolder(objectPath))
                {
                    ret = FFOLib.DeleteFolder(objectPath);
                }

                if (!ret)
                {
                    // 削除できなかったパスを溜める
                    errorObjects.Add(objectPath);
                }
            }

            string errorMessage = string.Empty;
            if(errorObjects.Count.Equals(1))
            {
                errorMessage = LangResource_jp.PARAMETER_EMPTY + "\n";
                errorMessage += "[ " + errorObjects[0] + " ]";
            }
            else if(1 < errorObjects.Count)
            {
                errorMessage = LangResource_jp.FAILE_DELETE_MULTIPLE + "\n";
                errorMessage += "[\n";
                foreach(var obj in errorObjects)
                {
                    errorMessage += obj + "\n";
                }
                errorMessage += "]";
            }

            if (!errorMessage.Equals(string.Empty))
            {
                Exception exception = new Exception(errorMessage);
            }

            LogManager.WriteCallStack_Out();
        }
    }
}
