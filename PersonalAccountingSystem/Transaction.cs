using System;

namespace PersonalAccountingSystem
{
    public class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // 唯一識別碼
        public DateTime Date { get; set; }                           // 日期
        public string Type { get; set; }                             // 收入/支出
        public string Category { get; set; }                         // 分類
        public int Amount { get; set; }                              // 金額
        public string Description { get; set; }                      // 備註
    }
}