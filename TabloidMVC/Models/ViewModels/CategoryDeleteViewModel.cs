using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class CategoryDeleteViewModel
    {
        public Category Category { get; set; }
        public Post Post { get; set; }
        public List<Category> CategoryOptions { get; set; }
        public string Message { get; set; } 
    }
}
