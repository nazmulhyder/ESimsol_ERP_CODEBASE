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
    public class SP_RatioSetupService : MarshalByRefObject, ISP_RatioSetupService
    {
        #region Private functions and declaration
        private SP_RatioSetup MapObject(NullHandler oReader)
        {
            SP_RatioSetup oSP_RatioSetup = new SP_RatioSetup();
            oSP_RatioSetup.AccountingRatioSetupID = oReader.GetInt32("AccountingRatioSetupID");
            oSP_RatioSetup.Name = oReader.GetString("Name");
            oSP_RatioSetup.RatioFormat = (EnumRatioFormat)oReader.GetInt16("RatioFormat");
            oSP_RatioSetup.DivisibleName = oReader.GetString("DivisibleName");
            oSP_RatioSetup.DividerName = oReader.GetString("DividerName");
            oSP_RatioSetup.DivisibleAmount = oReader.GetDouble("DivisibleAmount");
            oSP_RatioSetup.DividerAmount = oReader.GetDouble("DividerAmount");
            oSP_RatioSetup.RatioBalance = oReader.GetDouble("RatioBalance");
            oSP_RatioSetup.RatioSetupType = (EnumRatioSetupType)oReader.GetInt16("RatioSetupType");
            oSP_RatioSetup.DivisibleComponent = oReader.GetInt32("DivisibleComponent");
            oSP_RatioSetup.DividerComponent = oReader.GetInt32("DividerComponent");
            return oSP_RatioSetup;
        }

        private SP_RatioSetup CreateObject(NullHandler oReader)
        {
            SP_RatioSetup oSP_RatioSetup = new SP_RatioSetup();
            oSP_RatioSetup = MapObject(oReader);
            return oSP_RatioSetup;
        }

        private List<SP_RatioSetup> CreateObjects(IDataReader oReader)
        {
            List<SP_RatioSetup> oSP_RatioSetup = new List<SP_RatioSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SP_RatioSetup oItem = CreateObject(oHandler);
                oSP_RatioSetup.Add(oItem);
            }
            return oSP_RatioSetup;
        }

        #endregion

        #region Interface implementation
        public SP_RatioSetupService() { }



        public List<SP_RatioSetup> Gets(int nAccountingRatioSetupID,DateTime dStartDate,DateTime dEndDate, int nBusinessUnitID, int nUserID)
        {
            List<SP_RatioSetup> oSP_RatioSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SP_RatioSetupDA.Gets(tc, nAccountingRatioSetupID, dStartDate, dEndDate, nBusinessUnitID);
                oSP_RatioSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SP_RatioSetup", e);
                #endregion
            }

            return oSP_RatioSetup;
        }

      

       
        #endregion
    }   
}