using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppGroup.Views.PopupWindows
{
    /// <summary>
    /// ConvertNameBulkWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ConvertNameBulkWindow : Window
    {
        public ConvertNameBulkWindow()
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

        #region イベントハンドラ

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion イベントハンドラ
    }
}
