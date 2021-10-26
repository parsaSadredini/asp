using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Model
{
    public class ProductViewModel
    {
        [Required]
        [MaxLength(200,ErrorMessage ="طول نام باید بیشتر از 200 باشد")]
        public string name { get; set; }
        [Required]
        public double price { get; set; }
        public int? OperatorID { get; set; }
        public int CategoryID { get; set; }
        
    }
}
