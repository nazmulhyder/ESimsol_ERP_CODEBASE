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

    public class ProformaInvoiceHistoryService : MarshalByRefObject, IProformaInvoiceHistoryService
    {
        #region Private functions and declaration
        private ProformaInvoiceHistory MapObject(NullHandler oReader)
        {
            ProformaInvoiceHistory oProformaInvoiceHistory = new ProformaInvoiceHistory();

            oProformaInvoiceHistory.ProformaInvoiceHistoryID = oReader.GetInt32("ProformaInvoiceHistoryID");
            oProformaInvoiceHistory.ProformaInvoiceID = oReader.GetInt32("ProformaInvoiceID");
            oProformaInvoiceHistory.PreviousStatus = (EnumPIStatus)oReader.GetInt32("PreviousStatus");
            oProformaInvoiceHistory.CurrentStatus = (EnumPIStatus)oReader.GetInt32("CurrentStatus");
            oProformaInvoiceHistory.OperateBy = oReader.GetInt32("OperateBy");
            oProformaInvoiceHistory.Note = oReader.GetString("Note");
            oProformaInvoiceHistory.OperateByName = oReader.GetString("OperateByName");
            oProformaInvoiceHistory.PINo = oReader.GetString("PINo");
            oProformaInvoiceHistory.OperationDateTime = oReader.GetDateTime("OperationDateTime");
            return oProformaInvoiceHistory;
        }

        private ProformaInvoiceHistory CreateObject(NullHandler oReader)
        {
            ProformaInvoiceHistory oProformaInvoiceHistory = new ProformaInvoiceHistory();
            oProformaInvoiceHistory = MapObject(oReader);
            return oProformaInvoiceHistory;
        }

        private List<ProformaInvoiceHistory> CreateObjects(IDataReader oReader)
        {
            List<ProformaInvoiceHistory> oProformaInvoiceHistory = new List<ProformaInvoiceHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProformaInvoiceHistory oItem = CreateObject(oHandler);
                oProformaInvoiceHistory.Add(oItem);
            }
            return oProformaInvoiceHistory;
        }

        #endregion

        #region Interface implementation
        public ProformaInvoiceHistoryService() { }

        public ProformaInvoiceHistory Save(ProformaInvoiceHistory oProformaInvoiceHistory, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<ProformaInvoiceHistory> _oProformaInvoiceHistorys = new List<ProformaInvoiceHistory>();
            oProformaInvoiceHistory.ErrorMessage = "";

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProformaInvoiceHistory.ProformaInvoiceHistoryID <= 0)
                {

                    reader = ProformaInvoiceHistoryDA.InsertUpdate(tc, oProformaInvoiceHistory, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    reader = ProformaInvoiceHistoryDA.InsertUpdate(tc, oProformaInvoiceHistory, EnumDBOperation.Update, nUserID, "");
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProformaInvoiceHistory = new ProformaInvoiceHistory();
                    oProformaInvoiceHistory = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProformaInvoiceHistory.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProformaInvoiceHistory. Because of " + e.Message, e);
                #endregion
            }
            return oProformaInvoiceHistory;
        }


        public ProformaInvoiceHistory Get(int id, Int64 nUserId)
        {
            ProformaInvoiceHistory oAccountHead = new ProformaInvoiceHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProformaInvoiceHistoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProformaInvoiceHistory", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProformaInvoiceHistory> Gets(int ProfromaInvoiceID, Int64 nUserID)
        {
            List<ProformaInvoiceHistory> oProformaInvoiceHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceHistoryDA.Gets(ProfromaInvoiceID, tc);
                oProformaInvoiceHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceHistory", e);
                #endregion
            }

            return oProformaInvoiceHistory;
        }

        public List<ProformaInvoiceHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<ProformaInvoiceHistory> oProformaInvoiceHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceHistoryDA.Gets(tc, sSQL);
                oProformaInvoiceHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceHistory", e);
                #endregion
            }

            return oProformaInvoiceHistory;
        }
        #endregion
    }
    

}
