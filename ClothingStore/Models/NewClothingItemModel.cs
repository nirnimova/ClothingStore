using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothingStore.Models
{
    public class NewClothingItemModel
    {
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

        public int Ammount { get; set; }

        public int ClothingItemTypeId { get; set; }
    }
}