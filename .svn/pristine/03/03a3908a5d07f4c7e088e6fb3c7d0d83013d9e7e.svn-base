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
    public class CompanyRuleDescriptionService : MarshalByRefObject, ICompanyRuleDescriptionService
    {
        #region Private functions and declaration
        private CompanyRuleDescription MapObject(NullHandler oReader)
        {
            CompanyRuleDescription oCompanyRuleDescription = new CompanyRuleDescription();

            oCompanyRuleDescription.CRDID = oReader.GetInt32("CRDID");
            oCompanyRuleDescription.CRNID = oReader.GetInt32("CRNID");
            oCompanyRuleDescription.Description = oReader.GetString("Description");
            oCompanyRuleDescription.IsActive = oReader.GetBoolean("IsActive");
            oCompanyRuleDescription.Header = oReader.GetString("Header");

            return oCompanyRuleDescription;
        }

        private CompanyRuleDescription CreateObject(NullHandler oReader)
        {
            CompanyRuleDescription oCompanyRuleDescription = MapObject(oReader);
            return oCompanyRuleDescription;
        }

        private List<CompanyRuleDescription> CreateObjects(IDataReader oReader)
        {
            List<CompanyRuleDescription> oCompanyRuleDescriptions = new List<CompanyRuleDescription>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CompanyRuleDescription oItem = CreateObject(oHandler);
                oCompanyRuleDescriptions.Add(oItem);
            }
            return oCompanyRuleDescriptions;
        }

        #endregion

        #region Interface implementation
        public CompanyRuleDescriptionService() { }

        public CompanyRuleDescription IUD(CompanyRuleDescription oCompanyRuleDescription, int nDBOperation, Int64 nUserID)
        {

            CompanyRuleName oCompanyRuleName = new CompanyRuleName();
            oCompanyRuleName = oCompanyRuleDescription.CompanyRuleName;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CompanyRuleDescriptionDA.IUD(tc, oCompanyRuleDescription, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oCompanyRuleDescription = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if(nDBOperation==3)
                {
                    oCompanyRuleDescription.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCompanyRuleDescription.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCompanyRuleDescription.CRNID = 0;
                #endregion
            }
            return oCompanyRuleDescription;
        }

        public CompanyRuleDescription Get(int nCRNID, Int64 nUserId)
        {
            CompanyRuleDescription oCompanyRuleDescription = new CompanyRuleDescription();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CompanyRuleDescriptionDA.Get(nCRNID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyRuleDescription = CreateObject(oReader);
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

                oCompanyRuleDescription.ErrorMessage = e.Message;
                #endregion
            }

            return oCompanyRuleDescription;
        }

        public CompanyRuleDescription Get(string sSQL, Int64 nUserId)
        {
            CompanyRuleDescription oCompanyRuleDescription = new CompanyRuleDescription();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CompanyRuleDescriptionDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyRuleDescription = CreateObject(oReader);
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

                oCompanyRuleDescription.ErrorMessage = e.Message;
                #endregion
            }

            return oCompanyRuleDescription;
        }

        public List<CompanyRuleDescription> Gets(Int64 nUserID)
        {
            List<CompanyRuleDescription> oCompanyRuleDescription = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyRuleDescriptionDA.Gets(tc);
                oCompanyRuleDescription = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CompanyRuleDescription", e);
                #endregion
            }
            return oCompanyRuleDescription;
        }

        public List<CompanyRuleDescription> Gets(string sSQL, Int64 nUserID)
        {
            List<CompanyRuleDescription> oCompanyRuleDescription = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyRuleDescriptionDA.Gets(sSQL, tc);
                oCompanyRuleDescription = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CompanyRuleDescription", e);
                #endregion
            }
            return oCompanyRuleDescription;
        }

        #endregion

        #region Activity
        public CompanyRuleDescription Activite(int nCRDID, Int64 nUserId)
        {
            CompanyRuleDescription oCompanyRuleDescription = new CompanyRuleDescription();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CompanyRuleDescriptionDA.Activity(nCRDID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyRuleDescription = CreateObject(oReader);
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
                oCompanyRuleDescription.ErrorMessage = e.Message;
                #endregion
            }

            return oCompanyRuleDescription;
        }


        #endregion

    }
}
