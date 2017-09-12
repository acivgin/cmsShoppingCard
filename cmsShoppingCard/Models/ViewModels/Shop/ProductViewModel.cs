using cmsShoppingCard.Models.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace cmsShoppingCard.Models.ViewModels.Shop
{
    public class ProductViewModel
    {
        public ProductViewModel() { }

        public ProductViewModel(ProductDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Descritpion = row.Descritpion;
            Price = row.Price;
            CategoryName = row.CategoryName;
            CategoryId = row.CategoryId;
            ImageName = row.ImageName;
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Descritpion { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        [DisplayName("Categories")]
        public int CategoryId { get; set; }
        public string ImageName { get; set; }


        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GaleryImages { get; set; }

    }
}