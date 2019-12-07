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
using ClothingStore.Data;

namespace ClothingStore.Controllers
{
    public class ClothingItemReturnsController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public JsonResult Return([FromBody]int clothingItemId)
        {
            using (ClothingStoreDBContext db = new ClothingStoreDBContext())
            {
                var repo = new ClothingStoreRepository(db);
                var data = "Success";

                try
                {
                    repo.Return(clothingItemId);
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
}