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

        internal ClothingItem GetClothingItemById(int clothingItemId) => (from ci in this._db.ClothingItems
                                                                          where ci.Id == clothingItemId
                                                                          select ci).Single();

        internal bool IsClothingItemPurchased(int clothingItemId) => (from cip in this._db.ClothingItemPurchases
                                                                      where cip.ClothingItemId == clothingItemId
                                                                      select true).SingleOrDefault();

        internal void AddClothingItemPurchase(ClothingItem clothingItem) =>
            this._db.ClothingItemPurchases.Add(new ClothingItemPurchase()
            {
                DatePurchashed = DateTime.Now,
                PriceAtPurchase = clothingItem.Price,
                ClothingItemId = clothingItem.Id,
            });

        internal Dictionary<int, int> GetPurchasesInMonthByMonthAndYear(int month, int year) => (from cip in _db.ClothingItemPurchases
                                                                                                 where cip.DatePurchashed.Month == month
                                                                                                 where cip.DatePurchashed.Year == year
                                                                                                 group cip by cip.DatePurchashed.Day into cipGroup
                                                                                                 select new
                                                                                                 {
                                                                                                     Day = cipGroup.Key,
                                                                                                     TotalPurchases = cipGroup.Count(),
                                                                                                 }).ToDictionary(itm => itm.Day, itm => itm.TotalPurchases);

        internal void AddItem(ClothingItem ci, int ammount)
        {
            //@@ Check Clothing Item Type
            try
            {
                var isClothingItemType = (from cit in this._db.ClothingItemTypes
                                          where cit.Id == ci.ClothingItemTypeId
                                          select true).Single();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Invalid Clothing Item Type Id!");
            }

            ClothingItem[] clothingItems = new ClothingItem[ammount];
            for (var i = 0; i < ammount; i++)
            {
                clothingItems[i] = new ClothingItem()
                {
                    Name = ci.Name,
                    Price = ci.Price,
                    BrandName = ci.BrandName,
                    ClothingItemTypeId = ci.ClothingItemTypeId,
                };
            }

            this._db.ClothingItems.AddRange(clothingItems);
            this._db.SaveChanges();
        }

        internal void Return(int clothingItemId)
        {
            ClothingItem clothingItem;

            //@@ Find Clothing Item
            try
            {
                clothingItem = (from ci in this._db.ClothingItems
                                where ci.Id == clothingItemId
                                select ci).Single();
            }
            catch (InvalidOperationException)
            {
                throw new Exception("Invalid Clothing Item Id!");
            }

            var clothingItemReturn = new ClothingItemReturn();

            //@@ Check Return Policy
            var returnPolicy = (from rp in this._db.ReturnPolicies
                                join ci in this._db.ClothingItems on rp.ClothingItemTypeId equals ci.ClothingItemTypeId
                                where ci.Id == clothingItemId
                                select rp).SingleOrDefault();

            ClothingItemPurchase clothingItemPurchase;

            try
            {
                clothingItemPurchase = (from cip in this._db.ClothingItemPurchases
                                        where cip.ClothingItemId == clothingItemId
                                        select cip).Single();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Can't return item. Purchase not Made for Item");
            }

            if (returnPolicy != null)
            {
                var daysPassedSincePurchase = (DateTime.Now - clothingItemPurchase.DatePurchashed).Days;

                if (daysPassedSincePurchase <= returnPolicy.DaysForFullRefund)
                    clothingItemReturn.IsStoreCredit = false;
                else if (daysPassedSincePurchase <= returnPolicy.DaysForStoreCredit)
                    clothingItemReturn.IsStoreCredit = true;
                else
                {
                    throw new InvalidOperationException("Can't return item. Item does nor meet return / refund policy");
                }
            }

            //@@ Add New Return
            clothingItemReturn.DateReturned = DateTime.Now;
            clothingItemReturn.ReturnedAmmount = clothingItem.Price;
            clothingItemReturn.ClothingItemId = clothingItemId;

            this._db.ClothingItemPurchases.Remove(clothingItemPurchase);
            this._db.ClothingItemReturns.Add(clothingItemReturn);
            this._db.SaveChanges();
        }

        internal void Save() => this._db.SaveChanges();
    }
}