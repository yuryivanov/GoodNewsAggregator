﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.Controllers
{
    public class RolesController : Controller
    {
        private readonly GoodNewsAggregatorContext _context;

        public RolesController(GoodNewsAggregatorContext context)
        {
            _context = context;
        }       
    }
}