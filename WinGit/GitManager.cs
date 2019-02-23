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

        public GitManager() { } //Default constructor, unused
        public GitManager(ref ListBox _fileList, ref ScrollViewer _scrollBar, ref TextBlock _readOut)
        {
            fileList = _fileList;
            scrollBar = _scrollBar;
            readOut = _readOut;
            showIgnored = false;
        }

        public void InputArgs(string newArgs, string repoDir)
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
                if (newArgs.Contains("git status")) ParseStatus(output);
            }
            else
            {
                PrintMessage("Error: Repository directory not found.");
                fileList.Items.Clear();
            }
        }

        public void ParseStatus(string output)
        {
            string[] fileNames = output.Split(new[] { "\n" }, StringSplitOptions.None);
            fileList.Items.Clear();
            for (int i = 0; i < fileNames.Length - 1; i++) //Iterate through strings returned from git status
            //"--porcelain" adds extra blank line at end, hence arraySize-1
            {
                ListBoxItem newItem = new ListBoxItem(); //Initialize a new ListBoxItem
                string data = fileNames[i].Substring(3);
                data = data.Replace("\"", string.Empty);
                newItem.Content = data; //Set the ListBoxItem equal to the filename only
                string trackInd = fileNames[i].Substring(0, 2); //Get first 3 chars (tracking index)
                //Untracked, added, modified, deleted, renamed, copied, unmerged (?AMDRCU)
                if (trackInd == "??" ||
                    trackInd == " M" ||
                    trackInd == " D" ||
                    trackInd == " R" ||
                    trackInd == " C" ||
                    trackInd == " U") newItem.Foreground = Brushes.DarkRed;
                else if (trackInd == "AA" ||
                    trackInd == "MM" ||
                    trackInd == "DD" ||
                    trackInd == "RR" ||
                    trackInd == "CC" ||
                    trackInd == "UU" ||
                    trackInd == "A " ||
                    trackInd == "M " ||
                    trackInd == "D " ||
                    trackInd == "R " ||
                    trackInd == "C " ||
                    trackInd == "U ") newItem.Foreground = Brushes.Green;
                else if (trackInd == "!!") newItem.Foreground = Brushes.DarkGray; //Ignored
                else newItem.Foreground = Brushes.Black;
                fileList.Items.Add(newItem); //The first 3 characters are the tracking indicator
            }
        }

        public void PrintMessage(string message)
        {
            readOut.Text += "\n" + message;
            scrollBar.ScrollToEnd();
        }

        //Git Functions
        public void GitStatus(string repoDir)
        {
            if (!showIgnored) InputArgs("git status --porcelain", repoDir);
            else InputArgs("git status --porcelain --ignored", repoDir);
        }

        public void AddFileToCommit(string repoDir)
        {
            string itemContent = "";
            try
            {
                itemContent = ((ListBoxItem)fileList.SelectedItem).Content.ToString();
                InputArgs(@"git add """ + itemContent + @"""", repoDir);
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
            GitStatus(repoDir);
        }

        public void RemoveFileFromCommit(string repoDir)
        {
            string itemContent = "";
            try
            {
                itemContent = ((ListBoxItem)fileList.SelectedItem).Content.ToString();
                InputArgs(@"git reset HEAD -- """ + itemContent + @"""", repoDir);
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
            GitStatus(repoDir);
        }

        public void GitCommit(string commitMessage, string repoDir)
        {
            if(commitMessage.Contains("\""))
            {
                PrintMessage("Error: Invalid character(s) in commit message.");
            }
            else InputArgs(@"git commit -m """ + commitMessage + @"""", repoDir);
        }
    }
}
