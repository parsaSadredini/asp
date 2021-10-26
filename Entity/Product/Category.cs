using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class Category: BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public int? CategoryID { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Category ParentCategory { get; set; }
        public ICollection<Category> ChildCategories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
