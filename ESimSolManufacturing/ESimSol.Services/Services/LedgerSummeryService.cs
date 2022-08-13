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
    public class LedgerSummeryService : MarshalByRefObject, ILedgerSummeryService
    {
        #region Private functions and declaration

        private LedgerSummery MapObject(NullHandler oReader)
        {
            LedgerSummery oLedgerSummery = new LedgerSummery();
            oLedgerSummery.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oLedgerSummery.CurrencyID = oReader.GetInt32("CurrencyID");
            oLedgerSummery.IsDebit = oReader.GetBoolean("IsDebit");
            oLedgerSummery.DrAmount = oReader.GetDouble("DrAmount");
            oLedgerSummery.CrAmount = oReader.GetDouble("CrAmount");
            oLedgerSummery.BUID = oReader.GetInt32("BUID");
            oLedgerSummery.AccountHeadCode = oReader.GetString("AccountHeadCode");
            oLedgerSummery.AccountHeadName = oReader.GetString("AccountHeadName");

            return oLedgerSummery;
        }

        private LedgerSummery CreateObject(NullHandler oReader)
        {
            LedgerSummery oLedgerSummery = new LedgerSummery();
            oLedgerSummery = MapObject(oReader);
            return oLedgerSummery;
        }

        private List<LedgerSummery> CreateObjects(IDataReader oReader)
        {
            List<LedgerSummery> oLedgerSummery = new List<LedgerSummery>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LedgerSummery oItem = CreateObject(oHandler);
                oLedgerSummery.Add(oItem);
            }
            return oLedgerSummery;
        }

        #endregion

        #region Interface implementation
        public List<LedgerSummery> Gets(LedgerSummery oLedgerSummery, Int64 nUserID)
        {
            List<LedgerSummery> oLedgerSummerys = new List<LedgerSummery>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LedgerSummeryDA.Gets(tc, oLedgerSummery);
                oLedgerSummerys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LedgerSummery", e);
                #endregion
            }
            return oLedgerSummerys;
        }

        #endregion
    }

}
