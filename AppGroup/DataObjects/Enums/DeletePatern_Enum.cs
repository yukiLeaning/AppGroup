using AppGroup.Framework;

namespace AppGroup.DataObjects.Enums
{
    /// <summary>
    /// 削除パターンを定義
    /// </summary>
    public class DeletePaternType : Enumeration
    {
        public static readonly DeletePaternType Invalid = new DeletePaternType(-1, "Invalid");
        public static readonly DeletePaternType File = new DeletePaternType(0, "File");
        public static readonly DeletePaternType Folder = new DeletePaternType(1, "Folder");
        
        public DeletePaternType(int id, string name)
            : base(id, name)
        {
        }
    }
}
