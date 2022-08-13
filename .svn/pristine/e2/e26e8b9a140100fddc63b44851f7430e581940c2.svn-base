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
    public class SignatureSetupService : MarshalByRefObject, ISignatureSetupService
    {
        #region Private functions and declaration
        private SignatureSetup MapObject(NullHandler oReader)
        {
            SignatureSetup oSignatureSetup = new SignatureSetup();
            oSignatureSetup.SignatureSetupID = oReader.GetInt32("SignatureSetupID");
            oSignatureSetup.ReportModule = (EnumReportModule)oReader.GetInt32("ReportModule");
            oSignatureSetup.ReportModuleInt = oReader.GetInt32("ReportModule");
            oSignatureSetup.SignatureCaption = oReader.GetString("SignatureCaption");
            oSignatureSetup.SignatureSequence = oReader.GetInt32("SignatureSequence");
            oSignatureSetup.DisplayDataColumn = oReader.GetString("DisplayDataColumn");
            oSignatureSetup.DisplayFixedName = oReader.GetString("DisplayFixedName");
            return oSignatureSetup;
        }

        private SignatureSetup CreateObject(NullHandler oReader)
        {
            SignatureSetup oSignatureSetup = new SignatureSetup();
            oSignatureSetup = MapObject(oReader);
            return oSignatureSetup;
        }

        private List<SignatureSetup> CreateObjects(IDataReader oReader)
        {
            List<SignatureSetup> oSignatureSetup = new List<SignatureSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SignatureSetup oItem = CreateObject(oHandler);
                oSignatureSetup.Add(oItem);
            }
            return oSignatureSetup;
        }

        #endregion

        #region Interface implementation
        public SignatureSetupService() { }

        public SignatureSetup Save(SignatureSetup oSignatureSetup, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSignatureSetup.SignatureSetupID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SignatureSetup, EnumRoleOperationType.Add);
                    reader = SignatureSetupDA.InsertUpdate(tc, oSignatureSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SignatureSetup, EnumRoleOperationType.Edit);
                    reader = SignatureSetupDA.InsertUpdate(tc, oSignatureSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSignatureSetup = new SignatureSetup();
                    oSignatureSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSignatureSetup = new SignatureSetup();
                oSignatureSetup.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save SignatureSetup. Because of " + e.Message, e);
                #endregion
            }
            return oSignatureSetup;
        }
        public string Delete(SignatureSetup oSignatureSetup, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SignatureSetup, EnumRoleOperationType.Delete);
                SignatureSetupDA.Delete(tc, oSignatureSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete SignatureSetup. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public SignatureSetup Get(int id, int nUserId)
        {
            SignatureSetup oAccountHead = new SignatureSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SignatureSetupDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get SignatureSetup", e);
                #endregion
            }

            return oAccountHead;
        }



        public List<SignatureSetup> Gets(int nUserID)
        {
            List<SignatureSetup> oSignatureSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SignatureSetupDA.Gets(tc);
                oSignatureSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SignatureSetup", e);
                #endregion
            }

            return oSignatureSetup;
        }
        public List<SignatureSetup> Gets(string sSQL, int nUserID)
        {
            List<SignatureSetup> oSignatureSetup = new List<SignatureSetup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_SignatureSetup where SignatureSetupID in (1,2,80,272,347,370,60,45)";
                }
                reader = SignatureSetupDA.Gets(tc, sSQL);
                oSignatureSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SignatureSetup", e);
                #endregion
            }

            return oSignatureSetup;
        }

        public List<SignatureSetup> GetsByReportModule(EnumReportModule eReportModule, int nUserID)
        {
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SignatureSetupDA.GetsByReportModule(tc, eReportModule);
                oSignatureSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSignatureSetups = new List<SignatureSetup>();
                SignatureSetup oSignatureSetup = new SignatureSetup();
                oSignatureSetup.ErrorMessage = e.Message;
                oSignatureSetups.Add(oSignatureSetup);
                #endregion
            }
            return oSignatureSetups;
        }
        #endregion
    }
}
