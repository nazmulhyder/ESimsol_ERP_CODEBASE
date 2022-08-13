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
    public class FabricLoomPlanService : MarshalByRefObject, IFabricLoomPlanService
    {
        #region Private functions and declaration

        private FabricLoomPlan MapObject(NullHandler oReader)
        {
            FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
            oFabricLoomPlan.FLPID = oReader.GetInt32("FLPID");
            oFabricLoomPlan.FSCDID = oReader.GetInt32("FSCDID");
            oFabricLoomPlan.FBID = oReader.GetInt32("FBID");
            oFabricLoomPlan.FMID = oReader.GetInt32("FMID");
            oFabricLoomPlan.FinishDate = oReader.GetDateTime("FinishDate");
            oFabricLoomPlan.Sequence = oReader.GetInt32("Sequence");
            oFabricLoomPlan.Status = (EnumFabricBatchState)oReader.GetInt32("Status");
            oFabricLoomPlan.Note = oReader.GetString("Note");
            oFabricLoomPlan.StartTime = oReader.GetDateTime("StartTime");
            oFabricLoomPlan.EndTime = oReader.GetDateTime("EndTime");
            oFabricLoomPlan.ExeNo = oReader.GetString("ExeNo");
            oFabricLoomPlan.WMCode = oReader.GetString("WMCode");
            oFabricLoomPlan.Composition = oReader.GetString("Composition");
            oFabricLoomPlan.Construction = oReader.GetString("Construction");
            oFabricLoomPlan.MachineCode = oReader.GetString("MachineCode");
            oFabricLoomPlan.ContractorName = oReader.GetString("BuyerName");
            oFabricLoomPlan.MachineName = oReader.GetString("MachineName");
            oFabricLoomPlan.Weave = oReader.GetString("Weave");
            oFabricLoomPlan.ReedNo = oReader.GetDouble("REED").ToString() + "/" + oReader.GetDouble("Dent").ToString();
            oFabricLoomPlan.REEDWidth = oReader.GetDouble("REEDWidth");
            oFabricLoomPlan.WarpColor = oReader.GetInt32("WarpColor");
            oFabricLoomPlan.WeftColor = oReader.GetInt32("WeftColor");
            oFabricLoomPlan.WarpLength = oReader.GetDouble("WarpLength");
            oFabricLoomPlan.Qty = oReader.GetDouble("Qty");
            oFabricLoomPlan.RequiredWarpLength = oReader.GetDouble("RequiredWarpLength");
            oFabricLoomPlan.GreigeDemand = oReader.GetDouble("GreigeDemand");
            oFabricLoomPlan.TFLength = oReader.GetDouble("TFLength");
            oFabricLoomPlan.OrderQty = oReader.GetDouble("OrderQty");
            oFabricLoomPlan.TotalEnds = oReader.GetDouble("TotalEnds");
            oFabricLoomPlan.BatchNo = oReader.GetString("BatchNo");
            oFabricLoomPlan.PlanStatus = (EnumFabricPlanStatus)oReader.GetInt32("PlanStatus");
            oFabricLoomPlan.FabricPrograme = (EnumFabricPrograme)oReader.GetInt32("FabricPrograme");
            oFabricLoomPlan.Priority = oReader.GetInt32("Priority");
            oFabricLoomPlan.BuyerID = oReader.GetInt32("BuyerID");
            oFabricLoomPlan.BuyerName = oReader.GetString("BuyerName");
            oFabricLoomPlan.FabricWeave = oReader.GetInt32("FabricWeave");
            oFabricLoomPlan.FinishType = oReader.GetString("FinishType");
            oFabricLoomPlan.HLReference = oReader.GetString("HLReference");
            oFabricLoomPlan.Dent = oReader.GetDouble("Dent");
            oFabricLoomPlan.REED = oReader.GetDouble("REED");
            oFabricLoomPlan.StyleNo = oReader.GetString("StyleNo");
            oFabricLoomPlan.ShiftID = oReader.GetInt32("ShiftID");
            oFabricLoomPlan.ExeQty = oReader.GetDouble("ExeQty");
            oFabricLoomPlan.PlanType = (EnumPlanType)oReader.GetInt32("PlanType");
            oFabricLoomPlan.FSpcType = (EnumFabricSpeType)oReader.GetInt32("FSpcType");

            return oFabricLoomPlan;
        }

        private FabricLoomPlan CreateObject(NullHandler oReader)
        {
            FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
            oFabricLoomPlan = MapObject(oReader);
            return oFabricLoomPlan;
        }

        private List<FabricLoomPlan> CreateObjects(IDataReader oReader)
        {
            List<FabricLoomPlan> oFabricLoomPlan = new List<FabricLoomPlan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricLoomPlan oItem = CreateObject(oHandler);
                oFabricLoomPlan.Add(oItem);
            }
            return oFabricLoomPlan;
        }

        #endregion

        #region Interface implementation
        public FabricLoomPlan Save(FabricLoomPlan oFabricLoomPlan, Int64 nUserID)
        {
            FabricLoomPlan oFLP = oFabricLoomPlan;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region FabricLoomPlan
                IDataReader reader;
                if (oFabricLoomPlan.FLPID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FabricLoomPlan, EnumRoleOperationType.Add);
                    reader = FabricLoomPlanDA.InsertUpdate(tc, oFabricLoomPlan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FabricLoomPlan, EnumRoleOperationType.Edit);
                    reader = FabricLoomPlanDA.InsertUpdate(tc, oFabricLoomPlan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricLoomPlan = new FabricLoomPlan();
                    oFabricLoomPlan = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Detail

                if (oFabricLoomPlan.FLPID > 0)
                {
                    string sFabricLoomPlanDetailIDs = "";
                    if (oFLP.FabricLoomPlanDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (FabricLoomPlanDetail oDRD in oFLP.FabricLoomPlanDetails)
                        {
                            oDRD.FLPID = oFabricLoomPlan.FLPID;
                            if (oDRD.FLPDID <= 0)
                            {
                                readerdetail = FabricLoomPlanDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = FabricLoomPlanDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sFabricLoomPlanDetailIDs = sFabricLoomPlanDetailIDs + oReaderDevRecapdetail.GetString("FLPDID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sFabricLoomPlanDetailIDs.Length > 0)
                    {
                        sFabricLoomPlanDetailIDs = sFabricLoomPlanDetailIDs.Remove(sFabricLoomPlanDetailIDs.Length - 1, 1);
                    }
                    FabricLoomPlanDetail oFabricLoomPlanDetail = new FabricLoomPlanDetail();
                    oFabricLoomPlanDetail.FLPID = oFabricLoomPlan.FLPID;
                    FabricLoomPlanDetailDA.Delete(tc, oFabricLoomPlanDetail, EnumDBOperation.Delete, nUserID, sFabricLoomPlanDetailIDs);
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
                    oFabricLoomPlan = new FabricLoomPlan();
                    oFabricLoomPlan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricLoomPlan;
        }
        public List<FabricLoomPlan> Swap(List<FabricLoomPlan> oFabricLoomPlans, Int64 nUserID)
        {
            FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
            List<FabricLoomPlan> oFabricLoomPlan_Return = new List<FabricLoomPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (FabricLoomPlan oItem in oFabricLoomPlans)
                {
                    IDataReader reader = FabricLoomPlanDA.Swap(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricLoomPlan = new FabricLoomPlan();
                        oFabricLoomPlan = CreateObject(oReader);
                        oFabricLoomPlan_Return.Add(oFabricLoomPlan);
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
                oFabricLoomPlan_Return = new List<FabricLoomPlan>();
                oFabricLoomPlan = new FabricLoomPlan();
                oFabricLoomPlan.ErrorMessage = e.Message.Split('~')[0];


                #endregion
            }
            return oFabricLoomPlan_Return;
        }

        public string Delete(FabricLoomPlan oFabricLoomPlan,Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.FabricLoomPlan, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricLoomPlan", oFabricLoomPlan.FLPID);
                FabricLoomPlanDA.Delete(tc, oFabricLoomPlan, EnumDBOperation.Delete, nUserId);
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

        public FabricLoomPlan Get(int id, Int64 nUserId)
        {
            FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricLoomPlanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricLoomPlan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricLoomPlan", e);
                #endregion
            }
            return oFabricLoomPlan;
        }

        public List<FabricLoomPlan> Gets(Int64 nUserID)
        {
            List<FabricLoomPlan> oFabricLoomPlans = new List<FabricLoomPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricLoomPlanDA.Gets(tc);
                oFabricLoomPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
                oFabricLoomPlan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricLoomPlans;
        }

        public List<FabricLoomPlan> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricLoomPlan> oFabricLoomPlans = new List<FabricLoomPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricLoomPlanDA.Gets(tc, sSQL);
                oFabricLoomPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricLoomPlan", e);
                #endregion
            }
            return oFabricLoomPlans;
        }

        public FabricLoomPlan UpdatePlanStatus(FabricLoomPlan oFabricLoomPlan, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricLoomPlanDA.UpdatePlanStatus(tc, oFabricLoomPlan);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricLoomPlan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricLoomPlan = new FabricLoomPlan();
                oFabricLoomPlan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricLoomPlan;
        }

        public List<FabricLoomPlan> SaveMultiplePlan(List<FabricLoomPlan> oFabricLoomPlans, Int64 nUserID)
        {
            FabricLoomPlan oFabricLoomPlan = new FabricLoomPlan();
            List<FabricLoomPlan> oFLPs = new List<FabricLoomPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (FabricLoomPlan oItem in oFabricLoomPlans)
                {

                    #region FabricLoomPlan
                    IDataReader reader;
                    if (oItem.FLPID <= 0)
                    {
                        reader = FabricLoomPlanDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FabricLoomPlanDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricLoomPlan = new FabricLoomPlan();
                        oFabricLoomPlan = CreateObject(oReader);
                        oFLPs.Add(oFabricLoomPlan);
                    }
                    reader.Close();
                    #endregion

                    #region Detail

                    if (oFabricLoomPlan.FLPID > 0)
                    {
                        if (oItem.FabricLoomPlanDetails.Count > 0)
                        {
                            IDataReader readerdetail;
                            foreach (FabricLoomPlanDetail oDRD in oItem.FabricLoomPlanDetails)
                            {
                                oDRD.FLPID = oFabricLoomPlan.FLPID;
                                if (oDRD.FLPDID <= 0)
                                {
                                    readerdetail = FabricLoomPlanDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                                }
                                else
                                {
                                    readerdetail = FabricLoomPlanDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");
                                }
                                //NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                                //if (readerdetail.Read())
                                //{
                                //    sFabricLoomPlanDetailIDs = sFabricLoomPlanDetailIDs + oReaderDevRecapdetail.GetString("FLPDID") + ",";
                                //}
                                readerdetail.Close();
                            }
                        }
                        //if (sFabricLoomPlanDetailIDs.Length > 0)
                        //{
                        //    sFabricLoomPlanDetailIDs = sFabricLoomPlanDetailIDs.Remove(sFabricLoomPlanDetailIDs.Length - 1, 1);
                        //}
                        //FabricLoomPlanDetail oFabricLoomPlanDetail = new FabricLoomPlanDetail();
                        //oFabricLoomPlanDetail.FLPID = oFabricLoomPlan.FLPID;
                        //FabricLoomPlanDetailDA.Delete(tc, oFabricLoomPlanDetail, EnumDBOperation.Delete, nUserID, sFabricLoomPlanDetailIDs);
                    }

                    #endregion

                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFLPs = new List<FabricLoomPlan>();
                oFabricLoomPlan = new FabricLoomPlan();
                oFabricLoomPlan.ErrorMessage = e.Message.Split('~')[0];
                oFLPs.Add(oFabricLoomPlan);
                #endregion
            }
            return oFLPs;
        }

        #endregion
    }

}
