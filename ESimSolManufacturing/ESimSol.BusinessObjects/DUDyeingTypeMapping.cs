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
    #region DUDyeingTypeMapping
    public class DUDyeingTypeMapping
    {
        public DUDyeingTypeMapping()
        {
        DyeingTypeMappingID =0;
        DyeingType =EumDyeingType.None;
        ProductID =0;
        Unit =0;
        ErrorMessage = string.Empty;
        Params = string.Empty;
        DUDyeingTypeMappings = new List<DUDyeingTypeMapping>();
        DyeingTypeInt = 0;
        }
        #region Properties
        public int DyeingTypeMappingID { get; set; }
        public EumDyeingType DyeingType { get; set; }
        public int ProductID { get; set; }
        public double Unit { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }

        #endregion
        #region Derived properties
        public int DyeingTypeInt { get; set; }
        public string ProductName { get; set; }

        public string DyeingTypeStr { get { return this.DyeingType.ToString(); } }
        public List<DUDyeingTypeMapping> DUDyeingTypeMappings { get; set; }
        #endregion
        #region Functions

        public static DUDyeingTypeMapping Get(int nDUDyeingTypeID, long nUserID)
        {
            return DUDyeingTypeMapping.Service.Get(nDUDyeingTypeID, nUserID);
        }
        public static List<DUDyeingTypeMapping> Gets(string sSQL, long nUserID)
        {
            return DUDyeingTypeMapping.Service.Gets(sSQL, nUserID);
        }
        public DUDyeingTypeMapping IUD(int nDBOperation, long nUserID)
        {
            return DUDyeingTypeMapping.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUDyeingTypeMappingService Service
        {
            get { return (IDUDyeingTypeMappingService)Services.Factory.CreateService(typeof(IDUDyeingTypeMappingService)); }
        }

        #endregion
    }
    #endregion
    #region  DUDyeingTypeMapping interface
    public interface IDUDyeingTypeMappingService
    {

        DUDyeingTypeMapping Get(int nDyeingTypeMappingID, Int64 nUserID);
        List<DUDyeingTypeMapping> Gets(string sSQL, Int64 nUserID);
        DUDyeingTypeMapping IUD(DUDyeingTypeMapping oDUDyeingTypeMapping, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
