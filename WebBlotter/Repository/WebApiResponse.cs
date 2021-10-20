using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Repository
{
    public class WebApiResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }

    }
}