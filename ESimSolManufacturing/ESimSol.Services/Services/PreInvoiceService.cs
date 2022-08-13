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
    public class PreInvoiceService : MarshalByRefObject, IPreInvoiceService
    {
        #region Private functions and declaration
        private PreInvoice MapObject(NullHandler oReader)
        {
            PreInvoice oPreInvoice = new PreInvoice();
            oPreInvoice.PreInvoiceID = oReader.GetInt32("PreInvoiceID");
            oPreInvoice.InvoiceNo = oReader.GetString("InvoiceNo");
            oPreInvoice.SQNo = oReader.GetString("SQNo");
            oPreInvoice.KommNo = oReader.GetString("KommNo");
            oPreInvoice.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oPreInvoice.ContractorID = oReader.GetInt32("ContractorID");
            oPreInvoice.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oPreInvoice.SalesQuotationID = oReader.GetInt32("SalesQuotationID");
            oPreInvoice.IsNewOrder = oReader.GetBoolean("IsNewOrder");
            oPreInvoice.VehicleLocation = oReader.GetInt32("VehicleLocation");
            oPreInvoice.PRNo = oReader.GetString("PRNo");
            oPreInvoice.SpecialInstruction = oReader.GetString("SpecialInstruction");
            oPreInvoice.ETAAgreement = oReader.GetString("ETAAgreement");
            oPreInvoice.ETAWeeks = oReader.GetString("ETAWeeks");
            oPreInvoice.CurrencyID = oReader.GetInt32("CurrencyID");
            oPreInvoice.OTRAmount = oReader.GetDouble("OTRAmount");
            oPreInvoice.VatAmount = oReader.GetDouble("VatAmount");
            oPreInvoice.RegistrationFee = oReader.GetDouble("RegistrationFee");
            oPreInvoice.TDSAmount = oReader.GetDouble("TDSAmount");
            oPreInvoice.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oPreInvoice.NetAmount = oReader.GetDouble("NetAmount");
            oPreInvoice.AdvanceAmount = oReader.GetDouble("AdvanceAmount");
            oPreInvoice.OTRAmount = oReader.GetDouble("OTRAmount");
            oPreInvoice.AdvanceDate = oReader.GetDateTime("AdvanceDate");
            oPreInvoice.PaymentMode = oReader.GetInt32("PaymentMode");
            oPreInvoice.BankID = oReader.GetInt32("BankID");
            oPreInvoice.BankName = oReader.GetString("BankName");
            oPreInvoice.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oPreInvoice.AttachmentDoc = oReader.GetInt32("AttachmentDoc");
            oPreInvoice.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oPreInvoice.SalesQuotationImageID = oReader.GetInt32("SalesQuotationImageID");
            oPreInvoice.PreInvoiceID = oReader.GetInt32("PreInvoiceID");
            oPreInvoice.BUID = oReader.GetInt32("BUID");
            oPreInvoice.InvoiceStatus = (EnumPreInvoiceStatus)oReader.GetInt32("InvoiceStatus");
            oPreInvoice.ChequeNo = oReader.GetString("ChequeNo");
            oPreInvoice.ChequeDate = oReader.GetDateTime("ChequeDate");
            oPreInvoice.ReceivedByName = oReader.GetString("ReceivedByName");
            oPreInvoice.InteriorColorName = oReader.GetString("InteriorColorName");
            oPreInvoice.ExteriorColorName = oReader.GetString("ExteriorColorName");
            oPreInvoice.EngineNo = oReader.GetString("EngineNo");
            oPreInvoice.ChassisNo = oReader.GetString("ChassisNo");
            oPreInvoice.ModelNo = oReader.GetString("ModelNo");
            oPreInvoice.CustomerName = oReader.GetString("CustomerName");
            oPreInvoice.CustomerShortName = oReader.GetString("CustomerShortName");
            oPreInvoice.CustomerAddress = oReader.GetString("CustomerAddress");
            oPreInvoice.CustomerCity = oReader.GetString("CustomerCity");
            oPreInvoice.CustomerLandline = oReader.GetString("CustomerLandline");
            oPreInvoice.CustomerCellPhone = oReader.GetString("CustomerCellPhone");
            oPreInvoice.CustomerEmail = oReader.GetString("CustomerEmail");
            oPreInvoice.CurrencyName = oReader.GetString("CurrencyName");
            oPreInvoice.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPreInvoice.ReceivedOn = oReader.GetString("ReceivedOn");
            oPreInvoice.ApprovedByName = oReader.GetString("ApprovedByName");
            oPreInvoice.Remarks = oReader.GetString("Remarks");
            oPreInvoice.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oPreInvoice.MarketingAccountID = oReader.GetInt32("MarketingAccountID");
            oPreInvoice.MarketingAccountName = oReader.GetString("MarketingAccountName");
            oPreInvoice.AdvanceDate = oReader.GetDateTime("AdvanceDate");
            oPreInvoice.CRate = oReader.GetDouble("CRate");
            oPreInvoice.ProductID = oReader.GetInt32("ProductID");
            oPreInvoice.ProductName = oReader.GetString("ProductName");
            oPreInvoice.Phone = oReader.GetString("Phone");
            oPreInvoice.Email = oReader.GetString("Email");

            oPreInvoice.HandoverDate = oReader.GetDateTime("HandoverDate");
            oPreInvoice.VehicleMileage = oReader.GetInt32("VehicleMileage");
            oPreInvoice.WheelCondition = oReader.GetString("WheelCondition");
            oPreInvoice.BodyWorkCondition = oReader.GetString("BodyWorkCondition");
            oPreInvoice.InteriorCondition = oReader.GetString("InteriorCondition");
            oPreInvoice.DealerPerson = oReader.GetString("DealerPerson");
            oPreInvoice.Owner = oReader.GetString("Owner");
            oPreInvoice.OwnerNID = oReader.GetString("OwnerNID");
            oPreInvoice.Note = oReader.GetString("Note");
            oPreInvoice.YearOfManufacture = oReader.GetString("YearOfManufacture");
            oPreInvoice.YearOfModel = oReader.GetString("YearOfModel");
            oPreInvoice.ModelShortName = oReader.GetString("ModelShortName");
            oPreInvoice.ETAValue = oReader.GetInt32("ETAValue");
            oPreInvoice.ETAType = (EnumDisplayPart)oReader.GetInt32("ETAType");
            oPreInvoice.IssueDate = oReader.GetDateTime("IssueDate");
            oPreInvoice.OfferedFreeService = oReader.GetInt32("OfferedFreeService");

            return oPreInvoice;
        }

        private PreInvoice CreateObject(NullHandler oReader)
        {
            PreInvoice oPreInvoice = new PreInvoice();
            oPreInvoice = MapObject(oReader);
            return oPreInvoice;
        }

        private List<PreInvoice> CreateObjects(IDataReader oReader)
        {
            List<PreInvoice> oPreInvoice = new List<PreInvoice>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PreInvoice oItem = CreateObject(oHandler);
                oPreInvoice.Add(oItem);
            }
            return oPreInvoice;
        }

        #endregion

        #region Interface implementation
        public PreInvoiceService() { }

        public PreInvoice Save(PreInvoice oPreInvoice, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPreInvoice.PreInvoiceID <= 0)
                {
                    reader = PreInvoiceDA.InsertUpdate(tc, oPreInvoice, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = PreInvoiceDA.InsertUpdate(tc, oPreInvoice, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPreInvoice = new PreInvoice();
                    oPreInvoice = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save PreInvoice. Because of " + e.Message, e);
                #endregion
            }
            return oPreInvoice;
        }
        public PreInvoice SaveAll(List<ServiceSchedule> oServiceSchedules, int nPreInvoiceID, Int64 nUserID)
        {
            TransactionContext tc = null;
            PreInvoice oPreInvoice = new PreInvoice();
            List<PreInvoice> oPreInvoiceList = new List<PreInvoice>();
            try
            {
                if (nPreInvoiceID > 0)
                {
                    tc = TransactionContext.Begin(true);
                    string ids = "";
                    foreach (ServiceSchedule item in oServiceSchedules)
                    {
                        if (item.ServiceScheduleID > 0)
                        {
                            ids = ids + item.ServiceScheduleID + ",";
                        }
                    }
                    if (ids.Length > 0) ids = ids.Remove(ids.Length - 1);
                    ServiceScheduleDA.DeleteList(tc, ids, nPreInvoiceID, nUserID);
                    IDataReader reader;
                    if (oServiceSchedules.Count > 0)
                    {

                        foreach (ServiceSchedule oItem in oServiceSchedules)
                        {
                            oItem.PreInvoiceID = nPreInvoiceID;
                            if (oItem.ServiceScheduleID <= 0)
                            {
                                reader = ServiceScheduleDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                reader = ServiceScheduleDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            }
                            NullHandler oReader = new NullHandler(reader);
                            if (reader.Read())
                            {

                            }
                            reader.Close();
                        }
                    }
                    tc.End();
                    oPreInvoice.ErrorMessage = "";
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oPreInvoice = new PreInvoice();
                    oPreInvoice.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oPreInvoice;
        }
        public string UpdateStatus(PreInvoice oPreInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);

                if (oPreInvoice.nRequest == 2)//Approve
                {
                    sMessage = "Approved";
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PreInvoice, EnumRoleOperationType.Approved);
                    DBTableReferenceDA.HasReference(tc, "PreInvoice", oPreInvoice.PreInvoiceID);

                    oPreInvoice.InvoiceStatus = EnumPreInvoiceStatus.Approved;
                    PreInvoiceDA.UpdateStatus(tc, oPreInvoice, EnumDBOperation.Approval, nUserID);
                }
                //else if (oPreInvoice.nRequest == 2)//Received
                //{
                //    sMessage = "Received";
                //    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PreInvoice, EnumRoleOperationType.Received);
                //    DBTableReferenceDA.HasReference(tc, "PreInvoice", oPreInvoice.PreInvoiceID);

                //    oPreInvoice.OrderStatus = EnumPreInvoiceStatus.Received;
                //    PreInvoiceDA.UpdateStatus(tc, oPreInvoice, EnumDBOperation.Receive, nUserID);
                //}
                //else if (oPreInvoice.nRequest == 3)//Deliverd
                //{
                //    sMessage = "Delivered";
                //    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PreInvoice, EnumRoleOperationType.DeliverToParty);
                //    DBTableReferenceDA.HasReference(tc, "PreInvoice", oPreInvoice.PreInvoiceID);

                //    oPreInvoice.OrderStatus = EnumPreInvoiceStatus.Done;
                //    PreInvoiceDA.UpdateStatus(tc, oPreInvoice, EnumDBOperation.Delivered, nUserID);
                //}
                //else if (oPreInvoice.nRequest == 4)//Cancel
                //{
                //    sMessage = "Cancel";
                //    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PreInvoice, EnumRoleOperationType.Cancel);
                //    DBTableReferenceDA.HasReference(tc, "PreInvoice", oPreInvoice.PreInvoiceID);

                //    oPreInvoice.OrderStatus = EnumPreInvoiceStatus.Canceled;
                //    PreInvoiceDA.UpdateStatus(tc, oPreInvoice, EnumDBOperation.Cancel, nUserID);
                //}
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    return e.Message.Split('!')[0];
                }
                #endregion
            }
            return sMessage;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PreInvoice oPreInvoice = new PreInvoice();
                oPreInvoice.PreInvoiceID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.PreInvoice, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "PreInvoice", id);
                PreInvoiceDA.Delete(tc, oPreInvoice, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Deleted";
        }

        public PreInvoice Get(int id, Int64 nUserId)
        {
            PreInvoice oAccountHead = new PreInvoice();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PreInvoiceDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get PreInvoice", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PreInvoice> Gets(Int64 nUserId)
        {
            List<PreInvoice> oPreInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PreInvoiceDA.Gets(tc);
                oPreInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PreInvoice", e);
                #endregion
            }

            return oPreInvoice;
        }


        public List<PreInvoice> Gets(string sSQL, Int64 nUserId)
        {
            List<PreInvoice> oPreInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PreInvoiceDA.Gets(tc, sSQL);
                oPreInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PreInvoice", e);
                #endregion
            }

            return oPreInvoice;
        }

        public PreInvoice UpdateForHandoverCheckList(PreInvoice oPreInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                PreInvoiceDA.UpdateForHandoverCheckList(tc, oPreInvoice);
                reader = PreInvoiceDA.Get(tc, oPreInvoice.PreInvoiceID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPreInvoice = new PreInvoice();
                    oPreInvoice = CreateObject(oReader);
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
                Message = Message.Split('!')[0];
                oPreInvoice.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PreInvoiceDetail. Because of " + e.Message, e);
                #endregion
            }
            return oPreInvoice;
        }

        #endregion
    }
}