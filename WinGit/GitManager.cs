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
        bool showIgnored;

        public GitManager() { } //Default constructor, unused
        public GitManager(ref ListBox _fileList, ref ScrollViewer _scrollBar, ref TextBlock _readOut, ref bool _showIgnored)
        {
            fileList = _fileList;
            scrollBar = _scrollBar;
            readOut = _readOut;
            showIgnored = _showIgnored;
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
                fileList.Items.Add(newItem); //The first 3 characters are the tracking indicator
            }
            //Print("Status refreshed.");
        }

        public void PrintMessage(string message)
        {
            readOut.Text += "\n" + message;
            scrollBar.ScrollToEnd();
        }

        //Git Functions
        private void GitStatus()
        {
            if (!showIgnored)
                InputArgs("git status --porcelain");
            else InputArgs("git status --porcelain --ignored");
        }

        private void AddFileToCommit()
        {
            string itemContent = "";
            try
            {
                itemContent = ((ListBoxItem)OutputBlock.SelectedItem).Content.ToString();
                InputArgs(@"git add """ + itemContent + @"""");
                Print("\"" + itemContent + "\" staged for commit.");
            }
            catch (NullReferenceException e)
            {
                Print("No file selected.");
            }
            GitStatus();
        }

        private void AddAllFiles()
        {
            InputArgs("git add .");
            GitStatus();
        }

        private void RemoveFileFromCommit()
        {
            string itemContent = "";
            try
            {
                itemContent = ((ListBoxItem)OutputBlock.SelectedItem).Content.ToString();
                InputArgs(@"git reset HEAD -- """ + itemContent + @"""");
                Print("\"" + itemContent + "\" removed from staging.");
            }
            catch (NullReferenceException e)
            {
                Print("No file selected.");
            }
            GitStatus();
        }

        private void RemoveAllFiles()
        {
            InputArgs("git reset HEAD -- .");
            GitStatus();
        }

        private void ClearFiles()
        {
            GitStatus();
        }
    }
}
