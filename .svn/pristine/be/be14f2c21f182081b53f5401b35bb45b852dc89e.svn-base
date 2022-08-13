using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class DeliveryPlanService : MarshalByRefObject, IDeliveryPlanService
    {
        #region Private functions and declaration
        private DeliveryPlan MapObject(NullHandler oReader)
        {
            DeliveryPlan oDeliveryPlan = new DeliveryPlan();
            oDeliveryPlan.DeliveryPlanID = oReader.GetInt32("DeliveryPlanID");
            oDeliveryPlan.DeliveryPlanDate = oReader.GetDateTime("DeliveryPlanDate");
            oDeliveryPlan.DeliveryOrderID = oReader.GetInt32("DeliveryOrderID");
            oDeliveryPlan.PlanNo = oReader.GetString("PlanNo");
            oDeliveryPlan.DOChallanStatus = oReader.GetString("DOChallanStatus");
            oDeliveryPlan.Sequence = oReader.GetInt32("Sequence");
            oDeliveryPlan.Remarks = oReader.GetString("Remarks");
            oDeliveryPlan.BUID = oReader.GetInt32("BUID");
            oDeliveryPlan.ProductNature = (EnumProductNature)oReader.GetInt32("ProductNature");
            oDeliveryPlan.ProductNatureInInt = oReader.GetInt32("ProductNature");
            oDeliveryPlan.DONo = oReader.GetString("DONo");
            oDeliveryPlan.RefNo = oReader.GetString("RefNo");
            oDeliveryPlan.BuyerName = oReader.GetString("BuyerName");
            oDeliveryPlan.DeliveryToAddress = oReader.GetString("DeliveryToAddress");
            oDeliveryPlan.CustomerName = oReader.GetString("CustomerName");
            oDeliveryPlan.ContractorID = oReader.GetInt32("ContractorID");
            oDeliveryPlan.BuyerID = oReader.GetInt32("BuyerID");
            oDeliveryPlan.DeliveryToName = oReader.GetString("DeliveryToName");
            return oDeliveryPlan;
        }

        private DeliveryPlan CreateObject(NullHandler oReader)
        {
            DeliveryPlan oDeliveryPlan = new DeliveryPlan();
            oDeliveryPlan = MapObject(oReader);
            return oDeliveryPlan;
        }

        private List<DeliveryPlan> CreateObjects(IDataReader oReader)
        {
            List<DeliveryPlan> oDeliveryPlan = new List<DeliveryPlan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DeliveryPlan oItem = CreateObject(oHandler);
                oDeliveryPlan.Add(oItem);
            }
            return oDeliveryPlan;
        }

        #endregion

        #region Interface implementation
        public DeliveryPlanService() { }

        public List<DeliveryPlan> Save(DeliveryPlan oDeliveryPlan, Int64 nUserId)
        {
            TransactionContext tc = null;
            List<DeliveryPlan> oDeliveryPlans = new List<DeliveryPlan>();
            oDeliveryPlans = oDeliveryPlan.DeliveryPlans;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (DeliveryPlan oItem in oDeliveryPlans)
                {
                    reader = null;
                    if (oItem.DeliveryPlanID <= 0)
                    {
                        reader = DeliveryPlanDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        reader = DeliveryPlanDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                    }
                    reader.Close();
                }
                reader = null;
                string sSQL = "SELECT * FROM View_DeliveryPlan as DP WHERE DP.BUID = " + oDeliveryPlan.BUID + " AND DP.ProductNature = " + oDeliveryPlan.ProductNatureInInt + " AND DP.YetToDeliveryChallanQty>0 Order By Sequence";
                reader = DeliveryPlanDA.Gets(tc, sSQL);
                oDeliveryPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save DeliveryPlan. Because of " + e.Message, e);
                #endregion
            }
            return oDeliveryPlans;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                
                DeliveryPlan oDeliveryPlan = new DeliveryPlan();
                oDeliveryPlan.DeliveryPlanID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.DeliveryPlan, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "DeliveryPlan", id);
                DeliveryPlanDA.Delete(tc, oDeliveryPlan, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                return e.Message.Split('!')[0];
                #endregion
            }
            return "deleted";
        }

        public DeliveryPlan Get(int id, Int64 nUserId)
        {
            DeliveryPlan oAccountHead = new DeliveryPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DeliveryPlanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DeliveryPlan", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DeliveryPlan> Gets(int BUID, int nProductNature, DateTime dPlanDate, int nContractorID, Int64 nUserId)
        {
            List<DeliveryPlan> oDeliveryPlans = new List<DeliveryPlan>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DeliveryPlanDA.Gets(BUID, nProductNature, dPlanDate,nContractorID, tc);
                oDeliveryPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DeliveryPlan", e);
                #endregion
            }

            return oDeliveryPlans;
        }
     
        
        public List<DeliveryPlan> Gets(string sSQL, Int64 nUserId)
        {
            List<DeliveryPlan> oDeliveryPlan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DeliveryPlanDA.Gets(tc, sSQL);
                oDeliveryPlan = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DeliveryPlan", e);
                #endregion
            }

            return oDeliveryPlan;
        }
        
    
       
        #endregion
    } 
}