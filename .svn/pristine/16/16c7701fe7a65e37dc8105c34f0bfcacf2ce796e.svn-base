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
    public class Export_LDBPService : MarshalByRefObject, IExport_LDBPService
    {
        #region Private functions and declaration
        private static Export_LDBP MapObject(NullHandler oReader)
        {
            Export_LDBP oExport_LDBP = new Export_LDBP();
            oExport_LDBP.Export_LDBPID = oReader.GetInt32("Export_LDBPID");
            oExport_LDBP.RefNo = oReader.GetString("RefNo");
            oExport_LDBP.BankAccountID = oReader.GetInt32("BankAccountID");
            oExport_LDBP.LetterIssueDate = oReader.GetDateTime("LetterIssueDate");
            oExport_LDBP.RequestBy = oReader.GetInt32("RequestBy");
            oExport_LDBP.ApprovedBy = oReader.GetInt32("ApprovedBy");
            //oExport_LDBP.ValueInPercentage = oReader.GetDouble("ValueInPercentage");
            oExport_LDBP.Note = oReader.GetString("Note");
            oExport_LDBP.AccountNo = oReader.GetString("AccountNo");
            oExport_LDBP.BankName = oReader.GetString("BankName");
            oExport_LDBP.BankID = oReader.GetInt32("BankID");
            oExport_LDBP.BankBranchID = oReader.GetInt32("BankBranchID");
            oExport_LDBP.BranchName = oReader.GetString("BranchName");
            oExport_LDBP.BranchAddress = oReader.GetString("BranchAddress");
            oExport_LDBP.RequestByName = oReader.GetString("RequestByName");
            oExport_LDBP.ApprovedByName = oReader.GetString("ApprovedByName");
            oExport_LDBP.CurrencyType = oReader.GetBoolean("CurrencyType");
            oExport_LDBP.Status = (EnumLCBillEvent)oReader.GetInt32("Status");
            oExport_LDBP.StatusInt = oReader.GetInt32("Status");
            oExport_LDBP.BUID = oReader.GetInt32("BUID");
            return oExport_LDBP;
        }

        public static Export_LDBP CreateObject(NullHandler oReader)
        {
            Export_LDBP oExport_LDBP = new Export_LDBP();
            oExport_LDBP = MapObject(oReader);
            return oExport_LDBP;
        }

        private List<Export_LDBP> CreateObjects(IDataReader oReader)
        {
            List<Export_LDBP> oExport_LDBPs = new List<Export_LDBP>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Export_LDBP oItem = CreateObject(oHandler);
                oExport_LDBPs.Add(oItem);
            }
            return oExport_LDBPs;
        }

        #endregion

        #region Interface implementation
        public Export_LDBPService() { }

        public Export_LDBP Save(Export_LDBP oExport_LDBP, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<Export_LDBPDetail> oExport_LDBPDetails = new List<Export_LDBPDetail>();
                Export_LDBPDetail oExport_LDBPDetail = new Export_LDBPDetail();
                oExport_LDBPDetails = oExport_LDBP.Export_LDBPDetails;

                string sExport_LDBPDetailIDs = "";

                #region Export_LDBP part
                IDataReader reader;
                if (oExport_LDBP.Export_LDBPID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "Export_LDBP", EnumRoleOperationType.Add);
                    reader = Export_LDBPDA.InsertUpdate(tc, oExport_LDBP, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "Export_LDBP", EnumRoleOperationType.Edit);
                    reader = Export_LDBPDA.InsertUpdate(tc, oExport_LDBP, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExport_LDBP = new Export_LDBP();
                    oExport_LDBP = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Invoice Purchase Detail Part
                if (oExport_LDBPDetails != null)
                {
                    foreach (Export_LDBPDetail oItem in oExport_LDBPDetails)
                    {
                        IDataReader readerdetail;
                        oItem.Export_LDBPID = oExport_LDBP.Export_LDBPID;
                        if (oItem.Export_LDBPDetailID <= 0)
                        {
                            readerdetail = Export_LDBPDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = Export_LDBPDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sExport_LDBPDetailIDs = sExport_LDBPDetailIDs + oReaderDetail.GetString("Export_LDBPDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sExport_LDBPDetailIDs.Length > 0)
                    {
                        sExport_LDBPDetailIDs = sExport_LDBPDetailIDs.Remove(sExport_LDBPDetailIDs.Length - 1, 1);
                    }
                    oExport_LDBPDetail = new Export_LDBPDetail();
                    oExport_LDBPDetail.Export_LDBPID = oExport_LDBP.Export_LDBPID;
                    Export_LDBPDetailDA.Delete(tc, oExport_LDBPDetail, EnumDBOperation.Delete, nUserID, sExport_LDBPDetailIDs);
                }

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
                Message = Message.Split('!')[0];
                oExport_LDBP.ErrorMessage = Message;
                #endregion
            }
            return oExport_LDBP;
        }
        public Export_LDBP Cancel_Request(Export_LDBP oExport_LDBP, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<Export_LDBPDetail> oExport_LDBPDetails = new List<Export_LDBPDetail>();
                Export_LDBPDetail oExport_LDBPDetail = new Export_LDBPDetail();
                oExport_LDBPDetails = oExport_LDBP.Export_LDBPDetails;

                string sExport_LDBPDetailIDs = "";

                #region Export_LDBP part
                IDataReader reader;
                if (oExport_LDBP.Export_LDBPID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "Export_LDBP", EnumRoleOperationType.Add);
                   reader = Export_LDBPDA.InsertUpdate(tc, oExport_LDBP, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "Export_LDBP", EnumRoleOperationType.Edit);
                    reader = Export_LDBPDA.InsertUpdate(tc, oExport_LDBP, EnumDBOperation.Cancel, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExport_LDBP = new Export_LDBP();
                    oExport_LDBP = CreateObject(oReader);
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
                Message = Message.Split('!')[0];
                oExport_LDBP.ErrorMessage = Message;
                #endregion
            }
            return oExport_LDBP;
        }
        public Export_LDBP Approved(Export_LDBP oExport_LDBP, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                                
                IDataReader reader;
                reader = Export_LDBPDA.InsertUpdate(tc, oExport_LDBP, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExport_LDBP = new Export_LDBP();
                    oExport_LDBP = CreateObject(oReader);
                }
                reader.Close();                                
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
                oExport_LDBP.ErrorMessage = Message;
                #endregion
            }
            return oExport_LDBP;
        }


        public string Delete(Export_LDBP oExport_LDBP, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
             
                Export_LDBPDA.Delete(tc, oExport_LDBP, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Export_LDBP. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public Export_LDBP Get(int id, Int64 nUserId)
        {
            Export_LDBP oAccountHead = new Export_LDBP();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = Export_LDBPDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Export_LDBP", e);
                #endregion
            }

            return oAccountHead;
        }
            
        public List<Export_LDBP> Gets(Int64 nUserID)
        {
            List<Export_LDBP> oExport_LDBP = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = Export_LDBPDA.Gets(tc);
                oExport_LDBP = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Export_LDBP", e);
                #endregion
            }

            return oExport_LDBP;
        }

        public List<Export_LDBP> Gets(string sSQL, Int64 nUserID)
        {
            List<Export_LDBP> oExport_LDBP = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = Export_LDBPDA.Gets(tc, sSQL);
                oExport_LDBP = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Export_LDBP", e);
                #endregion
            }

            return oExport_LDBP;
        }

        public List<Export_LDBP> WaitForApproval( int nBUID,Int64 nUserID)
        {
            List<Export_LDBP> oExport_LDBP = new List<Export_LDBP>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = Export_LDBPDA.WaitForApproval(tc, nBUID);
                oExport_LDBP = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Export_LDBP", e);
                #endregion
            }

            return oExport_LDBP;
        }       
        #endregion
    }
}
