using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region FnQCTestGroup
    public class FnQCTestGroup : BusinessObject
    {
        public FnQCTestGroup()
        {
            FnQCTestGroupID = 0;
            Name = "";
            Note = "";
            SLNo = 0;
            ErrorMessage = "";
            //RequestType = EnumProductNature.Hanger;
        }

        #region Property
        public int FnQCTestGroupID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int SLNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions
        public static List<FnQCTestGroup> Gets(long nUserID)
        {
            return FnQCTestGroup.Service.Gets(nUserID);
        }
        public static List<FnQCTestGroup> Gets(string sSQL, long nUserID)
        {
            return FnQCTestGroup.Service.Gets(sSQL, nUserID);
        }
        public FnQCTestGroup Get(int id, long nUserID)
        {
            return FnQCTestGroup.Service.Get(id, nUserID);
        }
        public FnQCTestGroup Save(long nUserID)
        {
            return FnQCTestGroup.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FnQCTestGroup.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFnQCTestGroupService Service
        {
            get { return (IFnQCTestGroupService)Services.Factory.CreateService(typeof(IFnQCTestGroupService)); }
        }
        #endregion
    }
    #endregion

    #region IFnQCTestGroup interface
    public interface IFnQCTestGroupService
    {
        FnQCTestGroup Get(int id, Int64 nUserID);
        List<FnQCTestGroup> Gets(Int64 nUserID);
        List<FnQCTestGroup> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FnQCTestGroup Save(FnQCTestGroup oFnQCTestGroup, Int64 nUserID);


    }
    #endregion
}
