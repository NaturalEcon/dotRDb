using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotRDb.Web.Models.ViewModel
{
    public class EdgeView
    {
        public string Name { get; set; }
    }

    public class EdgeTypeView
    {
        public string DisplayName { get; set; }
        public int EdgeTypeId { get; set; }
    }
}