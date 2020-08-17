using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSMALAPI
{
    public partial class MALAuthForm : Form
    {
        public MALAuthForm()
        {
            InitializeComponent();
        }
        public string retKey;
        private void authorize_Click(object sender, EventArgs e)
        {
            retKey = authKey.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
