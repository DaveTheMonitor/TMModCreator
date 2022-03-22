using System;
using System.Collections.Generic;
using System.Text;

namespace DaveTheMonitor.TMModCreator
{
    public struct Vector2
    {
        public float X;
        public float Y;
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public Vector2()
        {
            this.X = 0;
            this.Y = 0;
        }
    }

    // Vector3 and Vector4 aren't used right now,
    // but will be needed for the particle template editor, once that's made.
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;
        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public Vector3()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }
    }
    // Vector3 and Vector4 aren't used right now,
    // but will be needed for the particle template editor, once that's made.
    public struct Vector4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;
        public Vector4(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }
        public Vector4()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.W = 0;
        }
    }

    public abstract class ItemBase
    {
        public ModItemDataXML ItemData;
        public ItemTypeData ItemTypeData;
        public ItemCombatData ItemCombatData;
        public ItemSwingTimeData ItemSwingTimeData;
        public ItemSoundData ItemSoundData;
        public SkillData SkillData;
        public bool ItemTypeDataSpecified { get { return ItemTypeData != null && !ItemTypeData.IsEmpty(); } }
        public bool ItemCombatDataSpecified { get { return !ItemCombatData.IsEmpty(); } }
        public bool ItemSwingTimeDataSpecified { get { return !ItemSwingTimeData.IsEmpty(); } }
        public bool ItemSoundDataSpecified { get { return !ItemSoundData.IsEmpty(); } }
        public bool SkillDataSpecified { get { return !SkillData.IsEmpty(); } }
        public ItemBase(ModItemDataXML itemData, ItemTypeData typeData, ItemCombatData combatData, ItemSwingTimeData swingData, ItemSoundData soundData, SkillData skillData)
        {
            this.ItemData = itemData;
            this.ItemTypeData = typeData;
            this.ItemCombatData = combatData;
            this.ItemSwingTimeData = swingData;
            this.ItemSoundData = soundData;
            this.SkillData = skillData;
        }
    }
    
    public class Item : ItemBase
    {
        public Image TextureSD;
        public Image TextureHD;
        public Image TextureSD64;
        public Image TextureHD64;
        public Image TextureSD96;
        public Image TextureHD96;
        internal void DisposeTextures()
        {
            if (TextureHD != Globals.missingTexture32 && TextureHD != Globals.missingTexture64) TextureHD.Dispose();
            if (TextureHD64 != Globals.missingTexture64) TextureHD64.Dispose();
            if (TextureHD96 != Globals.missingTexture96) TextureHD96.Dispose();

            if (TextureSD != Globals.missingTexture16) TextureSD.Dispose();
            if (TextureSD64 != Globals.missingTexture64) TextureSD64.Dispose();
            if (TextureSD96 != Globals.missingTexture96) TextureSD96.Dispose();
        }
        internal void DisposeTextures(DisposeTextureOptions options)
        {
            if (options == DisposeTextureOptions.HD)
            {
                if (TextureHD != Globals.missingTexture32 && TextureHD != Globals.missingTexture64) TextureHD.Dispose();
                if (TextureHD64 != Globals.missingTexture64) TextureHD64.Dispose();
                if (TextureHD96 != Globals.missingTexture96) TextureHD96.Dispose();
            }
            else if (options == DisposeTextureOptions.SD)
            {
                if (TextureSD != Globals.missingTexture16) TextureSD.Dispose();
                if (TextureSD64 != Globals.missingTexture64) TextureSD64.Dispose();
                if (TextureSD96 != Globals.missingTexture96) TextureSD96.Dispose();
            }
        }
        public Item(ModItemDataXML itemData) : base(itemData, new ItemTypeData(), new ItemCombatData(), new ItemSwingTimeData(), new ItemSoundData(), new SkillData())
        {
            this.ItemData = itemData;
            this.TextureHD = Globals.missingTexture32;
            this.TextureHD64 = Globals.missingTexture64;
            this.TextureHD96 = Globals.missingTexture96;

            this.TextureSD = Globals.missingTexture16;
            this.TextureSD64 = Globals.missingTexture64;
            this.TextureSD96 = Globals.missingTexture96;
        }
        public Item() : base(new ModItemDataXML(), new ItemTypeData(), new ItemCombatData(), new ItemSwingTimeData(), new ItemSoundData(), new SkillData())
        {
            this.TextureHD = Globals.missingTexture32;
            this.TextureHD64 = Globals.missingTexture64;
            this.TextureHD96 = Globals.missingTexture96;

            this.TextureSD = Globals.missingTexture16;
            this.TextureSD64 = Globals.missingTexture64;
            this.TextureSD96 = Globals.missingTexture96;
        }
        public Item(ItemTemplate template) : base(template.ItemData, template.ItemTypeData, template.ItemCombatData, template.ItemSwingTimeData, template.ItemSoundData, template.SkillData)
        {
            if (template.currentItem)
            {
                this.TextureHD = ModCreator.ScaleTexture(Globals.selectedItem.TextureHD, 32);
                this.TextureHD64 = ModCreator.ScaleTexture(Globals.selectedItem.TextureHD64, 64);
                this.TextureHD96 = ModCreator.ScaleTexture(Globals.selectedItem.TextureHD96, 96);

                this.TextureSD = ModCreator.ScaleTexture(Globals.selectedItem.TextureSD, 16);
                this.TextureSD64 = ModCreator.ScaleTexture(Globals.selectedItem.TextureSD64, 64);
                this.TextureSD96 = ModCreator.ScaleTexture(Globals.selectedItem.TextureSD96, 96);
            }
            else
            {
                using (Image textureHD = Image.FromFile(Path.Combine(Globals.templatesDirectory, "textures", template.TextureHD)))
                {
                    this.TextureHD = textureHD;
                    this.TextureHD64 = ModCreator.ScaleTexture(textureHD, 64);
                    this.TextureHD96 = ModCreator.ScaleTexture(textureHD, 96);
                }

                using (Image textureSD = Image.FromFile(Path.Combine(Globals.templatesDirectory, "textures", template.TextureSD)))
                {
                    this.TextureSD = textureSD;
                    this.TextureSD64 = ModCreator.ScaleTexture(textureSD, 64);
                    this.TextureSD96 = ModCreator.ScaleTexture(textureSD, 96);
                }
            }
        }
    }

    public class ItemTemplate : ItemBase
    {
        public string TextureHD;
        public string TextureSD;
        internal bool currentItem;
        public ItemTemplate() : base(new ModItemDataXML(), new ItemTypeData(), new ItemCombatData(), new ItemSwingTimeData(), new ItemSoundData(), new SkillData())
        {
            this.TextureHD = string.Empty;
            this.TextureSD = string.Empty;
            this.currentItem = false;
        }
        public ItemTemplate(Item item, bool currentItem = false) : base(item.ItemData, item.ItemTypeData, item.ItemCombatData, item.ItemSwingTimeData, item.ItemSoundData, item.SkillData)
        {
            this.TextureHD = string.Empty;
            this.TextureSD = string.Empty;
            this.currentItem = currentItem;
        }
    }

    public class ItemTypeData
    {
        public ItemUse? Use;
        public ItemType? Type;
        public ItemSubType? SubType;
        public string? ClassID;
        public ItemInvType? Inv;
        public string? CombatID;
        public ItemModelType? Model;
        public ItemSwingType? Swing;
        public EquipIndex? Equip;

        public bool UseSpecified { get { return Use != null && Use != ItemUse.Item; } }
        public bool TypeSpecified { get { return Type != null && Type != ItemType.Item; } }
        public bool SubTypeSpecified { get { return SubType != null && SubType != ItemSubType.None; } }
        public bool ClassIDSpecified { get { return ClassID != null && !ClassID.Equals(string.Empty) && !ClassID.Equals("None"); } }
        public bool InvSpecified { get { return Inv != null && Inv != ItemInvType.Other; } }
        public bool CombatIDSpecified { get { return CombatID != null && !CombatID.Equals(string.Empty); } }
        public bool ModelSpecified { get { return Model != null && Model != ItemModelType.Item; } }
        public bool SwingSpecified { get { return Swing != null && Swing != ItemSwingType.Item; } }
        public bool EquipSpecified { get { return Equip != null && Equip != EquipIndex.LeftHand; } }
        public bool IsEmpty()
        {
            return !(UseSpecified || TypeSpecified || SubTypeSpecified || ClassIDSpecified || InvSpecified || CombatIDSpecified || ModelSpecified || SwingSpecified || EquipSpecified);
        }

        public ItemTypeData(ModItemTypeDataXML data)
        {
            this.Use = data.Use;
            this.Type = data.Type;
            this.SubType = data.SubType;
            this.ClassID = data.ClassID;
            this.Inv = data.Inv;
            this.CombatID = data.CombatID;
            this.Model = data.Model;
            this.Swing = data.Swing;
            this.Equip = data.Equip;
        }
        public ItemTypeData() { }
    }

    public class ItemCombatData
    {
        public short? Health;
        public short? Attack;
        public short? Strength;
        public short? Defence;
        public short? Ranged;
        public short? Looting;

        public bool HealthSpecified { get { return Health != null && Health != 0; } }
        public bool AttackSpecified { get { return Attack != null && Attack != 0; } }
        public bool StrengthSpecified { get { return Strength != null && Strength != 0; } }
        public bool DefenceSpecified { get { return Defence != null && Defence != 0; } }
        public bool RangedSpecified { get { return Ranged != null && Ranged != 0; } }
        public bool LootingSpecified { get { return Looting != null && Looting != 0; } }
        public bool IsEmpty()
        {
            return !(HealthSpecified || AttackSpecified || StrengthSpecified || DefenceSpecified || RangedSpecified || LootingSpecified);
        }

        public ItemCombatData(ModItemCombatDataXML data)
        {
            this.Health = data.Health;
            this.Attack = data.Attack;
            this.Strength = data.Strength;
            this.Defence = data.Defence;
            this.Ranged = data.Ranged;
            this.Looting = data.Looting;
        }
        public ItemCombatData() { }
    }

    public class ItemSwingTimeData
    {
        public float Time;
        public float Pause;
        public float ExtendedPause;
        public float RetractTime;
        public bool RetractSmooth;

        public bool TimeSpecified { get { return Time != 0.27f; } }
        public bool PauseSpecified { get { return Pause != 0; } }
        public bool ExtendedPauseSpecified { get { return ExtendedPause != 0; } }
        public bool RetractTimeSpecified { get { return RetractTime != -1; } }
        public bool RetractSmoothSpecified { get { return RetractSmooth != false; } }
        public bool IsEmpty()
        {
            return !(TimeSpecified || PauseSpecified || ExtendedPauseSpecified || RetractTimeSpecified || RetractSmoothSpecified);
        }

        public ItemSwingTimeData(ModItemSwingTimeDataXML data)
        {
            this.Time = ModCreator.GetNull(data.Time, 0.27f);
            this.Pause = ModCreator.GetNull(data.Pause, 0);
            this.ExtendedPause = ModCreator.GetNull(data.ExtendedPause, 0);
            this.RetractTime = ModCreator.GetNull(data.RetractTime, -1);
            this.RetractSmooth = ModCreator.GetNull(data.RetractSmooth, false);
        }
        public ItemSwingTimeData()
        {
            this.Time = 0.27f;
            this.Pause = 0;
            this.ExtendedPause = 0;
            this.RetractTime = -1;
            this.RetractSmooth = false;
        }
    }

    public class ItemSoundData
    {
        public ItemSoundGroup? Group;
        public ItemSoundXML? Sounds;

        public bool GroupSpecified { get { return Group != null && Group != ItemSoundGroup.None; } }
        public bool SoundsSpecified { get { return Sounds != null; } }
        public bool IsEmpty()
        {
            return !(GroupSpecified || SoundsSpecified);
        }

        public ItemSoundData(ModItemSoundDataXML data)
        {
            this.Group = data.Group;
            this.Sounds = data.Sounds;
        }
        public ItemSoundData() { }
    }

    public class ItemTypeClass
    {
        public string ClassID;
        public ushort? Power;
        public ushort? MaxResistance;
        internal bool editable;
        internal bool deletable;

        public bool PowerSpecified { get { return Power != 0; } }
        public bool MaxResistanceSpecified { get { return MaxResistance != 0; } }

        public ItemTypeClass(string classID, ushort power, ushort resistance, bool editable = true, bool deletable = true)
        {
            this.ClassID = classID;
            this.Power = power;
            this.MaxResistance = resistance;
            this.editable = editable;
            this.deletable = deletable;
        }
        public ItemTypeClass(ModItemTypeClassDataXML itemClass, bool editable = true, bool deletable = true)
        {
            this.ClassID = itemClass.ClassID;
            this.Power = itemClass.Power;
            this.MaxResistance = itemClass.MaxResistance;
            this.editable = editable;
            this.deletable = deletable;
        }
        public ItemTypeClass()
        {
            this.ClassID = "CantMine";
        }
    }

    public class Blueprint
    {
        public string ItemID;
        public CraftingType? CraftType;
        public bool? IsValid;
        public bool? IsDefault;
        public Vector2? Depth;
        public InventoryItemNDXML? Result;
        public InventoryItemXML? Material11;
        public InventoryItemXML? Material12;
        public InventoryItemXML? Material13;
        public InventoryItemXML? Material21;
        public InventoryItemXML? Material22;
        public InventoryItemXML? Material23;
        public InventoryItemXML? Material31;
        public InventoryItemXML? Material32;
        public InventoryItemXML? Material33;

        public bool ItemIDSpecified { get { return ItemID != null; } }
        public bool CraftTypeSpecified { get { return CraftType != null; } }
        public bool IsValidSpecified { get { return IsValid != null; } }
        public bool IsDefaultSpecified { get { return IsDefault != null; } }
        public bool DepthSpecified { get { return Depth != null; } }
        public bool ResultSpecified { get { return Result != null; } }
        public bool Material11Specified { get { return Material11 != null; } }
        public bool Material12Specified { get { return Material12 != null; } }
        public bool Material13Specified { get { return Material13 != null; } }
        public bool Material21Specified { get { return Material21 != null; } }
        public bool Material22Specified { get { return Material22 != null; } }
        public bool Material23Specified { get { return Material23 != null; } }
        public bool Material31Specified { get { return Material31 != null; } }
        public bool Material32Specified { get { return Material32 != null; } }
        public bool Material33Specified { get { return Material33 != null; } }

        public Blueprint(ModBlueprintDataXML data)
        {
            this.ItemID = data.ItemID;
            this.CraftType = data.CraftType;
            this.IsValid = data.IsValid;
            this.IsDefault = data.IsDefault;
            this.Depth = data.Depth;
            this.Result = new InventoryItemNDXML(data.Result);
            this.Material11 = new InventoryItemXML(data.Material11);
            this.Material12 = new InventoryItemXML(data.Material12);
            this.Material13 = new InventoryItemXML(data.Material13);
            this.Material21 = new InventoryItemXML(data.Material21);
            this.Material22 = new InventoryItemXML(data.Material22);
            this.Material23 = new InventoryItemXML(data.Material23);
            this.Material31 = new InventoryItemXML(data.Material31);
            this.Material32 = new InventoryItemXML(data.Material32);
            this.Material33 = new InventoryItemXML(data.Material33);
        }
        public Blueprint()
        {
            this.ItemID = "None";
        }
    }

    public class InventoryItemNDXML
    {
        public string ItemID;
        public int? Count;

        public bool CountSpecified { get { return Count != 100; } }
        
        public InventoryItemNDXML(ModInventoryItemNDXML? data)
        {
            if (data != null)
            {
                this.ItemID = data.ItemID;
                this.Count = data.Count;
            }
            else
            {
                this.ItemID = "None";
                this.Count = 0;
            }
        }
        public InventoryItemNDXML()
        {
            this.ItemID = "None";
            this.Count = 0;
        }
    }

    public class InventoryItemXML
    {
        public string ItemID;
        public ushort? Durability;
        public int? Count;

        public bool DurabilitySpecified { get { return Durability != 0; } }
        public bool CountSpecified { get { return Count != 100; } }
        public InventoryItemXML(ModInventoryItemXML? data)
        {
            if (data != null)
            {
                this.ItemID = data.ItemID;
                this.Count = data.Count;
            }
            else
            {
                this.ItemID = "None";
                this.Count = 0;
            }
        }
        public InventoryItemXML()
        {
            this.ItemID = "None";
            this.Count = 0;
        }
    }

    public class SkillData
    {
        public SkillType? UseSkill;
        public int? UseReq;
        public int? MineReq;
        public SkillType? CraftSkill;
        public int? CraftReq;

        public bool UseSkillSpecified { get { return UseSkill != null && UseSkill != SkillType.None; } }
        public bool UseReqSpecified { get { return UseReq != null && UseReq != 0; } }
        public bool MineReqSpecified { get { return MineReq != null && MineReq != 0; } }
        public bool CraftSkillSpecified { get { return CraftSkill != null && CraftSkill != SkillType.Crafting; } }
        public bool CraftReqSpecified { get { return CraftReq != null && CraftReq != 0; } }
        public bool IsEmpty() { return !(UseSkillSpecified || UseReqSpecified || MineReqSpecified || CraftSkillSpecified || CraftReqSpecified); }
        public SkillData(ModSkillDataXML data)
        {
            this.UseSkill = data.UseSkill;
            this.UseReq = data.UseReq;
            this.MineReq = data.MineReq;
            this.CraftSkill = data.CraftSkill;
            this.CraftReq = data.CraftReq;
        }
        public SkillData() { }
    }
}
