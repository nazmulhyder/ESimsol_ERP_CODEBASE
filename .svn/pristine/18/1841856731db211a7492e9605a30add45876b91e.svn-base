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
    public class CompanyRuleNameService : MarshalByRefObject, ICompanyRuleNameService
    {
        #region Private functions and declaration
        private CompanyRuleName MapObject(NullHandler oReader)
        {
            CompanyRuleName oCompanyRuleName = new CompanyRuleName();

            oCompanyRuleName.CRNID = oReader.GetInt32("CRNID");
            oCompanyRuleName.Description = oReader.GetString("Description");
            oCompanyRuleName.IsActive = oReader.GetBoolean("IsActive");
            oCompanyRuleName.EncryptedID = Global.Encrypt(oCompanyRuleName.CRNID.ToString());
            return oCompanyRuleName;
        }

        private CompanyRuleName CreateObject(NullHandler oReader)
        {
            CompanyRuleName oCompanyRuleName = MapObject(oReader);
            return oCompanyRuleName;
        }

        private List<CompanyRuleName> CreateObjects(IDataReader oReader)
        {
            List<CompanyRuleName> oCompanyRuleNames = new List<CompanyRuleName>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CompanyRuleName oItem = CreateObject(oHandler);
                oCompanyRuleNames.Add(oItem);
            }
            return oCompanyRuleNames;
        }

        #endregion

        #region Interface implementation
        public CompanyRuleNameService() { }

        public CompanyRuleName IUD(CompanyRuleName oCompanyRuleName, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CompanyRuleNameDA.IUD(tc, oCompanyRuleName, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyRuleName = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oCompanyRuleName.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCompanyRuleName.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCompanyRuleName.CRNID = 0;
                #endregion
            }
            return oCompanyRuleName;
        }

        public CompanyRuleName Get(int nCRNID, Int64 nUserId)
        {
            CompanyRuleName oCompanyRuleName = new CompanyRuleName();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CompanyRuleNameDA.Get(nCRNID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyRuleName = CreateObject(oReader);
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

                oCompanyRuleName.ErrorMessage = e.Message;
                #endregion
            }

            return oCompanyRuleName;
        }

        public CompanyRuleName Get(string sSQL, Int64 nUserId)
        {
            CompanyRuleName oCompanyRuleName = new CompanyRuleName();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CompanyRuleNameDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyRuleName = CreateObject(oReader);
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

                oCompanyRuleName.ErrorMessage = e.Message;
                #endregion
            }

            return oCompanyRuleName;
        }

        public List<CompanyRuleName> Gets(Int64 nUserID)
        {
            List<CompanyRuleName> oCompanyRuleName = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyRuleNameDA.Gets(tc);
                oCompanyRuleName = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CompanyRuleName", e);
                #endregion
            }
            return oCompanyRuleName;
        }

        public List<CompanyRuleName> Gets(string sSQL, Int64 nUserID)
        {
            List<CompanyRuleName> oCompanyRuleName = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CompanyRuleNameDA.Gets(sSQL, tc);
                oCompanyRuleName = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CompanyRuleName", e);
                #endregion
            }
            return oCompanyRuleName;
        }

        #endregion

        #region Activity
        public CompanyRuleName Activite(int nCRNID, Int64 nUserId)
        {
            CompanyRuleName oCompanyRuleName = new CompanyRuleName();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CompanyRuleNameDA.Activity(nCRNID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCompanyRuleName = CreateObject(oReader);
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
                oCompanyRuleName.ErrorMessage = e.Message;
                #endregion
            }

            return oCompanyRuleName;
        }


        #endregion

    }
}
