using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using dotRDb.Web.Models;
using dotRDb.Web.Models.ViewModel;
using dotRDb.Web.Services;
using Newtonsoft.Json;

namespace dotRDb.Web.Controllers
{
    public class GraphController : ApiController
    {
        private const int ItemsPerPage = 20;
        private readonly dotRDbEntities _db = ContextManager.EntityContext;

        private IQueryable _lastResultSet = SessionManager.LastResultSet;
        private Node _lastNode = SessionManager.LastQueriedNode;

        [System.Web.Mvc.HttpPost]
        public int AddEdge(Edge edge)
        {
            
            _db.Edges.Add(edge);
            var breakpoint = 0;
            return _db.SaveChanges();
        }

        [System.Web.Mvc.HttpPost]
        public bool EdgeIsNew(Edge edge)
        {
            if (edge == null || edge.ToNodeId == 0 || edge.FromNodeId == 0)
            {
                return false;
            }
            return !_db.Edges.Any(e => e.FromNodeId == edge.FromNodeId && e.ToNodeId == edge.ToNodeId);
        }

        [System.Web.Mvc.HttpGet]
        public IEnumerable<NodeTypeView> GetNodeTypes()
        {
            return _db.NodeTypes.Select(nodeType => new NodeTypeView
                                                    {
                                                        NodeTypeId = nodeType.NodeTypeId,
                                                        TypeName = nodeType.TypeName
                                                    }
                                        )
                                        .ToList();
        }

        [System.Web.Mvc.HttpGet]
        public int GetNodeType(int nodeId)
        {
            return  _db.Nodes.Single(v => v.NodeId == nodeId)
                             .NodeTypeId ?? -1;
        }

        [System.Web.Mvc.HttpGet]
        public IEnumerable<EdgeTypeView> GetEdgeTypes(int anyType = -1, int fromType = -1, int toType = -1)
        {
            IQueryable<EdgeType> data = _db.EdgeTypes.AsQueryable();
            if (fromType != -1)
            {
                data = data.Where(e => e.FromType == fromType);
            }
            if (toType != -1)
            {
                data = data.Where(e => e.ToType == toType);
            }
            if (anyType != -1)
            {
                data = data.Where(e => e.ToType == anyType || e.FromType == anyType);
            }
            return data.Select(t => new EdgeTypeView
                        {
                            DisplayName = t.DisplayName,
                            EdgeTypeId = t.EdgeTypeId
                        })
                        .ToList();
        }

        [System.Web.Mvc.HttpGet]
        public NodeView GetNode(int nodeId, bool invalid = false)
        {
            if (invalid || _lastNode == null || _lastNode.NodeId != nodeId)
            {
                _lastNode = _db.Nodes.Single(v => v.NodeId == nodeId);
            }
            return new NodeView
            {
                Name = _lastNode.Name,
                NodeId = _lastNode.NodeId
            };
        }

        public IEnumerable<DataView> GetNodeData(int nodeId)
        {
            if (_lastNode == null || _lastNode.NodeId != nodeId)
            {
                GetNode(nodeId, true);
            }
            if (_lastNode == null)
            {
                return null;
            }
            return  _db.Data.Where(datum => datum.DatumType == DataTypes.NodeData)
                            .Where(datum => datum.NodeId == nodeId)
                            .Select(datum => new DataView
                            {
                                DatumId = datum.DatumId,
                                Name = datum.Name,
                                Value = datum.Value ?? 0.0,
                                Unit = datum.Unit,
                                CreatedOn = datum.DateCreated ?? DateTime.Now
                            })
                            .ToList();
        }

        public IEnumerable<EdgeView> GetEdgeSet(int nodeId, int edgeTypeId)
        {
            return _db.Edges.Where(e => e.FromNodeId == nodeId && e.EdgeTypeId == edgeTypeId)
                             .Select(e => new EdgeView
                             {
                                 Name = _db.Resources.FirstOrDefault(v => v.NodeId == e.ToNodeId).Name
                             })
                             .ToList();
        }

        [System.Web.Mvc.HttpGet]
        public JsonNetResult GetNodeAndData(int nodeId)
        {
            //Return:
            // Node,
            // About,
            // Edges separated by type
            var node = _db.Nodes.SingleOrDefault(v => v.NodeId == nodeId);
            if (node == null)
            {
                return null;
            }
            var nodeData = _db.Data.Where(datum => datum.DatumType == DataTypes.NodeData)
                .Where(datum => datum.NodeId == nodeId)
                .ToList();
            var aboutType = node.NodeTypeId;

            #region Resource

            if (aboutType == NodeTypes.Resource)
            {
                var about = _db.Resources.SingleOrDefault(r => r.NodeId == nodeId);
                if (about == null)
                {
                    return null;
                }
                var subclasses = about.Node.Subclasses
                                           .Select(sub => sub.Resources.First());
                var superclasses = _db.Edges.Where(e => e.ToNodeId == about.NodeId && e.EdgeTypeId == EdgeTypes.ResourceSubclass)
                                            .Select(e => _db.Resources.FirstOrDefault(r => r.NodeId == e.FromNodeId));
                var data = new Tuple<Node,
                    int,
                    Resource,
                    List<Datum>,
                    List<KeyValuePair<string, IEnumerable<Resource>>>
                    >
                    (
                    node,
                    aboutType.Value,
                    about,
                    nodeData,
                    new List<KeyValuePair<string, IEnumerable<Resource>>>
                    {
                        new KeyValuePair<string, IEnumerable<Resource>>(
                            "subclasses",
                            subclasses
                            ),
                        new KeyValuePair<string, IEnumerable<Resource>>(
                            "superclasses",
                            superclasses
                            )
                    });
                return new JsonNetResult
                {
                    Data = data
                };
            }

            #endregion

            return null;
        }

