using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels.Product;
using WebStore.Infrastructure.Map;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class SectionViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;
        public SectionViewComponent(IProductData productData)
        {
            _ProductData = productData;
        }

        public IViewComponentResult Invoke(string SectionId)
        {
            var section_id = int.TryParse(SectionId, out var id) ? id : (int?)null;
            var section = GetSections(section_id, out var parent_section_id);
            return View(new SectionCompleteViewModel
            {
                Sections = section,
                CurrentSectionId = section_id,
                CurrentParentSection = parent_section_id
            });
        }
        public IEnumerable<SectionViewModel> GetSections(int? SectionId, out int? ParentSectionId)
        {
            ParentSectionId = null;
            var sections = _ProductData.GetSections();

            var parent_sections = sections.Where(s => s.ParentId == null).Select(SectionViewModelMapper.CreateViewModel).ToList();

            foreach (var parent_section in parent_sections)
            {
                var child_sections = sections.Where(s => s.ParentId == parent_section.Id).Select(SectionViewModelMapper.CreateViewModel);

                foreach (var child_section in child_sections)
                {
                    if (child_section.Id == SectionId)
                        ParentSectionId = parent_section.Id;
                    parent_section.ChildSections.Add(child_section);
                }
                parent_section.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }
            parent_sections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            return parent_sections;
        }
    }
}