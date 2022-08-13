using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class FNBatchQCDetailService : MarshalByRefObject, IFNBatchQCDetailService
    {
        #region Private functions and declaration
        private FNBatchQCDetail MapObject(NullHandler oReader)
        {
            FNBatchQCDetail oFNBatchQCDetail = new FNBatchQCDetail();
            oFNBatchQCDetail.FNBatchQCDetailID = oReader.GetInt32("FNBatchQCDetailID");
            oFNBatchQCDetail.FNBatchQCID = oReader.GetInt32("FNBatchQCID");
            oFNBatchQCDetail.Grade = (EnumFBQCGrade)oReader.GetInt16("Grade");
            oFNBatchQCDetail.LotNo = oReader.GetString("LotNo");
            oFNBatchQCDetail.Qty = oReader.GetDouble("Qty");
            oFNBatchQCDetail.GSM = oReader.GetDouble("GSM");
            oFNBatchQCDetail.Bowl_Skew = oReader.GetDouble("Bowl_Skew");
            oFNBatchQCDetail.PointsYard = oReader.GetDouble("PointsYard");
            oFNBatchQCDetail.IsLock = oReader.GetBoolean("IsLock");
            oFNBatchQCDetail.LockDate = oReader.GetDateTime("LockDate");
            oFNBatchQCDetail.StoreRcvDate = oReader.GetDateTime("StoreRcvDate");
            oFNBatchQCDetail.RcvByID = oReader.GetInt32("RcvByID");
            oFNBatchQCDetail.DBServerDate = oReader.GetDateTime("DBServerDate");
            oFNBatchQCDetail.ShadeID = (EnumFNShade)oReader.GetInt16("ShadeID");
            oFNBatchQCDetail.IsPassed =oReader.GetInt16("IsPassed");
            oFNBatchQCDetail.QtyDC = oReader.GetDouble("QtyDC");
            oFNBatchQCDetail.QtyExe = oReader.GetDouble("QtyExe");
            oFNBatchQCDetail.LotID = oReader.GetInt32("LotID");
            oFNBatchQCDetail.FSCDID = oReader.GetInt32("FSCDID");
            oFNBatchQCDetail.QtyRC = oReader.GetDouble("QtyRC");
            oFNBatchQCDetail.ProDate = oReader.GetDateTime("ProDate");
            oFNBatchQCDetail.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oFNBatchQCDetail.DeliveryBy = oReader.GetInt32("DeliveryBy");
            return oFNBatchQCDetail;
        }

        private FNBatchQCDetail CreateObject(NullHandler oReader)
        {
            FNBatchQCDetail oFNBatchQCDetail = MapObject(oReader);
            return oFNBatchQCDetail;
        }

        private List<FNBatchQCDetail> CreateObjects(IDataReader oReader)
        {
            List<FNBatchQCDetail> oFNBatchQCDetail = new List<FNBatchQCDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNBatchQCDetail oItem = CreateObject(oHandler);
                oFNBatchQCDetail.Add(oItem);
            }
            return oFNBatchQCDetail;
        }

        #endregion

        #region Interface implementation
        public FNBatchQCDetailService() { }

        public FNBatchQCDetail IUD(FNBatchQCDetail oFNBatchQCDetail, int nDBOperation, Int64 nUserID)
        {
            FNBatchQC oFNBatchQC = new FNBatchQC();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    if (oFNBatchQCDetail.FNBatchQCID <= 0)
                    {
                        reader = FNBatchQCDA.IUD(tc, oFNBatchQCDetail.FNBatchQC, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFNBatchQC = new FNBatchQC();
                            oFNBatchQC = FNBatchQCService.CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    oFNBatchQCDetail.FNBatchQCID = (oFNBatchQCDetail.FNBatchQCID > 0) ? oFNBatchQCDetail.FNBatchQCID : oFNBatchQC.FNBatchQCID;
                    reader = FNBatchQCDetailDA.IUD(tc, oFNBatchQCDetail, nDBOperation, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNBatchQCDetail = new FNBatchQCDetail();
                        oFNBatchQCDetail = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FNBatchQCDetailDA.IUD(tc, oFNBatchQCDetail, nDBOperation, nUserID);
                    oReader = new NullHandler(reader);
                    reader.Close();
                    oFNBatchQCDetail.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            oFNBatchQCDetail.FNBatchQC = oFNBatchQC;
            return oFNBatchQCDetail;
        }

        public FNBatchQCDetail Get(int nFNBatchQCDetailID, Int64 nUserId)
        {
            FNBatchQCDetail oFNBatchQCDetail = new FNBatchQCDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNBatchQCDetailDA.Get(tc, nFNBatchQCDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatchQCDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFNBatchQCDetail;
        }

        public List<FNBatchQCDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            FNBatchQCDetail oFNBatchQCDetail = new FNBatchQCDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchQCDetailDA.Gets(tc, sSQL);
                oFNBatchQCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQCDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oFNBatchQCDetails.Add(oFNBatchQCDetail);
                #endregion
            }

            return oFNBatchQCDetails;
        }

        public FNBatchQCDetail LockFNBatchQCDetail(FNBatchQCDetail oFNBQCDetail, Int64 nUserID)
        {
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();

            List<int> FNBatchQCIDs = new List<int>(oFNBQCDetail.FNBatchQCDetailIDs.Split(',').Select(int.Parse).ToList());
            FNBatchQCDetail oFNBatchQCDetail = new FNBatchQCDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                NullHandler oReader;
                IDataReader reader;
                foreach (int Id in FNBatchQCIDs)
                {
                    oFNBatchQCDetail = new FNBatchQCDetail();
                    oFNBatchQCDetail.FNBatchQCDetailID = Id;
                    oFNBatchQCDetail.ProDate = oFNBQCDetail.ProDate;
                    oFNBatchQCDetail.DeliveryDate = oFNBQCDetail.DeliveryDate;
                    reader = FNBatchQCDetailDA.IUD(tc, oFNBatchQCDetail, (int)EnumDBOperation.Approval, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNBatchQCDetail = new FNBatchQCDetail();
                        oFNBatchQCDetail = CreateObject(oReader);
                    }
                    reader.Close();
                }

                if (oFNBatchQCDetail.FNBatchQCID > 0)
                {
                    string sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCID=" + oFNBatchQCDetail.FNBatchQCID + "";
                    reader = FNBatchQCDetailDA.Gets(tc, sSQL);
                    oFNBatchQCDetails = CreateObjects(reader);
                    reader.Close();
                }
                tc.End();
                oFNBatchQCDetail=new FNBatchQCDetail();
                oFNBatchQCDetail.FNBatchQCDetails = oFNBatchQCDetails;
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQCDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFNBatchQCDetail;
        }

        public FNBatchQCDetail ReceiveInDelivery(FNBatchQCDetail oFNBatchQCDetail, Int64 nUserID)
        {
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            TransactionContext tc = null;
            try
            {
                if (oFNBatchQCDetail.WorkingUnitID <= 0)
                    throw new Exception("Select receive store.");

                tc = TransactionContext.Begin(true);
                NullHandler oReader;
                IDataReader reader;


                oFNBatchQCDetail.FNBatchQCDetails.ForEach(x => x.WorkingUnitID = oFNBatchQCDetail.WorkingUnitID);
                foreach (FNBatchQCDetail oItem in oFNBatchQCDetail.FNBatchQCDetails)
                {
                    reader = FNBatchQCDetailDA.ReceiveInDelivery(tc, oItem, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNBatchQCDetail = new FNBatchQCDetail();
                        oFNBatchQCDetail = CreateObject(oReader);
                    }
                    reader.Close();
                }

                if (oFNBatchQCDetail.FNBatchQCID > 0)
                {
                    string sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCID=" + oFNBatchQCDetail.FNBatchQCID + " And IsLock=1";
                    reader = FNBatchQCDetailDA.Gets(tc, sSQL);
                    oFNBatchQCDetails = CreateObjects(reader);
                    reader.Close();
                }
                tc.End();
                oFNBatchQCDetail=new FNBatchQCDetail();
                oFNBatchQCDetail.FNBatchQCDetails = oFNBatchQCDetails;
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQCDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFNBatchQCDetail;
        }

        public FNBatchQCDetail ReceiveInDeliveryNew(FNBatchQCDetail oFNBatchQCDetail, Int64 nUserID)
        {
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            TransactionContext tc = null;
            try
            {
                if (oFNBatchQCDetail.WorkingUnitID <= 0)
                    throw new Exception("Select receive store.");

                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = FNBatchQCDetailDA.ReceiveInDeliveryNew(tc, oFNBatchQCDetail, nUserID);
                oFNBatchQCDetails = CreateObjects(reader);
                reader.Close();

                if (oFNBatchQCDetail.FNBatchQCID > 0)
                {
                    string sSQL = "SELECT * FROM FNBatchQCDetail WHERE FNBatchQCID=" + oFNBatchQCDetail.FNBatchQCID + " And IsLock=1";
                    reader = FNBatchQCDetailDA.Gets(tc, sSQL);
                    oFNBatchQCDetails = CreateObjects(reader);
                    reader.Close();
                }
                tc.End();
                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.FNBatchQCDetails = oFNBatchQCDetails;
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQCDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFNBatchQCDetail;
        }

        public List<FNBatchQCDetail> ExcessQtyUpdate(List<FNBatchQCDetail> oFNBatchQCDetails, Int64 nUserID)
        {
            FNBatchQCDetail oFNBatchQCDetail = new FNBatchQCDetail();
            List<FNBatchQCDetail> oFNBatchQCDetails_Return = new List<FNBatchQCDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
             
                IDataReader reader;
                foreach (FNBatchQCDetail oItem in oFNBatchQCDetails)
                {
                    reader = FNBatchQCDetailDA.ExcessQtyUpdate(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNBatchQCDetail = new FNBatchQCDetail();
                        oFNBatchQCDetail = CreateObject(oReader);
                        oFNBatchQCDetails_Return.Add(oFNBatchQCDetail);
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

                oFNBatchQCDetail = new FNBatchQCDetail();
                oFNBatchQCDetail.ErrorMessage = e.Message.Split('~')[0];
                oFNBatchQCDetails_Return.Add(oFNBatchQCDetail);

                #endregion
            }
            return oFNBatchQCDetails_Return;
        }
        #endregion
    }
}
