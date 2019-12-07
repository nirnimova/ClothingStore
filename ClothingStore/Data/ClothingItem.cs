using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothingStore.Data
{
    public class ClothingItem
    {
        /// <summary>
        /// קוד פריט לבוש
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// שם המלא של פריט הלבוש
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// מחיר פריט הלבוש
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// שם החברה
        /// </summary>
        public string BrandName { get; set; }

        public int ClothingItemTypeId { get; set; }
    }
}