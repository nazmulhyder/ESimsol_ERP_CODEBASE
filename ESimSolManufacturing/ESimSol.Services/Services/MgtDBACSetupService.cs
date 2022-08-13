using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class MgtDBACSetupService : MarshalByRefObject, IMgtDBACSetupService
    {
        #region Private functions and declaration

        private MgtDBACSetup MapObject(NullHandler oReader) 
        {
            MgtDBACSetup oMgtDBACSetup = new MgtDBACSetup();
            oMgtDBACSetup.MgtDBACSetupID = oReader.GetInt32("MgtDBACSetupID");
            oMgtDBACSetup.MgtDBACType = (EnumMgtDBACType)oReader.GetInt32("MgtDBACType");
            oMgtDBACSetup.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oMgtDBACSetup.Remarks = oReader.GetString("Remarks");
            oMgtDBACSetup.AccountHeadName = oReader.GetString("AccountHeadName");

            return oMgtDBACSetup;
        }

        private MgtDBACSetup CreateObject(NullHandler oReader)
        {
            MgtDBACSetup oMgtDBACSetup = new MgtDBACSetup();
            oMgtDBACSetup = MapObject(oReader);
            return oMgtDBACSetup;
        }

        private List<MgtDBACSetup> CreateObjects(IDataReader oReader)
        {
            List<MgtDBACSetup> oMgtDBACSetup = new List<MgtDBACSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MgtDBACSetup oItem = CreateObject(oHandler);
                oMgtDBACSetup.Add(oItem);
            }
            return oMgtDBACSetup;
        }

        #endregion

        #region Interface implementation
        public MgtDBACSetup Save(MgtDBACSetup oMgtDBACSetup, Int64 nUserID)
        {
            MgtDBACSetup _oMgtDBACSetup = new MgtDBACSetup();
            TransactionContext tc = null;
            oMgtDBACSetup.ErrorMessage = "";
            try
            {
                int nCoount = oMgtDBACSetup.AccountHeadIDs.Split(',').Length;
                for (int i = 0; i < nCoount; i++)
                {
                    MgtDBACSetup oMgtDBACSU = new MgtDBACSetup();
                    oMgtDBACSetup.AccountHeadID = Convert.ToInt32(oMgtDBACSetup.AccountHeadIDs.Split(',')[i]);
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    if (oMgtDBACSetup.MgtDBACSetupID <= 0)
                    {
                        reader = MgtDBACSetupDA.InsertUpdate(tc, oMgtDBACSetup, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = MgtDBACSetupDA.InsertUpdate(tc, oMgtDBACSetup, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oMgtDBACSU = new MgtDBACSetup();
                        oMgtDBACSU = CreateObject(oReader);
                    }
                    reader.Close();
                    tc.End();

                    _oMgtDBACSetup.MgtDBACSetups.Add(oMgtDBACSU);
                }
                    
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                _oMgtDBACSetup.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save MgtDBACSetup. Because of " + e.Message, e);
                #endregion
            }
            return _oMgtDBACSetup;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MgtDBACSetup oMgtDBACSetup = new MgtDBACSetup();
                oMgtDBACSetup.MgtDBACSetupID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.MgtDBACSetup, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "MgtDBACSetup", id);
                MgtDBACSetupDA.Delete(tc, oMgtDBACSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public MgtDBACSetup Get(int id, Int64 nUserId)
        {
            MgtDBACSetup oMgtDBACSetup = new MgtDBACSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = MgtDBACSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMgtDBACSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MgtDBACSetup", e);
                #endregion
            }
            return oMgtDBACSetup;
        }

        public List<MgtDBACSetup> Gets(Int64 nUserID)
        {
            List<MgtDBACSetup> oMgtDBACSetups = new List<MgtDBACSetup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MgtDBACSetupDA.Gets(tc);
                oMgtDBACSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                MgtDBACSetup oMgtDBACSetup = new MgtDBACSetup();
                oMgtDBACSetup.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oMgtDBACSetups;
        }

        public List<MgtDBACSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<MgtDBACSetup> oMgtDBACSetups = new List<MgtDBACSetup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MgtDBACSetupDA.Gets(tc, sSQL);
                oMgtDBACSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MgtDBACSetup", e);
                #endregion
            }
            return oMgtDBACSetups;
        }

        #endregion
    }

}
