using Bloom.DataObjects.Enums;
using Bloom.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bloom.Views.PopupWindows
{
    /// <summary>
    /// DeleteWindow ViewModel
    /// </summary>
    internal class CopyFolderWindow_ViewModel : INotifyPropertyChanged
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
    public partial class CopyFolderWindow : Window
    {
        private CopyFolderWindow_ViewModel m_ViewModel;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CopyFolderWindow()
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
            this.m_ViewModel = new CopyFolderWindow_ViewModel();
            this.DataContext = this.m_ViewModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="copyObjects"></param>
        /// <returns></returns>
        private string CopyObject(string[] copyObjects)
        {
            string errorMessage = string.Empty;
            // コピーできるフォルダは1つのみとする
            if (copyObjects.Length.Equals(0) && (1 < copyObjects.Length))
            {
                errorMessage = "失敗しました";
            }



            return errorMessage;
        }

        #region イベントハンドラ
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            /* NOP */
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
                return CopyObject(dropObjects);
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
