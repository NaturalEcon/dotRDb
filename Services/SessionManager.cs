using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using dotRDb.Web.Models;

namespace dotRDb.Web.Services
{
    public class SessionManager
    {
        public static IQueryable LastResultSet
        {
            get
            {
                var key = "Session_" + HttpContext.Current.GetHashCode().ToString("x");
                return (IQueryable) HttpContext.Current.Items[key];
            }
            set
            {
                var key = "Session_" + HttpContext.Current.GetHashCode().ToString("x");
                HttpContext.Current.Items[key] = value;
            }
        }

        public static Node LastQueriedNode
        {
            get
            {
                var key = "Node_" + HttpContext.Current.GetHashCode().ToString("x");
                return (Node)HttpContext.Current.Items[key];
            }
            set
            {
                var key = "Node_" + HttpContext.Current.GetHashCode().ToString("x");
                HttpContext.Current.Items[key] = value;
            }
        }

        private static DateTime TimeLastWritten
        {
            get
            {
                var key = "LastWritten_" + HttpContext.Current.GetHashCode().ToString("x");
                return (DateTime?)HttpContext.Current.Items[key] ?? new DateTime(1,1,1);
            }
            set
            {
                var key = "LastWritten_" + HttpContext.Current.GetHashCode().ToString("x");
                HttpContext.Current.Items[key] = value;
            }
        }
        private static readonly TimeSpan TimeToHoldPages = new TimeSpan(0,0,5,0);
        public static IQueryable<Resource> FirstFivePages
        {
            get
            {
                if (DateTime.Now.CompareTo(TimeLastWritten.Add(TimeToHoldPages)) >= 0)
                {
                    var key = "FirstFive_" + HttpContext.Current.GetHashCode().ToString("x");
                    return (IQueryable<Resource>)HttpContext.Current.Items[key];
                }
                return null;
            }
            set
            {
                TimeLastWritten = DateTime.Now;
                var key = "FirstFive_" + HttpContext.Current.GetHashCode().ToString("x");
                HttpContext.Current.Items[key] = value;
            }
        }
}
}