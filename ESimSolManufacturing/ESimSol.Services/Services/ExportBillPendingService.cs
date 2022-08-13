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
    public class ExportBillPendingService : MarshalByRefObject, IExportBillPendingService
    {
        #region Private functions and declaration
        private ExportBillPending MapObject(NullHandler oReader)
        {
            ExportBillPending oExportBillPending = new ExportBillPending();
            oExportBillPending.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oExportBillPending.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportBillPending.ProductID = oReader.GetInt32("ProductID");
            oExportBillPending.MUnitID = oReader.GetInt32("MUnitID");
            oExportBillPending.PIQty = oReader.GetDouble("PIQty");
            oExportBillPending.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportBillPending.PIAmount = oReader.GetDouble("PIAmount");
            oExportBillPending.DeliveryQty = oReader.GetDouble("DeliveryQty");
            oExportBillPending.DeliveryAmount = oReader.GetDouble("DeliveryAmount");
            oExportBillPending.BillQty = oReader.GetDouble("BillQty");
            oExportBillPending.BillAmount = oReader.GetDouble("BillAmount");
            oExportBillPending.BillPendingQty = oReader.GetDouble("BillPendingQty");
            oExportBillPending.BillPendingAmount = oReader.GetDouble("BillPendingAmount");

            oExportBillPending.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportBillPending.BUID = oReader.GetInt32("BUID");
            oExportBillPending.BUType = (EnumBusinessUnitType)oReader.GetDouble("BUType");
            oExportBillPending.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportBillPending.ExportLCType = (EnumExportLCType)oReader.GetDouble("ExportLCType");
            oExportBillPending.ApplicantID = oReader.GetInt32("ApplicantID");
            oExportBillPending.OpeningDate = oReader.GetDateTime("OpeningDate");
            oExportBillPending.NegoBankBranchID = oReader.GetInt32("NegoBankBranchID");
            oExportBillPending.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportBillPending.LCAmount = oReader.GetDouble("LCAmount");
            oExportBillPending.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportBillPending.LastDeliveryDate = oReader.GetDateTime("LastDeliveryDate");

            oExportBillPending.PINo = oReader.GetString("PINo");
            oExportBillPending.ApplicantName = oReader.GetString("ApplicantName");
            oExportBillPending.MUnitSymbol = oReader.GetString("MUnitSymbol");
            oExportBillPending.BUName = oReader.GetString("BUName");
            oExportBillPending.BankName = oReader.GetString("BankName");
            oExportBillPending.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oExportBillPending.ProductName = oReader.GetString("ProductName");
            return oExportBillPending;
        }

        private ExportBillPending CreateObject(NullHandler oReader)
        {
            ExportBillPending oExportBillPending = new ExportBillPending();
            oExportBillPending = MapObject(oReader);
            return oExportBillPending;
        }

        private List<ExportBillPending> CreateObjects(IDataReader oReader)
        {
            List<ExportBillPending> oExportBillPending = new List<ExportBillPending>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportBillPending oItem = CreateObject(oHandler);
                oExportBillPending.Add(oItem);
            }
            return oExportBillPending;
        }

        #endregion

        #region Interface implementation
        public ExportBillPendingService() { }
        public List<ExportBillPending> Gets(string sSQL, EnumReportLayout eEnumReportLayout,  Int64 nUserID)
        {
            List<ExportBillPending> oExportBillPending = new List<ExportBillPending>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportBillPendingDA.Gets(tc, sSQL, eEnumReportLayout);
                oExportBillPending = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillPending", e);
                #endregion
            }

            return oExportBillPending;
        }
        #endregion
    }
}
