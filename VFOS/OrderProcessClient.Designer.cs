namespace VFPO
{
    partial class OrderProcessClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderProcessClient));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tbOrdersList = new System.Windows.Forms.TabControl();
            this.tabTodaysOrders = new System.Windows.Forms.TabPage();
            this.pnlAdminCtrls = new System.Windows.Forms.Panel();
            this.cbxOrderStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStoreName = new System.Windows.Forms.Label();
            this.cbxStoreNames = new System.Windows.Forms.ComboBox();
            this.gvOrdersList = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlUserModes = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxStoreStatus = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbxUserModes = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.helpTips1 = new VFPO.HelpTips();
            this.tbOrdersList.SuspendLayout();
            this.tabTodaysOrders.SuspendLayout();
            this.pnlAdminCtrls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrdersList)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.pnlUserModes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Take Away Order Processing App";
            this.notifyIcon1.BalloonTipTitle = "Take Away Order Processing App";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Take Away Order Processing App";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // tbOrdersList
            // 
            this.tbOrdersList.Controls.Add(this.tabTodaysOrders);
            this.tbOrdersList.Location = new System.Drawing.Point(7, 135);
            this.tbOrdersList.Name = "tbOrdersList";
            this.tbOrdersList.SelectedIndex = 0;
            this.tbOrdersList.Size = new System.Drawing.Size(1037, 450);
            this.tbOrdersList.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tbOrdersList.TabIndex = 3;
            // 
            // tabTodaysOrders
            // 
            this.tabTodaysOrders.CausesValidation = false;
            this.tabTodaysOrders.Controls.Add(this.pnlAdminCtrls);
            this.tabTodaysOrders.Controls.Add(this.gvOrdersList);
            this.tabTodaysOrders.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTodaysOrders.Location = new System.Drawing.Point(4, 22);
            this.tabTodaysOrders.Name = "tabTodaysOrders";
            this.tabTodaysOrders.Padding = new System.Windows.Forms.Padding(3);
            this.tabTodaysOrders.Size = new System.Drawing.Size(1029, 424);
            this.tabTodaysOrders.TabIndex = 0;
            this.tabTodaysOrders.Text = "Orders";
            this.tabTodaysOrders.UseVisualStyleBackColor = true;
            // 
            // pnlAdminCtrls
            // 
            this.pnlAdminCtrls.Controls.Add(this.cbxOrderStatus);
            this.pnlAdminCtrls.Controls.Add(this.label1);
            this.pnlAdminCtrls.Controls.Add(this.lblStoreName);
            this.pnlAdminCtrls.Controls.Add(this.cbxStoreNames);
            this.pnlAdminCtrls.Location = new System.Drawing.Point(442, 12);
            this.pnlAdminCtrls.Name = "pnlAdminCtrls";
            this.pnlAdminCtrls.Size = new System.Drawing.Size(574, 28);
            this.pnlAdminCtrls.TabIndex = 11;
            // 
            // cbxOrderStatus
            // 
            this.cbxOrderStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOrderStatus.FormattingEnabled = true;
            this.cbxOrderStatus.Location = new System.Drawing.Point(377, 3);
            this.cbxOrderStatus.Name = "cbxOrderStatus";
            this.cbxOrderStatus.Size = new System.Drawing.Size(192, 23);
            this.cbxOrderStatus.TabIndex = 4;
            this.cbxOrderStatus.SelectedIndexChanged += new System.EventHandler(this.cbxOrderStatus_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(288, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Order Status:";
            // 
            // lblStoreName
            // 
            this.lblStoreName.AutoSize = true;
            this.lblStoreName.Location = new System.Drawing.Point(43, 7);
            this.lblStoreName.Name = "lblStoreName";
            this.lblStoreName.Size = new System.Drawing.Size(42, 17);
            this.lblStoreName.TabIndex = 9;
            this.lblStoreName.Text = "Store:";
            // 
            // cbxStoreNames
            // 
            this.cbxStoreNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStoreNames.FormattingEnabled = true;
            this.cbxStoreNames.Location = new System.Drawing.Point(87, 4);
            this.cbxStoreNames.Name = "cbxStoreNames";
            this.cbxStoreNames.Size = new System.Drawing.Size(192, 23);
            this.cbxStoreNames.TabIndex = 3;
            this.cbxStoreNames.SelectedIndexChanged += new System.EventHandler(this.cbxStoreNames_SelectedIndexChanged);
            // 
            // gvOrdersList
            // 
            this.gvOrdersList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvOrdersList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gvOrdersList.Location = new System.Drawing.Point(6, 46);
            this.gvOrdersList.MultiSelect = false;
            this.gvOrdersList.Name = "gvOrdersList";
            this.gvOrdersList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gvOrdersList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvOrdersList.Size = new System.Drawing.Size(1010, 372);
            this.gvOrdersList.TabIndex = 5;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 585);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1363, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // pnlUserModes
            // 
            this.pnlUserModes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlUserModes.Controls.Add(this.label2);
            this.pnlUserModes.Controls.Add(this.cbxStoreStatus);
            this.pnlUserModes.Controls.Add(this.label9);
            this.pnlUserModes.Controls.Add(this.cbxUserModes);
            this.pnlUserModes.Location = new System.Drawing.Point(388, 100);
            this.pnlUserModes.Name = "pnlUserModes";
            this.pnlUserModes.Size = new System.Drawing.Size(706, 28);
            this.pnlUserModes.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(374, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Set Store Status To:";
            // 
            // cbxStoreStatus
            // 
            this.cbxStoreStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStoreStatus.FormattingEnabled = true;
            this.cbxStoreStatus.Items.AddRange(new object[] {
            "Online",
            "Offline",
            "Busy",
            "Idle"});
            this.cbxStoreStatus.Location = new System.Drawing.Point(480, 3);
            this.cbxStoreStatus.Name = "cbxStoreStatus";
            this.cbxStoreStatus.Size = new System.Drawing.Size(169, 21);
            this.cbxStoreStatus.TabIndex = 2;
            this.cbxStoreStatus.SelectedIndexChanged += new System.EventHandler(this.cbxStoreStatus_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(129, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Login as:";
            // 
            // cbxUserModes
            // 
            this.cbxUserModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUserModes.FormattingEnabled = true;
            this.cbxUserModes.Items.AddRange(new object[] {
            "POS Client",
            "Administrator"});
            this.cbxUserModes.Location = new System.Drawing.Point(182, 3);
            this.cbxUserModes.Name = "cbxUserModes";
            this.cbxUserModes.Size = new System.Drawing.Size(169, 21);
            this.cbxUserModes.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::VFPO.Properties.Resources.VFOS_logo;
            this.pictureBox1.Location = new System.Drawing.Point(11, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(331, 69);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 100;
            this.pictureBox1.TabStop = false;
            // 
            // helpTips1
            // 
            this.helpTips1.Location = new System.Drawing.Point(1046, 145);
            this.helpTips1.Name = "helpTips1";
            this.helpTips1.Size = new System.Drawing.Size(266, 440);
            this.helpTips1.TabIndex = 99;
            // 
            // OrderProcessClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1380, 475);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.helpTips1);
            this.Controls.Add(this.pnlUserModes);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tbOrdersList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OrderProcessClient";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "  VenturesSky Food Ordering System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.OrderProcess_Load);
            this.tbOrdersList.ResumeLayout(false);
            this.tabTodaysOrders.ResumeLayout(false);
            this.pnlAdminCtrls.ResumeLayout(false);
            this.pnlAdminCtrls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrdersList)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlUserModes.ResumeLayout(false);
            this.pnlUserModes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TabControl tbOrdersList;
        private System.Windows.Forms.TabPage tabTodaysOrders;
        private System.Windows.Forms.DataGridView gvOrdersList;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Panel pnlUserModes;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbxUserModes;
        private System.Windows.Forms.Panel pnlAdminCtrls;
        private System.Windows.Forms.ComboBox cbxOrderStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStoreName;
        private System.Windows.Forms.ComboBox cbxStoreNames;
        private HelpTips helpTips1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxStoreStatus;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

