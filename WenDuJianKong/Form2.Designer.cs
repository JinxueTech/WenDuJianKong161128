﻿namespace WenDuJianKong
{
    partial class Form2
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.wangKongIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.floorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.temperature1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.temperature2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.temperature3DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state3DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.temperature4DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state4DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tempStateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.wenDuJianKongDataSet = new WenDuJianKong.WenDuJianKongDataSet();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tempStateTableAdapter = new WenDuJianKong.WenDuJianKongDataSetTableAdapters.TempStateTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tempStateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wenDuJianKongDataSet)).BeginInit();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.wangKongIDDataGridViewTextBoxColumn,
            this.floorDataGridViewTextBoxColumn,
            this.temperature1DataGridViewTextBoxColumn,
            this.state1DataGridViewTextBoxColumn,
            this.temperature2DataGridViewTextBoxColumn,
            this.state2DataGridViewTextBoxColumn,
            this.temperature3DataGridViewTextBoxColumn,
            this.state3DataGridViewTextBoxColumn,
            this.temperature4DataGridViewTextBoxColumn,
            this.state4DataGridViewTextBoxColumn,
            this.timeDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.tempStateBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(884, 562);
            this.dataGridView1.TabIndex = 10;
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // wangKongIDDataGridViewTextBoxColumn
            // 
            this.wangKongIDDataGridViewTextBoxColumn.DataPropertyName = "WangKongID";
            this.wangKongIDDataGridViewTextBoxColumn.HeaderText = "网络控制器ID";
            this.wangKongIDDataGridViewTextBoxColumn.Name = "wangKongIDDataGridViewTextBoxColumn";
            this.wangKongIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // floorDataGridViewTextBoxColumn
            // 
            this.floorDataGridViewTextBoxColumn.DataPropertyName = "floor";
            this.floorDataGridViewTextBoxColumn.HeaderText = "楼层";
            this.floorDataGridViewTextBoxColumn.Name = "floorDataGridViewTextBoxColumn";
            this.floorDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // temperature1DataGridViewTextBoxColumn
            // 
            this.temperature1DataGridViewTextBoxColumn.DataPropertyName = "Temperature1";
            this.temperature1DataGridViewTextBoxColumn.HeaderText = "01室温度";
            this.temperature1DataGridViewTextBoxColumn.Name = "temperature1DataGridViewTextBoxColumn";
            this.temperature1DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // state1DataGridViewTextBoxColumn
            // 
            this.state1DataGridViewTextBoxColumn.DataPropertyName = "State1";
            this.state1DataGridViewTextBoxColumn.HeaderText = "01室状态";
            this.state1DataGridViewTextBoxColumn.Name = "state1DataGridViewTextBoxColumn";
            this.state1DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // temperature2DataGridViewTextBoxColumn
            // 
            this.temperature2DataGridViewTextBoxColumn.DataPropertyName = "Temperature2";
            this.temperature2DataGridViewTextBoxColumn.HeaderText = "02室温度";
            this.temperature2DataGridViewTextBoxColumn.Name = "temperature2DataGridViewTextBoxColumn";
            this.temperature2DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // state2DataGridViewTextBoxColumn
            // 
            this.state2DataGridViewTextBoxColumn.DataPropertyName = "State2";
            this.state2DataGridViewTextBoxColumn.HeaderText = "02室状态";
            this.state2DataGridViewTextBoxColumn.Name = "state2DataGridViewTextBoxColumn";
            this.state2DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // temperature3DataGridViewTextBoxColumn
            // 
            this.temperature3DataGridViewTextBoxColumn.DataPropertyName = "Temperature3";
            this.temperature3DataGridViewTextBoxColumn.HeaderText = "03室温度";
            this.temperature3DataGridViewTextBoxColumn.Name = "temperature3DataGridViewTextBoxColumn";
            this.temperature3DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // state3DataGridViewTextBoxColumn
            // 
            this.state3DataGridViewTextBoxColumn.DataPropertyName = "State3";
            this.state3DataGridViewTextBoxColumn.HeaderText = "03室状态";
            this.state3DataGridViewTextBoxColumn.Name = "state3DataGridViewTextBoxColumn";
            this.state3DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // temperature4DataGridViewTextBoxColumn
            // 
            this.temperature4DataGridViewTextBoxColumn.DataPropertyName = "Temperature4";
            this.temperature4DataGridViewTextBoxColumn.HeaderText = "04室温度";
            this.temperature4DataGridViewTextBoxColumn.Name = "temperature4DataGridViewTextBoxColumn";
            this.temperature4DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // state4DataGridViewTextBoxColumn
            // 
            this.state4DataGridViewTextBoxColumn.DataPropertyName = "State4";
            this.state4DataGridViewTextBoxColumn.HeaderText = "04室状态";
            this.state4DataGridViewTextBoxColumn.Name = "state4DataGridViewTextBoxColumn";
            this.state4DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // timeDataGridViewTextBoxColumn
            // 
            this.timeDataGridViewTextBoxColumn.DataPropertyName = "Time";
            this.timeDataGridViewTextBoxColumn.HeaderText = "最近更新时间";
            this.timeDataGridViewTextBoxColumn.Name = "timeDataGridViewTextBoxColumn";
            this.timeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tempStateBindingSource
            // 
            this.tempStateBindingSource.DataMember = "TempState";
            this.tempStateBindingSource.DataSource = this.wenDuJianKongDataSet;
            // 
            // wenDuJianKongDataSet
            // 
            this.wenDuJianKongDataSet.DataSetName = "WenDuJianKongDataSet";
            this.wenDuJianKongDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 562);
            this.panel1.TabIndex = 11;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(884, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel1.Text = "系统提示：";
            // 
            // tempStateTableAdapter
            // 
            this.tempStateTableAdapter.ClearBeforeFill = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.panel1);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "北区五号楼平面分布图";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tempStateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wenDuJianKongDataSet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private WenDuJianKongDataSet wenDuJianKongDataSet;
        private System.Windows.Forms.BindingSource tempStateBindingSource;
        private WenDuJianKongDataSetTableAdapters.TempStateTableAdapter tempStateTableAdapter;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn wangKongIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn floorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn temperature1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn state1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn temperature2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn state2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn temperature3DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn state3DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn temperature4DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn state4DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeDataGridViewTextBoxColumn;
    }
}