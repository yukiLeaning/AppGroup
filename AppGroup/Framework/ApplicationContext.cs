using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGroup.Framework
{
    class ApplicationContext
    {
        private static ApplicationContext instnace;
        
        public static ApplicationContext GetInstance()
        {
            if(instnace == null)
            {
                instnace = new ApplicationContext();
            }
            return instnace;
        }

        /// <summary>
        /// コンストラクタ(外部アクセス禁止)
        /// </summary>
        private ApplicationContext() { }
    }
}
