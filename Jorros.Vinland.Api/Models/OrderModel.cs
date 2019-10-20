using System;
using System.Collections.Generic;

namespace Jorros.Vinland.Api.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        public string User { get; set; }

        public IEnumerable<BottleModel> Bottles { get; set; }
    }
}