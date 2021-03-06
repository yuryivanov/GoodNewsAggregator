﻿using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodNewsAggregator.Models.ViewModels.News
{
    public class CreateNewsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Text { get; set; }
        public double? GoodnessCoefficient { get; set; }

        public Guid? RSS_Id { get; set; }

        public SelectList Sources { get; set; }       
    }
}