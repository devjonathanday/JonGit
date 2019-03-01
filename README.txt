README
--
JonGit, Version 1.0
Developed by Jonathan Day

Extract files before running!
Place the entire JonGit folder in an accessible location.
--

USER GUIDE
--
This program is intended for use with GitHub, to easily operate Git without using the command line.
The [Manual Entry] button located in the bottom left will launch the command line, if the user feels it is necessary.

To commit and push your changed to a NEW repository:
1. Click [Browse...] and find your local folder, or select the folder from the 'Recent' drop-down box.
2. Click [Initialize] to initialize the repository in the selected directory.
3. Click [Refresh], or press ENTER from the text box to view changed files.
3. Use the [Add] and [Reset] buttons on the right to manage files staged for commit.
4. Once the intended files have been added, enter a commit message in the text box next to [Commit].
5. Click [Commit] to commit the changes to the repository.
6. Enter the URL of your Git repository and click [Add Origin].
7. click [Push] with 'Set upstream branch' checked and push to the master branch.
8. View your repository on GitHub and ensure the push was successful.

To commit and push your changes to an EXISTING repository:
Follow the same steps as above, except [Initialize] and [Add Origin] are not necessary if the repository has already been configured.
'Set upstream branch' can be left unchecked, if the remote branch has already been set up to track 'master'.

To CLONE an existing repository:
1. Click [Browse...] and find your local folder, or select the folder from the 'Recent' drop-down box.
2. Enter the URL of your Git repository, then click [Clone] to copy the files from GitHub into your local folder.

To PULL changes from GitHub:
1. Click [Browse...] and find your local folder, or select the folder from the 'Recent' drop-down box.
2. Enter the URL of your Git repository and click [Pull] to pull your changes.
--

NOTES
--
[Recent]
The 'Recent' drop down box will display a maximum of 5 repositories.
If the 'recentRepos.txt' file is moved or deleted, another will be created in its place.

[Refresh]
Filenames in GREEN have been staged for commit, files in RED have not. Ignored files, if made viewable, will be GRAY.
Nested files may not be displayed until their directory is added, which is a function of 'git status'.

[Commit/Push]
Unfortunately, this program is only able to push to the master branch of a given repository.