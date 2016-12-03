namespace WenDuJianKong
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.总线ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.房间温度01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.房间状态01 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.房间温度02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.房间状态02 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.房间温度03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.房间状态03 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.房间温度04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.房间状态04 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.总线ID,
            this.房间温度01,
            this.房间状态01,
            this.房间温度02,
            this.房间状态02,
            this.房间温度03,
            this.房间状态03,
            this.房间温度04,
            this.房间状态04});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(500, 562);
            this.dataGridView1.TabIndex = 10;
            // 
            // 总线ID
            // 
            this.总线ID.HeaderText = "总线ID";
            this.总线ID.Name = "总线ID";
            this.总线ID.ReadOnly = true;
            // 
            // 房间温度01
            // 
            this.房间温度01.HeaderText = "房间温度01";
            this.房间温度01.Name = "房间温度01";
            this.房间温度01.ReadOnly = true;
            // 
            // 房间状态01
            // 
            this.房间状态01.HeaderText = "房间状态01";
            this.房间状态01.Name = "房间状态01";
            this.房间状态01.ReadOnly = true;
            this.房间状态01.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.房间状态01.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 房间温度02
            // 
            this.房间温度02.HeaderText = "房间温度02";
            this.房间温度02.Name = "房间温度02";
            this.房间温度02.ReadOnly = true;
            // 
            // 房间状态02
            // 
            this.房间状态02.HeaderText = "房间状态02";
            this.房间状态02.Name = "房间状态02";
            this.房间状态02.ReadOnly = true;
            this.房间状态02.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.房间状态02.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 房间温度03
            // 
            this.房间温度03.HeaderText = "房间温度03";
            this.房间温度03.Name = "房间温度03";
            this.房间温度03.ReadOnly = true;
            // 
            // 房间状态03
            // 
            this.房间状态03.HeaderText = "房间状态03";
            this.房间状态03.Name = "房间状态03";
            this.房间状态03.ReadOnly = true;
            this.房间状态03.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.房间状态03.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 房间温度04
            // 
            this.房间温度04.HeaderText = "房间温度04";
            this.房间温度04.Name = "房间温度04";
            this.房间温度04.ReadOnly = true;
            // 
            // 房间状态04
            // 
            this.房间状态04.HeaderText = "房间状态04";
            this.房间状态04.Name = "房间状态04";
            this.房间状态04.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(462, 562);
            this.panel1.TabIndex = 11;
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 总线ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 房间温度01;
        private System.Windows.Forms.DataGridViewComboBoxColumn 房间状态01;
        private System.Windows.Forms.DataGridViewTextBoxColumn 房间温度02;
        private System.Windows.Forms.DataGridViewComboBoxColumn 房间状态02;
        private System.Windows.Forms.DataGridViewTextBoxColumn 房间温度03;
        private System.Windows.Forms.DataGridViewComboBoxColumn 房间状态03;
        private System.Windows.Forms.DataGridViewTextBoxColumn 房间温度04;
        private System.Windows.Forms.DataGridViewComboBoxColumn 房间状态04;
        private System.Windows.Forms.Panel panel1;
    }
}