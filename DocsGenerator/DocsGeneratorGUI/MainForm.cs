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
        public MainForm()
        {
            InitializeComponent();
            errorMessage = string.Empty;
            lblError.Text = string.Empty;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbOutputPath.Text = saveFileDialog.FileName;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            if (validate())
            {

                DocsGenerator.DocsGenerator generator = new DocsGenerator.DocsGenerator();
                generator.GenerateDocs(tbInputPath.Text, tbOutputPath.Text);
            }
            else
            {
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
    }
}
