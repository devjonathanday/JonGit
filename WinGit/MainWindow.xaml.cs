//using System;
//using System.Collections.Generic;
using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Forms;

namespace WinGit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Process commandWindow = new Process();
        public GitManager GM = new GitManager();
        public FileStream reposFS = File.Open(Directory.GetCurrentDirectory() + "\\recentRepos.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite); //Creates the text file to be used
        
        public string[] recentRepos;

        public MainWindow()
        {
            InitializeComponent();
            //ResizeMode = ResizeMode.NoResize;
            GM = new GitManager(ref OutputBlock, ref ReadoutScrollViewer, ref ReadoutBlock);
            reposFS.Close();
            recentRepos = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\recentRepos.txt"); //Reads the lines from the text file
            for(int i = 0; i < recentRepos.Length; i++) RecentRepoDirsList.Items.Add(recentRepos[i]);
        }

        //WPF Buttons
        private void GitStatusButton(object sender, RoutedEventArgs e) { GM.GitStatus(repoDirText.Text); }
        private void AddFileButton(object sender, RoutedEventArgs e) { GM.AddFileToCommit(repoDirText.Text); }
        private void AddAllButton(object sender, RoutedEventArgs e) { GM.AddAllFiles(repoDirText.Text); }
        private void RemoveFileButton(object sender, RoutedEventArgs e) { GM.RemoveFileFromCommit(repoDirText.Text); }
        private void RemoveAllButton(object sender, RoutedEventArgs e) { GM.RemoveAllFiles(repoDirText.Text); }
        private void IgnoredFilesCheckBox_Checked(object sender, RoutedEventArgs e) { GM.showIgnored = true; GM.GitStatus(repoDirText.Text); }
        private void IgnoredFilesCheckBox_Unchecked(object sender, RoutedEventArgs e) { GM.showIgnored = false; GM.GitStatus(repoDirText.Text); }
        private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e) { if (e.Key == Key.Return || e.Key == Key.Enter) GM.GitStatus(repoDirText.Text); }
        private void CommitButton(object sender, RoutedEventArgs e) { GM.GitCommit(CommitMessageText.Text, repoDirText.Text); }
        private void OpenCMDButton(object sender, RoutedEventArgs e) { GM.OpenCMD(repoDirText.Text); }
        private void GitInitButton(object sender, RoutedEventArgs e) { GM.GitInit(repoDirText.Text); }
        private void AddOriginButton(object sender, RoutedEventArgs e) { GM.RemoteAddOrigin(WebOriginTextBox.Text, repoDirText.Text); }
        private void GitPushButton(object sender, RoutedEventArgs e)
        {
            GM.GitPush(repoDirText.Text);
            RecentRepoDirsList.Items.Add(repoDirText.Text);
        }
        private void BrowseForRepoDir(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            repoDirText.Text = folderBrowser.SelectedPath;
        }
        private void GitPullButton(object sender, RoutedEventArgs e) { GM.GitPull(repoDirText.Text, WebOriginTextBox.Text); }
        private void SetUpstreamCheckBox_Unchecked(object sender, RoutedEventArgs e) { GM.setUpstream = false; }
        private void SetUpstreamCheckBox_Checked(object sender, RoutedEventArgs e) { GM.setUpstream = false; }
        private void UpdateRepoDir(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { repoDirText.Text = RecentRepoDirsList.SelectedItem.ToString(); }
    }
}
