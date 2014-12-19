using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using dotRDb.Web.Models;
using dotRDb.Web.Models.ViewModel;
using dotRDb.Web.Services;
using EntityFramework.Extensions;
using Newtonsoft.Json;

namespace dotRDb.Web.Controllers
{
    public class ResourceController : ApiController
    {
        private const int ItemsPerPage = 20;
        private readonly dotRDbEntities _db = ContextManager.EntityContext;
        private IQueryable _lastResultSet = SessionManager.LastResultSet;
        private IQueryable<Resource> _firstFivePages = SessionManager.FirstFivePages;
        
        
        public JsonNetResult AllResources()
        {
            return new JsonNetResult
            {
                Data = _db.Resources.ToList(),
            };
        }

        [HttpGet]
        public IEnumerable<ResourceView> ResourcePage(int p = 1)
        {
            return _db.Resources.OrderBy(r => r.ResourceId)
                                .Skip((p - 1) * ItemsPerPage)
                                .Take(ItemsPerPage)
                                .Select(r =>
                                    new ResourceView
                                    {
                                        Name = r.Name,
                                        Description = r.Description,
                                        ResourceId = r.ResourceId,
                                        NodeId = r.NodeId
                                    });
        }

        [HttpGet]
        public JsonNetResult GetResource(int resourceId)
        {
            return new JsonNetResult
            {
                Data = _db.Resources.FirstOrDefault(r => r.ResourceId == resourceId),
            };
        }

        [HttpGet]
        public JsonNetResult SearchResources(string s)
        {
            return new JsonNetResult
            {
                Data = _db.Resources.Where(r => r.Name.Contains(s))
            };
        }

        [HttpPost]
        public int DeleteResource(int resourceId)
        {

            var resources = _db.Resources.Where(r => r.ResourceId == resourceId);
            var resource = resources.Single();
            var conf = _db.Edges.Where(e => e.ToNodeId == resource.NodeId || e.FromNodeId == resource.NodeId)
                                .Delete();
            conf += _db.Nodes.Where(v => v.NodeId == resource.NodeId)
                             .Delete();
            conf += resources.Delete();
            return conf;
        }

        [HttpGet]
        public JsonNetResult ResourceList(List<int> resourceIds)
        {
            var resources = _db.Resources.Where(r => resourceIds.Contains(r.ResourceId));

            return new JsonNetResult
            {
                Formatting = Formatting.Indented,
                Data = resources,
            };
        }


        [HttpGet]
        public JsonNetResult IsNew(string resourceName)
        {
            var matches = _db.Resources.Where(r => r.Name == resourceName)
                                       .Select(match => match.ResourceId)
                                       .ToList();
            var data = new Tuple<string, List<int>>("true", new List<int>());
            if (matches.Any())
            {
                data = new Tuple<string, List<int>>("false", matches);
            }
            return new JsonNetResult
            {
                Data = data
            };
        }

        [HttpPost]
        public void AddResource(Resource resource)
        {
            if (resource.Name == null)
            {
                return;
            }
            var myNode = new Node
            {
                NodeTypeId = NodeTypes.Resource
            };
            _db.Nodes.Add(myNode);
            var savedResource = new Resource
            {
                Name = SanitizerService.SanitizeForSql(resource.Name),
                Description = SanitizerService.SanitizeForSql(resource.Description),
                Node = myNode
            };
            _db.Resources.Add(savedResource);
            _db.SaveChanges();
        }

        [HttpPost]
        public void AddResources(IEnumerable<Resource> resources)
        {
            bool containsDirtyInputs = resources.Any(
                resource => SanitizerService.InputsNeedSanitizing(new[]
                {
                    resource.Name, 
                    resource.Description
                })
            );
            if (containsDirtyInputs) return;
            using (var db = new dotRDbEntities())
            {
                db.Resources.AddRange(resources);
                db.SaveChanges();
            }
        }
    }
}