using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{

    public class CIStatementSetupService : MarshalByRefObject, ICIStatementSetupService
    {
        #region Private functions and declaration
        private CIStatementSetup MapObject(NullHandler oReader)
        {
            CIStatementSetup oCIStatementSetup = new CIStatementSetup();
            oCIStatementSetup.CIStatementSetupID = oReader.GetInt32("CIStatementSetupID");
            oCIStatementSetup.CIHeadType = (EnumCISSetup)oReader.GetInt32("CIHeadType");
            oCIStatementSetup.CIHeadTypeInt = oReader.GetInt32("CIHeadType");
            oCIStatementSetup.AccountHeadName = oReader.GetString("AccountHeadName");
            oCIStatementSetup.AccountCode = oReader.GetString("AccountCode");
            oCIStatementSetup.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oCIStatementSetup.DisplayCaption = oReader.GetString("DisplayCaption");
            oCIStatementSetup.ComponentType = (EnumComponentType)oReader.GetInt32("ComponentType");
            return oCIStatementSetup;
        }

        private CIStatementSetup CreateObject(NullHandler oReader)
        {
            CIStatementSetup oCIStatementSetup = new CIStatementSetup();
            oCIStatementSetup = MapObject(oReader);
            return oCIStatementSetup;
        }

        private List<CIStatementSetup> CreateObjects(IDataReader oReader)
        {
            List<CIStatementSetup> oCIStatementSetup = new List<CIStatementSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CIStatementSetup oItem = CreateObject(oHandler);
                oCIStatementSetup.Add(oItem);
            }
            return oCIStatementSetup;
        }

        #endregion

        #region Interface implementation
        public CIStatementSetupService() { }

        public List<CIStatementSetup> Save(CIStatementSetup oCIStatementSetup, Int64 nUserID)
        {
            List<CIStatementSetup> oCIStatementSetups = new List<CIStatementSetup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (CIStatementSetup oItem in oCIStatementSetup.CIStatementSetups)
                {
                    reader = CIStatementSetupDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oCIStatementSetup = new CIStatementSetup();
                        oCIStatementSetup = CreateObject(oReader);
                    }
                    reader.Close();
                }
                reader = null;
                reader = CIStatementSetupDA.Gets(tc);
                oCIStatementSetups = CreateObjects(reader);
                reader.Close();
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save CIStatementSetup. Because of " + e.Message, e);
                #endregion
            }
            return oCIStatementSetups;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CIStatementSetup oCIStatementSetup = new CIStatementSetup();
                oCIStatementSetup.CIStatementSetupID = id;
                CIStatementSetupDA.Delete(tc, oCIStatementSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public CIStatementSetup Get(int id, Int64 nUserId)
        {
            CIStatementSetup oCIStatementSetup = new CIStatementSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = CIStatementSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCIStatementSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CIStatementSetup", e);
                #endregion
            }
            return oCIStatementSetup;
        }

  

        public List<CIStatementSetup> Gets(Int64 nUserID)
        {
            List<CIStatementSetup> oCIStatementSetups = new List<CIStatementSetup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CIStatementSetupDA.Gets(tc);
                oCIStatementSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CIStatementSetup", e);
                #endregion
            }
            return oCIStatementSetups;
        }
        public List<CIStatementSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<CIStatementSetup> oCIStatementSetups = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CIStatementSetupDA.Gets(tc, sSQL);
                oCIStatementSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CIStatementSetup", e);
                #endregion
            }
            return oCIStatementSetups;
        }

        #endregion
    }   
    
  
}
