namespace VFPO
{
    partial class ConfigWizard
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
            this.label1 = new System.Windows.Forms.Label();
            this.systemSettingsControl1 = new VFPO.SystemSettingsControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(167, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(407, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Configure VFOS Application Settings";
            // 
            // systemSettingsControl1
            // 
            this.systemSettingsControl1.BackColor = System.Drawing.SystemColors.Window;
            this.systemSettingsControl1.Location = new System.Drawing.Point(28, 58);
            this.systemSettingsControl1.Name = "systemSettingsControl1";
            this.systemSettingsControl1.Size = new System.Drawing.Size(672, 338);
            this.systemSettingsControl1.TabIndex = 0;
            // 
            // ConfigWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(735, 424);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.systemSettingsControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConfigWizard";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " VFOS Configuration Wizard";
            this.Load += new System.EventHandler(this.ConfigWizard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SystemSettingsControl systemSettingsControl1;
        private System.Windows.Forms.Label label1;
    }
}