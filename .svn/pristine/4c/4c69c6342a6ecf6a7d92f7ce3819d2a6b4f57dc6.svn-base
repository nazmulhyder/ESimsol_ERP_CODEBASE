using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    
    public class SampleInvoiceService : MarshalByRefObject, ISampleInvoiceService
    {
        #region Private functions and declaration
        private SampleInvoice MapObject(NullHandler oReader)
        {
            SampleInvoice oSampleInvoice = new SampleInvoice();
            oSampleInvoice.SampleInvoiceID = oReader.GetInt32("SampleInvoiceID");
            oSampleInvoice.SampleInvoiceNo = oReader.GetString("SampleInvoiceNo");
            oSampleInvoice.InvoiceNo = oReader.GetString("InvoiceNo");
            oSampleInvoice.Amount = oReader.GetDouble("Amount");
            oSampleInvoice.CurrencyID = oReader.GetInt32("CurrencyID");
            oSampleInvoice.BUID = oReader.GetInt32("BUID");
            oSampleInvoice.RateUnit = oReader.GetInt32("RateUnit");
            oSampleInvoice.SampleInvoiceDate = oReader.GetDateTime("SampleInvoiceDate");
            oSampleInvoice.ContractorID = oReader.GetInt32("ContractorID");
            oSampleInvoice.ContractorPersopnnalID = oReader.GetInt32("ContractorPersopnnalID");
            oSampleInvoice.CurrentStatusInt = oReader.GetInt16("CurrentStatus");
            oSampleInvoice.ApproveBy = oReader.GetInt32("ApproveBy");
            oSampleInvoice.ApprovalRemark = oReader.GetString("ApprovalRemark");
            oSampleInvoice.ApprovedDate = oReader.GetDateTime("ApproveDate");
            oSampleInvoice.PaymentDate = oReader.GetDateTime("PaymentDate");
            oSampleInvoice.RequirementDate = oReader.GetDateTime("RequirementDate");
            oSampleInvoice.Remark = oReader.GetString("Remark");
            oSampleInvoice.InvoiceType = oReader.GetInt32("InvoiceType");
            oSampleInvoice.PaymentType = oReader.GetInt32("PaymentType");
            oSampleInvoice.ExportPIID = oReader.GetInt32("ExportPIID");
            oSampleInvoice.CurrentStatus = (EnumSampleInvoiceStatus)oReader.GetInt32("CurrentStatus");
            oSampleInvoice.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oSampleInvoice.CurrencyName = oReader.GetString("CurrencyName");
            oSampleInvoice.ExchangeCurrencySymbol = oReader.GetString("ExchangeCurrencySymbol");
            oSampleInvoice.ExchangeCurrencyName = oReader.GetString("ExchangeCurrencyName");
            oSampleInvoice.ExchangeCurrencyID = oReader.GetInt32("ExchangeCurrencyID");
            oSampleInvoice.MRNo = oReader.GetString("MRNo");
            oSampleInvoice.AlreadyPaid = oReader.GetDouble("AlreadyPaid");
            oSampleInvoice.AlreadyDiscount = oReader.GetDouble("AlreadyDiscount");
            oSampleInvoice.IsWillVoucherEffect = oReader.GetBoolean("IsWillVoucherEffect");
            oSampleInvoice.AlreadyAdditionalAmount = oReader.GetDouble("AlreadyAdditionalAmount");
            oSampleInvoice.Charge = oReader.GetDouble("Charge");
            oSampleInvoice.Discount = oReader.GetDouble("Discount");
            oSampleInvoice.YetToAdjust = oReader.GetDouble("YetToAdjust");
            oSampleInvoice.WaitForAdjust = oReader.GetDouble("WaitForAdjust");
            
            //Derive 
            oSampleInvoice.ContractorName = oReader.GetString("ContractorName");
            oSampleInvoice.ContractorPersopnnalName = oReader.GetString("ContractorPersopnnalName");
            oSampleInvoice.ContractNo = oReader.GetString("ContractNo");
            oSampleInvoice.Email = oReader.GetString("Email");
            oSampleInvoice.ApproveByName = oReader.GetString("ApproveByName");
            oSampleInvoice.ConversionRate = oReader.GetDouble("CRate");
            oSampleInvoice.ContractorAddress = oReader.GetString("ContractorAddress");
            //oSampleInvoice.BillAmount = oReader.GetDouble("BillAmount");
            oSampleInvoice.Amount_Paid = oReader.GetDouble("Amount_Paid");
            oSampleInvoice.IsAdvance = oReader.GetBoolean("IsAdvance");
            
            oSampleInvoice.PaymentSettlementStatus = (EnumSettlementStatus)oReader.GetInt16("PaymentSettlementStatus");
            oSampleInvoice.ProductionSettlementStatus = (EnumSettlementStatus)oReader.GetInt16("ProductionSettlementStatus");
            oSampleInvoice.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oSampleInvoice.MKTPName = oReader.GetString("MKTPName");
            oSampleInvoice.PreparebyName = oReader.GetString("PreparebyName");
            oSampleInvoice.IsPaymentDone = oReader.GetBoolean("IsPaymentDone");
            oSampleInvoice.ExportPINo = oReader.GetString("PINo");
            oSampleInvoice.LCNo = oReader.GetString("LCNo");
            return oSampleInvoice;
        }
   
        private SampleInvoice CreateObject(NullHandler oReader)
        {
            SampleInvoice oSampleInvoice = new SampleInvoice();
            oSampleInvoice= MapObject(oReader);
            return oSampleInvoice;
        }
   
        private List<SampleInvoice> CreateObjects(IDataReader oReader)
        {
            List<SampleInvoice> oSampleInvoices = new List<SampleInvoice>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleInvoice oItem = CreateObject(oHandler);
                oSampleInvoices.Add(oItem);
            }
            return oSampleInvoices;
        }

        #endregion

        #region Interface implementation
        public SampleInvoiceService() { }
   
        public SampleInvoice Save(SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);

                List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
                oSampleInvoiceDetails = oSampleInvoice.SampleInvoiceDetails;

                IDataReader reader;
                if (oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.DocCharge || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Qty || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Value || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Commission || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.ReturnAdjustment)
                {
                    if (oSampleInvoice.SampleInvoiceID <= 0)
                    {
                        reader = SampleInvoiceDA.InsertUpdate_Adj(tc, oSampleInvoice, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = SampleInvoiceDA.InsertUpdate_Adj(tc, oSampleInvoice, EnumDBOperation.Update, nUserID);
                    }
                }
                else
                {
                    if (oSampleInvoice.SampleInvoiceID <= 0)
                    {
                        reader = SampleInvoiceDA.InsertUpdate(tc, oSampleInvoice, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        VoucherDA.CheckVoucherReference(tc, "SampleInvoice", "SampleInvoiceID", oSampleInvoice.SampleInvoiceID);
                        reader = SampleInvoiceDA.InsertUpdate(tc, oSampleInvoice, EnumDBOperation.Update, nUserID);
                    }
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = new SampleInvoice();
                    oSampleInvoice = CreateObject(oReader);
                }
                reader.Close();
                oSampleInvoice.Amount = 0;
                oSampleInvoice.Amount =oSampleInvoice.Amount+oSampleInvoice.Charge - oSampleInvoice.Discount;
                #region SampleInvoiceDetails
                foreach (SampleInvoiceDetail oItem in oSampleInvoiceDetails)
                {
                    IDataReader readerPPC;

                    oItem.SampleInvoiceID = oSampleInvoice.SampleInvoiceID;

                    if (oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.DocCharge || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Qty || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Value || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Commission || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.ReturnAdjustment)
                    {
                        oSampleInvoice.Amount = oSampleInvoice.Amount + oItem.Amount;
                        if (oItem.SampleInvoiceDetailID <= 0)
                        {
                            readerPPC = SampleInvoiceDetailDA.InsertUpdate_Adj(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerPPC = SampleInvoiceDetailDA.InsertUpdate_Adj(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                    }
                    else
                    {
                        oItem.Amount = oItem.Qty * oItem.UnitPrice;
                        oSampleInvoice.Amount = oSampleInvoice.Amount +( oItem.Qty*oItem.UnitPrice);
                        if (oItem.DyeingOrderID > 0)
                        {
                            //SampleInvoiceDetailDA.UpdateInvoiceID(tc, oItem.SampleInvoiceID, oItem.DyeingOrderID);
                            readerPPC = SampleInvoiceDetailDA.InsertUpdate_AddDO(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            if (oItem.SampleInvoiceDetailID <= 0 && oItem.DyeingOrderID <= 0)
                            {
                                readerPPC = SampleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                readerPPC = SampleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            }
                        }
                    }
                    NullHandler oReaderDetailPPC = new NullHandler(readerPPC);
                  
                    readerPPC.Close();
                }
               
            
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                //#region Handle Exception
                //if (tc != null)
                //    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                //#endregion
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                
                oSampleInvoice.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Product. Because of " + e.Message, e);
                #endregion
            }
            return oSampleInvoice;
        }
        public SampleInvoice UpdateVoucherEffect(SampleInvoice oSampleInvoice, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SampleInvoiceDA.UpdateVoucherEffect(tc, oSampleInvoice);
                IDataReader reader;
                reader = SampleInvoiceDA.Get(tc, oSampleInvoice.SampleInvoiceID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = new SampleInvoice();
                    oSampleInvoice = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSampleInvoice = new SampleInvoice();
                oSampleInvoice.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oSampleInvoice;

        }
        public SampleInvoice Save_Revise(SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            
            try
            {
                tc = TransactionContext.Begin(true);

                List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
                oSampleInvoiceDetails = oSampleInvoice.SampleInvoiceDetails;

                IDataReader reader;
                if (oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.DocCharge || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Qty || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Value || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Commission || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.ReturnAdjustment)
                {
                    if (oSampleInvoice.SampleInvoiceID <= 0)
                    {
                        reader = SampleInvoiceDA.InsertUpdate_Adj(tc, oSampleInvoice, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = SampleInvoiceDA.InsertUpdate_Adj(tc, oSampleInvoice, EnumDBOperation.Update, nUserID);
                    }
                }
                else
                {
                    if (oSampleInvoice.SampleInvoiceID <= 0)
                    {
                        reader = SampleInvoiceDA.InsertUpdateLog(tc, oSampleInvoice, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = SampleInvoiceDA.InsertUpdateLog(tc, oSampleInvoice, EnumDBOperation.Update, nUserID);
                    }
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = new SampleInvoice();
                    oSampleInvoice = CreateObject(oReader);
                }
                reader.Close();
                oSampleInvoice.Amount = 0;
                oSampleInvoice.Amount = oSampleInvoice.Amount + oSampleInvoice.Charge - oSampleInvoice.Discount;
                #region SampleInvoiceDetail
                foreach (SampleInvoiceDetail oItem in oSampleInvoiceDetails)
                {
                    IDataReader readerPPC;

                    oItem.SampleInvoiceID = oSampleInvoice.SampleInvoiceID;

                    if (oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.DocCharge || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Qty || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Value || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Commission || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.ReturnAdjustment)
                    {
                        oSampleInvoice.Amount = oSampleInvoice.Amount + oItem.Amount;
                        if (oItem.SampleInvoiceDetailID <= 0)
                        {
                            readerPPC = SampleInvoiceDetailDA.InsertUpdate_Adj(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerPPC = SampleInvoiceDetailDA.InsertUpdate_Adj(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                    }
                    else
                    {
                        oItem.Amount = oItem.Qty * oItem.UnitPrice;
                        oSampleInvoice.Amount = oSampleInvoice.Amount + (oItem.Qty * oItem.UnitPrice);
                        if (oItem.DyeingOrderID > 0)
                        {
                            //SampleInvoiceDetailDA.UpdateInvoiceID(tc, oItem.SampleInvoiceID, oItem.DyeingOrderID);
                            readerPPC = SampleInvoiceDetailDA.InsertUpdate_AddDO(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            if (oItem.SampleInvoiceDetailID <= 0 && oItem.DyeingOrderID <= 0)
                            {
                                readerPPC = SampleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                readerPPC = SampleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            }
                        }
                    }
                    NullHandler oReaderDetailPPC = new NullHandler(readerPPC);

                    readerPPC.Close();
                }


                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                //#region Handle Exception
                //if (tc != null)
                //    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                //#endregion
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSampleInvoice.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Product. Because of " + e.Message, e);
                #endregion
            }
            return oSampleInvoice;
        }
        public SampleInvoice Save_AddDO(SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            double nAmount = 0;
            try
            {
                tc = TransactionContext.Begin(true);

                List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
                oSampleInvoiceDetails = oSampleInvoice.SampleInvoiceDetails;
             
                #region Purchase Payment Contract
                foreach (SampleInvoiceDetail oItem in oSampleInvoiceDetails)
                {
                    IDataReader readerPPC;

                    oItem.SampleInvoiceID = oSampleInvoice.SampleInvoiceID;
                    oSampleInvoice.Amount = oSampleInvoice.Amount + oItem.Amount;

                    if (oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.DocCharge || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Qty || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Value || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Commission || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.ReturnAdjustment)
                    {
                        if (oItem.SampleInvoiceDetailID <= 0)
                        {
                            readerPPC = SampleInvoiceDetailDA.InsertUpdate_Adj(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerPPC = SampleInvoiceDetailDA.InsertUpdate_Adj(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                    }
                    else
                    {
                        oItem.Amount = oItem.Qty * oItem.UnitPrice;
                        oSampleInvoice.Amount = oSampleInvoice.Amount + (oItem.Qty * oItem.UnitPrice);
                        if (oItem.DyeingOrderID > 0)
                        {
                            //SampleInvoiceDetailDA.UpdateInvoiceID(tc, oItem.SampleInvoiceID, oItem.DyeingOrderID);
                            readerPPC = SampleInvoiceDetailDA.InsertUpdate_AddDO(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            if (oItem.SampleInvoiceDetailID <= 0 && oItem.DyeingOrderID <= 0)
                            {
                                readerPPC = SampleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                readerPPC = SampleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            }
                        }
                    }
                    NullHandler oReaderDetailPPC = new NullHandler(readerPPC);

                    readerPPC.Close();
                }


                #endregion
                tc.End();
            }
            catch (Exception e)
            {
               
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSampleInvoice.ErrorMessage = e.Message.Split('~')[0];
               
                #endregion
            }
            return oSampleInvoice;
        }
   
        public SampleInvoice Save_Rate(SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            //double nAmount = 0;
            try
            {
                tc = TransactionContext.Begin(true);

                List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
                List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
                oSampleInvoiceDetails = oSampleInvoice.SampleInvoiceDetails;
                oDyeingOrderDetails = oSampleInvoice.DyeingOrderDetails;
                #region Purchase Payment Contract
                foreach (DyeingOrderDetail oItem in oDyeingOrderDetails)
                {
                   // oSampleInvoice.Amount = oSampleInvoice.Amount +( oItem.Qty * oItem.UnitPrice);
                    if (oItem.DyeingOrderDetailID > 0)
                    {
                        if (String.IsNullOrEmpty(oItem.RGB))
                        {
                            oItem.RGB = "";
                        }
                        oItem.RGB = oItem.RGB.Trim();
                        DyeingOrderDetailDA.Update_Rate(tc, oItem);
                    }
                }

                #endregion
                #region Purchase Payment Contract
                foreach (SampleInvoiceDetail oItem in oSampleInvoiceDetails)
                {
                    IDataReader readerPPC;

                    oItem.SampleInvoiceID = oSampleInvoice.SampleInvoiceID;
                    oSampleInvoice.Amount = oSampleInvoice.Amount + oItem.Amount;

                        oItem.Amount = oItem.Qty * oItem.UnitPrice;
                        oSampleInvoice.Amount = oSampleInvoice.Amount + (oItem.Qty * oItem.UnitPrice);
                        if (oItem.DyeingOrderID > 0)
                        {
                            //SampleInvoiceDetailDA.UpdateInvoiceID(tc, oItem.SampleInvoiceID, oItem.DyeingOrderID);
                            readerPPC = SampleInvoiceDetailDA.InsertUpdate_AddDO(tc, oItem, EnumDBOperation.Insert, nUserID);
                            //NullHandler oReaderDetailPPC = new NullHandler(readerPPC);

                            readerPPC.Close();
                        }
                   
                }


                #endregion
                //IDataReader reader;
               
                //    if (oSampleInvoice.SampleInvoiceID <= 0)
                //    {
                //        reader = SampleInvoiceDA.InsertUpdate(tc, oSampleInvoice, EnumDBOperation.Insert, nUserID);
                //    }
                //    else
                //    {
                //        reader = SampleInvoiceDA.InsertUpdate(tc, oSampleInvoice, EnumDBOperation.Update, nUserID);
                //    }

                    IDataReader reader = SampleInvoiceDA.Get(tc, oSampleInvoice.SampleInvoiceID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSampleInvoice = new SampleInvoice();
                        oSampleInvoice = CreateObject(oReader);
                    }
                    reader.Close();
              
                
                tc.End();
            }
            catch (Exception e)
            {
                //#region Handle Exception
                //if (tc != null)
                //    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                //#endregion
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSampleInvoice.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Product. Because of " + e.Message, e);
                #endregion
            }
            return oSampleInvoice;
        }
        public SampleInvoice SaveFromOrder(SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);

                List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
                oSampleInvoiceDetails = oSampleInvoice.SampleInvoiceDetails;

                IDataReader reader;
                    if (oSampleInvoice.SampleInvoiceID <= 0)
                    {
                        reader = SampleInvoiceDA.InsertUpdate(tc, oSampleInvoice, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = SampleInvoiceDA.InsertUpdate(tc, oSampleInvoice, EnumDBOperation.Update, nUserID);
                    }
             
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = new SampleInvoice();
                    oSampleInvoice = CreateObject(oReader);
                }
                reader.Close();
                #region Purchase Payment Contract
                foreach (SampleInvoiceDetail oItem in oSampleInvoiceDetails)
                {
                    IDataReader readerPPC;

                    oItem.SampleInvoiceID = oSampleInvoice.SampleInvoiceID;
                    oSampleInvoice.Amount = oSampleInvoice.Amount + oItem.Amount;

                    oItem.Amount = oItem.Qty * oItem.UnitPrice;
                    oSampleInvoice.Amount = oSampleInvoice.Amount + (oItem.Qty * oItem.UnitPrice);
                    if (oItem.DyeingOrderID > 0)
                    {
                        SampleInvoiceDetailDA.UpdateInvoiceID(tc, oItem.SampleInvoiceID, oItem.DyeingOrderID);
                        //readerPPC = SampleInvoiceDetailDA.InsertUpdate_AddDO(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    if (oItem.SampleInvoiceDetailID <= 0)
                    {
                        readerPPC = SampleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerPPC = SampleInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }

                    NullHandler oReaderDetailPPC = new NullHandler(readerPPC);

                    readerPPC.Close();
                }


                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                //#region Handle Exception
                //if (tc != null)
                //    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                //#endregion
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSampleInvoice.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Product. Because of " + e.Message, e);
                #endregion
            }
            return oSampleInvoice;
        }
        public SampleInvoice UpdateSampleInvoice(SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);

                List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
                oSampleInvoiceDetails = oSampleInvoice.SampleInvoiceDetails;

                IDataReader reader;
                if (oSampleInvoice.SampleInvoiceID <= 0)
                {
                    reader = SampleInvoiceDA.UpdateSampleInvoice(tc, oSampleInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //VoucherDA.CheckVoucherReference(tc, "SampleInvoice", "SampleInvoiceID", oSampleInvoice.SampleInvoiceID);
                    reader = SampleInvoiceDA.UpdateSampleInvoice(tc, oSampleInvoice, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = new SampleInvoice();
                    oSampleInvoice = CreateObject(oReader);
                }
                reader.Close();
                
                tc.End();
            }
            catch (Exception e)
            {
                //#region Handle Exception
                //if (tc != null)
                //    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                //#endregion
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSampleInvoice.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Product. Because of " + e.Message, e);
                #endregion
            }
            return oSampleInvoice;
        }
        //
        public SampleInvoice UpdateSInvoiceNo(SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SampleInvoiceDA.UpdateSInvoiceNo(tc, oSampleInvoice, nUserID, EnumDBOperation.Insert);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = new SampleInvoice();
                    oSampleInvoice = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSampleInvoice.ErrorMessage = e.Message;
                oSampleInvoice.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSampleInvoice;
        }
        public SampleInvoice RemoveExportPIFromBill(SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                SampleInvoiceDA.RemoveExportPIFromBill(tc, oSampleInvoice, nUserID);
                tc.End();
                oSampleInvoice.ErrorMessage = Global.DeleteMessage;
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oSampleInvoice = new SampleInvoice();
                oSampleInvoice.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oSampleInvoice;
        }
   
        public SampleInvoice ExportPI_Attach(SampleInvoice oSampleInvoice, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceDA.ExportPI_Attach(tc, oSampleInvoice, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = CreateObject(oReader);
                }
                reader.Close();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oSampleInvoice = new SampleInvoice();
                    oSampleInvoice.ErrorMessage = Global.DeleteMessage;
                }
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oSampleInvoice = new SampleInvoice();
                oSampleInvoice.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oSampleInvoice;
        }
        public string Delete(SampleInvoice oSampleInvoice, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.DocCharge || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Qty || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Value || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Commission || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.ReturnAdjustment)
                {
                    SampleInvoiceDA.Delete_Adj(tc, oSampleInvoice, EnumDBOperation.Delete, nUserId);
                }
                else
                {
                    VoucherDA.CheckVoucherReference(tc, "SampleInvoice", "SampleInvoiceID", oSampleInvoice.SampleInvoiceID);
                    SampleInvoiceDA.Delete(tc, oSampleInvoice, EnumDBOperation.Delete, nUserId);
                }
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
        public SampleInvoice Get(int id, Int64 nUserId)
        {
            SampleInvoice oSampleInvoice = new SampleInvoice();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SampleInvoiceDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SampleInvoice", e);
                #endregion
            }

            return oSampleInvoice;
        }
        public SampleInvoice Get(string sSampleInvoiceNo, Int64 nUserId)
        {
            SampleInvoice oSampleInvoice = new SampleInvoice();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SampleInvoiceDA.Get(tc, sSampleInvoiceNo);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SampleInvoice", e);
                #endregion
            }

            return oSampleInvoice;
        }
        public List<SampleInvoice> Gets( Int64 nUserId)
        {
            List<SampleInvoice> oSampleInvoices = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceDA.Gets(tc);
                oSampleInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoices", e);
                #endregion
            }

            return oSampleInvoices;
        }
        public List<SampleInvoice> Gets(string nSQL, Int64 nUserId)
        {
            List<SampleInvoice> oSampleInvoices = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceDA.Gets(tc, nSQL);
                oSampleInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoices", e);
                #endregion
            }

            return oSampleInvoices;
        }
        public List<SampleInvoice> GetsForPayment(string nSQL, Int64 nUserId)
        {
            List<SampleInvoice> oSampleInvoices = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                string sTemp = "";
              
                IDataReader reader = null;
                reader = SampleInvoiceDA.Gets(tc, nSQL);
                oSampleInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoices", e);
                #endregion
            }

            return oSampleInvoices;
        }
        public List<SampleInvoice> Gets(string sContractorIDs, int ePaymentType, Int64 nUserId)
        {
            List<SampleInvoice> oSampleInvoices = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceDA.Gets(tc, sContractorIDs, ePaymentType);
                oSampleInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoices", e);
                #endregion
            }

            return oSampleInvoices;
        }
        public List<SampleInvoice> GetsbyNo(string sSampleInvoiceNo, int ePaymentType, Int64 nUserId)
        {
            List<SampleInvoice> oSampleInvoices = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceDA.GetsByNo(tc, sSampleInvoiceNo, ePaymentType);
                oSampleInvoices = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoices", e);
                #endregion
            }

            return oSampleInvoices;
        }
        public SampleInvoice Approve(SampleInvoice oSampleInvoice, Int64 nUserID)
        {
            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                if (oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.DocCharge || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Qty || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Value || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Commission || oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.ReturnAdjustment)
                {

                    reader = SampleInvoiceDA.InsertUpdate_Adj(tc, oSampleInvoice, EnumDBOperation.Approval, nUserID);
                }
                else
                {
                    reader = SampleInvoiceDA.InsertUpdate(tc, oSampleInvoice, EnumDBOperation.Approval, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get SampleInvoice", e);
                oSampleInvoice.ErrorMessage = e.Message;
                #endregion
            }

            return oSampleInvoice;
        }
        public SampleInvoice Cancel(SampleInvoice oSampleInvoice, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = SampleInvoiceDA.InsertUpdate(tc, oSampleInvoice, EnumDBOperation.Cancel, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoice = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get SampleInvoice", e);
                oSampleInvoice.ErrorMessage = e.Message;
                #endregion
            }

            return oSampleInvoice;
        }
        public List<SampleInvoice> ExportPISNA(int nExportPI, Int64 nUserID)
        {
            SampleInvoice oSampleInvoice = new SampleInvoice();
            List<SampleInvoice> oSampleInvoices_Return = new List<SampleInvoice>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = SampleInvoiceDA.ExportPISNA(tc, nExportPI, nUserID);
                oSampleInvoices_Return = CreateObjects(reader);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSampleInvoice = new SampleInvoice();
                oSampleInvoice.ErrorMessage = e.Message.Split('~')[0];
                oSampleInvoices_Return.Add(oSampleInvoice);

                #endregion
            }
            return oSampleInvoices_Return;
        }

        #endregion
    }
}
