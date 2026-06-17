namespace PersonalAccountingSystem
{
    partial class PersonalAccountingSystem
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gpbEdit = new System.Windows.Forms.GroupBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblTotalExpense = new System.Windows.Forms.Label();
            this.lblTotalIncome = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.rbIncome = new System.Windows.Forms.RadioButton();
            this.rbExpense = new System.Windows.Forms.RadioButton();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbFilterType = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.lblSearch = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.gpbEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvRecords
            // 
            this.dgvRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRecords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dgvRecords.Location = new System.Drawing.Point(349, 108);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.RowHeadersWidth = 51;
            this.dgvRecords.RowTemplate.Height = 27;
            this.dgvRecords.Size = new System.Drawing.Size(679, 515);
            this.dgvRecords.TabIndex = 0;
            this.dgvRecords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecords_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "日期";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "收支";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "分類";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "金額";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "備註";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            // 
            // gpbEdit
            // 
            this.gpbEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gpbEdit.Controls.Add(this.lblBalance);
            this.gpbEdit.Controls.Add(this.lblTotalExpense);
            this.gpbEdit.Controls.Add(this.lblTotalIncome);
            this.gpbEdit.Controls.Add(this.btnSave);
            this.gpbEdit.Controls.Add(this.btnDelete);
            this.gpbEdit.Controls.Add(this.btnInsert);
            this.gpbEdit.Controls.Add(this.lblDescription);
            this.gpbEdit.Controls.Add(this.lblAmount);
            this.gpbEdit.Controls.Add(this.txtDescription);
            this.gpbEdit.Controls.Add(this.txtAmount);
            this.gpbEdit.Controls.Add(this.cmbCategory);
            this.gpbEdit.Controls.Add(this.rbIncome);
            this.gpbEdit.Controls.Add(this.rbExpense);
            this.gpbEdit.Controls.Add(this.dtpDate);
            this.gpbEdit.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.gpbEdit.Location = new System.Drawing.Point(29, 39);
            this.gpbEdit.Name = "gpbEdit";
            this.gpbEdit.Size = new System.Drawing.Size(295, 584);
            this.gpbEdit.TabIndex = 1;
            this.gpbEdit.TabStop = false;
            this.gpbEdit.Text = "編輯";
            // 
            // lblBalance
            // 
            this.lblBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(22, 531);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(149, 25);
            this.lblBalance.TabIndex = 13;
            this.lblBalance.Text = "目前餘額：0 元";
            // 
            // lblTotalExpense
            // 
            this.lblTotalExpense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotalExpense.AutoSize = true;
            this.lblTotalExpense.Location = new System.Drawing.Point(22, 487);
            this.lblTotalExpense.Name = "lblTotalExpense";
            this.lblTotalExpense.Size = new System.Drawing.Size(129, 25);
            this.lblTotalExpense.TabIndex = 12;
            this.lblTotalExpense.Text = "總支出：0 元";
            // 
            // lblTotalIncome
            // 
            this.lblTotalIncome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotalIncome.AutoSize = true;
            this.lblTotalIncome.Location = new System.Drawing.Point(22, 443);
            this.lblTotalIncome.Name = "lblTotalIncome";
            this.lblTotalIncome.Size = new System.Drawing.Size(129, 25);
            this.lblTotalIncome.TabIndex = 11;
            this.lblTotalIncome.Text = "總收入：0 元";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(22, 377);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(114, 36);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "儲存變更";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(157, 335);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(114, 36);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "刪除紀錄";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(22, 335);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(114, 36);
            this.btnInsert.TabIndex = 8;
            this.btnInsert.Text = "新增紀錄";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(22, 275);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(52, 25);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "備註";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(22, 230);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(52, 25);
            this.lblAmount.TabIndex = 6;
            this.lblAmount.Text = "金額";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(81, 270);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(190, 34);
            this.txtDescription.TabIndex = 5;
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(81, 225);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(190, 34);
            this.txtAmount.TabIndex = 4;
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "食",
            "衣",
            "住",
            "行",
            "育",
            "樂",
            "其他"});
            this.cmbCategory.Location = new System.Drawing.Point(22, 158);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(248, 33);
            this.cmbCategory.TabIndex = 3;
            this.cmbCategory.Text = "分類";
            // 
            // rbIncome
            // 
            this.rbIncome.AutoSize = true;
            this.rbIncome.Location = new System.Drawing.Point(182, 106);
            this.rbIncome.Name = "rbIncome";
            this.rbIncome.Size = new System.Drawing.Size(73, 29);
            this.rbIncome.TabIndex = 2;
            this.rbIncome.Text = "收入";
            this.rbIncome.UseVisualStyleBackColor = true;
            // 
            // rbExpense
            // 
            this.rbExpense.AutoSize = true;
            this.rbExpense.Checked = true;
            this.rbExpense.Location = new System.Drawing.Point(37, 106);
            this.rbExpense.Name = "rbExpense";
            this.rbExpense.Size = new System.Drawing.Size(73, 29);
            this.rbExpense.TabIndex = 1;
            this.rbExpense.TabStop = true;
            this.rbExpense.Text = "支出";
            this.rbExpense.UseVisualStyleBackColor = true;
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(22, 46);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(248, 34);
            this.dtpDate.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtSearch.Location = new System.Drawing.Point(508, 49);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(229, 34);
            this.txtSearch.TabIndex = 2;
            // 
            // cmbFilterType
            // 
            this.cmbFilterType.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cmbFilterType.FormattingEnabled = true;
            this.cmbFilterType.Items.AddRange(new object[] {
            "全部",
            "收入",
            "支出"});
            this.cmbFilterType.Location = new System.Drawing.Point(764, 50);
            this.cmbFilterType.Name = "cmbFilterType";
            this.cmbFilterType.Size = new System.Drawing.Size(105, 33);
            this.cmbFilterType.TabIndex = 3;
            this.cmbFilterType.Text = "收／支";
            // 
            // btnFilter
            // 
            this.btnFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnFilter.Location = new System.Drawing.Point(875, 49);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(132, 34);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "開始篩選";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblSearch.Location = new System.Drawing.Point(370, 54);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(132, 25);
            this.lblSearch.TabIndex = 5;
            this.lblSearch.Text = "輸入關鍵字：";
            // 
            // PersonalAccountingSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 664);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.cmbFilterType);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.gpbEdit);
            this.Controls.Add(this.dgvRecords);
            this.MinimumSize = new System.Drawing.Size(1067, 711);
            this.Name = "PersonalAccountingSystem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "個人記帳系統";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PersonalAccountingSystem_FormClosing);
            this.Load += new System.EventHandler(this.PersonalAccountingSystem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.gpbEdit.ResumeLayout(false);
            this.gpbEdit.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRecords;
        private System.Windows.Forms.GroupBox gpbEdit;
        private System.Windows.Forms.RadioButton rbIncome;
        private System.Windows.Forms.RadioButton rbExpense;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblTotalExpense;
        private System.Windows.Forms.Label lblTotalIncome;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbFilterType;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Label lblSearch;
    }
}

