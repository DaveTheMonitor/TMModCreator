namespace DaveTheMonitor.TMModCreator
{
    partial class ExportModForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportModForm));
            this.modNameTextBox = new System.Windows.Forms.TextBox();
            this.exportModButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.newFolderCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // modNameTextBox
            // 
            this.modNameTextBox.Enabled = false;
            this.modNameTextBox.Location = new System.Drawing.Point(12, 42);
            this.modNameTextBox.Name = "modNameTextBox";
            this.modNameTextBox.PlaceholderText = "Mod Name";
            this.modNameTextBox.Size = new System.Drawing.Size(250, 27);
            this.modNameTextBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.modNameTextBox, "Mod Name");
            this.modNameTextBox.TextChanged += new System.EventHandler(this.modNameTextBox_TextChanged);
            // 
            // exportModButton
            // 
            this.exportModButton.Location = new System.Drawing.Point(12, 75);
            this.exportModButton.Name = "exportModButton";
            this.exportModButton.Size = new System.Drawing.Size(122, 29);
            this.exportModButton.TabIndex = 1;
            this.exportModButton.Text = "Build Mod";
            this.toolTip.SetToolTip(this.exportModButton, "Build the mod to the selected folder.\r\nBe warned if a mod already exists in the t" +
        "arget folder, it\'ll be overwritten.");
            this.exportModButton.UseVisualStyleBackColor = true;
            this.exportModButton.Click += new System.EventHandler(this.exportModButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(140, 75);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(122, 29);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // newFolderCheckBox
            // 
            this.newFolderCheckBox.AutoSize = true;
            this.newFolderCheckBox.Location = new System.Drawing.Point(12, 12);
            this.newFolderCheckBox.Name = "newFolderCheckBox";
            this.newFolderCheckBox.Size = new System.Drawing.Size(154, 24);
            this.newFolderCheckBox.TabIndex = 3;
            this.newFolderCheckBox.Text = "Create New Folder";
            this.toolTip.SetToolTip(this.newFolderCheckBox, resources.GetString("newFolderCheckBox.ToolTip"));
            this.newFolderCheckBox.UseVisualStyleBackColor = true;
            this.newFolderCheckBox.CheckedChanged += new System.EventHandler(this.newFolderCheckBox_CheckedChanged);
            // 
            // ExportModForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 116);
            this.ControlBox = false;
            this.Controls.Add(this.newFolderCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.exportModButton);
            this.Controls.Add(this.modNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "ExportModForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Build Mod";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExportModForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal TextBox modNameTextBox;
        private Button exportModButton;
        private Button cancelButton;
        private ToolTip toolTip;
        private CheckBox newFolderCheckBox;
    }
}