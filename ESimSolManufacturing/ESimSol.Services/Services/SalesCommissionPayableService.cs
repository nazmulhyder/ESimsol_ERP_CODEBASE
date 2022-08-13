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
    public class SalesCommissionPayableService : MarshalByRefObject, ISalesCommissionPayableService
    {
    
        #region Private functions and declaration
        private static SalesCommissionPayable MapObject(NullHandler oReader)
        {
            SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
            oSalesCommissionPayable.SalesCommissionPayableID = oReader.GetInt32("SalesCommissionPayableID");
            oSalesCommissionPayable.ExportPIID = oReader.GetInt32("ExportPIID");
            oSalesCommissionPayable.ExportBillID = oReader.GetInt32("ExportBillID");
            oSalesCommissionPayable.BUID = oReader.GetInt32("BUID");
            oSalesCommissionPayable.Status = oReader.GetInt32("Status");
            oSalesCommissionPayable.CPName = oReader.GetString("CPName");
            oSalesCommissionPayable.Phone = oReader.GetString("Phone");
            oSalesCommissionPayable.ContractorName = oReader.GetString("ContractorName");
            oSalesCommissionPayable.CurrencyID = oReader.GetInt32("CurrencyID");
            oSalesCommissionPayable.CommissionAmount = oReader.GetDouble("CommissionAmount");
            oSalesCommissionPayable.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oSalesCommissionPayable.Percentage = oReader.GetDouble("Percentage");
            oSalesCommissionPayable.MaturityAmount = oReader.GetDouble("MaturityAmount");
            oSalesCommissionPayable.RealizeAmount = oReader.GetDouble("RealizeAmount");
            oSalesCommissionPayable.AdjOverdueAmount = oReader.GetDouble("AdjOverdueAmount");
            oSalesCommissionPayable.AdjAdd = oReader.GetDouble("AdjAdd");
            oSalesCommissionPayable.AdjDeduct = oReader.GetDouble("AdjDeduct");
            oSalesCommissionPayable.AdjPayable = oReader.GetDouble("AdjPayable");
            
            oSalesCommissionPayable.PIDate = oReader.GetDateTime("PIDate");
            oSalesCommissionPayable.LDBCDate = oReader.GetDateTime("LDBCDate");
            oSalesCommissionPayable.MaturityDate = oReader.GetDateTime("MaturityDate");
            oSalesCommissionPayable.MaturityReceivedDate = oReader.GetDateTime("MaturityReceivedDate");
            oSalesCommissionPayable.RelizationDate = oReader.GetDateTime("RelizationDate");
            oSalesCommissionPayable.LCOpeningDate = oReader.GetDateTime("LCOpeningDate");
            oSalesCommissionPayable.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oSalesCommissionPayable.Amount_Paid = oReader.GetDouble("Amount_Paid");
            oSalesCommissionPayable.ContractorID = oReader.GetInt32("ContractorID");
            oSalesCommissionPayable.Currency = oReader.GetString("Currency");
            oSalesCommissionPayable.ExportLCNo = oReader.GetString("ExportLCNo");
            oSalesCommissionPayable.PINo = oReader.GetString("PINo");
            oSalesCommissionPayable.LDBCNo = oReader.GetString("LDBCNo");
            oSalesCommissionPayable.Percentage_Maturity = oReader.GetDouble("Percentage_Maturity");
            oSalesCommissionPayable.Status_Payable = (EnumLSalesCommissionStatus)oReader.GetInt16("Status_Payable");
            oSalesCommissionPayable.CRate = oReader.GetDouble("CRate");
            oSalesCommissionPayable.CurrencyID_BC = oReader.GetInt32("CurrencyID_BC");
            oSalesCommissionPayable.Amount_Bill = oReader.GetDouble("Amount_Bill");
            oSalesCommissionPayable.ExportBillNo = oReader.GetString("ExportBillNo");
            oSalesCommissionPayable.Amount_W_Paid = oReader.GetDouble("Amount_W_Paid");
            oSalesCommissionPayable.IsWillVoucherEffect = oReader.GetBoolean("IsWillVoucherEffect");
            
            return oSalesCommissionPayable;
        }

        public static SalesCommissionPayable CreateObject(NullHandler oReader)
        {
            SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
            oSalesCommissionPayable = MapObject(oReader);            
            return oSalesCommissionPayable;
        }

        private List<SalesCommissionPayable> CreateObjects(IDataReader oReader)
        {
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesCommissionPayable oItem = CreateObject(oHandler);
                oSalesCommissionPayables.Add(oItem);
            }
            return oSalesCommissionPayables;
        }

        #endregion

        #region Interface implementation
        public SalesCommissionPayableService() { }        
        public SalesCommissionPayable Get(int id, Int64 nUserId)
        {
            SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesCommissionPayableDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesCommissionPayable = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oSalesCommissionPayable;
        }
    
        public SalesCommissionPayable Save(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            TransactionContext tc = null;
        
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSalesCommissionPayable.SalesCommissionPayableID <= 0)
                {
                    reader = SalesCommissionPayableDA.InsertUpdate(tc, oSalesCommissionPayable, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = SalesCommissionPayableDA.InsertUpdate(tc, oSalesCommissionPayable, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesCommissionPayable = new SalesCommissionPayable();
                    oSalesCommissionPayable = CreateObject(oReader);
                }
                reader.Close();

                //#region Terms & Condition Part
                //if (oSalesCommissionPayableDetails != null)
                //{
                //    foreach (SalesCommissionPayableDetail oItem in oSalesCommissionPayableDetails)
                //    {
                //        if (oItem.Qty > 0)
                //        {
                //            IDataReader readertnc;
                //            oItem.SalesCommissionPayableID = oSalesCommissionPayable.SalesCommissionPayableID;
                //            if (oItem.SalesCommissionPayableDetailID <= 0)
                //            {
                //                readertnc = SalesCommissionPayableDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                //            }
                //            else
                //            {
                //                readertnc = SalesCommissionPayableDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                //            }
                //            NullHandler oReaderTNC = new NullHandler(readertnc);

                //            if (readertnc.Read())
                //            {
                //                sSalesCommissionPayableDetaillIDs = sSalesCommissionPayableDetaillIDs + oReaderTNC.GetString("SalesCommissionPayableDetailID") + ",";
                //            }
                //            readertnc.Close();
                //        }
                //    }

              
                //}
                //#endregion
                                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oSalesCommissionPayable = new SalesCommissionPayable();
                oSalesCommissionPayable.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSalesCommissionPayable;
        }
        public SalesCommissionPayable VoucherEffect(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesCommissionPayableDA.VoucherEffect(tc, oSalesCommissionPayable);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesCommissionPayable = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSalesCommissionPayable = new SalesCommissionPayable();
                oSalesCommissionPayable.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSalesCommissionPayable;
        }
    
        public List<SalesCommissionPayable> SaveAll(List<SalesCommissionPayable> oSalesCommissionPayables, Int64 nUserID)
        {

            SalesCommissionPayable oSalesCommissionPayable = new SalesCommissionPayable();
            List<SalesCommissionPayable> oSalesCommissionPayables_Return = new List<SalesCommissionPayable>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (SalesCommissionPayable oItem in oSalesCommissionPayables)
                {
                    if (oItem.SalesCommissionPayableID <= 0)
                    {
                        reader = SalesCommissionPayableDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = SalesCommissionPayableDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    //IDataReader reader = SalesCommissionPayableDA.Get(tc, oItem.SalesCommissionPayableID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSalesCommissionPayable = new SalesCommissionPayable();
                        oSalesCommissionPayable = CreateObject(oReader);
                        oSalesCommissionPayables_Return.Add(oSalesCommissionPayable);
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

                oSalesCommissionPayable = new SalesCommissionPayable();
                oSalesCommissionPayable.ErrorMessage = e.Message.Split('~')[0];
                oSalesCommissionPayables_Return.Add(oSalesCommissionPayable);

                #endregion
            }
            return oSalesCommissionPayables_Return;
        }
        public string Delete(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SalesCommissionPayableDA.Delete(tc, oSalesCommissionPayable, EnumDBOperation.Delete, nUserID);                
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
 
        public SalesCommissionPayable Approved(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SalesCommissionPayableDA.InsertUpdate(tc, oSalesCommissionPayable, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesCommissionPayable = new SalesCommissionPayable();
                    oSalesCommissionPayable = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSalesCommissionPayable = new SalesCommissionPayable();
                oSalesCommissionPayable.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSalesCommissionPayable;
        }
        public List<SalesCommissionPayable> ApprovedPayable(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            SalesCommissionPayable _oSalesCommissionPayable = new SalesCommissionPayable();
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                //reader = SalesCommissionPayableDA.PayableApproval(tc, oSalesCommissionPayable, EnumDBOperation.Approval, nUserID);
                //NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    _oSalesCommissionPayable = new SalesCommissionPayable();
                //    _oSalesCommissionPayable = CreateObject(oReader);
                //    oSalesCommissionPayables.Add(_oSalesCommissionPayable);
                //}
                //reader.Close();

                #region ApprovedPayable
                if (oSalesCommissionPayable != null && oSalesCommissionPayable.SalesCommissionPayables.Count > 0)
                {
                    foreach (var oItem in oSalesCommissionPayable.SalesCommissionPayables)
                    {
                        if (oItem.SalesCommissionPayableID <= 0)
                        {
                            throw new Exception("Invalid Sales Commission Payble.!");
                        }
                        else
                        {
                            reader = SalesCommissionPayableDA.PayableApproval(tc, oItem, EnumDBOperation.Approval, nUserID);
                            NullHandler oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                _oSalesCommissionPayable = new SalesCommissionPayable();
                                _oSalesCommissionPayable = CreateObject(oReader);
                                oSalesCommissionPayables.Add(_oSalesCommissionPayable);
                            }
                            reader.Close();
                        }
                    }
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                _oSalesCommissionPayable = new SalesCommissionPayable();
                _oSalesCommissionPayable.ErrorMessage = e.Message.Split('~')[0];
                oSalesCommissionPayables.Add(_oSalesCommissionPayable);

                #endregion
            }
            return oSalesCommissionPayables;
        }
        public SalesCommissionPayable ApprovedPayableForPI(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            SalesCommissionPayable _oSalesCommissionPayable = new SalesCommissionPayable();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                #region ApprovedPayable
                if (oSalesCommissionPayable != null && oSalesCommissionPayable.SalesCommissionPayableID > 0)
                {
                    reader = SalesCommissionPayableDA.PayableApproval(tc, oSalesCommissionPayable, EnumDBOperation.Approval, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        _oSalesCommissionPayable = new SalesCommissionPayable();
                        _oSalesCommissionPayable = CreateObject(oReader);
                    }
                    reader.Close();
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                _oSalesCommissionPayable = new SalesCommissionPayable();
                _oSalesCommissionPayable.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }
            return oSalesCommissionPayable;
        }
        public List<SalesCommissionPayable> ApprovedRequestedPayable(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            SalesCommissionPayable _oSalesCommissionPayable = new SalesCommissionPayable();
            List<SalesCommissionPayable> oSalesCommissionPayables = new List<SalesCommissionPayable>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                //sreader = SalesCommissionPayableDA.PayableApproval(tc, oSalesCommissionPayable, EnumDBOperation.Request, nUserID);
                //NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    _oSalesCommissionPayable = new SalesCommissionPayable();
                //    _oSalesCommissionPayable = CreateObject(oReader);
                //    oSalesCommissionPayables.Add(_oSalesCommissionPayable);
                //}
                //reader.Close();

                #region Request
                if (oSalesCommissionPayable != null && oSalesCommissionPayable.SalesCommissionPayables.Count > 0)
                {
                    foreach (var oItem in oSalesCommissionPayable.SalesCommissionPayables)
                    {
                        if (oItem.SalesCommissionPayableID <= 0)
                        {
                            throw new Exception("Invalid Sales Commission Payble.!");
                        }
                        else
                        {
                            reader = SalesCommissionPayableDA.PayableApproval(tc, oItem, EnumDBOperation.Request, nUserID);
                            NullHandler oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                _oSalesCommissionPayable = new SalesCommissionPayable();
                                _oSalesCommissionPayable = CreateObject(oReader);
                                oSalesCommissionPayables.Add(_oSalesCommissionPayable);
                            }
                            reader.Close();
                        }
                    }
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                _oSalesCommissionPayable = new SalesCommissionPayable();
                _oSalesCommissionPayable.ErrorMessage = e.Message.Split('~')[0];
                oSalesCommissionPayables.Add(_oSalesCommissionPayable);

                #endregion
            }
            return oSalesCommissionPayables;
        }

        public SalesCommissionPayable ApprovedRequestedPayableForPI(SalesCommissionPayable oSalesCommissionPayable, Int64 nUserID)
        {
            SalesCommissionPayable _oSalesCommissionPayable = new SalesCommissionPayable();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                
                #region Request
                if (oSalesCommissionPayable != null && oSalesCommissionPayable.SalesCommissionPayableID > 0)
                {
                    reader = SalesCommissionPayableDA.PayableApproval(tc, oSalesCommissionPayable, EnumDBOperation.Request, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        _oSalesCommissionPayable = new SalesCommissionPayable();
                        _oSalesCommissionPayable = CreateObject(oReader);
                    }
                    reader.Close();
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                _oSalesCommissionPayable = new SalesCommissionPayable();
                _oSalesCommissionPayable.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }
            return _oSalesCommissionPayable;
        }
        public List<SalesCommissionPayable> Gets(int nExportPIID,Int64 nUserId)
        {
            List<SalesCommissionPayable> oSalesCommissionPayables = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesCommissionPayableDA.Gets(tc, nExportPIID);
                oSalesCommissionPayables = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oSalesCommissionPayables;
        }
        public List<SalesCommissionPayable> Gets(string sSQL, Int64 nUserId)
        {
            List<SalesCommissionPayable> oSalesCommissionPayables = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesCommissionPayableDA.Gets(tc, sSQL);
                oSalesCommissionPayables = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oSalesCommissionPayables;
        }
     
     
     
        #endregion
    }
}