        [System.Web.Mvc.HttpGet]
        public IEnumerable<NodeView> SearchNodes(string s, string type, int maxResults = 5)
        {
            IQueryable<Node> data = _db.Resources.Where(r => r.Name.Contains(s))
                                                 .Select(r => r.Node)
                                                 .Take(maxResults);
            if (!string.IsNullOrEmpty(type))
            {
                data = data.Where(r => r.NodeType.TypeName == type);
            }
            return data.Select(v => new NodeView
            {
                Name = v.Name,
                NodeId = v.NodeId,
                NodeTypeId = v.NodeTypeId.Value
            })
            .ToList();
        }

        [System.Web.Mvc.HttpGet]
        public IEnumerable<NodeView> GetNodesForNewEdge(string s, string field, int toNodeId = -1, int toNodeTypeId = -1, int fromNodeId = -1, int fromNodeTypeId = -1, int edgeTypeId = -1, int maxResults = 5)
        {
            var paramCheck = !string.IsNullOrEmpty(s) ||
                              toNodeId == -1 || fromNodeId == -1 ||
                              edgeTypeId == -1 ||
                              EdgeTypes.Heterogeneous.Contains(edgeTypeId);
            if (!paramCheck || string.IsNullOrEmpty(field))
            {
                return null;
            }

            if (field == "from")
            {
                return GetNodes(s, true, toNodeId, fromNodeTypeId, edgeTypeId, maxResults).Select(v => new NodeView
                {
                    Name = v.Name,
                    NodeId = v.NodeId,
                    NodeTypeId = v.NodeTypeId.Value
                });
            }
            if (field == "to")
            {
                return GetNodes(s, false, fromNodeId, fromNodeTypeId, edgeTypeId, maxResults).Select(v => new NodeView
                {
                    Name = v.Name,
                    NodeId = v.NodeId,
                    NodeTypeId = v.NodeTypeId.Value
                });
            }

            return null;
        }

        private IQueryable<Node> GetNodes(string s, bool isFrom = true, int otherNodeId = -1, int otherNodeTypeId = -1, int edgeTypeId = -1,
            int maxResults = 5)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            IQueryable<Node> nodes;
            if (_lastResultSet != null && ((IQueryable<Node>)_lastResultSet).Any(r => r.Name.StartsWith(s)))
            {
                nodes = (IQueryable<Node>)_lastResultSet;
            }
            else
            {
                nodes = _db.Resources.Where(r => r.Name.StartsWith(s))
                    .Select(r => r.Node)
                    .Concat(_db.Processes.Where(p => p.Name.StartsWith(s))
                        .Select(r => r.Node))
                    .Concat(_db.Actors.Where(a => a.Name.StartsWith(s))
                        .Select(a => a.Node));
            }
            //If a type Id is selected, ensure supplied to value is valid and filter
            if (EdgeTypes.AllTypes.Contains(edgeTypeId))
            {
                var edgeType = _db.EdgeTypes.Single(e => e.EdgeTypeId == edgeTypeId);
                var firstMatchType = isFrom ? edgeType.ToType.Value : edgeType.FromType.Value;
                var secondMatchType = isFrom ? edgeType.FromType.Value : edgeType.ToType.Value;

                if (otherNodeId != -1 && otherNodeTypeId != firstMatchType)
                {
                    return null;
                }

                nodes = nodes.Where(v => v.NodeTypeId == secondMatchType);
            }
            //If a To node is selected, perform sanity check and filter
            if (otherNodeId != -1)
            {
                var possibleEdgeTypes =
                    _db.EdgeTypes.Where(e => (isFrom ? e.ToType : e.FromType) == otherNodeTypeId);
                var possibleNodeTypes = possibleEdgeTypes.Select(e => isFrom ? e.FromType : e.ToType);
                var possibleEdgeTypeIds = possibleEdgeTypes.Select(e => isFrom ? e.ToType : e.FromType);
                if (!possibleEdgeTypeIds.Contains(otherNodeTypeId))
                {
                    return null;
                }

                nodes = nodes.Where(v => possibleNodeTypes.Contains(v.NodeTypeId));
            }
            //Filter by search string
            _lastResultSet = nodes;
            return nodes.Take(maxResults);
        }
    }
}