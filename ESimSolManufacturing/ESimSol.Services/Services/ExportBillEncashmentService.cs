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
    public class ExportBillEncashmentService : MarshalByRefObject, IExportBillEncashmentService
    {
        #region Private functions and declaration
        private ExportBillEncashment MapObject(NullHandler oReader)
        {
            ExportBillEncashment oExportBillEncashment = new ExportBillEncashment();
            oExportBillEncashment.ExportBillEncashmentID = oReader.GetInt32("ExportBillEncashmentID");
            oExportBillEncashment.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportBillEncashment.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oExportBillEncashment.SubledgerID = oReader.GetInt32("SubledgerID");
            oExportBillEncashment.LoanInstallmentID = oReader.GetInt32("LoanInstallmentID");        
            oExportBillEncashment.CurrencyID = oReader.GetInt32("CurrencyID");            
            oExportBillEncashment.CCRate = oReader.GetDouble("CCRate");
            oExportBillEncashment.Amount = oReader.GetDouble("Amount");
            oExportBillEncashment.AccountCode = oReader.GetString("AccountCode");
            oExportBillEncashment.AccountHeadName = oReader.GetString("AccountHeadName");
            oExportBillEncashment.SubledgerCode = oReader.GetString("SubledgerCode");
            oExportBillEncashment.SubledgerName = oReader.GetString("SubledgerName");
            oExportBillEncashment.CurrencyName = oReader.GetString("CurrencyName");
            oExportBillEncashment.Currency = oReader.GetString("Currency");
            oExportBillEncashment.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportBillEncashment.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportBillEncashment.EncashmentDate = oReader.GetDateTime("EncashmentDate");
            oExportBillEncashment.LoanNo = oReader.GetString("LoanNo");
            oExportBillEncashment.LoanRefType = (EnumLoanRefType)oReader.GetInt32("LoanRefType");
            oExportBillEncashment.LoanLCID = oReader.GetInt32("LoanLCID");
            oExportBillEncashment.LoanStartDate = oReader.GetDateTime("LoanStartDate");
            oExportBillEncashment.LoanCurencyID = oReader.GetInt32("LoanCurencyID");
            oExportBillEncashment.LoanID = oReader.GetInt32("LoanID");
            oExportBillEncashment.LoanCurency = oReader.GetString("LoanCurency");
            oExportBillEncashment.PrincipalAmount = oReader.GetDouble("PrincipalAmount");
            oExportBillEncashment.PrincipalAmountBC = oReader.GetDouble("PrincipalAmountBC");
            oExportBillEncashment.InstallmentPrincipalAmount = oReader.GetDouble("InstallmentPrincipalAmount");           
            oExportBillEncashment.TotalInterestAmount = oReader.GetDouble("TotalInterestAmount");
            oExportBillEncashment.ChargeAmount = oReader.GetDouble("ChargeAmount");
            oExportBillEncashment.DiscountPaidAmount = oReader.GetDouble("DiscountPaidAmount");
            oExportBillEncashment.DiscountRcvAmount = oReader.GetDouble("DiscountRcvAmount");
            oExportBillEncashment.TotalPayableAmount = oReader.GetDouble("TotalPayableAmount");            
            oExportBillEncashment.LoanLCNo = oReader.GetString("LoanLCNo");
            return oExportBillEncashment;
        }

        private ExportBillEncashment CreateObject(NullHandler oReader)
        {
            ExportBillEncashment oExportBillEncashment = new ExportBillEncashment();
            oExportBillEncashment = MapObject(oReader);
            return oExportBillEncashment;
        }

        private List<ExportBillEncashment> CreateObjects(IDataReader oReader)
        {
            List<ExportBillEncashment> oExportBillEncashments = new List<ExportBillEncashment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportBillEncashment oItem = CreateObject(oHandler);
                oExportBillEncashments.Add(oItem);
            }
            return oExportBillEncashments;
        }

        #endregion

        #region Interface implementation
        public ExportBillEncashmentService() { }

        public ExportBillEncashment Save(ExportBillEncashment oExportBillEncashment, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region ExportBillEncashment part
                IDataReader reader;
                if (oExportBillEncashment.ExportBillEncashmentID <= 0)
                {                    
                    reader = ExportBillEncashmentDA.InsertUpdate(tc, oExportBillEncashment, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {                    
                    reader = ExportBillEncashmentDA.InsertUpdate(tc, oExportBillEncashment, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportBillEncashment = new ExportBillEncashment();
                    oExportBillEncashment = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('~')[0];
                oExportBillEncashment.ErrorMessage = Message;
                #endregion
            }
            return oExportBillEncashment;
        }
        public string Delete(ExportBillEncashment oExportBillEncashment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                ExportBillEncashmentDA.Delete(tc, oExportBillEncashment, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ExportBillEncashment. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ExportBillEncashment Get(int id, Int64 nUserId)
        {
            ExportBillEncashment oAccountHead = new ExportBillEncashment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportBillEncashmentDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ExportBillEncashment", e);
                #endregion
            }

            return oAccountHead;
        }
            
        public List<ExportBillEncashment> Gets(int nExportBillID, Int64 nUserID)
        {
            List<ExportBillEncashment> oExportBillEncashment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillEncashmentDA.Gets(tc, nExportBillID);
                oExportBillEncashment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillEncashment", e);
                #endregion
            }

            return oExportBillEncashment;
        }

        public List<ExportBillEncashment> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportBillEncashment> oExportBillEncashment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportBillEncashmentDA.Gets(tc, sSQL);
                oExportBillEncashment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportBillEncashment", e);
                #endregion
            }

            return oExportBillEncashment;
        }

        #endregion
    }
}
