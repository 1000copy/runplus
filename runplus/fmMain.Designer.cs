﻿namespace runplus
{
    partial class fmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lvLinks = new System.Windows.Forms.ListView();
            this.edQuery = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnViewKeywords = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lvLinks
            // 
            this.lvLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLinks.HideSelection = false;
            this.lvLinks.Location = new System.Drawing.Point(12, 56);
            this.lvLinks.Name = "lvLinks";
            this.lvLinks.Size = new System.Drawing.Size(527, 99);
            this.lvLinks.TabIndex = 0;
            this.lvLinks.UseCompatibleStateImageBehavior = false;
            this.lvLinks.SelectedIndexChanged += new System.EventHandler(this.lvLinks_SelectedIndexChanged);
            this.lvLinks.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // edQuery
            // 
            this.edQuery.Location = new System.Drawing.Point(12, 33);
            this.edQuery.Name = "edQuery";
            this.edQuery.Size = new System.Drawing.Size(527, 21);
            this.edQuery.TabIndex = 5;
            this.edQuery.TextChanged += new System.EventHandler(this.edQuery_Changed);
            this.edQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(112, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(39, 33);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnViewKeywords
            // 
            this.btnViewKeywords.Location = new System.Drawing.Point(226, 4);
            this.btnViewKeywords.Name = "btnViewKeywords";
            this.btnViewKeywords.Size = new System.Drawing.Size(35, 23);
            this.btnViewKeywords.TabIndex = 7;
            this.btnViewKeywords.Text = "Keywords";
            this.btnViewKeywords.UseVisualStyleBackColor = true;
            this.btnViewKeywords.Click += new System.EventHandler(this.btnViewKeywords_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(185, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(35, 23);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "R";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.button1_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 158);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnViewKeywords);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.edQuery);
            this.Controls.Add(this.lvLinks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmMain";
            this.Opacity = 0.9;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fmMain";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form1_PreviewKeyDown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvLinks;
        private System.Windows.Forms.TextBox edQuery;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnViewKeywords;
        private System.Windows.Forms.Button btnRefresh;
    }
}

