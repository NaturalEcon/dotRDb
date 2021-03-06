//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace dotRDb.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Node
    {
        public Node()
        {
            this.Actors = new HashSet<Actor>();
            this.OutgoingEdges = new HashSet<Edge>();
            this.IncomingEdges = new HashSet<Edge>();
            this.Processes = new HashSet<Process>();
            this.Resources = new HashSet<Resource>();
            this.Data = new HashSet<Datum>();
        }
    
        public int NodeId { get; set; }
        public Nullable<int> NodeTypeId { get; set; }
    
        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<Edge> OutgoingEdges { get; set; }
        public virtual ICollection<Edge> IncomingEdges { get; set; }
        public virtual NodeType NodeType { get; set; }
        public virtual ICollection<Process> Processes { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<Datum> Data { get; set; }
    }
}
