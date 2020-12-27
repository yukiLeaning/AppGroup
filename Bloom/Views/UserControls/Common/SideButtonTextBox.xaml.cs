using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Bloom.Views.UserControls.Common
{
    internal class SideButtonTextBox_ModelView
    {
        private string _TextBoxText;
        public string TextBoxText
        {
            get { return _TextBoxText; }
            set
            {
                if(this._TextBoxText == value)
                {
                    return;
                }
                this._TextBoxText = value;
                this.OnPropertyChanged(nameof(TextBoxText));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// SideButtonTextBox.xaml の相互作用ロジック
    /// </summary>
    public partial class SideButtonTextBox : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(SideButtonTextBox_ModelView),
                new PropertyMetadata("default text")
                );

        public string ButtonText
        {
            set
            {
                if(value == this.m_Button.Content as string)
                {
                    return;
                }
                this.m_Button.Content = value;
            }
        }

        public string Text
        {
            get
            {
                return this.m_TextBox.Text;
            }
            set
            {
                if(value == this.m_TextBox.Text)
                {
                    return;
                }
                this.m_TextBox.Text = value;
            }
        }

        public event EventHandler _Button_Click;
        public EventHandler Button_Click
        {
            private get
            {
                return _Button_Click;
            }
            set
            {
                _Button_Click = value;
            }
        }

        public SideButtonTextBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_Button_Click(object sender, RoutedEventArgs e)
        {
            if(this.Button_Click == null)
            {
                return;
            }
            this.Button_Click(sender, e);
        }
    }
}
