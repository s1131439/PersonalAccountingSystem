using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonalAccountingSystem
{
    /// <summary>
    /// 核心業務邏輯：個人記帳系統主表單
    /// </summary>
    public partial class PersonalAccountingSystem : Form
    {
        #region --- 全域變數與參數宣告 ---

        // 記憶體資料庫：建立一個強型別清單，用來即時存放動態的記帳資料
        private List<Transaction> records = new List<Transaction>();

        // 實體檔案資料庫：定義 JSON 存檔的標準相對路徑與檔名
        private string filePath = "accounting_data.json";

        // 狀態控管鎖 A：記錄當前是否處於「修改模式」。-1 代表全新新增，0 以上代表正在編輯的陣列索引
        private int editingIndex = -1;

        // 狀態控管鎖 B：變更警報器。記錄是否有尚未儲存至硬碟的實體資料變更
        private bool isUnsaved = false;

        #endregion

        #region --- 建構式與系統初始化 ---

        /// <summary>
        /// 表單建構式
        /// </summary>
        public PersonalAccountingSystem()
        {
            // 載入由設計視窗自動生成的 UI 元件配置結構
            InitializeComponent();
        }

        /// <summary>
        /// 事件：視窗載入完畢 (Form_Load)
        /// </summary>
        private void PersonalAccountingSystem_Load(object sender, EventArgs e)
        {
            // 檢查硬碟中是否存在上一次的歷史記帳 JSON 檔案
            if (File.Exists(filePath))
            {
                try
                {
                    // 1. 從硬碟讀取完整的 JSON 結構字串
                    string jsonString = File.ReadAllText(filePath);

                    // 2. 利用 JSON 反序列化技術，將字串還原為 C# 物件清單
                    records = JsonSerializer.Deserialize<List<Transaction>>(jsonString);

                    // 3. 安全防禦：若檔案為空導致反序列化失敗，重新初始化清單防止 NullReference 崩潰
                    if (records == null)
                    {
                        records = new List<Transaction>();
                    }

                    // 4. 讀檔完畢後，即時呼叫核心繪製方法，將歷史帳目渲染至介面表格
                    UpdateGridAndLabels();
                }
                catch (Exception ex)
                {
                    // 核心例外處理：若檔案損毀或格式不符，彈出 Error 視窗阻斷程式崩潰
                    MessageBox.Show($"讀取舊資料失敗，檔案可能損毀。\n錯誤原因：{ex.Message}", "讀檔錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // 5. 狀態連動初始化：強制呼叫清空方法，確保所有輸入框與 btnClear 按鈕的狀態同步歸零
            ClearInputs();
        }

        #endregion

        #region --- 核心業務：新增、修改與刪除事件 ---

        /// <summary>
        /// 事件：點擊「新增紀錄 / 確認修改」按鈕
        /// </summary>
        private void btnInsert_Click(object sender, EventArgs e)
        {
            // 攔截無效的下拉選單：確保使用者必須點選消費分類
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("請選擇消費分類！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 攔截非法金額輸入：確保不可為空、不可為非數字文字、不可低於或等於 0 元
            if (string.IsNullOrWhiteSpace(txtAmount.Text) || !int.TryParse(txtAmount.Text, out int amount) || amount <= 0)
            {
                MessageBox.Show("請輸入正確的大於 0 的金額數字！", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 計算收支狀態：利用三元運算子快速判斷當前的 RadioButton 勾選狀態
            string type = rbExpense.Checked ? "支出" : "收入";

            // 路由判斷：依據狀態鎖 editingIndex 分流處理「全新新增」或「舊案修改」
            if (editingIndex == -1)
            {
                // 模式 A：全新新增模式
                Transaction newRecord = new Transaction
                {
                    Date = dtpDate.Value,
                    Type = type,
                    Category = cmbCategory.SelectedItem.ToString(),
                    Amount = amount,
                    Description = txtDescription.Text
                };

                // 將新物件壓入後台全域 List 清單中
                records.Add(newRecord);

                // 拉響變更警報：標記系統目前有未存檔的變更
                isUnsaved = true;
            }
            else
            {
                // 模式 B：修改舊資料模式 (精準覆蓋 List 記憶體中該指定索引的欄位值)
                records[editingIndex].Date = dtpDate.Value;
                records[editingIndex].Type = type;
                records[editingIndex].Category = cmbCategory.SelectedItem.ToString();
                records[editingIndex].Amount = amount;
                records[editingIndex].Description = txtDescription.Text;

                // 彈出資訊提示視窗告知使用者修改成功
                MessageBox.Show("資料修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // 後置作業：同步重繪前端表格，並呼叫清空方法將狀態解鎖
            UpdateGridAndLabels();
            ClearInputs();
        }

        /// <summary>
        /// 事件：點擊「刪除紀錄」按鈕
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 檢查使用者是否確實選取了 DataGridView 中的任意資料列
            if (dgvRecords.CurrentRow == null || dgvRecords.CurrentRow.Index < 0)
            {
                MessageBox.Show("請先點擊右邊表格，選取一筆你想刪除的紀錄！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 抓取當前反白列的物理索引編號
            int selectedIndex = dgvRecords.CurrentRow.Index;

            // 邊界防禦：防止使用者選取到表格最下方的未啟用空白預留列
            if (selectedIndex >= records.Count) return;

            // 彈出詢問視窗，避免誤刪重要帳目
            DialogResult result = MessageBox.Show("確定要刪除這筆記帳紀錄嗎？", "確認刪除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                // 1. 自後台 List 資料庫中將指定索引列永久移除
                records.RemoveAt(selectedIndex);

                // 2. 標記資料變更，啟動未儲存警報
                isUnsaved = true;

                // 3. 立即重繪前端 DataGridView 並更新動態看板金額
                UpdateGridAndLabels();
            }
        }

        #endregion

        #region --- 資料檢索、過濾與持久化 (JSON 讀寫) ---

        /// <summary>
        /// 事件：點擊「儲存變更」按鈕
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 配置進階 JSON 引進參數：啟用縮排縮進（WriteIndented），確保儲存出來的文字檔具備人類高可讀性
                var options = new JsonSerializerOptions { WriteIndented = true };

                // 2. 核心序列化技術：將 List 陣列物件高壓轉換成標準格式的 JSON 字串
                string jsonString = JsonSerializer.Serialize(records, options);

                // 3. I/O 串流寫入：將字串完全覆蓋寫入硬碟中
                File.WriteAllText(filePath, jsonString);

                // 4. 成功提示與狀態解除：解除未儲存的安全警戒狀態
                MessageBox.Show("資料已成功儲存！", "儲存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isUnsaved = false;
            }
            catch (Exception ex)
            {
                // 串流安全攔截：避免檔案遭鎖定或其他未知硬體錯誤導致程式崩潰
                MessageBox.Show($"存檔失敗，錯誤原因：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 事件：點擊「開始篩選」按鈕 (LINQ 進階多條件複合檢索)
        /// </summary>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            // 1. 清空前端 DataGridView，準備接收過濾後的乾淨資料
            dgvRecords.Rows.Clear();

            // 2. 將全域資料轉化為可列舉的 LINQ 查詢管道
            var filteredRecords = records.AsEnumerable();

            // 3. 複合條件 A：動態檢查關鍵字輸入框是否包含文字
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                // 模糊搜尋：同時比對「備註描述」或「消費分類」是否包含該字根
                filteredRecords = filteredRecords.Where(r =>
                    (r.Description != null && r.Description.Contains(txtSearch.Text)) ||
                    r.Category.Contains(txtSearch.Text));
            }

            // 4. 複合條件 B：檢查收支類型過濾下拉選單是否已被選取
            if (cmbFilterType.SelectedIndex != -1)
            {
                string selectedText = cmbFilterType.SelectedItem.ToString();

                // 條件分支：依據文字進行收支類型的強型別精準過濾
                if (selectedText == "支出" || selectedText == "只看支出")
                {
                    filteredRecords = filteredRecords.Where(r => r.Type == "支出");
                }
                else if (selectedText == "收入" || selectedText == "只看收入")
                {
                    filteredRecords = filteredRecords.Where(r => r.Type == "收入");
                }
            }

            // 5. 最終前端渲染：將 LINQ 檢索出的過濾資料集，重新逐行繪製到 DataGridView 上
            foreach (var record in filteredRecords)
            {
                dgvRecords.Rows.Add(
                    record.Date.ToString("yyyy-MM-dd"),
                    record.Type,
                    record.Category,
                    record.Amount,
                    record.Description
                );
            }
        }

        #endregion

        #region --- 使用者介面連動與體驗優化 (UI/UX) ---

        /// <summary>
        /// 核心方法：將記憶體 List 重新填入 DataGridView，並即時更新加總三大財務看板標籤
        /// </summary>
        private void UpdateGridAndLabels()
        {
            // 1. 清除舊有的資料行殘留
            dgvRecords.Rows.Clear();

            int totalIncome = 0;
            int totalExpense = 0;

            // 2. 迴圈遍歷，一邊繪製前端表格，一邊在後台進行動態累加計算
            foreach (var record in records)
            {
                dgvRecords.Rows.Add(
                    record.Date.ToString("yyyy-MM-dd"),
                    record.Type,
                    record.Category,
                    record.Amount,
                    record.Description
                );

                // 計算邏輯分流
                if (record.Type == "收入")
                    totalIncome += record.Amount;
                else
                    totalExpense += record.Amount;
            }

            // 3. 將計算出的最新財務金額格式化，更新至三大前端動態看板
            lblTotalIncome.Text = $"總收入：{totalIncome} 元";
            lblTotalExpense.Text = $"總支出：{totalExpense} 元";

            int balance = totalIncome - totalExpense;
            lblBalance.Text = $"目前餘額：{balance} 元";
        }

        /// <summary>
        /// 核心方法：重設所有前端控制項內容，回歸初始化狀態
        /// </summary>
        private void ClearInputs()
        {
            // 1. 文字與選擇狀態清空
            txtAmount.Clear();
            txtDescription.Clear();
            cmbCategory.SelectedIndex = -1;
            rbExpense.Checked = true;

            // 2. 關鍵歸零：徹底解除修改模式，將主按鈕換回預設新增樣式
            editingIndex = -1;
            btnInsert.Text = "新增紀錄";
            btnInsert.UseVisualStyleBackColor = true;

            // 3. 物理狀態鎖：因為欄位已完全清空，直接將「取消/清空」按鈕變更為停用灰色狀態
            btnClear.Enabled = false;
        }

        /// <summary>
        /// 狀態連動：即時檢查所有必填欄位的空值狀況，動態決定清空按鈕是否亮起
        /// </summary>
        private void CheckFieldsEmpty()
        {
            // 邏輯閘：只要滿足任一輸入框有字、分類有選，或者正在修改狀態，按鈕即亮起
            if (!string.IsNullOrEmpty(txtAmount.Text) ||
                !string.IsNullOrEmpty(txtDescription.Text) ||
                cmbCategory.SelectedIndex != -1 ||
                editingIndex != -1)
            {
                btnClear.Enabled = true; // 開啟按鈕（亮起）
            }
            else
            {
                btnClear.Enabled = false; // 停用按鈕（變灰）
            }
        }

        /// <summary>
        /// 事件：點擊「取消/清空欄位」按鈕
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (editingIndex == -1)
            {
                // 情境一：單純的新增階段清空
                ClearInputs();
                MessageBox.Show("欄位已清空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // 情境二：取消原本的表格雙擊修改模式，回歸到原始新增狀態
                ClearInputs();
                MessageBox.Show("已取消修改變更，回到全新新增模式！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 事件：雙擊右側 DataGridView 儲存格 (啟動進階雙擊修改模式)
        /// </summary>
        private void dgvRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 邊界安全攔截：點擊到欄位標題標頭時不執行
            if (e.RowIndex < 0 || e.RowIndex >= records.Count) return;

            // 1. 關鍵錨點切換：切換狀態鎖，記下目前正在修改這筆資料的物理索引
            editingIndex = e.RowIndex;

            // 2. 讀取指定物件，將舊有資料回填至左側輸入面板中
            var record = records[editingIndex];
            dtpDate.Value = record.Date;
            if (record.Type == "支出") rbExpense.Checked = true; else rbIncome.Checked = true;
            cmbCategory.SelectedItem = record.Category;
            txtAmount.Text = record.Amount.ToString();
            txtDescription.Text = record.Description;

            // 3. 視覺化引導：動態切換主按鈕為醒目的橘色「確認修改」，提示使用者操作改變
            btnInsert.Text = "確認修改";
            btnInsert.BackColor = System.Drawing.Color.Orange;

            // 4. 閃電聚焦：自動將游標瞬移聚焦到金額欄位並全選反白，提供使用者極速修改體驗
            txtAmount.Focus();
            txtAmount.SelectAll();

            // 5. 連動：進入修改狀態後，不論如何都強行亮起取消編輯按鈕
            btnClear.Enabled = true;
        }

        #endregion

        #region --- 隱藏細節優化：鍵盤與安全關閉攔截事件 ---

        /// <summary>
        /// 鍵盤攔截：限制金額輸入框只能打「0-9 的純數字」與「Backspace 鍵」 (物理級前端防呆)
        /// </summary>
        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 若輸入的按鍵非數字，且同時不為退格控制鍵(代碼8)，則透過 Handled 強制沒收輸入
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 鍵盤快捷連動：在備註區按下鍵盤 Enter 鍵時，視同點擊新增/確認按鈕
        /// </summary>
        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 沒收按鍵，防止 Windows 系統發出難聽的「嗶」警示音
                e.SuppressKeyPress = true;

                // 導向執行點擊事件
                btnInsert_Click(sender, e);
            }
        }

        /// <summary>
        /// 安全機制：當使用者點選視窗右上角「X」關閉時，攔截並實作未儲存防禦提示
        /// </summary>
        private void PersonalAccountingSystem_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 若當前記憶體狀態安全（無任何未存檔異動），直接放行關閉
            if (!isUnsaved) return;

            // 彈出具有「是、否、取消」三向路由的進階對話框
            DialogResult result = MessageBox.Show(
                "您有尚未儲存的變更，是否進行儲存？",
                "提醒",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // 分支一：選擇儲存，自動引導點擊 btnSave 按鈕執行 JSON 存檔，隨後順序關閉
                btnSave_Click(sender, e);
            }
            else if (result == DialogResult.No)
            {
                // 分支二：選擇否，代表放棄本次開啟期間的所有變動，放行不阻擋關閉
            }
            else if (result == DialogResult.Cancel)
            {
                // 分支三：選擇取消，核心防禦：駁回關閉事件，完美將視窗與未存檔資料完整保留在畫面上
                e.Cancel = true;
            }
        }

        // 以下為文字與元件內容變更時，觸發即時重新評估清空按鈕亮燈狀況的輕量事件
        private void txtAmount_TextChanged(object sender, EventArgs e) => CheckFieldsEmpty();
        private void txtDescription_TextChanged(object sender, EventArgs e) => CheckFieldsEmpty();
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e) => CheckFieldsEmpty();

        #endregion
    }
}