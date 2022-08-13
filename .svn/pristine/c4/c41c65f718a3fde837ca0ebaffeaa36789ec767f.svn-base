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
    #region FabricClaimDetail
    public class FabricClaimDetail : BusinessObject
    {
        public FabricClaimDetail()
        {
            FabricClaimDetailID = 0;
            FabricClaimID = 0;
            ClaimSettlementType = EnumImportClaimSettleType.None;
            FSCDID = 0;
            ParentFSCDID = 0;
            ParentExeNo = "";
            QtyInPercent = 0;
            Remarks = "";
            Amount = 0;
            ProductCode = "";
            ProductName = "";
            ProductCount = "";
            ExeNoFull = "";
            MUName = "";
            FabricNo = "";
            FabricNum = "";
            ConstructionPI = "";
            OptionNo = "";
            Qty_PI = 0;
            FabricDesignName = "";
            FabricProductName = "";
            ProcessTypeName = "";
            FabricWeaveName = "";
            FinishTypeName = "";
            LDNo = "";
            OwnLDNo = "";
            ShadeCount = 0;
            Code = "";
            ErrorMessage = "";
        }

        #region Property
        public int FabricClaimDetailID { get; set; }
        public int FabricClaimID { get; set; }
        public EnumImportClaimSettleType ClaimSettlementType { get; set; }
        public int FSCDID { get; set; }
        public int ParentFSCDID { get; set; }
        public string ParentExeNo { get; set; }
        public double QtyInPercent { get; set; }
        public string Remarks { get; set; }
        public double Amount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCount { get; set; }
        public string ExeNoFull { get; set; }
        public string MUName { get; set; }
        public string FabricNo { get; set; }
        public string FabricNum { get; set; }
        public string ConstructionPI { get; set; }
        public string OptionNo { get; set; }
        public double Qty_PI { get; set; }
        public string FabricDesignName { get; set; }
        public string FabricProductName { get; set; }
        public string ProcessTypeName { get; set; }
        public string FabricWeaveName { get; set; }
        public string FinishTypeName { get; set; }
        public string LDNo { get; set; }
        public string OwnLDNo { get; set; }
        public int ShadeCount { get; set; }
        public string Code { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ClaimSettlementTypeInString
        {
            get { return EnumObject.jGet(this.ClaimSettlementType); }
        }
        #endregion

        #region Functions
        public static List<FabricClaimDetail> Gets(long nUserID)
        {
            return FabricClaimDetail.Service.Gets(nUserID);
        }
        public static List<FabricClaimDetail> Gets(string sSQL, long nUserID)
        {
            return FabricClaimDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricClaimDetail Get(int id, long nUserID)
        {
            return FabricClaimDetail.Service.Get(id, nUserID);
        }
        public FabricClaimDetail Save(long nUserID)
        {
            return FabricClaimDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricClaimDetail.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricClaimDetailService Service
        {
            get { return (IFabricClaimDetailService)Services.Factory.CreateService(typeof(IFabricClaimDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricClaimDetail interface
    public interface IFabricClaimDetailService
    {
        FabricClaimDetail Get(int id, Int64 nUserID);
        List<FabricClaimDetail> Gets(Int64 nUserID);
        List<FabricClaimDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricClaimDetail Save(FabricClaimDetail oFabricClaimDetail, Int64 nUserID);
    }
    #endregion
}
