using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class EmployeeLoanRefundService : MarshalByRefObject, IEmployeeLoanRefundService
    {
        #region Private functions and declaration
        private EmployeeLoanRefund MapObject(NullHandler oReader)
        {
            EmployeeLoanRefund oEmployeeLoanRefund = new EmployeeLoanRefund();
            oEmployeeLoanRefund.ELRID = oReader.GetInt32("ELRID");
            oEmployeeLoanRefund.EmployeeLoanID = oReader.GetInt32("EmployeeLoanID");
            oEmployeeLoanRefund.NoOfInstallmentRefund = oReader.GetInt32("NoOfInstallmentRefund");
            oEmployeeLoanRefund.Amount = oReader.GetDouble("Amount");
            oEmployeeLoanRefund.ServiceCharge = oReader.GetDouble("ServiceCharge");
            oEmployeeLoanRefund.Note = oReader.GetString("Note");
            oEmployeeLoanRefund.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeLoanRefund.ApproveDate = oReader.GetDateTime("ApproveDate");
            oEmployeeLoanRefund.RefundNo = oReader.GetString("RefundNo");
            oEmployeeLoanRefund.RefundDate = oReader.GetDateTime("RefundDate");

            oEmployeeLoanRefund.ApproveByName = oReader.GetString("ApproveByName");
            oEmployeeLoanRefund.LoanCode = oReader.GetString("LoanCode");
            oEmployeeLoanRefund.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeLoanRefund.EmployeeCode = oReader.GetString("EmployeeCode");
            
            
            return oEmployeeLoanRefund;
        }

        public static EmployeeLoanRefund CreateObject(NullHandler oReader)
        {
            EmployeeLoanRefund oEmployeeLoanRefund = new EmployeeLoanRefund();
            EmployeeLoanRefundService oService = new EmployeeLoanRefundService();
            oEmployeeLoanRefund = oService.MapObject(oReader);
            return oEmployeeLoanRefund;
        }
        private List<EmployeeLoanRefund> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLoanRefund> oEmployeeLoanRefunds = new List<EmployeeLoanRefund>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLoanRefund oItem = CreateObject(oHandler);
                oEmployeeLoanRefunds.Add(oItem);
            }
            return oEmployeeLoanRefunds;
        }

        #endregion

        #region Interface implementation
        public EmployeeLoanRefundService() { }

        public EmployeeLoanRefund IUD(EmployeeLoanRefund oEmployeeLoanRefund, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeLoanRefundDA.IUD(tc, oEmployeeLoanRefund, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanRefund = new EmployeeLoanRefund();
                    oEmployeeLoanRefund = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oEmployeeLoanRefund = new EmployeeLoanRefund(); oEmployeeLoanRefund.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oEmployeeLoanRefund.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oEmployeeLoanRefund;
        }

        public EmployeeLoanRefund Get(int nELRID, long nUserID)
        {
            EmployeeLoanRefund oEmployeeLoanRefund = new EmployeeLoanRefund();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanRefundDA.Get(tc, nELRID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanRefund = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oEmployeeLoanRefund.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoanRefund;
        }

        public List<EmployeeLoanRefund> Gets(string sSQL, long nUserID)
        {
            List<EmployeeLoanRefund> oEmployeeLoanRefunds = new List<EmployeeLoanRefund>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanRefundDA.Gets(tc, sSQL);
                oEmployeeLoanRefunds = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                EmployeeLoanRefund oEmployeeLoanRefund = new EmployeeLoanRefund();
                oEmployeeLoanRefund.ErrorMessage = e.Message;
                oEmployeeLoanRefunds.Add(oEmployeeLoanRefund);
                #endregion
            }

            return oEmployeeLoanRefunds;
        }


        #endregion
    }
}