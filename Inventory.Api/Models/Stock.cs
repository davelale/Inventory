using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Api.Models
{
    public class Stock : IValidatableObject
    {
        [Key]
        public string Description { get; set; }
        public int NumberInStock { get; set; }
        public DateTime DeliveryDate { get; set; }
        [Range(0, 999)]
        public int NumberToBeDelivered { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NumberInStock < 0)
            {
                yield return new ValidationResult("Out of Stock", new[] { "NumberInStock" });
            }
        }
    }
}
