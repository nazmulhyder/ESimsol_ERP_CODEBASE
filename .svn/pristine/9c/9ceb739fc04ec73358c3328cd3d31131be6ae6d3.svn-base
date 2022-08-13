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
    public class FabricBatchProductionService : MarshalByRefObject, IFabricBatchProductionService
    {
        #region Private functions and declaration
        private FabricBatchProduction MapObject(NullHandler oReader)
        {
            FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
            oFabricBatchProduction.FBPID = oReader.GetInt32("FBPID");
            oFabricBatchProduction.FBID = oReader.GetInt32("FBID");
            oFabricBatchProduction.BatchNo = oReader.GetString("BatchNo");
            oFabricBatchProduction.FabricSalesContractDetailID = oReader.GetInt32("FabricSalesContractDetailID");
            oFabricBatchProduction.FabricMachineName = oReader.GetString("FabricMachineName");
            oFabricBatchProduction.Qty = oReader.GetDouble("Qty");
            oFabricBatchProduction.FabricBatchStatus = (EnumFabricBatchState)oReader.GetInt32("FabricBatchStatus");
            oFabricBatchProduction.FabricBatchStatusInInt = oReader.GetInt32("FabricBatchStatus");
            oFabricBatchProduction.FEOID = oReader.GetInt32("FEOID");
            oFabricBatchProduction.FEONo = oReader.GetString("FEONo");
            oFabricBatchProduction.FEOSID = oReader.GetInt32("FEOSID");
            oFabricBatchProduction.FSpcType = (EnumFabricSpeType)oReader.GetInt32("FSpcType");
            oFabricBatchProduction.BuyerName = oReader.GetString("BuyerName");
            //oFabricBatchProduction.Reed = oReader.GetString("Reed");
            oFabricBatchProduction.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oFabricBatchProduction.FMID = oReader.GetInt32("FMID");
            oFabricBatchProduction.Construction = oReader.GetString("Construction");
            oFabricBatchProduction.StartTime = oReader.GetDateTime("StartTime");
            oFabricBatchProduction.EndTime = oReader.GetDateTime("EndTime");
            oFabricBatchProduction.PlanType = (EnumPlanType)oReader.GetInt32("PlanType");
          
            oFabricBatchProduction.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricBatchProduction.WeavingProcess = (EnumWeavingProcess) oReader.GetInt32("WeavingProcess");
            oFabricBatchProduction.Texture = oReader.GetString("Texture");
            oFabricBatchProduction.FabricBatchQty = oReader.GetDouble("FabricBatchQty");
            oFabricBatchProduction.MachineStatus = (EnumMachineStatus) oReader.GetInt32("MachineStatus");
            oFabricBatchProduction.TotalEnds = oReader.GetDouble("TotalEnds");
            oFabricBatchProduction.NoOfSection = oReader.GetInt32("NoOfSection");
            oFabricBatchProduction.WarpCount = oReader.GetDouble("WarpCount");
            oFabricBatchProduction.FabricWeave = oReader.GetString("FabricWeave");
            oFabricBatchProduction.ProductID = oReader.GetInt32("ProductID");
            oFabricBatchProduction.ProductName = oReader.GetString("ProductName");
            oFabricBatchProduction.MachineCode = oReader.GetString("Code");
            oFabricBatchProduction.BatchStatus = (oFabricBatchProduction.EndTime != DateTime.MinValue) ? EnumBatchStatus.Out : ((oFabricBatchProduction.StartTime != DateTime.MinValue) ? EnumBatchStatus.Run : EnumBatchStatus.None);
            oFabricBatchProduction.ColorName = oReader.GetString("ColorName");
            oFabricBatchProduction.ShiftID = oReader.GetInt32("ShiftID");
            oFabricBatchProduction.NoOfColor = oReader.GetInt32("NoOfColor");
            oFabricBatchProduction.NoOfColorWF = oReader.GetInt32("NoOfColorWF");
            oFabricBatchProduction.ProductionStatus = (EnumProductionStatus)oReader.GetInt32("ProductionStatus");
            oFabricBatchProduction.IsDirect = oReader.GetBoolean("IsDirect");
            oFabricBatchProduction.WarpBeam = oReader.GetInt32("WarpBeam");
            //Nazmul
            oFabricBatchProduction.FinishDate = oReader.GetDateTime("FinishDate");
            oFabricBatchProduction.SettingLength = oReader.GetDouble("SettingLength"); 

            return oFabricBatchProduction;
        }
        private FabricBatchProduction CreateObject(NullHandler oReader)
        {
            FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
            oFabricBatchProduction = MapObject(oReader);
            return oFabricBatchProduction;
        }
        private List<FabricBatchProduction> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchProduction> oFabricBatchProduction = new List<FabricBatchProduction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchProduction oItem = CreateObject(oHandler);
                oFabricBatchProduction.Add(oItem);
            }
            return oFabricBatchProduction;
        }
        #endregion

        #region Interface implementation
        public FabricBatchProduction Save(FabricBatchProduction oFabricBatchProduction, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<FabricBatchProductionBeam> oFBPBs = new List<FabricBatchProductionBeam>();
                oFBPBs = oFabricBatchProduction.FBPBs;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricBatchProduction.FBPID <= 0)
                {
                    reader = FabricBatchProductionDA.InsertUpdate(tc, oFabricBatchProduction, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricBatchProductionDA.InsertUpdate(tc, oFabricBatchProduction, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProduction = new FabricBatchProduction();
                    oFabricBatchProduction = CreateObject(oReader);
                }
                reader.Close();

                #region Insert Beams
                if (oFBPBs.Count > 0)
                {
                    foreach (FabricBatchProductionBeam oItem in oFBPBs)
                    {
                        IDataReader readerFBPF;
                        oItem.FBPID = oFabricBatchProduction.FBPID;
                        if (oItem.FBPBeamID <= 0)
                        {
                            readerFBPF = FabricBatchProductionBeamDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerFBPF = FabricBatchProductionBeamDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        readerFBPF.Close();
                    }

                    #region Get Beams
                    oFabricBatchProduction.FBPBs = new List<FabricBatchProductionBeam>();
                    FabricBatchProductionBeamService oFBPBS = new FabricBatchProductionBeamService();
                    IDataReader readerFBPBs = null;
                    readerFBPBs = FabricBatchProductionBeamDA.GetsByFabricBatchProduction(tc, oFabricBatchProduction.FBPID);
                    oFabricBatchProduction.FBPBs = oFBPBS.CreateObjects(readerFBPBs);
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
                oFabricBatchProduction = new FabricBatchProduction();
                oFabricBatchProduction.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchProduction;
        }

        public string Delete(FabricBatchProduction oFBP, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DBTableReferenceDA.HasReference(tc, "FabricBatchProduction", oFBP.FBPID);
                FabricBatchProductionDA.Delete(tc, oFBP, EnumDBOperation.Delete, nUserId);
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

        public FabricBatchProduction Get(int id, Int64 nUserId)
        {
            FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchProductionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProduction = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProduction = new FabricBatchProduction();
                oFabricBatchProduction.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchProduction;
        }

        public FabricBatchProduction GetByBatchAndWeavingType(int FBID,int nProcess,  Int64 nUserId)
         {
             FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
             TransactionContext tc = null;
             try
             {
                 tc = TransactionContext.Begin();
                 IDataReader reader = FabricBatchProductionDA.GetByBatchAndWeavingType(tc, FBID, nProcess);
                 NullHandler oReader = new NullHandler(reader);
                 if (reader.Read())
                 {
                     oFabricBatchProduction = CreateObject(oReader);
                 }
                 reader.Close();
                 tc.End();
             }
             catch (Exception e)
             {
                 #region Handle Exception
                 if (tc != null)
                     tc.HandleError();
                 oFabricBatchProduction = new FabricBatchProduction();
                 oFabricBatchProduction.ErrorMessage = e.Message.Split('!')[0];
                 #endregion
             }
             return oFabricBatchProduction;
         }
        public List<FabricBatchProduction> Gets(int nFBID, Int64 nUserID)
        {
            List<FabricBatchProduction> oFabricBatchProductions = new List<FabricBatchProduction>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchProductionDA.Gets(nFBID,tc);
                oFabricBatchProductions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
                oFabricBatchProduction.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchProductions.Add(oFabricBatchProduction);
                #endregion
            }
            return oFabricBatchProductions;
        }
        public List<FabricBatchProduction> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBatchProduction> oFabricBatchProductions = new List<FabricBatchProduction>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchProductionDA.Gets(tc, sSQL);
                oFabricBatchProductions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricBatchProduction oFabricBatchProduction = new FabricBatchProduction();
                oFabricBatchProduction.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchProductions.Add(oFabricBatchProduction);
                #endregion
            }
            return oFabricBatchProductions;
        }

        public List<FabricBatchProduction> ImportFabricBatchProduction(List<FabricBatchProduction> oFabricBatchProductions, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            NullHandler oReader;
            List<FabricBatchProduction> oFBPs = new List<FabricBatchProduction>();
            FabricBatchProduction oFBP = new FabricBatchProduction();

            foreach (FabricBatchProduction oItem in oFabricBatchProductions)
            {
                tc = TransactionContext.Begin(true);

                try
                {
                    DateTime dtRunOut = oItem.EndTime;
                    int nBeamID = oItem.BeamID;
                    oFBP = oItem;

                    var oFBPBMs = oItem.FabricBatchProductionBatchMans;
                    if (oItem.FBPID <= 0)
                    {
                        reader = FabricBatchProductionDA.ImportFabricBatchProductionDA(tc, oItem, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFBP = new FabricBatchProduction();
                            oFBP = CreateObject(oReader);
                            oFBP.BeamID = nBeamID;
                            oFBP.EndTime = dtRunOut;
                        }
                        reader.Close();

                    }

                    if (oFBP.FBPID > 0 && oFBPBMs.Any())
                    {
                        oFBPBMs.ForEach(x => { x.FBPID = oFBP.FBPID; });

                        //Insert FabricBatchProductionBatchMan
                        foreach (FabricBatchProductionBatchMan oFBPBM in oFBPBMs)
                        {
                            var oFBPBreakages = oFBPBM.FBPBreakages;
                            var otemp = new FabricBatchProductionBatchMan();
                            oFBPBM.Qty = Global.GetYard(oFBPBM.Qty, 2);
                            reader = FabricBatchProductionBatchManDA.IUD(tc, EnumDBOperation.Insert, oFBPBM, nUserID);
                            oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                otemp = new FabricBatchProductionBatchMan();
                                otemp = FabricBatchProductionBatchManService.CreateObject(oReader);
                            }
                            reader.Close();

                            //Insert FabricBatchProductionBreakage
                            if (otemp.FBPBID > 0 && oFBPBreakages.Any())
                            {
                                oFBPBreakages.ForEach(x => { x.FBLDetailID = otemp.FBPBID; });
                                foreach (FabricBatchProductionBreakage oFBPB in oFBPBreakages)
                                {
                                    reader = FabricBatchProductionBreakageDA.IUD(tc, EnumDBOperation.Insert, oFBPB, nUserID);
                                    reader.Close();
                                }
                            }
                        }
                    }


                    //Runout
                    if (oFBP.FBPID > 0 && oFBP.EndTime != DateTime.MinValue)
                    {
                        reader = FabricBatchProductionDA.ImportFabricBatchProductionDA(tc, oFBP, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFBP = new FabricBatchProduction();
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
                    oFBP.FBPID = 0; 
                    oFBP.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                    oFBPs.Add(oFBP);
                    #endregion
                }
                    
            }
            return oFBPs;
        }

    


        #endregion
    }
}
