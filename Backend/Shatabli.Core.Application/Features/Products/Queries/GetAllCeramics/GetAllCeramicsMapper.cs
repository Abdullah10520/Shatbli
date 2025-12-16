using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Products.Queries.GetAllCeramics
{
    public class GetAllCeramicsMapper : Profile
    {
        public GetAllCeramicsMapper() 
        {
            CreateProjection<Product, CeramicDTO>();
        }
    }
}
