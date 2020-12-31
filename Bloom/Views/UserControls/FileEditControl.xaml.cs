using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using Bloom.DataObjects;

namespace Bloom.Views.UserControls
{
    /// <summary>
    /// FileEditControl ViewModel
    /// </summary>
    internal class FileEditControl_ViewModel : INotifyPropertyChanged
    {
        private string _Name;
        public string Name 
        {
            get { return this._Name; }
            set
            {
                if(this._Name == value)
                {
                    return;
                }
                this._Name = value;
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        private string _FullName;
        public string FullName 
        {
            get { return this._FullName; }
            set
            {
                if(this._FullName == value)
                {
                    return;
                }
                this._FullName = value;
                this.OnPropertyChanged(nameof(this.FullName));
            }
        }

        private long _Size;
        public long Size 
        {
            get { return this._Size; }
            set
            {
                if(this._Size == value)
                {
                    return;
                }
                this._Size = value;
                this.OnPropertyChanged(nameof(this.Size));
            }
        }

        private DateTime _CreateTime;
        public DateTime CreateTime 
        {
            get { return this._CreateTime; }
            set
            {
                if(this._CreateTime == value)
                {
                    return;
                }
                this._CreateTime = value;
                this.OnPropertyChanged(nameof(this.CreateTime));
            }
        }

        private DateTime _UpdateTime;
        public DateTime UpdateTime 
        {
            get { return this._UpdateTime; }
            set
            {
                if(this._UpdateTime == value)
                {
                    return;
                }
                this._UpdateTime = value;
                this.OnPropertyChanged(nameof(this.UpdateTime));
            }
        }
        
        private string _Owner;
        public string Owner
        {
            get { return this._Owner; }
            set
            {
                if(this._Owner == value)
                {
                    return;
                }
                this._Owner = value;
                this.OnPropertyChanged(nameof(this.Owner));
            }
        }

        private string _OriginalName;
        public string OriginalName
        {
            get { return this._OriginalName; }
            set
            {
                if(this._OriginalName == value)
                {
                    return;
                }
                this._OriginalName = value;
                this.OnPropertyChanged(nameof(this.OriginalName));
            }
        }

        private string _Attributes;
        public string Attributes
        {
            get { return this._Attributes; }
            set
            {
                if(this._Attributes == value)
                {
                    return;
                }
                this._Attributes = value;
                this.OnPropertyChanged(nameof(this.Attributes));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// FileEditControl.xaml の相互作用ロジック
    /// </summary>
    public partial class FileEditControl : UserControl
    {
        FileEditControl_ViewModel viewModel = new FileEditControl_ViewModel();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileEditControl()
        {
            this.InitializeComponent();
            this.DataContext = this.viewModel;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        public void SetFileInfomation(SFileInfo fileInfo)
        {
            this.viewModel.Name = fileInfo.Name;
            this.viewModel.FullName = fileInfo.FullName;
            this.viewModel.Size = fileInfo.Size;
            this.viewModel.CreateTime = fileInfo.CreateTime;
            this.viewModel.UpdateTime = fileInfo.UpdateTime;
            this.viewModel.Owner = fileInfo.Owner;
            this.viewModel.OriginalName = fileInfo.OriginalName;
            this.viewModel.Attributes = fileInfo.Attributes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SFileInfo GetFileInfo()
        {
            SFileInfo fileInfo = new SFileInfo()
            {
                Name = this.viewModel.Name,
                FullName = this.viewModel.FullName,
                Size = this.viewModel.Size,
                CreateTime = this.viewModel.CreateTime,
                UpdateTime = this.viewModel.UpdateTime,
                Owner = this.viewModel.Owner,
                OriginalName = this.viewModel.OriginalName,
                Attributes = this.viewModel.Attributes
            };
            return fileInfo;
        }
    }
}
