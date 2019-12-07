using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothingStore.Data
{
    /// <summary>
    /// סוג פריט לבוש
    /// </summary>
    public class ClothingItemType
    {
        /// <summary>
        /// קוד הפריט
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// שם הפריט
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// האם ניתן להחזרה
        /// </summary>
        public bool IsReturnable { get; set; }
    }
}