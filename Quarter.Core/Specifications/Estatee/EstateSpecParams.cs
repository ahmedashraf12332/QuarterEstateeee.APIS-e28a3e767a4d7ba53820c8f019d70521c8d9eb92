using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Specifications.Estatee
{
    public class EstateSpecParams
    {
        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }

        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public int PageSize { get; set; } = 3;
        public int PageIndex { get; set; } = 3;
    }
}

