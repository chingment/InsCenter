﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinSdk.MsgPush
{
    public class LinkEventMsg : BaseEventMsg
    {
        public override string EventKey
        {
            get; set;
        }
    }
}
