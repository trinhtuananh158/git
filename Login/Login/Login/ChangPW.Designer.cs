namespace Login
{
    partial class ChangPW
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtOldpw = new System.Windows.Forms.TextBox();
            this.txtNewpw = new System.Windows.Forms.TextBox();
            this.txtRe = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Old password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "New password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Re type";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(117, 26);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(140, 20);
            this.txtUser.TabIndex = 1;
            // 
            // txtOldpw
            // 
            this.txtOldpw.Location = new System.Drawing.Point(117, 58);
            this.txtOldpw.Name = "txtOldpw";
            this.txtOldpw.PasswordChar = '*';
            this.txtOldpw.Size = new System.Drawing.Size(140, 20);
            this.txtOldpw.TabIndex = 1;
            // 
            // txtNewpw
            // 
            this.txtNewpw.Location = new System.Drawing.Point(117, 94);
            this.txtNewpw.Name = "txtNewpw";
            this.txtNewpw.PasswordChar = '*';
            this.txtNewpw.Size = new System.Drawing.Size(140, 20);
            this.txtNewpw.TabIndex = 1;
            // 
            // txtRe
            // 
            this.txtRe.Location = new System.Drawing.Point(117, 139);
            this.txtRe.Name = "txtRe";
            this.txtRe.PasswordChar = '*';
            this.txtRe.Size = new System.Drawing.Size(140, 20);
            this.txtRe.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 190);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChangPW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 259);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtRe);
            this.Controls.Add(this.txtNewpw);
            this.Controls.Add(this.txtOldpw);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ChangPW";
            this.Text = "ChangPW";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtOldpw;
        private System.Windows.Forms.TextBox txtNewpw;
        private System.Windows.Forms.TextBox txtRe;
        private System.Windows.Forms.Button button1;
    }
}