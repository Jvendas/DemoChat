
namespace DemoChat
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRegister = new System.Windows.Forms.Button();
            this.tbSaltedHash = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSalt = new System.Windows.Forms.TextBox();
            this.btnGenerateSaltedHash = new System.Windows.Forms.Button();
            this.tbSizePass = new System.Windows.Forms.TextBox();
            this.tbSizeSalt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.circularPictureBox1 = new DemoChat.CircularPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.circularPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(88, 301);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(200, 20);
            this.tbUserName.TabIndex = 18;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(88, 376);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(200, 20);
            this.tbPassword.TabIndex = 18;
            this.tbPassword.TabStop = false;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(145)))), ((int)(((byte)(108)))));
            this.btnLogin.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(228)))), ((int)(((byte)(199)))));
            this.btnLogin.Location = new System.Drawing.Point(196, 435);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(92, 42);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cambria", 10.25F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(67)))), ((int)(((byte)(50)))));
            this.label1.Location = new System.Drawing.Point(87, 282);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cambria", 10.25F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(67)))), ((int)(((byte)(50)))));
            this.label2.Location = new System.Drawing.Point(87, 348);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(145)))), ((int)(((byte)(108)))));
            this.btnRegister.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegister.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(228)))), ((int)(((byte)(199)))));
            this.btnRegister.Location = new System.Drawing.Point(88, 435);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(92, 42);
            this.btnRegister.TabIndex = 19;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // tbSaltedHash
            // 
            this.tbSaltedHash.Location = new System.Drawing.Point(28, 515);
            this.tbSaltedHash.Multiline = true;
            this.tbSaltedHash.Name = "tbSaltedHash";
            this.tbSaltedHash.ReadOnly = true;
            this.tbSaltedHash.Size = new System.Drawing.Size(271, 20);
            this.tbSaltedHash.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 496);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "Salted Password Hash";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 542);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "Salt";
            // 
            // tbSalt
            // 
            this.tbSalt.HideSelection = false;
            this.tbSalt.Location = new System.Drawing.Point(28, 558);
            this.tbSalt.Multiline = true;
            this.tbSalt.Name = "tbSalt";
            this.tbSalt.ReadOnly = true;
            this.tbSalt.Size = new System.Drawing.Size(271, 20);
            this.tbSalt.TabIndex = 23;
            // 
            // btnGenerateSaltedHash
            // 
           this.btnGenerateSaltedHash.Location = new System.Drawing.Point(224, 583);
            this.btnGenerateSaltedHash.Name = "btnGenerateSaltedHash";
            this.btnGenerateSaltedHash.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateSaltedHash.TabIndex = 24;
            this.btnGenerateSaltedHash.Text = "Generate";
            this.btnGenerateSaltedHash.UseVisualStyleBackColor = true;
            this.btnGenerateSaltedHash.Click += new System.EventHandler(this.btnGenerateSaltedHash_Click);
            // 
            // tbSizePass
            // 
            this.tbSizePass.Location = new System.Drawing.Point(306, 515);
            this.tbSizePass.Name = "tbSizePass";
            this.tbSizePass.ReadOnly = true;
            this.tbSizePass.Size = new System.Drawing.Size(50, 20);
            this.tbSizePass.TabIndex = 25;
            // 
            // tbSizeSalt
            // 
            this.tbSizeSalt.Location = new System.Drawing.Point(306, 558);
            this.tbSizeSalt.Name = "tbSizeSalt";
            this.tbSizeSalt.ReadOnly = true;
            this.tbSizeSalt.Size = new System.Drawing.Size(50, 20);
            this.tbSizeSalt.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(306, 496);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 12);
            this.label5.TabIndex = 27;
            this.label5.Text = "Size (Bits)";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(306, 542);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "Size (Bits)";
            // 
            // circularPictureBox1
            // 
            this.circularPictureBox1.Image = global::DemoChat.Properties.Resources.Avatar_Jogador;
            this.circularPictureBox1.Location = new System.Drawing.Point(103, 98);
            this.circularPictureBox1.Name = "circularPictureBox1";
            this.circularPictureBox1.Size = new System.Drawing.Size(163, 147);
            this.circularPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.circularPictureBox1.TabIndex = 0;
            this.circularPictureBox1.TabStop = false;
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(243)))), ((int)(((byte)(220)))));
            this.ClientSize = new System.Drawing.Size(368, 618);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbSizeSalt);
            this.Controls.Add(this.tbSizePass);
            this.Controls.Add(this.btnGenerateSaltedHash);
            this.Controls.Add(this.tbSalt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbSaltedHash);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.circularPictureBox1);
            this.Font = new System.Drawing.Font("Cambria", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLogin";
            this.Text = "DemoChat";
            this.Load += new System.EventHandler(this.FormLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.circularPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CircularPictureBox circularPictureBox1;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.TextBox tbSaltedHash;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSalt;
        private System.Windows.Forms.Button btnGenerateSaltedHash;
        private System.Windows.Forms.TextBox tbSizePass;
        private System.Windows.Forms.TextBox tbSizeSalt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}