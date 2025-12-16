using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Products.Queries.GetCeramicById
{
    public class GetCeramicByIdMapper : Profile
    {
        public GetCeramicByIdMapper() 
        {
            CreateProjection<Product, CeramicDTO>();
        }
    }
}
