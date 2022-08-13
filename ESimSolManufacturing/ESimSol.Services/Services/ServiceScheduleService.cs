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
    public class ServiceScheduleService : MarshalByRefObject, IServiceScheduleService
    {
        #region Private functions and declaration
        private ServiceSchedule MapObject(NullHandler oReader)
        {
            ServiceSchedule oServiceSchedule = new ServiceSchedule();
            oServiceSchedule.ServiceScheduleID = oReader.GetInt32("ServiceScheduleID");
            oServiceSchedule.PreInvoiceID = oReader.GetInt32("PreInvoiceID");
            oServiceSchedule.ServiceDate = oReader.GetDateTime("ServiceDate");
            
            oServiceSchedule.ChargeType = (EnumServiceILaborChargeType)oReader.GetInt16("ChargeType");
            oServiceSchedule.IsDone = oReader.GetBoolean("IsDone");
            oServiceSchedule.DoneDate = oReader.GetDateTime("DoneDate");
            oServiceSchedule.IsSMSSend = oReader.GetBoolean("IsSMSSend");
            oServiceSchedule.IsEmailSend = oReader.GetBoolean("IsEmailSend");
            oServiceSchedule.EmailBody = oReader.GetString("EmailBody");
            oServiceSchedule.Status = (EnumServiceScheduleStatus)oReader.GetInt32("Status");
            oServiceSchedule.Remarks = oReader.GetString("Remarks");

            oServiceSchedule.InvoiceNo = oReader.GetString("InvoiceNo");
            oServiceSchedule.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oServiceSchedule.CustomerName = oReader.GetString("CustomerName");
            oServiceSchedule.CustomerShortName = oReader.GetString("CustomerShortName");
            oServiceSchedule.ModelNo = oReader.GetString("ModelNo");
            oServiceSchedule.ServiceInterval = oReader.GetInt32("ServiceInterval");
            oServiceSchedule.ContractorID = oReader.GetInt32("ContractorID");
            
            oServiceSchedule.ServiceDurationInMonth = oReader.GetInt32("ServiceDurationInMonth");

            oServiceSchedule.CRM = oReader.GetString("CRM");
            oServiceSchedule.CustomerPhoneNo = oReader.GetString("CustomerPhoneNo");
            oServiceSchedule.LastServiceDone = oReader.GetDateTime("LastServiceDone");
            oServiceSchedule.UpcomingServiceDate = oReader.GetDateTime("UpcomingServiceDate");
            oServiceSchedule.TotalFreeService = oReader.GetInt32("TotalFreeService");
            oServiceSchedule.RemainingFreeService = oReader.GetInt32("RemainingFreeService");
            oServiceSchedule.Warrenty = oReader.GetDouble("Warrenty");
            oServiceSchedule.RemaingWarrenty = oReader.GetDouble("RemaingWarrenty");

            oServiceSchedule.VinNo = oReader.GetString("VinNo");
            oServiceSchedule.RegistrationNo = oReader.GetString("RegistrationNo");
            oServiceSchedule.kommNo = oReader.GetString("kommNo");
            oServiceSchedule.SalesPersonName = oReader.GetString("SalesPersonName");
            oServiceSchedule.ChassisNo = oReader.GetString("ChassisNo");
            oServiceSchedule.IsPhoneCall = false;
            
            return oServiceSchedule;
        }

        private ServiceSchedule CreateObject(NullHandler oReader)
        {
            ServiceSchedule oServiceSchedule = new ServiceSchedule();
            oServiceSchedule = MapObject(oReader);
            return oServiceSchedule;
        }

        private List<ServiceSchedule> CreateObjects(IDataReader oReader)
        {
            List<ServiceSchedule> oServiceSchedule = new List<ServiceSchedule>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ServiceSchedule oItem = CreateObject(oHandler);
                oServiceSchedule.Add(oItem);
            }
            return oServiceSchedule;
        }

        #endregion

        #region Interface implementation
        public ServiceScheduleService() { }
        public ServiceSchedule Save(ServiceSchedule oServiceSchedule, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oServiceSchedule.ServiceScheduleID <= 0)
                {
                    reader = ServiceScheduleDA.InsertUpdate(tc, oServiceSchedule, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ServiceScheduleDA.InsertUpdate(tc, oServiceSchedule, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceSchedule = new ServiceSchedule();
                    oServiceSchedule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ServiceSchedule. Because of " + e.Message, e);
                #endregion
            }
            return oServiceSchedule;
        }
        public ServiceSchedule ReSchedule(ServiceSchedule oServiceSchedule, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ServiceScheduleDA.ReSchedule(tc, oServiceSchedule, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceSchedule = new ServiceSchedule();
                    oServiceSchedule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ServiceSchedule. Because of " + e.Message, e);
                #endregion
            }
            return oServiceSchedule;
        }

        
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ServiceSchedule oServiceSchedule = new ServiceSchedule();
                oServiceSchedule.ServiceScheduleID = id;
                ServiceScheduleDA.Delete(tc, oServiceSchedule, EnumDBOperation.Delete, nUserId);
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

        public ServiceSchedule Done(ServiceSchedule oServiceSchedule, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ServiceSchedule> oServiceSchedules = new List<ServiceSchedule>();
            oServiceSchedules = oServiceSchedule.ServiceSchedules;
            string sSIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                if (oServiceSchedules.Count > 0)
                {
                    foreach (ServiceSchedule oItem in oServiceSchedules)
                    {
                        ServiceScheduleDA.Done(tc, oItem);
                        sSIDs += oItem.ServiceScheduleID + ",";
                    }
                    sSIDs = sSIDs.Substring(0, sSIDs.Length - 1);
                    oServiceSchedule.ServiceSchedules = new List<ServiceSchedule>();
                    StringBuilder sSQL = new StringBuilder("SELECT * FROM View_ServiceSchedule WHERE ServiceScheduleID IN ("+sSIDs+")");
                    IDataReader reader = null;
                    reader = ServiceScheduleDA.Gets(tc, sSQL.ToString());
                    oServiceSchedules = CreateObjects(reader);
                    reader.Close();
                    oServiceSchedule.ServiceSchedules = oServiceSchedules;
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oServiceSchedule = new ServiceSchedule();
                oServiceSchedule.ErrorMessage = "Failed to Save ServiceSchedule. Because of " + e.Message;
                #endregion
            }
            return oServiceSchedule;
        }


        public ServiceSchedule PhoneCall(ServiceSchedule oServiceSchedule, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ServiceSchedule> oServiceSchedules = new List<ServiceSchedule>();
            oServiceSchedules = oServiceSchedule.ServiceSchedules;
            string sSIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                if (oServiceSchedules.Count > 0)
                {
                    foreach (ServiceSchedule oItem in oServiceSchedules)
                    {

                        ServiceScheduleDA.PhoneCall(tc, oItem);
                       
                        sSIDs += oItem.ServiceScheduleID + ",";
                    }
                    sSIDs = sSIDs.Substring(0, sSIDs.Length - 1);
                    oServiceSchedule.ServiceSchedules = new List<ServiceSchedule>();
                    StringBuilder sSQL = new StringBuilder("SELECT * FROM View_ServiceSchedule WHERE ServiceScheduleID IN (" + sSIDs + ")");
                    IDataReader reader = null;
                    reader = ServiceScheduleDA.Gets(tc, sSQL.ToString());
                    oServiceSchedules = CreateObjects(reader);
                    reader.Close();
                    oServiceSchedule.ServiceSchedules = oServiceSchedules;
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oServiceSchedule = new ServiceSchedule();
                oServiceSchedule.ErrorMessage = "Failed to Save ServiceSchedule. Because of " + e.Message;
                #endregion
            }
            return oServiceSchedule;
        }
       
        public ServiceSchedule SendEmailOrSMS(ServiceSchedule oServiceSchedule, bool bIsEmail, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ServiceSchedule> oServiceSchedules = new List<ServiceSchedule>();
            oServiceSchedules = oServiceSchedule.ServiceSchedules;
            string sSIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                if (oServiceSchedules.Count > 0)
                {
                    foreach (ServiceSchedule oItem in oServiceSchedules)
                    {
                        if (bIsEmail)
                        {
                            ServiceScheduleDA.EmailSend(tc, oItem);
                        }
                        else
                        {
                            ServiceScheduleDA.SMSSend(tc, oItem);
                        }
                        sSIDs += oItem.ServiceScheduleID + ",";
                    }
                    sSIDs = sSIDs.Substring(0, sSIDs.Length - 1);
                    oServiceSchedule.ServiceSchedules = new List<ServiceSchedule>();
                    StringBuilder sSQL = new StringBuilder("SELECT * FROM View_ServiceSchedule WHERE ServiceScheduleID IN (" + sSIDs + ")");
                    IDataReader reader = null;
                    reader = ServiceScheduleDA.Gets(tc, sSQL.ToString());
                    oServiceSchedules = CreateObjects(reader);
                    reader.Close();
                    oServiceSchedule.ServiceSchedules = oServiceSchedules;
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oServiceSchedule = new ServiceSchedule();
                oServiceSchedule.ErrorMessage = "Failed to Save ServiceSchedule. Because of " + e.Message;
                #endregion
            }
            return oServiceSchedule;
        }
        public ServiceSchedule Get(int id, Int64 nUserId)
        {
            ServiceSchedule oServiceSchedule = new ServiceSchedule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ServiceScheduleDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceSchedule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ServiceSchedule", e);
                #endregion
            }
            return oServiceSchedule;
        }
        public List<ServiceSchedule> Gets(Int64 nUserID)
        {
            List<ServiceSchedule> oServiceSchedules = new List<ServiceSchedule>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ServiceScheduleDA.Gets(tc);
                oServiceSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ServiceSchedule", e);
                #endregion
            }
            return oServiceSchedules;
        }
        public List<ServiceSchedule> Gets(string sSQL, Int64 nUserID)
        {
            List<ServiceSchedule> oServiceSchedules = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ServiceScheduleDA.Gets(tc, sSQL);
                oServiceSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ServiceSchedule", e);
                #endregion
            }
            return oServiceSchedules;
        }
        #endregion
    }
}