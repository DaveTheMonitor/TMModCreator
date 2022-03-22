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
        internal static readonly Image missingTexture96;
        internal static readonly Image transparent;
        internal static readonly ItemTypeData emptyItemTypeData;
        internal static readonly ItemCombatData emptyItemCombatData;
        internal static readonly ItemSwingTimeData emptyItemSwingTimeData;
        internal static readonly ItemSoundData emptyItemSoundData;
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
        internal static void RegisterItem(Item item)
        {
            itemsList.Add(item);
            items.Add(item.ItemData.ItemID, item);
        }
        internal static void RegisterClass(ItemTypeClass itemClass)
        {
            classesList.Add(itemClass);
            classes.Add(itemClass.ClassID, itemClass);
        }
        internal static void ClearItemsList(ComboBox comboBox)
        {
            items.Clear();
            itemsList.Clear();
            comboBox.Items.Clear();
            comboBox.SelectedIndex = -1;
            comboBox.Text = null;
        }
        internal static void ClearClassesList(ComboBox comboBox)
        {
            classes.Clear();
            classesList.Clear();
            baseClasses.Clear();
            comboBox.Items.Clear();
            comboBox.SelectedIndex = -1;
            comboBox.Text = null;
        }

        static Globals()
        {
            devBuild = true;
            maxSkillLevel = 175;
            missingTexture16 = Image.FromFile("resources/MissingTexture16.png");
            missingTexture32 = ModCreator.ScaleTexture(missingTexture16, 32);
            missingTexture64 = ModCreator.ScaleTexture(missingTexture16, 64);
            missingTexture96 = ModCreator.ScaleTexture(missingTexture16, 96);
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

        internal static void CreateBaseClasses()
        {
            RegisterClass(new ItemTypeClass("None", 0, 0, false, false));
            RegisterClass(new ItemTypeClass("CantMine", 0, 0, false, false));
            RegisterClass(new ItemTypeClass("Hand", 200, 1600, true, false));
            RegisterClass(new ItemTypeClass("Wood", 300, 1800, true, false));
            RegisterClass(new ItemTypeClass("Bronze", 325, 2150, true, false));
            RegisterClass(new ItemTypeClass("Iron", 360, 2500, true, false));
            RegisterClass(new ItemTypeClass("Steel", 450, 3150, true, false));
            RegisterClass(new ItemTypeClass("GreenstoneGold", 600, 4200, true, false));
            RegisterClass(new ItemTypeClass("Platinum", 800, 4200, true, false));
            RegisterClass(new ItemTypeClass("Diamond", 900, 5400, true, false));
            RegisterClass(new ItemTypeClass("Ruby", 1000, 5800, true, false));
            RegisterClass(new ItemTypeClass("Titanium", 1200, 65000, true, false));
            RegisterClass(new ItemTypeClass("SledgeHammer", 65000, 65000, true, false));
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
}
