using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.ViewModels.Product
{
    public class SectionCompleteViewModel
    {
        public IEnumerable<SectionViewModel> Sections { get; set; }
        public int? CurrentParentSection { get; set; }
        public int? CurrentSectionId { get; set; }
    }
    public class BrandCompleteViewModel
    {
        public IEnumerable<BrandViewModel> Brands { get; set; }
        public int? CurrentBrandId { get; set; }
    }
}
