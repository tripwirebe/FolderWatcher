using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace FolderWatcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSourceSelect_Click(object sender, EventArgs e)
        {
            fldrdSelect.ShowDialog();
            Properties.Settings.Default.SourcePath = fldrdSelect.SelectedPath;
            txtSource.Text = Properties.Settings.Default.SourcePath;
            Properties.Settings.Default.Save();
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            fldrdSelect.ShowDialog();
            Properties.Settings.Default.TargetPath = fldrdSelect.SelectedPath;
            txtTarget.Text = Properties.Settings.Default.TargetPath;
            Properties.Settings.Default.Save();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            fsWatcher.Path = Properties.Settings.Default.SourcePath;
            fsWatcher.Created += FsWatcher_Created;
            btnSave.Enabled = false;
        }

        private void FsWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            WaitForFile(e.FullPath);
            if (e.FullPath.EndsWith(".png") || e.FullPath.EndsWith(".jpg"))
            {
                try
                {
                    File.Copy(e.FullPath, string.Concat(Properties.Settings.Default.TargetPath, "\\", e.Name), true);
                }
                catch (IOException ex)
                {
                    throw;
                }
            }
            File.Delete(e.FullPath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fsWatcher.Created -= FsWatcher_Created;
            button1.Enabled = false;
        }
        private void WaitForFile(string fullPath)
        {
            while (true)
            {
                try
                {
                    using (StreamReader stream = new StreamReader(fullPath))
                    {
                        break;
                    }
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}