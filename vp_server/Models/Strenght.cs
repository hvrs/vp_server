﻿using System;
using System.Collections.Generic;

namespace vp_server.Models;

public partial class Strenght
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
