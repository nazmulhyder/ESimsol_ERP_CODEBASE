using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{

    #region BusinessUnitWiseAccountHead
    public class BusinessUnitWiseAccountHead : BusinessObject
    {
        public BusinessUnitWiseAccountHead()
        {
            BusinessUnitWiseAccountHeadID = 0;
            BusinessUnitID = 0;
            AccountHeadID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int BusinessUnitWiseAccountHeadID { get; set; }
        public int BusinessUnitID { get; set; }
        public int AccountHeadID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Propertise
        public List<BusinessUnitWiseAccountHead> BusinessUnitWiseAccountHeads { get; set; }

        public Company Company { get; set; }
        public TChartsOfAccount TChartsOfAccount { get; set; }
        #endregion

        #region Functions
        public string CopyBasicChartOfAccount(int nCompanyID, int nUserID)
        {
            return BusinessUnitWiseAccountHead.Service.CopyBasicChartOfAccount(nCompanyID, nUserID);
        }
        public BusinessUnitWiseAccountHead Get(int id, int nUserID)
        {
            return BusinessUnitWiseAccountHead.Service.Get(id, nUserID);
        }
        public BusinessUnitWiseAccountHead Save(int nUserID)
        {
            return BusinessUnitWiseAccountHead.Service.Save(this, nUserID);
        }
        public BusinessUnitWiseAccountHead SaveFromCOA(int nUserID)
        {
            return BusinessUnitWiseAccountHead.Service.SaveFromCOA(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return BusinessUnitWiseAccountHead.Service.Delete(id, nUserID);
        }
        public static List<BusinessUnitWiseAccountHead> Gets(int nUserID)
        {
            return BusinessUnitWiseAccountHead.Service.Gets(nUserID);
        }
        public static List<BusinessUnitWiseAccountHead> Gets(int nBUID, int nUserID)
        {
            return BusinessUnitWiseAccountHead.Service.Gets(nBUID, nUserID);
        }
        public static List<BusinessUnitWiseAccountHead> GetsByCOA(int nAHID, int nUserID)
        {
            return BusinessUnitWiseAccountHead.Service.GetsByCOA(nAHID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBusinessUnitWiseAccountHeadService Service
        {
            get { return (IBusinessUnitWiseAccountHeadService)Services.Factory.CreateService(typeof(IBusinessUnitWiseAccountHeadService)); }
        }
        #endregion
    }
    #endregion

    #region IBusinessUnitWiseAccountHead interface
    public interface IBusinessUnitWiseAccountHeadService
    {
        BusinessUnitWiseAccountHead Get(int id, int nUserID);
        List<BusinessUnitWiseAccountHead> Gets(int nUserID);
        List<BusinessUnitWiseAccountHead> Gets(int nBUID, int nUserID);
        List<BusinessUnitWiseAccountHead> GetsByCOA(int nAHID, int nUserID);
        string CopyBasicChartOfAccount(int id, int nUserID);
        string Delete(int id, int nUserID);
        BusinessUnitWiseAccountHead Save(BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead, int nUserID);
        BusinessUnitWiseAccountHead SaveFromCOA(BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead, int nUserID);
    }
    #endregion
    

}
