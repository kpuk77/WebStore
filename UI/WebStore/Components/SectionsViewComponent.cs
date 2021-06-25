using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData productData) => _ProductData = productData;

        public IViewComponentResult Invoke()
        {
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

            return View(parentSectionsViews);
        }
    }
}
