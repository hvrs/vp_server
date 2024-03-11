﻿using System;
using System.Collections.Generic;

namespace vp_server.Models;

public partial class ReplenishmentProduct
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}
