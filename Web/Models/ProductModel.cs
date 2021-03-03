using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Web.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }


        public string GenerateCeoLink()
        {
            string str = Name.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-");   
            return str;
        }
    }
}