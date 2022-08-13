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
    public class ChequeService : MarshalByRefObject, IChequeService
    {
        #region Declaration
        int _nSerialNo = 0;
        #endregion
        #region Private functions and declaration
        private Cheque MapObject(NullHandler oReader)
        {
            Cheque oCheque = new Cheque();
            oCheque.ChequeID = oReader.GetInt32("ChequeID");
            oCheque.ChequeBookID = oReader.GetInt32("ChequeBookID");
            oCheque.ChequeStatus = (EnumChequeStatus)oReader.GetInt16("ChequeStatus");
            oCheque.PaymentType = (EnumPaymentType)oReader.GetInt16("PaymentType");
            oCheque.ChequeNo = oReader.GetString("ChequeNo");
            oCheque.ChequeDate = oReader.GetDateTime("ChequeDate");
            oCheque.PayTo = oReader.GetInt32("PayTo");
            oCheque.IssueFigureID = oReader.GetInt32("IssueFigureID");
            oCheque.Amount = oReader.GetDouble("Amount");
            oCheque.VoucherReference = oReader.GetString("VoucherReference");
            oCheque.Note = oReader.GetString("Note");
            oCheque.RegisterPrint = oReader.GetBoolean("RegisterPrint");
            oCheque.BankAccountID = oReader.GetInt32("BankAccountID");
            oCheque.BankBranchID = oReader.GetInt32("BankBranchID");
            oCheque.BankID = oReader.GetInt32("BankID");
            oCheque.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oCheque.BookCodePartOne = oReader.GetString("BookCodePartOne");
            oCheque.BookCodePartTwo = oReader.GetString("BookCodePartTwo");
            oCheque.ChequeSetupID = oReader.GetInt32("ChequeSetupID");
            oCheque.AccountNo = oReader.GetString("AccountNo");
            oCheque.BankName = oReader.GetString("BankName");
            oCheque.BankShortName = oReader.GetString("BankShortName");
            oCheque.BankBranchName = oReader.GetString("BankBranchName");
            oCheque.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oCheque.ContractorName = oReader.GetString("ContractorName");
            oCheque.ChequeIssueTo = oReader.GetString("ChequeIssueTo");
            oCheque.SecondLineIssueTo = oReader.GetString("SecondLineIssueTo");
            oCheque.OperationBy = oReader.GetInt32("OperationBy");
            oCheque.OperationDateTime = oReader.GetDateTime("OperationDateTime");
            oCheque.OperationByName = oReader.GetString("OperationByName");
            oCheque.ContractorPhone = oReader.GetString("ContractorPhone");
            oCheque.ContractorAddress = oReader.GetString("ContractorAddress");
            oCheque.SerialNo = ++_nSerialNo;
            return oCheque;
        }

        private Cheque CreateObject(NullHandler oReader)
        {
            Cheque oCheque = new Cheque();
            oCheque = MapObject(oReader);
            return oCheque;
        }

        private List<Cheque> CreateObjects(IDataReader oReader)
        {
            List<Cheque> oCheques = new List<Cheque>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Cheque oItem = CreateObject(oHandler);
                oCheques.Add(oItem);
            }
            return oCheques;
        }

        #endregion

        #region Interface implementation
        public ChequeService() { }

        public Cheque Save(Cheque oCheque, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {                
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                if (oCheque.ChequeID <= 0)
                {
                    reader = ChequeDA.InsertUpdate(tc, oCheque, EnumDBOperation.Insert, nCurrentUserID,"");
                }
                else
                {
                    reader = ChequeDA.InsertUpdate(tc, oCheque, EnumDBOperation.Update, nCurrentUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCheque = new Cheque();
                    oCheque = CreateObject(oReader);
                }
                reader.Close();              
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCheque.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Cheque. Because of " + e.Message, e);
                #endregion
            }
            
            return oCheque;
        }
        public Cheque UpdateChequeStatus(Cheque oCheque, ChequeHistory oChequeHistory, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader;
                reader = ChequeDA.UpdateChequeStatus(tc, oCheque, oChequeHistory, nCurrentUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCheque = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update Cheque Status. Because of " + e.Message, e);
                #endregion
            }
            return oCheque;
        }
        public List<Cheque> UpdateChequeStatus(List<ChequeHistory> oChequeHistorys, int nCurrentUserID)
        {
            TransactionContext tc = null;
            List<Cheque> oCheques = new List<Cheque>();
            Cheque oCheque = new Cheque();

            try
            {
                tc = TransactionContext.Begin();
                foreach (ChequeHistory oItem in oChequeHistorys)
                {
                    IDataReader reader;
                    oCheque = new Cheque();
                    reader = ChequeDA.UpdateChequeStatus(tc, oCheque, oItem, nCurrentUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oCheque = CreateObject(oReader);
                        oCheques.Add(oCheque);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Update Cheque Status. Because of " + e.Message, e);
                #endregion
            }
            return oCheques;
        }
        public string ConfirmRegisterPrint(List<Cheque> oCheques, int nCurrentUserID)
        {
            TransactionContext tc = null;            
            Cheque oCheque = new Cheque();

            try
            {
                tc = TransactionContext.Begin();
                foreach (Cheque oItem in oCheques)
                {
                    ChequeDA.ConfirmRegisterPrint(tc, oItem, nCurrentUserID);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Update Cheque Status. Because of " + e.Message, e);
                #endregion
            }
            return Global.SessionParamSetMessage;
        }
        public DataSet ChequeTacker(string sSQL, int nCurrentUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChequeDA.Gets(tc, sSQL);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FROM Cheque Tracker", e);
                #endregion
            }
            return oDataSet;
        }        
        public string Delete(int nChequeID, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Cheque oCheque = new Cheque();
                oCheque.ChequeID = nChequeID;
                AuthorizationRoleDA.CheckUserPermission(tc, nCurrentUserID, EnumModuleName.Cheque, EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "Cheque", nChequeID);
                ChequeDA.Delete(tc, oCheque, EnumDBOperation.Delete, nCurrentUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data Delete Successfully.";
        }

        public Cheque Get(int id, int nCurrentUserID)
        {
            Cheque oCheque = new Cheque();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChequeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCheque = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Cheque", e);
                #endregion
            }

            return oCheque;
        }
        public List<Cheque> Gets(int nCurrentUserID)
        {
            List<Cheque> oCheque = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeDA.Gets(tc);
                oCheque = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Cheque", e);
                #endregion
            }

            return oCheque;
        }
        public List<Cheque> Gets(int nChequeBookID, int eSealed, int nCurrentUserID)
        {
            List<Cheque> oCheque = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeDA.Gets(tc, nChequeBookID, eSealed);
                oCheque = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Cheque", e);
                #endregion
            }

            return oCheque;
        }

        public List<Cheque> Gets(int nChequeBookID, int nCurrentUserID)
        {
            List<Cheque> oCheques = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeDA.Gets(tc, nChequeBookID);
                oCheques = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Cheque", e);
                #endregion
            }

            return oCheques;
        }



        public List<Cheque> GetsByChequeNo(string sChequeNo, int nCurrentUserID)
        {
            List<Cheque> oCheques = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ChequeDA.GetsByChequeNo(tc, sChequeNo);
                oCheques = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Cheque", e);
                #endregion
            }

            return oCheques;
        }

        public List<Cheque> Gets(string sSQL, int nCurrentUserID)
        {
            List<Cheque> oCheques = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeDA.Gets(tc, sSQL);
                oCheques = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Cheque", e);
                #endregion
            }

            return oCheques;
        }        
        #endregion
    }
}
