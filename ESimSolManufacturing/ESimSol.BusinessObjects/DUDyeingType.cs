using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.BusinessObjects
{
    #region DUDyeingType
    public class DUDyeingType :BusinessObject
    {
        public DUDyeingType()
        {
            DUDyeingTypeID = 0;
            DyeingType = EumDyeingType.None;
            Name = string.Empty;
            Activity = true;
            Params = string.Empty;
            DyeingTypeInt = 0;
            ErrorMessage = string.Empty;
            Capacity = 0;
        }
        #region properties
        public int DUDyeingTypeID { get; set; }
        public EumDyeingType DyeingType { get; set; }
        public int DyeingTypeInt { get; set; }
        public string Name {get; set;}
        public double Capacity {get; set;}
        public bool Activity {get; set;}
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived Properties
        public string DyeingTypeStr { get { return this.DyeingType.ToString(); } }
        #endregion
        #region Functions

        public static DUDyeingType Get(int nDUDyeingTypeID, long nUserID)
        {
            return DUDyeingType.Service.Get(nDUDyeingTypeID, nUserID);
        }
        public static List<DUDyeingType> Gets(string sSQL, long nUserID)
        {
            return DUDyeingType.Service.Gets(sSQL, nUserID);
        }
        public static List<DUDyeingType> GetsActivity(bool bActivity, long nUserID)
        {
            return DUDyeingType.Service.GetsActivity(bActivity, nUserID);
        }
        public DUDyeingType IUD(int nDBOperation, long nUserID)
        {
            return DUDyeingType.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUDyeingTypeService Service
        {
            get { return (IDUDyeingTypeService)Services.Factory.CreateService(typeof(IDUDyeingTypeService)); }
        }

        #endregion
    }
    #endregion
    #region  DUDyeingType interface
    public interface IDUDyeingTypeService
    {

        DUDyeingType Get(int nDUDyeingTypeID, Int64 nUserID);
        List<DUDyeingType> Gets(string sSQL, Int64 nUserID);
        List<DUDyeingType> GetsActivity(bool bActivity, Int64 nUserID);
        DUDyeingType IUD(DUDyeingType oDUDyeingType, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
