using System;

namespace dotRDb.Web.Models.ViewModel
{
    public class NodeView
    {
        public int NodeId { get; set; }
        public int NodeTypeId { get; set; }
        public string Name { get; set; }
    }

    public class NodeTypeView
    {
        public int NodeTypeId { get; set; }
        public string TypeName { get; set; }
    }

    public class NodeDataView
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateObserved { get; set; }
    }
}