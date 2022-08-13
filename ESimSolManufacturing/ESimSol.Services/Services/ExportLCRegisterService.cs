using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    public class ExportLCRegisterService : MarshalByRefObject, IExportLCRegisterService
    {
        #region Private functions and declaration
        private ExportLCRegister MapObject(NullHandler oReader)
        {
            ExportLCRegister oExportLCRegister = new ExportLCRegister();

            oExportLCRegister.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportLCRegister.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportLCRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportLCRegister.Qty = (oReader.GetDouble("Qty")/ (oReader.GetDouble("RateUnit")<=0 ? 1 : oReader.GetDouble("RateUnit")) );
            oExportLCRegister.Qty_DO = oReader.GetDouble("Qty_DO");
            oExportLCRegister.Qty_DC = oReader.GetDouble("Qty_DC");
            oExportLCRegister.BUID = oReader.GetInt32("BUID");
            oExportLCRegister.PINo = oReader.GetString("PINo");
            oExportLCRegister.LCValue = oReader.GetDouble("LCValue");
            oExportLCRegister.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oExportLCRegister.BuyerID = oReader.GetInt32("BuyerID");
            oExportLCRegister.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oExportLCRegister.VersionNo = oReader.GetInt32("VersionNo");
            oExportLCRegister.LCOpenDate = oReader.GetDateTime("LCOpenDate");
            oExportLCRegister.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oExportLCRegister.LCReceiveDate = oReader.GetDateTime("LCReceiveDate");
            oExportLCRegister.UDRecDate = oReader.GetDateTime("UDRecDate");
            oExportLCRegister.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportLCRegister.ExpiryDate = oReader.GetDateTime("ExpiryDate");
            oExportLCRegister.UDRcvType = (EnumUDRcvType)oReader.GetInt32("UDRcvType");
            oExportLCRegister.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportLCRegister.PIDate = oReader.GetDateTime("PIDate");
            oExportLCRegister.LCNo = oReader.GetString("LCNo");
            oExportLCRegister.ApplicantID = oReader.GetInt32("ApplicantID");
            oExportLCRegister.LCStatus = (EnumLCStatus)oReader.GetInt32("LCStatus");
            oExportLCRegister.NegoBankBranchID = oReader.GetInt32("NegoBankBranchID");
            oExportLCRegister.IssueBankBranchID = oReader.GetInt32("IssueBankBranchID");
            oExportLCRegister.NoteQuery = oReader.GetString("NoteQuery");
            oExportLCRegister.NoteUD = oReader.GetString("NoteUD");
            oExportLCRegister.HaveQuery = oReader.GetBoolean("HaveQuery");
            oExportLCRegister.GetOriginalCopy = oReader.GetBoolean("GetOriginalCopy");
            oExportLCRegister.Currency = oReader.GetString("Currency");
            oExportLCRegister.ApplicantName = oReader.GetString("ApplicantName");
            oExportLCRegister.BuyerName = oReader.GetString("BuyerName");
            oExportLCRegister.MKTPersonName = oReader.GetString("MKTPersonName");
            oExportLCRegister.NegoBankName = oReader.GetString("NegoBankName");
            oExportLCRegister.IssueBankName = oReader.GetString("IssueBankName");
            oExportLCRegister.Value_DO = oReader.GetDouble("DOValue");
            oExportLCRegister.Value_DC = oReader.GetDouble("ChallanValue");
            oExportLCRegister.Qty_Invoice = ( oReader.GetDouble("Qty_Invoice") / (oReader.GetDouble("RateUnit")<=0 ? 1 : oReader.GetDouble("RateUnit")));
            oExportLCRegister.ProductName = oReader.GetString("ProductName");
            oExportLCRegister.Acc_Bank = oReader.GetInt32("Acc_Bank");
            oExportLCRegister.ExportLCType = (EnumExportLCType)oReader.GetInt32("ExportLCType");
            if (oExportLCRegister.ExportLCType == EnumExportLCType.TT || oExportLCRegister.ExportLCType == EnumExportLCType.FDD) { oExportLCRegister.LCNo = oExportLCRegister.ExportLCType.ToString()+""+oExportLCRegister.LCNo; }
            return oExportLCRegister;
        }

        private ExportLCRegister CreateObject(NullHandler oReader)
        {
            ExportLCRegister oExportLCRegister = new ExportLCRegister();
            oExportLCRegister = MapObject(oReader);
            return oExportLCRegister;
        }

        private List<ExportLCRegister> CreateObjects(IDataReader oReader)
        {
            List<ExportLCRegister> oExportLCRegister = new List<ExportLCRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLCRegister oItem = CreateObject(oHandler);
                oExportLCRegister.Add(oItem);
            }
            return oExportLCRegister;
        }

        #endregion

        #region Interface implementation
        public ExportLCRegisterService() { }
        public List<ExportLCRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportLCRegister> oExportLCRegister = new List<ExportLCRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportLCRegisterDA.Gets(tc, sSQL);
                oExportLCRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportLCRegister", e);
                #endregion
            }

            return oExportLCRegister;
        }
        #endregion
    }
}
