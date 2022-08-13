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
    #region RMOutRegister
    public class RMOutRegister : BusinessObject
    {
        public RMOutRegister()
        {
            BUID = 0;
            RMRequisitionID = 0;
            ProductionSheetID = 0;
            ProductionRecipeID = 0;
            RMProductID = 0;
            RMProductCode = "";
            RMProductName = "";
            RequisitionQty = 0;
            RMOutQty = 0;
            RemainingQty = 0;
            RMUnitSymbol = "";
            ProductionSheetNo = "";
            ExportPIID = 0;
            ExportPINo = "";
            ContractorName = "";
            FinishGoodsID = 0;
            FinishGoodsName = "";
            FinisgGoodsQty = 0;
            FGUnitSymbol = "";
            RMRequisitionNo = "";
            RMRequisitionDate = DateTime.Today;
            ErrorMessage = "";
            SearchingData = "";
        }

        #region Properties
        public int BUID { get; set; }
        public int RMRequisitionID { get; set; }
        public int ProductionSheetID { get; set; }
        public int ProductionRecipeID { get; set; }
        public int RMProductID { get; set; }
        public string RMProductCode { get; set; }
        public string RMProductName { get; set; }
        public double RequisitionQty { get; set; }
        public double RMOutQty { get; set; }
        public double RemainingQty { get; set; }
        public string RMUnitSymbol { get; set; }
        public string ProductionSheetNo { get; set; }
        public int ExportPIID { get; set; }
        public string ExportPINo { get; set; }
        public string ContractorName { get; set; }
        public int FinishGoodsID { get; set; }
        public string FinishGoodsName { get; set; }
        public double FinisgGoodsQty { get; set; }
        public string FGUnitSymbol { get; set; }
        public string RMRequisitionNo { get; set; }
        public DateTime RMRequisitionDate { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchingData { get; set; }
        #endregion

        #region Derived Property

        public string RMRequisitionDateSt
        {
            get
            {
                if (this.RMRequisitionDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.RMRequisitionDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion

        #region Functions
        public static List<RMOutRegister> Gets(string sSQL, long nUserID)
        {
            return RMOutRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRMOutRegisterService Service
        {
            get { return (IRMOutRegisterService)Services.Factory.CreateService(typeof(IRMOutRegisterService)); }
        }
        #endregion
    }
    #endregion

    #region IRMOutRegister interface

    public interface IRMOutRegisterService
    {
        List<RMOutRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
