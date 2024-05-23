using System;
using System.Collections.Generic;

namespace vp_server.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public float Sum { get; set; }

    public bool IsViewed { get; set; }

    public int TransactionStatusId { get; set; }

    public virtual TransactionStatus TransactionStatus { get; set; } = null!;

    public virtual ICollection<TransactionsAndProduct> TransactionsAndProducts { get; set; } = new List<TransactionsAndProduct>();
}
