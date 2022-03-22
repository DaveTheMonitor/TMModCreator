namespace DaveTheMonitor.TMModCreator
{
    partial class NewModForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.newModButton = new System.Windows.Forms.Button();
            this.modNameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(140, 45);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(122, 29);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // newModButton
            // 
            this.newModButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.newModButton.Enabled = false;
            this.newModButton.Location = new System.Drawing.Point(12, 45);
            this.newModButton.Name = "newModButton";
            this.newModButton.Size = new System.Drawing.Size(122, 29);
            this.newModButton.TabIndex = 4;
            this.newModButton.Text = "New Mod";
            this.newModButton.UseVisualStyleBackColor = true;
            // 
            // modNameTextBox
            // 
            this.modNameTextBox.Location = new System.Drawing.Point(12, 12);
            this.modNameTextBox.Name = "modNameTextBox";
            this.modNameTextBox.PlaceholderText = "Mod Name";
            this.modNameTextBox.Size = new System.Drawing.Size(250, 27);
            this.modNameTextBox.TabIndex = 3;
            this.modNameTextBox.TextChanged += new System.EventHandler(this.modNameTextBox_TextChanged);
            // 
            // NewModForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 86);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.newModButton);
            this.Controls.Add(this.modNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "NewModForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Mod";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewModForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button cancelButton;
        private Button newModButton;
        internal TextBox modNameTextBox;
    }
}