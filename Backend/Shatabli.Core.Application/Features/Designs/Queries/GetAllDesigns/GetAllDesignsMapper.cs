using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetAllDesigns
{
    public class GetAllDesignsMapper : Profile
    {
        public GetAllDesignsMapper() 
        {
            CreateProjection<Design, GetAllDesignDTO>();
        }    
    }
}
