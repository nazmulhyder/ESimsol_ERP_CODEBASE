using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region SparePartsRequisitionDetail

    public class SparePartsRequisitionDetail : BusinessObject
    {
        public SparePartsRequisitionDetail()
        {
            SparePartsRequisitionDetailID = 0;
            SparePartsRequisitionID = 0;
            ProductID = 0;
            UnitID = 0;
            Quantity = 0;
            ProductCode = "";
            ProductName = "";
            SparePartsRequisitionDetailLogID = 0;
            SparePartsRequisitionLogID = 0;
            Remarks = "";
            TotalLotBalance = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int SparePartsRequisitionDetailID { get; set; }
        public int SparePartsRequisitionID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public double Quantity { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int SparePartsRequisitionDetailLogID { get; set; }
        public int SparePartsRequisitionLogID { get; set; }
        public string Remarks { get; set; }
        public double TotalLotBalance { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions

        public static List<SparePartsRequisitionDetail> Gets(int ProductID, long nUserID)
        {
            return SparePartsRequisitionDetail.Service.Gets(ProductID, nUserID);
        }
        public static List<SparePartsRequisitionDetail> GetsLog(int id, long nUserID)
        {
            return SparePartsRequisitionDetail.Service.GetsLog(id, nUserID);
        }
        public static List<SparePartsRequisitionDetail> Gets(string sSQL, long nUserID)
        {

            return SparePartsRequisitionDetail.Service.Gets(sSQL, nUserID);
        }
        public SparePartsRequisitionDetail Get(int SparePartsRequisitionDetailID, long nUserID)
        {
            return SparePartsRequisitionDetail.Service.Get(SparePartsRequisitionDetailID, nUserID);
        }

        public SparePartsRequisitionDetail Save(long nUserID)
        {
            return SparePartsRequisitionDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static ISparePartsRequisitionDetailService Service
        {
            get { return (ISparePartsRequisitionDetailService)Services.Factory.CreateService(typeof(ISparePartsRequisitionDetailService)); }
        }

        #endregion
    }
    #endregion

    #region ISparePartsRequisitionDetail interface

    public interface ISparePartsRequisitionDetailService
    {
        SparePartsRequisitionDetail Get(int SparePartsRequisitionDetailID, Int64 nUserID);
        List<SparePartsRequisitionDetail> Gets(int ProductID, Int64 nUserID);
        List<SparePartsRequisitionDetail> GetsLog(int id, Int64 nUserID);
        List<SparePartsRequisitionDetail> Gets(string sSQL, Int64 nUserID);
        SparePartsRequisitionDetail Save(SparePartsRequisitionDetail oSparePartsRequisitionDetail, Int64 nUserID);
    }
    #endregion

}
