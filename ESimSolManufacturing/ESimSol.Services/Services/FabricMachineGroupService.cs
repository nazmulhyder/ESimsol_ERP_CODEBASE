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
    public class FabricMachineGroupService : MarshalByRefObject, IFabricMachineGroupService
    {
        #region Private functions and declaration
        private FabricMachineGroup MapObject(NullHandler oReader)
        {
            FabricMachineGroup oFabricMachineGroup = new FabricMachineGroup();
            oFabricMachineGroup.FabricMachineGroupID = oReader.GetInt32("FabricMachineGroupID");
            oFabricMachineGroup.Name = oReader.GetString("Name");
            oFabricMachineGroup.Note = oReader.GetString("Note");
            oFabricMachineGroup.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFabricMachineGroup.LastUpdateDateTime = Convert.ToDateTime(oReader.GetDateTime("LastUpdateDateTime"));
            oFabricMachineGroup.LastUpdateByName = oReader.GetString("LastUpdateByName");
            return oFabricMachineGroup;
        }
        private FabricMachineGroup CreateObject(NullHandler oReader)
        {
            FabricMachineGroup oFabricMachineGroup = new FabricMachineGroup();
            oFabricMachineGroup = MapObject(oReader);
            return oFabricMachineGroup;
        }

        private List<FabricMachineGroup> CreateObjects(IDataReader oReader)
        {
            List<FabricMachineGroup> oFabricMachineGroup = new List<FabricMachineGroup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricMachineGroup oItem = CreateObject(oHandler);
                oFabricMachineGroup.Add(oItem);
            }
            return oFabricMachineGroup;
        }
        #endregion

        #region Interface implementation
        public FabricMachineGroup Save(FabricMachineGroup oFabricMachineGroup, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricMachineGroup.FabricMachineGroupID <= 0)
                {

                    reader = FabricMachineGroupDA.InsertUpdate(tc, oFabricMachineGroup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricMachineGroupDA.InsertUpdate(tc, oFabricMachineGroup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricMachineGroup = new FabricMachineGroup();
                    oFabricMachineGroup = CreateObject(oReader);
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
                    oFabricMachineGroup = new FabricMachineGroup();
                    oFabricMachineGroup.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricMachineGroup;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricMachineGroup oFabricMachineGroup = new FabricMachineGroup();
                oFabricMachineGroup.FabricMachineGroupID = id;
                DBTableReferenceDA.HasReference(tc, "FabricMachineGroup", id);
                FabricMachineGroupDA.Delete(tc, oFabricMachineGroup, EnumDBOperation.Delete, nUserId);
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
        public FabricMachineGroup Get(int id, Int64 nUserId)
        {
            FabricMachineGroup oFabricMachineGroup = new FabricMachineGroup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricMachineGroupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricMachineGroup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricMachineGroup", e);
                #endregion
            }
            return oFabricMachineGroup;
        }
        public List<FabricMachineGroup> Gets(Int64 nUserID)
        {
            List<FabricMachineGroup> oFabricMachineGroups = new List<FabricMachineGroup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricMachineGroupDA.Gets(tc);
                oFabricMachineGroups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricMachineGroup oFabricMachineGroup = new FabricMachineGroup();
                oFabricMachineGroup.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricMachineGroups;
        }
        public List<FabricMachineGroup> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricMachineGroup> oFabricMachineGroups = new List<FabricMachineGroup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricMachineGroupDA.Gets(tc, sSQL);
                oFabricMachineGroups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricMachineGroup", e);
                #endregion
            }
            return oFabricMachineGroups;
        }

        #endregion
    }
}