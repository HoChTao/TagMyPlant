using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TagMyPlant.Models
{
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Barcode")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Barcode should only contain numeric characters.")]
        public string Code { get; set; }
        public string? Type { get; set; }
        public string? ImageURL { get; set; }
        public string? PdfUrlDE { get; set; }
        public string? PdfUrlGB { get; set; }

        public Device()
        {
        }
    }
}
