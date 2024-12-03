using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.API.Models.EN
{
    [Table("Bodega")]
    public class Bodega
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
    }
}
