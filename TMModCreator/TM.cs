using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DaveTheMonitor.TMModCreator
{
    public enum ItemSelectModeFlag : byte
    {
        Fill = 1,
        MultiTexture = 2
    }
    public enum PluralType : byte
    {
        None,
        S,
        ES
    }
    public enum ItemUse : byte
    {
        None,
        Block,
        Item
    }
    public enum ItemType : byte
    {
        None,
        Block,
        Item,
        Tool,
        Weapon,
        Armor,
        Power,
        Food,
        Decor,
        Jewelry
    }
    public enum ItemSubType : byte
    {
        None,
        Bow,
        Arrow,
        Shield,
        Edible,
        TillTool,
        HarvestTool,
        Grenade,
        GrenadeLauncher,
        Key,
        Door,
        RangedWeapon,
        BlockCanBeOpened,
        Leaves,
        Gun,
        RapidSwing,
        Potion
    }
    public enum ItemInvType : byte
    {
        None,
        Natural,
        Stone,
        Ore,
        Flora,
        Utility,
        Building,
        Color,
        Tool,
        Weapon,
        Armor,
        Food,
        Jewelry,
        Key,
        Other
    }
    public enum ItemModelType : byte
    {
        None,
        Block,
        IconBlock,
        BigIconBlock,
        Item,
        MediumItem,
        MediumItemFront,
        ItemTLBR,
        Tool,
        Hatchet,
        Weapon,
        WeaponTLBR,
        WeaponTRBL,
        BigWeapon,
        SteelScimitar,
        SteelClaymore,
        Bow,
        Arrow,
        Armor,
        Shield,
        Key,
        Jewelry,
        Door,
        Torch,
        GunHand,
        GunRifle,
        Clipboard,
        Staff
    }
    public enum ItemSwingType : byte
    {
        None,
        Block,
        Item,
        IconBlock,
        Ramp,
        Weapon,
        WeaponTRBL,
        Spear,
        Bow,
        Shield,
        Eating,
        Arrow,
        SwitchArrow,
        GunHand,
        GunRifle,
        Key,
        Staff
    }
    public enum EquipIndex : byte
    {
        None,
        Head,
        Neck,
        Body,
        Legs,
        Feet,
        LeftSide,
        RightSide,
        LeftHand,
        RightHand
    }
    public enum ItemSoundGroup : byte
    {
        None,
        Base,
        BodyHit,
        EnvCaveIn,
        EnvExplosion,
        EnvNightfall,
        GenPickup,
        GuiAccept,
        GuiCancel,
        GuiInvalid,
        GuiMoveCursor,
        GuiSelect,
        GuiTransfer,
        GuiGamerJoined,
        GuiTxtMsgIn,
        ItemActivate,
        ItemArmor,
        ItemBlock,
        ItemBow,
        ItemCooked,
        ItemCrop,
        ItemDiamantiumTool,
        ItemDiamondTool,
        ItemWoodDoor,
        ItemMetalDoor,
        ItemEarth,
        ItemFire,
        ItemFlora,
        ItemGem,
        ItemGlass,
        ItemGrass,
        ItemGreenstoneTool,
        ItemIronTool,
        ItemJewelry,
        ItemKey,
        ItemLava,
        ItemLeather,
        ItemMetal,
        ItemOre,
        ItemPorous,
        ItemPortcullis,
        ItemRareTool,
        ItemRaw,
        ItemRock,
        ItemRope,
        ItemRubyTool,
        ItemSand,
        ItemShield,
        ItemSkill,
        ItemSnow,
        ItemSteelTool,
        ItemStone,
        ItemTile,
        ItemTitaniumTool,
        ItemTree,
        ItemWater,
        ItemWood,
        ItemWoodTool,
        ItemWool,
        ItemGun,
        ItemGunSMG,
        ItemGunLaser
    }
    public enum CraftingType : byte
    {
        Crafting,
        Furnace
    }
    public enum SkillType : byte
    {
        None,
        Health,
        Strength,
        Attack,
        Defence,
        Ranged,
        Mining,
        Digging,
        Chopping,
        Building,
        Crafting,
        Smelting,
        Smithing,
        Farming,
        Cooking,
        Looting,
        Combat,
        Total
    }
    public class ModInventoryItemNDXML
    {
        public string ItemID;
        public int Count;
        public ModInventoryItemNDXML(InventoryItemNDXML data)
        {
            ItemID = data.ItemID;
            if (data.Count == null)
            {
                if (data.ItemID.ToLower() == "none") Count = 0;
                else Count = 1;
            }
            else Count = (int)data.Count;
        }
        public ModInventoryItemNDXML()
        {
            ItemID = "None";
            Count = 0;
        }
    }
    public class ModInventoryItemXML
    {
        public string ItemID;
        public ushort Durability;
        public int Count;
        public ModInventoryItemXML(InventoryItemXML data)
        {
            ItemID = data.ItemID;
            if (data.Durability == null) Durability = 0;
            else Durability = (ushort)data.Durability;
            if (data.Count == null)
            {
                if (data.ItemID.ToLower() == "none") Count = 0;
                else Count = 1;
            }
            else Count = (int)data.Count;
        }
        public ModInventoryItemXML()
        {
            ItemID = "None";
            Durability = 0;
            Count = 0;
        }
    }
    public class ItemSoundXML
    {
        public string[] Step;
        public string[] Mine;
        public string[] Dig;
        public string[] Chop;
        public string[] Use;
        public string[] UseFail;
        public string[] Hit;
        ItemSoundXML()
        {
            Step = new string[0];
            Mine = new string[0];
            Dig = new string[0];
            Chop = new string[0];
            Use = new string[0];
            UseFail = new string[0];
            Hit = new string[0];
        }
    }

    #nullable enable
    public class ModItemDataXML
    {
        public string ItemID;
        public string Name;
        public string Desc;
        public bool? IsValid;
        public bool? IsEnabled;
        public bool? LockedDD;
        public bool? LockedCR;
        public bool? LockedSU;
        public int? MinCSPrice;
        public int? StackSize;
        public ushort? Durability;
        public float? StrikeDamage;
        public float? StrikeReach;
        public short? HealPower;
        public ushort? BurnTime;
        public float? SmeltTime;
        public byte? ParticleLight;
        public ItemSelectModeFlag? SelectFlag;
        public bool? CanDropIfLocked;
        public ushort? DropChance;
        public PluralType? Plural;

        public bool IsValidSpecified { get { return IsValid != null && IsValid != true; } }
        public bool IsEnabledSpecified { get { return IsEnabled != null && IsEnabled != true; } }
        public bool LockedDDSpecified { get { return LockedDD != null && LockedDD != true; } }
        public bool LockedCRSpecified { get { return LockedCR != null && LockedCR != false; } }
        public bool LockedSUSpecified { get { return LockedSU != null && LockedSU != false; } }
        public bool MinCSPriceSpecified { get { return MinCSPrice != null && MinCSPrice != -1; } }
        public bool StackSizeSpecified { get { return StackSize != null && StackSize != 100; } }
        public bool DurabilitySpecified { get { return Durability != null && Durability != 0; } }
        public bool StrikeDamageSpecified { get { return StrikeDamage != null && StrikeDamage != 0; } }
        public bool StrikeReachSpecified { get { return StrikeReach != null && StrikeReach != 0; } }
        public bool HealPowerSpecified { get { return HealPower != null && HealPower != 0; } }
        public bool BurnTimeSpecified { get { return BurnTime != null && BurnTime != 0; } }
        public bool SmeltTimeSpecified { get { return SmeltTime != null && SmeltTime != 0; } }
        public bool ParticleLightSpecified { get { return ParticleLight != null && ParticleLight != 0; } }
        public bool SelectFlagSpecified { get { return SelectFlag != null; } }
        public bool CanDropIfLockedSpecified { get { return CanDropIfLocked != null; } }
        public bool DropChanceSpecified { get { return DropChance != null; } }
        public bool PluralSpecified { get { return Plural != null && Plural != PluralType.None; } }
        public ModItemDataXML(string id, string name, string desc)
        {
            ItemID = id;
            Name = name;
            Desc = desc;
        }
        public ModItemDataXML()
        {
            ItemID = string.Empty;
            Name = string.Empty;
            Desc = string.Empty;
        }

    }
    public class ModItemTypeDataXML
    {
        public string ItemID;
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
        public ModItemTypeDataXML(string id, ItemTypeData data, bool hasCombatData)
        {
            ItemID = id;
            Use = data.Use;
            Type = data.Type;
            SubType = data.SubType;
            ClassID = data.ClassID;
            Inv = data.Inv;
            CombatID = hasCombatData ? id : data.CombatID;
            Model = data.Model;
            Swing = data.Swing;
            Equip = data.Equip;
        }
        public ModItemTypeDataXML()
        {
            ItemID = string.Empty;
        }
    }
    public class ModItemCombatDataXML
    {
        public string CombatID;
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
        public ModItemCombatDataXML(string id, ItemCombatData data)
        {
            CombatID = id;
            Health = data.Health;
            Attack = data.Attack;
            Strength = data.Strength;
            Defence = data.Defence;
            Ranged = data.Ranged;
            Looting = data.Looting;
        }
        public ModItemCombatDataXML()
        {
            CombatID = string.Empty;
        }
    }
    public class ModItemSwingTimeDataXML
    {
        public string ItemID;
        public float? Time;
        public float? Pause;
        public float? ExtendedPause;
        public float? RetractTime;
        public bool? RetractSmooth;

        public bool TimeSpecified { get { return Time != null && Time != 0.27f; } }
        public bool PauseSpecified { get { return Pause != null && Pause != 0; } }
        public bool ExtendedPauseSpecified { get { return ExtendedPause != null && ExtendedPause != 0; } }
        public bool RetractTimeSpecified { get { return RetractTime != null && RetractTime != -1; } }
        public bool RetractSmoothSpecified { get { return RetractSmooth != null && RetractSmooth != false; } }
        public ModItemSwingTimeDataXML(string id, ItemSwingTimeData data)
        {
            ItemID = id;
            Time = data.Time;
            Pause = data.Pause;
            ExtendedPause = data.ExtendedPause;
            RetractTime = data.RetractTime;
            RetractSmooth = data.RetractSmooth;
        }
        public ModItemSwingTimeDataXML()
        {
            ItemID = string.Empty;
        }
    }
    public class ModItemSoundDataXML
    {
        public string ItemID;
        public ItemSoundGroup? Group;
        public ItemSoundXML? Sounds;

        public bool GroupSpecified { get { return Group != null && Group != ItemSoundGroup.None; } }
        public bool SoundsSpecified { get { return Sounds != null; } }
        public ModItemSoundDataXML(string id, ItemSoundData data)
        {
            ItemID = id;
            Group = data.Group;
            Sounds = data.Sounds;
        }
        public ModItemSoundDataXML()
        {
            ItemID = string.Empty;
        }
    }
    public class ItemXML
    {
        public string ItemID;

        public ItemXML(string id)
        {
            ItemID = id;
        }
        public ItemXML()
        {
            ItemID = string.Empty;
        }
    }
    public class ModItemTypeClassDataXML
    {
        public string ClassID;
        public ushort? Power;
        public ushort? MaxResistance;

        public bool PowerSpecified { get { return Power != null; } }
        public bool MaxResistanceSpecified { get { return MaxResistance != null; } }
        public ModItemTypeClassDataXML(ItemTypeClass data)
        {
            ClassID = data.ClassID;
            Power = data.Power;
            MaxResistance = data.MaxResistance;
        }
        public ModItemTypeClassDataXML()
        {
            ClassID = string.Empty;
        }
    }
    public class ModBlueprintDataXML
    {
        public string ItemID;
        public CraftingType? CraftType;
        public bool? IsValid;
        public bool? IsDefault;
        public Vector2? Depth;
        public ModInventoryItemNDXML? Result;
        public ModInventoryItemXML? Material11;
        public ModInventoryItemXML? Material12;
        public ModInventoryItemXML? Material13;
        public ModInventoryItemXML? Material21;
        public ModInventoryItemXML? Material22;
        public ModInventoryItemXML? Material23;
        public ModInventoryItemXML? Material31;
        public ModInventoryItemXML? Material32;
        public ModInventoryItemXML? Material33;

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
        public ModBlueprintDataXML(Blueprint data)
        {
            ItemID = data.ItemID;
            CraftType = data.CraftType;
            IsValid = data.IsValid;
            IsDefault = data.IsDefault;
            Depth = data.Depth;
            if (data.Result != null)
            {
                InventoryItemNDXML itemXML = (InventoryItemNDXML)data.Result;
                if (itemXML.ItemID == null) itemXML.ItemID = data.ItemID;
                if (itemXML.Count == null) itemXML.Count = 1;
                Result = new ModInventoryItemNDXML(itemXML);
            }
            else Result = new ModInventoryItemNDXML() { ItemID = data.ItemID, Count = 1 };
            if (data.Material11 != null) Material11 = new ModInventoryItemXML((InventoryItemXML)data.Material11);
            else Material11 = null;
            if (data.Material12 != null) Material12 = new ModInventoryItemXML((InventoryItemXML)data.Material12);
            else Material12 = null;
            if (data.Material13 != null) Material13 = new ModInventoryItemXML((InventoryItemXML)data.Material13);
            else Material13 = null;
            if (data.Material21 != null) Material21 = new ModInventoryItemXML((InventoryItemXML)data.Material21);
            else Material21 = null;
            if (data.Material22 != null) Material22 = new ModInventoryItemXML((InventoryItemXML)data.Material22);
            else Material22 = null;
            if (data.Material23 != null) Material23 = new ModInventoryItemXML((InventoryItemXML)data.Material23);
            else Material23 = null;
            if (data.Material31 != null) Material31 = new ModInventoryItemXML((InventoryItemXML)data.Material31);
            else Material31 = null;
            if (data.Material32 != null) Material32 = new ModInventoryItemXML((InventoryItemXML)data.Material32);
            else Material32 = null;
            if (data.Material33 != null) Material33 = new ModInventoryItemXML((InventoryItemXML)data.Material33);
            else Material33 = null;
        }
        public ModBlueprintDataXML()
        {
            ItemID = string.Empty;
        }
    }
    public class ModSkillDataXML
    {
        public string ItemID;
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
        public ModSkillDataXML(string id, SkillData data)
        {
            this.ItemID = id;
            this.UseSkill = data.UseSkill;
            this.UseReq = data.UseReq;
            this.MineReq = data.MineReq;
            this.CraftSkill = data.CraftSkill;
            this.CraftReq = data.CraftReq;
        }
        public ModSkillDataXML()
        {
            this.ItemID = string.Empty;
        }
    }
}
