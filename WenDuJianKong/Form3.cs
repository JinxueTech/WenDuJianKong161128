﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WenDuJianKong
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“tempstate3DataSet.TempState3”中。您可以根据需要移动或删除它。
            this.tempState3TableAdapter.Fill(this.tempstate3DataSet.TempState3);

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3 || e.ColumnIndex == 5 || e.ColumnIndex == 7 || e.ColumnIndex == 9)
                {
                    if (e.Value == System.DBNull.Value)
                    {
                        return;
                    }
                    else
                    {
                        int val = Convert.ToInt32(e.Value);
                        switch (val)
                        {
                            case 0:
                                e.Value = "防冻";
                                break;
                            case 1:
                                e.Value = "无人";
                                break;
                            case 2:
                                e.Value = "有人";
                                break;
                            default:
                                break;
                        }
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                e.FormattingApplied = false;
                toolStripStatusLabel1.Text = "系统提示：" + ex.ToString();
            }
        }
    }
}
