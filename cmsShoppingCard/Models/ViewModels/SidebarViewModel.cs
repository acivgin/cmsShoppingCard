using cmsShoppingCard.Models.Data;
using System.ComponentModel;
using System.Web.Mvc;

namespace cmsShoppingCard.Models.ViewModels
{
    public class SidebarViewModel
    {
        public SidebarViewModel() { }

        public SidebarViewModel(SidebarDTO row)
        {
            Id = row.Id;
            Body = row.Body;
        }
        public int Id { get; set; }

        [DisplayName("Sidebar")]
        [AllowHtml]
        public string Body { get; set; }
    }
}