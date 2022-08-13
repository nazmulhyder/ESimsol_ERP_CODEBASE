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
    public class FabricChemicalPlanService : MarshalByRefObject, IFabricChemicalPlanService
    {
        #region Private functions and declaration

        private FabricChemicalPlan MapObject(NullHandler oReader)
        {
            FabricChemicalPlan oFabricChemicalPlan = new FabricChemicalPlan();
            oFabricChemicalPlan.FabricChemicalPlanID = oReader.GetInt32("FabricChemicalPlanID");
            oFabricChemicalPlan.FabricSizingPlanID = oReader.GetInt32("FabricSizingPlanID");
            oFabricChemicalPlan.FBID = oReader.GetInt32("FBID");
            oFabricChemicalPlan.ProductID = oReader.GetInt32("ProductID");
            oFabricChemicalPlan.Qty = oReader.GetDouble("Qty");
            oFabricChemicalPlan.BatchNo = oReader.GetString("BatchNo");
            oFabricChemicalPlan.ProductName = oReader.GetString("ProductName");
            oFabricChemicalPlan.ProductCode = oReader.GetString("ProductCode");
            oFabricChemicalPlan.MUnitID = oReader.GetInt32("MUnitID");
            oFabricChemicalPlan.MUnitName = oReader.GetString("MUnitName");
            oFabricChemicalPlan.SizingPlanNo = oReader.GetString("SizingPlanNo");

            return oFabricChemicalPlan;
        }

        private FabricChemicalPlan CreateObject(NullHandler oReader)
        {
            FabricChemicalPlan oFabricChemicalPlan = new FabricChemicalPlan();
            oFabricChemicalPlan = MapObject(oReader);
            return oFabricChemicalPlan;
        }

        private List<FabricChemicalPlan> CreateObjects(IDataReader oReader)
        {
            List<FabricChemicalPlan> oFabricChemicalPlan = new List<FabricChemicalPlan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricChemicalPlan oItem = CreateObject(oHandler);
                oFabricChemicalPlan.Add(oItem);
            }
            return oFabricChemicalPlan;
        }

        #endregion

        #region Interface implementation
        public FabricChemicalPlan Save(FabricChemicalPlan oFabricChemicalPlan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricChemicalPlan.FabricChemicalPlanID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricChemicalPlan", EnumRoleOperationType.Add);
                    reader = FabricChemicalPlanDA.InsertUpdate(tc, oFabricChemicalPlan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricChemicalPlan", EnumRoleOperationType.Edit);
                    reader = FabricChemicalPlanDA.InsertUpdate(tc, oFabricChemicalPlan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricChemicalPlan = new FabricChemicalPlan();
                    oFabricChemicalPlan = CreateObject(oReader);
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
                    oFabricChemicalPlan = new FabricChemicalPlan();
                    oFabricChemicalPlan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricChemicalPlan;
        }

        public List<FabricChemicalPlan> SaveMultiple(List<FabricChemicalPlan> oFabricChemicalPlans, Int64 nUserID)
        {
            FabricChemicalPlan oFabricChemicalPlan = new FabricChemicalPlan();
            List<FabricChemicalPlan> oFCPs = new List<FabricChemicalPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (FabricChemicalPlan oItem in oFabricChemicalPlans)
                {
                    if (oItem.FabricChemicalPlanID <= 0)
                    {
                        reader = FabricChemicalPlanDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FabricChemicalPlanDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricChemicalPlan = new FabricChemicalPlan();
                        oFabricChemicalPlan = CreateObject(oReader);
                        oFCPs.Add(oFabricChemicalPlan);
                    }
                    reader.Close();
                }
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oFabricChemicalPlan = new FabricChemicalPlan();
                    oFabricChemicalPlan.ErrorMessage = e.Message.Split('!')[0];
                    oFCPs = new List<FabricChemicalPlan>();
                    oFCPs.Add(oFabricChemicalPlan);
                }
                #endregion
            }
            return oFCPs;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricChemicalPlan oFabricChemicalPlan = new FabricChemicalPlan();
                oFabricChemicalPlan.FabricChemicalPlanID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FabricChemicalPlan", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricChemicalPlan", id);
                FabricChemicalPlanDA.Delete(tc, oFabricChemicalPlan, EnumDBOperation.Delete, nUserId);
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

        public FabricChemicalPlan Get(int id, Int64 nUserId)
        {
            FabricChemicalPlan oFabricChemicalPlan = new FabricChemicalPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricChemicalPlanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricChemicalPlan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricChemicalPlan", e);
                #endregion
            }
            return oFabricChemicalPlan;
        }

        public List<FabricChemicalPlan> Gets(Int64 nUserID)
        {
            List<FabricChemicalPlan> oFabricChemicalPlans = new List<FabricChemicalPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricChemicalPlanDA.Gets(tc);
                oFabricChemicalPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricChemicalPlan oFabricChemicalPlan = new FabricChemicalPlan();
                oFabricChemicalPlan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricChemicalPlans;
        }

        public List<FabricChemicalPlan> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricChemicalPlan> oFabricChemicalPlans = new List<FabricChemicalPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricChemicalPlanDA.Gets(tc, sSQL);
                oFabricChemicalPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricChemicalPlan", e);
                #endregion
            }
            return oFabricChemicalPlans;
        }

        #endregion
    }

}
