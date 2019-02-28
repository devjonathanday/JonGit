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
        string recentReposFileName = (Directory.GetCurrentDirectory() + "\\recentRepos.txt"); //Filename for list of recent repos.

        public MainWindow()
        {
            InitializeComponent();
            //ResizeMode = ResizeMode.NoResize;
            GM = new GitManager(ref OutputBlock, ref ReadoutScrollViewer, ref ReadoutBlock);
            if(!File.Exists(recentReposFileName)) File.Create(recentReposFileName); //Creates the recent repo text file if it doesn't exist.
            UpdateRecentRepos();
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
            //string[] repoList = File.ReadAllLines(recentReposFileName); //Read repo names into an array
            //string[] newRepoList = new string[5]; //Create new array 1 element larger
            //newRepoList[0] = repoDirText.Text; //Set the first element of the new array
            //for(int i = 1; i < Math.Min(5, repoList.Length); i++) //Iterate through the list, or just go to 5 if it is too large
            //{
            //    newRepoList[i] = repoList[]
            //}

            //if (!RecentRepoDirsList.Items.Contains(repoDirText.Text))
            //{
            //    //RecentRepoDirsList.Items.Insert(0, repoDirText.Text);
            //    //while (RecentRepoDirsList.Items.Count > 3) RecentRepoDirsList.Items.RemoveAt(RecentRepoDirsList.Items.Count - 1);
            //    //FileStream recentRepos = File.Open(recentReposFileName, FileMode.Open);
                
            //}
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
        public void UpdateRecentRepos() //Iterates through text file and updates ComboBox with recently pushed repos
        {
            string[] recentRepos = File.ReadAllLines(recentReposFileName); //Read the repo names and put them into a string array.
            for (int i = 0; i < recentRepos.Length; i++) RecentRepoDirsList.Items.Add(recentRepos[i]); //Add the repo names into the combo box.
        }
    }
}
