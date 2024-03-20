using System;
using System.Collections.Generic;

namespace vp_server.Models;

public partial class TransactionStatus
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
