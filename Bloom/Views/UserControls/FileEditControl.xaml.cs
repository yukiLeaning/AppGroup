using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private FileAttributes _Attributes;
        public FileAttributes Attributes
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
        public FileEditControl()
        {
            InitializeComponent();
        }
    }
}
