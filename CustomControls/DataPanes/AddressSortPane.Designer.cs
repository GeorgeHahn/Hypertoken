namespace Sniffer
{
    partial class AddressSortPane
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Checked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AddrColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeDeltaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RepeatsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Checked,
            this.AddrColumn,
            this.DataColumn,
            this.TimeDeltaColumn,
            this.RepeatsColumn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(371, 341);
            this.dataGridView1.TabIndex = 0;
            // 
            // Checked
            // 
            this.Checked.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Checked.HeaderText = "";
            this.Checked.Name = "Checked";
            this.Checked.Width = 5;
            // 
            // AddrColumn
            // 
            this.AddrColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AddrColumn.HeaderText = "Address";
            this.AddrColumn.Name = "AddrColumn";
            this.AddrColumn.ReadOnly = true;
            this.AddrColumn.ToolTipText = "Address of this device";
            this.AddrColumn.Width = 70;
            // 
            // DataColumn
            // 
            this.DataColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DataColumn.HeaderText = "Data";
            this.DataColumn.Name = "DataColumn";
            this.DataColumn.ReadOnly = true;
            this.DataColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DataColumn.ToolTipText = "Data contained in last message";
            // 
            // TimeDeltaColumn
            // 
            this.TimeDeltaColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TimeDeltaColumn.HeaderText = "T (ms)";
            this.TimeDeltaColumn.Name = "TimeDeltaColumn";
            this.TimeDeltaColumn.ReadOnly = true;
            this.TimeDeltaColumn.ToolTipText = "Milliseconds between last two messages";
            this.TimeDeltaColumn.Width = 61;
            // 
            // RepeatsColumn
            // 
            this.RepeatsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.RepeatsColumn.HeaderText = "R";
            this.RepeatsColumn.Name = "RepeatsColumn";
            this.RepeatsColumn.ReadOnly = true;
            this.RepeatsColumn.ToolTipText = "Quantity of messages for this address";
            this.RepeatsColumn.Width = 40;
            // 
            // AddressSortPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "AddressSortPane";
            this.Size = new System.Drawing.Size(371, 341);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Checked;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddrColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeDeltaColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RepeatsColumn;

    }
}
