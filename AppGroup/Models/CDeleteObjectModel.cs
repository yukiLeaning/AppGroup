using AppGroup.DataObjects.Enums;
using UtilityLib;

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

        public string DeleteObject(string objectPath)
        {
            var errorMessage = string.Empty;
            
            return errorMessage;
        }

        /// <summary>
        /// 削除プロセスを実行する.
        /// </summary>
        private void ExecuteDelteProcess(string objectPath, DeletePaternType selectPatern)
        {
            bool result = false;
            if (DeletePaternType.File == selectPatern)
            {
                result = FFOLib.DeleteFile(objectPath);
            }
            else if(DeletePaternType.Folder == selectPatern)
            {
                result = FFOLib.DeleteFolder(objectPath);
            }
            else
            {
                throw new System.InvalidOperationException("無効な操作です.");
            }

            if (!result)
            {
                throw new System.InvalidOperationException("オブジェクトの削除に失敗しました。");
            }
        }

        /// <summary>
        /// 指定されたパスのオブジェクトは何か.
        /// </summary>
        /// <returns</returns>
        private DeletePaternType WhatObject(string objectPath)
        {
            DeletePaternType result = DeletePaternType.Invalid;
            
            if (FFOLib.IsFile(objectPath))
            {
                result = DeletePaternType.File;
            }
            else if (FFOLib.IsFolder(objectPath))
            {
                result = DeletePaternType.Folder;
            }
            return result;
        }
    }
}
