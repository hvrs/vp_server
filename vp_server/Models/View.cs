using System;
using System.Collections.Generic;

namespace vp_server.Models;

public partial class View
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}

public class ViewDTO
{
    public int ProductId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
}