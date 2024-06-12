﻿using Microsoft.OData.Edm;

namespace Odata_Api.DTO
{
    public class OrderRequest
    {
        public int StaffId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public  DateTime OrderDate{ get; set; }     
        public int Price { get; set; }
    }
}
