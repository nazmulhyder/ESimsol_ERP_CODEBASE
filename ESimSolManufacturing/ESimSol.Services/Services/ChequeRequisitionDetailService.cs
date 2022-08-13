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
    public class ChequeRequisitionDetailService : MarshalByRefObject, IChequeRequisitionDetailService
    {

        #region Private functions and declaration
        private ChequeRequisitionDetail MapObject(NullHandler oReader)
        {
            ChequeRequisitionDetail oChequeRequisitionDetail = new ChequeRequisitionDetail();
            oChequeRequisitionDetail.ChequeRequisitionDetailID = oReader.GetInt32("ChequeRequisitionDetailID");
            oChequeRequisitionDetail.ChequeRequisitionID = oReader.GetInt32("ChequeRequisitionID");
            oChequeRequisitionDetail.VoucherBillID = oReader.GetInt32("VoucherBillID");
            oChequeRequisitionDetail.Amount = oReader.GetDouble("Amount");
            oChequeRequisitionDetail.Remarks = oReader.GetString("Remarks");
            oChequeRequisitionDetail.BillNo = oReader.GetString("BillNo");
            oChequeRequisitionDetail.BillDate = oReader.GetDateTime("BillDate");
            oChequeRequisitionDetail.AccountHeadName = oReader.GetString("AccountHeadName");
            oChequeRequisitionDetail.BillAmount = oReader.GetDouble("BillAmount");
            oChequeRequisitionDetail.RemainningBalance = oReader.GetDouble("RemainningBalance");
            return oChequeRequisitionDetail;
        }

        private ChequeRequisitionDetail CreateObject(NullHandler oReader)
        {
            ChequeRequisitionDetail oChequeRequisitionDetail = new ChequeRequisitionDetail();
            oChequeRequisitionDetail = MapObject(oReader);
            return oChequeRequisitionDetail;
        }

        private List<ChequeRequisitionDetail> CreateObjects(IDataReader oReader)
        {
            List<ChequeRequisitionDetail> oChequeRequisitionDetails = new List<ChequeRequisitionDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ChequeRequisitionDetail oItem = CreateObject(oHandler);
                oChequeRequisitionDetails.Add(oItem);
            }
            return oChequeRequisitionDetails;
        }
        #endregion

        #region Interface implementation
        public ChequeRequisitionDetailService() { }

        public ChequeRequisitionDetail Save(ChequeRequisitionDetail oChequeRequisitionDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oChequeRequisitionDetail.ChequeRequisitionDetailID <= 0)
                {
                    reader = ChequeRequisitionDetailDA.InsertUpdate(tc, oChequeRequisitionDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ChequeRequisitionDetailDA.InsertUpdate(tc, oChequeRequisitionDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeRequisitionDetail = new ChequeRequisitionDetail();
                    oChequeRequisitionDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Save ChequeRequisitionDetail \n" + e.Message, e);
                #endregion
            }
            return oChequeRequisitionDetail;
        }
        public string Delete(ChequeRequisitionDetail oChequeRequisitionDetail, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ChequeRequisitionDetailDA.Delete(tc, oChequeRequisitionDetail, EnumDBOperation.Delete, nUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //throw new Exception(e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ChequeRequisitionDetail Get(int nChequeRequisitionDetailID, int nUserID)
        {
            ChequeRequisitionDetail oChequeRequisitionDetail = new ChequeRequisitionDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChequeRequisitionDetailDA.Get(tc, nChequeRequisitionDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeRequisitionDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get ChequeRequisitionDetail \n" + e.Message, e);
                #endregion
            }
            return oChequeRequisitionDetail;
        }

        public List<ChequeRequisitionDetail> Gets(int nUserID)
        {
            List<ChequeRequisitionDetail> oChequeRequisitionDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeRequisitionDetailDA.Gets(tc);
                oChequeRequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get ChequeRequisitionDetail \n" + e.Message, e);
                #endregion
            }

            return oChequeRequisitionDetails;
        }
        public List<ChequeRequisitionDetail> Gets(int nChequeRequisitionID, int nUserID)
        {
            List<ChequeRequisitionDetail> oChequeRequisitionDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeRequisitionDetailDA.Gets(tc, nChequeRequisitionID);
                oChequeRequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get ChequeRequisitionDetail \n" + e.Message, e);
                #endregion
            }

            return oChequeRequisitionDetails;
        }
        public List<ChequeRequisitionDetail> Gets(string sSQL, int nUserID)
        {
            List<ChequeRequisitionDetail> oChequeRequisitionDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeRequisitionDetailDA.Gets(tc, sSQL);
                oChequeRequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get ChequeRequisitionDetails \n" + e.Message, e);
                #endregion
            }
            return oChequeRequisitionDetails;
        }
        #endregion
    }
}
