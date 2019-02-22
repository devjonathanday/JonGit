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
        public Process commandWindow = new Process();
        bool showIgnored = false;
        public GitManager GM = new GitManager();

        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            repoDirText.Text = System.IO.Directory.GetCurrentDirectory();
            GM = new GitManager(ref OutputBlock, ref ReadoutScrollViewer, ref ReadoutBlock, ref showIgnored);
        }

        //private void InputArgs(string newArgs)
        //{
        //    Process newProcess = new Process();
        //    if (Directory.Exists(repoDirText.Text))
        //    {
        //        newProcess.StartInfo.WorkingDirectory = repoDirText.Text;
        //        newProcess.StartInfo.FileName = "cmd.exe";
        //        newProcess.StartInfo.Arguments = "/C" + newArgs;
        //        newProcess.StartInfo.UseShellExecute = false;
        //        newProcess.StartInfo.RedirectStandardOutput = true;
        //        newProcess.StartInfo.CreateNoWindow = true;
        //        newProcess.Start();
        //        string output = newProcess.StandardOutput.ReadToEnd();
        //        if (newArgs.Contains("git status")) ParseStatus(output);
        //    }
        //    else
        //    {
        //        Print("Error: Repository directory not found.");
        //        OutputBlock.Items.Clear();
        //    }
        //}

        //WPF Buttons
        private void GitStatusButton(object sender, RoutedEventArgs e) { GitStatus(); }
        private void AddFileButton(object sender, RoutedEventArgs e) { AddFileToCommit(); }
        private void AddAllButton(object sender, RoutedEventArgs e) { AddAllFiles(); }
        private void RemoveFileButton(object sender, RoutedEventArgs e) { RemoveFileFromCommit(); }
        private void RemoveAllButton(object sender, RoutedEventArgs e) { RemoveAllFiles(); }
        private void ClearButton(object sender, RoutedEventArgs e) { ClearFiles(); }
        private void IgnoredFilesCheckBox_Checked(object sender, RoutedEventArgs e) { showIgnored = true; }
        private void IgnoredFilesCheckBox_Unchecked(object sender, RoutedEventArgs e) { showIgnored = false; }

        private void ParseStatus(string output)
        {
            string[] fileNames = output.Split(new[] { "\n" }, StringSplitOptions.None);
            OutputBlock.Items.Clear();
            for (int i = 0; i < fileNames.Length - 1; i++) //Iterate through strings returned from git status
            //"--porcelain" adds extra blank line at end, hence arraySize-1
            {
                ListBoxItem newItem = new ListBoxItem(); //Initialize a new ListBoxItem
                newItem.Content = fileNames[i].Substring(3); //Set the ListBoxItem equal to the filename only
                string trackInd = fileNames[i].Substring(0, 3); //Get first 3 chars (tracking index)
                //Untracked, modified, deleted, renamed, copied, unmerged
                if (trackInd.Contains("?") ||
                    trackInd.Contains("M") ||
                    trackInd.Contains("D") ||
                    trackInd.Contains("R") ||
                    trackInd.Contains("C") ||
                    trackInd.Contains("U")) newItem.Foreground = Brushes.DarkRed;
                else if (fileNames[i].Substring(0, 3).Contains("A")) newItem.Foreground = Brushes.Green; //Added
                else if (fileNames[i].Substring(0, 3).Contains("!")) newItem.Foreground = Brushes.LightGray; //Ignored
                else newItem.Foreground = Brushes.White;
                OutputBlock.Items.Add(newItem); //The first 3 characters are the tracking indicator
            }
            //Print("Status refreshed.");
        }

        //Git Functions
        //private void GitStatus()
        //{
        //    if (!showIgnored)
        //        InputArgs("git status --porcelain");
        //    else InputArgs("git status --porcelain --ignored");
        //}

        //private void AddFileToCommit()
        //{
        //    string itemContent = "";
        //    try
        //    {
        //        itemContent = ((ListBoxItem)OutputBlock.SelectedItem).Content.ToString();
        //        InputArgs(@"git add """ + itemContent + @"""");
        //        Print("\"" + itemContent + "\" staged for commit.");
        //    }
        //    catch (NullReferenceException e)
        //    {
        //        Print("No file selected.");
        //    }
        //    GitStatus();
        //}

        //private void AddAllFiles()
        //{
        //    InputArgs("git add .");
        //    GitStatus();
        //}

        //private void RemoveFileFromCommit()
        //{
        //    string itemContent = "";
        //    try
        //    {
        //        itemContent = ((ListBoxItem)OutputBlock.SelectedItem).Content.ToString();
        //        InputArgs(@"git reset HEAD -- """ + itemContent + @"""");
        //        Print("\"" + itemContent + "\" removed from staging.");
        //    }
        //    catch (NullReferenceException e)
        //    {
        //        Print("No file selected.");
        //    }
        //    GitStatus();
        //}

        //private void RemoveAllFiles()
        //{
        //    InputArgs("git reset HEAD -- .");
        //    GitStatus();
        //}

        //private void ClearFiles()
        //{
        //    GitStatus();
        //}

        //private void Print(string message)
        //{
        //    ReadoutBlock.Text += "\n" + message;
        //    ReadoutScrollViewer.ScrollToEnd();
        //}
    }
}
