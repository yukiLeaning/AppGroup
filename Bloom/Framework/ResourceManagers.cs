using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Bloom.Properties;

namespace Bloom.Framework
{
    class ResourceManagers
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ResourceManagers()
        {
            /* TODO：多言語対応させたい */
            //OSの言語を取得する
            string osLang = CultureInfo.CurrentCulture.Name;

            switch (osLang)
            {
                case "ja-JP":
                    
                    break;
                case "en-US":
                default:
                    
                    break;
            }
        }
    }
}
