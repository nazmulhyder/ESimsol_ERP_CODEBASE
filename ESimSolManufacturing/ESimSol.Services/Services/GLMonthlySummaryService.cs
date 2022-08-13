
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
    public class GLMonthlySummaryService : MarshalByRefObject, IGLMonthlySummaryService
    {
        #region Private functions and declaration
        bool _bIsDateRange = false;
        private GLMonthlySummary MapObject(NullHandler oReader)
        {
            GLMonthlySummary oGLMonthlySummary = new GLMonthlySummary();
            oGLMonthlySummary.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oGLMonthlySummary.ComponentType = (EnumComponentType)oReader.GetInt32("ComponentType");
            oGLMonthlySummary.DebitAmount = oReader.GetDouble("DebitAmount");
            oGLMonthlySummary.CreditAmount = oReader.GetDouble("CreditAmount");
            oGLMonthlySummary.ClosingAmount = oReader.GetDouble("ClosingAmount");
            oGLMonthlySummary.NameOfMonth = oReader.GetString("NameOfMonth");
            oGLMonthlySummary.StartDate = oReader.GetDateTime("StartDate");
            oGLMonthlySummary.EndDate = oReader.GetDateTime("EndDate");
            return oGLMonthlySummary;
        }

        private GLMonthlySummary CreateObject(NullHandler oReader)
        {
            GLMonthlySummary oGLMonthlySummary = new GLMonthlySummary();
            oGLMonthlySummary = MapObject(oReader);
            return oGLMonthlySummary;
        }

        private List<GLMonthlySummary> CreateObjects(IDataReader oReader)
        {
            List<GLMonthlySummary> oGLMonthlySummary = new List<GLMonthlySummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GLMonthlySummary oItem = CreateObject(oHandler);
                oGLMonthlySummary.Add(oItem);
            }
            return oGLMonthlySummary;
        }

        #endregion

        #region Interface implementation
        public GLMonthlySummaryService() { }
        public List<GLMonthlySummary> Gets(int nAccountHead, DateTime dStartDate, DateTime dEndDate, int nCurrencyID, bool bISApproved, string nBusinessUnitIDs, int nUserID)
        {
            List<GLMonthlySummary> oGLMonthlySummary = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GLMonthlySummaryDA.Gets(tc, nAccountHead, dStartDate, dEndDate, nCurrencyID, bISApproved, nBusinessUnitIDs);
                _bIsDateRange = true;
                oGLMonthlySummary = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GLMonthlySummary", e);
                #endregion
            }

            return oGLMonthlySummary;
        }
       
        #endregion
    }
}
