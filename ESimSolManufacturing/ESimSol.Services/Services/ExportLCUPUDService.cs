using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
namespace ESimSol.Services.Services
{
    public class ExportLCUPUDService : MarshalByRefObject, IExportLCUPUDService
    {
        #region Private functions and declaration

        private ExportLCUPUD MapObject(NullHandler oReader)
        {
            ExportLCUPUD oExportLCUPUD = new ExportLCUPUD();
            oExportLCUPUD.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportLCUPUD.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportLCUPUD.FileNo = oReader.GetString("FileNo");
            oExportLCUPUD.PINo = oReader.GetString("PINo");
            oExportLCUPUD.PIDate = oReader.GetDateTime("PIDate");
            oExportLCUPUD.LCNo = oReader.GetString("LCNo");
            oExportLCUPUD.IssueDate = oReader.GetDateTime("IssueDate");
            oExportLCUPUD.ContractorID = oReader.GetInt32("ContractorID");
            oExportLCUPUD.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oExportLCUPUD.VersionNo = oReader.GetInt32("VersionNo");
            oExportLCUPUD.LCOpenDate = oReader.GetDateTime("LCOpenDate");
            oExportLCUPUD.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oExportLCUPUD.LCReceiveDate = oReader.GetDateTime("LCReceiveDate");
            oExportLCUPUD.LCStatus = oReader.GetInt32("LCStatus");
            oExportLCUPUD.BankBranchID_Advice = oReader.GetInt32("BankBranchID_Advice");
            oExportLCUPUD.BankBranchID_Issue = oReader.GetInt32("BankBranchID_Issue");
            oExportLCUPUD.BankBranchID_Negotiation = oReader.GetInt32("BankBranchID_Negotiation");
            oExportLCUPUD.Amount = oReader.GetDouble("Amount");
            oExportLCUPUD.Qty = oReader.GetDouble("Qty");
            oExportLCUPUD.Qty_DC = oReader.GetDouble("Qty_DC");
            oExportLCUPUD.Amount_DC = oReader.GetDouble("Amount_DC");
            oExportLCUPUD.Amount_Bill = oReader.GetDouble("Amount_Bill");
            oExportLCUPUD.Qty_Bill = oReader.GetDouble("Qty_Bill");
            oExportLCUPUD.Amount_DO = oReader.GetDouble("Amount_DO");
            oExportLCUPUD.Qty_DO = oReader.GetDouble("Qty_DO");
            oExportLCUPUD.BUID = oReader.GetInt32("BUID");
            oExportLCUPUD.ContractorName = oReader.GetString("ContractorName");
            oExportLCUPUD.BUID = oReader.GetInt32("BUID");
            oExportLCUPUD.LastDeliveryDate = oReader.GetDateTime("LastDeliveryDate");
            oExportLCUPUD.DOValue = oReader.GetDouble("DOValue");
            oExportLCUPUD.DOQty = oReader.GetDouble("DOQty");
            oExportLCUPUD.MKTPersonName = oReader.GetString("MKTPersonName");
            oExportLCUPUD.Currency = oReader.GetString("Currency");
            oExportLCUPUD.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportLCUPUD.BuyerID = oReader.GetInt32("BuyerID");
            oExportLCUPUD.BuyerName = oReader.GetString("BuyerName");
            oExportLCUPUD.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportLCUPUD.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportLCUPUD.BBranchName_Issue = oReader.GetString("BBranchName_Issue");
            oExportLCUPUD.InvoiceNo = oReader.GetString("InvoiceNo");
            oExportLCUPUD.UDID = oReader.GetInt32("UDID");
            oExportLCUPUD.UDNo = oReader.GetString("UDNo");
            oExportLCUPUD.UDRecdDate = oReader.GetDateTime("UDRecdDate");
            oExportLCUPUD.UPNo = oReader.GetString("UPNo");
            oExportLCUPUD.ExportUPDate = oReader.GetDateTime("ExportUPDate");
            oExportLCUPUD.MUnitID = oReader.GetInt32("MUnitID");
            oExportLCUPUD.MUSymbol = oReader.GetString("MUSymbol");
            oExportLCUPUD.BUName = oReader.GetString("BUName");
            oExportLCUPUD.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportLCUPUD.ExpiryDate = oReader.GetDateTime("ExpiryDate");
            oExportLCUPUD.AUDNo = oReader.GetString("AUDNo");
            oExportLCUPUD.ADate = oReader.GetDateTime("ADate");
            
            return oExportLCUPUD;
        }

        private ExportLCUPUD CreateObject(NullHandler oReader)
        {
            ExportLCUPUD oExportLCUPUD = new ExportLCUPUD();
            oExportLCUPUD = MapObject(oReader);
            return oExportLCUPUD;
        }

        private List<ExportLCUPUD> CreateObjects(IDataReader oReader)
        {
            List<ExportLCUPUD> oExportLCUPUD = new List<ExportLCUPUD>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLCUPUD oItem = CreateObject(oHandler);
                oExportLCUPUD.Add(oItem);
            }
            return oExportLCUPUD;
        }

        #endregion

        #region Interface implementation

        public List<ExportLCUPUD> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportLCUPUD> oExportLCUPUDs = new List<ExportLCUPUD>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportLCUPUDDA.Gets(tc, sSQL);
                oExportLCUPUDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportLCUPUD", e);
                #endregion
            }
            return oExportLCUPUDs;
        }

        #endregion
    }

}
