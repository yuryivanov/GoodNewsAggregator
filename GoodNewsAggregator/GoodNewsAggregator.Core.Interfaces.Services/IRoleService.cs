using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> FindRoles();
        Task<RoleDto> FindRoleById(Guid id);

        //Task<IEnumerable<RSSDto>> GetRssByNewsId(Guid? id);
        //Task<RSSDto> GetRssById(Guid? id);
        //Task<RSSDto> EditRss(RSSDto rssDto);
        //Task RemoveRangeRss(IEnumerable<RSSDto> rSSDtos);
        //Task AddRangeNews(IEnumerable<RSSDto> rss);
    }
}
