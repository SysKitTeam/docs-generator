namespace DocsGeneratorGUI
{
    partial class UnprocessedDocumentsForm
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
            this.lblUnprocessedDocuments = new System.Windows.Forms.Label();
            this.lbUnprocessedDocuments = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lblUnprocessedDocuments
            // 
            this.lblUnprocessedDocuments.AutoSize = true;
            this.lblUnprocessedDocuments.Location = new System.Drawing.Point(12, 22);
            this.lblUnprocessedDocuments.Name = "lblUnprocessedDocuments";
            this.lblUnprocessedDocuments.Size = new System.Drawing.Size(279, 17);
            this.lblUnprocessedDocuments.TabIndex = 0;
            this.lblUnprocessedDocuments.Text = "List of documents that were not processed:";
            // 
            // lbUnprocessedDocuments
            // 
            this.lbUnprocessedDocuments.FormattingEnabled = true;
            this.lbUnprocessedDocuments.ItemHeight = 16;
            this.lbUnprocessedDocuments.Location = new System.Drawing.Point(12, 42);
            this.lbUnprocessedDocuments.Name = "lbUnprocessedDocuments";
            this.lbUnprocessedDocuments.Size = new System.Drawing.Size(368, 276);
            this.lbUnprocessedDocuments.TabIndex = 1;
            // 
            // UnprocessedDocumentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 335);
            this.Controls.Add(this.lbUnprocessedDocuments);
            this.Controls.Add(this.lblUnprocessedDocuments);
            this.Name = "UnprocessedDocumentsForm";
            this.Text = "Unprocessed Documents";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUnprocessedDocuments;
        private System.Windows.Forms.ListBox lbUnprocessedDocuments;
    }
}