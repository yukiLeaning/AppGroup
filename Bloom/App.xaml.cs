using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Bloom.Framework;

namespace Bloom
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        //初期処理を記載
        public App()
        {
            ResourceManagers resourceManagers = new ResourceManagers();
        }
    }
}
