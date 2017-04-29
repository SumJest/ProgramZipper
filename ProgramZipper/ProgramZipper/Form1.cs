using System;
using sumjest;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
namespace ProgramZipper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ArrayList<string> paths = new ArrayList<string>();

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string[] files = ofd.FileNames;
                foreach (string path in files)
                {
                    if (checkedListBox1.Items.Contains(path)) { continue; }
                    checkedListBox1.Items.Add(path, CheckState.Checked);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.Items.Count >0)
            {
                checkedListBox1.Items.RemoveAt(checkedListBox1.Items.Count-1);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!checkedListBox1.Items.Contains(ofd.SelectedPath)) checkedListBox1.Items.Add(ofd.SelectedPath, CheckState.Checked);
            }
        }


        private void FileSearchFunction(string Dir)
        {
            
            System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(Dir);
            System.IO.DirectoryInfo[] SubDir = DI.GetDirectories();
            for (int i = 0; i < SubDir.Length; ++i) this.FileSearchFunction(SubDir[i].FullName);
            System.IO.FileInfo[] FI = DI.GetFiles();
            for (int i = 0; i < FI.Length; ++i) paths.Add(FI[i].FullName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Applications | *.exe";
            ofd.Multiselect = false;
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK) { textBox1.Text = ofd.FileName; }
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 1)
            {
                MessageBox.Show("Please choose only one file", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!File.Exists(files[0]))
            {
                MessageBox.Show("Please drop file!", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Path.GetExtension(files[0]).Equals(".exe"))
            {
                MessageBox.Show("Please drop exe file!", "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            textBox1.Text = files[0];
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            InstallingOption opt =new InstallingOption(default(string[]), "", "");
            string temppath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ZipperTemp";
            if (Directory.Exists(temppath))
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (Directory.Exists(temppath + string.Format(" ({0})", i))) { continue; }
                    temppath += string.Format(" ({0})", i);
                    break;
                }
            }
            Directory.CreateDirectory(temppath);
            foreach (string item in checkedListBox1.Items)
            {
                if (File.Exists(item))
                {
                    try
                    {
                        File.Copy(item, temppath + "\\" + Path.GetFileName(item));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid file " + item + "\n" + ex.Message, ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (Directory.Exists(item))
                {
                    try
                    {
                        DirectoryCopy(item, temppath + "\\" + Path.GetFileName(item), true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid Directory " + item + "\n" + ex.Message, ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            //try
            //{
                File.Copy(textBox1.Text, temppath + "\\" + Path.GetFileName(textBox1.Text));
                opt.EXEFile = Path.GetFileName(textBox1.Text);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Invalid exe file  " + textBox1.Text + "\n" + ex.Message, ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            InstOption formIO = new InstOption();
            paths.Clear();
            //foreach (object obj in checkedListBox1.CheckedItems)
            //{
            //    if (System.IO.File.Exists(obj.ToString()))
            //    {
            //        if (System.IO.Path.GetExtension(obj.ToString()).Equals(".ico")) { formIO.comboBox1.Items.Add(System.IO.Path.GetFileName(obj.ToString())); }
            //    }
            //    if (System.IO.Directory.Exists(obj.ToString()))
            //    {
            //        FileSearchFunction(obj.ToString());
            //        foreach (string s in paths.toArray())
            //        {
            //            if (System.IO.Path.GetExtension(s).Equals(".ico")) { formIO.comboBox1.Items.Add(System.IO.Path.GetFileName(obj.ToString()) + s.Replace(obj.ToString(), "")); }
            //        }
            //    }
            //}
            FileSearchFunction(temppath);
            foreach (string s in paths.toArray())
            {
                if(Path.GetExtension(s).Equals(".ico")){ formIO.comboBox1.Items.Add(s.Replace(temppath + "\\","")); }
            }
            formIO.Owner = this;
            formIO.ShowDialog();
            opt.ExtDefaultIcon = formIO.comboBox1.SelectedItem as string;
            Console.WriteLine(formIO.comboBox1.SelectedItem);
            opt.Extensions = formIO.textBox1.Lines;
            try
            {
                StreamWriter sw =  File.CreateText(temppath + "\\" + "Installing.info");
                sw.WriteLine(opt.EXEFile);
                sw.WriteLine(opt.ExtDefaultIcon);
                foreach (string s in opt.Extensions)
                {
                    sw.WriteLine(s);
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Zip-archive file | *.zip";
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (File.Exists(sfd.FileName)) { File.Delete(sfd.FileName); }
                ZipFile.CreateFromDirectory(temppath, sfd.FileName);
                
            }
            paths.Clear();
            //FileSearchFunction(temppath);
            //foreach (string s in paths.toArray())
            //{
            //    File.Delete(s);
            //}
            Directory.Delete(temppath, true);
        }
    }
}
