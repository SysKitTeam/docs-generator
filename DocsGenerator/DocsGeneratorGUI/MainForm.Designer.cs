namespace DocsGeneratorGUI
{
    partial class MainForm
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
            this.lblInputPath = new System.Windows.Forms.Label();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.tbInputPath = new System.Windows.Forms.TextBox();
            this.tbOutputPath = new System.Windows.Forms.TextBox();
            this.gbAfter = new System.Windows.Forms.GroupBox();
            this.rbtnDefaultViewer = new System.Windows.Forms.RadioButton();
            this.rbtnFileExplorer = new System.Windows.Forms.RadioButton();
            this.rbtnDoNothing = new System.Windows.Forms.RadioButton();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.linkLblUnprocessed = new System.Windows.Forms.LinkLabel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gbAfter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInputPath
            // 
            this.lblInputPath.AutoSize = true;
            this.lblInputPath.Location = new System.Drawing.Point(27, 31);
            this.lblInputPath.Name = "lblInputPath";
            this.lblInputPath.Size = new System.Drawing.Size(88, 17);
            this.lblInputPath.TabIndex = 0;
            this.lblInputPath.Text = "GitHub URL:";
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.AutoSize = true;
            this.lblOutputPath.Location = new System.Drawing.Point(27, 59);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(55, 17);
            this.lblOutputPath.TabIndex = 1;
            this.lblOutputPath.Text = "Output:";
            // 
            // tbInputPath
            // 
            this.tbInputPath.Location = new System.Drawing.Point(121, 28);
            this.tbInputPath.Name = "tbInputPath";
            this.tbInputPath.Size = new System.Drawing.Size(304, 22);
            this.tbInputPath.TabIndex = 2;
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Location = new System.Drawing.Point(121, 56);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(304, 22);
            this.tbOutputPath.TabIndex = 3;
            // 
            // gbAfter
            // 
            this.gbAfter.Controls.Add(this.rbtnDefaultViewer);
            this.gbAfter.Controls.Add(this.rbtnFileExplorer);
            this.gbAfter.Controls.Add(this.rbtnDoNothing);
            this.gbAfter.Location = new System.Drawing.Point(30, 134);
            this.gbAfter.Name = "gbAfter";
            this.gbAfter.Size = new System.Drawing.Size(234, 111);
            this.gbAfter.TabIndex = 4;
            this.gbAfter.TabStop = false;
            this.gbAfter.Text = "After generating:";
            // 
            // rbtnDefaultViewer
            // 
            this.rbtnDefaultViewer.AutoSize = true;
            this.rbtnDefaultViewer.Location = new System.Drawing.Point(7, 76);
            this.rbtnDefaultViewer.Name = "rbtnDefaultViewer";
            this.rbtnDefaultViewer.Size = new System.Drawing.Size(202, 21);
            this.rbtnDefaultViewer.TabIndex = 2;
            this.rbtnDefaultViewer.Text = "Open file in default browser";
            this.rbtnDefaultViewer.UseVisualStyleBackColor = true;
            // 
            // rbtnFileExplorer
            // 
            this.rbtnFileExplorer.AutoSize = true;
            this.rbtnFileExplorer.Location = new System.Drawing.Point(6, 48);
            this.rbtnFileExplorer.Name = "rbtnFileExplorer";
            this.rbtnFileExplorer.Size = new System.Drawing.Size(161, 21);
            this.rbtnFileExplorer.TabIndex = 1;
            this.rbtnFileExplorer.Text = "Open in File Explorer";
            this.rbtnFileExplorer.UseVisualStyleBackColor = true;
            // 
            // rbtnDoNothing
            // 
            this.rbtnDoNothing.AutoSize = true;
            this.rbtnDoNothing.Checked = true;
            this.rbtnDoNothing.Location = new System.Drawing.Point(6, 21);
            this.rbtnDoNothing.Name = "rbtnDoNothing";
            this.rbtnDoNothing.Size = new System.Drawing.Size(98, 21);
            this.rbtnDoNothing.TabIndex = 0;
            this.rbtnDoNothing.TabStop = true;
            this.rbtnDoNothing.Text = "Do nothing";
            this.rbtnDoNothing.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(431, 54);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(83, 26);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(273, 195);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(241, 51);
            this.btnGenerate.TabIndex = 6;
            this.btnGenerate.Text = "Generate!";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(30, 92);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(54, 17);
            this.lblError.TabIndex = 7;
            this.lblError.Text = "lblError";
            // 
            // linkLblUnprocessed
            // 
            this.linkLblUnprocessed.AutoSize = true;
            this.linkLblUnprocessed.Location = new System.Drawing.Point(270, 134);
            this.linkLblUnprocessed.Name = "linkLblUnprocessed";
            this.linkLblUnprocessed.Size = new System.Drawing.Size(245, 17);
            this.linkLblUnprocessed.TabIndex = 8;
            this.linkLblUnprocessed.TabStop = true;
            this.linkLblUnprocessed.Text = "Some documents were not processed";
            this.linkLblUnprocessed.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblUnprocessed_LinkClicked);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(270, 175);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(62, 17);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "lblStatus";
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 261);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.linkLblUnprocessed);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.gbAfter);
            this.Controls.Add(this.tbOutputPath);
            this.Controls.Add(this.tbInputPath);
            this.Controls.Add(this.lblOutputPath);
            this.Controls.Add(this.lblInputPath);
            this.Name = "MainForm";
            this.Text = "DocsGenerator";
            this.gbAfter.ResumeLayout(false);
            this.gbAfter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInputPath;
        private System.Windows.Forms.Label lblOutputPath;
        private System.Windows.Forms.TextBox tbInputPath;
        private System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.GroupBox gbAfter;
        private System.Windows.Forms.RadioButton rbtnDefaultViewer;
        private System.Windows.Forms.RadioButton rbtnFileExplorer;
        private System.Windows.Forms.RadioButton rbtnDoNothing;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.LinkLabel linkLblUnprocessed;
        private System.Windows.Forms.Label lblStatus;
    }
}

