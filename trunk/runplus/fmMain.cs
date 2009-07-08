using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;


namespace runplus
{

    public partial class fmMain : Form
    {
        private RunLinks links = new RunLinks();
        public fmMain()
        {
            InitializeComponent();
            this.lvLinks.View = View.Details;
            //this.listView1.Dock = DockStyle.Bottom;

            this.lvLinks.Columns.Add("name");
            this.lvLinks.Columns[0].Width = 200;
            this.lvLinks.Columns.Add("fullname");
            this.lvLinks.Columns[1].Width = 300;
            this.lvLinks.Columns.Add("keyname");
            this.lvLinks.Columns[2].Width = 100;
            //this.lvLinks.Columns[1].Width = 0;
            this.Text = "Runplus";
       
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.lvLinks.HideSelection = false;
            this.lvLinks.MultiSelect = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ControlBox = true;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.timer1.Enabled = false;
        }
        void DoHotKey()
        {
            this.Visible = !this.Visible ;
            if (this.Visible)
            {
                ShellHelper.SwitchToThisWindow(this.Handle, true);
                this.edQuery.Focus();
                this.edQuery.SelectAll();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            Hotkey.ProcessHotKey(m);
        }


       


        private void doRefresh(string query,Hashtable ht)
        {
            this.lvLinks.Items.Clear();
            // 
            string querykey = query.ToLower();                    
            if (querykey == "")
            {
                foreach (DictionaryEntry k in ht)
                {
                    RunLink link = ((RunLink)(k.Value));
                    ListViewItem lvi = this.lvLinks.Items.Add(link.FileName);
                    lvi.SubItems.Add(link.FullFileName);
                    lvi.Tag = link;
                }
            }
            else
            {
                // 优先列出相等的
                foreach (DictionaryEntry k in ht)
                {                    
                    if (IsEquals(querykey,  k))
                    {
                        DoPopulate(k);
                    }

                }
                // 其实列出打头的
                foreach (DictionaryEntry k in ht)
                {
                    string innerkey = k.Key.ToString();
                    if (!IsEquals(querykey, k) && IsStartWiths(querykey,k))
                    {
                        DoPopulate(k);
                    }
                }
                // 然后列出包含的
                foreach (DictionaryEntry k in ht)
                {
                    string innerkey = k.Key.ToString();
                    if (!innerkey.Equals(querykey) && !innerkey.StartsWith(querykey) && IsContains(querykey,k))
                    {
                        DoPopulate(k);
                    }
                }
            }
            if (this.lvLinks.Items.Count > 0)
            {
                this.pictureBox1.Show();
                string Path2Link = this.lvLinks.Items[0].SubItems[1].Text;
                try
                {
                    this.pictureBox1.Image = Icon.FromHandle(ShellHelper.GetIcon(Path2Link)).ToBitmap();
                }
                catch
                {
                }
            }
            else
                this.pictureBox1.Hide();

        }

        private static bool IsEquals(string querykey, DictionaryEntry k)
        {
            string innerkey = k.Key.ToString();
            return (k.Value.GetType() == typeof(RunLink) && innerkey.Equals(querykey))
                                    || (k.Value.GetType() == typeof(KeyRunLink) && ((KeyRunLink)(k.Value)).KeyName.Equals(querykey));
        }
        private static bool IsStartWiths(string querykey, DictionaryEntry k)
        {
            string innerkey = k.Key.ToString();
            return (k.Value.GetType() == typeof(RunLink) && innerkey.StartsWith(querykey))
                                    || (k.Value.GetType() == typeof(KeyRunLink) && ((KeyRunLink)(k.Value)).KeyName.StartsWith(querykey));
        }
        private static bool IsContains(string querykey, DictionaryEntry k)
        {
            string innerkey = k.Key.ToString();
            return (k.Value.GetType() == typeof(RunLink) && innerkey.Contains(querykey))
                                    || (k.Value.GetType() == typeof(KeyRunLink) && ((KeyRunLink)(k.Value)).KeyName.Contains(querykey));
        }
        private DictionaryEntry DoPopulate(DictionaryEntry k)
        {
            RunLink link = ((RunLink)(k.Value));
            ListViewItem lvi = this.lvLinks.Items.Add(link.FileName);
            lvi.Tag = k.Value;
            lvi.SubItems.Add(link.FullFileName);
            if (k.Value.GetType()==typeof(KeyRunLink))
                lvi.SubItems.Add(((KeyRunLink)link).KeyName);
            return k;
        }

        private void lvLinks_SelectedIndexChanged(object sender, EventArgs e)
        {
 
            if (this.lvLinks.SelectedIndices.Count > 0)
            {
                int index = this.lvLinks.SelectedIndices[0];
                string Path2Link = this.lvLinks.Items[index].SubItems[1].Text;
                this.pictureBox1.Image = Icon.FromHandle(ShellHelper.GetIcon(Path2Link)).ToBitmap();
                
            }
        }



        private void edQuery_Changed(object sender, EventArgs e)
        {
            doRefresh(this.edQuery.Text.ToLower(),links.HT);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.lvLinks.Items.Count == 0) return;
            if (e.KeyCode == Keys.Enter)
            {
                if (this.lvLinks.Items.Count > 0)
                {
                    string Path2Link = "";
                    RunLink link = null;
                    if (this.lvLinks.SelectedIndices.Count == 0)
                    {
                        Path2Link = this.lvLinks.Items[0].SubItems[1].Text;
                        link = (RunLink)(this.lvLinks.Items[0].Tag);
                    }
                    else
                    {
                        Path2Link = this.lvLinks.Items[this.lvLinks.SelectedIndices[0]].SubItems[1].Text;                     
                        link = (RunLink)(this.lvLinks.Items[this.lvLinks.SelectedIndices[0]].Tag);
                    }
                    Process.Start(Path2Link);
                    this.links.AddKeyWord(edQuery.Text, link);
                    this.Visible = false;
                }
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode ==Keys.Down)
            {
                if (this.lvLinks.SelectedIndices.Count == 0 )
                    this.lvLinks.SelectedIndices.Add(0);
                int newindex = e.KeyCode == Keys.Up ? this.lvLinks.SelectedIndices[0] - 1 : this.lvLinks.SelectedIndices[0] + 1;
                if (newindex >=0 && newindex <= this.lvLinks.Items.Count -1)
                    this.lvLinks.SelectedIndices.Add(newindex);

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.btnViewKeywords.Visible = false;
            this.btnRefresh.Visible = false;
            try
            {
                Hotkey.Register(this.Handle, HotkeyModifiers.MOD_WIN, Keys.Space, DoHotKey);
            }
            catch
            {
                MessageBox.Show("hotkey register failure ! exit .");
                Application.ExitThread();
            }
            Reload();
            
        }

        private void Reload()
        {
            
            links.Populate();
            doRefresh("", links.HT);            
        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.lvLinks.SelectedIndices.Count > 0)
            {
                int index = this.lvLinks.SelectedIndices[0];
                string Path2Link = this.lvLinks.Items[index].SubItems[1].Text;
                this.pictureBox1.Image = Icon.FromHandle(ShellHelper.GetIcon(Path2Link)).ToBitmap();
                Process.Start(Path2Link);
                this.Visible = false;
            }
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hotkey.UnRegist(this.Handle, DoHotKey);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.edQuery.Focus();
            //AnimateWindow(this.Handle, 1000, AW_CENTER | AW_ACTIVATE | AW_BLEND);


        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                for (int i = 1; i < 5; i++)
                {
                    Application.DoEvents();
                    this.Opacity = i*20/10.0;
                }
                this.Visible = false;
            }
        }
        private bool direction = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > .98) 
                direction = false;
            if (this.Opacity < 0.80)
                direction = true;

            if ( direction )
            {
                this.Opacity += 0.02;
            }
            else 
            {
                this.Opacity -= 0.02;
            }

        }

        private void btnViewKeywords_Click(object sender, EventArgs e)
        {
             ViewKeyWord v = new ViewKeyWord ();
             //v.DoShow(this.links.Keywords);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reload();
        }
    }
}
