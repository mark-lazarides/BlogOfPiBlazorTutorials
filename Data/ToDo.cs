using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBlogOfPiGettingStarted.Data
{
  public class ToDo
  {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage ="Task Name is required.")]
    [StringLength(maximumLength:15, ErrorMessage ="Task Name is too long.")]
    public string Name { get; set; }

    [Required(ErrorMessage ="Status is required.")]
    public string Status { get; set; }

    [Required(ErrorMessage ="Due Date is required.")]
    public DateTime DueDate { get; set; }
  }
}
