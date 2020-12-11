using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using UtilityLib;

namespace AppGroup.Views.PopupWindows
{
    /// <summary>
    /// DeleteWindow ViewModel
    /// </summary>
    internal class DeleteWindow_ViewModel : INotifyPropertyChanged
    {
        private bool _IsEnabledDeleteButton;
        public bool IsEnabledDeleteButton 
        {
            get { return this._IsEnabledDeleteButton; }
            set
            {
                if(this._IsEnabledDeleteButton == value)
                {
                    return;
                }
                this._IsEnabledDeleteButton = value;
                this.OnPropertyChanged("IsEnabledDeleteButton");
            }
        }

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
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DeleteWindow()
        {
            InitializeComponent();
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
            this.m_ViewModel.IsEnabledDeleteButton = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deleteObjects">削除するファイルパス</param>
        /// <returns>エラー文言(No ErrorならEmpty)</returns>
        private string DeleteObject(string[] deleteObjects)
        {
            if (deleteObjects.Length.Equals(0))
            {
                return string.Empty;
            }

            List<string> errorObjects = new List<string>();
            foreach(var objectPath in deleteObjects)
            {
                bool ret = false;
                if (FFOLib.IsFile(objectPath))
                {
                    ret = FFOLib.DeleteFile(objectPath);
                }
                else if (FFOLib.IsFolder(objectPath))
                {
                    ret = FFOLib.DeleteFolder(objectPath);
                }

                if (!ret)
                {
                    // 削除できなかったパスを溜める
                    errorObjects.Add(objectPath);
                }
            }

            string errorMessage = string.Empty;
            if(errorObjects.Count.Equals(1))
            {
                errorMessage = "This module is not delete.\n";
                errorMessage += "[ " + errorObjects[0] + " ]";
            }
            else if(1 < errorObjects.Count)
            {
                errorMessage = "There module are not delete.\n";
                errorMessage += "[\n";
                foreach(var obj in errorObjects)
                {
                    errorMessage += obj + "\n";
                }
                errorMessage += "]";
            }

            return errorMessage;
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
                return DeleteObject(dropObjects);
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
