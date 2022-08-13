using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class SalesCommissionLCService : MarshalByRefObject, ISalesCommissionLCService
    {
        #region Private functions and declaration
        private static SalesCommissionLC MapObject(NullHandler oReader)
        {
            SalesCommissionLC oSalesCommissionLC = new SalesCommissionLC();
            oSalesCommissionLC.ExportPIID = oReader.GetInt32("ExportPIID");
            oSalesCommissionLC.ExportLCID = oReader.GetInt32("ExportLCID");
            oSalesCommissionLC.PINo = oReader.GetString("PINo");
            oSalesCommissionLC.LCNo = oReader.GetString("LCNo");
            oSalesCommissionLC.PIDate = oReader.GetDateTime("PIDate");
            oSalesCommissionLC.Amount = oReader.GetDouble("Amount");
            oSalesCommissionLC.Currency = oReader.GetString("Currency");
            oSalesCommissionLC.LCOpenDate = oReader.GetDateTime("LCOpenDate");
            oSalesCommissionLC.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oSalesCommissionLC.VersionNo = oReader.GetInt32("VersionNo");
            oSalesCommissionLC.ContractorID = oReader.GetInt32("ContractorID");
            oSalesCommissionLC.ContractorName = oReader.GetString("ContractorName");
            oSalesCommissionLC.BuyerID = oReader.GetInt32("BuyerID");
            oSalesCommissionLC.BuyerName = oReader.GetString("BuyerName");
            oSalesCommissionLC.InvoiceValue = oReader.GetDouble("InvoiceValue");
            oSalesCommissionLC.Com_PI = oReader.GetDouble("Com_PI");
            oSalesCommissionLC.Com_Dis = oReader.GetDouble("Com_Dis");
            oSalesCommissionLC.Com_Payable = oReader.GetDouble("Com_Payable");
            oSalesCommissionLC.Com_Paid = oReader.GetDouble("Com_Paid");
            oSalesCommissionLC.BUID = oReader.GetInt32("BUID");
            oSalesCommissionLC.Status =(EnumLSalesCommissionStatus) oReader.GetInt16("Status");
            oSalesCommissionLC.Status =oSalesCommissionLC.Status ==0 ?EnumLSalesCommissionStatus.Initialize : oSalesCommissionLC.Status;

            return oSalesCommissionLC;
        }


        public static SalesCommissionLC CreateObject(NullHandler oReader)
        {
            SalesCommissionLC oSalesCommissionLC = MapObject(oReader);
            return oSalesCommissionLC;
        }

        private List<SalesCommissionLC> CreateObjects(IDataReader oReader)
        {
            List<SalesCommissionLC> oSalesCommissionLCs = new List<SalesCommissionLC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesCommissionLC oItem = CreateObject(oHandler);
                oSalesCommissionLCs.Add(oItem);
            }
            return oSalesCommissionLCs;
        }

        #endregion

        #region Interface implementation
        public SalesCommissionLCService() { }
        public SalesCommissionLC GetByExportPIID(int id, Int64 nUserId)
        {
            SalesCommissionLC oSalesCommission = new SalesCommissionLC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesCommissionLCDA.GetByExportPIID(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesCommission = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oSalesCommission;
        }
        public List<SalesCommissionLC> IUD(SalesCommissionLC oSalesCommissionLC, int nDBOperation, Int64 nUserID)
        {
            List<SalesCommissionLC> oSalesCommissionLCs = new List<SalesCommissionLC>();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update )
                {
                  
                    reader = SalesCommissionLCDA.IUD(tc, oSalesCommissionLC, nDBOperation, nUserID);
                    oSalesCommissionLCs = CreateObjects(reader);
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSalesCommissionLC = new SalesCommissionLC();
                oSalesCommissionLC.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('~')[1] : ex.Message;
                oSalesCommissionLCs.Add(oSalesCommissionLC);
                #endregion
            }
            return oSalesCommissionLCs;
        }
        public List<SalesCommissionLC> Gets(string sSQL, Int64 nUserID)
        {
            List<SalesCommissionLC> oSCLCS = new List<SalesCommissionLC>();
            SalesCommissionLC oSCLC = new SalesCommissionLC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesCommissionLCDA.Gets(tc, sSQL);
                oSCLCS = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSCLC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oSCLCS.Add(oSCLC);
                #endregion
            }

            return oSCLCS;
        }

        #endregion
    }
}
