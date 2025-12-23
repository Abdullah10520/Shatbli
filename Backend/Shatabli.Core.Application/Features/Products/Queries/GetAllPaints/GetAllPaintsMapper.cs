using AutoMapper;
using Shatabli.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Products.Queries.GetAllPaints
{
    public class GetAllPaintsMapper : Profile
    {
        public GetAllPaintsMapper()
        {
            CreateProjection<Product, PaintsDTO>();
        }
    }
}
