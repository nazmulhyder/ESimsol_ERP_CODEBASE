using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.Services
{
    public class WUSubContractTermsConditionService : MarshalByRefObject, IWUSubContractTermsConditionService
    {
        #region Private functions and declaration
        private WUSubContractTermsCondition MapObject(NullHandler oReader)
        {
            WUSubContractTermsCondition oWUSubContractTermsCondition = new WUSubContractTermsCondition();
            oWUSubContractTermsCondition.WUSubContractTermsConditionID = oReader.GetInt32("WUSubContractTermsConditionID");
            oWUSubContractTermsCondition.WUSubContractID = oReader.GetInt32("WUSubContractID");
            oWUSubContractTermsCondition.TermsAndCondition = oReader.GetString("TermsAndCondition");
            return oWUSubContractTermsCondition;
        }

        private List<WUSubContractTermsCondition> CreateObjects(IDataReader oReader)
        {
            List<WUSubContractTermsCondition> oWUSubContractTermsConditions = new List<WUSubContractTermsCondition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WUSubContractTermsCondition oType = CreateObject(oHandler);
                oWUSubContractTermsConditions.Add(oType);
            }
            return oWUSubContractTermsConditions;
        }

        private WUSubContractTermsCondition CreateObject(NullHandler oReader)
        {
            WUSubContractTermsCondition oWUSubContractTermsCondition = new WUSubContractTermsCondition();
            oWUSubContractTermsCondition = MapObject(oReader);
            return oWUSubContractTermsCondition;
        }
        #endregion

        #region Interface implementation
        public WUSubContractTermsConditionService() { }

        public List<WUSubContractTermsCondition> Gets(int id, Int64 nUserID)
        {
            List<WUSubContractTermsCondition> oWUSubContractTermsConditions = new List<WUSubContractTermsCondition>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUSubContractTermsConditionDA.Gets(tc, id);
                oWUSubContractTermsConditions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                WUSubContractTermsCondition oWUSubContractTermsCondition = new WUSubContractTermsCondition();
                oWUSubContractTermsCondition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }

            return oWUSubContractTermsConditions;
        }

        public List<WUSubContractTermsCondition> Get(string sSQL, int nCurrentUserID)
        {
            List<WUSubContractTermsCondition> oWUSubContractTermsCondition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUSubContractTermsConditionDA.Get(tc, sSQL);
                oWUSubContractTermsCondition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WUSubContractTermsCondition", e);
                #endregion
            }

            return oWUSubContractTermsCondition;
        }

        #endregion
    }
}
