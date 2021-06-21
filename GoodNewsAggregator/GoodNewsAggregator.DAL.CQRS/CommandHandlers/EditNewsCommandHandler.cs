using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers
{
    public class EditNewsCommandHandler : IRequestHandler<EditNewsCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public EditNewsCommandHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(EditNewsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var oldNews = _dbContext.News.FirstOrDefault(news => news.Id.Equals(request.News.Id));

                //var addedNews = request.News.  News.Select(n => _mapper.Map<News>(n));
                var EditedNews = _mapper.Map<News>(request.News);

                _dbContext.News.Remove(oldNews);
                _dbContext.News.Add(EditedNews);
                //How many records in db are changed, if 0 - request failed
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, "AddCommentCommandHandler was not successful");
                throw;
            }            
        }
    }
}
