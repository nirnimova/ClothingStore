using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClothingStore.Models;

namespace ClothingStore.Data
{
    public class ClothingStoreRepository
    {
        private ClothingStoreDBContext _db;

        public ClothingStoreRepository(ClothingStoreDBContext db)
        {
            this._db = db;
        }

        internal ClothingItem GetClothingItemById(int clothingItemId) => (
            from ci in this._db.ClothingItems
            where ci.Id == clothingItemId
            select ci).Single();

        internal bool IsClothingItemPurchased(int clothingItemId) => (
            from cip in this._db.ClothingItemPurchases
            where cip.ClothingItemId == clothingItemId
            select true).SingleOrDefault();

        internal ClothingItemPurchase GetPurchaseForClothingItem(int clothingItemId) => (
            from cip in this._db.ClothingItemPurchases
            where cip.ClothingItemId == clothingItemId
            select cip).Single();

        internal Dictionary<int, int> GetPurchasesInMonthByMonthAndYear(int month, int year) => (
            from cip in _db.ClothingItemPurchases
            where cip.DatePurchashed.Month == month
            where cip.DatePurchashed.Year == year
            group cip by cip.DatePurchashed.Day into cipGroup
            select new
            {
                Day = cipGroup.Key,
                TotalPurchases = cipGroup.Count(),
            }).ToDictionary(itm => itm.Day, itm => itm.TotalPurchases);

        internal Dictionary<int, int> GetReturnsInMonthByMonthAndYear(int month, int year) => (
             from cir in _db.ClothingItemReturns
             where cir.DateReturned.Month == month
             where cir.DateReturned.Year == year
             group cir by cir.DateReturned.Day into cirGroup
             select new
             {
                 Day = cirGroup.Key,
                 TotalReturns = cirGroup.Count(),
             }).ToDictionary(itm => itm.Day, itm => itm.TotalReturns);

        internal bool IsClothingItemType(int clothingItemTypeId) => (
            from cit in this._db.ClothingItemTypes
            where cit.Id == clothingItemTypeId
            select true).Single();

        internal ReturnPolicy GetReturnPolicyForClothingItem(int clothingItemId) => (
            from rp in this._db.ReturnPolicies
            join ci in this._db.ClothingItems on rp.ClothingItemTypeId equals ci.ClothingItemTypeId
            where ci.Id == clothingItemId
            select rp).SingleOrDefault();

        internal void AddPurchase(ClothingItem clothingItem) =>
            this._db.ClothingItemPurchases.Add(new ClothingItemPurchase()
            {
                DatePurchashed = DateTime.Now,
                PriceAtPurchase = clothingItem.Price,
                ClothingItemId = clothingItem.Id,
            });

        internal void RemovePurchase(ClothingItemPurchase clothingItemPurchase) => this._db.ClothingItemPurchases.Remove(clothingItemPurchase);
        internal void AddRangeClothingItems(IEnumerable<ClothingItem> clothingItems) => this._db.ClothingItems.AddRange(clothingItems);
        internal void AddReturn(ClothingItemReturn clothingItemReturn) => this._db.ClothingItemReturns.Add(clothingItemReturn);
        internal void Save() => this._db.SaveChanges();
    }
}