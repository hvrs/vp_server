﻿using vp_server.Models;
namespace vp_server.Models.ViewModels
{
    public class ProductsInTransactionWithStatus
    {
        public int Id { get; set; }
        public List<TransactionProductDTO> tpDTO {  get; set; }
        public string Status { get; set; } = "";
    }
}
