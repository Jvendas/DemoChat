
namespace DemoChat
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.sidebar = new System.Windows.Forms.Panel();
            this.header = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.directorySearcher2 = new System.DirectoryServices.DirectorySearcher();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.sidebar.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebar
            // 
            this.sidebar.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.sidebar.Controls.Add(this.textBox1);
            this.sidebar.Controls.Add(this.panel2);
            this.sidebar.Controls.Add(this.label2);
            this.sidebar.Controls.Add(this.label1);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.Location = new System.Drawing.Point(0, 0);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(304, 623);
            this.sidebar.TabIndex = 0;
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Location = new System.Drawing.Point(304, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(763, 77);
            this.header.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Ivory;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(304, 77);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 546);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MV Boli", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(87, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Real Chat";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MV Boli", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Conversas";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightYellow;
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 182);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(304, 441);
            this.panel2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Ivory;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(304, 72);
            this.button1.TabIndex = 0;
            this.button1.Text = "Sala 1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Ivory;
            this.button2.Location = new System.Drawing.Point(0, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(304, 72);
            this.button2.TabIndex = 1;
            this.button2.Text = "Sala 2";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.button3.Dock = System.Windows.Forms.DockStyle.Top;
            this.button3.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Ivory;
            this.button3.Location = new System.Drawing.Point(0, 144);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(304, 72);
            this.button3.TabIndex = 2;
            this.button3.Text = "Sala 3";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.button4.Dock = System.Windows.Forms.DockStyle.Top;
            this.button4.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.Ivory;
            this.button4.Location = new System.Drawing.Point(0, 216);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(304, 72);
            this.button4.TabIndex = 3;
            this.button4.Text = "Sala 4";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.button5.Dock = System.Windows.Forms.DockStyle.Top;
            this.button5.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.Ivory;
            this.button5.Location = new System.Drawing.Point(0, 288);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(304, 72);
            this.button5.TabIndex = 4;
            this.button5.Text = "Sala 5";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // directorySearcher2
            // 
            this.directorySearcher2.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher2.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher2.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(32, 92);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(241, 26);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "SEARCH BAR???";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1067, 623);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.header);
            this.Controls.Add(this.sidebar);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.sidebar.ResumeLayout(false);
            this.sidebar.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel sidebar;
        private System.Windows.Forms.Panel header;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.DirectoryServices.DirectorySearcher directorySearcher2;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

