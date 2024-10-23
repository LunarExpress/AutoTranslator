using FolderBrowserForWPF;
using Microsoft.Win32;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace AutoTranslate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string regex;
        public MainWindow()
        {
            InitializeComponent();
        }
        public string GetComboBox() 
        {
            string from = "";
            ComboBox lang = (ComboBox)FindName("Lang");
            switch (lang.SelectedIndex)
            {
                case 0:
                    from = "en";
                    break;
                case 1:
                    from = "pt";
                    break;
                case 2:
                    from = "jp";
                    break;
                case 3:
                    from = "spa";
                    break;
                case 4:
                    from = "ru";
                    break;
                case 5:
                    from = "auto";
                    break;
                default:
                    from = "en";
                    break;
            }
            return from;
        }
        public void SetDataBase(object sender, RoutedEventArgs e)
        {
            Dialog dialog = new Dialog();
            string FolderPath = "";
            string from = GetComboBox();
            if (dialog.ShowDialog() == true)
            {
                FolderPath = dialog.FileName;
                MainStream(FolderPath,"zh",from);
                return;
            }
            else 
                return;
        }

        public void SetAdconfig(object sender, RoutedEventArgs e) 
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string FilePath = "";
            if (dialog.ShowDialog() == true)
            {
                FilePath = dialog.FileName;
                AdConfigStream(FilePath);
                return;
            }
            else
                return;
        }

        public void SetRegMode(object sender, RoutedEventArgs e) 
        {
            Dialog dialog = new Dialog();
            string FolderPath = "";
            if (dialog.ShowDialog() == true)
            {
                FolderPath = dialog.FileName;
                RegStream(FolderPath);
                return;
            }
            else
                return;
        }

        public void ShowLog(string content) 
        {
            TextBlock LogBlock = (TextBlock)FindName("Log");
            LogBlock.Text = content;
        }
        public void SetRegex(object sender, TextChangedEventArgs e) 
        {
            TextBox textBox = (TextBox)sender;
            regex = @textBox.Text;
        }
        public void MainStream(string FolderPath,string to ="zh",string from = "en") 
        {
             FileStream.FolderTranslate(regex,FolderPath,to,from);
        }

        public void RegStream(string FolderPath) 
        {
             FileStream.FolderRegexTranslate(regex,FolderPath);
        }


        public void AdConfigStream(string FilePath) 
        {
            string to = "zh";
            string from = GetComboBox();
            FileStream.AdConfigTranslate(FilePath,from,to);

        }
    }
}
