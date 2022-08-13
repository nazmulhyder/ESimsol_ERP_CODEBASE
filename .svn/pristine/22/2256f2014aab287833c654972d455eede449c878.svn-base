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
    public class MaterialTransactionRuleService : MarshalByRefObject, IMaterialTransactionRuleService
    {
        #region Private functions and declaration
        private MaterialTransactionRule MapObject(NullHandler oReader)
        {
            MaterialTransactionRule oMaterialTransactionRule = new MaterialTransactionRule();
            oMaterialTransactionRule.MaterialTransactionRuleID = oReader.GetInt32("MaterialTransactionRuleID");
            oMaterialTransactionRule.LocationID = oReader.GetInt32("LocationID");
            oMaterialTransactionRule.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oMaterialTransactionRule.TriggerParentType = (EnumTriggerParentsType)oReader.GetInt16("TriggerParentType");
            oMaterialTransactionRule.InOutType = (EnumInOutType)oReader.GetInt16("InOutType");
            oMaterialTransactionRule.Direction = oReader.GetBoolean("Direction");
            oMaterialTransactionRule.ProductType = (EnumProductType)oReader.GetInt16("ProductType");
            oMaterialTransactionRule.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oMaterialTransactionRule.Note = oReader.GetString("Note");
            oMaterialTransactionRule.IsActive = oReader.GetBoolean("IsActive");
            oMaterialTransactionRule.OperationUnitName = oReader.GetString("OperationUnitName");
            oMaterialTransactionRule.LocationName = oReader.GetString("LocationName");
            oMaterialTransactionRule.ProductCategoryName = oReader.GetString("ProductCategoryName");


            return oMaterialTransactionRule;
        }

        private MaterialTransactionRule CreateObject(NullHandler oReader)
        {
            MaterialTransactionRule oMaterialTransactionRule = new MaterialTransactionRule();
            oMaterialTransactionRule = MapObject(oReader);
            return oMaterialTransactionRule;
        }

        private List<MaterialTransactionRule> CreateObjects(IDataReader oReader)
        {
            List<MaterialTransactionRule> oMaterialTransactionRule = new List<MaterialTransactionRule>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MaterialTransactionRule oItem = CreateObject(oHandler);
                oMaterialTransactionRule.Add(oItem);
            }
            return oMaterialTransactionRule;
        }

        #endregion

        #region Private functions and declaration For Bi Directed Rule
        private MaterialTransactionRule MapObjectForBiDirectedRule(NullHandler oReader)
        {
            MaterialTransactionRule oMaterialTransactionRule = new MaterialTransactionRule();
            oMaterialTransactionRule.MaterialTransactionRuleID = oReader.GetInt32("MaterialTransactionRuleID");
            oMaterialTransactionRule.LocationID = oReader.GetInt32("LocationID");
            oMaterialTransactionRule.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oMaterialTransactionRule.TriggerParentType = (EnumTriggerParentsType)oReader.GetInt16("TriggerParentType");
            oMaterialTransactionRule.InOutType = (EnumInOutType)oReader.GetInt16("InOutType");
            oMaterialTransactionRule.Direction = oReader.GetBoolean("Direction");
            oMaterialTransactionRule.ProductType = (EnumProductType)oReader.GetInt16("ProductType");
            oMaterialTransactionRule.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oMaterialTransactionRule.Note = oReader.GetString("Note");
            oMaterialTransactionRule.IsActive = oReader.GetBoolean("IsActive");
            oMaterialTransactionRule.OperationUnitName = oReader.GetString("OperationUnitName");
            oMaterialTransactionRule.LocationName = oReader.GetString("LocationName");
            oMaterialTransactionRule.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oMaterialTransactionRule.MAP = oReader.GetString("MAP");
            

            return oMaterialTransactionRule;
        }

        private MaterialTransactionRule CreateObjectForBiDirectedRule(NullHandler oReader)
        {
            MaterialTransactionRule oMaterialTransactionRule = new MaterialTransactionRule();
            oMaterialTransactionRule = MapObjectForBiDirectedRule(oReader);
            return oMaterialTransactionRule;
        }

        private List<MaterialTransactionRule> CreateObjectsForBiDirectedRule(IDataReader oReader)
        {
            List<MaterialTransactionRule> oMaterialTransactionRule = new List<MaterialTransactionRule>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MaterialTransactionRule oItem = CreateObjectForBiDirectedRule(oHandler);
                oMaterialTransactionRule.Add(oItem);
            }
            return oMaterialTransactionRule;
        }

        #endregion

        #region Interface implementation
        public MaterialTransactionRuleService() { }

        public MaterialTransactionRule Save(MaterialTransactionRule oMaterialTransactionRule, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMaterialTransactionRule.MaterialTransactionRuleID <= 0)
                {
                    reader = MaterialTransactionRuleDA.Insert(tc, oMaterialTransactionRule, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = MaterialTransactionRuleDA.Update(tc, oMaterialTransactionRule, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMaterialTransactionRule = new MaterialTransactionRule();
                    oMaterialTransactionRule = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Save Document Book. Because of " + e.Message, e);
                oMaterialTransactionRule.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oMaterialTransactionRule;
        }

        public string SaveMAP(MaterialTransactionRule oMaterialTransactionRule, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MaterialTransactionRuleDA.InsertMAP(tc, oMaterialTransactionRule, EnumDBOperation.Delete, nUserId);
                tc.End();
                return "Data Save Successfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Document Book. Because of " + e.Message, e);
                return e.Message.Split('!')[0];
                #endregion
            }
            
        }

        public string Delete(MaterialTransactionRule oMaterialTransactionRule, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);



                MaterialTransactionRuleDA.Delete(tc, oMaterialTransactionRule, EnumDBOperation.Delete, nUserId);
                tc.End();
                return "Delete sucessfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oMaterialTransactionRule.ErrorMessage = e.Message;
                return e.Message.Split('!')[0];
                #endregion
            }

        }

        public MaterialTransactionRule Get(int id, Int64 nUserId)
        {
            MaterialTransactionRule oMaterialTransactionRule = new MaterialTransactionRule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MaterialTransactionRuleDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMaterialTransactionRule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Material Transaction Rule ", e);
                #endregion
            }

            return oMaterialTransactionRule;
        }

        public MaterialTransactionRule Get(int eTPT, int eInOutType, bool bDirection, int eProductType, int nWorkingUnitID, int nOEvalue, string sDbObjectName, Int64 nUserID)
        {
            MaterialTransactionRule oMaterialTransactionRule = new MaterialTransactionRule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MaterialTransactionRuleDA.Get(tc,eTPT,eInOutType,bDirection,eProductType,nWorkingUnitID,nUserID,nOEvalue,sDbObjectName);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMaterialTransactionRule = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oMaterialTransactionRule;
        }

        public List<MaterialTransactionRule> Gets(Int64 nUserId)
        {
            List<MaterialTransactionRule> oMaterialTransactionRule = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaterialTransactionRuleDA.Gets(tc);
                oMaterialTransactionRule = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Material Transaction Rule ", e);
                #endregion
            }

            return oMaterialTransactionRule;
        }

        public List<MaterialTransactionRule> GetsForBiDirectionalRule(string sSQL, Int64 nUserId)
        {
            List<MaterialTransactionRule> oMaterialTransactionRule = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaterialTransactionRuleDA.GetsForBiDirectionalRule(tc, sSQL);
                oMaterialTransactionRule = CreateObjectsForBiDirectedRule(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Material Transaction Rule ", e);
                #endregion
            }

            return oMaterialTransactionRule;
        }
        public List<MaterialTransactionRule> Gets(string sSQL, Int64 nUserId)
        {
            List<MaterialTransactionRule> oMaterialTransactionRule = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaterialTransactionRuleDA.Gets(tc, sSQL);
                oMaterialTransactionRule = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Material Transaction Rule ", e);
                #endregion
            }

            return oMaterialTransactionRule;
        }
        #endregion
    }
}