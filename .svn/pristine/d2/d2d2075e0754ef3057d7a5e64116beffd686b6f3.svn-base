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
    public class ExportFundAllocationService : MarshalByRefObject, IExportFundAllocationService
    {
        private ExportFundAllocation MapObject(NullHandler oReader)
        {
            ExportFundAllocation oExportFundAllocation = new ExportFundAllocation();

            oExportFundAllocation.ExportFundAllocationID = oReader.GetInt32("ExportFundAllocationID");
            oExportFundAllocation.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportFundAllocation.ExportFundAllocationHeadID = oReader.GetInt32("ExportFundAllocationHeadID");
            oExportFundAllocation.Amount = oReader.GetDouble("Amount");
            oExportFundAllocation.AmountInPercent = oReader.GetDouble("AmountInPercent");
            oExportFundAllocation.Remarks = oReader.GetString("Remarks");
            oExportFundAllocation.ApprovedByName = oReader.GetString("ApprovedByName");
            oExportFundAllocation.ExportFundAllocationHeadName = oReader.GetString("ExportFundAllocationHeadName");
            oExportFundAllocation.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oExportFundAllocation.CurrencySymbol = oReader.GetString("CurrencySymbol");
            return oExportFundAllocation;
        }
        private ExportFundAllocation CreateObject(NullHandler oReader)
        {
            ExportFundAllocation oExportFundAllocation = new ExportFundAllocation();
            oExportFundAllocation = MapObject(oReader);
            return oExportFundAllocation;
        }

        private List<ExportFundAllocation> CreateObjects(IDataReader oReader)
        {
            List<ExportFundAllocation> oExportFundAllocation = new List<ExportFundAllocation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportFundAllocation oItem = CreateObject(oHandler);
                oExportFundAllocation.Add(oItem);
            }
            return oExportFundAllocation;
        }

        public ExportFundAllocation Save(ExportFundAllocation oExportFundAllocation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportFundAllocation.ExportFundAllocationID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ExportFundAllocationHead, EnumRoleOperationType.Add);
                    reader = ExportFundAllocationDA.InsertUpdate(tc, oExportFundAllocation, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ExportFundAllocationHead, EnumRoleOperationType.Edit);
                    reader = ExportFundAllocationDA.InsertUpdate(tc, oExportFundAllocation, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportFundAllocation = new ExportFundAllocation();
                    oExportFundAllocation = CreateObject(oReader);
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
                    oExportFundAllocation = new ExportFundAllocation();
                    oExportFundAllocation.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oExportFundAllocation;
        }

        public ExportFundAllocation Get(int id, Int64 nUserId)
        {
            ExportFundAllocation oExportFundAllocation = new ExportFundAllocation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportFundAllocationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportFundAllocation = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportFundAllocation", e);
                #endregion
            }

            return oExportFundAllocation;
        }
        public List<ExportFundAllocation> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportFundAllocation> oExportFundAllocation = new List<ExportFundAllocation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportFundAllocationDA.Gets(tc, sSQL);
                oExportFundAllocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportFundAllocation", e);
                #endregion
            }
            return oExportFundAllocation;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportFundAllocation oExportFundAllocation = new ExportFundAllocation();
                oExportFundAllocation.ExportFundAllocationID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ExportFundAllocationHead, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ExportFundAllocation", id);
                ExportFundAllocationDA.Delete(tc, oExportFundAllocation, EnumDBOperation.Delete, nUserId);
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

        public List<ExportFundAllocation> ApprovedExportFundAllocation(List<ExportFundAllocation> oExportFundAllocations, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                int nExportLCID =0;
                foreach (ExportFundAllocation oItem in oExportFundAllocations)
                {
                    nExportLCID = oItem.ExportLCID;
                    ExportFundAllocationDA.Approved(tc, oItem, nUserId);
                }
                reader = ExportFundAllocationDA.Gets(tc, "SELECT * FROM VIEW_ExportFundAllocation WHERE ExportLCID=" + nExportLCID + "");
                oExportFundAllocations = CreateObjects(reader);
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExportFundAllocation _oExportFundAllocation = new ExportFundAllocation();
                _oExportFundAllocation.ErrorMessage = e.Message;
                oExportFundAllocations.Add(_oExportFundAllocation);
                #endregion
            }
            return oExportFundAllocations;
        }
        public List<ExportFundAllocation> UndoApprovedExportFundAllocation(List<ExportFundAllocation> oExportFundAllocations, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                int nExportLCID = 0;
                foreach (ExportFundAllocation oItem in oExportFundAllocations)
                {
                    nExportLCID = oItem.ExportLCID;
                    ExportFundAllocationDA.UndoApproved(tc, oItem, nUserId);
                }
                reader = ExportFundAllocationDA.Gets(tc, "SELECT * FROM VIEW_ExportFundAllocation WHERE ExportLCID=" + nExportLCID + "");
                oExportFundAllocations = CreateObjects(reader);
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExportFundAllocation _oExportFundAllocation = new ExportFundAllocation();
                _oExportFundAllocation.ErrorMessage = e.Message;
                oExportFundAllocations.Add(_oExportFundAllocation);
                #endregion
            }
            return oExportFundAllocations;
        }
        public DataSet Gets(ExportFundAllocation oExportFundAllocation, Int64 nUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportFundAllocationDA.Gets(tc, oExportFundAllocation);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[2]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DataSet", e);
                #endregion
            }
            return oDataSet;
        }
    }
}
