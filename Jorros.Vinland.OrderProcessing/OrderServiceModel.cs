using System;
using System.Collections.Generic;

namespace Jorros.Vinland.OrderProcessing
{
    public class OrderServiceModel
    {
        public string User { get; set; }

        public IEnumerable<BottleServiceModel> Bottles { get; set; }

        public DateTime OrderDate { get; set; }

        public Guid ReferenceId { get; set; }
    }
}