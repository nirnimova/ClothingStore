using ClothingStore.BL;
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
            var clothingStoreBL = new ClothingStoreBL();

            clothingStoreBL.AddItem(new ClothingItem()
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

        [System.Web.Mvc.HttpGet]
        public JsonResult GetStatisticsForMonthAndYear(int month, int year)
        {
            var clothingStoreBL = new ClothingStoreBL();

            return new JsonResult()
            {
                Data = clothingStoreBL.GetStatisticsForMonthAndYear(month, year),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
        }
    }
}
