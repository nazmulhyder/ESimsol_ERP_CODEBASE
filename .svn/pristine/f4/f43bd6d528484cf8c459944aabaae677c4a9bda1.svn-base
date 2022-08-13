using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ProductionTimeSetup
    
    public class ProductionTimeSetup : BusinessObject
    {
        public ProductionTimeSetup()
        {
            ProductionTimeSetupID = 0;
            BUID  = 0;
            OffDay = "Friday";
            RegularTime  = 10;
            OverTime  = 2;
            BUName = "";
            BUShortName = "";
            ErrorMessage = "";

        }

        #region Properties
        public int ProductionTimeSetupID { get; set; }
        public string BUName { get; set; }
        public string BUShortName { get; set; }
        public string OffDay { get; set; }
        public int ProductionTimeSetupTypeInt { get; set; }
        public int BUID { get; set; }
        public double RegularTime { get; set; }
        public double OverTime { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        
        public string RegularTimeInString
        {
            get
            {
                return this.RegularTime.ToString()+" Hours";
            }
        }
        public string OverTimeInString
        {
            get
            {
                return this.OverTime.ToString() + " Hours";
            }
        }

         
        #endregion

        #region Functions

        public static List<ProductionTimeSetup> Gets(long nUserID)
        {
            return ProductionTimeSetup.Service.Gets(nUserID);
        }

        public static List<ProductionTimeSetup> Gets(string sSQL, long nUserID)
        {
            return ProductionTimeSetup.Service.Gets(sSQL, nUserID);
        }
        

        public ProductionTimeSetup Get(int id, long nUserID)
        {
            return ProductionTimeSetup.Service.Get(id, nUserID);
        }

        public ProductionTimeSetup GetByBU(int buid, long nUserID)//id is buid
        {
            return ProductionTimeSetup.Service.GetByBU(buid, nUserID);
        }
        public ProductionTimeSetup Save(long nUserID)
        {
            return ProductionTimeSetup.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ProductionTimeSetup.Service.Delete(id, nUserID);
        }        
        #endregion

        #region ServiceFactory
        internal static IProductionTimeSetupService Service
        {
            get { return (IProductionTimeSetupService)Services.Factory.CreateService(typeof(IProductionTimeSetupService)); }
        }

        #endregion
    }
    #endregion

    #region IProductionTimeSetup interface
     
    public interface IProductionTimeSetupService
    {
        ProductionTimeSetup Get(int id, Int64 nUserID);
        ProductionTimeSetup GetByBU(int buid, Int64 nUserID);        
         
        List<ProductionTimeSetup> Gets(Int64 nUserID);
         
        List<ProductionTimeSetup> Gets(string sSQL, Int64 nUserID);        
         
        string Delete(int id, Int64 nUserID);
         
        ProductionTimeSetup Save(ProductionTimeSetup oProductionTimeSetup, Int64 nUserID);
    }
    #endregion
}
