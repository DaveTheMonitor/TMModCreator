namespace DaveTheMonitor.TMModCreator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            devLabel.Visible = Globals.devBuild;
            purchasePriceNumeric.Maximum = int.MaxValue;
            stackSizeNumeric.Maximum = int.MaxValue;
            durabilityNumeric.Maximum = ushort.MaxValue;
            damageNumeric.Maximum = decimal.MaxValue;
            reachNumeric.Maximum = decimal.MaxValue;
            healPowerNumeric.Maximum = short.MaxValue;
            burnTimeNumeric.Maximum = ushort.MaxValue;
            smeltTimeNumeric.Maximum = decimal.MaxValue;
            swingTimeNumeric.Maximum = decimal.MaxValue;
            extendedPauseNumeric.Maximum = decimal.MaxValue;
            retractNumeric.Maximum = decimal.MaxValue;
            pauseNumeric.Maximum = decimal.MaxValue;
            particleLightNumeric.Maximum = 15;

            combatHealthNumeric.Minimum = short.MinValue;
            combatHealthNumeric.Maximum = short.MaxValue;
            combatAttackNumeric.Minimum = short.MinValue;
            combatAttackNumeric.Maximum = short.MaxValue;
            combatStrengthNumeric.Minimum = short.MinValue;
            combatStrengthNumeric.Maximum = short.MaxValue;
            combatDefenceNumeric.Minimum = short.MinValue;
            combatDefenceNumeric.Maximum = short.MaxValue;
            combatRangedNumeric.Minimum = short.MinValue;
            combatRangedNumeric.Maximum = short.MaxValue;
            combatLootingNumeric.Minimum = short.MinValue;
            combatLootingNumeric.Maximum = short.MaxValue;

            useReqNumeric.Maximum = Globals.maxSkillLevel;
            craftReqNumeric.Maximum = Globals.maxSkillLevel;

            blueprintDurabilityNumeric.Maximum = ushort.MaxValue;

            otherDataPanels.Add(itemTypeDataPanel);
            otherDataPanels.Add(itemCombatDataPanel);
            otherDataPanels.Add(itemSwingDataPanel);
            otherDataPanels.Add(itemSoundDataPanel);
            otherDataPanels.Add(itemSkillDataPanel);
            otherDataPanels.Add(blueprintDataPanel);
            foreach (Panel panel in otherDataPanels)
            {
                panel.Location = new Point(0, 43);
            }
            UpdateOtherDataPanelVisibility(0);

            foreach (string plural in Enum.GetNames(typeof(PluralType)))
            {
                pluralComboBox.Items.Add(plural);
            }
            foreach (string group in Enum.GetNames(typeof(ItemSoundGroup)))
            {
                itemSoundGroupComboBox.Items.Add(group);
            }
            string skillTotal = "Total";
            foreach (string skill in Enum.GetNames(typeof(SkillType)))
            {
                if (!skill.Equals(skillTotal))
                {
                    useSkillComboBox.Items.Add(skill);
                    craftSkillComboBox.Items.Add(skill);
                }
            }

            for (byte i = 0; i < 9; i++)
            {
                BlueprintDataPictureBox bpBox = new BlueprintDataPictureBox(i, this);
                bpBoxes[i] = bpBox;

                bpBox.BorderStyle = BorderStyle.Fixed3D;
                bpBox.Parent = blueprintDataPanel;
                bpBox.Size = new Size(40, 40);
                bpBox.Image = null;
                bpBox.Name = $"blueprintDataPictureBox{i}";
                bpBox.Visible = true;
                bpBox.SizeMode = PictureBoxSizeMode.CenterImage;
                int x = 11 + (i % 3 * 40);
                int y = 137 + ((int)Math.Floor(i / 3f) * 40);
                bpBox.Location = new Point(x, y);
                bpBox.BackColor = SystemColors.ControlDark;
            }
            blueprintTypeComboBox.SelectedIndex = 0;

            blueprintOutputCountLabel = new Label();
            blueprintOutputCountLabel.Parent = blueprintOutputPictureBox;
            blueprintOutputCountLabel.BackColor = Color.Transparent;
            blueprintOutputCountLabel.Location = new Point(1, 1);
            blueprintOutputPictureBox.Click += new EventHandler(BlueprintOutputPictureBoxClick);
            blueprintOutputCountLabel.Click += new EventHandler(BlueprintOutputPictureBoxClick);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void loadModButton_Click(object sender, EventArgs e)
        {
            LoadMod();
        }

        private void itemsComboBox_TextChanged(object sender, EventArgs e)
        {
            if (!Globals.items.ContainsKey(itemsComboBox.Text) && !string.IsNullOrWhiteSpace(itemsComboBox.Text) && !itemsComboBox.Text.Contains(' ') && !string.IsNullOrWhiteSpace(Globals.modName)) addItemButton.Enabled = true;
            else addItemButton.Enabled = false;
        }
        
        private void itemsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (itemsComboBox.SelectedIndex != -1)
            {
                OpenItem((string)itemsComboBox.SelectedItem);
            }
        }
        
        private void addItemButton_Click(object sender, EventArgs e)
        {
            AddItem();
        }
        
        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.Name = nameTextBox.Text;
        }
        
        private void descTextBox_TextChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.Desc = descTextBox.Text;
        }
        
        private void purchaseableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            purchasePriceNumeric.Enabled = purchaseableCheckBox.Checked;
            if (purchaseableCheckBox.Checked)
            {
                Globals.selectedItem.ItemData.MinCSPrice = (int)purchasePriceNumeric.Value;
            }
            else
            {
                Globals.selectedItem.ItemData.MinCSPrice = -1;
            }
        }
        
        private void isValidCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.IsValid = isValidCheckBox.Checked;
        }
        
        private void isEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.IsEnabled = isEnabledCheckBox.Checked;
        }
        
        private void lockedDDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.LockedDD = lockedDDCheckBox.Checked;
        }
        
        private void lockedSUCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.LockedSU = lockedSUCheckBox.Checked;
        }
        
        private void lockedCRCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.LockedCR = lockedCRCheckBox.Checked;
        }
        
        private void purchasePriceNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.MinCSPrice = (int)Math.Ceiling(((float)purchasePriceNumeric.Value / 1.2f));
        }
        
        private void pluralComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.Plural = ModCreator.GetEnumValue<PluralType>((string)pluralComboBox.SelectedItem);
        }
        
        private void durabilityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (durabilityCheckBox.Checked)
            {
                durabilityNumeric.Enabled = true;
                stackSizeNumeric.Enabled = false;
                Globals.selectedItem.ItemData.Durability = (ushort)durabilityNumeric.Value;
                if (durabilityNumeric.Value > 0) Globals.selectedItem.ItemData.StackSize = 100;
            }
            else
            {
                durabilityNumeric.Enabled = false;
                stackSizeNumeric.Enabled = true;
                Globals.selectedItem.ItemData.Durability = 0;
                Globals.selectedItem.ItemData.StackSize = (int)stackSizeNumeric.Value;
            }
        }
        
        private void stackSizeNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.StackSize = (int)stackSizeNumeric.Value;
        }
        
        private void durabilityNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.Durability = (ushort)durabilityNumeric.Value;
            Globals.selectedItem.ItemData.StackSize = 100;
        }
        
        private void healPowerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (healPowerCheckBox.Checked)
            {
                healPowerNumeric.Enabled = true;
                Globals.selectedItem.ItemData.HealPower = (short)healPowerNumeric.Value;
            }
            else
            {
                healPowerNumeric.Enabled = false;
                Globals.selectedItem.ItemData.HealPower = 0;
            }
        }
        
        private void burnTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (burnTimeCheckBox.Checked)
            {
                burnTimeNumeric.Enabled = true;
                Globals.selectedItem.ItemData.BurnTime = (ushort)burnTimeNumeric.Value;
            }
            else
            {
                burnTimeNumeric.Enabled = false;
                Globals.selectedItem.ItemData.BurnTime = 0;
            }
        }

        private void smeltTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (smeltTimeCheckBox.Checked)
            {
                smeltTimeNumeric.Enabled = true;
                Globals.selectedItem.ItemData.SmeltTime = (float)smeltTimeNumeric.Value;
            }
            else
            {
                smeltTimeNumeric.Enabled = false;
                Globals.selectedItem.ItemData.SmeltTime = 0;
            }
        }

        private void damageNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.StrikeDamage = (float)damageNumeric.Value;
        }
        
        private void reachNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.StrikeReach = (float)reachNumeric.Value;
        }
        
        private void healPowerNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.HealPower = (short)healPowerNumeric.Value;
        }
        
        private void burnTimeNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.BurnTime = (ushort)burnTimeNumeric.Value;
        }
        
        private void smeltTimeNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.SmeltTime = (short)smeltTimeNumeric.Value;
        }
        
        private void particleLightNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.ParticleLight = (byte)particleLightNumeric.Value;
        }
        
        private void canDropIfLockedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemData.CanDropIfLocked = canDropIfLockedCheckBox.Checked;
        }
        
        private void deleteItemButton_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
        
        private void changeHDTextureButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string path;
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image files (*.png, *.bmp, *.jpg, *.tif)|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff|All files (*.*)|*.*";
                dialog.Multiselect = false;
                dialog.Title = "Open an existing texture.";
                result = dialog.ShowDialog();
                path = dialog.FileName;
            }
            if (result == DialogResult.OK)
            {
                ChangeTexture(path, TextureType.HDItem);
            }
        }
        
        private void changeHDTextureButton_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0) ChangeTexture(files[0], TextureType.HDItem);
            }
        }
        
        private void changeHDTextureButton_DragOver(object sender, DragEventArgs e)
        {
            DragEventEffect(e, DataFormats.FileDrop);
        }
        
        private void changeSDTextureButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string path;
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image files (*.png, *.bmp, *.jpg, *.tif)|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff|All files (*.*)|*.*";
                dialog.Multiselect = false;
                dialog.Title = "Open an existing texture.";
                result = dialog.ShowDialog();
                path = dialog.FileName;
            }
            if (result == DialogResult.OK)
            {
                ChangeTexture(path, TextureType.SD);
            }
        }
        
        private void changeSDTextureButton_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0) ChangeTexture(files[0], TextureType.SD);
            }
        }
        
        private void changeSDTextureButton_DragOver(object sender, DragEventArgs e)
        {
            DragEventEffect(e, DataFormats.FileDrop);
        }
        
        private void equipComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemTypeData.Equip = ModCreator.GetEnumValue<EquipIndex>(equipComboBox.Text);
        }
        
        private void swingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemTypeData.Swing = ModCreator.GetEnumValue<ItemSwingType>((string)swingComboBox.SelectedItem);
        }
        
        private void modelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemTypeData.Model = ModCreator.GetEnumValue<ItemModelType>((string)modelComboBox.SelectedItem);
        }
        
        private void invComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemTypeData.Inv = ModCreator.GetEnumValue<ItemInvType>((string)invComboBox.SelectedItem);
        }
        
        private void itemClassComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemTypeData.ClassID = (string)itemClassComboBox.SelectedItem;
        }
        
        private void subTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemTypeData.SubType = ModCreator.GetEnumValue<ItemSubType>((string)subTypeComboBox.SelectedItem);
        }
        
        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemTypeData.Type = ModCreator.GetEnumValue<ItemType>((string)typeComboBox.SelectedItem);
        }
        
        private void otherItemDataComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOtherDataPanelVisibility(otherItemDataComboBox.SelectedIndex);
        }
        
        private void classComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (classComboBox.SelectedIndex != -1)
            {
                OpenClass((string)classComboBox.SelectedItem);
            }
        }
        
        private void classComboBox_TextChanged(object sender, EventArgs e)
        {
            if (!Globals.classes.ContainsKey(classComboBox.Text) && !string.IsNullOrWhiteSpace(classComboBox.Text) && !classComboBox.Text.Contains(' ') && !string.IsNullOrWhiteSpace(Globals.modName)) addClassButton.Enabled = true;
            else addClassButton.Enabled = false;
        }
        
        private void classPowerNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedClass.Power = (ushort)classPowerNumeric.Value;
        }
        
        private void classResistanceNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedClass.MaxResistance = (ushort)classResistanceNumeric.Value;
        }
        
        private void addClassButton_Click(object sender, EventArgs e)
        {
            ItemTypeClass itemClass = new ItemTypeClass(classComboBox.Text, 0, 0);
            RegisterClass(itemClass);
            OpenClass(itemClass.ClassID);
        }
        
        private void deleteClassButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            if (ModifierKeys == Keys.Shift)
            {
                result = DialogResult.Yes;
            }
            else
            {
                result = MessageBox.Show(this, $"{Globals.selectedClass.ClassID} will be permanently removed. This action cannot be undone. Any items using this class will have their class set to CantMine.", $"Permanently remove {Globals.selectedClass.ClassID}?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            if (result == DialogResult.Yes)
            {
                DeleteClass(Globals.selectedClass.ClassID);
            }
        }
        
        private void retractTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (retractTimeCheckBox.Checked)
            {
                retractNumeric.Enabled = true;
                Globals.selectedItem.ItemSwingTimeData.RetractTime = (float)retractNumeric.Value;
            }
            else
            {
                retractNumeric.Enabled = false;
                Globals.selectedItem.ItemSwingTimeData.RetractTime = -1;
            }
            UpdateSwingTimePreview();
        }

        private void swingTimeNumeric_ValueChanged(object sender, EventArgs e)
        {
            ItemSwingTimeData swing = Globals.selectedItem.ItemSwingTimeData;
            float remainingTime = GetAvailableSwingTime(swing, -(swing.Time - (float)swingTimeNumeric.Value));
            if (remainingTime <= 0)
            {
                swingTimeNumeric.Value -= (decimal)remainingTime;
            }
            if (swingTimeNumeric.Value < 0.01m)
            {
                swingTimeNumeric.Value = 0.01m;
            }
            swing.Time = (float)swingTimeNumeric.Value;
            UpdateSwingTimePreview();
        }

        private void extendedPauseNumeric_ValueChanged(object sender, EventArgs e)
        {
            ItemSwingTimeData swing = Globals.selectedItem.ItemSwingTimeData;
            float remainingTime = GetAvailableSwingTime(swing, swing.ExtendedPause);
            if (remainingTime >= 0 && (float)extendedPauseNumeric.Value > remainingTime)
            {
                extendedPauseNumeric.Value = (decimal)remainingTime;
            }
            swing.ExtendedPause = (float)extendedPauseNumeric.Value;
            UpdateSwingTimePreview();
        }

        private void retractNumeric_ValueChanged(object sender, EventArgs e)
        {
            ItemSwingTimeData swing = Globals.selectedItem.ItemSwingTimeData;
            if (swing.RetractTime < 0) return;
            float remainingTime = GetAvailableSwingTime(swing, swing.RetractTime);
            if (remainingTime >= 0 && (float)retractNumeric.Value > remainingTime)
            {
                retractNumeric.Value = (decimal)remainingTime;
            }
            swing.RetractTime = (float)retractNumeric.Value;
            UpdateSwingTimePreview();
        }

        private void pauseNumeric_ValueChanged(object sender, EventArgs e)
        {
            ItemSwingTimeData swing = Globals.selectedItem.ItemSwingTimeData;
            float remainingTime = GetAvailableSwingTime(swing, swing.Pause);
            if (remainingTime >= 0 && (float)pauseNumeric.Value > remainingTime)
            {
                pauseNumeric.Value = (decimal)remainingTime;
            }
            swing.Pause = (float)pauseNumeric.Value;
            UpdateSwingTimePreview();
        }

        private void combatHealthNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemCombatData.Health = (short)combatHealthNumeric.Value;
        }

        private void combatAttackNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemCombatData.Attack = (short)combatAttackNumeric.Value;
        }

        private void combatRangedNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemCombatData.Ranged = (short)combatRangedNumeric.Value;
        }

        private void combatStrengthNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemCombatData.Strength = (short)combatStrengthNumeric.Value;
        }

        private void combatDefenceNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemCombatData.Defence = (short)combatDefenceNumeric.Value;
        }

        private void combatLootingNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemCombatData.Looting = (short)combatLootingNumeric.Value;
        }

        private void itemSoundGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.ItemSoundData.Group = ModCreator.GetEnumValue<ItemSoundGroup>((string)itemSoundGroupComboBox.SelectedItem);
        }

        private void buildModButton_Click(object sender, EventArgs e)
        {
            ExportMod();
        }

        private void newModButton_Click(object sender, EventArgs e)
        {
            NewMod();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.N:
                        {
                            if (newModButton.Enabled) NewMod();
                            e.Handled = true;
                            break;
                        }
                    case Keys.O:
                        {
                            if (loadModButton.Enabled) LoadMod();
                            e.Handled = true;
                            break;
                        }
                    case Keys.E:
                    case Keys.B:
                        {
                            if (buildModButton.Enabled) ExportMod();
                            e.Handled = true;
                            break;
                        }
                    case Keys.I:
                        {
                            if (addItemButton.Enabled) AddItem();
                            e.Handled = true;
                            break;
                        }
                    case Keys.R:
                        {
                            if (deleteItemButton.Enabled) DeleteItem();
                            e.Handled = true;
                            break;
                        }
                }
            }
            if (e.KeyCode == Keys.Enter && itemsComboBox.Focused && addItemButton.Enabled) AddItem();
        }

        private void useSkillComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.SkillData.UseSkill = ModCreator.GetEnumValue<SkillType>((string)useSkillComboBox.SelectedItem);
        }

        private void useReqNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.SkillData.UseReq = (int)useReqNumeric.Value;
        }

        private void craftSkillComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.SkillData.CraftSkill = ModCreator.GetEnumValue<SkillType>((string)craftSkillComboBox.SelectedItem);
        }

        private void craftReqNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.SkillData.CraftReq = (int)craftReqNumeric.Value;
        }

        private void blueprintCountNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (selectedBPBox == null)
            {
                int count = (int)blueprintCountNumeric.Value;
                Globals.selectedItem.BlueprintData.Count = count;
                if (count > 1)
                {
                    blueprintOutputCountLabel.Visible = true;
                    blueprintOutputCountLabel.Text = count.ToString();
                }
                else
                {
                    blueprintOutputCountLabel.Visible = false;
                }
            }
            else
            {
                selectedBPBox.SetCountLabel((int)blueprintCountNumeric.Value);
                Globals.selectedItem.BlueprintData.Materials[selectedBPBox.MaterialIndex].Count = (int)blueprintCountNumeric.Value;
            }
        }

        private void blueprintDurabilityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedBPBox == null) return;
            blueprintDurabilityNumeric.Enabled = blueprintDurabilityCheckBox.Checked;
            if (blueprintDurabilityCheckBox.Checked)
            {
                Globals.selectedItem.BlueprintData.Materials[selectedBPBox.MaterialIndex].Durability = (ushort)blueprintDurabilityNumeric.Value;
            }
            else
            {
                Globals.selectedItem.BlueprintData.Materials[selectedBPBox.MaterialIndex].Durability = 0;
            }
        }

        private void blueprintDurabilityNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (selectedBPBox == null) return;
            Globals.selectedItem.BlueprintData.Materials[selectedBPBox.MaterialIndex].Durability = (ushort)blueprintDurabilityNumeric.Value;
        }

        private void blueprintItemIDComboBox_TextChanged(object sender, EventArgs e)
        {
            if (selectedBPBox == null) return;
            Globals.selectedItem.BlueprintData.Materials[selectedBPBox.MaterialIndex].ItemID = blueprintItemIDComboBox.Text;
            if (Globals.items.TryGetValue(blueprintItemIDComboBox.Text, out Item? item))
            {
                selectedBPBox.SetTexture(item.TextureHD);
            }
            else if (blueprintItemIDComboBox.Text != null && !blueprintItemIDComboBox.Text.Equals(string.Empty) && !blueprintItemIDComboBox.Text.Equals(Globals.stringNone)) selectedBPBox.SetTexture(Globals.unknownTexture16);
            else selectedBPBox.SetTexture(null);
        }

        private void blueprintIsValidCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.BlueprintData.IsValid = blueprintIsValidCheckBox.Checked;
        }

        private void blueprintIsDefaultCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.BlueprintData.IsDefault = blueprintIsDefaultCheckBox.Checked;
            blueprintDepthLabel.Enabled = blueprintIsDefaultCheckBox.Checked;
            blueprintMinDepthNumeric.Enabled = blueprintIsDefaultCheckBox.Checked;
            blueprintMaxDepthNumeric.Enabled = blueprintIsDefaultCheckBox.Checked;
            if (blueprintIsDefaultCheckBox.Checked)
            {
                Globals.selectedItem.BlueprintData.Depth = new Vector2((float)blueprintMinDepthNumeric.Value / 100, (float)blueprintMaxDepthNumeric.Value / 100);
            }
            else
            {
                Globals.selectedItem.BlueprintData.Depth = new Vector2(0, 0);
            }
        }

        private void blueprintMinDepthNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.BlueprintData.Depth.X = (float)blueprintMinDepthNumeric.Value / 100;
        }

        private void blueprintMaxDepthNumeric_ValueChanged(object sender, EventArgs e)
        {
            Globals.selectedItem.BlueprintData.Depth.Y = (float)blueprintMaxDepthNumeric.Value / 100;
        }

        private void blueprintTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeselectBlueprintPictureBox();
            CraftingType type = ModCreator.GetEnumValue<CraftingType>((string)blueprintTypeComboBox.SelectedItem);
            Globals.selectedItem.BlueprintData.CraftType = type;
            int yLocation;
            if (type == CraftingType.Crafting)
            {
                yLocation = 217;
                foreach (BlueprintDataPictureBox bpBox in bpBoxes)
                {
                    if (bpBox.MaterialIndex > 5) bpBox.Location = new Point(11 + (bpBox.MaterialIndex % 3 * 40), yLocation);
                    bpBox.Visible = true;
                }
            }
            else
            {
                yLocation = 177;
                foreach (BlueprintDataPictureBox bpBox in bpBoxes)
                {
                    if (bpBox.MaterialIndex > 5) bpBox.Location = new Point(11 + (bpBox.MaterialIndex % 3 * 40), yLocation);
                    else bpBox.Visible = false;
                }
            }
        }

        private void BlueprintOutputPictureBoxClick(object? sender, EventArgs e)
        {
            DeselectBlueprintPictureBox();
            blueprintOutputPictureBox.Focus();
            blueprintOutputPictureBox.BackColor = SystemColors.Control;
            blueprintItemIDComboBox.Text = Globals.selectedItem.ItemData.ItemID;
            blueprintItemIDComboBox.Enabled = false;
            blueprintDurabilityCheckBox.Checked = false;
            blueprintDurabilityCheckBox.Enabled = false;
            blueprintDurabilityNumeric.Value = 0;
            blueprintDurabilityNumeric.Enabled = false;
            blueprintCountLabel.Enabled = true;
            blueprintCountNumeric.Enabled = true;
            blueprintCountNumeric.Value = Globals.selectedItem.BlueprintData.Count;

            string toolTip = "How many of this item is crafted.";
            blueprintDataToolTip.SetToolTip(blueprintCountLabel, toolTip);
            blueprintDataToolTip.SetToolTip(blueprintCountNumeric, toolTip);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Globals.modName != null)
            {
                DialogResult result = MessageBox.Show(this, "Closing the app will cause you to lose any unsaved progress. If you don't want to lose your progress and you haven't already, build your mod first. Click No to close without building.", "Build mod before closing?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                switch (result)
                {
                    case DialogResult.Yes:
                        {
                            DialogResult exportResult = ExportMod();
                            if (exportResult != DialogResult.OK) e.Cancel = true;
                            break;
                        }
                    case DialogResult.No:
                        {
                            break;
                        }
                    default:
                        {
                            e.Cancel = true;
                            break;
                        }
                }
            }
        }
    }
}
