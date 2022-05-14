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
    public partial class AddItemForm : Form
    {
        internal List<ItemTemplate> itemTemplates = new List<ItemTemplate>();
        public AddItemForm(string itemID)
        {
            InitializeComponent();
            
            itemIDTextBox.Text = itemID;
            templateComboBox.Items.Add("No Template");
            templateComboBox.SelectedIndex = 0;
            if (Globals.selectedItem != Globals.emptyItem)
            {
                ItemTemplate template = new ItemTemplate(Globals.selectedItem, true);
                itemTemplates.Add(template);
                templateComboBox.Items.Add($"Current Item ({template.ItemData.Name})");
            }
            string[] templateFiles = Directory.GetFiles(Globals.templatesDirectory, "*.xml");
            foreach (string file in templateFiles)
            {
                ItemTemplate item = ModCreator.Deserialize<ItemTemplate>(file);
                itemTemplates.Add(item);
                templateComboBox.Items.Add(item.ItemData.Name);
            }
        }

        private void itemIDTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(itemIDTextBox.Text) || itemIDTextBox.Text.Contains(' ') || Globals.items.ContainsKey(itemIDTextBox.Text))
            {
                addItemButton.Enabled = false;
            }
            else
            {
                addItemButton.Enabled = true;
            }
        }

        private void AddItemForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
            else if (e.KeyCode == Keys.Enter && itemIDTextBox.Focused && addItemButton.Enabled)
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
