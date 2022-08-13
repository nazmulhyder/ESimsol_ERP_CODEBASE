using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Data;

namespace ESimSol.BusinessObjects
{
    public class ExportFundAllocation : BusinessObject
    {
        public ExportFundAllocation()
        {
            ExportFundAllocationID = 0;
            ExportLCID = 0;
            ExportFundAllocationHeadID = 0;
            ExportFundAllocationHeadName = "";
            Amount = 0;
            AmountInPercent = 0;
            Remarks = "";
            ApprovedBy = 0;
            ApprovedByName = "";
            ErrorMessage = "";
            CurrencySymbol = "";
            SQL = "";
        }

        #region Properties
        public int ExportFundAllocationID { get; set; }
        public int ExportLCID { get; set; }
        public int ApprovedBy { get; set; }
        public int ExportFundAllocationHeadID { get; set; }
        public double Amount { get; set; }
        public double AmountInPercent { get; set; }
        public string Remarks { get; set; }
        public string SQL { get; set; }
        public string CurrencySymbol { get; set; }
        public string ExportFundAllocationHeadName { get; set; }
        public string ApprovedByName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived Properties
        public string AmountSt
        {
            get
            {
                if (this.Amount < 0)
                {
                    return this.CurrencySymbol + " (" + Global.MillionFormat(this.Amount * (-1)) + ")";
                }
                else
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat(this.Amount);
                }
            }
        }
        #endregion
        #region Functions
        public ExportFundAllocation Save(long nUserID)
        {
            return ExportFundAllocation.Service.Save(this, nUserID);
        }
        public static List<ExportFundAllocation> Gets(string sSQL, long nUserID)
        {
            return ExportFundAllocation.Service.Gets(sSQL, nUserID);
        }
        public ExportFundAllocation Get(int id, long nUserID)
        {
            return ExportFundAllocation.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ExportFundAllocation.Service.Delete(id, nUserID);
        }
        public List<ExportFundAllocation> ApprovedExportFundAllocation(List<ExportFundAllocation> oExportFundAllocations, long nUserID)
        {
            return ExportFundAllocation.Service.ApprovedExportFundAllocation(oExportFundAllocations, nUserID);
        }
        public List<ExportFundAllocation> UndoApprovedExportFundAllocation(List<ExportFundAllocation> oExportFundAllocations, long nUserID)
        {
            return ExportFundAllocation.Service.UndoApprovedExportFundAllocation(oExportFundAllocations, nUserID);
        }
        public static DataSet Gets(ExportFundAllocation oExportFundAllocation, long nUserID)
        {
            return ExportFundAllocation.Service.Gets(oExportFundAllocation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportFundAllocationService Service
        {
            get { return (IExportFundAllocationService)Services.Factory.CreateService(typeof(IExportFundAllocationService)); }
        }
        #endregion
    }

    #region IExportFundAllocationService interface

    public interface IExportFundAllocationService
    {
        ExportFundAllocation Save(ExportFundAllocation oExportFundAllocation, Int64 nUserID);
        List<ExportFundAllocation> Gets(string sSQL, long nUserID);
        ExportFundAllocation Get(int id, long nUserID);
        string Delete(int id, long nUserID);
        List<ExportFundAllocation> ApprovedExportFundAllocation(List<ExportFundAllocation> oExportFundAllocations, long nUserID);
        List<ExportFundAllocation> UndoApprovedExportFundAllocation(List<ExportFundAllocation> oExportFundAllocations, long nUserID);

        DataSet Gets(ExportFundAllocation oExportFundAllocation, long nUserID);
    }
    #endregion
}
