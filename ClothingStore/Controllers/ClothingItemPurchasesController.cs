using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using System.Web.Mvc;
using ClothingStore.BL;
using ClothingStore.Data;

namespace ClothingStore.Controllers
{
    public class ClothingItemPurchasesController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public JsonResult Purchase([FromBody]int clothingItemId)
        {
            var clothingStoreBL = new ClothingStoreBL();
            var data = "Success";

            try
            {
                clothingStoreBL.Purchase(clothingItemId);
            }
            catch (Exception ex)
            {
                data = ex.Message;
            }

            return new JsonResult()
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
        }
    }
}