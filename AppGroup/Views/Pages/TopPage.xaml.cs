﻿using AppGroup.Views.PopupWindows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using AppGroup.Framework;
using AppGroup.Properties;

namespace AppGroup.Views.Pages
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
            ConvertNameBulk
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
        }

        /// <summary>
        /// 引数で渡されたWindowを開く
        /// </summary>
        /// <param name="window">Windowインスタンス</param>
        /// <returns></returns>
        private void Open_Window(Window window)
        {
            window.Owner = (Window)this.Parent;
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
        #endregion 変更不要
    }
}
