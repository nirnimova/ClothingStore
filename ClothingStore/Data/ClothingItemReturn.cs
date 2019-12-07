using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothingStore.Data
{
    public class ClothingItemReturn
    {
        public int Id { get; set; }

        /// <summary>
        /// תאריך ההחזרה
        /// </summary>
        public DateTime DateReturned { get; set; }

        /// <summary>
        /// סכום הכסף שהוחזר
        /// </summary>
        public double ReturnedAmmount { get; set; }

        /// <summary>
        /// האם הוחזר זיכוי לחנות או החזר מלא
        /// </summary>
        public bool IsStoreCredit { get; set; }

        public int ClothingItemId { get; set; }
    }
}