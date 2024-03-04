using System;
using System.Collections.Generic;

namespace vp_server.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public double Sum { get; set; }

    public bool IsViewed { get; set; }

    public virtual ICollection<TransactionsAndProduct> TransactionsAndProducts { get; set; } = new List<TransactionsAndProduct>();
}
