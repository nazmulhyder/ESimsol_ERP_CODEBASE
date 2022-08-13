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
    public class FabricWarpPlanService : MarshalByRefObject, IFabricWarpPlanService
    {
        #region Private functions and declaration

        private FabricWarpPlan MapObject(NullHandler oReader)
        {
            FabricWarpPlan oFabricWarpPlan = new FabricWarpPlan();
            oFabricWarpPlan.FabricWarpPlanID = oReader.GetInt32("FabricWarpPlanID");
            oFabricWarpPlan.FSCDID = oReader.GetInt32("FSCDID");
            oFabricWarpPlan.FEOSID = oReader.GetInt32("FEOSID");
            oFabricWarpPlan.FMID = oReader.GetInt32("FMID");
            oFabricWarpPlan.FinishDate = oReader.GetDateTime("FinishDate");
            oFabricWarpPlan.Sequence = oReader.GetInt32("Sequence");
            oFabricWarpPlan.Status = (EnumFabricBatchState)oReader.GetInt32("Status");
            oFabricWarpPlan.Note = oReader.GetString("Note");
            oFabricWarpPlan.StartTime = oReader.GetDateTime("StartTime");
            oFabricWarpPlan.EndTime = oReader.GetDateTime("EndTime");
            oFabricWarpPlan.ExeNo = oReader.GetString("ExeNo");
            oFabricWarpPlan.WMCode = oReader.GetString("WMCode");
            oFabricWarpPlan.Composition = oReader.GetString("Composition");
            oFabricWarpPlan.Construction = oReader.GetString("Construction");
            oFabricWarpPlan.ContractorName = oReader.GetString("BuyerName");
            oFabricWarpPlan.MachineName = oReader.GetString("MachineName");
            oFabricWarpPlan.Weave = oReader.GetString("Weave");
            oFabricWarpPlan.ReedNo = oReader.GetDouble("REED").ToString() + "/" + oReader.GetDouble("Dent").ToString();
            oFabricWarpPlan.REEDWidth = oReader.GetDouble("REEDWidth");
            oFabricWarpPlan.WarpColor = oReader.GetInt32("WarpColor");
            oFabricWarpPlan.WeftColor = oReader.GetInt32("WeftColor");
            oFabricWarpPlan.WarpLength = oReader.GetDouble("WarpLength");
            oFabricWarpPlan.WarpBeam = oReader.GetString("WarpBeam");
            oFabricWarpPlan.Qty = oReader.GetDouble("Qty");
            oFabricWarpPlan.RequiredWarpLength = oReader.GetDouble("RequiredWarpLength");
            oFabricWarpPlan.GreigeDemand = oReader.GetDouble("GreigeDemand");
            oFabricWarpPlan.TFLength = oReader.GetDouble("TFLength");
            oFabricWarpPlan.OrderQty = oReader.GetDouble("OrderQty");
            oFabricWarpPlan.TotalEnds = oReader.GetDouble("TotalEnds");
            oFabricWarpPlan.BatchNo = oReader.GetString("BatchNo");
            oFabricWarpPlan.PlanStatus = (EnumFabricPlanStatus)oReader.GetInt32("PlanStatus");
            //oFabricWarpPlan.BalanceQty = oReader.GetDouble("BalanceQty");
            oFabricWarpPlan.QtySizing = oReader.GetDouble("QtySizing");
            oFabricWarpPlan.Priority = oReader.GetInt32("Priority");
            oFabricWarpPlan.ExeQty = oReader.GetDouble("ExeQty");
            oFabricWarpPlan.FBID = oReader.GetInt32("FBID");
            oFabricWarpPlan.PlanType = (EnumPlanType)oReader.GetInt32("PlanType");
            oFabricWarpPlan.FSpcType = (EnumFabricSpeType)oReader.GetInt32("FSpcType");
            
            return oFabricWarpPlan;
        }

        private FabricWarpPlan CreateObject(NullHandler oReader)
        {
            FabricWarpPlan oFabricWarpPlan = new FabricWarpPlan();
            oFabricWarpPlan = MapObject(oReader);
            return oFabricWarpPlan;
        }

        private List<FabricWarpPlan> CreateObjects(IDataReader oReader)
        {
            List<FabricWarpPlan> oFabricWarpPlan = new List<FabricWarpPlan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricWarpPlan oItem = CreateObject(oHandler);
                oFabricWarpPlan.Add(oItem);
            }
            return oFabricWarpPlan;
        }

        #endregion

        #region Interface implementation
        public FabricWarpPlan Save(FabricWarpPlan oFabricWarpPlan, Int64 nUserID)
        {
            FabricWarpPlan oUG = new FabricWarpPlan();
            oUG = oFabricWarpPlan;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region FabricWarpPlan
                IDataReader reader;
                if (oFabricWarpPlan.FabricWarpPlanID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FabricWarpPlan, EnumRoleOperationType.Add);
                    reader = FabricWarpPlanDA.InsertUpdate(tc, oFabricWarpPlan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FabricWarpPlan, EnumRoleOperationType.Edit);
                    reader = FabricWarpPlanDA.InsertUpdate(tc, oFabricWarpPlan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricWarpPlan = new FabricWarpPlan();
                    oFabricWarpPlan = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region FabricWarpPlanDetail

               

                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oFabricWarpPlan = new FabricWarpPlan();
                    oFabricWarpPlan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricWarpPlan;
        }
        public List<FabricWarpPlan> Swap(List<FabricWarpPlan> oFabricWarpPlans, Int64 nUserID)
        {
            FabricWarpPlan oFabricWarpPlan = new FabricWarpPlan();
            List<FabricWarpPlan> oFabricWarpPlan_Return = new List<FabricWarpPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (FabricWarpPlan oItem in oFabricWarpPlans)
                {
                    IDataReader reader = FabricWarpPlanDA.Swap(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricWarpPlan = new FabricWarpPlan();
                        oFabricWarpPlan = CreateObject(oReader);
                        oFabricWarpPlan_Return.Add(oFabricWarpPlan);
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
                oFabricWarpPlan_Return = new List<FabricWarpPlan>();
                oFabricWarpPlan = new FabricWarpPlan();
                oFabricWarpPlan.ErrorMessage = e.Message.Split('~')[0];


                #endregion
            }
            return oFabricWarpPlan_Return;
        }

        public string Delete(FabricWarpPlan oFabricWarpPlan,Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.FabricWarpPlan, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricWarpPlan", oFabricWarpPlan.FabricWarpPlanID);
                FabricWarpPlanDA.Delete(tc, oFabricWarpPlan, EnumDBOperation.Delete, nUserId);
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

        public FabricWarpPlan Get(int id, Int64 nUserId)
        {
            FabricWarpPlan oFabricWarpPlan = new FabricWarpPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricWarpPlanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricWarpPlan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricWarpPlan", e);
                #endregion
            }
            return oFabricWarpPlan;
        }

        public List<FabricWarpPlan> Gets(Int64 nUserID)
        {
            List<FabricWarpPlan> oFabricWarpPlans = new List<FabricWarpPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricWarpPlanDA.Gets(tc);
                oFabricWarpPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricWarpPlan oFabricWarpPlan = new FabricWarpPlan();
                oFabricWarpPlan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricWarpPlans;
        }

        public List<FabricWarpPlan> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricWarpPlan> oFabricWarpPlans = new List<FabricWarpPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricWarpPlanDA.Gets(tc, sSQL);
                oFabricWarpPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricWarpPlan", e);
                #endregion
            }
            return oFabricWarpPlans;
        }

        public FabricWarpPlan UpdatePlanStatus(FabricWarpPlan oFabricWarpPlan,  Int64 nUserId)
        {
            TransactionContext tc = null;
            int nCount = 0;
            try
            {
                tc = TransactionContext.Begin();
                //nCount = FabricWarpPlanDA.GetsProStatus(tc, oFabricWarpPlan.FabricWarpPlanID, EnumWeavingProcess.Warping);
                //if (nCount>0)
                //{
                //    throw new Exception("Already Production Completed. You Can not Change");
                //}
                IDataReader reader = FabricWarpPlanDA.UpdatePlanStatus(tc, oFabricWarpPlan, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricWarpPlan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricWarpPlan = new FabricWarpPlan();
                oFabricWarpPlan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricWarpPlan;
        }

      

        #endregion
    }

}
