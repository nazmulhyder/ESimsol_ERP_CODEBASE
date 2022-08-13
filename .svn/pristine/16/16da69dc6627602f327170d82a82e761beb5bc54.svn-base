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
    public class LeaveLedgerService : MarshalByRefObject, ILeaveLedgerService
    {
        #region Private functions and declaration
        private LeaveLedger MapObject(NullHandler oReader)
        {
            LeaveLedger oLeaveLedger = new LeaveLedger();
            oLeaveLedger.LeaveName = oReader.GetString("LeaveName");
            oLeaveLedger.TotalDay = oReader.GetInt32("TotalDay");
            oLeaveLedger.Addition = oReader.GetInt32("Addition");
            oLeaveLedger.Deduction = oReader.GetInt32("Deduction");
            oLeaveLedger.AppliedFull = oReader.GetInt32("AppliedFull");
            oLeaveLedger.AppliedHalf = oReader.GetInt32("AppliedHalf");
            oLeaveLedger.AppliedShort = oReader.GetInt32("AppliedShort");
            oLeaveLedger.RecommendedFull = oReader.GetInt32("RecommendedFull");
            oLeaveLedger.RecommendedHalf = oReader.GetInt32("RecommendedHalf");
            oLeaveLedger.RecommendedShort = oReader.GetInt32("RecommendedShort");
            oLeaveLedger.ApprovedFull = oReader.GetInt32("ApprovedFull");
            oLeaveLedger.ApprovedHalf = oReader.GetInt32("ApprovedHalf");
            oLeaveLedger.ApprovedShort = oReader.GetInt32("ApprovedShort");
            oLeaveLedger.Paid = oReader.GetInt32("Paid");
            oLeaveLedger.Unpaid = oReader.GetInt32("Unpaid");
            //oLeaveLedger.Balance = oReader.GetInt32("Balance");
            return oLeaveLedger;
        }

        private LeaveLedger CreateObject(NullHandler oReader)
        {
            LeaveLedger oLeaveLedger = MapObject(oReader);
            return oLeaveLedger;
        }

        private List<LeaveLedger> CreateObjects(IDataReader oReader)
        {
            List<LeaveLedger> oLeaveLedger = new List<LeaveLedger>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LeaveLedger oItem = CreateObject(oHandler);
                oLeaveLedger.Add(oItem);
            }
            return oLeaveLedger;
        }

        #endregion

        #region Interface implementation
        public LeaveLedgerService() { }

        public List<LeaveLedger> Gets(int nEmployeeID,int nACSID,Int64 nUserID)
        {
            List<LeaveLedger> oLeaveLedgers = new List<LeaveLedger>(); ;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LeaveLedgerDA.Gets( nEmployeeID,nACSID,tc);
                oLeaveLedgers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                LeaveLedger oLeaveLedger = new LeaveLedger();
                oLeaveLedger.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                oLeaveLedgers.Add(oLeaveLedger);
                #endregion
            }

            return oLeaveLedgers;
        }
        #endregion
    }
}
