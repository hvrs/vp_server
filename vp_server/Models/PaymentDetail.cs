using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vp_server.Models;

public partial class PaymentDetail
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Не указано название банка")]
    public string BankName { get; set; } = null!;

    [Required(ErrorMessage ="Не указанан ИНН ")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Введено не числовое значение")]
    [StringLength(12,MinimumLength =10, ErrorMessage = "Длина от 10 до 12 цифр")]
    public string BankInn { get; set; } = null!;
    [Required(ErrorMessage ="Не указан КПП")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Введено не числовое значение")]
    [StringLength(9, MinimumLength = 9, ErrorMessage = "Длина 9 цифр")]
    public string BankKpp { get; set; } = null!;
    [Required(ErrorMessage = "Не указан К/C Банка")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Введено не числовое значение")]
    [StringLength(20, MinimumLength = 20, ErrorMessage = "Длина 20 цифр")]
    public string BankKs { get; set; } = null!;
    [Required(ErrorMessage = "Не указан БИК")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Введено не числовое значение")]
    [StringLength(9, MinimumLength = 9, ErrorMessage = "Длина 9 цифр")]
    public string Bik { get; set; } = null!;
    [Required(ErrorMessage = "Не указан персональный расчетный счет")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Введено не числовое значение")]
    [StringLength(20, MinimumLength = 20, ErrorMessage = "Длина 20 цифр")]
    public string PersonalRs { get; set; } = null!;
    [Required(ErrorMessage = "Не указано название фирмы")]
    public string FirmName { get; set; } = null!;
}
