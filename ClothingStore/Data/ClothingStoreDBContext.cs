using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClothingStore.Data
{
    public class ClothingStoreDBContext : DbContext
    {
        public ClothingStoreDBContext() : base("DefaultConnection")
        {

        }

        public DbSet<ClothingItem> ClothingItems { get; set; }
        public DbSet<ClothingItemPurchase> ClothingItemPurchases { get; set; }
        public DbSet<ClothingItemReturn> ClothingItemReturns { get; set; }
        public DbSet<ClothingItemType> ClothingItemTypes { get; set; }
        public DbSet<ReturnPolicy> ReturnPolicies { get; set; }
    }
}