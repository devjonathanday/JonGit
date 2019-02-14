using System;
using System.Collections.Generic;
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
using System.Diagnostics;

namespace WinGit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string workingDir;
        public string bashDir;
        public StringWriter sw = new StringWriter();
        Process bash = new Process();
        //public ProcessStartInfo bash;

        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            workingDir = System.IO.Directory.GetCurrentDirectory();
            bashDir = bashDirTextBox.Text;
            bash.StartInfo.FileName = bashDir;
            bash.Start();
        }

        private void InputArgs(string newArgs)
        {
            //Process[] currentProcesses = Process.GetProcesses();
            //foreach (Process p in currentProcesses)
            //    OutputText.Text += p.ProcessName;
            //bash.StartInfo.FileName = bashDir;
            //bash.StartInfo.Arguments = newArgs;
            bash.Start();
        }

        private void CleanUpGit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //bash.Kill();
        }

        private void GitStatus(object sender, RoutedEventArgs e)
        {
            InputArgs("git status");
        }

        private void ChangeDir(object sender, RoutedEventArgs e)
        {
            InputArgs("cd ..");
        }
    }
}
