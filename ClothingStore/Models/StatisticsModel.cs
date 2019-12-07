using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothingStore.Models
{
    public class StatisticsModel
    {
        public int Day { get; set; }
        public int? Purchases { get; set; }
        public int? Returns { get; set; }
    }
}