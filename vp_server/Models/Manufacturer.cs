using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vp_server.Models;

public partial class Manufacturer
{
    public int Id { get; set; }
    [Required(ErrorMessage ="Поле обязтельно для заполнения")]
    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
