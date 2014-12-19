using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using dotRDb.Web.Models;

namespace dotRDb.Web.Services
{
    public static class ContextManager
    {
        public static dotRDbEntities EntityContext
        {
            get
            {
                var key = "Entity_" + HttpContext.Current.GetHashCode().ToString("x");
                var context = (dotRDbEntities) HttpContext.Current.Items[key];
                if (context == null)
                {
                    context = new dotRDbEntities();
                    HttpContext.Current.Items[key] = context;
                }
                return context;
            }
        }
    }
}