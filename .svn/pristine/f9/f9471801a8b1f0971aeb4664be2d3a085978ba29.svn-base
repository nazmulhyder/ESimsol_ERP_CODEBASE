using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class EmailConfigService : MarshalByRefObject, IEmailConfigService
    {
        #region Private functions and declaration
        private EmailConfig MapObject(NullHandler oReader)
        {
            EmailConfig oEmailConfig = new EmailConfig();
            oEmailConfig.EmailConfigID = oReader.GetInt32("EmailConfigID");
            oEmailConfig.BUID = oReader.GetInt32("BUID");
            oEmailConfig.BUName = oReader.GetString("BUName");
            oEmailConfig.EmailAddress = oReader.GetString("EmailAddress");
            oEmailConfig.EmailPassword = oReader.GetString("EmailPassword");
            oEmailConfig.Remarks = oReader.GetString("Remarks");
            oEmailConfig.PortNumber = oReader.GetString("PortNumber");
            oEmailConfig.HostName = oReader.GetString("HostName");
            oEmailConfig.EmailDisplayName = oReader.GetString("EmailDisplayName");
            oEmailConfig.SSLRequired = oReader.GetBoolean("SSLRequired");
            return oEmailConfig;
        }

        private EmailConfig CreateObject(NullHandler oReader)
        {
            EmailConfig oEmailConfig = new EmailConfig();
            oEmailConfig = MapObject(oReader);
            return oEmailConfig;
        }

        private List<EmailConfig> CreateObjects(IDataReader oReader)
        {
            List<EmailConfig> oEmailConfig = new List<EmailConfig>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmailConfig oItem = CreateObject(oHandler);
                oEmailConfig.Add(oItem);
            }
            return oEmailConfig;
        }

        #endregion

        #region Interface implementation
        public EmailConfigService() { }

        public EmailConfig Save(EmailConfig oEmailConfig, Int64 nUserID)
        {
            TransactionContext tc = null;
            oEmailConfig.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEmailConfig.EmailConfigID <= 0)
                {
                   
                    reader = EmailConfigDA.InsertUpdate(tc, oEmailConfig, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                   
                    reader = EmailConfigDA.InsertUpdate(tc, oEmailConfig, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmailConfig = new EmailConfig();
                    oEmailConfig = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oEmailConfig.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save EmailConfig. Because of " + e.Message, e);
                #endregion
            }
            return oEmailConfig;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmailConfig oEmailConfig = new EmailConfig();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.EmailConfig, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "EmailConfig", id);
                oEmailConfig.EmailConfigID = id;
                EmailConfigDA.Delete(tc, oEmailConfig, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete EmailConfig. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public EmailConfig Get(int id, Int64 nUserId)
        {
            EmailConfig oAccountHead = new EmailConfig();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmailConfigDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmailConfig", e);
                #endregion
            }

            return oAccountHead;
        }

        public EmailConfig GetByBU(int nBUID, Int64 nUserId)
        {
            EmailConfig oAccountHead = new EmailConfig();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmailConfigDA.GetByBU(tc, nBUID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmailConfig", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<EmailConfig> Gets(Int64 nUserID)
        {
            List<EmailConfig> oEmailConfig = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmailConfigDA.Gets(tc);
                oEmailConfig = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmailConfig", e);
                #endregion
            }

            return oEmailConfig;
        }
      
        public List<EmailConfig> Gets(string sSQL, Int64 nUserID)
        {
            List<EmailConfig> oEmailConfig = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmailConfigDA.Gets(tc, sSQL);
                oEmailConfig = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmailConfig", e);
                #endregion
            }

            return oEmailConfig;
        }

        #endregion
    }
}
