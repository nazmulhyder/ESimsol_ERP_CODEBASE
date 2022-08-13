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
    public class FnOrderExecuteService : MarshalByRefObject, IFnOrderExecuteService
    {
        #region Private functions and declaration

        private FnOrderExecute MapObject(NullHandler oReader)
        {
            FnOrderExecute oFnOrderExecute = new FnOrderExecute();
            oFnOrderExecute.FnOrderExecuteID = oReader.GetInt32("FnOrderExecuteID");
            oFnOrderExecute.FSCDID = oReader.GetInt32("FSCDID");
            oFnOrderExecute.FNLabdipDetailID = oReader.GetInt32("FNLabdipDetailID");
            oFnOrderExecute.ShadeID = oReader.GetInt32("ShadeID");
            oFnOrderExecute.IssueDate = oReader.GetDateTime("IssueDate");
            oFnOrderExecute.FinishWidth = oReader.GetDouble("FinishWidth");
            oFnOrderExecute.NoOfFrame = oReader.GetDouble("NoOfFrame");
            oFnOrderExecute.DyeType = oReader.GetString("DyeType");
            oFnOrderExecute.Qty = oReader.GetDouble("Qty");
            oFnOrderExecute.Percentage = oReader.GetDouble("Percentage");
            oFnOrderExecute.GreigeWidth = oReader.GetDouble("GreigeWidth");
            oFnOrderExecute.GL = oReader.GetDouble("GL");
            oFnOrderExecute.Note = oReader.GetString("Note");
            oFnOrderExecute.LDNo = oReader.GetString("LDNo");
            return oFnOrderExecute;
        }

        private FnOrderExecute CreateObject(NullHandler oReader)
        {
            FnOrderExecute oFnOrderExecute = new FnOrderExecute();
            oFnOrderExecute = MapObject(oReader);
            return oFnOrderExecute;
        }

        private List<FnOrderExecute> CreateObjects(IDataReader oReader)
        {
            List<FnOrderExecute> oFnOrderExecute = new List<FnOrderExecute>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FnOrderExecute oItem = CreateObject(oHandler);
                oFnOrderExecute.Add(oItem);
            }
            return oFnOrderExecute;
        }

        #endregion

        #region Interface implementation
        public FnOrderExecute Save(FnOrderExecute oFnOrderExecute, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFnOrderExecute.FnOrderExecuteID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FnOrderExecute", EnumRoleOperationType.Add);
                    reader = FnOrderExecuteDA.InsertUpdate(tc, oFnOrderExecute, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FnOrderExecute", EnumRoleOperationType.Edit);
                    reader = FnOrderExecuteDA.InsertUpdate(tc, oFnOrderExecute, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFnOrderExecute = new FnOrderExecute();
                    oFnOrderExecute = CreateObject(oReader);
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
                    oFnOrderExecute = new FnOrderExecute();
                    oFnOrderExecute.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFnOrderExecute;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FnOrderExecute oFnOrderExecute = new FnOrderExecute();
                oFnOrderExecute.FnOrderExecuteID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FnOrderExecute", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FnOrderExecute", id);
                FnOrderExecuteDA.Delete(tc, oFnOrderExecute, EnumDBOperation.Delete, nUserId);
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

        public FnOrderExecute Get(int id, Int64 nUserId)
        {
            FnOrderExecute oFnOrderExecute = new FnOrderExecute();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FnOrderExecuteDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFnOrderExecute = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FnOrderExecute", e);
                #endregion
            }
            return oFnOrderExecute;
        }

        public List<FnOrderExecute> Gets(Int64 nUserID)
        {
            List<FnOrderExecute> oFnOrderExecutes = new List<FnOrderExecute>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FnOrderExecuteDA.Gets(tc);
                oFnOrderExecutes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FnOrderExecute oFnOrderExecute = new FnOrderExecute();
                oFnOrderExecute.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFnOrderExecutes;
        }

        public List<FnOrderExecute> Gets(string sSQL, Int64 nUserID)
        {
            List<FnOrderExecute> oFnOrderExecutes = new List<FnOrderExecute>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FnOrderExecuteDA.Gets(tc, sSQL);
                oFnOrderExecutes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FnOrderExecute", e);
                #endregion
            }
            return oFnOrderExecutes;
        }

        #endregion
    }

}
