using System;
using System.Collections.Generic;
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

namespace WinGit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string workingDir;
        public string bashDir;

        public MainWindow()
        {
            InitializeComponent();
            workingDir = System.IO.Directory.GetCurrentDirectory();
            bashDir = workingDir + "\\Git\\git-cmd.exe";
        }

        private void StartBash(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(bashDir);
        }

        private void InputToGit(string command)
        {

        }
    }
}
