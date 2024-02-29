using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    [DisplayName("Category Name in English")]
    public string NameEN{ get; set; }
	[DisplayName("Category Name in Russian")]
	public string NameRU { get; set; }
	[DisplayName("Display Order")]
    [Range(1, 100, ErrorMessage = "Display order must be between 1-100")]
    public int DisplayOrder { get; set; }


}
