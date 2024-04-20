using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vp_server.Models;

public partial class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Не указано название продукта")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина названия должна быть не более 50 символов")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Не указана стоимость")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Введено не числовое значение")]
    public double Cost { get; set; }
    [Required(ErrorMessage ="Не указан материал")]
    public string Material { get; set; } = null!;

    public byte[]? Image { get; set; }

    public string? Taste { get; set; }

    public int CategoryId { get; set; }

    public int? NicotineTypeId { get; set; }

    public int? StrengthId { get; set; }

    public int? ManufacturerId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual NicotineType? NicotineType { get; set; }

    public virtual ICollection<ProductCount> ProductCounts { get; set; } = new List<ProductCount>();

    public virtual ICollection<ReplenishmentProduct> ReplenishmentProducts { get; set; } = new List<ReplenishmentProduct>();

    public virtual Strenght? Strength { get; set; }

    public virtual ICollection<TransactionsAndProduct> TransactionsAndProducts { get; set; } = new List<TransactionsAndProduct>();

    public virtual ICollection<View> Views { get; set; } = new List<View>();
}
