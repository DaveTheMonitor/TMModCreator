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
    public partial class ExportModForm : Form
    {
        public ExportModForm()
        {
            InitializeComponent();
            modNameTextBox.Text = Globals.modName;
            if (Globals.modName == null) exportModButton.Enabled = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void exportModButton_Click(object sender, EventArgs e)
        {
            StartExportMod();
        }

        private void StartExportMod()
        {
            DialogResult result;
            string rootPath;
            using (FolderBrowserDialog browser = new FolderBrowserDialog())
            {
                browser.Description = "Select a folder to build this mod to.";
                browser.ShowNewFolderButton = false;
                browser.UseDescriptionForTitle = true;
                result = browser.ShowDialog(this);
                rootPath = browser.SelectedPath;
            }
            if (result == DialogResult.OK)
            {
                string path;
                if (newFolderCheckBox.Checked)
                {
                    path = Path.Combine(rootPath, modNameTextBox.Text);
                    Globals.modName = modNameTextBox.Text;
                }
                else
                {
                    path = rootPath;
                    Globals.modName = new DirectoryInfo(path).Name;
                }
                DialogResult overwrite = DialogResult.Yes;
                bool exists = Directory.Exists(path);
                if (exists) overwrite = ModCreator.ShowWarningBox(this, "Overwrite existing mod?", $"The mod {Globals.modName} is already present in this folder. This tool is still in development, so it is recommended to backup your mod before overwriting it.", MessageBoxButtons.YesNo);
                if (overwrite == DialogResult.Yes)
                {
                    if (!exists) Directory.CreateDirectory(path);
                    ExportMod(path);
                    DialogResult = DialogResult.OK;
                }
            }
        }
        internal static void ExportMod(string path)
        {
            List<ModItemDataXML> itemData = new List<ModItemDataXML>();
            List<ModItemTypeDataXML> typeData = new List<ModItemTypeDataXML>();
            List<ModItemCombatDataXML> combatData = new List<ModItemCombatDataXML>();
            List<ModItemSwingTimeDataXML> swingTimeData = new List<ModItemSwingTimeDataXML>();
            List<ModItemSoundDataXML> soundData = new List<ModItemSoundDataXML>();
            List<ModItemTypeClassDataXML> classData = new List<ModItemTypeClassDataXML>();
            List<ModSkillDataXML> skillData = new List<ModSkillDataXML>();
            foreach (Item item in Globals.itemsList)
            {
                itemData.Add(item.ItemData);
                if (item.ItemTypeDataSpecified) typeData.Add(new ModItemTypeDataXML(item.ItemData.ItemID, item.ItemTypeData, item.ItemCombatDataSpecified));
                if (item.ItemCombatDataSpecified) combatData.Add(new ModItemCombatDataXML(item.ItemData.ItemID, item.ItemCombatData));
                if (item.ItemSwingTimeDataSpecified) swingTimeData.Add(new ModItemSwingTimeDataXML(item.ItemData.ItemID, item.ItemSwingTimeData));
                if (item.ItemSoundDataSpecified) soundData.Add(new ModItemSoundDataXML(item.ItemData.ItemID, item.ItemSoundData));
                if (item.SkillDataSpecified) skillData.Add(new ModSkillDataXML(item.ItemData.ItemID, item.SkillData));
            }
            foreach (ItemTypeClass itemClass in Globals.classesList)
            {
                if (!Globals.IsBaseClass(itemClass)) classData.Add(new ModItemTypeClassDataXML(itemClass));
            }
            SaveFile(itemData.ToArray(), Path.Combine(path, "ItemData.xml"));
            SaveFile(typeData.ToArray(), Path.Combine(path, "ItemTypeData.xml"));
            SaveFile(combatData.ToArray(), Path.Combine(path, "ItemCombatData.xml"));
            SaveFile(swingTimeData.ToArray(), Path.Combine(path, "ItemSwingTimeData.xml"));
            SaveFile(soundData.ToArray(), Path.Combine(path, "ItemSoundData.xml"));
            SaveFile(classData.ToArray(), Path.Combine(path, "ItemTypeClassData.xml"));
            SaveFile(skillData.ToArray(), Path.Combine(path, "SkillData.xml"));

            static void SaveFile<T>(T[] data, string path, bool deleteIfNotExist = true)
            {
                if (data.Length > 0)
                {
                    ModCreator.Serialize(data, path);
                }
                else if (deleteIfNotExist)
                {
                    File.Delete(path);
                }
            }
            ExportTextures(path);
        }

        private static void ExportTextures(string path)
        {
            DrawTextureAtlas(TextureType.SD, Path.Combine(path, "TPI_16.png"));
            DrawTextureAtlas(TextureType.HDItem, Path.Combine(path, "TPI_32.png"));
            List<ItemXML> texturesXML = new List<ItemXML>();
            foreach (Item item in Globals.itemsList)
            {
                texturesXML.Add(new ItemXML(item.ItemData.ItemID));
            }
            ModCreator.Serialize(texturesXML.ToArray(), Path.Combine(path, "ItemTextures16.xml"));
            ModCreator.Serialize(texturesXML.ToArray(), Path.Combine(path, "ItemTextures32.xml"));
        }

        private static void DrawTextureAtlas(TextureType type, string path)
        {
            int size = type == TextureType.SD ? 16 : type == TextureType.HDItem ? 32 : 64;
            int width = Math.Min(Globals.itemsList.Count * size, size * 32);
            int height = ((int)Math.Floor((decimal)(Globals.itemsList.Count - 1) / 32) + 1) * size;
            using Bitmap bmp = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(bmp);
            for (int i = 0; i < Globals.itemsList.Count; i++)
            {
                Item item = Globals.itemsList[i];
                Image texture = type == TextureType.SD ? item.TextureSD : item.TextureHD;
                g.DrawImage(texture, new Rectangle(ModCreator.GetTexturePosition(bmp.Size, size, i), new Size(size, size)));
            }
            bmp.Save(path);
        }

        private void newFolderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            modNameTextBox.Enabled = newFolderCheckBox.Checked;
        }

        private void modNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(modNameTextBox.Text)) exportModButton.Enabled = false;
            else exportModButton.Enabled = true;
        }

        private void ExportModForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) DialogResult = DialogResult.Cancel;
            else if (e.KeyCode == Keys.Enter && modNameTextBox.Focused && exportModButton.Enabled) StartExportMod();
        }
    }
}
