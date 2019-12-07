using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothingStore.Data
{
    /// <summary>
    /// מדיניות החזרה עבור פריט לבוש הניתן להחזרה
    /// </summary>
    public class ReturnPolicy
    {
        /// <summary>
        /// קוד מדיניות החזרה
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// מספר הימים לקבלת החזר מלא
        /// </summary>
        public int DaysForFullRefund { get; set; }

        /// <summary>
        /// מספר הימים לקבלת זיכוי בחנות
        /// </summary>
        public int DaysForStoreCredit { get; set; }

        public int ClothingItemTypeId { get; set; }
    }
}