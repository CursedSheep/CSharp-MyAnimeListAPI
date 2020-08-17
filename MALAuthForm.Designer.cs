/**********************************************************************************
        Copyright (C) 2020  CursedSheep

        This program is free software: you can redistribute it and/or modify
        it under the terms of the GNU General Public License as published by
        the Free Software Foundation, either version 3 of the License, or
        (at your option) any later version.

        This program is distributed in the hope that it will be useful,
        but WITHOUT ANY WARRANTY; without even the implied warranty of
        MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
        GNU General Public License for more details.
**********************************************************************************/  
        
        
namespace CSMALAPI
{
    partial class MALAuthForm
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
            this.authorize = new System.Windows.Forms.Button();
            this.authKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // authorize
            // 
            this.authorize.Location = new System.Drawing.Point(123, 76);
            this.authorize.Name = "authorize";
            this.authorize.Size = new System.Drawing.Size(75, 23);
            this.authorize.TabIndex = 0;
            this.authorize.Text = "Check";
            this.authorize.UseVisualStyleBackColor = true;
            this.authorize.Click += new System.EventHandler(this.authorize_Click);
            // 
            // authKey
            // 
            this.authKey.Location = new System.Drawing.Point(12, 45);
            this.authKey.Name = "authKey";
            this.authKey.Size = new System.Drawing.Size(305, 20);
            this.authKey.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(111, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Authorization";
            // 
            // MALAuthForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 113);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.authKey);
            this.Controls.Add(this.authorize);
            this.MaximizeBox = false;
            this.Name = "MALAuthForm";
            this.ShowIcon = false;
            this.Text = "Auth Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button authorize;
        private System.Windows.Forms.TextBox authKey;
        private System.Windows.Forms.Label label1;
    }
}
