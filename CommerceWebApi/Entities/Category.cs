using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommerceWebApi.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Category Parent { get; set; }

        public ICollection<Category> ChildCategories { get; set; }

    }
}