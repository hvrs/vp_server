using System;
using System.Collections.Generic;

namespace vp_server.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public float Cost { get; set; }

    public string? Material { get; set; }

    public byte[]? Image { get; set; }

    public string? Taste { get; set; }

    public int CategoryId { get; set; }

    public int? NicotineTypeId { get; set; }

    public int? StrengthId { get; set; }

    public int ManufacturerId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual NicotineType? NicotineType { get; set; }

    public virtual ICollection<ProductBasket> ProductBaskets { get; set; } = new List<ProductBasket>();

    public virtual ICollection<ProductCount> ProductCounts { get; set; } = new List<ProductCount>();

    public virtual ICollection<ReplenishmentProduct> ReplenishmentProducts { get; set; } = new List<ReplenishmentProduct>();

    public virtual Strenght? Strength { get; set; }

    public virtual ICollection<TransactionsAndProduct> TransactionsAndProducts { get; set; } = new List<TransactionsAndProduct>();

    public virtual ICollection<View> Views { get; set; } = new List<View>();
}
