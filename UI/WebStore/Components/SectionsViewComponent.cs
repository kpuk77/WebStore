using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;

using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData productData) => _ProductData = productData;

        public IViewComponentResult Invoke(string sectionId)
        {
            //var sectId = int.TryParse(sectionId, out var id) ? id : (int?)null;
            int.TryParse(sectionId, out var id);

            var sections = GetSections(id, out var parentId);

            ViewBag.SectionId = id;
            ViewBag.ParentId = parentId;

            return View(sections);
        }

        private IEnumerable<SectionViewModel> GetSections(int? id, out int? parentId)
        {
            parentId = null;
            var sections = _ProductData.GetSections();

            var parentSections = sections.Where(s => s.ParentId == null);
            var parentSectionsViews = parentSections.Select(s => new SectionViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Order = s.Order
            }).ToList();

            foreach (var parent in parentSectionsViews)
            {
                var childs = sections.Where(s => s.ParentId == parent.Id);

                foreach (var child in childs)
                {
                    if (child.Id == id)
                        parentId = child.ParentId;

                    parent.ChildSections.Add(new SectionViewModel
                    {
                        Id = child.Id,
                        Name = child.Name,
                        Order = child.Order,
                        Parent = parent
                    });
                }

                parent.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }

            parentSectionsViews.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            return parentSectionsViews;
        }
    }
}
