using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloom.Models
{
    public enum EModel : int
    {
        CopyFolderModel, 
        DeleteObjectModel,
        ResourceMonitorModel,
        FileEditModel
    }

    class CModelFactory
    {
        // メンバ変数
        static CModelFactory instance = new CModelFactory();
        Dictionary<EModel, AModel> modelTable = null;
        
        /// <summary>
        /// モデルFactoryクラスのインスタンスを取得.
        /// </summary>
        /// <returns>モデルFactoryインスタンス</returns>
        static public CModelFactory GetInstnace()
        {
            if (instance == null)
            {
                // どんな状況???
                instance = new CModelFactory();
            }
            return instance;
        }
        
        /// <summary>
        /// Factoryに登録されているModelを取得する.
        /// </summary>
        /// <param name="eModel">モデルに紐づく列挙値</param>
        /// <returns>
        /// Modelのインスタンス.
        /// </returns>
        /// <remarks>
        /// インスタンスの取得に失敗した場合、nullを返却する.
        /// </remarks>
        public AModel GetModel(EModel eModel)
        {
            AModel ret = null;
            modelTable.TryGetValue(eModel, out ret);

            return ret;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private CModelFactory()
        {
            // メンバ変数の初期化
            modelTable = new Dictionary<EModel, AModel>();

            // 初期化関数呼び出し
            this.InitializeModelTable();
        }

        /// <summary>
        /// Modelインスタンス管理テーブルの初期化
        /// </summary>
        private void InitializeModelTable()
        {
            modelTable.Add(EModel.CopyFolderModel,      new CCopyFolederConfigurationModel());
            modelTable.Add(EModel.DeleteObjectModel,    new CDeleteObjectModel());
            modelTable.Add(EModel.ResourceMonitorModel, new CResourceMonitorModel());
            modelTable.Add(EModel.FileEditModel,        new CFileEditModel());
        }
    }
}
