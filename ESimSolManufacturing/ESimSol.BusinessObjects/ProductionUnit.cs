using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ProductionUnit

    public class ProductionUnit : BusinessObject
    {
        public ProductionUnit()
        {
            ProductionUnitID = 0;
            ProductionUnitType = EnumProductionUnitType.None;
            Name = "";
            ShortName = "";
            RefID = 0;
            FBUID = 0;
            RefName = "";
            ErrorMessage = "";
        }

        #region Properties
        public int ProductionUnitID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public EnumProductionUnitType ProductionUnitType { get; set; }
        public int RefID { get; set; }
        public int FBUID { get; set; }
        public string RefName { get; set; }

        #region Derived Property
        public string ErrorMessage { get; set; }
        public string BUID { get; set; }
        public int ProductionUnitTypeInt
        {
            get
            {
                return (int)this.ProductionUnitType;
            }
        }
        public string ProductionUnitTypeST
        {
            get
            {
                return EnumObject.jGet(this.ProductionUnitType);
            }
        }

        #endregion

        #endregion

        #region Functions
        public static List<ProductionUnit> Gets(long nUserID)
        {
            return ProductionUnit.Service.Gets(nUserID);
        }
        public static List<ProductionUnit> Gets(int nBUID, long nUserID)
        {
            return ProductionUnit.Service.Gets(nBUID, nUserID);
        }
        public static List<ProductionUnit> Gets(string sSQL, long nUserID)
        {
            return ProductionUnit.Service.Gets(sSQL, nUserID);
        }
        public ProductionUnit Get(int id, long nUserID)
        {
            return ProductionUnit.Service.Get(id, nUserID);
        }
        public ProductionUnit GetByType(int nProductionUnitType, long nUserID)
        {
            return ProductionUnit.Service.GetByType(nProductionUnitType, nUserID);
        }

        public ProductionUnit Save(long nUserID)
        {
            return ProductionUnit.Service.Save(this, nUserID);
        }
      
        public string Delete(long nUserID)
        {
            return ProductionUnit.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IProductionUnitService Service
        {
            get { return (IProductionUnitService)Services.Factory.CreateService(typeof(IProductionUnitService)); }
        }
        #endregion
    }
    #endregion


    #region IProductionUnit interface

    public interface IProductionUnitService
    {
        ProductionUnit Get(int id, Int64 nUserID);
        ProductionUnit GetByType(int nProductionUnitType, Int64 nUserID);
        List<ProductionUnit> Gets(string sSQL, long nUserID);
        List<ProductionUnit> Gets(Int64 nUserID);
        List<ProductionUnit> Gets(int nBUID, Int64 nUserID);
        string Delete(ProductionUnit oProductionUnit, Int64 nUserID);
        ProductionUnit Save(ProductionUnit oProductionUnit, Int64 nUserID);
    }
    #endregion
}
