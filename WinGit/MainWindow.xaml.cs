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
            if (!File.Exists(recentReposFileName)) File.Create(recentReposFileName); //Creates the recent repo text file if it doesn't exist.
            //UpdateRecentRepos();
        }

        //WPF Buttons
        private void GitStatusButton(object sender, RoutedEventArgs e)
        {
            GM.GitStatus(repoDirText.Text);

            //Update recent repositories
            string[] repoArr = File.ReadAllLines(recentReposFileName); //Read repo names from file, put into an array
            if (repoArr.Length == 0) { File.WriteAllText(recentReposFileName, repoDirText.Text); }
            else
            {
                List<string> repoList = new List<string>(); //Convert array into list
                repoList.Add(repoDirText.Text); //Add the current repo to the top of the list
                for (int i = 0; i < Math.Min(repoArr.Length, 4); i++) //Iterate through the list up to 5 times
                    if (repoArr[i] != repoDirText.Text) //Check for duplicates
                        repoList.Add(repoArr[i]); //Add the rest of the repos into the list
                File.WriteAllLines(recentReposFileName, repoList.ToArray()); //Convert back into an array for writing
            }
            UpdateRecentRepos(); //Update the combo box
        }
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
        private void GitPushButton(object sender, RoutedEventArgs e) { GM.GitPush(repoDirText.Text); }
        private void BrowseForRepoDir(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            repoDirText.Text = folderBrowser.SelectedPath;
        }
        private void GitPullButton(object sender, RoutedEventArgs e) { GM.GitPull(repoDirText.Text, WebOriginTextBox.Text); }
        private void SetUpstreamCheckBox_Unchecked(object sender, RoutedEventArgs e) { GM.setUpstream = false; }
        private void SetUpstreamCheckBox_Checked(object sender, RoutedEventArgs e) { GM.setUpstream = false; }
        private void UpdateRepoDir(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { if(RecentRepoDirsList.SelectedItem != null) repoDirText.Text = RecentRepoDirsList.SelectedItem.ToString(); }
        public void UpdateRecentRepos() //Iterates through text file and updates ComboBox with recently pushed repos
        {
            string[] recentRepos = File.ReadAllLines(recentReposFileName); //Read the repo names and put them into a string array.
            RecentRepoDirsList.Items.Clear();
            for (int i = 0; i < recentRepos.Length; i++) RecentRepoDirsList.Items.Add(recentRepos[i]); //Add the repo names into the combo box.
        }
        private void FocusedRecentsBox(object sender, RoutedEventArgs e) { UpdateRecentRepos(); }
    }
}
