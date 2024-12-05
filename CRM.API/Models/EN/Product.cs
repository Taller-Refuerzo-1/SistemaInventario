using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.API.Models.EN
{
    [Table("Product")]
    public class Product
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
        }
    }

