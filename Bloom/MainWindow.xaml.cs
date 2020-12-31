using Bloom.Views.Pages;
using System.Windows;
using System.Windows.Controls;

namespace Bloom
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        /// <remarks>アプリケーションを終了させる際の処理を記載</remarks>
        ~MainWindow()
        {
            //NOP
        }

        #region イベントハンドラ
        /// <summary>
        /// ロードイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Page page = new TopPage();
            this.AddChild(page);
        }

        /// <summary>
        /// アンロードイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            //NOP
        }
        #endregion
    }
}
