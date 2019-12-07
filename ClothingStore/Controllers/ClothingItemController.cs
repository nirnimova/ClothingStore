using ClothingStore.Data;
using ClothingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace ClothingStore.Controllers
{
    public class ClothingItemController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public JsonResult AddItem(NewClothingItemModel data)
        {
            using (ClothingStoreDBContext db = new ClothingStoreDBContext())
            {
                var repo = new ClothingStoreRepository(db);

                repo.AddItem(new ClothingItem()
                {
                    Name = data.Name,
                    Price = data.Price,
                    BrandName = data.BrandName,
                    ClothingItemTypeId = data.ClothingItemTypeId,
                }, data.Ammount);

                return new JsonResult()
                {
                    Data = "Success",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };
            }
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult GetStatisticsForMonthAndYear(int month, int year)
        {
            using (ClothingStoreDBContext db = new ClothingStoreDBContext())
            {
                var repo = new ClothingStoreRepository(db);

                StatisticsModel[] result = repo.GetStatisticsForMonthAndYear(month, year);

                return new JsonResult()
                {
                    Data = result,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };
            }
        }
    }
}
