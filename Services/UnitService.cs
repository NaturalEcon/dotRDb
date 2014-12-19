using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuantityTypes;

namespace dotRDb.Web.Services
{
    public class UnitService
    {
        public bool TryParse(string quantity, out IQuantity quantityType)
        {
            quantityType = null;
            return false;
        }
    }
}