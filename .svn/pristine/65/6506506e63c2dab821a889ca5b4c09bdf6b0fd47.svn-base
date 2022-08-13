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
    public class FnQCTestGroupService : MarshalByRefObject, IFnQCTestGroupService
    {
        #region Private functions and declaration

        private FnQCTestGroup MapObject(NullHandler oReader)
        {
            FnQCTestGroup oFnQCTestGroup = new FnQCTestGroup();
            oFnQCTestGroup.FnQCTestGroupID = oReader.GetInt32("FnQCTestGroupID");
            oFnQCTestGroup.Name = oReader.GetString("Name");
            oFnQCTestGroup.Note = oReader.GetString("Note");
            oFnQCTestGroup.SLNo = oReader.GetInt32("SLNo");
            
            return oFnQCTestGroup;
        }

        private FnQCTestGroup CreateObject(NullHandler oReader)
        {
            FnQCTestGroup oFnQCTestGroup = new FnQCTestGroup();
            oFnQCTestGroup = MapObject(oReader);
            return oFnQCTestGroup;
        }

        private List<FnQCTestGroup> CreateObjects(IDataReader oReader)
        {
            List<FnQCTestGroup> oFnQCTestGroup = new List<FnQCTestGroup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FnQCTestGroup oItem = CreateObject(oHandler);
                oFnQCTestGroup.Add(oItem);
            }
            return oFnQCTestGroup;
        }

        #endregion

        #region Interface implementation
        public FnQCTestGroup Save(FnQCTestGroup oFnQCTestGroup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region FnQCTestGroup
                IDataReader reader;
                if (oFnQCTestGroup.FnQCTestGroupID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FnQCTestGroup, EnumRoleOperationType.Add);
                    reader = FnQCTestGroupDA.InsertUpdate(tc, oFnQCTestGroup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.FnQCTestGroup, EnumRoleOperationType.Edit);
                    reader = FnQCTestGroupDA.InsertUpdate(tc, oFnQCTestGroup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFnQCTestGroup = new FnQCTestGroup();
                    oFnQCTestGroup = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oFnQCTestGroup = new FnQCTestGroup();
                    oFnQCTestGroup.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFnQCTestGroup;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FnQCTestGroup oFnQCTestGroup = new FnQCTestGroup();
                oFnQCTestGroup.FnQCTestGroupID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.FnQCTestGroup, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FnQCTestGroup", id);
                FnQCTestGroupDA.Delete(tc, oFnQCTestGroup, EnumDBOperation.Delete, nUserId);
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

        public FnQCTestGroup Get(int id, Int64 nUserId)
        {
            FnQCTestGroup oFnQCTestGroup = new FnQCTestGroup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FnQCTestGroupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFnQCTestGroup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FnQCTestGroup", e);
                #endregion
            }
            return oFnQCTestGroup;
        }

        public List<FnQCTestGroup> Gets(Int64 nUserID)
        {
            List<FnQCTestGroup> oFnQCTestGroups = new List<FnQCTestGroup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FnQCTestGroupDA.Gets(tc);
                oFnQCTestGroups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FnQCTestGroup oFnQCTestGroup = new FnQCTestGroup();
                oFnQCTestGroup.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFnQCTestGroups;
        }

        public List<FnQCTestGroup> Gets(string sSQL, Int64 nUserID)
        {
            List<FnQCTestGroup> oFnQCTestGroups = new List<FnQCTestGroup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FnQCTestGroupDA.Gets(tc, sSQL);
                oFnQCTestGroups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FnQCTestGroup", e);
                #endregion
            }
            return oFnQCTestGroups;
        }

        #endregion
    }

}
