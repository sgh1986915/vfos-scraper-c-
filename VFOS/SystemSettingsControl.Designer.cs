namespace VFPO
{
    partial class SystemSettingsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPrintCopies = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxPrinterNames = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtXMLAPIURL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSecurityKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStoreID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxInterval = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxPrintTemplates = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnPreview = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtPrintCopies
            // 
            this.txtPrintCopies.Location = new System.Drawing.Point(228, 180);
            this.txtPrintCopies.Name = "txtPrintCopies";
            this.txtPrintCopies.Size = new System.Drawing.Size(52, 20);
            this.txtPrintCopies.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(158, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Print Copies:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxPrinterNames
            // 
            this.cbxPrinterNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPrinterNames.FormattingEnabled = true;
            this.cbxPrinterNames.Location = new System.Drawing.Point(228, 122);
            this.cbxPrinterNames.Name = "cbxPrinterNames";
            this.cbxPrinterNames.Size = new System.Drawing.Size(302, 21);
            this.cbxPrinterNames.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(184, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Printer:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(227, 246);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(93, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtXMLAPIURL
            // 
            this.txtXMLAPIURL.Location = new System.Drawing.Point(227, 95);
            this.txtXMLAPIURL.Name = "txtXMLAPIURL";
            this.txtXMLAPIURL.Size = new System.Drawing.Size(303, 20);
            this.txtXMLAPIURL.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(122, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "XML RPC API URL:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSecurityKey
            // 
            this.txtSecurityKey.Location = new System.Drawing.Point(227, 69);
            this.txtSecurityKey.Name = "txtSecurityKey";
            this.txtSecurityKey.Size = new System.Drawing.Size(303, 20);
            this.txtSecurityKey.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(126, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Store Security Key:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStoreID
            // 
            this.txtStoreID.Location = new System.Drawing.Point(227, 43);
            this.txtStoreID.Name = "txtStoreID";
            this.txtStoreID.Size = new System.Drawing.Size(303, 20);
            this.txtStoreID.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Store Id:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxInterval
            // 
            this.cbxInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxInterval.FormattingEnabled = true;
            this.cbxInterval.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cbxInterval.Location = new System.Drawing.Point(227, 149);
            this.cbxInterval.Name = "cbxInterval";
            this.cbxInterval.Size = new System.Drawing.Size(94, 21);
            this.cbxInterval.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Orders Download Interval:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(327, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "( in minutes )";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxPrintTemplates
            // 
            this.cbxPrintTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPrintTemplates.FormattingEnabled = true;
            this.cbxPrintTemplates.Items.AddRange(new object[] {
            "Classic Borderless",
            "Professional"});
            this.cbxPrintTemplates.Location = new System.Drawing.Point(227, 206);
            this.cbxPrintTemplates.Name = "cbxPrintTemplates";
            this.cbxPrintTemplates.Size = new System.Drawing.Size(158, 21);
            this.cbxPrintTemplates.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(146, 209);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Print Template:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPreview
            // 
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreview.Location = new System.Drawing.Point(391, 204);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(139, 23);
            this.btnPreview.TabIndex = 27;
            this.btnPreview.Text = "Preview Print Template";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(110, 295);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(321, 12);
            this.label9.TabIndex = 28;
            this.label9.Text = "1. Print template preview  is available only after saving configuration settings." +
    "";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(110, 317);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(510, 12);
            this.label10.TabIndex = 29;
            this.label10.Text = "2. Please provide valid application settings, otherwise system will fail to conne" +
    "ct to server and download orders information.";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(80, 283);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 12);
            this.label11.TabIndex = 30;
            this.label11.Text = "Note:-";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SystemSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.cbxPrintTemplates);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbxInterval);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPrintCopies);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbxPrinterNames);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtXMLAPIURL);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSecurityKey);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStoreID);
            this.Controls.Add(this.label2);
            this.Name = "SystemSettingsControl";
            this.Size = new System.Drawing.Size(672, 343);
            this.Load += new System.EventHandler(this.SystemSettingsControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPrintCopies;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbxPrinterNames;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtXMLAPIURL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSecurityKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStoreID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbxPrintTemplates;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
    }
}
