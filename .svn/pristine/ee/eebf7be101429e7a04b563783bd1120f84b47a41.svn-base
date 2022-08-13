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
    public class SP_ChangesInEquityService : MarshalByRefObject, ISP_ChangesInEquityService
    {
        #region Private functions and declaration
        private SP_ChangesInEquity MapObject(NullHandler oReader)
        {
            SP_ChangesInEquity oSP_ChangesInEquity = new SP_ChangesInEquity();
            oSP_ChangesInEquity.TransactionType = (EnumEquityTransactionType)oReader.GetInt16("TransactionType");
            oSP_ChangesInEquity.ShareCapital = oReader.GetDouble("ShareCapital");
            oSP_ChangesInEquity.SharePremium = oReader.GetDouble("SharePremium");
            oSP_ChangesInEquity.ExcessOfIssuePrice = oReader.GetDouble("ExcessOfIssuePrice");
            oSP_ChangesInEquity.CapitalReserve = oReader.GetDouble("CapitalReserve");
            oSP_ChangesInEquity.RevaluationSurplus = oReader.GetDouble("RevaluationSurplus");
            oSP_ChangesInEquity.FairValueGainOnInvestment = oReader.GetDouble("FairValueGainOnInvestment");
            oSP_ChangesInEquity.RetainedEarnings = oReader.GetDouble("RetainedEarnings");
            oSP_ChangesInEquity.TotalAmount = oReader.GetDouble("TotalAmount");
            
            
            return oSP_ChangesInEquity;
        }

        private SP_ChangesInEquity CreateObject(NullHandler oReader)
        {
            SP_ChangesInEquity oSP_ChangesInEquity = new SP_ChangesInEquity();
            oSP_ChangesInEquity = MapObject(oReader);
            return oSP_ChangesInEquity;
        }

        private List<SP_ChangesInEquity> CreateObjects(IDataReader oReader)
        {
            List<SP_ChangesInEquity> oSP_ChangesInEquity = new List<SP_ChangesInEquity>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SP_ChangesInEquity oItem = CreateObject(oHandler);
                oSP_ChangesInEquity.Add(oItem);
            }
            return oSP_ChangesInEquity;
        }

        #endregion

        #region Interface implementation
        public SP_ChangesInEquityService() { }



        public List<SP_ChangesInEquity> Gets(int nAccountingSessionID, int nBusinessUnitID, int nUserID)
        {
            List<SP_ChangesInEquity> oSP_ChangesInEquity = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SP_ChangesInEquityDA.Gets(tc, nAccountingSessionID, nBusinessUnitID);
                oSP_ChangesInEquity = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SP_ChangesInEquity", e);
                #endregion
            }

            return oSP_ChangesInEquity;
        }

      

       
        #endregion
    }   
}