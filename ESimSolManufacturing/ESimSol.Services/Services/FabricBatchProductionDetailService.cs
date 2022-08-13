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
    public class FabricBatchProductionDetailService : MarshalByRefObject, IFabricBatchProductionDetailService
    {
        #region Private functions and declaration
        public static FabricBatchProductionDetail MapObject(NullHandler oReader)
        {
            FabricBatchProductionDetail oFabricBatchProductionDetail = new FabricBatchProductionDetail();
            oFabricBatchProductionDetail.FBPDetailID = oReader.GetInt32("FBPDetailID");
            oFabricBatchProductionDetail.FBPID = oReader.GetInt32("FBPID");
            oFabricBatchProductionDetail.ShiftStartTime = oReader.GetDateTime("ShiftStartTime");
            oFabricBatchProductionDetail.ShiftEndTime = oReader.GetDateTime("ShiftEndTime");
            oFabricBatchProductionDetail.EmployeeID = oReader.GetInt32("EmployeeID");
            oFabricBatchProductionDetail.EmployeeName = oReader.GetString("EmployeeName");
            oFabricBatchProductionDetail.Note = oReader.GetString("Note");
            oFabricBatchProductionDetail.ShiftID = oReader.GetInt32("ShiftID");
            oFabricBatchProductionDetail.ShiftName = oReader.GetString("ShiftName");
            oFabricBatchProductionDetail.Qty = oReader.GetDouble("Qty");
            oFabricBatchProductionDetail.QtyBatch = oReader.GetDouble("QtyBatch");
            oFabricBatchProductionDetail.BatchNo = oReader.GetString("BatchNo");
            oFabricBatchProductionDetail.TotalEnds = oReader.GetDouble("TotalEnds");
            oFabricBatchProductionDetail.ProductionDate = oReader.GetDateTime("ProductionDate");
            oFabricBatchProductionDetail.NoOfBreakage = oReader.GetInt32("NoOfBreakage");
            oFabricBatchProductionDetail.EntryDate = oReader.GetDateTime("EntryDate");
            oFabricBatchProductionDetail.UserName = oReader.GetString("UserName");
            oFabricBatchProductionDetail.FBID = oReader.GetInt32("FBID");
            oFabricBatchProductionDetail.FMID = oReader.GetInt32("FMID");
            oFabricBatchProductionDetail.MachineName = oReader.GetString("MachineName");
            oFabricBatchProductionDetail.NoOfFrame = oReader.GetDouble("NoOfFrame");
            oFabricBatchProductionDetail.ProductionStatus = (EnumProductionStatus)oReader.GetInt32("ProductionStatus");
            oFabricBatchProductionDetail.FEOSID = oReader.GetInt32("FEOSID");
            oFabricBatchProductionDetail.ReedCount = oReader.GetDouble("ReedCount");

            return oFabricBatchProductionDetail;
        }

        public static FabricBatchProductionDetail CreateObject(NullHandler oReader)
        {
            FabricBatchProductionDetail oFabricBatchProductionDetail = new FabricBatchProductionDetail();
            oFabricBatchProductionDetail = MapObject(oReader);
            return oFabricBatchProductionDetail;
        }

        private List<FabricBatchProductionDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchProductionDetail> oFabricBatchProductionDetail = new List<FabricBatchProductionDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchProductionDetail oItem = CreateObject(oHandler);
                oFabricBatchProductionDetail.Add(oItem);
            }
            return oFabricBatchProductionDetail;
        }

        #endregion

        public List<FabricBatchProductionDetail> Gets(int nFBPID, Int64 nUserID)
        {
            List<FabricBatchProductionDetail> oFabricBatchProductionDetails = new List<FabricBatchProductionDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchProductionDetailDA.Gets(tc, nFBPID);
                oFabricBatchProductionDetails = CreateObjects(reader);
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
            return oFabricBatchProductionDetails;
        }

        public List<FabricBatchProductionDetail> Gets(string sSql, Int64 nUserID)
        {
            List<FabricBatchProductionDetail> oFabricBatchProductionDetails = new List<FabricBatchProductionDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchProductionDetailDA.Gets(tc, sSql);
                oFabricBatchProductionDetails = CreateObjects(reader);
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
            return oFabricBatchProductionDetails;
        }

        public FabricBatchProductionDetail Get(int id, Int64 nUserId)
        {
            FabricBatchProductionDetail oFabricBatchProductionDetail = new FabricBatchProductionDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchProductionDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricBatchProductionDetail", e);
                #endregion
            }
            return oFabricBatchProductionDetail;
        }

        
        public FabricBatchProductionDetail Save(FabricBatchProductionDetail oFabricBatchProductionDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin(true);

                IDataReader reader;
                if (oFabricBatchProductionDetail.FBPDetailID <= 0)
                {
                    reader = FabricBatchProductionDetailDA.IUD(tc, EnumDBOperation.Insert, oFabricBatchProductionDetail, nUserID);
                }
                else
                {
                    reader = FabricBatchProductionDetailDA.IUD(tc, EnumDBOperation.Update, oFabricBatchProductionDetail, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionDetail = new FabricBatchProductionDetail();
                    oFabricBatchProductionDetail = CreateObject(oReader);
                }
                else
                {
                    oFabricBatchProductionDetail.ErrorMessage = "Invalid fabric batch production batch man !!!";
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionDetail = new FabricBatchProductionDetail();
                oFabricBatchProductionDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchProductionDetail;
        }

        public FabricBatchProductionDetail SaveDetailWithBeam(FabricBatchProductionDetail oFabricBatchProductionDetail, Int64 nUserID)
        {
            FabricBatchProductionBeam oFabricBatchProductionBeam = new FabricBatchProductionBeam();
            FabricBatchProductionDetail oUG = new FabricBatchProductionDetail();
            oUG = oFabricBatchProductionDetail;
            double nQty = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region FabricBatchProductionDetail
                IDataReader reader;
                if (oFabricBatchProductionDetail.FBPDetailID <= 0)
                {
                    reader = FabricBatchProductionDetailDA.IUD(tc, EnumDBOperation.Insert, oFabricBatchProductionDetail, nUserID);
                }
                else
                {
                    reader = FabricBatchProductionDetailDA.IUD(tc, EnumDBOperation.Update, oFabricBatchProductionDetail, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionDetail = new FabricBatchProductionDetail();
                    oFabricBatchProductionDetail = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Detail

                if (oFabricBatchProductionDetail.FBPDetailID > 0)
                {
                    nQty = 0;
                    if (oUG.FabricBatchProductionBeams.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (FabricBatchProductionBeam oDRD in oUG.FabricBatchProductionBeams)
                        {
                            oDRD.FBPDetailID = oFabricBatchProductionDetail.FBPDetailID;
                            nQty = nQty + oDRD.Qty;
                            if (oDRD.FBPBeamID <= 0)
                            {
                                readerdetail = FabricBatchProductionBeamDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                readerdetail = FabricBatchProductionBeamDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID);

                            }
                       
                            readerdetail.Close();
                        }
                        oFabricBatchProductionDetail.Qty = nQty;
                    }
                  
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
                    oFabricBatchProductionDetail = new FabricBatchProductionDetail();
                    oFabricBatchProductionDetail.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricBatchProductionDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchProductionDetail oFabricBatchProductionDetail = new FabricBatchProductionDetail();
                oFabricBatchProductionDetail.FBPDetailID = id;
                FabricBatchProductionDetailDA.Delete(tc, EnumDBOperation.Delete, oFabricBatchProductionDetail, nUserId);
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
        public string MultipleApprove(String FBPDetailIDS, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchProductionDetailDA.MultipleApprove(tc, FBPDetailIDS, nUserId);
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
            return "Success";
        }
        public string MultipleDelete(String FBPDetailIDS, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchProductionDetailDA.MultipleDelete(tc, FBPDetailIDS, nUserId);
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
