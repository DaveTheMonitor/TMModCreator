namespace DaveTheMonitor.TMModCreator
{
    public partial class MainForm : Form
    {
        List<Panel> otherDataPanels = new List<Panel>();
        internal BlueprintDataPictureBox[] bpBoxes = new BlueprintDataPictureBox[9];
        internal BlueprintDataPictureBox? selectedBPBox = null;
        Label blueprintOutputCountLabel;

        private void UpdateOtherDataPanelVisibility(int index)
        {
            for (int i = 0; i < otherDataPanels.Count; i++)
            {
                if (i != index) otherDataPanels[i].Visible = false;
                else otherDataPanels[i].Visible = true;
            }
        }

        private void LoadMod()
        {
            if (Globals.modName != null)
            {
                DialogResult confirmResult = ModCreator.ShowWarningBox(this, "Open a new mod?", $"Open a new mod? All unsaved progress will be lost. If you want to keep your current progress, build the current mod before opening a new one.", MessageBoxButtons.YesNo);
                if (confirmResult != DialogResult.Yes) return;
            }
            DialogResult result;
            string path;
            using (FolderBrowserDialog browser = new FolderBrowserDialog())
            {
                browser.Description = "Open an existing mod.";
                browser.ShowNewFolderButton = false;
                browser.UseDescriptionForTitle = true;
                result = browser.ShowDialog(this);
                path = browser.SelectedPath;
            }
            if (result == DialogResult.OK)
            {
                LoadMod(path);
            }
        }

        private void LoadMod(string path)
        {
            string[] files = Directory.GetFiles(path);
            string itemDataPath = Path.Combine(path, "ItemData.xml");
            if (!files.Contains(itemDataPath))
            {
                ModCreator.ShowErrorBox(this, "Invalid Mod", "The selected folder is not a valid mod; missing ItemData.xml.");
                return;
            }
            if (Globals.modName != null)
            {
                UnloadMod();
            }
            string itemTypeDataPath = Path.Combine(path, "ItemTypeData.xml");
            string itemCombatDataPath = Path.Combine(path, "ItemCombatData.xml");
            string itemSwingTimeDataPath = Path.Combine(path, "ItemSwingTimeData.xml");
            string itemSoundDataPath = Path.Combine(path, "ItemSoundData.xml");
            string skillDataPath = Path.Combine(path, "SkillData.xml");
            string blueprintDataPath = Path.Combine(path, "BlueprintData.xml");
            string itemClassPath = Path.Combine(path, "ItemTypeClassData.xml");
            ModItemDataXML[] itemData = ModCreator.Deserialize<ModItemDataXML[]>(Path.Combine(itemDataPath));
            foreach (ModItemDataXML itemXML in itemData)
            {
                RegisterItem(new Item(itemXML));
            }

            // This should probably be changed before a full release.
            if (files.Contains(itemTypeDataPath))
            {
                ModItemTypeDataXML[] typeData = ModCreator.Deserialize<ModItemTypeDataXML[]>(itemTypeDataPath);
                foreach (ModItemTypeDataXML typeXML in typeData)
                {
                    try
                    {
                        Item item = Globals.items[typeXML.ItemID];
                        item.ItemTypeData = new ItemTypeData(typeXML);
                    }
                    catch
                    {
                        throw new Exception($"Invalid ItemTypeData.xml; {typeXML.ItemID} not present in ItemData.xml.");
                    }
                }
            }
            if (files.Contains(itemCombatDataPath))
            {
                ModItemCombatDataXML[] combatData = ModCreator.Deserialize<ModItemCombatDataXML[]>(itemCombatDataPath);
                foreach (ModItemCombatDataXML combatXML in combatData)
                {
                    try
                    {
                        foreach (Item item in Globals.itemsList)
                        {
                            if (item.ItemTypeData != null && item.ItemTypeData.CombatID == combatXML.CombatID)
                            {
                                item.ItemCombatData = new ItemCombatData(combatXML);
                            }
                        }
                    }
                    catch
                    {
                        throw new Exception($"Invalid ItemCombatData.xml.");
                    }
                }
            }
            if (files.Contains(itemSwingTimeDataPath))
            {
                ModItemSwingTimeDataXML[] swingTimeData = ModCreator.Deserialize<ModItemSwingTimeDataXML[]>(itemSwingTimeDataPath);
                foreach (ModItemSwingTimeDataXML swingXML in swingTimeData)
                {
                    try
                    {
                        Item item = Globals.items[swingXML.ItemID];
                        item.ItemSwingTimeData = new ItemSwingTimeData(swingXML);
                    }
                    catch
                    {
                        throw new Exception($"Invalid ItemSwingTimeData.xml; {swingXML.ItemID} not present in ItemData.xml.");
                    }
                }
            }
            if (files.Contains(itemSoundDataPath))
            {
                ModItemSoundDataXML[] soundData = ModCreator.Deserialize<ModItemSoundDataXML[]>(itemSoundDataPath);
                foreach (ModItemSoundDataXML soundXML in soundData)
                {
                    try
                    {
                        Item item = Globals.items[soundXML.ItemID];
                        item.ItemSoundData = new ItemSoundData(soundXML);
                    }
                    catch
                    {
                        throw new Exception($"Invalid ItemSoundData.xml; {soundXML.ItemID} not present in ItemData.xml.");
                    }
                }
            }
            if (files.Contains(skillDataPath))
            {
                ModSkillDataXML[] skillData = ModCreator.Deserialize<ModSkillDataXML[]>(skillDataPath);
                foreach (ModSkillDataXML skillXML in skillData)
                {
                    try
                    {
                        Item item = Globals.items[skillXML.ItemID];
                        item.SkillData = new SkillData(skillXML);
                    }
                    catch
                    {
                        throw new Exception($"Invalid ItemSkillData.xml; {skillXML.ItemID} not present in ItemData.xml.");
                    }
                }
            }
            if (files.Contains(blueprintDataPath))
            {
                ModBlueprintDataXML[] blueprintData = ModCreator.Deserialize<ModBlueprintDataXML[]>(blueprintDataPath);
                foreach (ModBlueprintDataXML blueprintXML in blueprintData)
                {
                    try
                    {
                        Item item = Globals.items[blueprintXML.ItemID];
                        item.BlueprintData = new BlueprintData(blueprintXML);
                    }
                    catch
                    {
                        throw new Exception($"Invalid BlueprintData.xml; {blueprintXML.ItemID} not present in ItemData.xml.");
                    }
                }
            }

            ReadTextures(path);
            Globals.CreateBaseClasses(this);
            if (files.Contains(itemClassPath))
            {
                ModItemTypeClassDataXML[] itemClasses = ModCreator.Deserialize<ModItemTypeClassDataXML[]>(itemClassPath);
                foreach (ModItemTypeClassDataXML itemClass in itemClasses)
                {
                    if (Globals.classes.ContainsKey(itemClass.ClassID))
                    {
                        ItemTypeClass baseClass = Globals.classes[itemClass.ClassID];
                        if (itemClass.PowerSpecified) baseClass.Power = itemClass.Power;
                        if (itemClass.MaxResistanceSpecified) baseClass.MaxResistance = itemClass.MaxResistance;
                    }
                    else
                    {
                        Globals.RegisterClass(new ItemTypeClass(itemClass), this);
                    }
                }
            }

            Globals.modName = new DirectoryInfo(path).Name;
            itemPropertiesPanel.Visible = true;
            buildModButton.Enabled = true;
        }

        private void UnloadMod()
        {
            UnloadTextureBox();
            DeselectItem();
            DeselectClass();
            DeselectBlueprintPictureBox();
            foreach (BlueprintDataPictureBox bpBox in bpBoxes)
            {
                bpBox.SetTexture(null);
                bpBox.SetCountLabel(1);
            }
            if (blueprintOutputPictureBox.Image != null) blueprintOutputPictureBox.Image.Dispose();
            blueprintOutputPictureBox.Image = null;
            blueprintOutputCountLabel.Visible = false;

            foreach (Item item in Globals.itemsList)
            {
                item.DisposeTextures();
            }
            Globals.ClearItemsList(this);
            Globals.ClearClassesList(this);
        }

        private void UnloadTextureBox()
        {
            if (hdTextureBox.Image != null) hdTextureBox.Image.Dispose();
            if (sdTextureBox.Image != null) sdTextureBox.Image.Dispose();
            hdTextureBox.Image = null;
            sdTextureBox.Image = null;
        }

        private void RegisterItem(Item item)
        {
            Globals.RegisterItem(item, this);
        }

        private void ReadTextures(string modPath)
        {
            ReadTextures(Path.Combine(modPath, "TPI_16.png"), Path.Combine(modPath, "ItemTextures16.xml"), TextureType.SD);
            ReadTextures(Path.Combine(modPath, "TPI_32.png"), Path.Combine(modPath, "ItemTextures32.xml"), TextureType.HDItem);

            foreach (Item item in Globals.itemsList)
            {
                if (item.TextureHD == null) item.TextureHD = Globals.missingTexture32;
                if (item.TextureSD == null) item.TextureSD = Globals.missingTexture16;
            }
        }
        private void ReadTextures(string path, string xmlPath, TextureType type)
        {
            int size = type == TextureType.SD ? 16 : type == TextureType.HDItem ? 32 : 64;
            if (File.Exists(path))
            {
                using (Image atlas = Image.FromFile(path))
                {
                    if (atlas.Width % size == 0 && atlas.Height % size == 0)
                    {
                        ItemXML[] textures = ModCreator.Deserialize<ItemXML[]>(xmlPath);
                        for (int i = 0; i < textures.Length; i++)
                        {
                            ItemXML item = textures[i];
                            if (type == TextureType.SD)
                            {
                                Globals.items[item.ItemID].TextureSD = ModCreator.ScaleAtlasTexture(atlas, size, size, i);
                            }
                            else
                            {
                                Globals.items[item.ItemID].TextureHD = ModCreator.ScaleAtlasTexture(atlas, size, size, i);
                            }
                        }
                    }
                    else
                    {
                        ModCreator.ShowErrorBox(this, "Invalid texture atlas.", $"{new FileInfo(path).Name} has an invalid size. The width and height of the texture atlas should be multiples of {size}. The atlas' size is {atlas.Width}x{atlas.Height}");
                    }
                }
            }
        }
        private void AddItem()
        {
            using AddItemForm addItemForm = new AddItemForm(itemsComboBox.Text);
            DialogResult result = addItemForm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                Item item;
                string itemID = addItemForm.itemIDTextBox.Text;
                if (addItemForm.templateComboBox.SelectedIndex > 0)
                {
                    try
                    {
                        ItemTemplate itemTemplate = addItemForm.itemTemplates[addItemForm.templateComboBox.SelectedIndex - 1];
                        item = new Item(itemTemplate);
                        item.ItemData.ItemID = itemID;
                    }
                    catch
                    {
                        ModCreator.ShowErrorBox(this, "Failed to load template.", $"The template {addItemForm.templateComboBox.SelectedItem} failed to be loaded. This is most likely because the textures could not be loaded. If you are sure that the textures aren't the issue, please contact the program creator, Dave The Monitor.");
                        return;
                    }
                }
                else
                {
                    item = new Item(new ModItemDataXML(itemID, itemID, itemID));
                }
                RegisterItem(item);
                OpenItem(itemID);
            }
        }
        private void DeselectItem()
        {
            changeHDTextureButton.Enabled = false;
            changeSDTextureButton.Enabled = false;
            deleteItemButton.Enabled = false;
            nameTextBox.Enabled = false;
            pluralComboBox.Enabled = false;
            descTextBox.Enabled = false;
            hdTextureBox.Enabled = false;
            sdTextureBox.Enabled = false;
            itemDataPanel.Enabled = false;
            otherItemDataPanel.Enabled = false;
            itemsComboBox.SelectedIndex = -1;
            Globals.selectedItem = Globals.emptyItem;
        }

        private void OpenItem(string itemID)
        {
            if (Globals.items.ContainsKey(itemID))
            {
                Item item = Globals.items[itemID];
                Globals.selectedItem = item;
                itemsComboBox.Text = itemID;
                nameTextBox.Text = item.ItemData.Name;
                descTextBox.Text = item.ItemData.Desc;
                SetTextureBox(item);

                int durability = ModCreator.GetNull(item.ItemData.Durability, 0);
                int sellPrice = ModCreator.GetNull(item.ItemData.MinCSPrice, -1);
                short healPower = ModCreator.GetNull<short>(item.ItemData.HealPower, 0);
                ushort burnTime = ModCreator.GetNull<ushort>(item.ItemData.BurnTime, 0);
                float smeltTime = ModCreator.GetNull(item.ItemData.SmeltTime, 0);
                isEnabledCheckBox.Checked = ModCreator.GetNull(item.ItemData.IsEnabled, true);
                isValidCheckBox.Checked = ModCreator.GetNull(item.ItemData.IsValid, true);
                lockedDDCheckBox.Checked = ModCreator.GetNull(item.ItemData.LockedDD, true);
                lockedSUCheckBox.Checked = ModCreator.GetNull(item.ItemData.LockedSU, false);
                lockedCRCheckBox.Checked = ModCreator.GetNull(item.ItemData.LockedCR, false);
                canDropIfLockedCheckBox.Checked = ModCreator.GetNull(item.ItemData.CanDropIfLocked, false);
                pluralComboBox.SelectedIndex = (int)ModCreator.GetNull(item.ItemData.Plural, PluralType.None);
                stackSizeNumeric.Value = ModCreator.GetNull(item.ItemData.StackSize, 100);
                damageNumeric.Value = (decimal)ModCreator.GetNull(item.ItemData.StrikeDamage, 0);
                reachNumeric.Value = (decimal)ModCreator.GetNull(item.ItemData.StrikeReach, 0);
                particleLightNumeric.Value = ModCreator.GetNull(item.ItemData.ParticleLight, 0);
                durabilityNumeric.Value = durability;
                healPowerNumeric.Value = healPower;
                burnTimeNumeric.Value = burnTime;
                smeltTimeNumeric.Value = (decimal)smeltTime;
                if (sellPrice > -1)
                {
                    purchaseableCheckBox.Checked = true;
                    purchasePriceNumeric.Enabled = true;
                    purchasePriceNumeric.Value = Math.Clamp((decimal)Math.Floor(sellPrice * 1.2f), 0, int.MaxValue);
                }
                else
                {
                    purchaseableCheckBox.Checked = false;
                    purchasePriceNumeric.Enabled = false;
                    purchasePriceNumeric.Value = 0;
                }
                if (durability > 0)
                {
                    durabilityCheckBox.Checked = true;
                    durabilityNumeric.Enabled = true;
                    stackSizeNumeric.Enabled = false;
                }
                else
                {
                    durabilityCheckBox.Checked = false;
                    durabilityNumeric.Enabled = false;
                    stackSizeNumeric.Enabled = true;
                }
                if (healPower > 0)
                {
                    healPowerCheckBox.Checked = true;
                    healPowerNumeric.Enabled = true;
                }
                else
                {
                    healPowerCheckBox.Checked = false;
                    healPowerNumeric.Enabled = false;
                }
                if (burnTime > 0)
                {
                    burnTimeCheckBox.Checked = true;
                    burnTimeNumeric.Enabled = true;
                }
                else
                {
                    burnTimeCheckBox.Checked = false;
                    burnTimeNumeric.Enabled = false;
                }
                if (smeltTime > 0)
                {
                    smeltTimeCheckBox.Checked = true;
                    smeltTimeNumeric.Enabled = true;
                }
                else
                {
                    smeltTimeCheckBox.Checked = false;
                    smeltTimeNumeric.Enabled = false;
                }

                typeComboBox.SelectedItem = ModCreator.GetNull(item.ItemTypeData.Type, ItemType.Item).ToString();
                subTypeComboBox.SelectedItem = ModCreator.GetNull(item.ItemTypeData.SubType, ItemSubType.None).ToString();
                itemClassComboBox.SelectedItem = ModCreator.GetNull(item.ItemTypeData.ClassID, "None").ToString();
                invComboBox.SelectedItem = ModCreator.GetNull(item.ItemTypeData.Inv, ItemInvType.Other).ToString();
                modelComboBox.SelectedItem = ModCreator.GetNull(item.ItemTypeData.Model, ItemModelType.Item).ToString();
                swingComboBox.SelectedItem = ModCreator.GetNull(item.ItemTypeData.Swing, ItemSwingType.Item).ToString();
                equipComboBox.SelectedItem = ModCreator.GetNull(item.ItemTypeData.Equip, EquipIndex.LeftHand).ToString();

                float retractTime = ModCreator.GetNull(item.ItemSwingTimeData.RetractTime, -1f);
                swingTimeNumeric.Value = (decimal)ModCreator.GetNull(item.ItemSwingTimeData.Time, 0.27f);
                extendedPauseNumeric.Value = (decimal)ModCreator.GetNull(item.ItemSwingTimeData.ExtendedPause, 0f);
                retractNumeric.Value = retractTime < 0 ? 0 : (decimal)retractTime;
                pauseNumeric.Value = (decimal)ModCreator.GetNull(item.ItemSwingTimeData.Pause, 0f);
                retractSmoothCheckBox.Checked = ModCreator.GetNull(item.ItemSwingTimeData.RetractSmooth, false);
                if (retractTime >= 0)
                {
                    retractTimeCheckBox.Checked = true;
                    retractNumeric.Enabled = true;
                }
                else
                {
                    retractTimeCheckBox.Checked = false;
                    retractNumeric.Enabled = false;
                }
                UpdateSwingTimePreview();

                combatHealthNumeric.Value = ModCreator.GetNull<short>(item.ItemCombatData.Health, 0);
                combatAttackNumeric.Value = ModCreator.GetNull<short>(item.ItemCombatData.Attack, 0);
                combatStrengthNumeric.Value = ModCreator.GetNull<short>(item.ItemCombatData.Strength, 0);
                combatDefenceNumeric.Value = ModCreator.GetNull<short>(item.ItemCombatData.Defence, 0);
                combatRangedNumeric.Value = ModCreator.GetNull<short>(item.ItemCombatData.Ranged, 0);
                combatLootingNumeric.Value = ModCreator.GetNull<short>(item.ItemCombatData.Looting, 0);

                itemSoundGroupComboBox.SelectedItem = ModCreator.GetNull(item.ItemSoundData.Group, ItemSoundGroup.None).ToString();

                useSkillComboBox.SelectedItem = ModCreator.GetNull(item.SkillData.UseSkill, SkillType.None).ToString();
                useReqNumeric.Value = ModCreator.GetNull(item.SkillData.UseReq, 0);
                craftSkillComboBox.SelectedItem = ModCreator.GetNull(item.SkillData.CraftSkill, SkillType.Crafting).ToString();
                craftReqNumeric.Value = ModCreator.GetNull(item.SkillData.CraftReq, 0);

                changeHDTextureButton.Enabled = true;
                changeSDTextureButton.Enabled = true;
                deleteItemButton.Enabled = true;
                nameTextBox.Enabled = true;
                pluralComboBox.Enabled = true;
                descTextBox.Enabled = true;
                hdTextureBox.Enabled = true;
                sdTextureBox.Enabled = true;
                itemDataPanel.Enabled = true;
                otherItemDataPanel.Enabled = true;
                if (otherItemDataComboBox.SelectedIndex == -1) otherItemDataComboBox.SelectedIndex = 0;

                DeselectBlueprintPictureBox();
                if (blueprintOutputPictureBox.Image != null) blueprintOutputPictureBox.Image.Dispose();
                blueprintOutputPictureBox.Image = ModCreator.ScaleTexture(item.TextureHD, 32);
                blueprintTypeComboBox.SelectedItem = ModCreator.GetNull(item.BlueprintData.CraftType, CraftingType.Crafting).ToString();
                blueprintIsValidCheckBox.Checked = ModCreator.GetNull(item.BlueprintData.IsValid, true);
                blueprintMinDepthNumeric.Value = (decimal)ModCreator.GetNull(item.BlueprintData.Depth.X, 0f) * 100;
                blueprintMaxDepthNumeric.Value = (decimal)ModCreator.GetNull(item.BlueprintData.Depth.Y, 0f) * 100;
                blueprintIsDefaultCheckBox.Checked = ModCreator.GetNull(item.BlueprintData.IsDefault, false);
                blueprintDepthLabel.Enabled = blueprintIsDefaultCheckBox.Checked;
                blueprintMinDepthNumeric.Enabled = blueprintIsDefaultCheckBox.Checked;
                blueprintMaxDepthNumeric.Enabled = blueprintIsDefaultCheckBox.Checked;
                foreach (BlueprintDataPictureBox bpBox in bpBoxes)
                {
                    ModInventoryItemXML material = item.BlueprintData.Materials[bpBox.MaterialIndex];
                    if (bpBox.Image != null) bpBox.SetTexture(null);
                    if (Globals.items.TryGetValue(material.ItemID, out Item? matItem))
                    {
                        bpBox.SetTexture(matItem.TextureHD);
                    }
                    else if (!material.ItemID.Equals(string.Empty) && !material.ItemID.Equals(Globals.stringNone))
                    {
                        bpBox.SetTexture(Globals.unknownTexture16);
                    }
                    bpBox.SetCountLabel(material.Count);
                }
                if (item.BlueprintData.Count > 1)
                {
                    blueprintOutputCountLabel.Visible = true;
                    blueprintOutputCountLabel.Text = item.BlueprintData.Count.ToString();
                }
                else
                {
                    blueprintOutputCountLabel.Visible = false;
                }
            }
        }
        private void DeselectBlueprintPictureBox()
        {
            blueprintOutputPictureBox.BackColor = SystemColors.ControlDark;
            if (selectedBPBox != null) selectedBPBox.DeselectBox(false);
            else
            {
                blueprintItemIDComboBox.Enabled = false;
                blueprintCountLabel.Enabled = false;
                blueprintCountNumeric.Enabled = false;
                blueprintDurabilityNumeric.Enabled = false;
                blueprintDurabilityCheckBox.Enabled = false;
            }
        }
        private void DeleteItem()
        {
            DialogResult result;
            if (ModifierKeys == Keys.Shift)
            {
                result = DialogResult.Yes;
            }
            else
            {
                result = MessageBox.Show(this, $"{Globals.selectedItem.ItemData.ItemID} will be permanently removed. This action cannot be undone.\r\n\r\nWarning: Removing items can break existing worlds using the mod. For this reason, it is ill-advised to remove an item if your mod has already been released.", $"Permanently remove {Globals.selectedItem.ItemData.ItemID}?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            if (result == DialogResult.Yes)
            {
                DeleteItem(Globals.selectedItem.ItemData.ItemID);
            }
        }
        private void DeleteItem(string itemID)
        {
            Item item = Globals.items[itemID];
            if (Globals.selectedItem == item)
            {
                UnloadTextureBox();
                itemsComboBox.Text = string.Empty;
                DeselectItem();
            }
            item.DisposeTextures();
            Globals.itemsList.Remove(item);
            Globals.items.Remove(item.ItemData.ItemID);
            itemsComboBox.Items.Remove(item.ItemData.ItemID);
        }
        void ChangeTexture(string path, TextureType type)
        {
            byte textureSize;
            if (type == TextureType.HDBlock) textureSize = 64;
            else if (type == TextureType.HDItem) textureSize = 32;
            else textureSize = 16;

            try
            {
                using Image originalTexture = Image.FromFile(path);
                if (originalTexture.Width != textureSize || originalTexture.Height != textureSize)
                {
                    ModCreator.ShowErrorBox(this, "Unsupported texture size.", $"The selected texture size is {originalTexture.Width}x{originalTexture.Height}. Only {textureSize}x{textureSize} is supported for {(type == TextureType.HDItem ? "HD item" : type == TextureType.HDBlock ? "HD Block" : "SD")} textures.");
                    return;
                }
                if (type == TextureType.SD)
                {
                    Globals.selectedItem.DisposeTextures(DisposeTextureOptions.SD);
                    Globals.selectedItem.TextureSD = ModCreator.ScaleTexture(originalTexture, textureSize);
                    SetTextureBox(Globals.selectedItem, TextureBoxOptions.SD);
                }
                else
                {
                    Globals.selectedItem.DisposeTextures(DisposeTextureOptions.HD);
                    Globals.selectedItem.TextureHD = ModCreator.ScaleTexture(originalTexture, textureSize);
                    SetTextureBox(Globals.selectedItem, TextureBoxOptions.HD);
                }
            }
            catch
            {
                ModCreator.ShowErrorBox(this, "There was a problem loading the texture.", "The image could not be loaded. Please ensure it is in the correct format and size (16x16 for SD items/blocks, 32x32 for HD items, 64x64 for HD blocks) and is not corrupted.");
            }
        }
        private static void DragEventEffect(DragEventArgs e, string format)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
        }
        private void OpenClass(string classID)
        {
            if (Globals.classes.ContainsKey(classID))
            {
                Globals.selectedClass = Globals.classes[classID];
                classPowerNumeric.Value = ModCreator.GetNull(Globals.selectedClass.Power, 0);
                classResistanceNumeric.Value = ModCreator.GetNull(Globals.selectedClass.MaxResistance, 0);

                classPowerLabel.Enabled = Globals.selectedClass.editable;
                classPowerNumeric.Enabled = Globals.selectedClass.editable;
                classResistanceLabel.Enabled = Globals.selectedClass.editable;
                classResistanceNumeric.Enabled = Globals.selectedClass.editable;
                deleteClassButton.Enabled = Globals.selectedClass.deletable;
            }
        }
        private void RegisterClass(ItemTypeClass itemClass)
        {
            Globals.RegisterClass(itemClass, this);
            classComboBox.Items.Add(itemClass.ClassID);
            itemClassComboBox.Items.Add(itemClass.ClassID);
        }
        private void DeleteClass(string classID)
        {
            ItemTypeClass itemClass = Globals.classes[classID];
            if (Globals.selectedClass == itemClass)
            {
                DeselectClass();
            }
            Globals.classesList.Remove(itemClass);
            Globals.classes.Remove(itemClass.ClassID);
            itemClassComboBox.Items.Remove(itemClass.ClassID);
            classComboBox.Items.Remove(itemClass.ClassID);
            foreach (Item item in Globals.itemsList)
            {
                if (item.ItemTypeData.ClassID == classID)
                {
                    item.ItemTypeData.ClassID = "CantMine";
                    if (Globals.selectedItem == item)
                    {
                        itemClassComboBox.SelectedIndex = itemClassComboBox.Items.IndexOf("CantMine");
                    }
                }
            }
        }
        private void DeselectClass()
        {
            classPowerLabel.Enabled = false;
            classPowerNumeric.Enabled = false;
            classResistanceLabel.Enabled = false;
            classResistanceNumeric.Enabled = false;
            deleteClassButton.Enabled = false;
            classComboBox.SelectedIndex = -1;
            Globals.selectedClass = Globals.emptyClass;
        }
        private void UpdateSwingTimePreview()
        {
            ItemSwingTimeData swing = Globals.selectedItem.ItemSwingTimeData;
            float extendTime = swing.Time - swing.ExtendedPause - swing.Pause;
            float retractTime;
            if (swing.RetractTime < 0)
            {
                extendTime /= 2;
                retractTime = extendTime;
            }
            else
            {
                retractTime = swing.RetractTime;
                extendTime -= retractTime;
            }

            int width = swingTimePreviewPanel.Width;
            int extendWidth = (int)(width * (extendTime / swing.Time));
            int extendedPauseWidth = (int)(width * (swing.ExtendedPause / swing.Time));
            int retractWidth = (int)(width * (retractTime / swing.Time));
            int pauseWidth = (int)(width * (swing.Pause / swing.Time));
            swingTimePreviewExtendPanel.Width = extendWidth;
            swingTimePreviewExtendedPausePanel.Location = new Point(extendWidth, 0);
            swingTimePreviewExtendedPausePanel.Width = extendedPauseWidth;
            swingTimePreviewRetractPanel.Location = new Point(swingTimePreviewExtendedPausePanel.Location.X + extendedPauseWidth);
            swingTimePreviewRetractPanel.Width = retractWidth;
            swingTimePreviewPausePanel.Location = new Point(swingTimePreviewRetractPanel.Location.X + retractWidth, 0);
            swingTimePreviewPausePanel.Width = pauseWidth;
            swingPreviewExtendToolTip.SetToolTip(swingTimePreviewExtendPanel, $"Extending ({FloatToString(extendTime)} seconds)");
            swingPreviewExtendedPauseToolTip.SetToolTip(swingTimePreviewExtendedPausePanel, $"Extended Pause ({FloatToString(swing.ExtendedPause)} seconds)");
            if (swing.RetractTime > 0) swingPreviewRetractToolTip.SetToolTip(swingTimePreviewRetractPanel, $"Retracting ({FloatToString(retractTime)} seconds)");
            else swingPreviewRetractToolTip.SetToolTip(swingTimePreviewRetractPanel, $"Automatic Retracting ({FloatToString(retractTime)} seconds)");
            swingPreviewPauseToolTip.SetToolTip(swingTimePreviewPausePanel, $"Pausing ({FloatToString(swing.Pause)} seconds)");
            SetVisibility(swingTimePreviewExtendPanel, extendTime);
            SetVisibility(swingTimePreviewExtendedPausePanel, swing.ExtendedPause);
            SetVisibility(swingTimePreviewRetractPanel, retractTime);
            SetVisibility(swingTimePreviewPausePanel, swing.Pause);

            static void SetVisibility(Panel panel, float time)
            {
                if (time > 0) panel.Visible = true;
                else panel.Visible = false;
            }
            static string FloatToString(float value)
            {
                return value.ToString("0.###");
            }
        }
        private float GetAvailableSwingTime(ItemSwingTimeData swing, float sub)
        {
            float remainingTime = swing.Time - swing.Pause - swing.ExtendedPause;
            if (swing.RetractTime >= 0)
            {
                remainingTime -= swing.RetractTime;
            }
            remainingTime += sub;
            return remainingTime;
        }
        private void ExportMod()
        {
            using ExportModForm exportModForm = new ExportModForm();
            exportModForm.ShowDialog();
            DialogResult result = exportModForm.DialogResult;
            if (result == DialogResult.OK)
            {
                MessageBox.Show(this, "Successfully built mod. If you encounter any problems with the mod, please contact the creator of this program, Dave The Monitor.", "Successfully built mod.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void NewMod()
        {
            bool needConfirm = false;
            DialogResult confirmResult = DialogResult.No;
            DialogResult result;
            string modName;
            if (Globals.modName != null)
            {
                needConfirm = true;
                confirmResult = ModCreator.ShowWarningBox(this, "Create a new mod?", $"Create a new mod? All unsaved progress will be lost. If you want to keep your current progress, build the current mod before creating a new one.", MessageBoxButtons.YesNo);
            }
            if ((needConfirm && confirmResult == DialogResult.Yes) || !needConfirm)
            {
                using (NewModForm newModForm = new NewModForm())
                {
                    newModForm.ShowDialog();
                    result = newModForm.DialogResult;
                    modName = newModForm.modNameTextBox.Text;
                }
                if (result == DialogResult.OK)
                {
                    if (needConfirm && confirmResult == DialogResult.Yes)
                    {
                        UnloadMod();
                    }
                    else if (needConfirm) return;
                    Globals.modName = modName;
                    Globals.selectedItem = Globals.emptyItem;
                    if (Globals.baseClasses.Count == 0) Globals.CreateBaseClasses(this);
                    foreach (ItemTypeClass itemClass in Globals.classesList)
                    {
                        classComboBox.Items.Add(itemClass.ClassID);
                        itemClassComboBox.Items.Add(itemClass.ClassID);
                    }

                    itemPropertiesPanel.Visible = true;
                    buildModButton.Enabled = true;
                }
            }
        }

        internal void SetTextureBox(Item item, TextureBoxOptions options = TextureBoxOptions.Both)
        {
            if (options == TextureBoxOptions.HD || options == TextureBoxOptions.Both)
            {
                if (hdTextureBox.Image != null) hdTextureBox.Image.Dispose();
                hdTextureBox.Image = ModCreator.ScaleTexture(item.TextureHD, 96);
            }
            if (options == TextureBoxOptions.SD || options == TextureBoxOptions.Both)
            {
                if (sdTextureBox.Image != null) sdTextureBox.Image.Dispose();
                sdTextureBox.Image = ModCreator.ScaleTexture(item.TextureSD, 96);
            }
        }
    }
}
