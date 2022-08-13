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
    public class KnitDyeingBatchService : MarshalByRefObject, IKnitDyeingBatchService
    {
        #region Private functions and declaration

        private KnitDyeingBatch MapObject(NullHandler oReader)
        {
            KnitDyeingBatch oKnitDyeingBatch = new KnitDyeingBatch();
            oKnitDyeingBatch.KnitDyeingBatchID = oReader.GetInt32("KnitDyeingBatchID");
            oKnitDyeingBatch.BUID = oReader.GetInt32("BUID");
            oKnitDyeingBatch.KnitDyingProgramID = oReader.GetInt32("KnitDyingProgramID");
            oKnitDyeingBatch.BatchIssueDate = oReader.GetDateTime("BatchIssueDate");
            oKnitDyeingBatch.BatchNo = oReader.GetString("BatchNo");
            oKnitDyeingBatch.RefObjectID = oReader.GetInt32("RefObjectID");
            oKnitDyeingBatch.ColorID = oReader.GetInt32("ColorID");
            oKnitDyeingBatch.FabricID = oReader.GetInt32("FabricID");
            oKnitDyeingBatch.WashTypeID = oReader.GetInt32("WashTypeID");
            oKnitDyeingBatch.FinishedGSMID = oReader.GetInt32("FinishedGSMID");
            oKnitDyeingBatch.MachineID = oReader.GetInt32("MachineID");
            oKnitDyeingBatch.LoadTime = oReader.GetDateTime("LoadTime");
            oKnitDyeingBatch.UnloadTime = oReader.GetDateTime("UnloadTime");
            oKnitDyeingBatch.MUnitID = oReader.GetInt32("MUnitID");
            oKnitDyeingBatch.MUName = oReader.GetString("MUName");
            oKnitDyeingBatch.TotalGrayQty = oReader.GetDouble("TotalGrayQty");
            oKnitDyeingBatch.TotalFinishQty = oReader.GetDouble("TotalFinishQty");
            oKnitDyeingBatch.ProcessLoss = oReader.GetDouble("ProcessLoss");
            oKnitDyeingBatch.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oKnitDyeingBatch.ApprovedByName = oReader.GetString("ApprovedByName");
            oKnitDyeingBatch.RefObjectNo = oReader.GetString("RefObjectNo");
            
            oKnitDyeingBatch.Remarks = oReader.GetString("Remarks");
            oKnitDyeingBatch.FabricName = oReader.GetString("FabricName");
            oKnitDyeingBatch.WashName = oReader.GetString("WashName");
            oKnitDyeingBatch.FinishedGSMName = oReader.GetString("FinishedGSMName");
            oKnitDyeingBatch.MachineName = oReader.GetString("MachineName");
            oKnitDyeingBatch.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oKnitDyeingBatch.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oKnitDyeingBatch.StyleNo = oReader.GetString("StyleNo");
            oKnitDyeingBatch.ColorName = oReader.GetString("ColorName");
            oKnitDyeingBatch.BuyerID = oReader.GetInt32("BuyerID");
            oKnitDyeingBatch.BuyerName = oReader.GetString("BuyerName");
            oKnitDyeingBatch.OrderQty = oReader.GetDouble("OrderQty");

            return oKnitDyeingBatch;
        }

        private KnitDyeingBatch CreateObject(NullHandler oReader)
        {
            KnitDyeingBatch oKnitDyeingBatch = new KnitDyeingBatch();
            oKnitDyeingBatch = MapObject(oReader);
            return oKnitDyeingBatch;
        }

        private List<KnitDyeingBatch> CreateObjects(IDataReader oReader)
        {
            List<KnitDyeingBatch> oKnitDyeingBatch = new List<KnitDyeingBatch>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnitDyeingBatch oItem = CreateObject(oHandler);
                oKnitDyeingBatch.Add(oItem);
            }
            return oKnitDyeingBatch;
        }

        #endregion

        #region Interface implementation
        public KnitDyeingBatch Save(KnitDyeingBatch oKnitDyeingBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnitDyeingBatch.KnitDyeingBatchID <= 0)
                {
                    reader = KnitDyeingBatchDA.InsertUpdate(tc, oKnitDyeingBatch, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = KnitDyeingBatchDA.InsertUpdate(tc, oKnitDyeingBatch, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingBatch = new KnitDyeingBatch();
                    oKnitDyeingBatch = CreateObject(oReader);
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
                    oKnitDyeingBatch = new KnitDyeingBatch();
                    oKnitDyeingBatch.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnitDyeingBatch;
        }
        public KnitDyeingBatch Approved(KnitDyeingBatch oKnitDyeingBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                KnitDyeingBatchDA.Approved(tc, oKnitDyeingBatch, EnumDBOperation.Approval, nUserID);
                reader = KnitDyeingBatchDA.Get(tc, oKnitDyeingBatch.KnitDyeingBatchID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingBatch = new KnitDyeingBatch();
                    oKnitDyeingBatch = CreateObject(oReader);
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
                    oKnitDyeingBatch = new KnitDyeingBatch();
                    oKnitDyeingBatch.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnitDyeingBatch;
        }
        public KnitDyeingBatch SaveAll(KnitDyeingBatch oKnitDyeingBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            KnitDyeingBatch objKnitDyeingBatch = new KnitDyeingBatch();
            KnitDyeingBatchDetail objKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
            List<KnitDyeingBatchDetail> objKnitDyeingBatchDetails = new List<KnitDyeingBatchDetail>();
            KnitDyeingBatch oUG = new KnitDyeingBatch();
            oUG = oKnitDyeingBatch;

            try
            {
                #region Save Parent
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnitDyeingBatch.KnitDyeingBatchID <= 0)
                {
                    reader = KnitDyeingBatchDA.InsertUpdate(tc, oKnitDyeingBatch, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = KnitDyeingBatchDA.InsertUpdate(tc, oKnitDyeingBatch, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    objKnitDyeingBatch = new KnitDyeingBatch();
                    objKnitDyeingBatch = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Knit Dyeing Batch Details
                string ids = "";
                objKnitDyeingBatch.KnitDyeingBatchDetails = oKnitDyeingBatch.KnitDyeingBatchDetails;
                if (objKnitDyeingBatch.KnitDyeingBatchID > 0)
                {
                    if (oKnitDyeingBatch.KnitDyeingBatchDetails.Count > 0)
                    {
                        #region Delete Previous
                        foreach (KnitDyeingBatchDetail item in oKnitDyeingBatch.KnitDyeingBatchDetails)
                        {
                            if (item.KnitDyeingBatchDetailID > 0)
                            {
                                ids = ids + item.KnitDyeingBatchDetailID + ",";
                            }
                        }
                        if (ids.Length > 0) ids = ids.Remove(ids.Length - 1);
                        KnitDyeingBatchDetailDA.DeleteList(tc, ids, objKnitDyeingBatch.KnitDyeingBatchID, nUserID);
                        #endregion

                        #region Save Updated Details
                        foreach (KnitDyeingBatchDetail item in objKnitDyeingBatch.KnitDyeingBatchDetails)
                        {
                            IDataReader readerDetail;
                            item.KnitDyeingBatchID = objKnitDyeingBatch.KnitDyeingBatchID;
                            if (item.KnitDyeingBatchDetailID <= 0)
                            {
                                readerDetail = KnitDyeingBatchDetailDA.InsertUpdate(tc, item, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                readerDetail = KnitDyeingBatchDetailDA.InsertUpdate(tc, item, EnumDBOperation.Update, nUserID);
                            }
                            NullHandler oReaderDetail = new NullHandler(readerDetail);
                            if (readerDetail.Read())
                            {
                                objKnitDyeingBatchDetail = new KnitDyeingBatchDetail();
                                objKnitDyeingBatchDetail = KnitDyeingBatchDetailService.CreateObject(oReaderDetail);
                            }
                            readerDetail.Close();
                            objKnitDyeingBatchDetails.Add(objKnitDyeingBatchDetail);
                        }
                        #endregion
                    }
                }
                #endregion

                #region Knit Dyeing Batch Yarns
                if (objKnitDyeingBatch.KnitDyeingBatchID > 0)
                {
                    string sKnitDyeingBatchYarnIDs = "";
                    if (oUG.KnitDyeingBatchYarns.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (KnitDyeingBatchYarn oDRD in oUG.KnitDyeingBatchYarns)
                        {
                            oDRD.KnitDyeingBatchID = objKnitDyeingBatch.KnitDyeingBatchID;
                            if (oDRD.KnitDyeingBatchYarnID <= 0)
                            {
                                readerdetail = KnitDyeingBatchYarnDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = KnitDyeingBatchYarnDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nKnitDyeingBatchYarnID = 0;
                            if (readerdetail.Read())
                            {
                                nKnitDyeingBatchYarnID = oReaderDevRecapdetail.GetInt32("KnitDyeingBatchYarnID");
                                sKnitDyeingBatchYarnIDs = sKnitDyeingBatchYarnIDs + oReaderDevRecapdetail.GetString("KnitDyeingBatchYarnID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sKnitDyeingBatchYarnIDs.Length > 0)
                    {
                        sKnitDyeingBatchYarnIDs = sKnitDyeingBatchYarnIDs.Remove(sKnitDyeingBatchYarnIDs.Length - 1, 1);
                    }
                    KnitDyeingBatchYarn oKnitDyeingBatchYarn = new KnitDyeingBatchYarn();
                    oKnitDyeingBatchYarn.KnitDyeingBatchID = objKnitDyeingBatch.KnitDyeingBatchID;
                    KnitDyeingBatchYarnDA.Delete(tc, oKnitDyeingBatchYarn, EnumDBOperation.Delete, nUserID, sKnitDyeingBatchYarnIDs);
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
                    objKnitDyeingBatch = new KnitDyeingBatch();
                    objKnitDyeingBatch.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            objKnitDyeingBatch.KnitDyeingBatchDetails = objKnitDyeingBatchDetails;
            return objKnitDyeingBatch;
        }
        public KnitDyeingBatch SaveKnitDyeingBatchGrayChallans(KnitDyeingBatch oKnitDyeingBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            KnitDyeingBatchGrayChallan oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
            try
            {
                tc = TransactionContext.Begin(true);
                #region Delete Previous
                string ids = "";
                foreach (KnitDyeingBatchGrayChallan item in oKnitDyeingBatch.KnitDyeingBatchGrayChallans)
                {
                    if (item.KnitDyeingBatchGrayChallanID > 0)
                    {
                        ids = ids + item.KnitDyeingBatchGrayChallanID + ",";
                    }
                }
                if (ids.Length > 0) ids = ids.Remove(ids.Length - 1);
                KnitDyeingBatchGrayChallanDA.DeleteList(tc, ids, oKnitDyeingBatch.KnitDyeingBatchID, nUserID);
                #endregion
                foreach (KnitDyeingBatchGrayChallan item in oKnitDyeingBatch.KnitDyeingBatchGrayChallans)
                {
                    IDataReader readerDetail;
                    if (item.KnitDyeingBatchGrayChallanID <= 0)
                    {
                        readerDetail = KnitDyeingBatchGrayChallanDA.InsertUpdate(tc, item, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerDetail = KnitDyeingBatchGrayChallanDA.InsertUpdate(tc, item, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerDetail);
                    if (readerDetail.Read())
                    {
                        oKnitDyeingBatchGrayChallan = new KnitDyeingBatchGrayChallan();
                        oKnitDyeingBatchGrayChallan = KnitDyeingBatchGrayChallanService.CreateObject(oReaderDetail);
                    }
                    readerDetail.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnitDyeingBatch = new KnitDyeingBatch();
                    oKnitDyeingBatch.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnitDyeingBatch;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnitDyeingBatch oKnitDyeingBatch = new KnitDyeingBatch();
                oKnitDyeingBatch.KnitDyeingBatchID = id;
                DBTableReferenceDA.HasReference(tc, "KnitDyeingBatch", id);
                KnitDyeingBatchDA.Delete(tc, oKnitDyeingBatch, EnumDBOperation.Delete, nUserId);
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

        public KnitDyeingBatch Get(int id, Int64 nUserId)
        {
            KnitDyeingBatch oKnitDyeingBatch = new KnitDyeingBatch();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnitDyeingBatchDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingBatch = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnitDyeingBatch", e);
                #endregion
            }
            return oKnitDyeingBatch;
        }

        public List<KnitDyeingBatch> Gets(Int64 nUserID)
        {
            List<KnitDyeingBatch> oKnitDyeingBatchs = new List<KnitDyeingBatch>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingBatchDA.Gets(tc);
                oKnitDyeingBatchs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnitDyeingBatch oKnitDyeingBatch = new KnitDyeingBatch();
                oKnitDyeingBatch.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnitDyeingBatchs;
        }

        public List<KnitDyeingBatch> Gets(string sSQL, Int64 nUserID)
        {
            List<KnitDyeingBatch> oKnitDyeingBatchs = new List<KnitDyeingBatch>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingBatchDA.Gets(tc, sSQL);
                oKnitDyeingBatchs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnitDyeingBatch", e);
                #endregion
            }
            return oKnitDyeingBatchs;
        }

        #endregion
    }

}
