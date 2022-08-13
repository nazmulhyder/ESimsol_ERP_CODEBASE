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
    public class KnittingFabricReceiveService : MarshalByRefObject, IKnittingFabricReceiveService
    {
        #region Private functions and declaration

        private KnittingFabricReceive MapObject(NullHandler oReader)
        {
            KnittingFabricReceive oKnittingFabricReceive = new KnittingFabricReceive();
            oKnittingFabricReceive.KnittingFabricReceiveID = oReader.GetInt32("KnittingFabricReceiveID");
            oKnittingFabricReceive.KnittingOrderID = oReader.GetInt32("KnittingOrderID");
            oKnittingFabricReceive.ReceiveNo = oReader.GetString("ReceiveNo");
            oKnittingFabricReceive.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oKnittingFabricReceive.PartyChallanNo = oReader.GetString("PartyChallanNo");
            oKnittingFabricReceive.Remarks = oReader.GetString("Remarks");
            oKnittingFabricReceive.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oKnittingFabricReceive.ApprovedByName = oReader.GetString("ApprovedByName");
            oKnittingFabricReceive.BUID = oReader.GetInt32("BUID");
            oKnittingFabricReceive.KnittingOrderNo = oReader.GetString("KnittingOrderNo");
            oKnittingFabricReceive.KnittingOrderDate = oReader.GetDateTime("KnittingOrderDate");
            oKnittingFabricReceive.BuyerName = oReader.GetString("BuyerName");
            oKnittingFabricReceive.StyleNo = oReader.GetString("StyleNo");
            oKnittingFabricReceive.KnittingOrderQty = oReader.GetDouble("KnittingOrderQty");
            oKnittingFabricReceive.BusinessSessionName = oReader.GetString("BusinessSessionName");
            oKnittingFabricReceive.FactoryName = oReader.GetString("FactoryName");
            oKnittingFabricReceive.StartDate = oReader.GetDateTime("StartDate");
            oKnittingFabricReceive.OrderType = (EnumKnittingOrderType)oReader.GetInt32("OrderType");
            return oKnittingFabricReceive;
        }

        private KnittingFabricReceive CreateObject(NullHandler oReader)
        {
            KnittingFabricReceive oKnittingFabricReceive = new KnittingFabricReceive();
            oKnittingFabricReceive = MapObject(oReader);
            return oKnittingFabricReceive;
        }

        private List<KnittingFabricReceive> CreateObjects(IDataReader oReader)
        {
            List<KnittingFabricReceive> oKnittingFabricReceive = new List<KnittingFabricReceive>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnittingFabricReceive oItem = CreateObject(oHandler);
                oKnittingFabricReceive.Add(oItem);
            }
            return oKnittingFabricReceive;
        }

        #endregion

        #region Interface implementation
        public KnittingFabricReceive Save(KnittingFabricReceive oKnittingFabricReceive, Int64 nUserID)
        {
            //TransactionContext tc = null;
            //try
            //{
            //    tc = TransactionContext.Begin(true);
            //    IDataReader reader;
            //    if (oKnittingFabricReceive.KnittingFabricReceiveID <= 0)
            //    {
            //        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingFabricReceive", EnumRoleOperationType.Add);
            //        reader = KnittingFabricReceiveDA.InsertUpdate(tc, oKnittingFabricReceive, EnumDBOperation.Insert, nUserID);
            //    }
            //    else
            //    {
            //        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingFabricReceive", EnumRoleOperationType.Edit);
            //        reader = KnittingFabricReceiveDA.InsertUpdate(tc, oKnittingFabricReceive, EnumDBOperation.Update, nUserID);
            //    }
            //    NullHandler oReader = new NullHandler(reader);
            //    if (reader.Read())
            //    {
            //        oKnittingFabricReceive = new KnittingFabricReceive();
            //        oKnittingFabricReceive = CreateObject(oReader);
            //    }
            //    reader.Close();
            //    tc.End();
            //}
            //catch (Exception e)
            //{
            //    #region Handle Exception
            //    if (tc != null)
            //    {
            //        tc.HandleError();
            //        oKnittingFabricReceive = new KnittingFabricReceive();
            //        oKnittingFabricReceive.ErrorMessage = e.Message.Split('!')[0];
            //    }
            //    #endregion
            //}
            //return oKnittingFabricReceive;

            KnittingFabricReceiveDetail oKnittingFabricReceiveDetail = new KnittingFabricReceiveDetail();

            KnittingFabricReceive oUG = new KnittingFabricReceive();

            oUG = oKnittingFabricReceive;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region KnittingFabricReceive
                IDataReader reader;
                if (oKnittingFabricReceive.KnittingFabricReceiveID <= 0)
                {

                    reader = KnittingFabricReceiveDA.InsertUpdate(tc, oKnittingFabricReceive, EnumDBOperation.Insert, nUserID);
                }
                else
                {

                    reader = KnittingFabricReceiveDA.InsertUpdate(tc, oKnittingFabricReceive, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingFabricReceive = new KnittingFabricReceive();
                    oKnittingFabricReceive = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region KnittingFabricReceiveDetail

                if (oKnittingFabricReceive.KnittingFabricReceiveID > 0)
                {
                    string sKnittingFabricReceiveIDs = "";
                    if (oUG.KnittingFabricReceiveDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (KnittingFabricReceiveDetail oDRD in oUG.KnittingFabricReceiveDetails)
                        {
                            oDRD.KnittingFabricReceiveID = oKnittingFabricReceive.KnittingFabricReceiveID;
                            if (oDRD.KnittingFabricReceiveDetailID <= 0)
                            {
                                readerdetail = KnittingFabricReceiveDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = KnittingFabricReceiveDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nKnittingFabricReceiveDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nKnittingFabricReceiveDetailID = oReaderDevRecapdetail.GetInt32("KnittingFabricReceiveDetailID");
                                sKnittingFabricReceiveIDs = sKnittingFabricReceiveIDs + oReaderDevRecapdetail.GetString("KnittingFabricReceiveDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sKnittingFabricReceiveIDs.Length > 0)
                    {
                        sKnittingFabricReceiveIDs = sKnittingFabricReceiveIDs.Remove(sKnittingFabricReceiveIDs.Length - 1, 1);
                    }
                    oKnittingFabricReceiveDetail = new KnittingFabricReceiveDetail();
                    oKnittingFabricReceiveDetail.KnittingFabricReceiveID = oKnittingFabricReceive.KnittingFabricReceiveID;
                    KnittingFabricReceiveDetailDA.Delete(tc, oKnittingFabricReceiveDetail, EnumDBOperation.Delete, nUserID, sKnittingFabricReceiveIDs);
                }

                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnittingFabricReceive = new KnittingFabricReceive();
                    oKnittingFabricReceive.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingFabricReceive;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnittingFabricReceive oKnittingFabricReceive = new KnittingFabricReceive();
                oKnittingFabricReceive.KnittingFabricReceiveID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "KnittingFabricReceive", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "KnittingFabricReceive", id);
                KnittingFabricReceiveDA.Delete(tc, oKnittingFabricReceive, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public KnittingFabricReceive Get(int id, Int64 nUserId)
        {
            KnittingFabricReceive oKnittingFabricReceive = new KnittingFabricReceive();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnittingFabricReceiveDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingFabricReceive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnittingFabricReceive", e);
                #endregion
            }
            return oKnittingFabricReceive;
        }

        public List<KnittingFabricReceive> Gets(Int64 nUserID)
        {
            List<KnittingFabricReceive> oKnittingFabricReceives = new List<KnittingFabricReceive>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingFabricReceiveDA.Gets(tc);
                oKnittingFabricReceives = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingFabricReceive oKnittingFabricReceive = new KnittingFabricReceive();
                oKnittingFabricReceive.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingFabricReceives;
        }

        public List<KnittingFabricReceive> Gets(string sSQL, Int64 nUserID)
        {
            List<KnittingFabricReceive> oKnittingFabricReceives = new List<KnittingFabricReceive>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingFabricReceiveDA.Gets(tc, sSQL);
                oKnittingFabricReceives = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingFabricReceive", e);
                #endregion
            }
            return oKnittingFabricReceives;
        }

        public KnittingFabricReceive Approve(KnittingFabricReceive oKnittingFabricReceive, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = KnittingFabricReceiveDA.Approve(tc, oKnittingFabricReceive, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingFabricReceive = new KnittingFabricReceive();
                    oKnittingFabricReceive = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnittingFabricReceive = new KnittingFabricReceive();
                    oKnittingFabricReceive.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingFabricReceive;

        }

        #endregion
    }

}
