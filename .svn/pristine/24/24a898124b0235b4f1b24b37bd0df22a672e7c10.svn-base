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
    public class ChequeBookService : MarshalByRefObject, IChequeBookService
    {
        #region Private functions and declaration
        private ChequeBook MapObject(NullHandler oReader)
        {
            ChequeBook oChequeBook = new ChequeBook();
            oChequeBook.ChequeBookID = oReader.GetInt32("ChequeBookID");
            oChequeBook.BankAccountID = oReader.GetInt32("BankAccountID");
            oChequeBook.BookCodePartOne = oReader.GetString("BookCodePartOne");
            oChequeBook.BookCodePartTwo = oReader.GetString("BookCodePartTwo");
            oChequeBook.PageCount = oReader.GetInt32("PageCount");
            oChequeBook.FirstChequeNo = oReader.GetString("FirstChequeNo");
            oChequeBook.IsActive = oReader.GetBoolean("IsActive");
            oChequeBook.ActivteBy = oReader.GetInt32("ActivteBy");
            oChequeBook.ActivateTime = oReader.GetDateTime("ActivateTime");
            oChequeBook.Note = oReader.GetString("Note");            
            oChequeBook.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oChequeBook.BankBranchID = oReader.GetInt32("BankBranchID");
            oChequeBook.BankID = oReader.GetInt32("BankID");
            oChequeBook.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oChequeBook.AccountNo = oReader.GetString("AccountNo");
            oChequeBook.AccountType = oReader.GetInt16("AccountType");
            oChequeBook.AccountName = oReader.GetString("AccountName");
            oChequeBook.BankName = oReader.GetString("BankName");
            oChequeBook.BankShortName = oReader.GetString("BankShortName");
            oChequeBook.BankBranchName = oReader.GetString("BankBranchName");
            oChequeBook.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oChequeBook.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oChequeBook.BusinessUnitNameCode = oReader.GetString("BusinessUnitNameCode");
            oChequeBook.ChequeSetupName = oReader.GetString("ChequeSetupName");
            
            return oChequeBook;
        }

        private ChequeBook CreateObject(NullHandler oReader)
        {
            ChequeBook oChequeBook = new ChequeBook();
            oChequeBook = MapObject(oReader);
            return oChequeBook;
        }

        private List<ChequeBook> CreateObjects(IDataReader oReader)
        {
            List<ChequeBook> oChequeBooks = new List<ChequeBook>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ChequeBook oItem = CreateObject(oHandler);
                oChequeBooks.Add(oItem);
            }
            return oChequeBooks;
        }

        #endregion

        #region Interface implementation
        public ChequeBookService() { }

        public ChequeBook Save(ChequeBook oChequeBook, int nCurrentUserID)
        {
            string sChequeIDs = "";
            TransactionContext tc = null;
            List<Cheque> oCheques = new List<Cheque>();
            oCheques = oChequeBook.Cheques;
            try
            {                
                tc = TransactionContext.Begin(true);
                #region Cheque Book Part
                IDataReader reader;

                if (oChequeBook.ChequeBookID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nCurrentUserID, EnumModuleName.ChequeBook, EnumRoleOperationType.Add);
                    reader = ChequeBookDA.InsertUpdate(tc, oChequeBook, EnumDBOperation.Insert, nCurrentUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nCurrentUserID, EnumModuleName.ChequeBook, EnumRoleOperationType.Edit);
                    reader = ChequeBookDA.InsertUpdate(tc, oChequeBook, EnumDBOperation.Update, nCurrentUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeBook = new ChequeBook();
                    oChequeBook = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Cheque Part
                foreach (Cheque oItem in oCheques)
                {
                    IDataReader chequereader;
                    oItem.ChequeBookID = oChequeBook.ChequeBookID;
                    if (oItem.ChequeID <= 0)
                    {
                        chequereader = ChequeDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nCurrentUserID, "");
                    }
                    else
                    {
                        chequereader = ChequeDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nCurrentUserID, "");
                    }
                    NullHandler oChequeReader = new NullHandler(chequereader);
                    if (chequereader.Read())
                    {
                        sChequeIDs = sChequeIDs + oChequeReader.GetString("ChequeID") + ",";
                    }
                    chequereader.Close();
                }
                if (sChequeIDs.Length > 0)
                {
                    sChequeIDs = sChequeIDs.Remove(sChequeIDs.Length - 1, 1);
                }
                Cheque oCheque = new Cheque();
                oCheque.ChequeBookID = oChequeBook.ChequeBookID;
                ChequeDA.Delete(tc, oCheque, EnumDBOperation.Delete, nCurrentUserID, sChequeIDs);
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oChequeBook.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ChequeBook. Because of " + e.Message, e);
                #endregion
            }
            
            return oChequeBook;
        }
        public ChequeBook ChequeBookActiveInActive(ChequeBook oChequeBook, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                AuthorizationRoleDA.CheckUserPermission(tc, nCurrentUserID, EnumModuleName.ChequeBook, EnumRoleOperationType.Edit);
                ChequeBookDA.ChequeBookActiveInActive(tc, oChequeBook);

                IDataReader reader;
                reader = ChequeBookDA.Get(tc, oChequeBook.ChequeBookID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeBook = new ChequeBook();
                    oChequeBook = CreateObject(oReader);
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
                throw new ServiceException("Failed to Active/InActive ChequeBook. Because of " + e.Message, e);
                #endregion
            }
            return oChequeBook;
        }
        public string Delete(int nChequeBookID, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ChequeBook oChequeBook = new ChequeBook();
                oChequeBook.ChequeBookID = nChequeBookID;
                AuthorizationRoleDA.CheckUserPermission(tc, nCurrentUserID, EnumModuleName.ChequeBook, EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "ChequeBook", nChequeBookID);
                ChequeBookDA.Delete(tc, oChequeBook, EnumDBOperation.Delete, nCurrentUserID);
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
            return Global.DeleteMessage;
        }

        public ChequeBook Get(int id, int nCurrentUserID)
        {
            ChequeBook oChequeBook = new ChequeBook();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChequeBookDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeBook = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ChequeBook", e);
                #endregion
            }

            return oChequeBook;
        }
        public List<ChequeBook> Gets(int nCurrentUserID)
        {
            List<ChequeBook> oChequeBook = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeBookDA.Gets(tc);
                oChequeBook = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeBook", e);
                #endregion
            }

            return oChequeBook;
        }

        public List<ChequeBook> Gets(bool bIsActive, int nCurrentUserID)
        {
            List<ChequeBook> oChequeBooks = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeBookDA.Gets(tc, bIsActive);
                oChequeBooks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeBook", e);
                #endregion
            }

            return oChequeBooks;
        }



        public List<ChequeBook> GetsByAccountNo(string sAccountNo, int nCurrentUserID)
        {
            List<ChequeBook> oChequeBooks = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ChequeBookDA.GetsByAccountNo(tc, sAccountNo);
                oChequeBooks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeBook", e);
                #endregion
            }

            return oChequeBooks;
        }

        public List<ChequeBook> Gets(string sSQL, int nCurrentUserID)
        {
            List<ChequeBook> oChequeBooks = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeBookDA.Gets(tc, sSQL);
                oChequeBooks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeBook", e);
                #endregion
            }

            return oChequeBooks;
        }        
        #endregion
    }
}
