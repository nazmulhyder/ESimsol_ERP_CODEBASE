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
    #region GarmentsType
    
    public class GarmentsType : BusinessObject
    {
        public GarmentsType()
        {
            GarmentsTypeID = 0;
            TypeName = "";
            Note = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int GarmentsTypeID { get; set; }
         
        public string TypeName { get; set; }
         
        public string Note { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public static List<GarmentsType> Gets(long nUserID)
        {
            return GarmentsType.Service.Gets( nUserID);
        }

        public GarmentsType Get(int id, long nUserID)
        {
            return GarmentsType.Service.Get(id, nUserID);
        }

        public GarmentsType Save(long nUserID)
        {
            return GarmentsType.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return GarmentsType.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IGarmentsTypeService Service
        {
            get { return (IGarmentsTypeService)Services.Factory.CreateService(typeof(IGarmentsTypeService)); }
        }

        #endregion
    }
    #endregion

    #region IGarmentsType interface
     
    public interface IGarmentsTypeService
    {
         
        GarmentsType Get(int id, Int64 nUserID);
         
        List<GarmentsType> Gets(Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        GarmentsType Save(GarmentsType oGarmentsType, Int64 nUserID);
    }
    #endregion
}
