using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Application.DTOs
{
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }
        public decimal Total { get; set; }
    }
}
