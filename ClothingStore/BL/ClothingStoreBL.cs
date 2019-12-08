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
                repo.AddPurchase(clothingItem);

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
                var returnsInMonth = repo.GetReturnsInMonthByMonthAndYear(month, year);

                for (var i = 0; i < daysInMonth; i++)
                {
                    result[i] = new StatisticsModel()
                    {
                        Day = i + 1,
                        Purchases = (purchasesInMonth.ContainsKey(i + 1)) ? (int?)purchasesInMonth[i + 1] : null,
                        Returns = (returnsInMonth.ContainsKey(i + 1)) ? (int?)returnsInMonth[i + 1] : null,
                    };
                }

                return result;
            } 
        }

        internal void AddItem(ClothingItem ci, int ammount)
        {
            using (ClothingStoreDBContext db = new ClothingStoreDBContext())
            {
                var repo = new ClothingStoreRepository(db);

                //@@ Check Clothing Item Type
                try
                {
                    var isClothingItemType = repo.IsClothingItemType(ci.ClothingItemTypeId);
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

                repo.AddRangeClothingItems(clothingItems);
                repo.Save(); 
            }
        }

        internal void Return(int clothingItemId)
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
                    throw new Exception("Invalid Clothing Item Id!");
                }

                var clothingItemReturn = new ClothingItemReturn();

                //@@ Check Return Policy
                var returnPolicy = repo.GetReturnPolicyForClothingItem(clothingItemId);

                ClothingItemPurchase clothingItemPurchase;

                try
                {
                    clothingItemPurchase = repo.GetPurchaseForClothingItem(clothingItemId);
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

                repo.RemovePurchase(clothingItemPurchase);
                repo.AddReturn(clothingItemReturn);
                repo.Save();
            }
        }
    }
}