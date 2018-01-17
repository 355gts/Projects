﻿using JoelScottFitness.Web.Attributes;
using System.Web;
using System.Web.Mvc;

namespace JoelScottFitness.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalExceptionAttribute());
        }
    }
}
