using System.Reflection;

namespace Bloom.Models
{
    class AModel : IModel
    {
        /// <summary>
        /// 関数名取得.
        /// </summary>
        /// <returns></returns>
        protected new string ToString() 
        {
            return MethodBase.GetCurrentMethod().Name;
        }
    }
}
