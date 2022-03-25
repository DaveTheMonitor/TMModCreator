using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DaveTheMonitor.TMModCreator
{
    internal enum DisposeTextureOptions : byte
    {
        HD,
        SD
    }
    internal enum TextureType : byte
    {
        HDBlock,
        HDItem,
        SD,
    }
    internal enum TextureBoxOptions : byte
    {
        HD,
        SD,
        Both
    }
    internal static class Globals
    {
        internal static readonly bool devBuild;
        internal static readonly int maxSkillLevel;
        internal static readonly Item emptyItem;
        internal static readonly ItemTypeClass emptyClass;
        internal static readonly List<ItemTypeClass> baseClasses;
        internal static readonly Image missingTexture16;
        internal static readonly Image missingTexture32;
        internal static readonly Image missingTexture64;
        internal static readonly Image unknownTexture16;
        internal static readonly Image transparent;
        internal static readonly ItemTypeData emptyItemTypeData;
        internal static readonly ItemCombatData emptyItemCombatData;
        internal static readonly ItemSwingTimeData emptyItemSwingTimeData;
        internal static readonly ItemSoundData emptyItemSoundData;
        internal static readonly string stringNone;
        internal static string templatesDirectory;
        internal static string? modName;
        internal static Item selectedItem;
        internal static List<Item> itemsList;
        internal static Dictionary<string, Item> items;

        internal static ItemTypeClass selectedClass;
        internal static List<ItemTypeClass> classesList;
        internal static Dictionary<string, ItemTypeClass> classes;

        internal static bool IsBaseClass(ItemTypeClass itemClass)
        {
            for (int i = 0; i < baseClasses.Count; i++)
            {
                ItemTypeClass baseClass = baseClasses[i];
                if (baseClass.ClassID.Equals(itemClass.ClassID) && baseClass.Power == itemClass.Power && baseClass.MaxResistance == itemClass.MaxResistance) return true;
            }
            return false;
        }
        internal static void RegisterItem(Item item, MainForm form)
        {
            itemsList.Add(item);
            items.Add(item.ItemData.ItemID, item);
            form.itemsComboBox.Items.Add(item.ItemData.ItemID);
            form.blueprintItemIDComboBox.Items.Add(item.ItemData.ItemID);
        }
        internal static void RegisterClass(ItemTypeClass itemClass, MainForm form)
        {
            classesList.Add(itemClass);
            classes.Add(itemClass.ClassID, itemClass);
            form.classComboBox.Items.Add(itemClass.ClassID);
            form.itemClassComboBox.Items.Add(itemClass.ClassID);
        }
        internal static void ClearItemsList(MainForm form)
        {
            items.Clear();
            itemsList.Clear();
            form.itemsComboBox.Items.Clear();
            form.itemsComboBox.SelectedIndex = -1;
            form.itemsComboBox.Text = null;
            form.blueprintItemIDComboBox.Items.Clear();
            form.blueprintItemIDComboBox.SelectedIndex = -1;
            form.blueprintItemIDComboBox.Text = null;
        }
        internal static void ClearClassesList(MainForm form)
        {
            classes.Clear();
            classesList.Clear();
            baseClasses.Clear();
            form.classComboBox.Items.Clear();
            form.classComboBox.SelectedIndex = -1;
            form.classComboBox.Text = null;
        }

        static Globals()
        {
            devBuild = true;
            maxSkillLevel = 175;
            stringNone = "None";
            missingTexture16 = Image.FromFile("resources/MissingTexture16.png");
            missingTexture32 = ModCreator.ScaleTexture(missingTexture16, 32);
            missingTexture64 = ModCreator.ScaleTexture(missingTexture16, 64);
            unknownTexture16 = Image.FromFile("resources/UnknownTexture16.png");
            emptyItem = new Item();
            emptyItemTypeData = new ItemTypeData()
            {
                ClassID = null,
                CombatID = null,
                Equip = EquipIndex.RightHand,
                Inv = ItemInvType.Other,
                Model = ItemModelType.Item,
                SubType = ItemSubType.None,
                Swing = ItemSwingType.Item,
                Type = ItemType.Item,
                Use = ItemUse.Item
            };
            emptyItemCombatData = new ItemCombatData()
            {
                Attack = 0,
                Defence = 0,
                Health = 0,
                Looting = 0,
                Ranged = 0,
                Strength = 0
            };
            emptyItemSwingTimeData = new ItemSwingTimeData()
            {
                ExtendedPause = 0,
                Pause = 0,
                RetractSmooth = false,
                RetractTime = -1,
                Time = 0.27f
            };
            emptyItemSoundData = new ItemSoundData()
            {
                Group = ItemSoundGroup.None,
                Sounds = null
            };
            transparent = new Bitmap(2, 2);
            templatesDirectory = "templates";
            modName = null;
            selectedItem = emptyItem;
            itemsList = new List<Item>();
            items = new Dictionary<string, Item>();

            emptyClass = new ItemTypeClass();
            classesList = new List<ItemTypeClass>();
            classes = new Dictionary<string, ItemTypeClass>();
            baseClasses = new List<ItemTypeClass>();
            selectedClass = emptyClass;
        }

        internal static void CreateBaseClasses(MainForm form)
        {
            RegisterClass(new ItemTypeClass("None", 0, 0, false, false), form);
            RegisterClass(new ItemTypeClass("CantMine", 0, 0, false, false), form);
            RegisterClass(new ItemTypeClass("Hand", 200, 1600, true, false), form);
            RegisterClass(new ItemTypeClass("Wood", 300, 1800, true, false), form);
            RegisterClass(new ItemTypeClass("Bronze", 325, 2150, true, false), form);
            RegisterClass(new ItemTypeClass("Iron", 360, 2500, true, false), form);
            RegisterClass(new ItemTypeClass("Steel", 450, 3150, true, false), form);
            RegisterClass(new ItemTypeClass("GreenstoneGold", 600, 4200, true, false), form);
            RegisterClass(new ItemTypeClass("Platinum", 800, 4200, true, false), form);
            RegisterClass(new ItemTypeClass("Diamond", 900, 5400, true, false), form);
            RegisterClass(new ItemTypeClass("Ruby", 1000, 5800, true, false), form);
            RegisterClass(new ItemTypeClass("Titanium", 1200, 65000, true, false), form);
            RegisterClass(new ItemTypeClass("SledgeHammer", 65000, 65000, true, false), form);
            baseClasses.Add(new ItemTypeClass("None", 0, 0, false, false));
            baseClasses.Add(new ItemTypeClass("CantMine", 0, 0, false, false));
            baseClasses.Add(new ItemTypeClass("Hand", 200, 1600, false, false));
            baseClasses.Add(new ItemTypeClass("Wood", 300, 1800, false, false));
            baseClasses.Add(new ItemTypeClass("Bronze", 325, 2150, false, false));
            baseClasses.Add(new ItemTypeClass("Iron", 360, 2500, false, false));
            baseClasses.Add(new ItemTypeClass("Steel", 450, 3150, false, false));
            baseClasses.Add(new ItemTypeClass("GreenstoneGold", 600, 4200, false, false));
            baseClasses.Add(new ItemTypeClass("Platinum", 800, 4200, false, false));
            baseClasses.Add(new ItemTypeClass("Diamond", 900, 5400, false, false));
            baseClasses.Add(new ItemTypeClass("Ruby", 1000, 5800, false, false));
            baseClasses.Add(new ItemTypeClass("Titanium", 1200, 65000, false, false));
            baseClasses.Add(new ItemTypeClass("SledgeHammer", 65000, 65000, false, false));
        }
    }
    internal class ModCreator
    {
        internal static T Deserialize<T>(string path)
        {
            try
            {
                using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    T? deserialized = (T?)serializer.Deserialize(stream);
                    if (deserialized != null) return deserialized;
                    else throw new Exception("Invalid XML");
                }
            }
            catch
            {
                throw new Exception($"Invalid XML: {new FileInfo(path).Name}");
            }
        }
        internal static void Serialize<T>(T data, string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, data);
            }
        }
        internal static void ShowErrorBox(IWin32Window owner, string caption, string message)
        {
            MessageBox.Show(owner, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        internal static DialogResult ShowWarningBox(IWin32Window owner, string caption, string message, MessageBoxButtons buttons)
        {
            return MessageBox.Show(owner, message, caption, buttons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        }

        internal static T GetEnumValue<T>(string value) where T : struct, Enum
        {
            bool parsed = Enum.TryParse<T>(value, out T enumValue);
            if (parsed) return enumValue;
            else return Enum.GetValues<T>()[0];
        }

        /// <summary>
        /// Gets the texture of the specified index from a texture atlas.
        /// </summary>
        internal static Bitmap ScaleAtlasTexture(Image atlas, int size, int srcSize, int index)
        {
            Bitmap texture = new Bitmap(size, size);
            using (Graphics textureG = Graphics.FromImage(texture))
            {
                textureG.InterpolationMode = InterpolationMode.NearestNeighbor;
                textureG.PixelOffsetMode = PixelOffsetMode.Half;
                textureG.DrawImage(atlas, new Rectangle(0, 0, size, size), new Rectangle(GetTexturePosition(atlas.Size, srcSize, index), new Size(srcSize, srcSize)), GraphicsUnit.Pixel);
            }
            return texture;
        }

        /// <summary>
        /// Scales the given texture to a specific size.
        /// </summary>
        internal static Bitmap ScaleTexture(Image original, int size)
        {
            Bitmap texture = new Bitmap(size, size);
            using (Graphics textureG = Graphics.FromImage(texture))
            {
                textureG.InterpolationMode = InterpolationMode.NearestNeighbor;
                textureG.PixelOffsetMode = PixelOffsetMode.Half;
                textureG.DrawImage(original, new Rectangle(0, 0, size, size), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
            }
            return texture;
        }

        internal static Point GetTexturePosition(Size atlasSize, int textureSize, int index)
        {
            decimal texturesWidth = atlasSize.Width / textureSize;
            Point texturePosition = new Point();
            texturePosition.Y = ((int)Math.Ceiling((index + 1) / texturesWidth) - 1) * textureSize;
            texturePosition.X = (int)(index % texturesWidth) * textureSize;
            return texturePosition;
        }

        internal static T GetNull<T>(T? value, T defaultValue) where T : struct
        {
            if (value == null) return defaultValue;
            else return (T)value;
        }
        internal static T GetNull<T>(T? value, T defaultValue) where T : class
        {
            if (value == null) return defaultValue;
            else return value;
        }
    }

    public class BlueprintDataPictureBox : PictureBox
    {
        public int MaterialIndex;
        public MainForm Form;
        public Label CountLabel;
        private void ClickMethod(object? sender, EventArgs e)
        {
            if (sender == null) return;
            SelectBox();
        }
        public void SetTexture(Image? texture)
        {
            if (Image != null) Image.Dispose();
            if (texture == null)
            {
                Image = null;
                return;
            }
            Image = ModCreator.ScaleTexture(texture, 32);
        }
        public void SetCountLabel(int count)
        {
            if (count <= 1) CountLabel.Visible = false;
            else
            {
                CountLabel.Visible = true;
                CountLabel.Text = count.ToString();
            }
        }
        public void DeselectBox(bool nextSelect)
        {
            if (!nextSelect)
            {
                Form.blueprintItemIDComboBox.Enabled = false;
                Form.blueprintCountLabel.Enabled = false;
                Form.blueprintCountNumeric.Enabled = false;
                Form.blueprintDurabilityNumeric.Enabled = false;
                Form.blueprintDurabilityCheckBox.Enabled = false;
                Form.selectedBPBox = null;
            }
            BackColor = SystemColors.ControlDark;
        }
        public void SelectBox()
        {
            Focus();
            Form.blueprintOutputPictureBox.BackColor = SystemColors.ControlDark;
            foreach (BlueprintDataPictureBox box in Form.bpBoxes)
            {
                if (box != this) box.DeselectBox(true);
            }
            Form.selectedBPBox = this;
            BackColor = SystemColors.Control;
            string toolTip = "How many of this material is required to craft this item.";
            Form.blueprintDataToolTip.SetToolTip(Form.blueprintCountLabel, toolTip);
            Form.blueprintDataToolTip.SetToolTip(Form.blueprintCountNumeric, toolTip);

            ModInventoryItemXML material = Globals.selectedItem.BlueprintData.Materials[MaterialIndex];
            Form.blueprintCountNumeric.Value = Math.Max(material.Count, 1);
            Form.blueprintDurabilityNumeric.Value = material.Durability;
            if (material.Durability == 0)
            {
                Form.blueprintDurabilityCheckBox.Checked = false;
            }
            else
            {
                Form.blueprintDurabilityCheckBox.Checked = true;
            }
            if (Form.blueprintItemIDComboBox.Items.Contains(material.ItemID))
            {
                Form.blueprintItemIDComboBox.SelectedItem = material.ItemID;
            }
            else if (material.ItemIDNone)
            {
                Form.blueprintItemIDComboBox.Text = string.Empty;
                Form.blueprintItemIDComboBox.SelectedItem = null;
            }
            else Form.blueprintItemIDComboBox.Text = material.ItemID;

            Form.blueprintItemIDComboBox.Enabled = true;
            Form.blueprintCountLabel.Enabled = true;
            Form.blueprintCountNumeric.Enabled = true;
            Form.blueprintDurabilityCheckBox.Enabled = true;
        }

        public BlueprintDataPictureBox(int materialIndex, MainForm form) : base()
        {
            MaterialIndex = materialIndex;
            Form = form;
            CountLabel = new Label();
            CountLabel.Parent = this;
            CountLabel.BackColor = Color.Transparent;
            CountLabel.Location = new Point(1, 1);
            Click += new EventHandler(ClickMethod);
            CountLabel.Click += new EventHandler(ClickMethod);
        }
    }
}
