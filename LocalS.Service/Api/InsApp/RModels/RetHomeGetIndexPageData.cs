﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalS.Service.Api.InsApp
{
    public class RetHomeGetIndexPageData
    {
        public RetHomeGetIndexPageData()
        {
            this.LNavGrids = new List<LNavGridModel>();
        }

        public List<LNavGridModel> LNavGrids { get; set; }
    }
}