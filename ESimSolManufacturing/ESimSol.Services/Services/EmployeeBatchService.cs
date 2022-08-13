using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class EmployeeBatchService : MarshalByRefObject, IEmployeeBatchService
    {
        private EmployeeBatch MapObject(NullHandler oReader)
        {
            EmployeeBatch oEmployeeBatch = new EmployeeBatch();
            oEmployeeBatch.EmployeeBatchID = oReader.GetInt32("EmployeeBatchID");
            oEmployeeBatch.BatchNo = oReader.GetString("BatchNo");
            oEmployeeBatch.BatchName = oReader.GetString("BatchName");
            oEmployeeBatch.CauseOfCreation = oReader.GetString("CauseOfCreation");
            oEmployeeBatch.CreateDate = oReader.GetDateTime("CreateDate");
            oEmployeeBatch.CreateBy = oReader.GetInt32("CreateBy");
            oEmployeeBatch.EmpCount = oReader.GetInt32("EmpCount");
            oEmployeeBatch.Remarks = oReader.GetString("Remarks");
            oEmployeeBatch.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oEmployeeBatch.ApprovedByName = oReader.GetString("ApprovedByName");
            oEmployeeBatch.CreateByName = oReader.GetString("CreateByName");


            return oEmployeeBatch;
        }
        private EmployeeBatch CreateObject(NullHandler oReader)
        {
            EmployeeBatch oEmployeeBatch = new EmployeeBatch();
            oEmployeeBatch = MapObject(oReader);
            return oEmployeeBatch;
        }

        private List<EmployeeBatch> CreateObjects(IDataReader oReader)
        {
            List<EmployeeBatch> oEmployeeBatch = new List<EmployeeBatch>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeBatch oItem = CreateObject(oHandler);
                oEmployeeBatch.Add(oItem);
            }
            return oEmployeeBatch;
        }

        public EmployeeBatch Save(EmployeeBatch oEmployeeBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sEmployeeBatchDetailIDs = "";
            List<EmployeeBatchDetail> oEmployeeBatchDetails = new List<EmployeeBatchDetail>();
            EmployeeBatchDetail oEmployeeBatchDetail = new EmployeeBatchDetail();
            oEmployeeBatchDetails = oEmployeeBatch.EmployeeBatchDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEmployeeBatch.EmployeeBatchID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.EmployeeBatch, EnumRoleOperationType.Add);
                    reader = EmployeeBatchDA.InsertUpdate(tc, oEmployeeBatch, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                     AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.EmployeeBatch, EnumRoleOperationType.Edit);
                    reader = EmployeeBatchDA.InsertUpdate(tc, oEmployeeBatch, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBatch = new EmployeeBatch();
                    oEmployeeBatch = CreateObject(oReader);
                }
                reader.Close();
                #region EmployeeBatch Detail
                foreach (EmployeeBatchDetail oItem in oEmployeeBatchDetails)
                {
                    IDataReader readerdetail;
                    oItem.EmployeeBatchID = oEmployeeBatch.EmployeeBatchID;
                    if (oItem.EmployeeBatchDetailID <= 0)
                    {
                        readerdetail = EmployeeBatchDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = EmployeeBatchDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sEmployeeBatchDetailIDs = sEmployeeBatchDetailIDs + oReaderDetail.GetString("EmployeeBatchDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sEmployeeBatchDetailIDs.Length > 0)
                {
                    sEmployeeBatchDetailIDs = sEmployeeBatchDetailIDs.Remove(sEmployeeBatchDetailIDs.Length - 1, 1);
                }
                oEmployeeBatchDetail = new EmployeeBatchDetail();
                oEmployeeBatchDetail.EmployeeBatchID = oEmployeeBatch.EmployeeBatchID;
                EmployeeBatchDetailDA.Delete(tc, oEmployeeBatchDetail, EnumDBOperation.Delete, nUserID, sEmployeeBatchDetailIDs);
                #endregion

                #region Get Employee Batch
                reader = EmployeeBatchDA.Get(tc, oEmployeeBatch.EmployeeBatchID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBatch = new EmployeeBatch();
                    oEmployeeBatch = CreateObject(oReader);
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
                    oEmployeeBatch = new EmployeeBatch();
                    oEmployeeBatch.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oEmployeeBatch;
        }

        public EmployeeBatch Get(int id, Int64 nUserId)
        {
            EmployeeBatch oEmployeeBatch = new EmployeeBatch();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeBatchDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBatch = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeBatch", e);
                #endregion
            }

            return oEmployeeBatch;
        }
        public List<EmployeeBatch> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeBatch> oEmployeeBatch = new List<EmployeeBatch>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeBatchDA.Gets(tc, sSQL);
                oEmployeeBatch = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Employee Batch", e);
                #endregion
            }
            return oEmployeeBatch;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeBatch oEmployeeBatch = new EmployeeBatch();
                oEmployeeBatch.EmployeeBatchID = id;
                 AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.EmployeeBatch, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "EmployeeBatch", id);
                EmployeeBatchDA.Delete(tc, oEmployeeBatch, EnumDBOperation.Delete, nUserId);
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

        public EmployeeBatch Approve(EmployeeBatch oEmployeeBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.EmployeeBatch, EnumRoleOperationType.Approved);
                EmployeeBatchDA.Approve(tc, oEmployeeBatch, nUserID);

                IDataReader reader;
                reader = EmployeeBatchDA.Get(tc, oEmployeeBatch.EmployeeBatchID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBatch = new EmployeeBatch();
                    oEmployeeBatch = CreateObject(oReader);
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
                    oEmployeeBatch = new EmployeeBatch();
                    oEmployeeBatch.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oEmployeeBatch;
        }
        public EmployeeBatch UndoApprove(EmployeeBatch oEmployeeBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.EmployeeBatch, EnumRoleOperationType.UnApproved);
                EmployeeBatchDA.UndoApprove(tc, oEmployeeBatch, nUserID);

                IDataReader reader;
                reader = EmployeeBatchDA.Get(tc, oEmployeeBatch.EmployeeBatchID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBatch = new EmployeeBatch();
                    oEmployeeBatch = CreateObject(oReader);
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
                    oEmployeeBatch = new EmployeeBatch();
                    oEmployeeBatch.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oEmployeeBatch;
        }
    }
}
