using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class DULedgerService : MarshalByRefObject, IDULedgerService
    {
        #region Private functions and declaration
        private DULedger MapObject(NullHandler oReader)
        {
            DULedger oDULedger = new DULedger();
            oDULedger.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDULedger.OrderNo = oReader.GetString("OrderNo");
            oDULedger.OrderDate = oReader.GetDateTime("OrderDate");
            oDULedger.ContractorID = oReader.GetInt32("ContractorID");
            oDULedger.PaymentType = oReader.GetInt32("PaymentType");
            oDULedger.DyeingOrderType = oReader.GetInt32("DyeingOrderType");
            oDULedger.ExportPIID = oReader.GetInt32("ExportPIID");
            oDULedger.Status = oReader.GetInt32("OrderStatus");
            oDULedger.SampleInvoiceID = oReader.GetInt32("SampleInvoiceID");
            oDULedger.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oDULedger.ContractorName = oReader.GetString("ContractorName");
            oDULedger.MKTPName = oReader.GetString("MKTPName");
            oDULedger.SampleInvocieNo = oReader.GetString("SampleInvocieNo");
            oDULedger.ExportPINo = oReader.GetString("ExportPINo");
            oDULedger.Amount = oReader.GetDouble("Amount");
            oDULedger.Qty = oReader.GetDouble("Qty");
            oDULedger.Qty_DC = oReader.GetDouble("Qty_DC");
            oDULedger.Qty_Paid = oReader.GetDouble("Qty_Paid");
            oDULedger.Amount_DC = oReader.GetDouble("Amount_DC");
            oDULedger.Amount_Paid = oReader.GetDouble("Amount_Paid");
            oDULedger.MUnitID = oReader.GetInt32("MUnitID");
            oDULedger.OrderCount = oReader.GetInt32("OrderCount");
            oDULedger.CurrencyID = oReader.GetInt32("CurrencyID");
            oDULedger.MUnitID = oReader.GetInt32("MUnitID");
            oDULedger.OrderType = oReader.GetString("OrderType");
            oDULedger.LCNo = oReader.GetString("LCNo");
            oDULedger.MUName = oReader.GetString("MUName");
            oDULedger.CurrencyName = oReader.GetString("CurrencyName");
            oDULedger.CurrencySymbol = oReader.GetString("CurrencySymbol");
            return oDULedger;

        }


        private DULedger CreateObject(NullHandler oReader)
        {
            DULedger oDULedger = MapObject(oReader);
            return oDULedger;
        }

        private List<DULedger> CreateObjects(IDataReader oReader)
        {
            List<DULedger> oDULedgers = new List<DULedger>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DULedger oItem = CreateObject(oHandler);
                oDULedgers.Add(oItem);
            }
            return oDULedgers;
        }

        #endregion

        #region Interface implementation
        public DULedgerService() { }
        public List<DULedger> Gets(DULedger oDULedger, Int64 nUserID)
        {
            List<DULedger> oDULedgers = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DULedgerDA.Gets(tc, oDULedger, nUserID);
                oDULedgers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Ledger!", e);
                #endregion
            }
            return oDULedgers;
        }
        #endregion
       
    }
}
