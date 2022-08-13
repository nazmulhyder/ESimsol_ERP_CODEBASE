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

    #region RMRequisitionMaterial
    public class RMRequisitionMaterial : BusinessObject
    {
        public RMRequisitionMaterial()
        {
            RMRequisitionMaterialID = 0;
            RMRequisitionID = 0;
            ProductionRecipeID = 0;
            Qty = 0;
            ProductID = 0;
            ProductName = "";
            ProductCode = "";
            SheetNo = "";
            RequiredQty = 0;
            OutQty = 0;
            QtyType = EnumQtyType.None;
            ProductionRecipeID = 0;
            MUnitID = 0;
            MUName = "";
            YetToRequisitionQty = 0;
            ProductionSheetID = 0;
            QtyInPercent = 0;
            ReportingUnit = "";
            ReportingQty = 0;
            MaterialOutQty = 0;
            SheetNoWithDescription = "";
            RequisitionNo = "";
            YetToOutQty = 0;
            StockBalance = 0;
            CurrentOutQty = 0;
            WUID = 0;
            RMRequisitionMaterials = new List<RMRequisitionMaterial>();
            ITransactions = new List<ITransaction>();
        }

        #region Property
        public int RMRequisitionMaterialID { get; set; }
        public int RMRequisitionID { get; set; }
        public int ProductID { get; set; }
        public double ProductionRecipeID { get; set; }
        public double RequiredQty { get; set; }
        public double OutQty { get; set; }
        public string Remarks { get; set; }       
        public string ProductCode { get; set; }
        public string ProductName { get; set; }       
        public string MUName { get; set; }
        public int MUnitID { get; set; }
        public EnumQtyType QtyType { get; set; }
        public int QtyTypeInt { get; set; }
        public string SheetNo { get; set; }
        public string ErrorMessage { get; set; }
        public string MUSymbol { get; set; }
        public int ProductionSheetID { get; set; }
        public double QtyInPercent { get; set; }
        public double YetToRequisitionQty { get; set; }
        public double Qty { get; set; }
        public string ReportingUnit { get; set; }
        public double ReportingQty { get; set; }
        public double MaterialOutQty { get; set; }
        public string SheetNoWithDescription { get; set; }
        public string RequisitionNo { get; set; }
        public double YetToOutQty { get; set; }
        public double StockBalance { get; set; }
        public double CurrentOutQty { get; set; }
        public int WUID { get; set; }
        public List<RMRequisitionMaterial> RMRequisitionMaterials { get; set; }
        public List<ITransaction> ITransactions { get; set; }
        #endregion

        #region Derived Property
        public string QtyTypeSt
        {
            get
            {
                return this.QtyType.ToString();
            }
        }
        public string RequiredQtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.RequiredQty);
            }
        }
        #endregion

        #region Functions
        public static List<RMRequisitionMaterial> Gets(int nRMRequisitionID, long nUserID)
        {
            return RMRequisitionMaterial.Service.Gets(nRMRequisitionID, nUserID);
        }
      
        public static List<RMRequisitionMaterial> Gets(string sSQL, long nUserID)
        {
            return RMRequisitionMaterial.Service.Gets(sSQL, nUserID);
        }
        public RMRequisitionMaterial Get(int id, long nUserID)
        {
            return RMRequisitionMaterial.Service.Get(id, nUserID);
        }
        public RMRequisitionMaterial CommitRawMaterialOut(EnumTriggerParentsType eTriggerParentType, long nUserID)
        {
            return RMRequisitionMaterial.Service.CommitRawMaterialOut(this, eTriggerParentType, nUserID);
        }
        public RMRequisitionMaterial ReceiveReturnQty(long nUserID)
        {
            return RMRequisitionMaterial.Service.ReceiveReturnQty(this,  nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRMRequisitionMaterialService Service
        {
            get { return (IRMRequisitionMaterialService)Services.Factory.CreateService(typeof(IRMRequisitionMaterialService)); }
        }
        #endregion
    }
    #endregion

    #region IRMRequisitionMaterial interface
    public interface IRMRequisitionMaterialService
    {
        RMRequisitionMaterial Get(int id, Int64 nUserID);
        List<RMRequisitionMaterial> Gets(int nRMRequisitionID, Int64 nUserID);     
        List<RMRequisitionMaterial> Gets(string sSQL, Int64 nUserID);
        RMRequisitionMaterial CommitRawMaterialOut(RMRequisitionMaterial oRMRequisitionMaterial, EnumTriggerParentsType eTriggerParentType, Int64 nUserID);
        RMRequisitionMaterial ReceiveReturnQty(RMRequisitionMaterial oRMRequisitionMaterial, Int64 nUserID);
        
    }
    #endregion
    
    
}
