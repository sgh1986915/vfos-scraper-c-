namespace OrderProcessor
{
    partial class OrdersList
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
            this.gvOrdersList = new System.Windows.Forms.DataGridView();
            this.testDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.testData = new OrderProcessor.TestData();
            this.ordersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ordersTableAdapter = new OrderProcessor.TestDataTableAdapters.OrdersTableAdapter();
            this.PrintOrder = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrdersList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ordersBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gvOrdersList
            // 
            this.gvOrdersList.AllowUserToAddRows = false;
            this.gvOrdersList.AllowUserToDeleteRows = false;
            this.gvOrdersList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvOrdersList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PrintOrder});
            this.gvOrdersList.Location = new System.Drawing.Point(12, 26);
            this.gvOrdersList.Name = "gvOrdersList";
            this.gvOrdersList.Size = new System.Drawing.Size(774, 238);
            this.gvOrdersList.TabIndex = 0;
            this.gvOrdersList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvOrdersList_CellContentClick);
            // 
            // testDataBindingSource
            // 
            this.testDataBindingSource.DataSource = this.testData;
            this.testDataBindingSource.Position = 0;
            // 
            // testData
            // 
            this.testData.DataSetName = "TestData";
            this.testData.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ordersBindingSource
            // 
            this.ordersBindingSource.DataMember = "Orders";
            this.ordersBindingSource.DataSource = this.testDataBindingSource;
            // 
            // ordersTableAdapter
            // 
            this.ordersTableAdapter.ClearBeforeFill = true;
            // 
            // PrintOrder
            // 
            this.PrintOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PrintOrder.HeaderText = "Print";
            this.PrintOrder.Name = "PrintOrder";
            this.PrintOrder.ReadOnly = true;
            this.PrintOrder.Text = "Print";
            this.PrintOrder.ToolTipText = "Reprint Order";
            this.PrintOrder.UseColumnTextForButtonValue = true;
            // 
            // OrdersList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(798, 438);
            this.ControlBox = false;
            this.Controls.Add(this.gvOrdersList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrdersList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OrdersList";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OrdersList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvOrdersList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ordersBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gvOrdersList;
        private System.Windows.Forms.BindingSource testDataBindingSource;
        private TestData testData;
        private System.Windows.Forms.BindingSource ordersBindingSource;
        private TestDataTableAdapters.OrdersTableAdapter ordersTableAdapter;
        private System.Windows.Forms.DataGridViewButtonColumn PrintOrder;
    }
}