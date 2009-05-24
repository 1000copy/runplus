using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
            FolderZipper.ZipUtil.ZipFiles(this.txtFolderName.Text, this.txtZipFileName.Text, txtPassword.Text);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderZipper.ZipUtil.UnZipFiles(this.txtZipFileName.Text, this.txtFolderName.Text, txtPassword.Text,false);

        }
    }
}