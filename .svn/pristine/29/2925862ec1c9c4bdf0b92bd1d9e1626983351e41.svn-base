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
    public class ExportTRService : MarshalByRefObject, IExportTRService
    {
        #region Private functions and declaration
        private ExportTR MapObject(NullHandler oReader)
        {
            ExportTR oExportTR = new ExportTR();
            oExportTR.ExportTRID = oReader.GetInt32("ExportTRID");
            oExportTR.TruckReceiptNo = oReader.GetString("TruckReceiptNo");
            oExportTR.TruckReceiptDate = oReader.GetDateTime("TruckReceiptDate");
            oExportTR.Carrier = oReader.GetString("Carrier");
            oExportTR.TruckNo = oReader.GetString("TruckNo");
            oExportTR.DriverName = oReader.GetString("DriverName");
            oExportTR.Activity = oReader.GetBoolean("Activity");
            oExportTR.BUID = oReader.GetInt32("BUID");
            return oExportTR;
        }
        private ExportTR CreateObject(NullHandler oReader)
        {
            ExportTR oExportTR = new ExportTR();
            oExportTR = MapObject(oReader);
            return oExportTR;
        }
        private List<ExportTR> CreateObjects(IDataReader oReader)
        {
            List<ExportTR> oExportTR = new List<ExportTR>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportTR oItem = CreateObject(oHandler);
                oExportTR.Add(oItem);
            }
            return oExportTR;
        }
        #endregion

        #region Interface implementation
        public ExportTR Save(ExportTR oExportTR, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportTR.ExportTRID <= 0)
                {
                    reader = ExportTRDA.InsertUpdate(tc, oExportTR, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportTRDA.InsertUpdate(tc, oExportTR, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportTR = new ExportTR();
                    oExportTR = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportTR = new ExportTR();
                oExportTR.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportTR;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportTR oExportTR = new ExportTR();
                oExportTR.ExportTRID = id;
                ExportTRDA.Delete(tc, oExportTR, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public ExportTR Get(int id, Int64 nUserId)
        {
            ExportTR oAccountHead = new ExportTR();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportTRDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ExportTR", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<ExportTR> Gets(Int64 nUserID)
        {
            List<ExportTR> oExportTR = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportTRDA.Gets(tc);
                oExportTR = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportTR", e);
                #endregion
            }
            return oExportTR;
        }
        public List<ExportTR> Gets(bool dActivity, int nBUID, Int64 nUserID)
        {
            List<ExportTR> oExportTR = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportTRDA.Gets(tc, dActivity, nBUID);
                oExportTR = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportTR", e);
                #endregion
            }
            return oExportTR;
        }
        public List<ExportTR> BUWiseGets( int nBUID, Int64 nUserID)
        {
            List<ExportTR> oExportTR = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportTRDA.BUWiseGets(tc, nBUID);
                oExportTR = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportTR", e);
                #endregion
            }
            return oExportTR;
        }
        
        public List<ExportTR> Gets (string sSQL, Int64 nUserID)
        {
            List<ExportTR> oExportTR = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportTRDA.Gets(tc, sSQL);
                oExportTR = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportTR", e);
                #endregion
            }
            return oExportTR;
        }
        
        #endregion
    }
}
