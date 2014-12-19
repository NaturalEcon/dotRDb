using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dotRDb.Web.Models;
using dotRDb.Web.Models.ViewModel;
using dotRDb.Web.Services;
using Newtonsoft.Json;
using QuantityTypes;

namespace dotRDb.Web.Controllers
{
    public class DataController : Controller
    {
        private const int ItemsPerPage = 20;
        private readonly dotRDbEntities _db = ContextManager.EntityContext;

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        [HttpGet]
        public DataView GetDatum(int datumId)
        {
            return _db.Data.Where(d => d.DatumId == datumId)
                           .Select(d => new DataView
                           {
                               DatumId = d.DatumId,
                               Name = d.Name,
                               Value = d.Value ?? 0.0,
                               Unit = d.Unit,
                               CreatedOn = d.DateCreated ?? DateTime.Now
                           })
                           .Single();
        }

        [HttpPost]
        public int AddDatum(Datum datum)
        {
            datum.DateCreated = DateTime.Now;
            
            _db.Data.Add(datum);
            return _db.SaveChanges();
        }

        //[HttpGet]
        //public JsonNetResult GetUnitTypes()
        //{
        //    QuantityType q = new QuantityType();
        //}

        [HttpGet]
        public IEnumerable<DataView> GetResourceData(int resourceId)
        {
            var nodeId = _db.Resources.Single(r => r.ResourceId == resourceId)
                                      .NodeId;
            return  _db.Data.Where(v => v.DatumType == DataTypes.NodeData)
                               .Where(v => v.NodeId == nodeId)
                               .Select(d => new DataView
                               {
                                   DatumId = d.DatumId,
                                   Name = d.Name,
                                   Value = d.Value ?? 0.0,
                                   Unit = d.Unit,
                                   CreatedOn = d.DateCreated ?? DateTime.Now
                               });
        }
    }
}
