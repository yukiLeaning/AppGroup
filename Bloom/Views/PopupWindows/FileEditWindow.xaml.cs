using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Bloom.DataObjects;
using Bloom.Models;
using Bloom.Views.UserControls;

namespace Bloom.Views.PopupWindows
{
    /// <summary>
    /// FileEditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FileEditWindow : Window
    {
        /// <summary>
        /// ViewAreaの列挙値
        /// </summary>
        /// <remarks>
        /// 追加した場合、areaTableにインスタンスをAddする
        /// </remarks>
        private enum ViewArea
        {
            SelectArea,
            EditArea
        }

        Dictionary<ViewArea, FrameworkElement> areaTable = new Dictionary<ViewArea, FrameworkElement>();
        CFileEditModel fileEditModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileEditWindow()
        {
            this.InitializeComponent();
            this.InitializeViewAreaTable();

            this.fileEditModel = CModelFactory.GetInstnace().GetModel(EModel.FileEditModel) as CFileEditModel;
        }

        /// <summary>
        /// ViewAreaテーブルの初期化
        /// </summary>
        private void InitializeViewAreaTable()
        {
            /* ViewAreaを追加した場合は、Addする */
            this.areaTable.Add(ViewArea.SelectArea, this.m_SelectArea);
            this.areaTable.Add(ViewArea.EditArea, this.m_EditArea);
        }

        /// <summary>
        /// Viewの初期化
        /// </summary>
        private void InitializeView()
        {
            this.m_FileInfoTab.Items.Clear();
            this.ChangeVisibleViewArea(ViewArea.SelectArea);
        }

        /// <summary>
        /// ViewArea変更
        /// </summary>
        /// <param name="viewArea">表示するViewArea</param>
        private void ChangeVisibleViewArea(ViewArea viewArea)
        {
            foreach(var value in this.areaTable)
            {
                if (value.Key == viewArea) continue;
                value.Value.Visibility = Visibility.Collapsed;
            }
            this.areaTable[viewArea].Visibility = Visibility.Visible;
        }

        #region イベントハンドラ
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
            if (dropObjects.Length.Equals(0)) return;

            this.m_FileInfoTab.Items.Clear(); //念の為初期化
            foreach(var value in dropObjects)
            {
                //ファイル情報取得
                SFileInfo? fileInfo = fileEditModel.GetFileInfo(value);
                if (!fileInfo.HasValue) continue;
                //ファイル編集コントロールを作成
                FileEditControl fileEditControl = new FileEditControl();
                fileEditControl.SetFileInfomation(fileInfo.Value);
                //TabItemを作成
                TabItem tabItem = new TabItem();
                tabItem.Header = fileInfo.Value.Name;
                tabItem.Content = fileEditControl;

                this.m_FileInfoTab.Items.Add(tabItem);
            }

            //ViewAreaを切り替え
            this.ChangeVisibleViewArea(ViewArea.EditArea);
        }

        //これどうにかしたい
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.InitializeView();
            if (this.IsActive)
            {
                e.Cancel = true;
                this.Visibility = Visibility.Collapsed;
            }
        }
        
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.InitializeView();
        }
        #endregion
    }
}
