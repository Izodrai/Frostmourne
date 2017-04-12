﻿using Frostmourne_basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAPI.Models
{
    public class Response
    {
        public Error Error { get; set; }
        public List<Bid> Bids { get; set; }
        public List<Symbol> Symbols { get; set; }

        public Response(Error _err, List<Bid> _bids, List<Symbol> _symbols)
        {
            this.Error = _err;
            this.Bids = _bids;
            this.Symbols = _symbols;
        }
    }
}