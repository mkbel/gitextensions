﻿using System;
using System.IO;
using System.Windows.Forms;
using GitCommands;
using GitCommands.Repository;
using ResourceManager;

namespace GitUI.CommandsDialogs
{
    public partial class FormInit : GitExtensionsForm
    {
        private readonly TranslationString _chooseDirectory =
            new TranslationString("Please choose a directory.");

        private readonly TranslationString _chooseDirectoryCaption =
            new TranslationString("Choose directory");

        private readonly TranslationString _chooseDirectoryNotFile =
            new TranslationString("Cannot initialize a new repository on a file.\nPlease choose a directory.");

        private readonly TranslationString _chooseDirectoryNotFileCaption =
            new TranslationString("Error");

        private readonly TranslationString _initMsgBoxCaption =
            new TranslationString("Create new repository");

        private readonly EventHandler<GitModuleEventArgs> GitModuleChanged;

        public FormInit(string dir, EventHandler<GitModuleEventArgs> GitModuleChanged)
        {
            this.GitModuleChanged = GitModuleChanged;
            InitializeComponent();
            Translate();

            Directory.DataSource = Repositories.RepositoryHistory.Repositories;
            Directory.DisplayMember = "Path";
            Directory.SelectedIndex = -1;

            if (string.IsNullOrEmpty(dir))
            {
                Directory.Text = AppSettings.DefaultCloneDestinationPath;
            }
            else
            {
                Directory.Text = dir;
            }
        }

        private void InitClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Directory.Text))
            {
                MessageBox.Show(this, _chooseDirectory.Text, _chooseDirectoryCaption.Text);
                return;
            }

            if (File.Exists(Directory.Text))
            {
                MessageBox.Show(this, _chooseDirectoryNotFile.Text, _chooseDirectoryNotFileCaption.Text);
                return;
            }

            GitModule module = new GitModule(Directory.Text);

            if (!System.IO.Directory.Exists(module.WorkingDir))
                System.IO.Directory.CreateDirectory(module.WorkingDir);

            MessageBox.Show(this, module.Init(Central.Checked, Central.Checked), _initMsgBoxCaption.Text);

            if (GitModuleChanged != null)
                GitModuleChanged(this, new GitModuleEventArgs(module));

            Repositories.AddMostRecentRepository(Directory.Text);

            Close();
        }

        private void BrowseClick(object sender, EventArgs e)
        {
            var userSelectedPath = OsShellUtil.PickFolder(this);

            if (userSelectedPath != null)
            {
                Directory.Text = userSelectedPath;
            }
        }
    }
}