using System;

namespace Jorros.Vinland.Data.Entities
{
    public class Bottle
    {
        public int Id { get; set; }

        public bool Confirmed { get; set; }

        public string WineryOrderReference { get; set; }

        public Guid ReferenceId { get; set; }

        public Order Order { get; set; }
    }
}