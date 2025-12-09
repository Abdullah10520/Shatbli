using MediatR;
using Shatbli.Core.Bases;

namespace Shatbli.Core.Features.Products.Commands.Models
{
    public class DeleteProductCommand : IRequest<Response<bool>>
    {
        public int ProductId { get; set; }
    }
}