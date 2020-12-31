using DLL.ConvertLib;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using DLL.UtilityLib;

namespace Bloom.Views.PopupWindows
{
    /// <summary>
    /// ConvertPDFWindow ViewModel
    /// </summary>
    internal class ConvertPDFWindow_ViewModel : INotifyPropertyChanged
    {
        private string _FolderPathText;
        public string FolderPathText
        {
            get { return _FolderPathText; }
            set
            {
                if(this._FolderPathText == value)
                {
                    return;
                }
                this._FolderPathText = value;
                this.OnPropertyChanged(nameof(FolderPathText));
            }
        }

        private string _SaveFolderPathText;
        public string SaveFolderPathText
        {
            get { return _SaveFolderPathText; }
            set
            {
                if(this._SaveFolderPathText == value)
                {
                    return;
                }
                this._SaveFolderPathText = value;
                this.OnPropertyChanged(nameof(SaveFolderPathText));
            }
        }

        private bool _IsEnableConvertButton;
        public bool IsEnableConvertButton
        {
            get { return _IsEnableConvertButton; }
            set
            {
                if(this._IsEnableConvertButton == value)
                {
                    return;
                }
                this._IsEnableConvertButton = value;
                this.OnPropertyChanged(nameof(IsEnableConvertButton));
            }
        }

        private int _SelectingListBoxItemNumber = -1;
        public int SelectingListBoxItemNumber
        {
            get { return _SelectingListBoxItemNumber; }
            set
            {
                if(this._SelectingListBoxItemNumber == value)
                {
                    return;
                }
                this._SelectingListBoxItemNumber = value;
                this.OnPropertyChanged(nameof(SelectingListBoxItemNumber));
            }
        }

        private bool _IsEnableSelectButton = false;
        public bool IsEnableSelectButton
        {
            get { return _IsEnableSelectButton; }
            set
            {
                if(this._IsEnableSelectButton == value)
                {
                    return;
                }
                this._IsEnableSelectButton = value;
                this.OnPropertyChanged(nameof(IsEnableSelectButton));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    /// <summary>
    /// ConvertPDF.xaml の相互作用ロジック
    /// </summary>
    public partial class ConvertPDFWindow : Window
    {
        private readonly string[] extensions = { ".jpeg", ".jpg", ".png", ".JPEG", ".JPG", ".PNG" };

        private ConvertPDFWindow_ViewModel m_ViewModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConvertPDFWindow()
        {
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
            this.m_ViewModel = new ConvertPDFWindow_ViewModel();
            this.DataContext = this.m_ViewModel;
        }

        /// <summary>
        /// ListBoxにファイルを追加する
        /// </summary>
        /// <param name="targetPath"></param>
        private void AddTargetFile(string targetPath)
        {
            if (string.IsNullOrEmpty(targetPath))
            {
                return;
            }
            if (FFOLib.IsFile(targetPath))
            {
                this.m_ImagesListBox.Items.Add(targetPath);
            }
            else if (FFOLib.IsFolder(targetPath))
            {
                string[] filePaths = { };
                FFOLib.GetFiles(targetPath, ref filePaths, extensions);

                foreach(var file in filePaths)
                {
                    this.m_ImagesListBox.Items.Add(file);
                }
            }
            else
            {
                MessageBox.Show("失敗");
            }
        }
        
        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            foreach(var item in this.m_ImagesListBox.Items)
            {
                list.Add(item as string);
            }
            
            //非同期
            Task<bool> task = Task.Run(() =>
            {
                return PDFConvert.ConvertImage2PDF(list.Convert2Array(), this.m_ViewModel.SaveFolderPathText);
            });

            bool result = task.Result;
            MessageBox.Show((result ? "成功" : "失敗"), "結果");
        }
        
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "FolderPathText":
                {
                    this.ChangedFolderPath();
                    break;
                }
                case "IsEnableConvertButton":
                {
                    this.ChangedIsEnableConvertButton();
                    break;
                }
                case "SelectingListBoxItemNumber":
                {
                    this.ChangedSelectingListBoxItemNumber();
                    break;
                }
                case "IsEnableSelectButton":
                {
                    this.ChangedIsEnableSelectButton();
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        
        private void ChangedFolderPath()
        {
            if (string.IsNullOrEmpty(this.m_ViewModel.FolderPathText))
            {
                this.m_ViewModel.IsEnableConvertButton = false;
            }
        }
        
        private void ChangedIsEnableConvertButton()
        {
            /* NOP */
        }

        private void ChangedSelectingListBoxItemNumber()
        {
            // サイドボタンの状態を更新する
            this.m_ViewModel.IsEnableSelectButton = (0 <= this.m_ViewModel.SelectingListBoxItemNumber);
        }

        private void ChangedIsEnableSelectButton()
        {
            /* NOP */
        }
        
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string path = this.m_ViewModel.FolderPathText;
            this.AddTargetFile(path);
        }
        
        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = false;
            dialog.Title = "ファイルを選択してください";
            dialog.InitialDirectory = @"C:\";
            
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.m_ViewModel.FolderPathText = dialog.FileName;
            }
        }

        private void SaveFileSelectButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = false;
            dialog.Title = "ファイルを選択してください";
            dialog.InitialDirectory = @"C:\";

            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.m_ViewModel.SaveFolderPathText = dialog.FileName;
            }
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = true;
            dialog.Title = "フォルダを選択してください";
            dialog.InitialDirectory = @"C:\";

            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.m_ViewModel.FolderPathText = dialog.FileName;
            }
        }
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            this.m_ImagesListBox.Items.Clear();
        }

        private void ImagesListBox_DragEnter(object sender, DragEventArgs e)
        {
            /* NOP */
        }

        private void ImagesListBox_DragLeave(object sender, DragEventArgs e)
        {
            /* NOP */
        }

        private void ImagesListBox_DragOver(object sender, DragEventArgs e)
        {
            /* NOP */
        }

        private void ImagesListBox_Drop(object sender, DragEventArgs e)
        {
            string[] dropObject = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            foreach (var obj in dropObject)
            {
                AddTargetFile(obj);
            }
        }

        private void SelectButton_Up_Click(object sender, RoutedEventArgs e)
        {
            #region ガード処理
            if ((this.m_ImagesListBox.SelectedIndex < 0) || (1 < this.m_ImagesListBox.SelectedItems.Count))
            {
                return;
            }
            #endregion

            var index = this.m_ImagesListBox.SelectedIndex;
            var item = this.m_ImagesListBox.SelectedItem;
            
            if(0 < index)
            {
                var nextIndex = index - 1;
                this.m_ImagesListBox.Items.RemoveAt(index);
                this.m_ImagesListBox.Items.Insert(nextIndex, item);
                this.m_ImagesListBox.SelectedIndex = nextIndex;
            }
        }

        private void SelectButton_Down_Click(object sender, RoutedEventArgs e)
        {
            #region ガード処理
            if ((this.m_ImagesListBox.SelectedIndex < 0) || (1 < this.m_ImagesListBox.SelectedItems.Count))
            {
                return;
            }
            #endregion

            var index = this.m_ImagesListBox.SelectedIndex;
            var item = this.m_ImagesListBox.SelectedItem;
            
            if(index < (this.m_ImagesListBox.Items.Count -1))
            {
                var nextIndex = index + 1;
                this.m_ImagesListBox.Items.RemoveAt(index);
                this.m_ImagesListBox.Items.Insert(nextIndex, item);
                this.m_ImagesListBox.SelectedIndex = nextIndex;
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
    }
}
