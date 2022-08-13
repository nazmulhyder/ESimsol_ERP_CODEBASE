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
    public class BUWiseConsumptionUnitService : MarshalByRefObject, IBUWiseConsumptionUnitService
    {
        #region Private functions and declaration

        private BUWiseConsumptionUnit MapObject(NullHandler oReader)
        {
            BUWiseConsumptionUnit oBUWiseConsumptionUnit = new BUWiseConsumptionUnit();
            oBUWiseConsumptionUnit.BUWiseConsumptionUnitID = oReader.GetInt32("BUWiseConsumptionUnitID");
            oBUWiseConsumptionUnit.BUID = oReader.GetInt32("BUID");
            oBUWiseConsumptionUnit.ConsumptionUnitID = oReader.GetInt32("ConsumptionUnitID");
            oBUWiseConsumptionUnit.BUName = oReader.GetString("BUName");
            oBUWiseConsumptionUnit.ConsumptionUnitName = oReader.GetString("ConsumptionUnitName");
            oBUWiseConsumptionUnit.ConsumptionUnitNote = oReader.GetString("ConsumptionUnitNote");
            return oBUWiseConsumptionUnit;
        }

        private BUWiseConsumptionUnit CreateObject(NullHandler oReader)
        {
            BUWiseConsumptionUnit oBUWiseConsumptionUnit = new BUWiseConsumptionUnit();
            oBUWiseConsumptionUnit = MapObject(oReader);
            return oBUWiseConsumptionUnit;
        }

        private List<BUWiseConsumptionUnit> CreateObjects(IDataReader oReader)
        {
            List<BUWiseConsumptionUnit> oBUWiseConsumptionUnit = new List<BUWiseConsumptionUnit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BUWiseConsumptionUnit oItem = CreateObject(oHandler);
                oBUWiseConsumptionUnit.Add(oItem);
            }
            return oBUWiseConsumptionUnit;
        }

        #endregion

        #region Interface implementation
        public BUWiseConsumptionUnit Save(BUWiseConsumptionUnit oBUWiseConsumptionUnit, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBUWiseConsumptionUnit.BUWiseConsumptionUnitID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "BUWiseConsumptionUnit", EnumRoleOperationType.Add);
                    reader = BUWiseConsumptionUnitDA.InsertUpdate(tc, oBUWiseConsumptionUnit, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "BUWiseConsumptionUnit", EnumRoleOperationType.Edit);
                    reader = BUWiseConsumptionUnitDA.InsertUpdate(tc, oBUWiseConsumptionUnit, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBUWiseConsumptionUnit = new BUWiseConsumptionUnit();
                    oBUWiseConsumptionUnit = CreateObject(oReader);
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
                    oBUWiseConsumptionUnit = new BUWiseConsumptionUnit();
                    oBUWiseConsumptionUnit.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oBUWiseConsumptionUnit;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BUWiseConsumptionUnit oBUWiseConsumptionUnit = new BUWiseConsumptionUnit();
                oBUWiseConsumptionUnit.BUWiseConsumptionUnitID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "BUWiseConsumptionUnit", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "BUWiseConsumptionUnit", id);
                BUWiseConsumptionUnitDA.Delete(tc, oBUWiseConsumptionUnit, EnumDBOperation.Delete, nUserId);
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

        public BUWiseConsumptionUnit Get(int id, Int64 nUserId)
        {
            BUWiseConsumptionUnit oBUWiseConsumptionUnit = new BUWiseConsumptionUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BUWiseConsumptionUnitDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBUWiseConsumptionUnit = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BUWiseConsumptionUnit", e);
                #endregion
            }
            return oBUWiseConsumptionUnit;
        }

        public List<BUWiseConsumptionUnit> Gets(int nID, Int64 nUserID)
        {
            List<BUWiseConsumptionUnit> oBUWiseConsumptionUnits = new List<BUWiseConsumptionUnit>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BUWiseConsumptionUnitDA.Gets(tc, nID);
                oBUWiseConsumptionUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                BUWiseConsumptionUnit oBUWiseConsumptionUnit = new BUWiseConsumptionUnit();
                oBUWiseConsumptionUnit.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oBUWiseConsumptionUnits;
        }

        public List<BUWiseConsumptionUnit> Gets(string sSQL, Int64 nUserID)
        {
            List<BUWiseConsumptionUnit> oBUWiseConsumptionUnits = new List<BUWiseConsumptionUnit>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BUWiseConsumptionUnitDA.Gets(tc, sSQL);
                oBUWiseConsumptionUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BUWiseConsumptionUnit", e);
                #endregion
            }
            return oBUWiseConsumptionUnits;
        }

        #endregion
    }

}
