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
    public class FabricBatchQCDetailService : MarshalByRefObject, IFabricBatchQCDetailService
    {
        #region Private functions and declaration
        public static FabricBatchQCDetail MapObject(NullHandler oReader)
        {
            FabricBatchQCDetail oFabricBatchQCDetail = new FabricBatchQCDetail();
            oFabricBatchQCDetail.Grade = (EnumFBQCGrade)oReader.GetInt32("Grade");
            oFabricBatchQCDetail.GradeInInt = oReader.GetInt32("Grade");
            oFabricBatchQCDetail.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oFabricBatchQCDetail.DeliveryBy = oReader.GetInt32("DeliveryBy");
            oFabricBatchQCDetail.LotNo = oReader.GetString("LotNo");
            oFabricBatchQCDetail.Qty = oReader.GetDouble("Qty");
            oFabricBatchQCDetail.Width = oReader.GetDouble("Width");
            oFabricBatchQCDetail.FBQCDetailID = oReader.GetInt32("FBQCDetailID");
            oFabricBatchQCDetail.FBQCID = oReader.GetInt32("FBQCID");
            oFabricBatchQCDetail.StoreRcvDate = oReader.GetDateTime("StoreRcvDate");
            oFabricBatchQCDetail.ReceiveBy = oReader.GetInt32("ReceiveBy");
            oFabricBatchQCDetail.Remark = oReader.GetString("Remark");
            oFabricBatchQCDetail.ShiftID = oReader.GetInt32("ShiftID");
            //oFabricBatchQCDetail.FBPID = oReader.GetInt32("FBPID");
            oFabricBatchQCDetail.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oFabricBatchQCDetail.FabricQCGradeID = oReader.GetInt32("FabricQCGradeID");
            //oFabricBatchQCDetail.ProductionStartTime = oReader.GetDateTime("ProductionStartTime");
            //oFabricBatchQCDetail.ProductionEndTime = oReader.GetDateTime("ProductionEndTime");
            oFabricBatchQCDetail.ReedCount = oReader.GetDouble("ReedCount");
            oFabricBatchQCDetail.Dent = oReader.GetString("Dent");
            oFabricBatchQCDetail.ProcessType = oReader.GetString("ProcessType");
            oFabricBatchQCDetail.QCGrade = oReader.GetString("QCGrade");
            //oFabricBatchQCDetail.WeftLot = oReader.GetString("WeftLot");
            oFabricBatchQCDetail.TSUID = oReader.GetInt32("TSUID");
            oFabricBatchQCDetail.LotID = oReader.GetInt32("LotID");
            oFabricBatchQCDetail.ProDate = oReader.GetDateTime("ProDate");
            oFabricBatchQCDetail.LoomNo = oReader.GetString("LoomNo");
            oFabricBatchQCDetail.DispoNo = oReader.GetString("DispoNo");
            oFabricBatchQCDetail.BuyerName = oReader.GetString("BuyerName");
            oFabricBatchQCDetail.Construction = oReader.GetString("Construction");
            oFabricBatchQCDetail.ShiftName = oReader.GetString("ShiftName");
            oFabricBatchQCDetail.LotBalance = oReader.GetDouble("LotBalance");
            oFabricBatchQCDetail.QtyTR = oReader.GetDouble("QtyTR");
            oFabricBatchQCDetail.BatchNo = oReader.GetString("BatchNo");
            oFabricBatchQCDetail.FEOSID = oReader.GetInt32("FEOSID");
            oFabricBatchQCDetail.QCGradeType = (EnumFBQCGrade)oReader.GetInt32("QCGradeType");
            oFabricBatchQCDetail.GradeSL = (EnumExcellColumn)oReader.GetInt32("GradeSL");
            oFabricBatchQCDetail.IsYD = oReader.GetBoolean("IsYD");
            oFabricBatchQCDetail.OrderType = (EnumFabricRequestType)oReader.GetInt32("OrderType");
            oFabricBatchQCDetail.FabricFaultType = (EnumFabricFaultType)oReader.GetInt32("FabricFaultType");
            oFabricBatchQCDetail.BUType = (EnumBusinessUnitType)oReader.GetInt32("BUType");
            oFabricBatchQCDetail.FPFID = oReader.GetInt32("FPFID");

            return oFabricBatchQCDetail;
        }
     
        public static FabricBatchQCDetail CreateObject(NullHandler oReader)
        {
            FabricBatchQCDetail oFabricBatchQCDetail = new FabricBatchQCDetail();
            oFabricBatchQCDetail = MapObject(oReader);
            return oFabricBatchQCDetail;
        }

        public static List<FabricBatchQCDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchQCDetail> oFabricBatchQCDetail = new List<FabricBatchQCDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchQCDetail oItem = CreateObject(oHandler);
                oFabricBatchQCDetail.Add(oItem);
            }
            return oFabricBatchQCDetail;
        }

        #endregion

        public List<FabricBatchQCDetail> Gets(int nFBQCID, Int64 nUserID)
        {
            List<FabricBatchQCDetail> oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchQCDetailDA.Gets(tc, nFBQCID);
                oFabricBatchQCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Fabric Yarn Out", e);
                #endregion
            }
            return oFabricBatchQCDetails;
        }
        public List<FabricBatchQCDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBatchQCDetail> oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchQCDetailDA.Gets(tc, sSQL);
                oFabricBatchQCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Gets Fabric Batch QC Detail info", e);
                #endregion
            }
            return oFabricBatchQCDetails;
        }
        
        public FabricBatchQCDetail Save(FabricBatchQCDetail oFabricBatchQCDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            FabricBatchQC oFabricBatchQC = new FabricBatchQC();
            try
            {
              
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (oFabricBatchQCDetail.FabricBatchQC != null && oFabricBatchQCDetail.FabricBatchQC.FBID > 0)
                {
                    if (oFabricBatchQCDetail.FabricBatchQC.FBQCID == 0)
                        reader = FabricBatchQCDA.InsertUpdate(tc, oFabricBatchQCDetail.FabricBatchQC, EnumDBOperation.Insert, nUserID);
                    else
                        reader = FabricBatchQCDA.InsertUpdate(tc, oFabricBatchQCDetail.FabricBatchQC, EnumDBOperation.Update, nUserID);

                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricBatchQC = new FabricBatchQC();
                        oFabricBatchQC = FabricBatchQCService.CreateObject(oReader);
                    }
                    reader.Close();
                    oFabricBatchQCDetail.FBQCID = oFabricBatchQC.FBQCID;
                }
                IDataReader readerDetail;
                if(oFabricBatchQCDetail.FBQCDetailID<=0)
                {
                    readerDetail = FabricBatchQCDetailDA.IUD(tc, EnumDBOperation.Insert, oFabricBatchQCDetail, nUserID);
                }
                else
                {
                    readerDetail = FabricBatchQCDetailDA.IUD(tc, EnumDBOperation.Update, oFabricBatchQCDetail, nUserID);
                }                
                NullHandler oReaderDetail = new NullHandler(readerDetail);
                if (readerDetail.Read())
                {
                    oFabricBatchQCDetail = new FabricBatchQCDetail();
                    oFabricBatchQCDetail = CreateObject(oReaderDetail);
                }
                readerDetail.Close();

                #region Get FBQC
                reader = FabricBatchQCDA.Get(tc, oFabricBatchQCDetail.FBQCID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchQCDetail.FabricBatchQC = FabricBatchQCService.CreateObject(oReader);
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
                oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchQCDetail;
        }
        public FabricBatchQCDetail LockFabricBatchQCDetail(FabricBatchQCDetail oFabricBatchQCDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<FabricBatchQCDetail> oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            try
            {
              
                    int nFBQCID = oFabricBatchQCDetail.FBQCID;
                    tc = TransactionContext.Begin(true);
                    FabricBatchQCDetailDA.LockFabricBatchQCDetail(tc, oFabricBatchQCDetail, nUserID);
                    IDataReader readerDetail;
                    readerDetail = FabricBatchQCDetailDA.Gets(tc, nFBQCID);
                    oFabricBatchQCDetail.FabricBatchQCDetails = CreateObjects(readerDetail);
                    readerDetail.Close();
                    tc.End();
                
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchQCDetail;
        }
        public FabricBatchQCDetail Lock(FabricBatchQCDetail oFabricBatchQCDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader readerDetail;
                FabricBatchQCDetailDA.Lock(tc, oFabricBatchQCDetail, nUserID);
                readerDetail = FabricBatchQCDetailDA.Get(tc,  oFabricBatchQCDetail.FBQCDetailID);
                NullHandler oReaderDetail = new NullHandler(readerDetail);
                if (readerDetail.Read())
                {
                    oFabricBatchQCDetail = new FabricBatchQCDetail();
                    oFabricBatchQCDetail = CreateObject(oReaderDetail);
                }
                readerDetail.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            
            return oFabricBatchQCDetail;
        }
        public FabricBatchQCDetail Get(int nFBQCDID, Int64 nUserID)
        {
            TransactionContext tc = null;
            FabricBatchQCDetail oFabricBatchQCDetail = new FabricBatchQCDetail();
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader readerDetail;
                readerDetail = FabricBatchQCDetailDA.Get(tc, nFBQCDID);
                NullHandler oReaderDetail = new NullHandler(readerDetail);
                if (readerDetail.Read())
                {
                    oFabricBatchQCDetail = new FabricBatchQCDetail();
                    oFabricBatchQCDetail = CreateObject(oReaderDetail);
                }
                readerDetail.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            
            return oFabricBatchQCDetail;
        }

        
        //public FabricBatchQCDetail ReceiveInDelivery(FabricBatchQCDetail oFabricBatchQCDetail, Int64 nUserID)
        //{
        //    TransactionContext tc = null;
        //    FabricBatchQC oFabricBatchQC = new FabricBatchQC();
        //    try
        //    {

        //        tc = TransactionContext.Begin(true);
        //        List<FabricBatchQCDetail> oFBQCDetails = new List<FabricBatchQCDetail>();
        //        oFBQCDetails = oFabricBatchQCDetail.FabricBatchQCDetails;
        //        if (oFBQCDetails.Count > 0)
        //        {
        //            foreach (FabricBatchQCDetail oItem in oFBQCDetails)
        //            {
        //                IDataReader readerDetail;
        //                oItem.ReceiveBy = Convert.ToInt32(nUserID);
        //                oItem.StoreRcvDate = DateTime.Now;
        //                oItem.WorkingUnitID = oFabricBatchQCDetail.WorkingUnitID;
        //                oItem.StoreRcvDate = oFabricBatchQCDetail.StoreRcvDate;
        //                readerDetail = FabricBatchQCDetailDA.ReceiveInDelivery(tc, oItem, nUserID);
        //                readerDetail.Close();
        //            }
        //        }

        //        #region Get FBQC
        //        IDataReader reader = FabricBatchQCDA.Get(tc, oFabricBatchQCDetail.FBQCID);
        //        NullHandler oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oFabricBatchQCDetail.FabricBatchQC = FabricBatchQCService.CreateObject(oReader);
        //        }
        //        reader.Close();
        //        #endregion
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        oFabricBatchQCDetail = new FabricBatchQCDetail();
        //        oFabricBatchQCDetail.ErrorMessage = e.Message.Split('!')[0];
        //        #endregion
        //    }
        //    return oFabricBatchQCDetail;
        //}
        public List<FabricBatchQCDetail> ReceiveInDelivery(FabricBatchQCDetail oFabricBatchQCDetail, Int64 nUserID)
        {
            List<FabricBatchQCDetail> oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                //reader = FabricBatchQCDetailDA.Gets(tc, sSQL);
                reader = FabricBatchQCDetailDA.ReceiveInDelivery(tc, oFabricBatchQCDetail, nUserID);
                oFabricBatchQCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Execute" + e.Message.Split('~')[0], e);
                #endregion
            }
            return oFabricBatchQCDetails;
        }
        
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchQCDetail oFabricBatchQCDetail = new FabricBatchQCDetail();
                oFabricBatchQCDetail.FBQCDetailID = id;
                FabricBatchQCDetailDA.Delete(tc,EnumDBOperation.Delete, oFabricBatchQCDetail,nUserId);
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
    }
    
  
}
