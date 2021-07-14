using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping.DTO.Products
{
    public static class SectionDTOMapping
    {
        public static Section FromDTO(this SectionDTO sectionDTO) => sectionDTO is null
            ? null
            : new Section
            {
                Id = sectionDTO.Id,
                Name = sectionDTO.Name,
                Order = sectionDTO.Order,
                ParentId = sectionDTO.ParentId,
            };

        public static SectionDTO ToDTO(this Section section) => section is null
            ? null
            : new SectionDTO()
            {
                Id = section.Id,
                Name = section.Name,
                Order = section.Order,
                ParentId = section.ParentId,
            };

        public static IEnumerable<Section> FromDTO(this IEnumerable<SectionDTO> sectionsDTO) =>
            sectionsDTO.Select(FromDTO);

        public static IEnumerable<SectionDTO> ToDTO(this IEnumerable<Section> sections) =>
            sections.Select(ToDTO);
    }
}