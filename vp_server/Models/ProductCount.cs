using System;
using System.Collections.Generic;

namespace vp_server.Models;

public partial class ProductCount
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int? Count { get; set; }

    public virtual Product Product { get; set; } = null!;
}
