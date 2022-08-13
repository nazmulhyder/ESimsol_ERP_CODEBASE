using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class DevelopmentRecapService : MarshalByRefObject, IDevelopmentRecapService
    {
        #region Private functions and declaration
        private DevelopmentRecap MapObject(NullHandler oReader)
        {
            DevelopmentRecap oDevelopmentRecap = new DevelopmentRecap();
            oDevelopmentRecap.DevelopmentRecapID = oReader.GetInt32("DevelopmentRecapID");
            oDevelopmentRecap.BUID = oReader.GetInt32("BUID");
            oDevelopmentRecap.BusinessSessionID = oReader.GetInt32("BusinessSessionID");
            oDevelopmentRecap.SessionName = oReader.GetString("SessionName");
            oDevelopmentRecap.DevelopmentRecapNo = oReader.GetString("DevelopmentRecapNo");
            oDevelopmentRecap.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oDevelopmentRecap.DevelopmentStatus = (EnumDevelopmentStatus)oReader.GetInt32("DevelopmentStatus");
            oDevelopmentRecap.InquiryReceivedDate = oReader.GetDateTime("InquiryReceivedDate");
            oDevelopmentRecap.GG = oReader.GetString("GG");
            oDevelopmentRecap.SampleQty = oReader.GetInt32("SampleQty");
            oDevelopmentRecap.SampleSizeID = oReader.GetInt32("SampleSizeID");
            oDevelopmentRecap.SampleReceivedDate = oReader.GetDateTime("SampleReceivedDate");
            oDevelopmentRecap.SampleSendingDate = oReader.GetDateTime("SampleSendingDate");
            oDevelopmentRecap.SendingDeadLine = oReader.GetDateTime("SendingDeadLine");
            oDevelopmentRecap.AwbNo = oReader.GetString("AwbNo");
            oDevelopmentRecap.Remarks = oReader.GetString("Remarks");
            oDevelopmentRecap.SpecialFinish = oReader.GetString("SpecialFinish");
            oDevelopmentRecap.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oDevelopmentRecap.TransportType = (EnumTransportType)oReader.GetInt32("TransportType");
            oDevelopmentRecap.YarnCategoryID = oReader.GetInt32("YarnCategoryID");
            oDevelopmentRecap.UnitID = oReader.GetInt32("UnitID");
            oDevelopmentRecap.Weight = oReader.GetString("Weight");
            oDevelopmentRecap.UnitName = oReader.GetString("UnitName");
            oDevelopmentRecap.BrandName = oReader.GetString("BrandName");
            oDevelopmentRecap.MerchandiserName = oReader.GetString("MerchandiserName");
            oDevelopmentRecap.YarnName = oReader.GetString("YarnName");
            oDevelopmentRecap.CollectionName = oReader.GetString("CollectionName");
            oDevelopmentRecap.StyleNo = oReader.GetString("StyleNo");
            oDevelopmentRecap.BuyerID = oReader.GetInt32("BuyerID");
            oDevelopmentRecap.BuyerContactPersonID = oReader.GetInt32("BuyerContactPersonID");
            oDevelopmentRecap.SampleSize = oReader.GetString("SampleSize");
            oDevelopmentRecap.BuyerName = oReader.GetString("BuyerName");
            oDevelopmentRecap.BuerContactPersonName = oReader.GetString("BuerContactPersonName"); 
            oDevelopmentRecap.PrepareBy = oReader.GetString("PrepareBy");
            oDevelopmentRecap.ProductID = oReader.GetInt32("ProductID");
            oDevelopmentRecap.ProductName = oReader.GetString("ProductName");
            oDevelopmentRecap.Count = oReader.GetString("Count");
            oDevelopmentRecap.DevelopmentType = oReader.GetInt32("DevelopmentType");
            oDevelopmentRecap.DevelopmentTypeName = oReader.GetString("DevelopmentTypeName");
            oDevelopmentRecap.OrderRecapQty = oReader.GetDouble("OrderRecapQty");
            oDevelopmentRecap.IsActive = oReader.GetBoolean("IsActive");
            oDevelopmentRecap.UnitPrice = oReader.GetDouble("UnitPrice");
            oDevelopmentRecap.CurrencyID = oReader.GetInt32("CurrencyID");
            oDevelopmentRecap.CurrencyName = oReader.GetString("CurrencyName");
            oDevelopmentRecap.CurrencySymbol = oReader.GetString("CurrencySymbol");
            return oDevelopmentRecap;
        }

        private DevelopmentRecap CreateObject(NullHandler oReader)
        {
            DevelopmentRecap oDevelopmentRecap = new DevelopmentRecap();
            oDevelopmentRecap = MapObject(oReader);
            return oDevelopmentRecap;
        }

        private List<DevelopmentRecap> CreateObjects(IDataReader oReader)
        {
            List<DevelopmentRecap> oDevelopmentRecaps = new List<DevelopmentRecap>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DevelopmentRecap oItem = CreateObject(oHandler);
                oDevelopmentRecaps.Add(oItem);
            }
            return oDevelopmentRecaps;
        }

        #endregion

        #region Interface implementation
        public DevelopmentRecapService() { }

        public DevelopmentRecap Save(DevelopmentRecap oDevelopmentRecap, Int64 nUserID)
        {

            #region Declartion
            List<DevelopmentRecapDetail> oDevelopmentRecapDetails = new List<DevelopmentRecapDetail>();
            DevelopmentRecapDetail oDevelopmentRecapDetail = new DevelopmentRecapDetail();
            List<DevelopmentRecapSizeColorRatio> oDevelopmentRecapSizeColorRatios = new List<DevelopmentRecapSizeColorRatio>();
            DevelopmentRecapSizeColorRatio oDevelopmentRecapSizeColorRatio = new DevelopmentRecapSizeColorRatio();
            List<DevelopmentYarnOption> oDevelopmentYarnOptions = new List<DevelopmentYarnOption>();
            DevelopmentYarnOption oDevelopmentYarnOption = new DevelopmentYarnOption();
            string sDevelopmentRecapDetailIDs = "";
            string sDevelopmentRecapSizeColorRatioIDs = "";
            string sDevelopmentYarnOptionIDs = "";
            DateTime dInqureiReceiveDate = oDevelopmentRecap.InquiryReceivedDate;
            DateTime dSampleReceivedDate = oDevelopmentRecap.SampleReceivedDate;
            DateTime dSampleSendingDate = oDevelopmentRecap.SampleSendingDate;
            DateTime dSendingDeadLine = oDevelopmentRecap.SendingDeadLine;

            oDevelopmentRecapDetails = oDevelopmentRecap.DevelopmentRecapDetails;
            oDevelopmentYarnOptions = oDevelopmentRecap.DevelopmentYarnOptions;
            
            
            # endregion

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                # region Development Recap
                IDataReader reader;
                if (oDevelopmentRecap.DevelopmentRecapID <= 0)
                {
                    reader = DevelopmentRecapDA.InsertUpdate(tc, oDevelopmentRecap, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DevelopmentRecap, EnumRoleOperationType.Edit);
                    reader = DevelopmentRecapDA.InsertUpdate(tc, oDevelopmentRecap, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentRecap = new DevelopmentRecap();
                    oDevelopmentRecap = CreateObject(oReader);

                }
                reader.Close();
                if (dInqureiReceiveDate != DateTime.MinValue)
                {
                    DevelopmentRecapDA.UpdateInqRcvDate(tc, oDevelopmentRecap.DevelopmentRecapID, dInqureiReceiveDate, nUserID);
                }
                if (dSampleSendingDate != DateTime.MinValue)
                {
                    DevelopmentRecapDA.UpdateSmplSendingDate(tc, oDevelopmentRecap.DevelopmentRecapID, dSampleSendingDate, nUserID);
                }
                if (dSampleReceivedDate != DateTime.MinValue)
                {
                    DevelopmentRecapDA.UpdateSmplRcvDate(tc, oDevelopmentRecap.DevelopmentRecapID, dSampleReceivedDate, nUserID);
                }
                if (dSendingDeadLine != DateTime.MinValue)
                {
                    DevelopmentRecapDA.UpdateSendingDeadLine(tc, oDevelopmentRecap.DevelopmentRecapID, dSendingDeadLine, nUserID);
                }
                # endregion Development Recap

                #region Development Recap Detail
                if (oDevelopmentRecap.DevelopmentRecapID > 0)
                {
                    if (oDevelopmentRecapDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (DevelopmentRecapDetail oDRD in oDevelopmentRecapDetails)
                        {
                            oDRD.DevelopmentRecapID = oDevelopmentRecap.DevelopmentRecapID;
                            if (oDRD.DevelopmentRecapDetailID <= 0)
                            {
                                readerdetail = DevelopmentRecapDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                readerdetail = DevelopmentRecapDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID);

                            }

                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);

                            int nDevelopmentRecapDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nDevelopmentRecapDetailID = oReaderDevRecapdetail.GetInt32("DevelopmentRecapDetailID");
                                sDevelopmentRecapDetailIDs = sDevelopmentRecapDetailIDs + oReaderDevRecapdetail.GetString("DevelopmentRecapDetailID") + ",";
                            }
                            readerdetail.Close();

                            #region DevelopmentRecap Size Color Ratio
                            if (oDRD.DevelopmentRecapSizeColorRatios.Count>0)
                            {
                                foreach (DevelopmentRecapSizeColorRatio oItem in oDRD.DevelopmentRecapSizeColorRatios)
                                {
                                    IDataReader readerSizeColor;
                                    oItem.DevelopmentRecapDetailID = nDevelopmentRecapDetailID;
                                    if (oItem.DevelopmentRecapSizeColorRatioID <= 0)
                                    {
                                        readerSizeColor = DevelopmentRecapSizeColorRatioDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                                    }
                                    else
                                    {
                                        readerSizeColor = DevelopmentRecapSizeColorRatioDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                                    }
                                    NullHandler oReaderSizeColorRatio = new NullHandler(readerSizeColor);
                                    if (readerSizeColor.Read())
                                    {
                                        sDevelopmentRecapSizeColorRatioIDs = sDevelopmentRecapSizeColorRatioIDs + oReaderSizeColorRatio.GetString("DevelopmentRecapSizeColorRatioID") + ",";
                                    }
                                    readerSizeColor.Close();
                                }
                                if (sDevelopmentRecapSizeColorRatioIDs.Length > 0)
                                {
                                    sDevelopmentRecapSizeColorRatioIDs = sDevelopmentRecapSizeColorRatioIDs.Remove(sDevelopmentRecapSizeColorRatioIDs.Length - 1, 1);
                                }
                                oDevelopmentRecapSizeColorRatio = new DevelopmentRecapSizeColorRatio();
                                oDevelopmentRecapSizeColorRatio.DevelopmentRecapDetailID = nDevelopmentRecapDetailID;
                                DevelopmentRecapSizeColorRatioDA.Delete(tc, oDevelopmentRecapSizeColorRatio, EnumDBOperation.Delete, nUserID, sDevelopmentRecapSizeColorRatioIDs);
                                sDevelopmentRecapSizeColorRatioIDs = "";
                            }



                            #endregion DevelopmentRecap Sise Color ratio
                            
                        }
                    }
                    if (sDevelopmentRecapDetailIDs.Length > 0)
                    {
                        sDevelopmentRecapDetailIDs = sDevelopmentRecapDetailIDs.Remove(sDevelopmentRecapDetailIDs.Length - 1, 1);
                    }
                    oDevelopmentRecapDetail = new DevelopmentRecapDetail();
                    oDevelopmentRecapDetail.DevelopmentRecapID = oDevelopmentRecap.DevelopmentRecapID;
                    DevelopmentRecapDetailDA.Delete(tc, oDevelopmentRecapDetail, EnumDBOperation.Delete, nUserID, sDevelopmentRecapDetailIDs);
               

                }
                #endregion

                #region Developement yarn Part

                if (oDevelopmentYarnOptions != null)
                {
                    foreach (DevelopmentYarnOption oItem in oDevelopmentYarnOptions)
                    {
                        IDataReader readerYarnOption;
                        oItem.DevelopmentRecapID =oDevelopmentRecap.DevelopmentRecapID;
                        if (oItem.DevelopmentYarnOptionID <= 0)
                        {
                            readerYarnOption = DevelopmentYarnOptionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerYarnOption = DevelopmentYarnOptionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderYarnOption = new NullHandler(readerYarnOption);
                        if (readerYarnOption.Read())
                        {
                            sDevelopmentYarnOptionIDs = sDevelopmentYarnOptionIDs + oReaderYarnOption.GetString("DevelopmentYarnOptionID") + ",";
                        }
                        readerYarnOption.Close();
                    }
                    if (sDevelopmentYarnOptionIDs.Length > 0)
                    {
                        sDevelopmentYarnOptionIDs = sDevelopmentYarnOptionIDs.Remove(sDevelopmentYarnOptionIDs.Length - 1, 1);
                    }
                    oDevelopmentYarnOption = new DevelopmentYarnOption();
                    oDevelopmentYarnOption.DevelopmentRecapID=oDevelopmentRecap.DevelopmentRecapID;
                    DevelopmentYarnOptionDA.Delete(tc, oDevelopmentYarnOption, EnumDBOperation.Delete, nUserID, sDevelopmentYarnOptionIDs);

                }

                #endregion Developmentyarn Part

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDevelopmentRecap = new DevelopmentRecap();
                oDevelopmentRecap.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oDevelopmentRecap;
        }

        # region  SaveInProduction

        public DevelopmentRecap SaveInProduction(DevelopmentRecap oDevelopmentRecap, Int64 nUserID)
        {
            //  int nProductionOrderID = oDevelopmentRecap.ProductionOrder.ProductionOrderID;
            List<ProductionOrder> _oProductionOrders = new List<ProductionOrder>();
            _oProductionOrders = oDevelopmentRecap.ProductionOrders;

            TransactionContext tc = null;
            DevelopmentRecap _oDevelopmentRecap = new DevelopmentRecap();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                foreach (ProductionOrder OItem in _oProductionOrders)
                {
                    reader = ProductionOrderDA.DevelopmentRecapSendToProducton(tc, OItem.ProductionOrderID, nUserID);
                    NullHandler OReaderProdeuctionOrder = new NullHandler(reader);
                    int nProdcutionOrder = 0;
                    if (reader.Read())
                    {
                        nProdcutionOrder = OReaderProdeuctionOrder.GetInt32("ProductionOrderID");
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

                _oDevelopmentRecap = new DevelopmentRecap();
                _oDevelopmentRecap.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return _oDevelopmentRecap;

        }

        #endregion SaveInProduction

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DevelopmentRecap oDevelopmentRecap = new DevelopmentRecap();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.DevelopmentRecap, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "DevelopmentRecap", id);
                oDevelopmentRecap.DevelopmentRecapID = id;
                DevelopmentRecapDA.Delete(tc, oDevelopmentRecap, EnumDBOperation.Delete, nUserId);
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
            return "Data delete successfully";
        }

        public DevelopmentRecap Get(int id, Int64 nUserId)
        {
            DevelopmentRecap oAccountHead = new DevelopmentRecap();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DevelopmentRecapDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get DevelopmentRecap", e);
                #endregion
            }

            return oAccountHead;
        }

        public DevelopmentRecap ActiveInActive(int id, bool bIsActive, Int64 nUserId)
        {
            DevelopmentRecap oAccountHead = new DevelopmentRecap();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                DevelopmentRecapDA.ActiveInActive(tc, id, bIsActive, nUserId);
                IDataReader reader = DevelopmentRecapDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get DevelopmentRecap", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DevelopmentRecap> GetsByTechnicalSheet(int nTechnicalSheetID, Int64 nUserId)
        {
            List<DevelopmentRecap> oDevelopmentRecaps = new List<DevelopmentRecap>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DevelopmentRecapDA.GetsByTechnicalSheet(tc, nTechnicalSheetID);
                oDevelopmentRecaps = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecap", e);
                #endregion
            }

            return oDevelopmentRecaps;
        }

        

        public DevelopmentRecap UpdateInqRcvDate(int id, DateTime dInqRcvDate, Int64 nUserID)
        {
            DevelopmentRecap oDevelopmentRecap = new DevelopmentRecap();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                DevelopmentRecapDA.UpdateInqRcvDate(tc, id, dInqRcvDate, nUserID);
                IDataReader reader = DevelopmentRecapDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentRecap = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDevelopmentRecap = new DevelopmentRecap();
                oDevelopmentRecap.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Update Sample Sending Date", e);
                #endregion
            }

            return oDevelopmentRecap;
        }

        public DevelopmentRecap UpdateSmplRcvDate(int id, DateTime dSmplRcvDate, Int64 nUserID)
        {
            DevelopmentRecap oDevelopmentRecap = new DevelopmentRecap();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                DevelopmentRecapDA.UpdateSmplRcvDate(tc, id, dSmplRcvDate, nUserID);
                IDataReader reader = DevelopmentRecapDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentRecap = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDevelopmentRecap = new DevelopmentRecap();
                oDevelopmentRecap.ErrorMessage = e.Message;

                #endregion
            }

            return oDevelopmentRecap;
        }

        public DevelopmentRecap UpdateSmplSendingDate(int id, DateTime dSmplSendingDate, Int64 nUserID)
        {
            DevelopmentRecap oDevelopmentRecap = new DevelopmentRecap();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                DevelopmentRecapDA.UpdateSmplSendingDate(tc, id, dSmplSendingDate, nUserID);
                IDataReader reader = DevelopmentRecapDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentRecap = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDevelopmentRecap = new DevelopmentRecap();
                oDevelopmentRecap.ErrorMessage = e.Message;

                #endregion
            }

            return oDevelopmentRecap;
        }

        public DevelopmentRecap UpdateSendingDeadLine(int id, DateTime dSendingDeadLine, Int64 nUserID)
        {
            DevelopmentRecap oDevelopmentRecap = new DevelopmentRecap();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                DevelopmentRecapDA.UpdateSendingDeadLine(tc, id, dSendingDeadLine, nUserID);
                IDataReader reader = DevelopmentRecapDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentRecap = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDevelopmentRecap = new DevelopmentRecap();
                oDevelopmentRecap.ErrorMessage = e.Message;

                #endregion
            }

            return oDevelopmentRecap;
        }

        public List<DevelopmentRecap> GetsRecapWithRowNumber(int nStartRow, int nEndRow, Int64 nUserID)
        {
            List<DevelopmentRecap> oDevelopmentRecap = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapDA.GetsRecapWithRowNumber(tc, nStartRow, nEndRow);
                oDevelopmentRecap = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecap", e);
                #endregion
            }

            return oDevelopmentRecap;
        }

        public List<DevelopmentRecap> GetsRecapWithDevelopmentRecaps(int nStartRow, int nEndRow, string sDevelopmentRecapIDs, bool bIsPrint, Int64 nUserID)
        {
            List<DevelopmentRecap> oDevelopmentRecap = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapDA.GetsRecapWithDevelopmentRecaps(tc, nStartRow, nEndRow, sDevelopmentRecapIDs, bIsPrint);
                oDevelopmentRecap = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecap", e);
                #endregion
            }

            return oDevelopmentRecap;
        }

        public List<DevelopmentRecap> Gets(Int64 nUserID)
        {
            List<DevelopmentRecap> oDevelopmentRecap = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapDA.Gets(tc);
                oDevelopmentRecap = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecap", e);
                #endregion
            }

            return oDevelopmentRecap;
        }

        public List<DevelopmentRecap> Gets(string sSql, Int64 nUserID)
        {
            List<DevelopmentRecap> oDevelopmentRecap = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapDA.Gets(tc, sSql);
                oDevelopmentRecap = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecap", e);
                #endregion
            }

            return oDevelopmentRecap;
        }

        public List<DevelopmentRecap> Gets_Report(int id, Int64 nUserID)
        {
            List<DevelopmentRecap> oDevelopmentRecap = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DevelopmentRecapDA.Gets_Report(tc, id);
                oDevelopmentRecap = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheet", e);
                #endregion
            }

            return oDevelopmentRecap;
        }

        public DevelopmentRecap ChangeStatus(DevelopmentRecap oDevelopmentRecap, ApprovalRequest oApprovalRequest, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDevelopmentRecap.ActionType == EnumDevelopmentRecapActionType.ApprovedDone)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DevelopmentRecap, EnumRoleOperationType.Approved); //For checikng Approved
                }
                if (oDevelopmentRecap.ActionType == EnumDevelopmentRecapActionType.Cancel)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DevelopmentRecap, EnumRoleOperationType.Cancel); // For Cheak Cancel
                }
                reader = DevelopmentRecapDA.ChangeStatus(tc, oDevelopmentRecap, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentRecap = new DevelopmentRecap();
                    oDevelopmentRecap = CreateObject(oReader);
                }
                reader.Close();

                if (oDevelopmentRecap.DevelopmentStatus == EnumDevelopmentStatus.RequestForApproved)
                {
                    IDataReader ApprovalRequestreader;
                    ApprovalRequestreader = ApprovalRequestDA.InsertUpdate(tc, oApprovalRequest, EnumDBOperation.Insert, nUserID);
                    if (ApprovalRequestreader.Read())
                    {

                    }
                    ApprovalRequestreader.Close();
                }

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
                oDevelopmentRecap.ErrorMessage = Message;
                #endregion
            }
            return oDevelopmentRecap;
        }

        #endregion Interface implementation
    }
}
