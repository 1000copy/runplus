using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace runplus
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.listView1.View = View.Details;
            //this.listView1.Dock = DockStyle.Bottom;

            this.listView1.Columns.Add("name");
            this.listView1.Columns[0].Width = 200;
            this.listView1.Columns.Add("fullname");
            this.listView1.Columns[1].Width = 300;
            this.listView1.Columns[1].Width = 0;
            this.Text = "Runplus";
            Hotkey.Regist(this.Handle, HotkeyModifiers.MOD_WIN, Keys.Space, Test);
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.listView1.HideSelection = false;
            this.listView1.MultiSelect = false;
        }
        void Test()
        {
            //MessageBox.Show("Test");
            //this.TopMost = true;
            this.Visible = !this.Visible ;
            if (this.Visible)
            {
                ShellHelper.SwitchToThisWindow(this.Handle, true);
                this.textBox1.Focus();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            Hotkey.ProcessHotKey(m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        } 
        private Hashtable ht = new Hashtable();


        private void doRefresh(string query)
        {
            this.listView1.Items.Clear();
            foreach (DictionaryEntry k in ht)
            {
                if (query == "" ||k.Key.ToString().Contains(query))
                {
                    ListViewItem lvi = this.listView1.Items.Add(k.Key.ToString());
                    lvi.SubItems.Add(k.Value.ToString());
                }
            }
            if (this.listView1.Items.Count > 0)
            {
                this.pictureBox1.Show();
                string Path2Link = this.listView1.Items[0].SubItems[1].Text;
                this.pictureBox1.Image = Icon.FromHandle(ShellHelper.GetIcon(Path2Link)).ToBitmap();
            }
            else
                this.pictureBox1.Hide();

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
 
            if (this.listView1.SelectedIndices.Count > 0)
            {
                int index = this.listView1.SelectedIndices[0];
                string Path2Link = this.listView1.Items[index].SubItems[1].Text;
                this.pictureBox1.Image = Icon.FromHandle(ShellHelper.GetIcon(Path2Link)).ToBitmap();
                
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
 
        }

        private void button2_Click(object sender, EventArgs e)
        {
   
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            doRefresh(this.textBox1.Text.ToLower());
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.listView1.Items.Count == 0) return;
            if (e.KeyCode == Keys.Enter)
            {
                if (this.listView1.Items.Count > 0)
                {
                    string Path2Link = "";
                    if (this.listView1.SelectedIndices.Count == 0)
                    {
                       Path2Link = this.listView1.Items[0].SubItems[1].Text;
                        
                    }else
                        Path2Link = this.listView1.Items[this.listView1.SelectedIndices[0]].SubItems[1].Text;
                    Process.Start(Path2Link);
                    this.Visible = false;
                }
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode ==Keys.Down)
            {
                if (this.listView1.SelectedIndices.Count == 0 )
                    this.listView1.SelectedIndices.Add(0);
                int newindex = e.KeyCode == Keys.Up ? this.listView1.SelectedIndices[0] - 1 : this.listView1.SelectedIndices[0] + 1;
                if (newindex >=0 && newindex <= this.listView1.Items.Count -1)
                    this.listView1.SelectedIndices.Add(newindex);

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string currdir = ShellHelper.GetStartMenu();
            //this.listView1.Items.Add(path);
            DirectoryInfo di = new DirectoryInfo(currdir);
            FileInfo[] rgFiles = di.GetFiles("*.lnk", SearchOption.AllDirectories);
            foreach (FileInfo fi in rgFiles)
            {
                string fullfilename = fi.DirectoryName + "\\" + fi.Name;
                string filename = fi.Name.ToLower();
                if (!ht.Contains(filename))
                    ht.Add(filename, fullfilename);
            }
            doRefresh("");
            this.Visible = false;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listView1.SelectedIndices.Count > 0)
            {
                int index = this.listView1.SelectedIndices[0];
                string Path2Link = this.listView1.Items[index].SubItems[1].Text;
                this.pictureBox1.Image = Icon.FromHandle(ShellHelper.GetIcon(Path2Link)).ToBitmap();
                Process.Start(Path2Link);
                this.Visible = false;
            }
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hotkey.UnRegist(this.Handle, Test);
        }
    }
}
