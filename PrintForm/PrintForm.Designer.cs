using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace PrintForm
{
    partial class PrintForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbNumber = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.dtickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dtPickerBegin = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.gbSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "票据编号:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(348, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "姓名:";
            // 
            // tbNumber
            // 
            this.tbNumber.Location = new System.Drawing.Point(89, 17);
            this.tbNumber.Name = "tbNumber";
            this.tbNumber.Size = new System.Drawing.Size(210, 21);
            this.tbNumber.TabIndex = 1;
            this.tbNumber.Text = "2017010511231";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(389, 17);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(206, 21);
            this.tbName.TabIndex = 2;
            this.tbName.Text = "张三1";
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.dtickerEnd);
            this.gbSearch.Controls.Add(this.dtPickerBegin);
            this.gbSearch.Controls.Add(this.label4);
            this.gbSearch.Controls.Add(this.label3);
            this.gbSearch.Controls.Add(this.tbName);
            this.gbSearch.Controls.Add(this.tbNumber);
            this.gbSearch.Controls.Add(this.label2);
            this.gbSearch.Controls.Add(this.label1);
            this.gbSearch.Location = new System.Drawing.Point(13, 16);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(690, 91);
            this.gbSearch.TabIndex = 0;
            this.gbSearch.TabStop = false;
            this.gbSearch.Text = "查询条件";
            // 
            // dtickerEnd
            // 
            this.dtickerEnd.Location = new System.Drawing.Point(389, 51);
            this.dtickerEnd.Name = "dtickerEnd";
            this.dtickerEnd.Size = new System.Drawing.Size(206, 21);
            this.dtickerEnd.TabIndex = 6;
            // 
            // dtPickerBegin
            // 
            this.dtPickerBegin.Location = new System.Drawing.Point(90, 54);
            this.dtPickerBegin.Name = "dtPickerBegin";
            this.dtPickerBegin.Size = new System.Drawing.Size(209, 21);
            this.dtPickerBegin.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(324, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "结束日期;";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "开始日期:";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(709, 27);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "检索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gbSearch);
            this.splitContainer1.Panel1.Controls.Add(this.btnSearch);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvList);
            this.splitContainer1.Size = new System.Drawing.Size(884, 662);
            this.splitContainer1.SplitterDistance = 124;
            this.splitContainer1.TabIndex = 5;
            // 
            // dgvList
            // 
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.Location = new System.Drawing.Point(0, 0);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowTemplate.Height = 23;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(884, 534);
            this.dgvList.TabIndex = 5;
            this.dgvList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellDoubleClick);
            this.dgvList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvList_RowPostPaint);
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 662);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrintForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发票打印：";
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox tbNumber;
        private TextBox tbName;
        private GroupBox gbSearch;
        private Button btnSearch;
        private SplitContainer splitContainer1;
        private PrintDocument printDocument;
        private DateTimePicker dtickerEnd;
        private DateTimePicker dtPickerBegin;
        private Label label4;
        private Label label3;
        private DataGridView dgvList;
    }
}

