using MediatR;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.CQRS.Queries
{
    public class GetAllExistingNewsUrlsQuery : IRequest<List<string>>
    {
    }
}
