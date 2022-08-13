using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class FabricBatchLoomService : MarshalByRefObject, IFabricBatchLoomService
    {
        #region Private functions and declaration
        private FabricBatchLoom MapObject(NullHandler oReader)
        {
            FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
            oFabricBatchLoom.FabricBatchLoomID = oReader.GetInt32("FabricBatchLoomID");
            oFabricBatchLoom.FBID = oReader.GetInt32("FBID");
            oFabricBatchLoom.BatchNo = oReader.GetString("BatchNo");
            oFabricBatchLoom.FLPID = oReader.GetInt32("FLPID");
            oFabricBatchLoom.FabricMachineName = oReader.GetString("FabricMachineName");
            oFabricBatchLoom.Qty = oReader.GetDouble("Qty");
            oFabricBatchLoom.FabricBatchStatus = (EnumFabricBatchState)oReader.GetInt32("FabricBatchStatus");
            oFabricBatchLoom.FabricBatchStatusInInt = oReader.GetInt32("FabricBatchStatus");
            oFabricBatchLoom.FEONo = oReader.GetString("FEONo");
            oFabricBatchLoom.BuyerName = oReader.GetString("BuyerName");
            oFabricBatchLoom.Reed = oReader.GetString("Reed");
            oFabricBatchLoom.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oFabricBatchLoom.FMID = oReader.GetInt32("FMID");
            oFabricBatchLoom.Construction = oReader.GetString("Construction");
            oFabricBatchLoom.StartTime = oReader.GetDateTime("StartTime");
            oFabricBatchLoom.EndTime = oReader.GetDateTime("EndTime");
            oFabricBatchLoom.WarpDoneQty = oReader.GetDouble("WarpDoneQty");
            oFabricBatchLoom.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricBatchLoom.WeavingProcess = (EnumWeavingProcess) oReader.GetInt32("WeavingProcess");
            oFabricBatchLoom.RPM = oReader.GetInt32("RPM");
            oFabricBatchLoom.Texture = oReader.GetString("Texture");
            oFabricBatchLoom.ReedCount = oReader.GetDouble("ReedCount");
            oFabricBatchLoom.Dent = oReader.GetString("Dent");
            oFabricBatchLoom.TSUID = oReader.GetInt32("TSUID");
            //oFabricBatchLoom.FabricBatchQty = oReader.GetDouble("FabricBatchQty");
            oFabricBatchLoom.MachineStatus = (EnumMachineStatus) oReader.GetInt32("MachineStatus");
            oFabricBatchLoom.TotalEnds = oReader.GetDouble("TotalEnds");
            oFabricBatchLoom.NoOfSection = oReader.GetInt32("NoOfSection");
            oFabricBatchLoom.WarpCount = oReader.GetDouble("WarpCount");
            oFabricBatchLoom.FabricWeave = oReader.GetString("FabricWeave");
            oFabricBatchLoom.ProductID = oReader.GetInt32("ProductID");
            oFabricBatchLoom.ProductName = oReader.GetString("ProductName");
            oFabricBatchLoom.MachineCode = oReader.GetString("Code");
            oFabricBatchLoom.BatchStatus = (oFabricBatchLoom.EndTime != DateTime.MinValue) ? EnumBatchStatus.Out : ((oFabricBatchLoom.StartTime != DateTime.MinValue) ? EnumBatchStatus.Run : EnumBatchStatus.None);
            oFabricBatchLoom.ColorName = oReader.GetString("ColorName");
            oFabricBatchLoom.ShiftID = oReader.GetInt32("ShiftID");
            oFabricBatchLoom.Status = (EnumFabricBatchStatus)oReader.GetInt32("Status");
            oFabricBatchLoom.FBPBeamID = oReader.GetInt32("FBPBeamID");
            oFabricBatchLoom.BeamID = oReader.GetInt32("BeamID");
            oFabricBatchLoom.QtyPro = oReader.GetDouble("QtyPro");
            oFabricBatchLoom.BuyerID = oReader.GetInt32("ContractorID");
            oFabricBatchLoom.FSCDID = oReader.GetInt32("FSCDID");
            oFabricBatchLoom.FEOSID = oReader.GetInt32("FEOSID");
            oFabricBatchLoom.FSpcType = (EnumFabricSpeType)oReader.GetInt32("FSpcType");
            oFabricBatchLoom.PlanType = (EnumPlanType)oReader.GetInt32("PlanType");
            oFabricBatchLoom.BeamQty = oReader.GetDouble("BeamQty");
            oFabricBatchLoom.BeamNo = oReader.GetString("BeamNo");
            oFabricBatchLoom.Efficiency = oReader.GetDouble("Efficiency");
            oFabricBatchLoom.Pick = oReader.GetDouble("Pick");

            return oFabricBatchLoom;
        }
        private FabricBatchLoom CreateObject(NullHandler oReader)
        {
            FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
            oFabricBatchLoom = MapObject(oReader);
            return oFabricBatchLoom;
        }
        private List<FabricBatchLoom> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchLoom> oFabricBatchLoom = new List<FabricBatchLoom>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchLoom oItem = CreateObject(oHandler);
                oFabricBatchLoom.Add(oItem);
            }
            return oFabricBatchLoom;
        }
        #endregion

        #region Interface implementation
        public FabricBatchLoom Save(FabricBatchLoom oFabricBatchLoom, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<FabricBatchLoomDetail> oFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
                oFabricBatchLoomDetails = oFabricBatchLoom.FabricBatchLoomDetails;

                //if (oFabricBatchLoom.WeavingProcess == EnumWeavingProcess.Loom && oFBPBs.Count>0)
                //{
                //    oFabricBatchLoom.BeamID = oFBPBs[0].BeamID;
                //}

                tc = TransactionContext.Begin(true);
             
                IDataReader reader;
                if (oFabricBatchLoom.FabricBatchLoomID <= 0)
                {
                    reader = FabricBatchLoomDA.InsertUpdate(tc, oFabricBatchLoom, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    if (oFabricBatchLoom.IsHold)
                        reader = FabricBatchLoomDA.InsertUpdate(tc, oFabricBatchLoom, EnumDBOperation.Hold, nUserID);
                    else if (oFabricBatchLoom.IsRunOut)
                        reader = FabricBatchLoomDA.InsertUpdate(tc, oFabricBatchLoom, EnumDBOperation.Active, nUserID);
                    else
                        reader = FabricBatchLoomDA.InsertUpdate(tc, oFabricBatchLoom, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchLoom = new FabricBatchLoom();
                    oFabricBatchLoom = CreateObject(oReader);
                }
                reader.Close();

                #region Insert Beams
                if (oFabricBatchLoomDetails.Count > 0)
                {
                    foreach (FabricBatchLoomDetail oItem in oFabricBatchLoomDetails)
                    {
                        IDataReader readerFBPF;
                        oItem.FabricBatchLoomID = oFabricBatchLoom.FabricBatchLoomID;
                        if (oItem.FBLDetailID <= 0)
                        {
                            readerFBPF = FabricBatchLoomDetailDA.IUD(tc, EnumDBOperation.Insert, oItem, nUserID);
                        }
                        else
                        {
                            readerFBPF = FabricBatchLoomDetailDA.IUD(tc, EnumDBOperation.Update, oItem, nUserID);
                        }
                        readerFBPF.Close();
                    }

                    #region Get Beams
                    oFabricBatchLoom.FabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
                    FabricBatchLoomDetailService oFBPBS = new FabricBatchLoomDetailService();
                    IDataReader readerFBPBs = null;
                    readerFBPBs = FabricBatchLoomDetailDA.Gets(tc, oFabricBatchLoom.FabricBatchLoomID);
                    oFabricBatchLoom.FabricBatchLoomDetails = oFBPBS.CreateObjects(readerFBPBs);
                    readerFBPBs.Close();
                    #endregion
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchLoom = new FabricBatchLoom();
                oFabricBatchLoom.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchLoom;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
                oFabricBatchLoom.FabricBatchLoomID = id;
                FabricBatchLoomDA.Delete(tc, oFabricBatchLoom, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public FabricBatchLoom Get(int id, Int64 nUserId)
        {
            FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchLoomDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchLoom = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchLoom = new FabricBatchLoom();
                oFabricBatchLoom.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchLoom;
        }
        public FabricBatchLoom GetByBatchAndWeavingType(int FBID,int nProcess,  Int64 nUserId)
         {
             FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
             TransactionContext tc = null;
             try
             {
                 tc = TransactionContext.Begin();
                 IDataReader reader = FabricBatchLoomDA.GetByBatchAndWeavingType(tc, FBID, nProcess);
                 NullHandler oReader = new NullHandler(reader);
                 if (reader.Read())
                 {
                     oFabricBatchLoom = CreateObject(oReader);
                 }
                 reader.Close();
                 tc.End();
             }
             catch (Exception e)
             {
                 #region Handle Exception
                 if (tc != null)
                     tc.HandleError();
                 oFabricBatchLoom = new FabricBatchLoom();
                 oFabricBatchLoom.ErrorMessage = e.Message.Split('!')[0];
                 #endregion
             }
             return oFabricBatchLoom;
         }
        public List<FabricBatchLoom> Gets(int nFBID, Int64 nUserID)
        {
            List<FabricBatchLoom> oFabricBatchLooms = new List<FabricBatchLoom>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchLoomDA.Gets(nFBID,tc);
                oFabricBatchLooms = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
                oFabricBatchLoom.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchLooms.Add(oFabricBatchLoom);
                #endregion
            }
            return oFabricBatchLooms;
        }
        public List<FabricBatchLoom> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBatchLoom> oFabricBatchLooms = new List<FabricBatchLoom>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchLoomDA.Gets(tc, sSQL);
                oFabricBatchLooms = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
                oFabricBatchLoom.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchLooms.Add(oFabricBatchLoom);
                #endregion
            }
            return oFabricBatchLooms;
        }
        public List<FabricBatchLoom> ImportFabricBatchLoom(List<FabricBatchLoom> oFabricBatchLooms, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            NullHandler oReader;
            List<FabricBatchLoom> oFBPs = new List<FabricBatchLoom>();
            FabricBatchLoom oFBP = new FabricBatchLoom();

            foreach (FabricBatchLoom oItem in oFabricBatchLooms)
            {
                tc = TransactionContext.Begin(true);

                try
                {
                    DateTime dtRunOut = oItem.EndTime;
                    int nBeamID = oItem.BeamID;
                    oFBP = oItem;

                    var oFBPBMs = oItem.FabricBatchLoomDetails;
                    if (oItem.FabricBatchLoomID <= 0)
                    {
                        reader = FabricBatchLoomDA.ImportFabricBatchLoomDA(tc, oItem, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFBP = new FabricBatchLoom();
                            oFBP = CreateObject(oReader);
                            oFBP.BeamID = nBeamID;
                            oFBP.EndTime = dtRunOut;
                        }
                        reader.Close();

                    }

                    if (oFBP.FabricBatchLoomID > 0 && oFBPBMs.Any())
                    {
                        oFBPBMs.ForEach(x => { x.FabricBatchLoomID = oFBP.FabricBatchLoomID; });

                        //Insert FabricBatchLoomBatchMan
                        foreach (FabricBatchLoomDetail oFBPBM in oFBPBMs)
                        {
                            var oFBPBreakages = oFBPBM.FBPBreakages;
                            var otemp = new FabricBatchLoomDetail();
                            oFBPBM.Qty = Global.GetYard(oFBPBM.Qty, 2);
                            reader = FabricBatchLoomDetailDA.IUD(tc, EnumDBOperation.Insert, oFBPBM, nUserID);
                            oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                otemp = new FabricBatchLoomDetail();
                                otemp = FabricBatchLoomDetailService.CreateObject(oReader);
                            }
                            reader.Close();

                            //Insert FabricBatchLoomBreakage
                            if (otemp.FBLDetailID > 0 && oFBPBreakages.Any())
                            {
                                oFBPBreakages.ForEach(x => { x.FBLDetailID = otemp.FBLDetailID; });
                                foreach (FabricBatchProductionBreakage oFBPB in oFBPBreakages)
                                {
                                    reader = FabricBatchProductionBreakageDA.IUD(tc, EnumDBOperation.Insert, oFBPB, nUserID);
                                    reader.Close();
                                }
                            }
                        }
                    }


                    //Runout
                    if (oFBP.FabricBatchLoomID > 0 && oFBP.EndTime != DateTime.MinValue)
                    {
                        reader = FabricBatchLoomDA.ImportFabricBatchLoomDA(tc, oFBP, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFBP = new FabricBatchLoom();
                            oFBP = CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    oFBPs.Add(oFBP);
                    tc.End();
                }
                catch (Exception ex)
                {
                    #region Handle Exception
                    tc.HandleError();
                    oFBP.FabricBatchLoomID = 0; 
                    oFBP.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                    oFBPs.Add(oFBP);
                    #endregion
                }
                    
            }
            return oFBPs;
        }
        public FabricBatchLoom UpdateWeaving(FabricBatchLoom oFBP, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                FabricBatchLoomDA.UpdateWeaving(tc, oFBP, nUserID);

                IDataReader reader;
                reader = FabricBatchLoomDA.Get(tc,oFBP.FabricBatchLoomID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFBP = new FabricBatchLoom();
                    oFBP = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFBP = new FabricBatchLoom();
                oFBP.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFBP;
        }
        public List<FabricBatchLoom> GetsSummery(string sSQL, Int64 nUserID)
        {
            List<FabricBatchLoom> oFabricBatchLooms = new List<FabricBatchLoom>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchLoomDA.GetsSummery(tc, sSQL);
                oFabricBatchLooms = CreateObjectsForSummery(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
                oFabricBatchLoom.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchLooms.Add(oFabricBatchLoom);
                #endregion
            }
            return oFabricBatchLooms;
        }
        private FabricBatchLoom MapObjectForsummery(NullHandler oReader)
        {
            FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
        
            oFabricBatchLoom.Qty =Global.GetMeter(oReader.GetDouble("Qty"),2);
            oFabricBatchLoom.RPM = oReader.GetInt32("RPM");
            oFabricBatchLoom.Efficiency = oReader.GetDouble("Efficiency");
            oFabricBatchLoom.TSUID = oReader.GetInt32("TSUID");
        
            return oFabricBatchLoom;
        }
        private FabricBatchLoom CreateObjectForSummery(NullHandler oReader)
        {
            FabricBatchLoom oFabricBatchLoom = new FabricBatchLoom();
            oFabricBatchLoom = MapObjectForsummery(oReader);
            return oFabricBatchLoom;
        }
        private List<FabricBatchLoom> CreateObjectsForSummery(IDataReader oReader)
        {
            List<FabricBatchLoom> oFabricBatchLoom = new List<FabricBatchLoom>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchLoom oItem = CreateObjectForSummery(oHandler);
                oFabricBatchLoom.Add(oItem);
            }
            return oFabricBatchLoom;
        }


        #endregion
    }
}
