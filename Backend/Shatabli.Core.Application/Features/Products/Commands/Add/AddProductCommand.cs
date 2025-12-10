using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Products.Commands.Add
{
    public class AddProductCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;  // For local storage
        public string Vendor { get; set; } = string.Empty;
        public ProductCategory Category { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
