using System;
using System.Collections.Generic;

namespace vp_server.Models;

public partial class TransactionsAndProduct
{
    public int Id { get; set; }

    public int TransactionId { get; set; }

    public int ProductId { get; set; }

    public int Quantitly { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
