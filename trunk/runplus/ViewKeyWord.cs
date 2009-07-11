using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace runplus
{
    public partial class ViewKeyWord : Form
    {
        public ViewKeyWord()
        {
            InitializeComponent();
            this.listView1.View = View.Details;
            //this.listView1.Dock = DockStyle.Bottom;

            this.listView1.Columns.Add("key");
            this.listView1.Columns[0].Width = 20;
            this.listView1.Columns.Add("filename");
            this.listView1.Columns[1].Width = 30;
            this.listView1.Columns.Add("fullname");
            this.listView1.Columns[2].Width = 300;
            this.Text = "View Keywords ";            
        }
        public void DoShow(Hashtable ht)
        {
            foreach (DictionaryEntry k in ht)
            {
                ListViewItem item = this.listView1.Items.Add(k.Key.ToString());
                Link link = (Link)k.Value;
                if (link != null)
                {
                    item.SubItems.Add(link.FileName);
                    item.SubItems.Add(link.FullFileName);
                }
            }
            Show();
        }
        
    }
}
