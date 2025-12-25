using MediatR;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Subscriptions.Queries.GetAllPlans
{
    public class GetAllPlansQuery : IRequest<Result<List<GetAllPlansQueryResponse>>>
    {
    }
}