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
    public class BDYEACService : MarshalByRefObject, IBDYEACService
    {
        #region Private functions and declaration
        private BDYEAC MapObject(NullHandler oReader)
        {
            BDYEAC oBDYEAC = new BDYEAC();
            oBDYEAC.BDYEACID = oReader.GetInt32("BDYEACID");
            oBDYEAC.ExportBillID = oReader.GetInt32("ExportBillID");
            oBDYEAC.ExportLCID= oReader.GetInt32("ExportLCID");
            oBDYEAC.MasterLCNos = oReader.GetString("MasterLCNos");
            oBDYEAC.MasterLCDates = oReader.GetString("MasterLCDates");
            oBDYEAC.GarmentsQty = oReader.GetString("GarmentsQty");
            oBDYEAC.BankName = oReader.GetString("BankName");
            oBDYEAC.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oBDYEAC.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oBDYEAC.BUID = oReader.GetInt32("BUID");
            oBDYEAC.SupplierName = oReader.GetString("SupplierName");
            oBDYEAC.ImportLCNo = oReader.GetString("ImportLCNo");
            oBDYEAC.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oBDYEAC.ExportBillNo = oReader.GetString("ExportBillNo");
            oBDYEAC.BillAmount = oReader.GetDouble("BillAmount");
            oBDYEAC.ExportLCNo = oReader.GetString("ExportLCNo");
            oBDYEAC.LCOpeningDate = oReader.GetDateTime("LCOpeningDate");
            oBDYEAC.PartyName = oReader.GetString("PartyName");
            oBDYEAC.PartyAddress = oReader.GetString("PartyAddress");
            oBDYEAC.BUName = oReader.GetString("BUName");
            oBDYEAC.BUAddress = oReader.GetString("BUAddress");
            oBDYEAC.BUShortName = oReader.GetString("BUShortName");
            oBDYEAC.ExportLCAmount = oReader.GetDouble("ExportLCAmount");
            oBDYEAC.Amount = oReader.GetDouble("Amount");
            oBDYEAC.IsPrint = oReader.GetBoolean("IsPrint");
            
            return oBDYEAC;
        }

        private BDYEAC CreateObject(NullHandler oReader)
        {
            BDYEAC oBDYEAC = new BDYEAC();
            oBDYEAC = MapObject(oReader);
            return oBDYEAC;
        }

        private List<BDYEAC> CreateObjects(IDataReader oReader)
        {
            List<BDYEAC> oBDYEAC = new List<BDYEAC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BDYEAC oItem = CreateObject(oHandler);
                oBDYEAC.Add(oItem);
            }
            return oBDYEAC;
        }

        #endregion

        #region Interface implementation
        public BDYEACService() { }

        public BDYEAC Save(BDYEAC oBDYEAC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                List<BDYEACDetail> oBDYEACDetails = new List<BDYEACDetail>();
                oBDYEACDetails = oBDYEAC.BDYEACDetails;
                string sBDYEACDetailIDs = "";
                if (oBDYEAC.BDYEACID <= 0)
                {
                    reader = BDYEACDA.InsertUpdate(tc, oBDYEAC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BDYEACDA.InsertUpdate(tc, oBDYEAC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBDYEAC = new BDYEAC();
                    oBDYEAC = CreateObject(oReader);
                }
                reader.Close();
                #region BDYEAC Detail Part
                if (oBDYEACDetails != null)
                {
                    foreach (BDYEACDetail oItem in oBDYEACDetails)
                    {
                        IDataReader readerdetail;
                        oItem.BDYEACID = oBDYEAC.BDYEACID;
                      
                        if (oItem.BDYEACDetailID <= 0)
                        {
                            readerdetail = BDYEACDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = BDYEACDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sBDYEACDetailIDs = sBDYEACDetailIDs + oReaderDetail.GetString("BDYEACDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sBDYEACDetailIDs.Length > 0)
                    {
                        sBDYEACDetailIDs = sBDYEACDetailIDs.Remove(sBDYEACDetailIDs.Length - 1, 1);
                    }
                }
                BDYEACDetail oBDYEACDetail = new BDYEACDetail();
                oBDYEACDetail.BDYEACID = oBDYEAC.BDYEACID;
                BDYEACDetailDA.Delete(tc, oBDYEACDetail, EnumDBOperation.Delete, nUserID, sBDYEACDetailIDs);
                #endregion

                #region Get BDYEAC
                reader = BDYEACDA.Get(tc, oBDYEAC.BDYEACID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBDYEAC = new BDYEAC();
                    oBDYEAC = CreateObject(oReader);
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
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save BDYEAC. Because of " + e.Message, e);
                #endregion
            }
            return oBDYEAC;
        }

        //
        public BDYEAC CreatePrint(BDYEAC oBDYEAC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                BDYEACDA.CreatePrint(tc, oBDYEAC);
                #region Get BDYEAC
                reader = BDYEACDA.Get(tc, oBDYEAC.BDYEACID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBDYEAC = new BDYEAC();
                    oBDYEAC = CreateObject(oReader);
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
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save BDYEAC. Because of " + e.Message, e);
                #endregion
            }
            return oBDYEAC;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BDYEAC oBDYEAC = new BDYEAC();
                oBDYEAC.BDYEACID = id;
                BDYEACDA.Delete(tc, oBDYEAC, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public BDYEAC Get(int id, Int64 nUserId)
        {
            BDYEAC oBDYEAC = new BDYEAC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BDYEACDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBDYEAC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BDYEAC", e);
                #endregion
            }
            return oBDYEAC;
        }

   
        public List<BDYEAC> Gets(Int64 nUserID)
        {
            List<BDYEAC> oBDYEACs = new List<BDYEAC>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BDYEACDA.Gets(tc);
                oBDYEACs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BDYEAC", e);
                #endregion
            }
            return oBDYEACs;
        }
        public List<BDYEAC> Gets(string sSQL,Int64 nUserID)
        {
            List<BDYEAC> oBDYEACs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BDYEACDA.Gets(tc,sSQL);
                oBDYEACs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BDYEAC", e);
                #endregion
            }
            return oBDYEACs;
        }


    
        #endregion
    }   
}