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
    [Serializable]
    public class ExpenditureHeadService : MarshalByRefObject, IExpenditureHeadService
    {
        #region Private functions and declaration
        private ExpenditureHead MapObject(NullHandler oReader)
        {
            ExpenditureHead oExpenditureHead = new ExpenditureHead();
            oExpenditureHead.ExpenditureHeadID = oReader.GetInt32("ExpenditureHeadID");
            oExpenditureHead.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oExpenditureHead.Name = oReader.GetString("Name");
            oExpenditureHead.Activity = oReader.GetBoolean("Activity");
            oExpenditureHead.AccountCode = oReader.GetString("AccountCode");
            oExpenditureHead.AccountHeadName = oReader.GetString("AccountHeadName");
            oExpenditureHead.ExpenditureHeadType = (EnumExpenditureHeadType) oReader.GetInt32("ExpenditureHeadType");
            oExpenditureHead.ExpenditureHeadTypeInt = oReader.GetInt32("ExpenditureHeadType");
            
            return oExpenditureHead;
        }

        private ExpenditureHead CreateObject(NullHandler oReader)
        {
            ExpenditureHead oExpenditureHead = new ExpenditureHead();
            oExpenditureHead = MapObject(oReader);
            return oExpenditureHead;
        }

        private List<ExpenditureHead> CreateObjects(IDataReader oReader)
        {
            List<ExpenditureHead> oExpenditureHeads = new List<ExpenditureHead>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExpenditureHead oItem = CreateObject(oHandler);
                oExpenditureHeads.Add(oItem);
            }
            return oExpenditureHeads;
        }
        #endregion

        #region Interface implementation
        public ExpenditureHeadService() { }

        #region Save Import Invoice & Import Invoice Product
        public ExpenditureHead Save(ExpenditureHead oExpenditureHead, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ExpenditureHeadMapping> oExpenditureHeadMappings = new List<ExpenditureHeadMapping>();
                oExpenditureHeadMappings = oExpenditureHead.ExpenditureHeadMappings;
                string sExpenditureHeadDetailIDS = "";

                IDataReader reader;
                if (oExpenditureHead.ExpenditureHeadID <= 0)
                {
                    reader = ExpenditureHeadDA.InsertUpdate(tc, oExpenditureHead, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExpenditureHeadDA.InsertUpdate(tc, oExpenditureHead, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExpenditureHead = CreateObject(oReader);
                }
                reader.Close();

                #region ExpenditureHead Part

                foreach (ExpenditureHeadMapping oItem in oExpenditureHeadMappings)
                {
                    IDataReader readerdetail;
                    oItem.ExpenditureHeadID = oExpenditureHead.ExpenditureHeadID;
                    if (oItem.ExpenditureHeadMappingID <= 0)
                    {
                        readerdetail = ExpenditureHeadMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ExpenditureHeadMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sExpenditureHeadDetailIDS = sExpenditureHeadDetailIDS + oReaderDetail.GetString("ExpenditureHeadMappingID") + ",";
                    }
                    readerdetail.Close();
                }
                ExpenditureHeadMapping oExpenditureHeadMapping = new ExpenditureHeadMapping();
                oExpenditureHeadMapping.ExpenditureHeadID = oExpenditureHead.ExpenditureHeadID;
                if (sExpenditureHeadDetailIDS.Length > 0)
                {
                    sExpenditureHeadDetailIDS = sExpenditureHeadDetailIDS.Remove(sExpenditureHeadDetailIDS.Length - 1, 1);
                }
                ExpenditureHeadMappingDA.Delete(tc, oExpenditureHeadMapping, EnumDBOperation.Delete, nUserID, sExpenditureHeadDetailIDS);
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oExpenditureHead = new ExpenditureHead();
                if (tc != null)
                    tc.HandleError();

                oExpenditureHead.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExpenditureHead;

        }
    
        public List<ExpenditureHead> Gets(string sSQL, Int64 nUserID)
        {
            List<ExpenditureHead> oExpenditureHead = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExpenditureHeadDA.Gets(tc, sSQL);
                oExpenditureHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExpenditureHead", e);
                #endregion
            }

            return oExpenditureHead;
        }

        #endregion

        #region Delete
        public String Delete(ExpenditureHead oExpenditureHead, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExpenditureHeadDA.Delete(tc, oExpenditureHead, EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                #endregion
            }
            return Global.DeleteMessage;
        }

        #endregion

        #region Retrive Information

        public ExpenditureHead Get(int nExpenditureHeadID, Int64 nUserID)
        {
            ExpenditureHead oExpenditureHead = new ExpenditureHead();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExpenditureHeadDA.Get(nExpenditureHeadID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExpenditureHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExpenditureHead", e);
                #endregion
            }

            return oExpenditureHead;
        }


        public List<ExpenditureHead> Gets(Int64 nUserID)
        {
            List<ExpenditureHead> oExpenditureHeads = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExpenditureHeadDA.Gets(tc);
                oExpenditureHeads = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExpenditureHeads", e);
                #endregion
            }

            return oExpenditureHeads;
        }
        public List<ExpenditureHead> Gets(int nOperationType,Int64 nUserID)
        {
            List<ExpenditureHead> oExpenditureHeads = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExpenditureHeadDA.Gets(tc, nOperationType);
                oExpenditureHeads = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExpenditureHeads", e);
                #endregion
            }

            return oExpenditureHeads;
        }

        #endregion



        #endregion


    }

}
