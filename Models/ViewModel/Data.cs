using System;

namespace dotRDb.Web.Models.ViewModel
{
    public class DataView
    {
        public int DatumId { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ObservedOn { get; set; }
    }
}