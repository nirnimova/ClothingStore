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

        internal void Purchase(int clothingItemId)
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
                throw new InvalidOperationException("Invalid Clothing Item Id!");
            }

            //@@ Check if Purchased
            var isPurchased = (from cip in this._db.ClothingItemPurchases
                               where cip.ClothingItemId == clothingItemId
                               select true).SingleOrDefault();

            if (isPurchased)
            {
                throw new InvalidOperationException("Can't purchase item. Already purchased");
            }

            //@@ Add New Purchase
            this._db.ClothingItemPurchases.Add(new ClothingItemPurchase()
            {
                DatePurchashed = DateTime.Now,
                PriceAtPurchase = clothingItem.Price,
                ClothingItemId = clothingItem.Id,
            });

            this._db.SaveChanges();
        }

        internal StatisticsModel[] GetStatisticsForMonthAndYear(int month, int year)
        {
            int daysInMonth;

            try
            {
                daysInMonth = DateTime.DaysInMonth(year, month);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("Year / Month Invalid");
            }

            var result = new StatisticsModel[daysInMonth];

            var purchasesInMonth = (from cip in _db.ClothingItemPurchases
                                    where cip.DatePurchashed.Month == month
                                    where cip.DatePurchashed.Year == year
                                    group cip by cip.DatePurchashed.Day into cipGroup
                                    select new
                                    {
                                        Day = cipGroup.Key,
                                        TotalPurchases = cipGroup.Count(),
                                    });

            var returnsInMonth = (from cir in _db.ClothingItemReturns
                                  where cir.DateReturned.Month == month
                                  where cir.DateReturned.Year == year
                                  group cir by cir.DateReturned.Day into cirGroup
                                  select new
                                  {
                                      Day = cirGroup.Key,
                                      TotalReturns = cirGroup.Count(),
                                  });

            for (var i = 0; i < daysInMonth; i++)
            {
                result[i] = new StatisticsModel()
                {
                    Day = i + 1,
                    Purchases = purchasesInMonth.Where(p => p.Day == i + 1).SingleOrDefault()?.TotalPurchases,
                    Returns = returnsInMonth.Where(r => r.Day == i + 1).SingleOrDefault()?.TotalReturns,
                };
            }

            return result;
        }

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
    }
}