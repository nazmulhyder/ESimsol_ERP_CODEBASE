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
    public class ChangesEquitySetupService : MarshalByRefObject, IChangesEquitySetupService
    {
        #region Private functions and declaration
        private ChangesEquitySetup MapObject(NullHandler oReader)
        {
            ChangesEquitySetup oChangesEquitySetup = new ChangesEquitySetup();
            oChangesEquitySetup.ChangesEquitySetupID = oReader.GetInt32("ChangesEquitySetupID");
            oChangesEquitySetup.EquityCategory = (EnumEquityCategory)oReader.GetInt32("EquityCategory");
            oChangesEquitySetup.EquityCategoryInt = oReader.GetInt32("EquityCategory");
            oChangesEquitySetup.Remarks = oReader.GetString("Remarks");
            return oChangesEquitySetup;
        }

        private ChangesEquitySetup CreateObject(NullHandler oReader)
        {
            ChangesEquitySetup oChangesEquitySetup = new ChangesEquitySetup();
            oChangesEquitySetup = MapObject(oReader);
            return oChangesEquitySetup;
        }

        private List<ChangesEquitySetup> CreateObjects(IDataReader oReader)
        {
            List<ChangesEquitySetup> oChangesEquitySetup = new List<ChangesEquitySetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ChangesEquitySetup oItem = CreateObject(oHandler);
                oChangesEquitySetup.Add(oItem);
            }
            return oChangesEquitySetup;
        }

        #endregion

        #region Interface implementation
        public ChangesEquitySetupService() { }

        public ChangesEquitySetup Save(ChangesEquitySetup oChangesEquitySetup, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<ChangesEquitySetupDetail> oChangesEquitySetupDetails = new List<ChangesEquitySetupDetail>();
                oChangesEquitySetupDetails = oChangesEquitySetup.ChangesEquitySetupDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;

                if (oChangesEquitySetup.ChangesEquitySetupID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ChangesEquitySetup, EnumRoleOperationType.Add);
                    reader = ChangesEquitySetupDA.InsertUpdate(tc, oChangesEquitySetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ChangesEquitySetup, EnumRoleOperationType.Edit);
                    reader = ChangesEquitySetupDA.InsertUpdate(tc, oChangesEquitySetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChangesEquitySetup = new ChangesEquitySetup();
                    oChangesEquitySetup = CreateObject(oReader);
                }
                reader.Close();

                #region Details
                

                if (oChangesEquitySetupDetails != null)
                {
                    string sChangesEquitySetupDetailIDIDs = "";
                    foreach (ChangesEquitySetupDetail oItem in oChangesEquitySetupDetails)
                    {
                        IDataReader readertnc;
                        
                        if (oItem.ChangesEquitySetupDetailID <= 0)
                        {
                            oItem.ChangesEquitySetupID = oChangesEquitySetup.ChangesEquitySetupID;
                            readertnc = ChangesEquitySetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            oItem.ChangesEquitySetupID = oChangesEquitySetup.ChangesEquitySetupID;
                            readertnc = ChangesEquitySetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);
                        if (readertnc.Read())
                        {
                            sChangesEquitySetupDetailIDIDs = sChangesEquitySetupDetailIDIDs + oReaderTNC.GetString("ChangesEquitySetupDetailID") + ",";
                        }
                        readertnc.Close();
                    }

                    if (sChangesEquitySetupDetailIDIDs.Length > 0)
                    {
                        sChangesEquitySetupDetailIDIDs = sChangesEquitySetupDetailIDIDs.Remove(sChangesEquitySetupDetailIDIDs.Length - 1, 1);
                    }
                    ChangesEquitySetupDetail oTempChangesEquitySetupDetail = new ChangesEquitySetupDetail();
                    oTempChangesEquitySetupDetail.ChangesEquitySetupID = oChangesEquitySetup.ChangesEquitySetupID;
                    ChangesEquitySetupDetailDA.Delete(tc, oTempChangesEquitySetupDetail, EnumDBOperation.Delete, nUserID, sChangesEquitySetupDetailIDIDs);
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save ChangesEquitySetup. Because of " + e.Message, e);
                #endregion
            }
            return oChangesEquitySetup;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ChangesEquitySetup oChangesEquitySetup = new ChangesEquitySetup();
                oChangesEquitySetup.ChangesEquitySetupID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ChangesEquitySetup, EnumRoleOperationType.Delete);
                ChangesEquitySetupDA.Delete(tc, oChangesEquitySetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ChangesEquitySetup. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ChangesEquitySetup Get(int id, int nUserId)
        {
            ChangesEquitySetup oAccountHead = new ChangesEquitySetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChangesEquitySetupDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ChangesEquitySetup", e);
                #endregion
            }

            return oAccountHead;
        }

       

        public List<ChangesEquitySetup> Gets(int nUserID)
        {
            List<ChangesEquitySetup> oChangesEquitySetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChangesEquitySetupDA.Gets(tc);
                oChangesEquitySetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChangesEquitySetup", e);
                #endregion
            }

            return oChangesEquitySetup;
        }

       
        public List<ChangesEquitySetup> Gets(string sSQL,int nUserID)
        {
            List<ChangesEquitySetup> oChangesEquitySetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                
                reader = ChangesEquitySetupDA.Gets(tc, sSQL);
                oChangesEquitySetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChangesEquitySetup", e);
                #endregion
            }

            return oChangesEquitySetup;
        }

       
        #endregion
    }   
}