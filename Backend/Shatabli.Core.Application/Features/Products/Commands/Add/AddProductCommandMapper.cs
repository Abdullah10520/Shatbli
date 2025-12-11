using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Products.Commands.Add
{
    public class AddProductCommandMapper : Profile
    {
        public AddProductCommandMapper() 
        {
            CreateMap<AddProductCommand, Product>();
        }
    }
}
