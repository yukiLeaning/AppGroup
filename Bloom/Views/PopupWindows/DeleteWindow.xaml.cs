using Bloom.Models;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Bloom.Views.PopupWindows
{
    /// <summary>
    /// DeleteWindow ViewModel
    /// </summary>
    internal class DeleteWindow_ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// DeleteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DeleteWindow : Window
    {
        private DeleteWindow_ViewModel m_ViewModel;
        private CDeleteObjectModel deleteModel;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DeleteWindow()
        {
            this.deleteModel = CModelFactory.GetInstnace().GetModel(EModel.DeleteObjectModel) as CDeleteObjectModel;

            this.InitializeComponent();
            this.InitializeViewModel();

            //イベント登録
            this.m_ViewModel.PropertyChanged += this.ViewModel_PropertyChanged;
        }

        /// <summary>
        /// ViewModelの初期化
        /// </summary>
        private void InitializeViewModel()
        {
            this.m_ViewModel = new DeleteWindow_ViewModel();
            this.DataContext = this.m_ViewModel;
        }
        
        #region イベントハンドラ
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
            default:
            {
                /* NOP */
                break;
            }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.IsActive)
            {
                e.Cancel = true;
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            var dropObjects = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (dropObjects.Length.Equals(0))
            {
                return;
            }

            Task<string> task = Task.Run(() =>
            {
                string errorMessage = string.Empty;
                try
                {
                    this.deleteModel.DeleteObject(dropObjects);
                }
                catch
                {

                }
                return errorMessage;
            });
            ProgressWindow progressWindow = new ProgressWindow();
            progressWindow.Show();
            string result = task.Result;
            progressWindow.Close();

            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show(result);
            }
        }
        #endregion
    }
}
