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
    public class ELSetupService : MarshalByRefObject, IELSetupService
    {
        #region Private functions and declaration
        private ELSetup MapObject(NullHandler oReader)
        {
            ELSetup oELSetup = new ELSetup();
            oELSetup.ELSetupID = oReader.GetInt32("ELSetupID");
            oELSetup.IsConsiderLeave = oReader.GetBoolean("IsConsiderLeave");
            oELSetup.IsConsiderDayOff = oReader.GetBoolean("IsConsiderDayOff");
            oELSetup.IsConsiderHoliday = oReader.GetBoolean("IsConsiderHoliday");
            oELSetup.IsConsiderAbsent = oReader.GetBoolean("IsConsiderAbsent");
            oELSetup.InactiveBy = oReader.GetInt32("InactiveBy");
            oELSetup.InactiveDate = oReader.GetDateTime("InactiveDate");
            oELSetup.ApproveBy = oReader.GetInt32("ApproveBy");
            oELSetup.ApproveByName = oReader.GetString("ApproveByName");
            oELSetup.InactiveByName = oReader.GetString("InactiveByName");
            oELSetup.ApproveDate = oReader.GetDateTime("ApproveDate");
            return oELSetup;
        }
        private ELSetup CreateObject(NullHandler oReader)
        {
            ELSetup oELSetup = new ELSetup();
            oELSetup = MapObject(oReader);
            return oELSetup;
        }
        private List<ELSetup> CreateObjects(IDataReader oReader)
        {
            List<ELSetup> oELSetup = new List<ELSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ELSetup oItem = CreateObject(oHandler);
                oELSetup.Add(oItem);
            }
            return oELSetup;
        }
        #endregion

        #region Interface implementation


        public ELSetup Save(ELSetup oELSetup, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oELSetup.ELSetupID <= 0)
                {
                    reader = ELSetupDA.InsertUpdate(tc, oELSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ELSetupDA.InsertUpdate(tc, oELSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELSetup = new ELSetup();
                    oELSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to save ELSetup " + e.Message, e);
                #endregion
            }
            return oELSetup;
        }


        public string Delete(int id, int nUserId)
        {
            string message = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ELSetup oELSetup = new ELSetup();
                oELSetup.ELSetupID = id;
                ELSetupDA.Delete(tc, oELSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
                message = Global.DeleteMessage;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                message = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }
            return message;
        }


        public ELSetup Get(string sSQL, Int64 nUserId)
        {
            ELSetup oELSetup = new ELSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ELSetupDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELSetup = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oELSetup;
        }


        public List<ELSetup> Gets(string sSQL, int nUserId)
        {
            List<ELSetup> oELSetup = new List<ELSetup>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ELSetupDA.Gets(tc, sSQL);
                oELSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ELSetup", e);
                #endregion
            }

            return oELSetup;
        }


        public List<ELSetup> Gets(int nUserId)
        {
            List<ELSetup> oELSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ELSetupDA.Gets(tc);
                oELSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ELSetup ", e);
                #endregion
            }

            return oELSetup;
        }


        public ELSetup Approve(ELSetup oELSetup, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oELSetup.ApproveBy <= 0)
                {
                    // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseQuotation", EnumRoleOperationType.Approved);
                    reader = ELSetupDA.InsertUpdate(tc, oELSetup, EnumDBOperation.Approval, nUserID);
                }
                else
                {
                    // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseQuotation", EnumRoleOperationType.Approved);
                    reader = ELSetupDA.InsertUpdate(tc, oELSetup, EnumDBOperation.Approval, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELSetup = new ELSetup();
                    oELSetup = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oELSetup = new ELSetup();
                oELSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oELSetup;
        }



        public ELSetup Inactive(ELSetup oELSetup, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oELSetup.InactiveBy <= 0)
                {
                    // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseQuotation", EnumRoleOperationType.Approved);
                    reader = ELSetupDA.InsertUpdate(tc, oELSetup, EnumDBOperation.InActive, nUserID);
                }
                else
                {
                    // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseQuotation", EnumRoleOperationType.Approved);
                    reader = ELSetupDA.InsertUpdate(tc, oELSetup, EnumDBOperation.InActive, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELSetup = new ELSetup();
                    oELSetup = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oELSetup = new ELSetup();
                oELSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oELSetup;
        }


        #endregion

       
    }
}
