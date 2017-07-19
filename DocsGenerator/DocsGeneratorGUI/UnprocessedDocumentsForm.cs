using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocsGeneratorGUI
{
    public partial class UnprocessedDocumentsForm : Form
    {
        public UnprocessedDocumentsForm(List<string> unprocessedDocumentsDetails)
        {
            InitializeComponent();
            lbUnprocessedDocuments.DataSource = unprocessedDocumentsDetails;
        }
    }
}
