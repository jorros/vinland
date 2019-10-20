using System;
using System.Collections.Generic;

namespace Jorros.Vinland.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string User { get; set; }

        public IEnumerable<Bottle> Bottles { get; set; }

        public DateTime OrderDate { get; set; }

        public Guid ReferenceId { get; set; }
    }
}