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
    public class FabricSizingPlanService : MarshalByRefObject, IFabricSizingPlanService
    {
        #region Private functions and declaration

        private FabricSizingPlan MapObject(NullHandler oReader)
        {
            FabricSizingPlan oFabricSizingPlan = new FabricSizingPlan();
            oFabricSizingPlan.FabricSizingPlanID = oReader.GetInt32("FabricSizingPlanID");
            oFabricSizingPlan.FSCDID = oReader.GetInt32("FSCDID");
            oFabricSizingPlan.FEOSID = oReader.GetInt32("FEOSID");
            oFabricSizingPlan.FMID = oReader.GetInt32("FMID");
            oFabricSizingPlan.FabricWarpPlanID = oReader.GetInt32("FabricWarpPlanID");
            oFabricSizingPlan.NoOfColor = oReader.GetInt32("NoOfColor");
            oFabricSizingPlan.FinishDate = oReader.GetDateTime("FinishDate");
            oFabricSizingPlan.Sequence = oReader.GetInt32("Sequence");
            oFabricSizingPlan.Status = (EnumFabricBatchState)oReader.GetInt32("Status");
            oFabricSizingPlan.Note = oReader.GetString("Note");
            oFabricSizingPlan.StartTime = oReader.GetDateTime("StartTime");
            oFabricSizingPlan.EndTime = oReader.GetDateTime("EndTime");
            oFabricSizingPlan.ExeNo = oReader.GetString("ExeNo");
            oFabricSizingPlan.Composition = oReader.GetString("Composition");
            oFabricSizingPlan.Construction = oReader.GetString("Construction");
            oFabricSizingPlan.ContractorName = oReader.GetString("BuyerName");
            oFabricSizingPlan.MachineName = oReader.GetString("MachineName");
            oFabricSizingPlan.Weave = oReader.GetString("Weave");
            oFabricSizingPlan.ReedNo = oReader.GetDouble("REED").ToString() + "/" + oReader.GetDouble("Dent").ToString();
            oFabricSizingPlan.REEDWidth = oReader.GetDouble("REEDWidth");
            oFabricSizingPlan.WarpColor = oReader.GetInt32("WarpColor");
            oFabricSizingPlan.WeftColor = oReader.GetInt32("WeftColor");
            oFabricSizingPlan.WarpLength = oReader.GetDouble("WarpLength");
            oFabricSizingPlan.WarpBeam = oReader.GetString("WarpBeam");
            oFabricSizingPlan.RequiredWarpLength = oReader.GetDouble("RequiredWarpLength");
            oFabricSizingPlan.Qty = oReader.GetDouble("Qty");
            oFabricSizingPlan.PlanStatus = (EnumFabricPlanStatus)oReader.GetInt32("PlanStatus");
            oFabricSizingPlan.BatchNo = oReader.GetString("BatchNo");
            oFabricSizingPlan.LFID = oReader.GetInt32("LFID");
            oFabricSizingPlan.FBID = oReader.GetInt32("FBID");
            oFabricSizingPlan.Priority = oReader.GetInt32("Priority");
            oFabricSizingPlan.WaterQty = oReader.GetDouble("WaterQty");
            oFabricSizingPlan.ExeQty = oReader.GetDouble("ExeQty");
            oFabricSizingPlan.PlanType = (EnumPlanType)oReader.GetInt32("PlanType");
            oFabricSizingPlan.FSpcType = (EnumFabricSpeType)oReader.GetInt32("FSpcType");

            return oFabricSizingPlan;
        }

        private FabricSizingPlan CreateObject(NullHandler oReader)
        {
            FabricSizingPlan oFabricSizingPlan = new FabricSizingPlan();
            oFabricSizingPlan = MapObject(oReader);
            return oFabricSizingPlan;
        }

        private List<FabricSizingPlan> CreateObjects(IDataReader oReader)
        {
            List<FabricSizingPlan> oFabricSizingPlan = new List<FabricSizingPlan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSizingPlan oItem = CreateObject(oHandler);
                oFabricSizingPlan.Add(oItem);
            }
            return oFabricSizingPlan;
        }

        #endregion

        #region Interface implementation
        public FabricSizingPlan Save(FabricSizingPlan oFabricSizingPlan, Int64 nUserID)
        {
            FabricSizingPlanDetail oFabricSizingPlanDetail = new FabricSizingPlanDetail();
            FabricSizingPlan oUG = new FabricSizingPlan();
            oUG = oFabricSizingPlan;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region FabricSizingPlan
                IDataReader reader;
                if (oFabricSizingPlan.FabricSizingPlanID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FabricSizingPlan, EnumRoleOperationType.Add);
                    reader = FabricSizingPlanDA.InsertUpdate(tc, oFabricSizingPlan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FabricSizingPlan, EnumRoleOperationType.Edit);
                    reader = FabricSizingPlanDA.InsertUpdate(tc, oFabricSizingPlan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSizingPlan = new FabricSizingPlan();
                    oFabricSizingPlan = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region FabricSizingPlanDetail

                if (oFabricSizingPlan.FabricSizingPlanID > 0)
                {
                    string sFabricSizingPlanDetailIDs = "";
                    if (oUG.FabricSizingPlanDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (FabricSizingPlanDetail oDRD in oUG.FabricSizingPlanDetails)
                        {
                            oDRD.FabricSizingPlanID = oFabricSizingPlan.FabricSizingPlanID;
                            if (oDRD.FSPDID <= 0)
                            {
                                readerdetail = FabricSizingPlanDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = FabricSizingPlanDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nFabricSizingPlanDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nFabricSizingPlanDetailID = oReaderDevRecapdetail.GetInt32("FSPDID");
                                sFabricSizingPlanDetailIDs = sFabricSizingPlanDetailIDs + oReaderDevRecapdetail.GetString("FSPDID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    //if (sFabricSizingPlanDetailIDs.Length > 0)
                    //{
                    //    sFabricSizingPlanDetailIDs = sFabricSizingPlanDetailIDs.Remove(sFabricSizingPlanDetailIDs.Length - 1, 1);
                    //}
                    //oFabricSizingPlanDetail = new FabricSizingPlanDetail();
                    //oFabricSizingPlanDetail.FabricSizingPlanID = oFabricSizingPlan.FabricSizingPlanID;
                    //FabricSizingPlanDetailDA.Delete(tc, oFabricSizingPlanDetail, EnumDBOperation.Delete, nUserID, sFabricSizingPlanDetailIDs);
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
                    oFabricSizingPlan = new FabricSizingPlan();
                    oFabricSizingPlan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricSizingPlan;
        }

        public string Delete(FabricSizingPlan oFabricSizingPlan,Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.FabricSizingPlan, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricSizingPlan", oFabricSizingPlan.FabricSizingPlanID);
                FabricSizingPlanDA.Delete(tc, oFabricSizingPlan, EnumDBOperation.Delete, nUserId);
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

        public FabricSizingPlan Get(int id, Int64 nUserId)
        {
            FabricSizingPlan oFabricSizingPlan = new FabricSizingPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricSizingPlanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSizingPlan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricSizingPlan", e);
                #endregion
            }
            return oFabricSizingPlan;
        }

        public List<FabricSizingPlan> Gets(Int64 nUserID)
        {
            List<FabricSizingPlan> oFabricSizingPlans = new List<FabricSizingPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricSizingPlanDA.Gets(tc);
                oFabricSizingPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricSizingPlan oFabricSizingPlan = new FabricSizingPlan();
                oFabricSizingPlan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricSizingPlans;
        }

        public List<FabricSizingPlan> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricSizingPlan> oFabricSizingPlans = new List<FabricSizingPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricSizingPlanDA.Gets(tc, sSQL);
                oFabricSizingPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricSizingPlan", e);
                #endregion
            }
            return oFabricSizingPlans;
        }

        public FabricSizingPlan UpdatePlanStatus(FabricSizingPlan oFabricSizingPlan, Int64 nUserId)
        {
            TransactionContext tc = null;
            int nCount = 0;
            try
            {
                tc = TransactionContext.Begin();

                //nCount = FabricSizingPlanDA.GetsProStatus(tc, oFabricSizingPlan.FabricSizingPlanID, EnumWeavingProcess.Sizing);
                //if (nCount > 0)
                //{
                //    throw new Exception("Already Production Completed. You Can not Change");
                //}

                IDataReader reader = FabricSizingPlanDA.UpdatePlanStatus(tc, oFabricSizingPlan, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSizingPlan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricSizingPlan = new FabricSizingPlan();
                oFabricSizingPlan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSizingPlan;
        }

        public FabricSizingPlan UpdateWaterQty(FabricSizingPlan oFabricSizingPlan, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricSizingPlanDA.UpdateWaterQty(tc, oFabricSizingPlan);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSizingPlan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricSizingPlan = new FabricSizingPlan();
                oFabricSizingPlan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSizingPlan;
        }
        public List<FabricSizingPlan> Swap(List<FabricSizingPlan> oFabricSizingPlans, Int64 nUserID)
        {
            FabricSizingPlan oFabricSizingPlan = new FabricSizingPlan();
            List<FabricSizingPlan> oFabricSizingPlan_Return = new List<FabricSizingPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (FabricSizingPlan oItem in oFabricSizingPlans)
                {
                    IDataReader reader = FabricSizingPlanDA.Swap(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricSizingPlan = new FabricSizingPlan();
                        oFabricSizingPlan = CreateObject(oReader);
                        oFabricSizingPlan_Return.Add(oFabricSizingPlan);
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
                oFabricSizingPlan_Return = new List<FabricSizingPlan>();
                oFabricSizingPlan = new FabricSizingPlan();
                oFabricSizingPlan.ErrorMessage = e.Message.Split('~')[0];
               

                #endregion
            }
            return oFabricSizingPlan_Return;
        }

        #endregion
    }

}
