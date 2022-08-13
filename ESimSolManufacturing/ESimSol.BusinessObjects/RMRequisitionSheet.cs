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

    #region RMRequisitionSheet
    public class RMRequisitionSheet : BusinessObject
    {
        public RMRequisitionSheet()
        {
            RMRequisitionSheetID = 0;
            RMRequisitionID = 0;
            ProductionSheetID = 0;
            PPQty = 0;
            SheetNo = "";
            SheetQty = 0;
            YetToPPQty = 0;
            BuyerName = "";
            ExportPINo = "";
            ApprovedByName = "";
        }

        #region Property
        public int RMRequisitionSheetID { get; set; }
        public int ProductionSheetID { get; set; }
        public int RMRequisitionID { get; set; }
        public double PPQty { get; set; }
        public double YetToPPQty { get; set; }
        public double SheetQty { get; set; }
        public string Remarks { get; set; }       
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UnitSymbol { get; set; }
        public int UnitID { get; set; }
        public string SheetNo { get; set; }
        public string BuyerName { get; set; }
        public string ExportPINo { get; set; }
        public string ApprovedByName { get; set; }
        public string ErrorMessage { get; set; }
       
        #endregion

        #region Derived Property
        public double CurrentOutQty { get; set; }
        public string SheetNoWithPlanQty
        {
            get
            {
                return this.SheetNo + "(" + Global.MillionFormat(this.PPQty)+") "+this.UnitSymbol;
            }
        }
        public string YetToPPQtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.YetToPPQty);
            }
        }
        public string QuantitySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.SheetQty)+" "+this.UnitSymbol;
            }
        } 
        #endregion

        #region Functions
        public static List<RMRequisitionSheet> Gets(int nRMRequisitionID, long nUserID)
        {
            return RMRequisitionSheet.Service.Gets(nRMRequisitionID, nUserID);
        }
        public static List<RMRequisitionSheet> Gets(string sSQL, long nUserID)
        {
            return RMRequisitionSheet.Service.Gets(sSQL, nUserID);
        }
        public RMRequisitionSheet Get(int id, long nUserID)
        {
            return RMRequisitionSheet.Service.Get(id, nUserID);
        }

     
        #endregion

        #region ServiceFactory
        internal static IRMRequisitionSheetService Service
        {
            get { return (IRMRequisitionSheetService)Services.Factory.CreateService(typeof(IRMRequisitionSheetService)); }
        }
        #endregion
    }
    #endregion

    #region IRMRequisitionSheet interface
    public interface IRMRequisitionSheetService
    {
        RMRequisitionSheet Get(int id, Int64 nUserID);
        List<RMRequisitionSheet> Gets(int nRMRequisitionID, Int64 nUserID);
     
        List<RMRequisitionSheet> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
    
    
}
