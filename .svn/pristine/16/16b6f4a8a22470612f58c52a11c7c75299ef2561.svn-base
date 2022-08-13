
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
    public class DyeingPeriodConfig:BusinessObject
    {
        public DyeingPeriodConfig()
        {
            DyeingPeriodConfigID = 0;
            ProductID = 0;
            DyeingCapacityID = 0;
            ReqDyeingPeriod = 0;
            Remarks = "";
            DyeingType = EumDyeingType.None;
            ProductCode = "";
            ProductName = "";
            BaseProductName = "";
        }
        #region Property
        public int DyeingPeriodConfigID { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BaseProductName { get; set; }
        public int DyeingCapacityID { get; set; }
        public double ReqDyeingPeriod { get; set; }
        public string Remarks { get; set; }
        public List<DyeingPeriodConfig> DyeingPeriodConfigs { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region derived property
        public EumDyeingType DyeingType { get; set; }
        
        public string DyeingTypeSt
        {
            get
            {
                return EnumObject.jGet(this.DyeingType);
            }
        }
        public string ReqDyeingPeriodSt
        {
            get
            {
                return Global.MillionFormat(this.ReqDyeingPeriod) + " Times of " + this.BaseProductName;
            }
        }
        #endregion

        #region Functions
        public static List<DyeingPeriodConfig> Gets(long nUserID)
        {
            return DyeingPeriodConfig.Service.Gets(nUserID);
        }
        public static List<DyeingPeriodConfig> Gets(string sSQL, Int64 nUserID)
        {
            return DyeingPeriodConfig.Service.Gets(sSQL, nUserID);
        }
        public DyeingPeriodConfig Get(int nId, long nUserID)
        {
            return DyeingPeriodConfig.Service.Get(nId, nUserID);
        }
        public DyeingPeriodConfig Save(long nUserID)
        {
            return DyeingPeriodConfig.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return DyeingPeriodConfig.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDyeingPeriodConfigService Service
        {
            get { return (IDyeingPeriodConfigService)Services.Factory.CreateService(typeof(IDyeingPeriodConfigService)); }
        }
        #endregion
    }
    #region IDyeingCapacity interface

    public interface IDyeingPeriodConfigService
    {
        DyeingPeriodConfig Get(int id, long nUserID);
        List<DyeingPeriodConfig> Gets(long nUserID);
        List<DyeingPeriodConfig> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        DyeingPeriodConfig Save(DyeingPeriodConfig oDyeingCapacity, long nUserID);
        
    }
    #endregion
}
