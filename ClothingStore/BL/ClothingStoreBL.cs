using ClothingStore.Data;
using ClothingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothingStore.BL
{
    public class ClothingStoreBL
    {
        internal void Purchase(int clothingItemId)
        {
            using (ClothingStoreDBContext db = new ClothingStoreDBContext())
            {
                var repo = new ClothingStoreRepository(db);

                ClothingItem clothingItem;
                
                //@@ Find Clothing Item
                try
                {
                    clothingItem = repo.GetClothingItemById(clothingItemId);
                }
                catch (InvalidOperationException)
                {
                    throw new InvalidOperationException("Invalid Clothing Item Id!");
                }

                //@@ Check if Purchased
                var isPurchased = repo.IsClothingItemPurchased(clothingItemId);
                if (isPurchased)
                {
                    throw new InvalidOperationException("Can't purchase item. Already purchased");
                }

                //@@ Add New Purchase
                repo.AddClothingItemPurchase(clothingItem);

                repo.Save();
            }
        }

        internal StatisticsModel[] GetStatisticsForMonthAndYear(int month, int year)
        {
            using (ClothingStoreDBContext db = new ClothingStoreDBContext())
            {
                var repo = new ClothingStoreRepository(db);

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

                var purchasesInMonth = repo.GetPurchasesInMonthByMonthAndYear(month, year);

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
                        //@@ TODO: Fix
                        Purchases = purchasesInMonth[i + 1],
                        Returns = returnsInMonth.Where(r => r.Day == i + 1).SingleOrDefault()?.TotalReturns,
                    };
                }

                return result;
            } 
        }
    }
}