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
    public class ExportFundAllocationHeadService : MarshalByRefObject, IExportFundAllocationHeadService
    {
        private ExportFundAllocationHead MapObject(NullHandler oReader)
        {
            ExportFundAllocationHead oExportFundAllocationHead = new ExportFundAllocationHead();

            oExportFundAllocationHead.ExportFundAllocationHeadID = oReader.GetInt32("ExportFundAllocationHeadID");
            oExportFundAllocationHead.Code = oReader.GetString("Code");
            oExportFundAllocationHead.Name = oReader.GetString("Name");
            oExportFundAllocationHead.Sequence = oReader.GetInt32("Sequence");
            oExportFundAllocationHead.Remarks = oReader.GetString("Remarks");
            return oExportFundAllocationHead;
        }
        private ExportFundAllocationHead CreateObject(NullHandler oReader)
        {
            ExportFundAllocationHead oExportFundAllocationHead = new ExportFundAllocationHead();
            oExportFundAllocationHead = MapObject(oReader);
            return oExportFundAllocationHead;
        }

        private List<ExportFundAllocationHead> CreateObjects(IDataReader oReader)
        {
            List<ExportFundAllocationHead> oExportFundAllocationHead = new List<ExportFundAllocationHead>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportFundAllocationHead oItem = CreateObject(oHandler);
                oExportFundAllocationHead.Add(oItem);
            }
            return oExportFundAllocationHead;
        }

        public ExportFundAllocationHead Save(ExportFundAllocationHead oExportFundAllocationHead, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportFundAllocationHead.ExportFundAllocationHeadID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ExportFundAllocationHead, EnumRoleOperationType.Add);
                    reader = ExportFundAllocationHeadDA.InsertUpdate(tc, oExportFundAllocationHead, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ExportFundAllocationHead, EnumRoleOperationType.Edit);
                    reader = ExportFundAllocationHeadDA.InsertUpdate(tc, oExportFundAllocationHead, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportFundAllocationHead = new ExportFundAllocationHead();
                    oExportFundAllocationHead = CreateObject(oReader);
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
                    oExportFundAllocationHead = new ExportFundAllocationHead();
                    oExportFundAllocationHead.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oExportFundAllocationHead;
        }

        public ExportFundAllocationHead Get(int id, Int64 nUserId)
        {
            ExportFundAllocationHead oExportFundAllocationHead = new ExportFundAllocationHead();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportFundAllocationHeadDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportFundAllocationHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportFundAllocationHead", e);
                #endregion
            }

            return oExportFundAllocationHead;
        }
        public List<ExportFundAllocationHead> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportFundAllocationHead> oExportFundAllocationHead = new List<ExportFundAllocationHead>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportFundAllocationHeadDA.Gets(tc, sSQL);
                oExportFundAllocationHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportFundAllocationHead", e);
                #endregion
            }
            return oExportFundAllocationHead;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportFundAllocationHead oExportFundAllocationHead = new ExportFundAllocationHead();
                oExportFundAllocationHead.ExportFundAllocationHeadID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ExportFundAllocationHead, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ExportFundAllocationHead", id);
                ExportFundAllocationHeadDA.Delete(tc, oExportFundAllocationHead, EnumDBOperation.Delete, nUserId);
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
        public List<ExportFundAllocationHead> RefreshSequence(List<ExportFundAllocationHead> oExportFundAllocationHead, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                foreach (ExportFundAllocationHead oItem in oExportFundAllocationHead)
                {
                    ExportFundAllocationHeadDA.UpdateSequence(tc, oItem);
                }
                reader = ExportFundAllocationHeadDA.Gets(tc, "SELECT * FROM VIEW_ExportFundAllocationHead ORDER BY Sequence");
                oExportFundAllocationHead = CreateObjects(reader);
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExportFundAllocationHead _oExportFundAllocationHead = new ExportFundAllocationHead();
                _oExportFundAllocationHead.ErrorMessage = e.Message;
                oExportFundAllocationHead.Add(_oExportFundAllocationHead);
                #endregion
            }
            return oExportFundAllocationHead;
        }
    }
}
