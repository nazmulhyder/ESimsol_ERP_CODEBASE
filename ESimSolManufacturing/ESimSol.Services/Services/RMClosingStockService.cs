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
    public class RMClosingStockService : MarshalByRefObject, IRMClosingStockService
    {
        #region Private functions and declaration
        private RMClosingStock MapObject(NullHandler oReader)
        {
            RMClosingStock oRMClosingStock = new RMClosingStock();
            oRMClosingStock.RMClosingStockID = oReader.GetInt32("RMClosingStockID");
            oRMClosingStock.SLNo = oReader.GetString("SLNo");
            oRMClosingStock.BUID = oReader.GetInt32("BUID");
            oRMClosingStock.BUName = oReader.GetString("BUName");
            oRMClosingStock.AccountingSessionID = oReader.GetInt32("AccountingSessionID");
            oRMClosingStock.AccountingSessionName= oReader.GetString("AccountingSessionName");
            oRMClosingStock.RMAccountHeadID = oReader.GetInt32("RMAccountHeadID");
            oRMClosingStock.RMAccountHeadName = oReader.GetString("RMAccountHeadName");
            oRMClosingStock.StockDate = oReader.GetDateTime("StockDate");
            oRMClosingStock.ClosingStockValue = oReader.GetDouble("ClosingStockValue");
            oRMClosingStock.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oRMClosingStock.ApprovedByName = oReader.GetString("ApprovedByName");
            oRMClosingStock.Remarks = oReader.GetString("Remarks");

            return oRMClosingStock;
        }

        private RMClosingStock CreateObject(NullHandler oReader)
        {
            RMClosingStock oRMClosingStock = new RMClosingStock();
            oRMClosingStock = MapObject(oReader);
            return oRMClosingStock;
        }

        private List<RMClosingStock> CreateObjects(IDataReader oReader)
        {
            List<RMClosingStock> oRMClosingStock = new List<RMClosingStock>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RMClosingStock oItem = CreateObject(oHandler);
                oRMClosingStock.Add(oItem);
            }
            return oRMClosingStock;
        }

        #endregion

        #region Interface implementation
        public RMClosingStockService() { }

        public RMClosingStock Save(RMClosingStock oRMClosingStock, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<RMClosingStockDetail> oRMClosingStockDetails = new List<RMClosingStockDetail>();
                RMClosingStockDetail oRMClosingStockDetail = new RMClosingStockDetail();
                oRMClosingStockDetails = oRMClosingStock.RMClosingStockDetails;
                string sRMClosingStockDetailIDs = "";

                IDataReader reader;
                if (oRMClosingStock.RMClosingStockID <= 0)
                {
                    reader = RMClosingStockDA.InsertUpdate(tc, oRMClosingStock, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = RMClosingStockDA.InsertUpdate(tc, oRMClosingStock, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMClosingStock = new RMClosingStock();
                    oRMClosingStock = CreateObject(oReader);
                }
                reader.Close();
                #region RMClosingStock Detail Part
                if (oRMClosingStockDetails != null)
                {
                    foreach (RMClosingStockDetail oItem in oRMClosingStockDetails)
                    {
                        IDataReader readerdetail;
                        oItem.RMClosingStockID = oRMClosingStock.RMClosingStockID;
                        if (oItem.RMClosingStockDetailID <= 0)
                        {
                            readerdetail = RMClosingStockDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");
                        } 
                        else
                        {
                            readerdetail = RMClosingStockDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sRMClosingStockDetailIDs = sRMClosingStockDetailIDs + oReaderDetail.GetString("RMClosingStockDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sRMClosingStockDetailIDs.Length > 0)
                    {
                        sRMClosingStockDetailIDs = sRMClosingStockDetailIDs.Remove(sRMClosingStockDetailIDs.Length - 1, 1);
                    }
                    oRMClosingStockDetail = new RMClosingStockDetail();
                    oRMClosingStockDetail.RMClosingStockID = oRMClosingStock.RMClosingStockID;
                    RMClosingStockDetailDA.Delete(tc, oRMClosingStockDetail, EnumDBOperation.Delete, nUserId, sRMClosingStockDetailIDs);
                }

                #endregion

                #region RMClosingStock Get
                reader = RMClosingStockDA.Get(tc, oRMClosingStock.RMClosingStockID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMClosingStock = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save RMClosingStock. Because of " + e.Message, e);
                #endregion
            }
            return oRMClosingStock;
        }

        public RMClosingStock Approve(RMClosingStock oRMClosingStock, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oRMClosingStock.RMClosingStockID>0)
                {
                    RMClosingStockDA.Approve(tc,oRMClosingStock, nUserId);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Approve RMClosingStock. Because of " + e.Message, e);
                #endregion
            }
            return oRMClosingStock;
        }

        public RMClosingStock UndoApprove(RMClosingStock oRMClosingStock, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oRMClosingStock.RMClosingStockID > 0)
                {
                    RMClosingStockDA.UndoApprove(tc, oRMClosingStock, nUserId);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Approve RMClosingStock. Because of " + e.Message, e);
                #endregion
            }
            return oRMClosingStock;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                RMClosingStock oRMClosingStock = new RMClosingStock();
                oRMClosingStock.RMClosingStockID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.RMClosingStock, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "RMClosingStock", id);
                RMClosingStockDA.Delete(tc, oRMClosingStock, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Deleted";
        }

        public RMClosingStock Get(int id, Int64 nUserId)
        {
            RMClosingStock oAccountHead = new RMClosingStock();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RMClosingStockDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get RMClosingStock", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<RMClosingStock> Gets(Int64 nUserId)
        {
            List<RMClosingStock> oRMClosingStock = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RMClosingStockDA.Gets(tc);
                oRMClosingStock = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RMClosingStock", e);
                #endregion
            }

            return oRMClosingStock;
        }


        public List<RMClosingStock> Gets(string sSQL, Int64 nUserId)
        {
            List<RMClosingStock> oRMClosingStock = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RMClosingStockDA.Gets(tc, sSQL);
                oRMClosingStock = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RMClosingStock", e);
                #endregion
            }

            return oRMClosingStock;
        }

        public List<RMClosingStock> GetsByName(string sName, Int64 nUserId)
        {
            List<RMClosingStock> oRMClosingStocks = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RMClosingStockDA.GetsByName(tc, sName);
                oRMClosingStocks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RMClosingStocks", e);
                #endregion
            }

            return oRMClosingStocks;
        }


        #endregion
    }
}