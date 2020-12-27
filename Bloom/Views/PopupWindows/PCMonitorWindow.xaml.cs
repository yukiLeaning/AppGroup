using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bloom.Views.PopupWindows
{
    /// <summary>
    /// PCMonitor.xaml の相互作用ロジック
    /// </summary>
    public partial class PCMonitorWindow : Window
    {
        public PCMonitorWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.IsActive)
            {
                e.Cancel = true;
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
