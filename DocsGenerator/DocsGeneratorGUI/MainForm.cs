using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocsGeneratorGUI
{
    public partial class MainForm : Form
    {
        private string errorMessage = string.Empty;
        private List<string> unprocessedDocumentsDetails;
        public MainForm()
        {
            InitializeComponent();
            errorMessage = string.Empty;
            lblError.Text = string.Empty;
            linkLblUnprocessed.Visible = false;
            lblStatus.Text = string.Empty;
            unprocessedDocumentsDetails = new List<string>();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf) | *.pdf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbOutputPath.Text = saveFileDialog.FileName;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            linkLblUnprocessed.Visible = false;
            lblError.Text = string.Empty;
            lblStatus.Text = "Processing...";
            if (validate())
            {

                DocsGenerator.DocsGenerator generator = new DocsGenerator.DocsGenerator();
                try
                {
                    generator.GenerateDocs(tbInputPath.Text, tbOutputPath.Text, tbBranch.Text);
                } catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Error.";
                    return;
                }
                lblStatus.Text = "Done!";
                if (generator.HasUnprocessedDocuments())
                {
                    linkLblUnprocessed.Visible = true;
                    unprocessedDocumentsDetails = generator.getUnprocessedDocumentsDetails();
                }
            }
            else
            {
                lblStatus.Text = "Failed.";
                lblError.Text = errorMessage;
                return;
            }

            if (rbtnFileExplorer.Checked)
            {
                Process.Start("explorer.exe", tbOutputPath.Text);
            } else if (rbtnDefaultViewer.Checked)
            {
                Process.Start(tbOutputPath.Text);
            }
        }

        private bool validate()
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(tbInputPath.Text))
            {
                errorMessage = "Input path (url) must be given!";
                return false;
            }
            if (!Uri.IsWellFormedUriString(tbInputPath.Text, UriKind.RelativeOrAbsolute))
            {
                errorMessage = "Given URL is not valid.";
                return false;
            }

            if (string.IsNullOrEmpty(tbOutputPath.Text))
            {
                errorMessage = "Output path must be given!";
                return false;
            }
            if (!tbOutputPath.Text.EndsWith(".pdf"))
            {
                errorMessage = "Output path must be a .pdf file!";
                return false;
            }

            return true;
        }

        private void linkLblUnprocessed_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UnprocessedDocumentsForm udForm = new UnprocessedDocumentsForm(unprocessedDocumentsDetails);
            udForm.ShowDialog();
        }
    }
}
