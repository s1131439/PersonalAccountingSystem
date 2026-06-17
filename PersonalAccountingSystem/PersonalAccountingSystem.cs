using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;

namespace PersonalAccountingSystem
{
    public partial class PersonalAccountingSystem : Form
    {
        // 建立一個清單來存放所有的記帳資料
        private List<Transaction> records = new List<Transaction>();

        // 定義存檔的檔名
        private string filePath = "accounting_data.json";
        private int editingIndex = -1;
        private bool isUnsaved = false;
        public PersonalAccountingSystem()
        {
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            // ----- 1. 防呆檢查 (保持不變) -----
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("請選擇消費分類！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtAmount.Text) || !int.TryParse(txtAmount.Text, out int amount) || amount <= 0)
            {
                MessageBox.Show("請輸入正確的大於 0 的金額數字！", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string type = rbExpense.Checked ? "支出" : "收入";

            // ----- 2. 判斷是「新資料新增」還是「舊資料修改」 -----
            if (editingIndex == -1)
            {
                // 模式 A：全新新增
                Transaction newRecord = new Transaction
                {
                    Date = dtpDate.Value,
                    Type = type,
                    Category = cmbCategory.SelectedItem.ToString(),
                    Amount = amount,
                    Description = txtDescription.Text
                };
                records.Add(newRecord);
                isUnsaved = true; // 有新資料或修改，代表有未儲存的變更
            }
            else
            {
                // 模式 B：修改舊資料 (直接覆蓋 list 裡該索引的資料)
                records[editingIndex].Date = dtpDate.Value;
                records[editingIndex].Type = type;
                records[editingIndex].Category = cmbCategory.SelectedItem.ToString();
                records[editingIndex].Amount = amount;
                records[editingIndex].Description = txtDescription.Text;

                // 修改完畢，將狀態重設回正常模式
                editingIndex = -1;
                btnInsert.Text = "新增紀錄";
                btnInsert.UseVisualStyleBackColor = true; // 恢復預設按鈕顏色
                MessageBox.Show("資料修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // ----- 3. 刷新 UI 與清空 -----
            UpdateGridAndLabels();
            ClearInputs();
        }
        // 負責把 records 清單更新到 DataGridView，並重新計算總金額
        private void UpdateGridAndLabels()
        {
            // 1. 清空 DataGridView 舊資料 (假設你的 DataGridView 叫 dgvRecords)
            dgvRecords.Rows.Clear();

            int totalIncome = 0;
            int totalExpense = 0;

            // 2. 將清單中的每一筆資料填入表格
            foreach (var record in records)
            {
                dgvRecords.Rows.Add(
                    record.Date.ToString("yyyy-MM-dd"),
                    record.Type,
                    record.Category,
                    record.Amount,
                    record.Description
                );

                // 3. 同步計算總收入與總支出
                if (record.Type == "收入")
                    totalIncome += record.Amount;
                else
                    totalExpense += record.Amount;
            }

            // 4. 更新下方的 Label 狀態 (請對照你實際的 Label 命名)
            lblTotalIncome.Text = $"總收入：{totalIncome} 元";
            lblTotalExpense.Text = $"總支出：{totalExpense} 元";

            int balance = totalIncome - totalExpense;
            lblBalance.Text = $"目前餘額：{balance} 元";
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            dgvRecords.Rows.Clear();

            var filteredRecords = records.AsEnumerable();

            // 1. 關鍵字篩選
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                filteredRecords = filteredRecords.Where(r =>
                    (r.Description != null && r.Description.Contains(txtSearch.Text)) ||
                    r.Category.Contains(txtSearch.Text));
            }

            // 2. 收支類型篩選 (修正後的安全寫法)
            if (cmbFilterType.SelectedIndex != -1)
            {
                string selectedText = cmbFilterType.SelectedItem.ToString();

                // 如果選「全部」就不篩選，選其他的就精準比對「支出」或「收入」
                if (selectedText == "支出" || selectedText == "只看支出")
                {
                    filteredRecords = filteredRecords.Where(r => r.Type == "支出");
                }
                else if (selectedText == "收入" || selectedText == "只看收入")
                {
                    filteredRecords = filteredRecords.Where(r => r.Type == "收入");
                }
            }

            // 3. 繪製到畫面上
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

        // 負責重設所有輸入狀態，不論新增完畢或取消修改都會呼叫此方法
        private void ClearInputs()
        {
            // 1. 清空所有輸入控制項
            txtAmount.Clear();
            txtDescription.Clear();
            cmbCategory.SelectedIndex = -1; // 取消選取分類
            rbExpense.Checked = true;       // 預設切回支出

            // 2. 解除修改狀態，回歸預設
            editingIndex = -1;
            btnInsert.Text = "新增紀錄";
            btnInsert.UseVisualStyleBackColor = true;

            // 3. ⚡ 關鍵新增：既然已經全空了，直接讓「清空/取消」按鈕變成停用狀態（灰色）
            btnClear.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 檢查使用者有沒有選取表格中的任何一列
            if (dgvRecords.CurrentRow == null || dgvRecords.CurrentRow.Index < 0)
            {
                MessageBox.Show("請先點擊右邊表格，選取一筆你想刪除的紀錄！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 獲取目前選取那一列的索引（第幾筆資料）
            int selectedIndex = dgvRecords.CurrentRow.Index;

            // 防止選到表格最後一列的空白預留列
            if (selectedIndex >= records.Count) return;

            // 跳出確認視窗，避免使用者誤刪（防呆機制）
            DialogResult result = MessageBox.Show("確定要刪除這筆記帳紀錄嗎？", "確認刪除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                // 1. 從後台的全域 records 清單中移除該筆資料
                records.RemoveAt(selectedIndex);
                isUnsaved = true; // 資料被刪除了，也算變更

                // 2. 重新刷新畫面與計算金額
                UpdateGridAndLabels();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 設定 JSON 的排版格式（讓存出來的文字檔比較美觀、易讀）
                var options = new JsonSerializerOptions { WriteIndented = true };

                // 2. 將 records 清單序列化成 JSON 字串
                string jsonString = JsonSerializer.Serialize(records, options);

                // 3. 將字串寫入檔案 (filePath 在最上方已經宣告為 "accounting_data.json")
                File.WriteAllText(filePath, jsonString);

                // 4. 提示使用者儲存成功
                MessageBox.Show("資料已成功儲存！", "儲存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isUnsaved = false; // 資料已經安全寫入硬碟，解除警報
            }
            catch (Exception ex)
            {
                // 如果存檔失敗（例如檔案被其他程式鎖定），跳出錯誤訊息
                MessageBox.Show($"存檔失敗，錯誤原因：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PersonalAccountingSystem_Load(object sender, EventArgs e)
        {
            // 檢查硬碟裡有沒有之前存過的記帳檔案
            if (File.Exists(filePath))
            {
                try
                {
                    // 1. 讀取檔案內的所有文字
                    string jsonString = File.ReadAllText(filePath);

                    // 2. 將 JSON 字串反序列化回 List<Transaction> 清單
                    records = JsonSerializer.Deserialize<List<Transaction>>(jsonString);

                    // 3. 如果清單為空，幫它重新初始化避免報錯
                    if (records == null)
                    {
                        records = new List<Transaction>();
                    }

                    // 4. 讀檔成功後，刷新介面把歷史資料顯示出來
                    UpdateGridAndLabels();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"讀取舊資料失敗，檔案可能損毀。\n錯誤原因：{ex.Message}", "讀檔錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // 在 Load 事件的最下面加上這行，讓程式剛啟動時輸入框與按鈕狀態正確初始化
            ClearInputs();
        }

        private void dgvRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= records.Count) return;

            // 1. 記下現在正在修改這一列
            editingIndex = e.RowIndex;

            // 2. 獲取該筆資料並帶入輸入框
            var record = records[editingIndex];
            dtpDate.Value = record.Date;
            if (record.Type == "支出") rbExpense.Checked = true; else rbIncome.Checked = true;
            cmbCategory.SelectedItem = record.Category;
            txtAmount.Text = record.Amount.ToString();
            txtDescription.Text = record.Description;

            // 3. 貼心提示：把新增按鈕的文字改成「確認修改」
            btnInsert.Text = "確認修改";
            btnInsert.BackColor = System.Drawing.Color.Orange;

            // 4. ⚡ 閃電引導：自動把游標鎖定到左邊的金額輸入框，並全選文字，方便他直接重打！
            txtAmount.Focus();
            txtAmount.SelectAll(); // 自動反白金額，使用者連 Backspace 都不用按，直接打字就能覆蓋
            // 在雙擊事件的最下面加上這行，確保進入修改模式時取消按鈕一定是亮起的！
            btnClear.Enabled = true;
        }

        private void PersonalAccountingSystem_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 如果目前沒有未儲存的變更，直接放行讓視窗關閉
            if (!isUnsaved) return;

            // 跳出「是、否、取消」三顆按鈕的對話框
            DialogResult result = MessageBox.Show(
                "您有尚未儲存的變更，是否進行儲存？",
                "提醒",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // 模式一：使用者按「是」 -> 自動幫忙觸發儲存按鈕的點擊事件
                btnSave_Click(sender, e);
                // 儲存完成後，放行關閉視窗 (不阻擋關閉)
            }
            else if (result == DialogResult.No)
            {
                // 模式二：使用者按「否」 -> 代表確定不儲存，放行關閉視窗 (不阻擋關閉)
            }
            else if (result == DialogResult.Cancel)
            {
                // 模式三：使用者按「取消」 -> ⚡ 關鍵！取消關閉視窗事件，讓畫面留在原處
                e.Cancel = true;
            }
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            // 檢查使用者按下的是不是 Enter 鍵
            if (e.KeyCode == Keys.Enter)
            {
                // ⚡ 關鍵行：消除按 Enter 的系統「嗶」提示音
                e.SuppressKeyPress = true;

                // 自動去執行新增/修改按鈕的點擊事件！
                btnInsert_Click(sender, e);
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 允許輸入數字 (0-9) 與 倒退鍵 (Backspace, 控制碼為 8)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                // ⚡ 關鍵行：將 Handled 設為 true，代表系統攔截這個按鍵，不讓它輸入進 TextBox
                e.Handled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // 如果目前本來就不是修改模式，只是單純的欄位清空
            if (editingIndex == -1)
            {
                ClearInputs();
                MessageBox.Show("欄位已清空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // 如果目前是修改模式，使用者主動按下取消
                ClearInputs();
                MessageBox.Show("已取消修改變更，回到全新新增模式！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // 檢查目前輸入框有沒有東西，動態決定要不要啟用清空按鈕
        private void CheckFieldsEmpty()
        {
            // 只要「金額有打字」或「備註有打字」或「分類有選取」或「正處於修改模式」
            if (!string.IsNullOrEmpty(txtAmount.Text) ||
                !string.IsNullOrEmpty(txtDescription.Text) ||
                cmbCategory.SelectedIndex != -1 ||
                editingIndex != -1)
            {
                btnClear.Enabled = true; // 亮起按鈕
            }
            else
            {
                btnClear.Enabled = false; // 保持灰色
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            CheckFieldsEmpty();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            CheckFieldsEmpty();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckFieldsEmpty();
        }
    }
}
