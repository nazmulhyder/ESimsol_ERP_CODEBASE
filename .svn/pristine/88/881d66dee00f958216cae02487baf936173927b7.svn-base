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
    #region KnittingYarnChallanDetail
    public class KnittingYarnChallanDetail : BusinessObject
    {
        public KnittingYarnChallanDetail()
        {
            KnittingYarnChallanDetailID = 0;
            KnittingYarnChallanID = 0;
            KnittingOrderDetailID = 0;
            KnittingCompositionID = 0;
            IssueStoreID = 0;
            YarnID = 0;
            LotID = 0;
            MUnitID = 0;
            Qty = 0;
            Remarks = "";
            KODOrderQty = 0;
            ErrorMessage = "";
        }

        #region Property
   
        public int KnittingYarnChallanDetailID { get; set; }
        public int KnittingYarnChallanID { get; set; }
        public int KnittingOrderDetailID { get; set; }
        public int KnittingCompositionID { get; set; }
        public int IssueStoreID { get; set; }
        public int YarnID { get; set; }
        public int LotID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public string BrandName { get; set; }
        public int ColorID { get; set; }
        public double BagQty { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        #endregion

        #region Derived Property
        public double YetToChallanQty { get; set; }
        public double KODQty { get; set; }
        public double KODOrderQty { get; set; }
        public string KODMUShortName { get; set; }
        public string BrandShortName { get; set; }
        public string BuyerName { get; set; }
        public int PAM { get; set; }
        public string ColorName { get; set; }
        public string KODStyleNo { get; set; }
        public string KODColorName { get; set; }
        public string MUnitName{ get; set; }
        public string  OperationUnitName{ get; set; }
        public string YarnName{ get; set; }
        public string YarnCode { get; set; } 
        public string LotNo{ get; set; }
        public double LotBalance { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public double ReturnBalance { get; set; }
        public double ChallanBalance { get; set; }

        public string ChallanWithLotandQty
        {
            get
            {
                return this.ChallanNo + " For Yarn-" + this.YarnName + " [" + this.YarnID + "] [ Balance -" + this.ChallanBalance + " " + this.MUnitName + " ] [Lot No: " + this.LotNo + "]";
            }
        }
        public string ChallanDateInString
        {
            get
            {
                return ChallanDate.ToString("dd MMM yyyy");
            }
        }
        public string StyleWithColor
        {
            get
            {
                return this.KODStyleNo + "(" + this.KODColorName + ")";
            }
        }
        public double ChallanQty { get; set; }
        
        #endregion

        #region Functions
        public static List<KnittingYarnChallanDetail> Gets(long nUserID)
        {
            return KnittingYarnChallanDetail.Service.Gets(nUserID);
        }
        public static List<KnittingYarnChallanDetail> Gets(string sSQL, long nUserID)
        {
            return KnittingYarnChallanDetail.Service.Gets(sSQL, nUserID);
        }
        public KnittingYarnChallanDetail Get(int id, long nUserID)
        {
            return KnittingYarnChallanDetail.Service.Get(id, nUserID);
        }
        public KnittingYarnChallanDetail Save(long nUserID)
        {
            return KnittingYarnChallanDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnittingYarnChallanDetail.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IKnittingYarnChallanDetailService Service
        {
            get { return (IKnittingYarnChallanDetailService)Services.Factory.CreateService(typeof(IKnittingYarnChallanDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IKnittingYarnChallanDetail interface
    public interface IKnittingYarnChallanDetailService
    {
        KnittingYarnChallanDetail Get(int id, Int64 nUserID);
        List<KnittingYarnChallanDetail> Gets(Int64 nUserID);
        List<KnittingYarnChallanDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        KnittingYarnChallanDetail Save(KnittingYarnChallanDetail oKnittingYarnChallanDetail, Int64 nUserID);
    }
    #endregion
}
