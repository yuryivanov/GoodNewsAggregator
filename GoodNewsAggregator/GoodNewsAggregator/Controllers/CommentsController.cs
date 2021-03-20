﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Core;

namespace GoodNewsAggregator.Controllers
{
    public class CommentsController : Controller
    {
        private readonly GoodNewsAggregatorContext _context;

        public CommentsController(GoodNewsAggregatorContext context)
        {
            _context = context;
        }       
    }
}