﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinSdk
{
    public class WxApiMediaUploadNewsModel
    {

        public WxApiMediaUploadNewsModel()
        {
            this.articles = new List<Article>();
        }


        public List< Article> articles { get; set; }


    }
}
