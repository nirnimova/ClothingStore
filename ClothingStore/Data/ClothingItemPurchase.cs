using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothingStore.Data
{
    public class ClothingItemPurchase
    {
        /// <summary>
        /// קוד רכישה
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// מחיר פריט בזמן רכישה
        /// </summary>
        public double PriceAtPurchase { get; set; }

        /// <summary>
        /// תאריך הרכישה
        /// </summary>
        public DateTime DatePurchashed { get; set; }

        public int ClothingItemId { get; set; }
    }
}