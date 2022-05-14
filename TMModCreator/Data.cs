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
        public BlueprintData BlueprintData;
        public bool ItemTypeDataSpecified { get { return ItemTypeData != null && !ItemTypeData.IsEmpty(); } }
        public bool ItemCombatDataSpecified { get { return !ItemCombatData.IsEmpty(); } }
        public bool ItemSwingTimeDataSpecified { get { return !ItemSwingTimeData.IsEmpty(); } }
        public bool ItemSoundDataSpecified { get { return !ItemSoundData.IsEmpty(); } }
        public bool SkillDataSpecified { get { return !SkillData.IsEmpty(); } }
        public bool BlueprintDataSpecified { get { return BlueprintData.MaterialsSpecified; } }
        public ItemBase(ModItemDataXML itemData, ItemTypeData typeData, ItemCombatData combatData, ItemSwingTimeData swingData, ItemSoundData soundData, SkillData skillData, BlueprintData blueprintData)
        {
            this.ItemData = itemData;
            this.ItemTypeData = typeData;
            this.ItemCombatData = combatData;
            this.ItemSwingTimeData = swingData;
            this.ItemSoundData = soundData;
            this.SkillData = skillData;
            this.BlueprintData = blueprintData;
        }
    }
    
    public class Item : ItemBase
    {
        public Image TextureSD;
        public Image TextureHD;
        internal void DisposeTextures()
        {
            if (TextureHD != Globals.missingTexture32 && TextureHD != Globals.missingTexture32)
            {
                TextureHD.Dispose();
            }
            if (TextureSD != Globals.missingTexture16)
            {
                TextureSD.Dispose();
            }
        }
        internal void DisposeTextures(DisposeTextureOptions options)
        {
            if (options == DisposeTextureOptions.HD)
            {
                if (TextureHD != Globals.missingTexture32 && TextureHD != Globals.missingTexture64)
                {
                    TextureHD.Dispose();
                }
            }
            else if (options == DisposeTextureOptions.SD)
            {
                if (TextureSD != Globals.missingTexture16)
                {
                    TextureSD.Dispose();
                }
            }
        }
        public Item Clone()
        {
            Item item = (Item)MemberwiseClone();
            item.TextureHD = (Image)TextureHD.Clone();
            item.TextureSD = (Image)TextureSD.Clone();
            item.ItemData = ItemData.Clone();
            item.ItemTypeData = ItemTypeData.Clone();
            item.ItemCombatData = ItemCombatData.Clone();
            item.ItemSwingTimeData = ItemSwingTimeData.Clone();
            item.ItemSoundData = ItemSoundData.Clone();
            item.SkillData = SkillData.Clone();
            item.BlueprintData = BlueprintData.Clone();
            return item;
        }
        public Item(ModItemDataXML itemData) : base(itemData, new ItemTypeData(), new ItemCombatData(), new ItemSwingTimeData(), new ItemSoundData(), new SkillData(), new BlueprintData())
        {
            this.ItemData = itemData;
            this.TextureHD = Globals.missingTexture32;
            this.TextureSD = Globals.missingTexture16;
        }
        public Item() : base(new ModItemDataXML(), new ItemTypeData(), new ItemCombatData(), new ItemSwingTimeData(), new ItemSoundData(), new SkillData(), new BlueprintData())
        {
            this.TextureHD = Globals.missingTexture32;
            this.TextureSD = Globals.missingTexture16;
        }
        public Item(ItemTemplate template) : base(template.ItemData, template.ItemTypeData, template.ItemCombatData, template.ItemSwingTimeData, template.ItemSoundData, template.SkillData, template.BlueprintData)
        {
            if (template.currentItem)
            {
                this.TextureHD = ModCreator.ScaleTexture(Globals.selectedItem.TextureHD, 32);
                this.TextureSD = ModCreator.ScaleTexture(Globals.selectedItem.TextureSD, 16);
            }
            else
            {
                this.TextureHD = Image.FromFile(Path.Combine(Globals.templatesDirectory, "textures", template.TextureHD));
                this.TextureSD = Image.FromFile(Path.Combine(Globals.templatesDirectory, "textures", template.TextureSD));
            }
        }
    }

    public class ItemTemplate : ItemBase
    {
        public string TextureHD;
        public string TextureSD;
        internal bool currentItem;
        public ItemTemplate() : base(new ModItemDataXML(), new ItemTypeData(), new ItemCombatData(), new ItemSwingTimeData(), new ItemSoundData(), new SkillData(), new BlueprintData())
        {
            this.TextureHD = string.Empty;
            this.TextureSD = string.Empty;
            this.currentItem = false;
        }
        public ItemTemplate(Item item, bool currentItem = false) : base(new ModItemDataXML(), new ItemTypeData(), new ItemCombatData(), new ItemSwingTimeData(), new ItemSoundData(), new SkillData(), new BlueprintData())
        {
            Item itemClone = item.Clone();
            ItemData = itemClone.ItemData;
            ItemTypeData = itemClone.ItemTypeData;
            ItemCombatData = itemClone.ItemCombatData;
            ItemSwingTimeData = itemClone.ItemSwingTimeData;
            ItemSoundData = itemClone.ItemSoundData;
            SkillData = itemClone.SkillData;
            BlueprintData = itemClone.BlueprintData;
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

        public ItemTypeData Clone()
        {
            return (ItemTypeData)MemberwiseClone();
        }
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

        public ItemCombatData Clone()
        {
            return (ItemCombatData)MemberwiseClone();
        }
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

        public ItemSwingTimeData Clone()
        {
            return (ItemSwingTimeData)MemberwiseClone();
        }
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

        public ItemSoundData Clone()
        {
            return (ItemSoundData)MemberwiseClone();
        }
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

    public class SkillData
    {
        public SkillType? UseSkill;
        public int? UseReq;
        public int? MineReq;
        public SkillType? CraftSkill;
        public int? CraftReq;

        public SkillData Clone()
        {
            return (SkillData)MemberwiseClone();
        }
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

    public class BlueprintData
    {
        public CraftingType CraftType;
        public bool IsValid;
        public bool IsDefault;
        public Vector2 Depth;
        public int Count;
        public ModInventoryItemXML[] Materials;

        public BlueprintData Clone()
        {
            BlueprintData data = (BlueprintData)MemberwiseClone();
            Materials = new ModInventoryItemXML[9];
            for (byte i = 0; i < 9; i++)
            {
                Materials[i] = data.Materials[i].Clone();
            }
            return data;
        }
        public bool MaterialsSpecified
        {
            get
            {
                bool specified = false;
                foreach (ModInventoryItemXML mat in Materials)
                {
                    if (!mat.ItemID.Equals(Globals.stringNone) && !mat.ItemID.Equals(string.Empty))
                    {
                        specified = true;
                    }
                }
                return specified;
            }
        }
        public BlueprintData()
        {
            CraftType = CraftingType.Crafting;
            IsValid = true;
            IsDefault = false;
            Depth = new Vector2(0, 0);
            Count = 1;
            Materials = new ModInventoryItemXML[9];
            for (int i = 0; i < Materials.Length; i++)
            {
                Materials[i] = new ModInventoryItemXML();
            }
        }
        public BlueprintData(ModBlueprintDataXML data)
        {
            CraftType = ModCreator.GetNull(data.CraftType, CraftingType.Crafting);
            IsValid = ModCreator.GetNull(data.IsValid, true);
            IsDefault = ModCreator.GetNull(data.IsDefault, false);
            Depth = ModCreator.GetNull(data.Depth, new Vector2(0, 0));
            if (data.Result != null)
            {
                Count = ModCreator.GetNull(data.Result.Count, 1);
            }
            else Count = 1;
            Materials = new ModInventoryItemXML[9];
            Materials[0] = ModCreator.GetNull(data.Material31, new ModInventoryItemXML());
            Materials[1] = ModCreator.GetNull(data.Material32, new ModInventoryItemXML());
            Materials[2] = ModCreator.GetNull(data.Material33, new ModInventoryItemXML());
            Materials[3] = ModCreator.GetNull(data.Material21, new ModInventoryItemXML());
            Materials[4] = ModCreator.GetNull(data.Material22, new ModInventoryItemXML());
            Materials[5] = ModCreator.GetNull(data.Material23, new ModInventoryItemXML());
            Materials[6] = ModCreator.GetNull(data.Material11, new ModInventoryItemXML());
            Materials[7] = ModCreator.GetNull(data.Material12, new ModInventoryItemXML());
            Materials[8] = ModCreator.GetNull(data.Material13, new ModInventoryItemXML());
        }
    }
}
