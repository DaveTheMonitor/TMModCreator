using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaveTheMonitor.TMModCreator
{
    public partial class NewModForm : Form
    {
        public NewModForm()
        {
            InitializeComponent();
        }

        private void modNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(modNameTextBox.Text)) newModButton.Enabled = false;
            else newModButton.Enabled = true;
        }

        private void NewModForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) DialogResult = DialogResult.Cancel;
            else if (e.KeyCode == Keys.Enter && modNameTextBox.Focused && newModButton.Enabled) DialogResult = DialogResult.OK;
        }
    }
}
