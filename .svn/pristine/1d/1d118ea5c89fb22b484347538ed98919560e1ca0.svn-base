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
    public class VOSummeryService : MarshalByRefObject, IVOSummeryService
    {
        #region Private functions and declaration
        private VOSummery MapObject(NullHandler oReader)
        {
            VOSummery oVOSummery = new VOSummery();
            oVOSummery.VOrderID = oReader.GetInt32("VOrderID");
            oVOSummery.OrderNo = oReader.GetString("OrderNo");
            oVOSummery.BUID = oReader.GetInt32("BUID");
            oVOSummery.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oVOSummery.SubledgerID = oReader.GetInt32("SubledgerID");
            oVOSummery.CurrencyID = oReader.GetInt32("CurrencyID");
            oVOSummery.CSymbol = oReader.GetString("CSymbol");
            oVOSummery.OpenIsDebit = oReader.GetBoolean("OpenIsDebit");
            oVOSummery.OpenDebit = oReader.GetDouble("OpenDebit");
            oVOSummery.OpenCredit = oReader.GetDouble("OpenCredit");
            oVOSummery.OpeningAmount = oReader.GetDouble("OpeningAmount");
            oVOSummery.DebitAmount = oReader.GetDouble("DebitAmount");
            oVOSummery.CreditAmount = oReader.GetDouble("CreditAmount");
            oVOSummery.CloseIsDebit = oReader.GetBoolean("CloseIsDebit");
            oVOSummery.ClosingBalance = oReader.GetDouble("ClosingBalance");
            oVOSummery.DueDays = oReader.GetInt32("DueDays");
            return oVOSummery;
        }

        private VOSummery CreateObject(NullHandler oReader)
        {
            VOSummery oVOSummery = new VOSummery();
            oVOSummery = MapObject(oReader);
            return oVOSummery;
        }

        private List<VOSummery> CreateObjects(IDataReader oReader)
        {
            List<VOSummery> oVOSummery = new List<VOSummery>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VOSummery oItem = CreateObject(oHandler);
                oVOSummery.Add(oItem);
            }
            return oVOSummery;
        }

        #endregion

        #region Interface implementation
        public VOSummeryService() { }

        public List<VOSummery> GetsVOSummerys(VOSummery oVOSummery, int nUserID)
        {
            List<VOSummery> oVOSummerys = new List<VOSummery>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VOSummeryDA.GetsVOSummerys(tc, oVOSummery, nUserID);
                oVOSummerys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VOSummery", e);
                #endregion
            }
            return oVOSummerys;
        }

        #endregion
    }
}
