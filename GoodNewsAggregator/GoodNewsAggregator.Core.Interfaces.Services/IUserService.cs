using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DataTransferObjects;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> FindUsers();
        Task<UserDto> FindUserById(Guid id);
        string GetPasswordHash(string modelPassword);
        Task<bool> RegisterUser(UserDto model);
        Task<UserDto> GetUserByEmail(string email);
        Task<int> AddAccessToken(AccessTokenDto accessTokenDto);
        Task<int> AddRefreshToken(RefreshTokenDto refreshTokenDto);
        Task<AccessTokenDto> GetAccessTokenByTokenString(string token);
        Task<RefreshTokenDto> GetRefreshTokenById(Guid id);

        //Task<IEnumerable<RSSDto>> GetRssByNewsId(Guid? id);
        //Task<RSSDto> GetRssById(Guid? id);
        //Task<RSSDto> EditRss(RSSDto rssDto);
        //Task RemoveRangeRss(IEnumerable<RSSDto> rSSDtos);
        //Task AddRangeNews(IEnumerable<RSSDto> rss);
    }
}
