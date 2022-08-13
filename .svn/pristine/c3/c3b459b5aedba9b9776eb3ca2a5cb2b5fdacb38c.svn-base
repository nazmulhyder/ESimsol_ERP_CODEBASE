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
    #region FGQCDetail
    public class FGQCDetail : BusinessObject
    {
        public FGQCDetail()
        {
            FGQCDetailID = 0;
            FGQCID = 0;
            RefType = EnumFGQCRefType.None;
            RefTypeInt = 0;
            RefID = 0;
            ProductID = 0;
            MUnitID = 0;
            Qty = 0;
            UnitPrice = 0;
            StoreID = 0;
            ProductCode = "";
            ProductName = "";
            MUName = "";
            RefNo = "";
            StoreName = "";
            ErrorMessage = "";
        }

        #region Property
        public int FGQCDetailID { get; set; }
        public int FGQCID { get; set; }
        public EnumFGQCRefType RefType { get; set; }
        public int RefTypeInt { get; set; }
        public int RefID { get; set; }
        public int ProductID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public int StoreID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string RefNo { get; set; }
        public string StoreName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public double Amount
        {
            get
            {
                return (this.Qty * this.UnitPrice);
            }
        }
        #endregion

        #region Functions
        public static List<FGQCDetail> Gets(int id, long nUserID)
        {
            return FGQCDetail.Service.Gets(id, nUserID);
        }
        public static List<FGQCDetail> Gets(string sSQL, long nUserID)
        {
            return FGQCDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<FGQCDetail> FGQCProcess(FGQC oFGQC, long nUserID)
        {
            return FGQCDetail.Service.FGQCProcess(oFGQC, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFGQCDetailService Service
        {
            get { return (IFGQCDetailService)Services.Factory.CreateService(typeof(IFGQCDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IFGQCDetail interface
    public interface IFGQCDetailService
    {
        List<FGQCDetail> Gets(int id, Int64 nUserID);
        List<FGQCDetail> Gets(string sSQL, Int64 nUserID);
        List<FGQCDetail> FGQCProcess(FGQC oFGQC, Int64 nUserID);
    }
    #endregion
}
