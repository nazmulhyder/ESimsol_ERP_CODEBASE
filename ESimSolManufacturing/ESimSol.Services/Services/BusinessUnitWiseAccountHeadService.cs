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

    public class BusinessUnitWiseAccountHeadService : MarshalByRefObject, IBusinessUnitWiseAccountHeadService
    {
        #region Private functions and declaration
        private BusinessUnitWiseAccountHead MapObject(NullHandler oReader)
        {
            BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead = new BusinessUnitWiseAccountHead();
            oBusinessUnitWiseAccountHead.BusinessUnitWiseAccountHeadID = oReader.GetInt32("BusinessUnitWiseAccountHeadID");
            oBusinessUnitWiseAccountHead.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oBusinessUnitWiseAccountHead.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            return oBusinessUnitWiseAccountHead;
        }

        private BusinessUnitWiseAccountHead CreateObject(NullHandler oReader)
        {
            BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead = new BusinessUnitWiseAccountHead();
            oBusinessUnitWiseAccountHead = MapObject(oReader);
            return oBusinessUnitWiseAccountHead;
        }

        private List<BusinessUnitWiseAccountHead> CreateObjects(IDataReader oReader)
        {
            List<BusinessUnitWiseAccountHead> oBusinessUnitWiseAccountHead = new List<BusinessUnitWiseAccountHead>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BusinessUnitWiseAccountHead oItem = CreateObject(oHandler);
                oBusinessUnitWiseAccountHead.Add(oItem);
            }
            return oBusinessUnitWiseAccountHead;
        }

        #endregion

        #region Interface implementation
        public BusinessUnitWiseAccountHeadService() { }

        public BusinessUnitWiseAccountHead Save(BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Business Location
                List<BusinessUnitWiseAccountHead> oBusinessUnitWiseAccountHeads = new List<BusinessUnitWiseAccountHead>();
                oBusinessUnitWiseAccountHeads = oBusinessUnitWiseAccountHead.BusinessUnitWiseAccountHeads;


                if (oBusinessUnitWiseAccountHeads != null)
                {
                    string sBusinessUnitWiseAccountHeadIDs = "";
                    foreach (BusinessUnitWiseAccountHead oItem in oBusinessUnitWiseAccountHeads)
                    {
                        IDataReader readertnc;

                        if (oItem.BusinessUnitWiseAccountHeadID <= 0)
                        {
                            readertnc = BusinessUnitWiseAccountHeadDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");
                        }
                        else
                        {
                            readertnc = BusinessUnitWiseAccountHeadDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);
                        if (readertnc.Read())
                        {
                            sBusinessUnitWiseAccountHeadIDs = sBusinessUnitWiseAccountHeadIDs + oReaderTNC.GetString("BusinessUnitWiseAccountHeadID") + ",";
                        }
                        readertnc.Close();
                    }

                    if (sBusinessUnitWiseAccountHeadIDs.Length > 0)
                    {
                        sBusinessUnitWiseAccountHeadIDs = sBusinessUnitWiseAccountHeadIDs.Remove(sBusinessUnitWiseAccountHeadIDs.Length - 1, 1);
                    }
                    BusinessUnitWiseAccountHead otempBusinessUnitWiseAccountHead = new BusinessUnitWiseAccountHead();
                    otempBusinessUnitWiseAccountHead.BusinessUnitID = oBusinessUnitWiseAccountHead.BusinessUnitID;
                    BusinessUnitWiseAccountHeadDA.Delete(tc, otempBusinessUnitWiseAccountHead, EnumDBOperation.Delete, nUserId, sBusinessUnitWiseAccountHeadIDs);
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBusinessUnitWiseAccountHead = new BusinessUnitWiseAccountHead();
                oBusinessUnitWiseAccountHead.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oBusinessUnitWiseAccountHead;
        }

        public BusinessUnitWiseAccountHead SaveFromCOA(BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead, int nUserId)
        {
            List<BusinessUnitWiseAccountHead> oBusinessUnitWiseAccountHeads = new List<BusinessUnitWiseAccountHead>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Business Location
                
                oBusinessUnitWiseAccountHeads = oBusinessUnitWiseAccountHead.BusinessUnitWiseAccountHeads;


                if (oBusinessUnitWiseAccountHeads != null)
                {
                    string sBusinessUnitIDs = "";
                    foreach (BusinessUnitWiseAccountHead oItem in oBusinessUnitWiseAccountHeads)
                    {

                        sBusinessUnitIDs = sBusinessUnitIDs + oItem.BusinessUnitID + ",";
                    }
                    if (sBusinessUnitIDs.Length > 0)
                    {
                        sBusinessUnitIDs = sBusinessUnitIDs.Remove(sBusinessUnitIDs.Length - 1, 1);
                    }
                    IDataReader readertnc;
                    readertnc = BusinessUnitWiseAccountHeadDA.IUDFromCOA(tc, oBusinessUnitWiseAccountHead, sBusinessUnitIDs, nUserId);
                    oBusinessUnitWiseAccountHeads = CreateObjects(readertnc);
                    oBusinessUnitWiseAccountHead.BusinessUnitWiseAccountHeads = oBusinessUnitWiseAccountHeads;
                    readertnc.Close();
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBusinessUnitWiseAccountHead = new BusinessUnitWiseAccountHead();
                oBusinessUnitWiseAccountHead.ErrorMessage = e.Message.Split('!')[0];
                
                #endregion
            }
            return oBusinessUnitWiseAccountHead;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            string sMessage = "Delete sucessfully";
            try
            {
                tc = TransactionContext.Begin(true);
                BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead = new BusinessUnitWiseAccountHead();
                oBusinessUnitWiseAccountHead.BusinessUnitWiseAccountHeadID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ChartsOfAccount, EnumRoleOperationType.Delete);
                BusinessUnitWiseAccountHeadDA.Delete(tc, oBusinessUnitWiseAccountHead, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                sMessage = e.Message.Split('!')[0];
            }

            return Global.DeleteMessage;
        }
        public string CopyBasicChartOfAccount(int nCompanyID, int nUserId)
        {
            TransactionContext tc = null;
            string sMessage = "Execution Successfully";
            try
            {
                tc = TransactionContext.Begin(true);
                BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead = new BusinessUnitWiseAccountHead();
                BusinessUnitWiseAccountHeadDA.CopyBasicChartOfAccount(tc, nCompanyID, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                sMessage = e.Message.Split('!')[0];
            }

            return sMessage;
        }
        public BusinessUnitWiseAccountHead Get(int id, int nUserId)
        {
            BusinessUnitWiseAccountHead oAccountHead = new BusinessUnitWiseAccountHead();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BusinessUnitWiseAccountHeadDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BusinessUnitWiseAccountHead", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BusinessUnitWiseAccountHead> Gets(int nUserId)
        {
            List<BusinessUnitWiseAccountHead> oBusinessUnitWiseAccountHead = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BusinessUnitWiseAccountHeadDA.Gets(tc);
                oBusinessUnitWiseAccountHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessUnitWiseAccountHead", e);
                #endregion
            }

            return oBusinessUnitWiseAccountHead;
        }
        public List<BusinessUnitWiseAccountHead> Gets(int nBUID, int nUserID)
        {
            List<BusinessUnitWiseAccountHead> oBusinessUnitWiseAccountHead = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BusinessUnitWiseAccountHeadDA.Gets(tc,nBUID);
                oBusinessUnitWiseAccountHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessUnitWiseAccountHead", e);
                #endregion
            }

            return oBusinessUnitWiseAccountHead;
        }
        public List<BusinessUnitWiseAccountHead> GetsByCOA(int nAHID, int nUserID)
        {
            List<BusinessUnitWiseAccountHead> oBusinessUnitWiseAccountHead = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BusinessUnitWiseAccountHeadDA.GetsByCOA(tc, nAHID);
                oBusinessUnitWiseAccountHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessUnitWiseAccountHead", e);
                #endregion
            }

            return oBusinessUnitWiseAccountHead;
        }
        #endregion
    } 
}
