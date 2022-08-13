
using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DyeingCapacity
    
    public class DyeingCapacity : BusinessObject
    {
        public DyeingCapacity()
        {
            DyeingCapacityID = 0;
            DyeingType = EumDyeingType.None;
            DyeingTypeInt = 0;
            ProductionHour = 0;
            ProductionCapacity = 0;
            MUnitId = 0;
            CapacityPerHour = 0;
            Remarks = "";
            BaseProductID = 0;
           
            ErrorMessage = "";
            ProdForeCasts = new List<MgtDBObj>();
        }

        #region Properties
        public int DyeingCapacityID { get; set; }
        public EumDyeingType DyeingType { get; set; }
        public int DyeingTypeInt { get; set; }
        public double ProductionHour { get; set; }
        public double ProductionCapacity { get; set; }
        public int MUnitId { get; set; }
        public double CapacityPerHour { get; set; }
        public string Remarks { get; set; }
        public int BaseProductID { get; set; }//new field
        public string ProductName { get; set; }
       
        public string ErrorMessage { get; set; }
        #endregion
        public string MUSymbol { get; set; }
        #region Derived Property
        public List<DyeingCapacity> DyeingCapacitys { get; set; }
        public List<MgtDBObj> ProdForeCasts { get; set; }
        public Company Company { get; set; }

        public string DyeingTypeSt
        {
            get
            {
                return EnumObject.jGet(this.DyeingType);
            }
        }
        
        #endregion

        #region Functions
        public static List<DyeingCapacity> Gets(long nUserID)
        {
            return DyeingCapacity.Service.Gets(nUserID);
        }
        public static List<DyeingCapacity> Gets(string sSQL, Int64 nUserID)
        {
            return DyeingCapacity.Service.Gets(sSQL, nUserID);
        }     
        public DyeingCapacity Get(int nId, long nUserID)
        {
            return DyeingCapacity.Service.Get(nId,nUserID);
        }               
        public DyeingCapacity Save(long nUserID)
        {
            return DyeingCapacity.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return DyeingCapacity.Service.Delete(nId, nUserID);
        }
      
        #endregion

        #region ServiceFactory
        internal static IDyeingCapacityService Service
        {
            get { return (IDyeingCapacityService)Services.Factory.CreateService(typeof(IDyeingCapacityService)); }
        }
        #endregion
    }
    #endregion

    #region IDyeingCapacity interface
    
    public interface IDyeingCapacityService
    {        
        DyeingCapacity Get(int id, long nUserID);        
        List<DyeingCapacity> Gets(long nUserID);
        List<DyeingCapacity> Gets(string sSQL, Int64 nUserID);        
        string Delete(int id, long nUserID);        
        DyeingCapacity Save(DyeingCapacity oDyeingCapacity, long nUserID);
        
    }
    #endregion
}