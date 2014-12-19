using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using QuantityTypes;

namespace dotRDb.Web.Models
{
    public static class NodeTypes
    {
        public const int Resource = 1;
        public const int Process = 2;
        public const int Actor = 3;
    }

    public static class EdgeTypes
    {
        public const int ResourceSubclass = 1;
        public const int ProcessSubclass = 2;
        public const int ActorSubclass = 3;
        public const int ResourceDependency = 4;
        public const int ProcessDependency = 5;
        public const int ActorDependency = 6;
        public const int ProcessOutput = 7;
        public const int ProcessPath = 8;
        public const int ActorOrganization = 9;
        public const int ActorTransfer = 10;

        public static readonly int[] AllTypes =
        {
            ResourceSubclass,
            ProcessSubclass,
            ActorSubclass,
            ResourceDependency,
            ProcessDependency,
            ActorDependency,
            ProcessOutput,
            ProcessPath,
            ActorOrganization,
            ActorTransfer
        };

        public static readonly int[] Heterogeneous =
        {
            ProcessDependency,
            ProcessOutput,
            ActorTransfer
        };
    }

    public partial class Node
    {
        #region Edge Selectors

        private readonly Func<Edge, bool> depCondition = e => (e.EdgeType.EdgeTypeId == EdgeTypes.ActorDependency
                                                            || e.EdgeType.EdgeTypeId == EdgeTypes.ProcessDependency
                                                            || e.EdgeType.EdgeTypeId == EdgeTypes.ResourceDependency);

        private readonly Func<Edge, bool> subCondition = e => (e.EdgeType.EdgeTypeId == EdgeTypes.ActorSubclass
                                                            || e.EdgeType.EdgeTypeId == EdgeTypes.ProcessSubclass
                                                            || e.EdgeType.EdgeTypeId == EdgeTypes.ResourceSubclass);

        public string Name
        {
            get
            {
                if (NodeTypeId == NodeTypes.Resource)
                {
                    return Resources.First().Name;
                }
                if (NodeTypeId == NodeTypes.Process)
                {
                    return Processes.First().Name;
                }
                if (NodeTypeId == NodeTypes.Actor)
                {
                    return Actors.First().Name;
                }
                return "";
            }
        }

        public bool IsSameType(Node other)
        {
            return NodeTypeId == other.NodeTypeId;
        }

        public ICollection<Edge> BackwardDependencies
        {
            get { return IncomingEdges.Where(depCondition).ToList(); }
        }

        public ICollection<Edge> ForwardDependencies
        {
            get { return OutgoingEdges.Where(depCondition).ToList(); }
        }

        public IEnumerable<Node> Subclasses
        {
            get { return this.OutgoingEdges.Where(subCondition)
                                           .Select(e => e.To); }
        }

        public IEnumerable<Node> Superclasses 
        {
            get { return this.IncomingEdges.Where(subCondition)
                                           .Select(e => e.From); } 
        }

        public ICollection<Edge> ProcessOutputs
        {
            get { return OutgoingEdges.Where(edge => edge.EdgeType.EdgeTypeId == EdgeTypes.ProcessOutput).ToList(); }
        }

        public ICollection<Edge> ProcessPaths
        {
            get
            {
                Func<Edge, bool> cond = edge => edge.EdgeTypeId == EdgeTypes.ProcessPath;
                return new List<Edge>( IncomingEdges.Where(cond)
                                                    .Union(OutgoingEdges.Where(cond) ) );
            }
        }
        #endregion

        public ICollection<Node> ConnectedNodes
        {
            get
            {
                var incoming = IncomingEdges.Select(edge => edge.From);
                var outgoing = OutgoingEdges.Select(edge => edge.To);
                return new List<Node>(incoming.Concat(outgoing));
            }
        }
    }

    public partial class Edge
    {

        public bool IsSameType(Edge other)
        {
            return EdgeType.EdgeTypeId == other.EdgeType.EdgeTypeId;
        } 
    }
}