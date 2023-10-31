using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class ListingResponse
    {
        public List<Listing> listings { get; set; } = new List<Listing>();
        public int pages { get; set; }
        public int currentPage { get; set; }
    }
}
