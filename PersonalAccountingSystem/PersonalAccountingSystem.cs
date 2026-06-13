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
        public PersonalAccountingSystem()
        {
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            // ----- 1. 防呆檢查 -----

            // 檢查分類有沒有選 (假設你的 ComboBox 叫 cmbCategory)
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("請選擇消費分類！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 檢查金額是否為空，以及是否為有效數字 (假設你的 TextBox 叫 txtAmount)
            if (string.IsNullOrWhiteSpace(txtAmount.Text) || !int.TryParse(txtAmount.Text, out int amount))
            {
                MessageBox.Show("請輸入正確的金額數字（不可包含中英文字母）！", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 金額不能為負數
            if (amount <= 0)
            {
                MessageBox.Show("金額必須大於 0 元！", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ----- 2. 讀取介面資料並建立物件 -----

            // 判斷是支出還是收入 (假設你的 RadioButton 叫 rbExpense 和 rbIncome)
            string type = rbExpense.Checked ? "支出" : "收入";

            Transaction newRecord = new Transaction
            {
                Date = dtpDate.Value,                    // 來自 DateTimePicker
                Type = type,
                Category = cmbCategory.SelectedItem.ToString(),
                Amount = amount,
                Description = txtDescription.Text        // 來自 TextBox
            };

            // ----- 3. 加進清單並更新 UI -----

            records.Add(newRecord);                      // 加進全域清單
            UpdateGridAndLabels();                       // 呼叫更新畫面的方法
            ClearInputs();                               // 呼叫清空輸入欄位的方法
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

        // 方便使用者繼續記下一筆，自動清空輸入框
        private void ClearInputs()
        {
            txtAmount.Clear();
            txtDescription.Clear();
            cmbCategory.SelectedIndex = -1; // 取消選取分類
            rbExpense.Checked = true;       // 預設切回支出
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
        }
    }
}
