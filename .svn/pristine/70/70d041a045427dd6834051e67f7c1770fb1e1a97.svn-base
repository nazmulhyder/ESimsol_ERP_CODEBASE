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
    public class PTUUnit2LogService : MarshalByRefObject, IPTUUnit2LogService
    {
        #region Private functions and declaration

        private PTUUnit2Log MapObject(NullHandler oReader)
        {
            PTUUnit2Log oPTUUnit2Log = new PTUUnit2Log();
            oPTUUnit2Log.PTUUnit2LogID = oReader.GetInt32("PTUUnit2LogID");
            oPTUUnit2Log.PTUUnit2ID = oReader.GetInt32("PTUUnit2ID");
            oPTUUnit2Log.BalanceQty = oReader.GetDouble("BalanceQty");
            oPTUUnit2Log.Qty = oReader.GetDouble("Qty");
            oPTUUnit2Log.RefNo = oReader.GetString("RefNo");
            oPTUUnit2Log.Note = oReader.GetString("Note");
            oPTUUnit2Log.PTUUnit2RefID = oReader.GetInt32("PTUUnit2RefID");
            oPTUUnit2Log.PTUUnit2RefType =  (EnumPTUUnit2Ref) oReader.GetInt32("PTUUnit2RefType");
            oPTUUnit2Log.PTUUnit2RefTypeInInt = oReader.GetInt32("PTUUnit2RefType");
            oPTUUnit2Log.SheetWiseActualFinish = oReader.GetDouble("SheetWiseActualFinish");
            oPTUUnit2Log.SheetWiseReject = oReader.GetDouble("SheetWiseReject");
            oPTUUnit2Log.POApproveByName = oReader.GetString("POApproveByName");
            oPTUUnit2Log.PODate = oReader.GetDateTime("PODate");
            oPTUUnit2Log.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oPTUUnit2Log.EntryPerson = oReader.GetString("EntryPerson");
            oPTUUnit2Log.MUSymbol = oReader.GetString("MUSymbol");
            oPTUUnit2Log.WorkingUnitName = oReader.GetString("WorkingUnitName");
            oPTUUnit2Log.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oPTUUnit2Log.DOApprovedByName = oReader.GetString("DOApprovedByName");
            oPTUUnit2Log.ContractBUName = oReader.GetString("ContractBUName");
            oPTUUnit2Log.ContractDate = oReader.GetDateTime("ContractDate");
            return oPTUUnit2Log;
        }

        private PTUUnit2Log CreateObject(NullHandler oReader)
        {
            PTUUnit2Log oPTUUnit2Log = new PTUUnit2Log();
            oPTUUnit2Log = MapObject(oReader);
            return oPTUUnit2Log;
        }

        private List<PTUUnit2Log> CreateObjects(IDataReader oReader)
        {
            List<PTUUnit2Log> oPTUUnit2Log = new List<PTUUnit2Log>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PTUUnit2Log oItem = CreateObject(oHandler);
                oPTUUnit2Log.Add(oItem);
            }
            return oPTUUnit2Log;
        }

        #endregion

        #region Interface implementation

        public PTUUnit2Log Get(int id, Int64 nUserId)
        {
            PTUUnit2Log oPTUUnit2Log = new PTUUnit2Log();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PTUUnit2LogDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUUnit2Log = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PTUUnit2Log", e);
                #endregion
            }
            return oPTUUnit2Log;
        }

        public List<PTUUnit2Log> Gets(int nID, int nEType, Int64 nUserID)
        {
            List<PTUUnit2Log> oPTUUnit2Logs = new List<PTUUnit2Log>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUUnit2LogDA.Gets(tc, nID,  nEType);
                oPTUUnit2Logs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                PTUUnit2Log oPTUUnit2Log = new PTUUnit2Log();
                oPTUUnit2Log.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPTUUnit2Logs;
        }

        public List<PTUUnit2Log> Gets(string sSQL, Int64 nUserID)
        {
            List<PTUUnit2Log> oPTUUnit2Logs = new List<PTUUnit2Log>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUUnit2LogDA.Gets(tc, sSQL);
                oPTUUnit2Logs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUUnit2Log", e);
                #endregion
            }
            return oPTUUnit2Logs;
        }

        #endregion
    }

    
   
}
