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
	public class FNProductionPlanService : MarshalByRefObject, IFNProductionPlanService
	{
        #region Private functions and declaration

        private FNProductionPlan MapObject(NullHandler oReader)
        {
            FNProductionPlan oFNProductionPlan = new FNProductionPlan();
            oFNProductionPlan.FNPPID = oReader.GetInt32("FNPPID");
            oFNProductionPlan.PlanNo = oReader.GetString("PlanNo");
            oFNProductionPlan.FSCDID = oReader.GetInt32("FSCDID");
            oFNProductionPlan.FNMachineID = oReader.GetInt32("FNMachineID");
            oFNProductionPlan.Qty = oReader.GetDouble("Qty");
            oFNProductionPlan.StartTime = oReader.GetDateTime("StartTime");
            oFNProductionPlan.EndTime = oReader.GetDateTime("EndTime");
            oFNProductionPlan.FabricNo = oReader.GetString("FabricNo");
            oFNProductionPlan.ColorName = oReader.GetString("ColorName");
            oFNProductionPlan.FNMachineNameCode = oReader.GetString("FNMachineNameCode");
            oFNProductionPlan.FNExONo = oReader.GetString("FNExONo");
            oFNProductionPlan.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");
            oFNProductionPlan.IsInHouse = oReader.GetBoolean("IsInHouse");
            //oFNProductionPlan.FEOID = oReader.GetInt32("FEOID");
            oFNProductionPlan.ReviseCount = oReader.GetInt32("ReviseCount");
            oFNProductionPlan.FNExOID = oReader.GetInt32("FNExOID");
            oFNProductionPlan.FinishWidth = oReader.GetString("FinishWidth");
            oFNProductionPlan.BuyerName = oReader.GetString("BuyerName");
            oFNProductionPlan.BuyerID = oReader.GetInt32("BuyerID");
            oFNProductionPlan.Construction = oReader.GetString("Construction");
            oFNProductionPlan.GSM = oReader.GetDouble("GSM");
            oFNProductionPlan.ReqFinishedGSM = oReader.GetDouble("ReqFinishedGSM");
            oFNProductionPlan.YetToBatchQty = oReader.GetDouble("YetToBatchQty");
            oFNProductionPlan.FNEODQty = oReader.GetDouble("FNEODQty");
            oFNProductionPlan.GreyQty = oReader.GetDouble("GreyQty");
            oFNProductionPlan.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFNProductionPlan.FinishTypeName = oReader.GetString("FinishTypeName");
            oFNProductionPlan.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oFNProductionPlan.DateWiseSequence = oReader.GetInt32("DateWiseSequence");
            oFNProductionPlan.FNTreatment = oReader.GetInt32("FNTreatment");
            oFNProductionPlan.BUID = oReader.GetInt32("BUID");
            oFNProductionPlan.EnumFNTreatment = (EnumFNTreatment)oReader.GetInt16("FNTreatment");
            oFNProductionPlan.PlanStatus = (EnumFabricPlanStatus)oReader.GetInt16("PlanStatus");
            oFNProductionPlan.Note = oReader.GetString("Note");
            oFNProductionPlan.ExeNo = oReader.GetString("ExeNo");
            oFNProductionPlan.ContractorID = oReader.GetInt32("ContractorID");
            oFNProductionPlan.ContractorName = oReader.GetString("ContractorName");
            oFNProductionPlan.SCDate = oReader.GetDateTime("SCDate");
            oFNProductionPlan.FabricDesignName = oReader.GetString("FabricDesignName");

            return oFNProductionPlan;
        }

        private FNProductionPlan CreateObject(NullHandler oReader)
        {
            FNProductionPlan oFNProductionPlan = new FNProductionPlan();
            oFNProductionPlan = MapObject(oReader);
            return oFNProductionPlan;
        }

        private List<FNProductionPlan> CreateObjects(IDataReader oReader)
        {
            List<FNProductionPlan> oFNProductionPlan = new List<FNProductionPlan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNProductionPlan oItem = CreateObject(oHandler);
                oFNProductionPlan.Add(oItem);
            }
            return oFNProductionPlan;
        }

        #endregion

        #region Interface implementation
        public FNProductionPlan Save(FNProductionPlan oFNProductionPlan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFNProductionPlan.FNPPID <= 0)
                {

                    reader = FNProductionPlanDA.InsertUpdate(tc, oFNProductionPlan, EnumDBOperation.Insert, nUserID);
                }
                else
                {

                    reader = FNProductionPlanDA.InsertUpdate(tc, oFNProductionPlan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNProductionPlan = new FNProductionPlan();
                    oFNProductionPlan = CreateObject(oReader);
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
                    oFNProductionPlan = new FNProductionPlan();
                    oFNProductionPlan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFNProductionPlan;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNProductionPlan oFNProductionPlan = new FNProductionPlan();
                oFNProductionPlan.FNPPID = id;
                DBTableReferenceDA.HasReference(tc, "FNProductionPlan", id);
                FNProductionPlanDA.Delete(tc, oFNProductionPlan, EnumDBOperation.Delete, nUserId);
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
        public FNProductionPlan Get(int id, Int64 nUserId)
        {
            FNProductionPlan oFNProductionPlan = new FNProductionPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNProductionPlanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNProductionPlan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNProductionPlan", e);
                #endregion
            }
            return oFNProductionPlan;
        }
        public List<FNProductionPlan> Gets(Int64 nUserID)
        {
            List<FNProductionPlan> oFNProductionPlans = new List<FNProductionPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNProductionPlanDA.Gets(tc);
                oFNProductionPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FNProductionPlan oFNProductionPlan = new FNProductionPlan();
                oFNProductionPlan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFNProductionPlans;
        }
        public List<FNProductionPlan> Gets(string sSQL, Int64 nUserID)
        {
            List<FNProductionPlan> oFNProductionPlans = new List<FNProductionPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNProductionPlanDA.Gets(tc, sSQL);
                oFNProductionPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNProductionPlan", e);
                #endregion
            }
            return oFNProductionPlans;
        }

        #endregion
	}

}
