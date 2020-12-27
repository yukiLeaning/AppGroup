using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

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

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileEditWindow()
        {
            this.InitializeComponent();
            this.InitializeViewAreaTable();
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
            if (dropObjects.Length.Equals(0))
            {
                return;
            }
            
            //ViewAreaを切り替え
            this.ChangeVisibleViewArea(ViewArea.EditArea);
        }

        //これどうにかしたい
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.IsActive)
            {
                e.Cancel = true;
                this.Visibility = Visibility.Collapsed;
            }
        }
        #endregion
    }
}
