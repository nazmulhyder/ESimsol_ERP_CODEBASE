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
    #region SparePartsChallan

    public class SparePartsChallan : BusinessObject
    {
        public SparePartsChallan()
        {
            SparePartsChallanID = 0;
            ChallanNo = "";
            ChallanDate = DateTime.Now;
            SparePartsRequisitionID = 0;
            StoreID = 0;
            BUID = 0;
            StoreName = "";
            DisburseBy = 0;
            DisburseByName = "";
            Remarks = "";
            CapitalResourceName = "";
            SparePartsChallanDetails = new List<SparePartsChallanDetail>();
        }

        #region Properties

        public int SparePartsChallanID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public int SparePartsRequisitionID { get; set; }
        public int StoreID { get; set; }
        public int BUID { get; set; }
        public string StoreName { get; set; }
        public int DisburseBy { get; set; }
        public string DisburseByName { get; set; }
        public string CapitalResourceName { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        public List<SparePartsChallanDetail> SparePartsChallanDetails { get; set; }
        #endregion

        #region Derived Property

        public string ChallanDateSt
        {
            get
            {
                return this.ChallanDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions

        public static List<SparePartsChallan> Gets(long nUserID)
        {
            return SparePartsChallan.Service.Gets(nUserID);
        }
        public static List<SparePartsChallan> Gets(string sSQL, Int64 nUserID)
        {
            return SparePartsChallan.Service.Gets(sSQL, nUserID);
        }
        public SparePartsChallan Get(int nId, long nUserID)
        {
            return SparePartsChallan.Service.Get(nId, nUserID);
        }
        public SparePartsChallan Save(long nUserID)
        {
            return SparePartsChallan.Service.Save(this, nUserID);
        }
        public SparePartsChallan Disburse(long nUserID)
        {
            return SparePartsChallan.Service.Disburse(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return SparePartsChallan.Service.Delete(nId, nUserID);
        }
        

        #endregion

        #region ServiceFactory
        internal static ISparePartsChallanService Service
        {
            get { return (ISparePartsChallanService)Services.Factory.CreateService(typeof(ISparePartsChallanService)); }
        }
        #endregion
    }
    #endregion

    #region ISparePartsChallan interface

    public interface ISparePartsChallanService
    {
        SparePartsChallan Get(int id, long nUserID);
        List<SparePartsChallan> Gets(long nUserID);
        List<SparePartsChallan> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        SparePartsChallan Save(SparePartsChallan oSparePartsChallan, long nUserID);
        SparePartsChallan Disburse(SparePartsChallan oSparePartsChallan, long nUserID);
    }
    #endregion
}