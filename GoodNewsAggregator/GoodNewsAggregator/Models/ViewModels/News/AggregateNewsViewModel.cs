using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodNewsAggregator.Models.ViewModels.News
{
    public class AggregateNewsViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; } = "Гость";
    }
}