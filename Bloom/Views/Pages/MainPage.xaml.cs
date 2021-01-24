using Bloom.Properties;
using Bloom.Views.PopupWindows;
using Bloom.Views.Pages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bloom.Views.Pages
{
    /// <summary>
    /// TopPage.xaml の相互作用ロジック
    /// </summary>
    public partial class MainPage : Page
    {
        internal enum ViewName
        {
            DeleteObject,
            ConfigurationCopy,
            ConvertPDF,
            PCMonitor,
            ConvertNameBulk,
            FileEdit,
            CreateDexision
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

        internal struct PageStruct
        {
            /// <summary>
            /// 不明
            /// </summary>
            internal Type type { get; set; }
            /// <summary>
            /// 動作関数
            /// </summary>
            internal Page page { get; set; }
            /// <summary>
            /// Window名
            /// </summary>
            internal string name { get; set; }

            internal PageStruct(Type type, Page page, string name)
            {
                this.type = type;
                this.page = page;
                this.name = name;
            }
        }

        private Dictionary<ViewName, WindowStruct> m_PopupWindowMap;
        private Dictionary<ViewName, PageStruct> m_ViewPageMap;

        /* Loadedイベント呼び出し後に格納されるため、
         * コンストラクタ、デストラクタでは使えない */
        private Window parentWindow;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.InitializePopupWindowMap();
            this.InitializeViewPageMap();
            this.InitializeEventHandler();
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MainPage()
        {
            //追加したイベントを削除
            Window parentWindow = (Window)this.Parent;
            this.SystemHeader.PreviewMouseLeftButtonDown -= (s, e) => parentWindow.DragMove();
        }

        /// <summary>
        /// PopupWindowMap初期化
        /// </summary>
        /// <remarks>
        /// PopupWindowを追加する場合、Mapに追加する.
        /// </remarks>
        private void InitializePopupWindowMap()
        {
            this.m_PopupWindowMap = new Dictionary<ViewName, WindowStruct>();
            // 完全削除
            this.m_PopupWindowMap.Add(
                ViewName.DeleteObject,
                new WindowStruct(
                    typeof(DeleteWindow), new DeleteWindow(), LangResource_jp.CONTENT_COMPLETE_REMOVAL
            ));
            // 構成コピー
            this.m_PopupWindowMap.Add(
                ViewName.ConfigurationCopy,
                new WindowStruct(
                    typeof(CopyFolderWindow), new CopyFolderWindow(), LangResource_jp.CONTENT_CONFIGURATION_COPY
            ));
            // PDF変換
            this.m_PopupWindowMap.Add(
                ViewName.ConvertPDF,
                new WindowStruct(
                    typeof(ConvertPDFWindow), new ConvertPDFWindow(), LangResource_jp.CONTENT_PDF_CONVERSION
            ));
            // PCモニター
            this.m_PopupWindowMap.Add(
                ViewName.PCMonitor,
                new WindowStruct(
                    typeof(PCMonitorWindow), new PCMonitorWindow(), LangResource_jp.CONTENT_PC_MONITOR
            ));
            // ファイル名一括変換
            this.m_PopupWindowMap.Add(
                ViewName.ConvertNameBulk,
                new WindowStruct(
                    typeof(ConvertNameBulkWindow), new ConvertNameBulkWindow(), LangResource_jp.CONTENT_FILE_NAME_BATCH_CONVERSION
            ));
            // ファイル編集
            this.m_PopupWindowMap.Add(
                ViewName.FileEdit,
                new WindowStruct(
                    typeof(FileEditWindow), new FileEditWindow(), LangResource_jp.CONTENT_FILE_EDIT
            ));
        }

        /// <summary>
        /// ViewPageMap初期化
        /// </summary>
        /// <remarks>
        /// ViewPageを追加する場合、Mapに追加する.
        /// </remarks>
        private void InitializeViewPageMap()
        {
            this.m_ViewPageMap = new Dictionary<ViewName, PageStruct>();
            // 決裁編集
            this.m_ViewPageMap.Add(
                ViewName.CreateDexision,
                new PageStruct(
                    typeof(CreateDexision), new CreateDexision(), LangResource_jp.CONTENT_CREATE_DEXISION
            ));
        }

        /// <summary>
        /// MainPageにイベントを登録
        /// </summary>
        private void InitializeEventHandler()
        {
            //ドラッグイベントを登録
            this.SystemHeader.PreviewMouseLeftButtonDown += (sender, e) => 
            {
                Window parentWindow = (Window)this.Parent;
                if ((parentWindow != null) && (e.LeftButton == MouseButtonState.Pressed))
                {
                    parentWindow.DragMove();
                }
            };
        }

        /// <summary>
        /// MainPageにイベントを登録(Loadedイベント後)
        /// </summary>
        private void AddEventHandler()
        {
            this.parentWindow.StateChanged += (sender, e) =>
            {
                if(this.parentWindow.WindowState == WindowState.Normal)
                {
                    this.m_ResizeButton.Content = 1;
                }
                else if(this.parentWindow.WindowState == WindowState.Maximized)
                {
                    this.m_ResizeButton.Content = 2;
                }
            };
        }

        /// <summary>
        /// 引数で渡されたWindowを開く
        /// </summary>
        /// <param name="window">Windowインスタンス</param>
        /// <returns></returns>
        private void OpenWindow(Window window)
        {
            Window parentWindow = (Window)this.Parent;
            window.Owner = parentWindow;
            window.Left = window.Owner.Left;
            window.Top = window.Owner.Top;

            // ダイアログで表示
            window.ShowDialog();
        }

        /// <summary>
        /// 引数で渡されたPageを
        /// </summary>
        /// <param name="page"></param>
        private void ShowPage(Page page)
        {
            this.m_ViewAre.Children.Add(page);
        }

        /// <summary>
        /// Windowの状態を最大化/標準に変更
        /// </summary>
        /// <remarks>
        /// 最大化されている状態で呼び出すと標準に戻る
        /// </remarks>
        private void ChangeMaximizedWindowState()
        {
            WindowState state = WindowState.Normal;
            switch (this.parentWindow.WindowState)
            {
                case WindowState.Maximized:
                    state = WindowState.Normal;
                    break;
                case WindowState.Normal:
                    state = WindowState.Maximized;
                    break;
                default:
                    break;
            }
            this.parentWindow.WindowState = state;
        }

        /// <summary>
        /// サイドボタンにボタンを追加する
        /// </summary>
        private void AddSideButtons()
        {
            foreach(ViewName name in Enum.GetValues(typeof(ViewName)))
            {
                string btnName = string.Empty;
                if (this.m_PopupWindowMap.ContainsKey(name))
                {
                    btnName = this.m_PopupWindowMap[name].name;
                }
                else if (this.m_ViewPageMap.ContainsKey(name))
                {
                    btnName = this.m_ViewPageMap[name].name;
                }
                else
                {
                    continue;
                }

                Button button = new Button()
                {
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Tag = name,
                    Template = this.Resources["SideButtonTemplate"] as ControlTemplate,
                    FontFamily = Application.Current.Resources["MainFont"] as System.Windows.Media.FontFamily,
                    FontSize = (double)Application.Current.Resources["M_TextSize"],
                    Content = btnName
                };
                button.Click += this.Button_Click;
                
                this.m_SideBar.Children.Add(button);
            }
        }

        /* イベントハンドラ */
        #region 変更不要
        //SideArea内のボタンクリックイベントハンドラ
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectButton = sender as Button;

            WindowStruct tmpWindowStruct;
            PageStruct tmpPageStruct;
            if (this.m_PopupWindowMap.TryGetValue((ViewName)(selectButton.Tag), out tmpWindowStruct))
            {
                this.OpenWindow(tmpWindowStruct.window);
            }
            else if(this.m_ViewPageMap.TryGetValue((ViewName)(selectButton.Tag), out tmpPageStruct))
            {
                this.ShowPage(tmpPageStruct.page);
            }
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //親のWindowを取得
            this.parentWindow = (Window)this.Parent;
            this.AddEventHandler();
            this.AddSideButtons();
            
        }
        
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (var value in this.m_PopupWindowMap)
            {
                value.Value.window.Close();
            }
            this.m_PopupWindowMap.Clear();
        }
        
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.WindowState = WindowState.Minimized;
        }
        
        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeMaximizedWindowState();
        }
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.Close();
        }
        
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MainIcon_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 変更不要
    }
}
