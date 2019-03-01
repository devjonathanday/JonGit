using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinGit
{
    public class GitManager
    {
        public ListBox fileList;
        public ScrollViewer scrollBar;
        public TextBlock readOut;
        public bool showIgnored;
        public bool setUpstream;
        public string branch = "master";

        public GitManager() { } //Default constructor, unused
        public GitManager(ref ListBox _fileList, ref ScrollViewer _scrollBar, ref TextBlock _readOut)
        {
            fileList = _fileList;
            scrollBar = _scrollBar;
            readOut = _readOut;
            showIgnored = false;
            setUpstream = false;
        }

        public string InputArgs(string newArgs, string repoDir)
        {
            Process newProcess = new Process();
            if (Directory.Exists(repoDir))
            {
                newProcess.StartInfo.WorkingDirectory = repoDir;
                newProcess.StartInfo.FileName = "cmd.exe";
                newProcess.StartInfo.Arguments = "/C" + newArgs;
                newProcess.StartInfo.UseShellExecute = false;
                newProcess.StartInfo.RedirectStandardOutput = true;
                newProcess.StartInfo.CreateNoWindow = true;
                newProcess.Start();
                string output = newProcess.StandardOutput.ReadToEnd();
                return output;
            }
            else
            {
                PrintMessage("Error: Repository directory not found.");
                fileList.Items.Clear();
                return string.Empty;
            }
        }

        public void OpenCMD(string repoDir)
        {
            Process newProcess = new Process();
            if (Directory.Exists(repoDir))
            {
                newProcess.StartInfo.WorkingDirectory = repoDir;
                newProcess.StartInfo.FileName = "cmd.exe";
                newProcess.Start();
            }
            else PrintMessage("Error: Repository directory not found.");

        }

        public void PrintMessage(string message)
        {
            readOut.Text += "\n" + message;
            scrollBar.ScrollToEnd();
        }

        //Git Functions

        public void GitInit(string repoDir)
        {
            PrintMessage(InputArgs("git init", repoDir));
        }

        public void GitStatus(string repoDir)
        {
            string[] stagedFileList = InputArgs("git diff --name-only --cached", repoDir).Split(new[] { "\n" }, StringSplitOptions.None);
            string[] unstagedFileList = InputArgs("git diff --name-only", repoDir).Split(new[] { "\n" }, StringSplitOptions.None);
            string[] allFiles = InputArgs("git status --porcelain --ignored", repoDir).Split(new[] { "\n" }, StringSplitOptions.None);
            fileList.Items.Clear(); //Clear the ListBox.

            //git adds new line after last file listed, therefore a blank string
            for (int i = 0; i < stagedFileList.Length - 1; i++) //Iterate through the files added to the commit
            {
                ListBoxItem newItem = new ListBoxItem(); //Initialize a new ListBoxItem
                newItem.Foreground = Brushes.Green; //Set the color
                newItem.Content = stagedFileList[i]; //Set the ListBoxItem equal to the filename
                fileList.Items.Add(newItem); //Add the item to the ListBox
            }
            //git adds new line after last file listed, therefore a blank string
            for (int i = 0; i < unstagedFileList.Length - 1; i++) //Iterate through the files NOT added to commit
            {
                ListBoxItem newItem = new ListBoxItem(); //Initialize a new ListBoxItem
                newItem.Foreground = Brushes.DarkRed; //Set the color
                newItem.Content = unstagedFileList[i]; //Set the ListBoxItem equal to the filename
                fileList.Items.Add(newItem); //Add the item to the ListBox
            }
            for (int i = 0; i < allFiles.Length - 1; i++) //git adds new line after last file listed, therefore a blank string
            {
                if (allFiles[i].Substring(0, 2).Contains("??"))
                {
                    allFiles[i] = allFiles[i].Substring(3);
                    allFiles[i].Replace("\"", string.Empty); //Remove the quotations (used for nested files, difficult to parse)
                    ListBoxItem newItem = new ListBoxItem(); //Initialize a new ListBoxItem
                    newItem.Foreground = Brushes.DarkRed; //Set the color
                    newItem.Content = allFiles[i]; //Set the ListBoxItem equal to the filename
                    fileList.Items.Add(newItem); //Add the item to the ListBox
                }
                if (showIgnored && allFiles[i].Substring(0, 2).Contains("!!"))
                {
                    allFiles[i] = allFiles[i].Substring(3);
                    allFiles[i].Replace("\"", string.Empty); //Remove the quotations (used for nested files, difficult to parse)
                    ListBoxItem newItem = new ListBoxItem(); //Initialize a new ListBoxItem
                    newItem.Foreground = Brushes.DarkSlateGray; //Set the color
                    newItem.Content = allFiles[i]; //Set the ListBoxItem equal to the filename
                    fileList.Items.Add(newItem); //Add the item to the ListBox
                }
            }
            if (fileList.Items.Count == 0) { PrintMessage("No files were changed in this repository."); }
        }

        public void AddFileToCommit(string repoDir)
        {
            string itemContent = "";
            try
            {
                itemContent = ((ListBoxItem)fileList.SelectedItem).Content.ToString();
                InputArgs("git add \"" + itemContent + "\"", repoDir);
                PrintMessage("\"" + itemContent + "\" staged for commit.");
            }
            catch (NullReferenceException e)
            {
                PrintMessage("No file selected.");
            }
            GitStatus(repoDir);
        }

        public void AddAllFiles(string repoDir)
        {
            InputArgs("git add .", repoDir);
            PrintMessage("Staged all eligible files for commit.");
            GitStatus(repoDir);
        }

        public void RemoveFileFromCommit(string repoDir)
        {
            string itemContent = "";
            try
            {
                itemContent = ((ListBoxItem)fileList.SelectedItem).Content.ToString();
                InputArgs("git reset HEAD -- \"" + itemContent + "\"", repoDir);
                PrintMessage("\"" + itemContent + "\" removed from staging.");
            }
            catch (NullReferenceException e)
            {
                PrintMessage("No file selected.");
            }
            GitStatus(repoDir);
        }

        public void RemoveAllFiles(string repoDir)
        {
            InputArgs("git reset HEAD -- .", repoDir);
            PrintMessage("Removed all files from staging.");
            GitStatus(repoDir);
        }

        public void GitCommit(string commitMessage, string repoDir)
        {
            if (commitMessage.Contains("\""))
            {
                PrintMessage("Error: Invalid character(s) in commit message.");
            }
            else
            {
                InputArgs(@"git commit -m """ + commitMessage + @"""", repoDir);
                PrintMessage("Committed to repository with message \"" + commitMessage + "\".");
            }
        }

        public void GitPush(string repoDir)
        {
            if(setUpstream) PrintMessage(InputArgs("git push --set-upstream origin master", repoDir));
            else PrintMessage(InputArgs("git push -u origin master", repoDir));
        }

        public void GitPull(string repoDir, string link)
        {
            if(link == string.Empty)
            {
                PrintMessage("Enter the web address for your repository.");
            }
            PrintMessage(InputArgs("git pull " + "https://github.com/" + link, repoDir));
        }

        public void RemoteAddOrigin(string link, string repoDir)
        {
            if (link == string.Empty)
            {
                PrintMessage("No web address entered.");
            }
            else
            {
                InputArgs("git remote add origin https://github.com/" + link, repoDir);
                PrintMessage("Added origin at https://github.com/" + link);
            }
        }
    }
}
