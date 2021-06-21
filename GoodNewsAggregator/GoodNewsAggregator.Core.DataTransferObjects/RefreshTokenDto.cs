using System;

namespace GoodNewsAggregator.Core.DataTransferObjects
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
    }
}
