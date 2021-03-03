using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name Required")]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public string ParentCategoryName { get; set; }
    }
}