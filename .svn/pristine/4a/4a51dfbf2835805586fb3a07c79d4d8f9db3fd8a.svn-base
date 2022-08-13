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
    public class FabricTransferPackingListDetailService : MarshalByRefObject, IFabricTransferPackingListDetailService
    {
        #region Private functions and declaration
        private FabricTransferPackingListDetail MapObject(NullHandler oReader)
        {
            FabricTransferPackingListDetail oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
            oFabricTransferPackingListDetail.FTPLDetailID = oReader.GetInt32("FTPLDetailID");
            oFabricTransferPackingListDetail.FTPListID = oReader.GetInt32("FTPListID");
            oFabricTransferPackingListDetail.LotID = oReader.GetInt32("LotID");
            oFabricTransferPackingListDetail.Qty = oReader.GetDouble("Qty");

            oFabricTransferPackingListDetail.LotNo = oReader.GetString("LotNo");
            oFabricTransferPackingListDetail.FEOID = oReader.GetInt32("FEOID");
            oFabricTransferPackingListDetail.FNExOID = oReader.GetInt32("FNExOID");
            oFabricTransferPackingListDetail.FabricID = oReader.GetInt32("FabricID");
            oFabricTransferPackingListDetail.ReceiveBy = oReader.GetInt32("ReceiveBy");
            oFabricTransferPackingListDetail.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oFabricTransferPackingListDetail.ReceiveByName = oReader.GetString("ReceiveByName");
            oFabricTransferPackingListDetail.WUID = oReader.GetInt32("WUID");
            oFabricTransferPackingListDetail.Grade = (EnumFBQCGrade)oReader.GetInt16("Grade");
            oFabricTransferPackingListDetail.StoreRcvDate = oReader.GetDateTime("StoreRcvDate");
            oFabricTransferPackingListDetail.WarpLot = oReader.GetString("WarpLot");
            oFabricTransferPackingListDetail.WeftLot = oReader.GetString("WeftLot");
            return oFabricTransferPackingListDetail;
        }
        private FabricTransferPackingListDetail CreateObject(NullHandler oReader)
        {
            FabricTransferPackingListDetail oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
            oFabricTransferPackingListDetail = MapObject(oReader);
            return oFabricTransferPackingListDetail;
        }
        private List<FabricTransferPackingListDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricTransferPackingListDetail> oFabricTransferPackingListDetail = new List<FabricTransferPackingListDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricTransferPackingListDetail oItem = CreateObject(oHandler);
                oFabricTransferPackingListDetail.Add(oItem);
            }
            return oFabricTransferPackingListDetail;
        }

        #endregion

        #region Interface implementation 
        public FabricTransferPackingListDetail Save(FabricTransferPackingListDetail oFabricTransferPackingListDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricTransferPackingList oFTPL = new FabricTransferPackingList();
                int Count = 0,
                    nFTPListID = 0;
                List<FabricTransferPackingListDetail> oFTPLDetails = new List<FabricTransferPackingListDetail>();
                if (oFabricTransferPackingListDetail.FTPLDetails.Count > 0)
                {
                    foreach (FabricTransferPackingListDetail oItem in oFabricTransferPackingListDetail.FTPLDetails)
                    {
                        Count++;
                        #region Fabric Transfer Packing List
                        if (oItem.FTPL != null)
                        {
                            IDataReader reader;
                            if (oItem.FTPL.FTPListID <= 0)
                            {
                                reader = FabricTransferPackingListDA.InsertUpdate(tc, oItem.FTPL, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                reader = FabricTransferPackingListDA.InsertUpdate(tc, oItem.FTPL, EnumDBOperation.Update, nUserID);
                            }
                            NullHandler oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                oFTPL = new FabricTransferPackingList();
                                oFTPL = FabricTransferPackingListService.CreateObject(oReader);
                                nFTPListID = oFTPL.FTPListID;
                            }
                            if (oFTPL.FTPListID <= 0)
                            {
                                oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                                oFabricTransferPackingListDetail.ErrorMessage = "Invalid Fabric Transfer Packing List";
                                return oFabricTransferPackingListDetail;
                            }
                            reader.Close();
                        }
                        #endregion

                        if (oItem.FTPListID == 0)
                        {
                            oItem.FTPListID = nFTPListID;
                        }

                        IDataReader readerdetail;
                        
                        if (oItem.FTPLDetailID <= 0)
                        {
                            readerdetail = FabricTransferPackingListDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = FabricTransferPackingListDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                            oFabricTransferPackingListDetail = CreateObject(oReaderDetail);
                            if (Count == 1)
                            {
                                oFabricTransferPackingListDetail.FTPL = oFTPL;
                            }
                        }
                        oFTPLDetails.Add(oFabricTransferPackingListDetail);
                        readerdetail.Close();
                    }

                    oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                    oFabricTransferPackingListDetail.FTPLDetails = oFTPLDetails;
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                oFabricTransferPackingListDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricTransferPackingListDetail;
        }

        public FabricTransferPackingListDetail Update(FabricTransferPackingListDetail oFabricTransferPackingListDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
             
                List<FabricTransferPackingListDetail> oFTPLDetails = new List<FabricTransferPackingListDetail>();
                IDataReader reader;
                NullHandler oReader;
                if (oFabricTransferPackingListDetail.FTPLDetailID > 0)
                {
                    reader = FabricTransferPackingListDetailDA.InsertUpdate(tc, oFabricTransferPackingListDetail, EnumDBOperation.Update, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                        oFabricTransferPackingListDetail = CreateObject(oReader);
                    }
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                oFabricTransferPackingListDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricTransferPackingListDetail;
        }
        public string Delete(int id, int nFTPListID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricTransferPackingListDetail oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                oFabricTransferPackingListDetail.FTPLDetailID = id;
                oFabricTransferPackingListDetail.FTPListID = nFTPListID;
                DBTableReferenceDA.HasReference(tc, "FabricTransferPackingListDetail", id);
                FabricTransferPackingListDetailDA.Delete(tc, oFabricTransferPackingListDetail, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public List<FabricTransferPackingListDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricTransferPackingListDetail> oFabricTransferPackingListDetails = new List<FabricTransferPackingListDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricTransferPackingListDetailDA.Gets(tc, sSQL);
                oFabricTransferPackingListDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferPackingListDetails = new List<FabricTransferPackingListDetail>();
                FabricTransferPackingListDetail oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                oFabricTransferPackingListDetail.ErrorMessage = e.Message.Split('!')[0];
                oFabricTransferPackingListDetails.Add(oFabricTransferPackingListDetail);
                #endregion
            }
            return oFabricTransferPackingListDetails;
        }
        public List<FabricTransferPackingListDetail> Gets(int nFTPListID, Int64 nUserID)
        {
            List<FabricTransferPackingListDetail> oFabricTransferPackingListDetails = new List<FabricTransferPackingListDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricTransferPackingListDetailDA.Gets(tc, nFTPListID);
                oFabricTransferPackingListDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferPackingListDetails = new List<FabricTransferPackingListDetail>();
                FabricTransferPackingListDetail oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                oFabricTransferPackingListDetail.ErrorMessage = e.Message.Split('!')[0];
                oFabricTransferPackingListDetails.Add(oFabricTransferPackingListDetail);
                #endregion
            }
            return oFabricTransferPackingListDetails;
        }
        public List<FabricTransferPackingListDetail> Gets(Int64 nUserID)
        {
            List<FabricTransferPackingListDetail> oFabricTransferPackingListDetails = new List<FabricTransferPackingListDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricTransferPackingListDetailDA.Gets(tc);
                oFabricTransferPackingListDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferPackingListDetails = new List<FabricTransferPackingListDetail>();
                FabricTransferPackingListDetail oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                oFabricTransferPackingListDetail.ErrorMessage = e.Message.Split('!')[0];
                oFabricTransferPackingListDetails.Add(oFabricTransferPackingListDetail);
                #endregion
            }
            return oFabricTransferPackingListDetails;
        }
        public FabricTransferPackingListDetail Get(int id, Int64 nUserId)
        {
            FabricTransferPackingListDetail oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricTransferPackingListDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricTransferPackingListDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferPackingListDetail = new FabricTransferPackingListDetail();
                oFabricTransferPackingListDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricTransferPackingListDetail;
        }

        #endregion
    }
}
