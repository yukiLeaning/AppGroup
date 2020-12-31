using Bloom.Views.PopupWindows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Bloom.Framework;
using Bloom.Properties;
using System.Windows.Input;

namespace Bloom.Views.Pages
{
    /// <summary>
    /// TopPage.xaml の相互作用ロジック
    /// </summary>
    public partial class TopPage : Page
    {
        internal enum WindowName
        {
            DeleteObject,
            ConfigurationCopy,
            ConvertPDF,
            PCMonitor,
            ConvertNameBulk,
            FileEdit
        }

        internal struct WindowStruct
        {
            /// <summary>
            /// 不明
            /// </summary>
            internal Type type { get; set; }
            /// <summary>
            /// 動作関数
            /// </summary>
            internal Window window { get; set; }
            /// <summary>
            /// Window名
            /// </summary>
            internal string name { get; set; }

            internal WindowStruct(Type type, Window window, string name)
            {
                this.type = type;
                this.window = window;
                this.name = name;
            }
        }

        private Dictionary<WindowName, WindowStruct> m_WindowMap;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TopPage()
        {
            this.InitializeComponent();
            this.InitializeWindowMap();
            
            //ドラッグイベントを登録
            this.SystemHeader.PreviewMouseLeftButtonDown += (s, e) => 
            {
                Window parentWindow = (Window)this.Parent;
                if ((parentWindow != null) && (e.LeftButton == MouseButtonState.Pressed))
                {
                    parentWindow.DragMove();
                }
            };
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~TopPage()
        {
            //追加したイベントを削除
            Window parentWindow = (Window)this.Parent;
            this.SystemHeader.PreviewMouseLeftButtonDown -= (s, e) => parentWindow.DragMove();
        }

        /// <summary>
        /// WindowMap初期化
        /// </summary>
        /// <remarks>
        /// Viewを追加する場合、Mapに追加する.
        /// </remarks>
        private void InitializeWindowMap()
        {
            this.m_WindowMap = new Dictionary<WindowName, WindowStruct>();
            // 完全削除
            this.m_WindowMap.Add(
                WindowName.DeleteObject,
                new WindowStruct(
                    typeof(DeleteWindow), new DeleteWindow(), LangResource_jp.CONTENT_COMPLETE_REMOVAL
            ));
            // 構成コピー
            this.m_WindowMap.Add(
                WindowName.ConfigurationCopy,
                new WindowStruct(
                    typeof(CopyFolderWindow), new CopyFolderWindow(), LangResource_jp.CONTENT_CONFIGURATION_COPY
            ));
            // PDF変換
            this.m_WindowMap.Add(
                WindowName.ConvertPDF,
                new WindowStruct(
                    typeof(ConvertPDFWindow), new ConvertPDFWindow(), LangResource_jp.CONTENT_PDF_CONVERSION
            ));
            // PCモニター
            this.m_WindowMap.Add(
                WindowName.PCMonitor,
                new WindowStruct(
                    typeof(PCMonitorWindow), new PCMonitorWindow(), LangResource_jp.CONTENT_PC_MONITOR
            ));
            // ファイル名一括変換
            this.m_WindowMap.Add(
                WindowName.ConvertNameBulk,
                new WindowStruct(
                    typeof(ConvertNameBulkWindow), new ConvertNameBulkWindow(), LangResource_jp.CONTENT_FILE_NAME_BATCH_CONVERSION
            ));
            // ファイル編集
            this.m_WindowMap.Add(
                WindowName.FileEdit,
                new WindowStruct(
                    typeof(FileEditWindow), new FileEditWindow(), LangResource_jp.CONTENT_FILE_EDIT
            ));
        }

        /// <summary>
        /// 引数で渡されたWindowを開く
        /// </summary>
        /// <param name="window">Windowインスタンス</param>
        /// <returns></returns>
        private void Open_Window(Window window)
        {
            Window parentWindow = (Window)this.Parent;
            window.Owner = parentWindow;
            window.Left = window.Owner.Left;
            window.Top = window.Owner.Top;

            // ダイアログで表示
            window.ShowDialog();
        }

        /* イベントハンドラ */
        #region 変更不要
        /// <summary>
        /// ButtonArea内のボタンクリックイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectButton = sender as Button;
            this.Open_Window(this.m_WindowMap[(WindowName)(selectButton.Tag)].window);
        }

        /// <summary>
        /// ロード時イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(WindowName name in Enum.GetValues(typeof(WindowName)))
            {
                //TODO:存在確認
                if (!this.m_WindowMap.ContainsKey(name))
                {
                    continue;
                }

                Button button = new Button()
                {
                    Width = 280,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Tag = name,
                    Content = this.m_WindowMap[name].name
                };
                button.Click += this.Button_Click;

                this.SelectArea.Children.Add(button);
            }
        }

        /// <summary>
        /// アンロード時イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (var value in this.m_WindowMap)
            {
                value.Value.window.Close();
            }
            this.m_WindowMap.Clear();
        }

        /// <summary>
        /// 最小化ボタン押下イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {

        }
        
        /// <summary>
        /// リサイズボタン押下イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 閉じるボタン押下イベントイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = (Window)this.Parent;
            parentWindow.Close();
        }
        
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion 変更不要
    }
}
