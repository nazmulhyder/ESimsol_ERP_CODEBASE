using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ExportPartyInfoDetailService : MarshalByRefObject, IExportPartyInfoDetailService
    {
        #region Private functions and declaration
        private ExportPartyInfoDetail MapObject(NullHandler oReader)
        {
            ExportPartyInfoDetail oExportPartyInfoDetail = new ExportPartyInfoDetail();
            oExportPartyInfoDetail.ExportPartyInfoDetailID = oReader.GetInt32("ExportPartyInfoDetailID");
            oExportPartyInfoDetail.ExportPartyInfoID = oReader.GetInt32("ExportPartyInfoID");
            oExportPartyInfoDetail.ContractorID = oReader.GetInt32("ContractorID");
            oExportPartyInfoDetail.BankBranchID = oReader.GetInt32("BankBranchID");
            oExportPartyInfoDetail.RefNo = oReader.GetString("RefNo");
            oExportPartyInfoDetail.RefDate = oReader.GetString("RefDate");
            oExportPartyInfoDetail.Activity = oReader.GetBoolean("Activity");
            oExportPartyInfoDetail.IsBank = oReader.GetBoolean("IsBank");
            oExportPartyInfoDetail.InfoCaption = oReader.GetString("InfoCaption");
            oExportPartyInfoDetail.PartyName = oReader.GetString("PartyName");
            return oExportPartyInfoDetail;
        }

        private ExportPartyInfoDetail CreateObject(NullHandler oReader)
        {
            ExportPartyInfoDetail oExportPartyInfoDetail = new ExportPartyInfoDetail();
            oExportPartyInfoDetail = MapObject(oReader);
            return oExportPartyInfoDetail;
        }

        private List<ExportPartyInfoDetail> CreateObjects(IDataReader oReader)
        {
            List<ExportPartyInfoDetail> oExportPartyInfoDetail = new List<ExportPartyInfoDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPartyInfoDetail oItem = CreateObject(oHandler);
                oExportPartyInfoDetail.Add(oItem);
            }
            return oExportPartyInfoDetail;
        }

        #endregion

        #region Interface implementation
        public ExportPartyInfoDetailService() { }

        public ExportPartyInfoDetail Save(ExportPartyInfoDetail oExportPartyInfoDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportPartyInfoDetail.ExportPartyInfoDetailID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ExportPartyInfo, EnumRoleOperationType.Add);
                    reader = ExportPartyInfoDetailDA.InsertUpdate(tc, oExportPartyInfoDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ExportPartyInfo, EnumRoleOperationType.Edit);
                    reader = ExportPartyInfoDetailDA.InsertUpdate(tc, oExportPartyInfoDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPartyInfoDetail = new ExportPartyInfoDetail();
                    oExportPartyInfoDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPartyInfoDetail = new ExportPartyInfoDetail();
                oExportPartyInfoDetail.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ExportPartyInfoDetail. Because of " + e.Message, e);
                #endregion
            }
            return oExportPartyInfoDetail;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportPartyInfoDetail oExportPartyInfoDetail = new ExportPartyInfoDetail();
                oExportPartyInfoDetail.ExportPartyInfoDetailID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ExportPartyInfo, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ExportPartyInfoDetail", id);
                ExportPartyInfoDetailDA.Delete(tc, oExportPartyInfoDetail, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ExportPartyInfoDetail. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ExportPartyInfoDetail Get(int id, int nUserId)
        {
            ExportPartyInfoDetail oAccountHead = new ExportPartyInfoDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPartyInfoDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportPartyInfoDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ExportPartyInfoDetail> Gets(int nUserID)
        {
            List<ExportPartyInfoDetail> oExportPartyInfoDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPartyInfoDetailDA.Gets(tc);
                oExportPartyInfoDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPartyInfoDetail", e);
                #endregion
            }

            return oExportPartyInfoDetail;
        }
        public List<ExportPartyInfoDetail> Gets(string sSQL, int nUserID)
        {
            List<ExportPartyInfoDetail> oExportPartyInfoDetail = new List<ExportPartyInfoDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_ExportPartyInfoDetail";
                }
                reader = ExportPartyInfoDetailDA.Gets(tc, sSQL);
                oExportPartyInfoDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPartyInfoDetail", e);
                #endregion
            }

            return oExportPartyInfoDetail;
        }
        public List<ExportPartyInfoDetail> GetsByParty(int nPartyID, int nUserID)
        {
            List<ExportPartyInfoDetail> oExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPartyInfoDetailDA.GetsByParty(tc, nPartyID);
                oExportPartyInfoDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
                ExportPartyInfoDetail oExportPartyInfoDetail = new ExportPartyInfoDetail();
                oExportPartyInfoDetail.ErrorMessage = e.Message;
                oExportPartyInfoDetails.Add(oExportPartyInfoDetail);
                #endregion
            }
            return oExportPartyInfoDetails;
        }
        public List<ExportPartyInfoDetail> GetsBy(int nPartyID, int nBankBranchID, int nUserID)
        {
            List<ExportPartyInfoDetail> oExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPartyInfoDetailDA.GetsBy(tc,  nPartyID,  nBankBranchID);
                oExportPartyInfoDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
                ExportPartyInfoDetail oExportPartyInfoDetail = new ExportPartyInfoDetail();
                oExportPartyInfoDetail.ErrorMessage = e.Message;
                oExportPartyInfoDetails.Add(oExportPartyInfoDetail);
                #endregion
            }
            return oExportPartyInfoDetails;
        }
        public List<ExportPartyInfoDetail> Gets(int nPartyID, int nBankBranchID, string sIDs, int nUserID)
        {
            List<ExportPartyInfoDetail> oExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPartyInfoDetailDA.Gets(tc, nPartyID, nBankBranchID,  sIDs);
                oExportPartyInfoDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
                ExportPartyInfoDetail oExportPartyInfoDetail = new ExportPartyInfoDetail();
                oExportPartyInfoDetail.ErrorMessage = e.Message;
                oExportPartyInfoDetails.Add(oExportPartyInfoDetail);
                #endregion
            }
            return oExportPartyInfoDetails;
        }
        public List<ExportPartyInfoDetail> GetsByPartyAndBill(int nPartyID, int nExportLCID, int nRefType,  int nUserID)
        {
            List<ExportPartyInfoDetail> oExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPartyInfoDetailDA.GetsByPartyAndBill(tc, nPartyID, nExportLCID, nRefType);
                oExportPartyInfoDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPartyInfoDetails = new List<ExportPartyInfoDetail>();
                ExportPartyInfoDetail oExportPartyInfoDetail = new ExportPartyInfoDetail();
                oExportPartyInfoDetail.ErrorMessage = e.Message;
                oExportPartyInfoDetails.Add(oExportPartyInfoDetail);
                #endregion
            }
            return oExportPartyInfoDetails;
        }
        #endregion
    }
}
