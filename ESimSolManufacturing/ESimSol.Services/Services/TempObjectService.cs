using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class TempObjectService : MarshalByRefObject, ITempObjectService
    {
        #region Private functions and declaration
        private TempObject MapObject(NullHandler oReader)
        {
            TempObject oTempObject = new TempObject();
            oTempObject.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oTempObject.AccountHeadCode = oReader.GetString("AccountHeadCode");
            oTempObject.AccountHeadName = oReader.GetString("AccountHeadName");
            oTempObject.SubGroupName = oReader.GetString("SubGroupName");
            oTempObject.IsDebit = oReader.GetBoolean("IsDebit");
            oTempObject.OCSID = oReader.GetInt32("OCSID");
            oTempObject.LedgerGroupSetUpID = oReader.GetInt32("LedgerGroupSetUpID");
            oTempObject.LedgerGroupName = oReader.GetString("LedgerGroupName");
            return oTempObject;
        }

        private TempObject CreateObject(NullHandler oReader)
        {
            TempObject oTempObject = new TempObject();
            oTempObject = MapObject(oReader);
            return oTempObject;
        }

        private List<TempObject> CreateObjects(IDataReader oReader)
        {
            List<TempObject> oTempObject = new List<TempObject>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TempObject oItem = CreateObject(oHandler);
                oTempObject.Add(oItem);
            }
            return oTempObject;
        }

        #endregion

        public List<TempObject> Gets(int nStatementSetupID, DateTime dstartDate, DateTime dendDate, int nBUID, int nUserId)
        {
            List<TempObject> oTempObjects = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TempObjectDA.Gets(tc, nStatementSetupID, dstartDate, dendDate, nBUID);
                oTempObjects = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oTempObjects;
        }
    }
}
