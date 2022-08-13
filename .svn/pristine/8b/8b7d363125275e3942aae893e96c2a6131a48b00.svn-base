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
    public class FabricRequisitionRollService : MarshalByRefObject, IFabricRequisitionRollService
    {
        #region Private functions and declaration

        private FabricRequisitionRoll MapObject(NullHandler oReader)
        {
            FabricRequisitionRoll oFabricRequisitionRoll = new FabricRequisitionRoll();
            oFabricRequisitionRoll.FabricRequisitionRollID = oReader.GetInt32("FabricRequisitionRollID");
            oFabricRequisitionRoll.FabricRequisitionDetailID = oReader.GetInt32("FabricRequisitionDetailID");
            oFabricRequisitionRoll.LotID = oReader.GetInt32("LotID");
            oFabricRequisitionRoll.DisburseBy = oReader.GetInt32("DisburseBy");
            oFabricRequisitionRoll.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oFabricRequisitionRoll.Qty = oReader.GetDouble("Qty");
            oFabricRequisitionRoll.ReceiveByName = oReader.GetString("ReceiveByName");
            oFabricRequisitionRoll.LotNo = oReader.GetString("LotNo");
            oFabricRequisitionRoll.FBQCDetailID = oReader.GetInt32("FBQCDetailID");
            oFabricRequisitionRoll.RollNo = oReader.GetDouble("RollNo");
            oFabricRequisitionRoll.DispoNo = oReader.GetString("DispoNo");
            oFabricRequisitionRoll.FabricBatchQCLotID = oReader.GetInt32("FabricBatchQCLotID");
            oFabricRequisitionRoll.YetQty = oReader.GetDouble("YetQty");
            return oFabricRequisitionRoll;
        }

        private FabricRequisitionRoll CreateObject(NullHandler oReader)
        {
            FabricRequisitionRoll oFabricRequisitionRoll = new FabricRequisitionRoll();
            oFabricRequisitionRoll = MapObject(oReader);
            return oFabricRequisitionRoll;
        }

        private List<FabricRequisitionRoll> CreateObjects(IDataReader oReader)
        {
            List<FabricRequisitionRoll> oFabricRequisitionRoll = new List<FabricRequisitionRoll>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricRequisitionRoll oItem = CreateObject(oHandler);
                oFabricRequisitionRoll.Add(oItem);
            }
            return oFabricRequisitionRoll;
        }

        #endregion

        #region Interface implementation
        public FabricRequisitionRoll Save(FabricRequisitionRoll oFabricRequisitionRoll, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricRequisitionRoll.FabricRequisitionRollID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricRequisitionRoll", EnumRoleOperationType.Add);
                    reader = FabricRequisitionRollDA.InsertUpdate(tc, oFabricRequisitionRoll, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricRequisitionRoll", EnumRoleOperationType.Edit);
                    reader = FabricRequisitionRollDA.InsertUpdate(tc, oFabricRequisitionRoll, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRequisitionRoll = new FabricRequisitionRoll();
                    oFabricRequisitionRoll = CreateObject(oReader);
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
                    oFabricRequisitionRoll = new FabricRequisitionRoll();
                    oFabricRequisitionRoll.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricRequisitionRoll;
        }

        public List<FabricRequisitionRoll> SaveFabricRequisitionRoll(List<FabricRequisitionRoll> oFabricRequisitionRolls, Int64 nUserID)
        {
            List<FabricRequisitionRoll> oFRRs = new List<FabricRequisitionRoll>();
            FabricRequisitionRoll oFRR = new FabricRequisitionRoll();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (FabricRequisitionRoll oItem in oFabricRequisitionRolls)
                {
                    if (oItem.FabricRequisitionRollID <= 0)
                    {
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricRequisitionRoll", EnumRoleOperationType.Add);
                        reader = FabricRequisitionRollDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricRequisitionRoll", EnumRoleOperationType.Edit);
                        reader = FabricRequisitionRollDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFRR = new FabricRequisitionRoll();
                        oFRR = CreateObject(oReader);
                        oFRRs.Add(oFRR);
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
                    oFRRs = new List<FabricRequisitionRoll>();
                    oFRR = new FabricRequisitionRoll();
                    oFRR.ErrorMessage = e.Message.Split('!')[0];
                    oFRRs.Add(oFRR);
                }
                #endregion
            }
            return oFRRs;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricRequisitionRoll oFabricRequisitionRoll = new FabricRequisitionRoll();
                oFabricRequisitionRoll.FabricRequisitionRollID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FabricRequisitionRoll", EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "FabricRequisitionRoll", id);
                FabricRequisitionRollDA.Delete(tc, oFabricRequisitionRoll, EnumDBOperation.Delete, nUserId);
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

        public FabricRequisitionRoll Get(int id, Int64 nUserId)
        {
            FabricRequisitionRoll oFabricRequisitionRoll = new FabricRequisitionRoll();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricRequisitionRollDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRequisitionRoll = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricRequisitionRoll", e);
                #endregion
            }
            return oFabricRequisitionRoll;
        }

        public FabricRequisitionRoll GetByDetailID(int id, Int64 nUserId)
        {
            FabricRequisitionRoll oFabricRequisitionRoll = new FabricRequisitionRoll();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricRequisitionRollDA.GetByDetailID(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRequisitionRoll = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricRequisitionRoll", e);
                #endregion
            }
            return oFabricRequisitionRoll;
        }

        public List<FabricRequisitionRoll> Gets(Int64 nUserID)
        {
            List<FabricRequisitionRoll> oFabricRequisitionRolls = new List<FabricRequisitionRoll>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricRequisitionRollDA.Gets(tc);
                oFabricRequisitionRolls = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricRequisitionRoll oFabricRequisitionRoll = new FabricRequisitionRoll();
                oFabricRequisitionRoll.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricRequisitionRolls;
        }

        public List<FabricRequisitionRoll> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricRequisitionRoll> oFabricRequisitionRolls = new List<FabricRequisitionRoll>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricRequisitionRollDA.Gets(tc, sSQL);
                oFabricRequisitionRolls = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricRequisitionRoll", e);
                #endregion
            }
            return oFabricRequisitionRolls;
        }

        #endregion
    }

}
