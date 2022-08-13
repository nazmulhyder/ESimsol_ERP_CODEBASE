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
    public class ExportIncentiveService : MarshalByRefObject, IExportIncentiveService
    {
        #region Private functions and declaration
        private ExportIncentive MapObject(NullHandler oReader)
        {
            ExportIncentive oExportIncentive = new ExportIncentive();
            oExportIncentive.ExportIncentiveID = oReader.GetInt32("ExportIncentiveID");

            oExportIncentive.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportIncentive.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportIncentive.FileNo = oReader.GetString("FileNo");
            oExportIncentive.OpeningDate = oReader.GetDateTime("OpeningDate");
           
            oExportIncentive.BankBranchID_Forwarding = oReader.GetInt32("BankBranchID_Forwarding");
          
            oExportIncentive.ApplicantID = oReader.GetInt32("ApplicantID");
            oExportIncentive.ContactPersonalID = oReader.GetInt32("ContactPersonalID");
            oExportIncentive.DeliveryToID = oReader.GetInt32("DeliveryToID");
            oExportIncentive.Amount = oReader.GetDouble("Amount");
            oExportIncentive.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportIncentive.NegoDays = oReader.GetInt32("NegoDays");
            oExportIncentive.HSCode = oReader.GetString("HSCode");
            oExportIncentive.AreaCode = oReader.GetString("AreaCode");
            oExportIncentive.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportIncentive.ExpiryDate = oReader.GetDateTime("ExpiryDate");
            oExportIncentive.AtSightDiffered = oReader.GetBoolean("AtSightDiffered");
            oExportIncentive.ShipmentFrom = oReader.GetString("ShipmentFrom");
            oExportIncentive.PartialShipmentAllowed = oReader.GetBoolean("PartialShipmentAllowed");
            oExportIncentive.TransShipmentAllowed = oReader.GetBoolean("TransShipmentAllowed");
            oExportIncentive.CurrentStatus = (EnumExportLCStatus)oReader.GetInt32("CurrentStatus");
            
            oExportIncentive.Remark = oReader.GetString("Remark");
            oExportIncentive.Remarks_Application = oReader.GetString("Remarks_Application");
            oExportIncentive.Remarks_BTMA = oReader.GetString("Remarks_BTMA");
            oExportIncentive.Remarks_PRC = oReader.GetString("Remarks_PRC");
            oExportIncentive.Remarks_Audit = oReader.GetString("Remarks_Audit");
            oExportIncentive.Remarks_Realized = oReader.GetString("Remarks_Realized");
            oExportIncentive.BUShortName = oReader.GetString("BUShortName");
            oExportIncentive.BUName = oReader.GetString("BUName");
            oExportIncentive.LiborRate = oReader.GetBoolean("LiborRate");
            oExportIncentive.BBankFDD = oReader.GetBoolean("BBankFDD");
            oExportIncentive.OverDueRate = oReader.GetDouble("OverDueRate");
            oExportIncentive.VersionNo = oReader.GetInt32("VersionNo");
            oExportIncentive.LCRecivedDate = oReader.GetDateTime("LCRecivedDate");
            oExportIncentive.IRC = oReader.GetString("IRC");
            oExportIncentive.ERC = oReader.GetString("ERC");
            oExportIncentive.FrightPrepaid = oReader.GetString("FrightPrepaid");
            oExportIncentive.DarkMedium = oReader.GetString("DarkMedium");
            oExportIncentive.Year = oReader.GetString("Year");
            oExportIncentive.GetOriginalCopy = oReader.GetBoolean("GetOriginalCopy");
            oExportIncentive.DCharge = oReader.GetDouble("DCharge");
            oExportIncentive.LCTramsID = oReader.GetInt32("LCTramsID");
            oExportIncentive.Stability = oReader.GetBoolean("Stability");
            oExportIncentive.Stability = oReader.GetBoolean("Stability");
            oExportIncentive.GarmentsQty = oReader.GetString("GarmentsQty");
            oExportIncentive.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oExportIncentive.TextileUnit = (EnumTextileUnit)oReader.GetInt32("TextileUnit");
           
            ///Derive from view
            oExportIncentive.CurrencyName = oReader.GetString("CurrencyName");
            oExportIncentive.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oExportIncentive.CurrencySymbol_Real = oReader.GetString("CurrencySymbol_Real");
            oExportIncentive.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportIncentive.ApplicantName = oReader.GetString("ApplicantName");
            oExportIncentive.BBranchName_Issue = oReader.GetString("BBranchName_Issue");
            oExportIncentive.BBranchAddress_Issue = oReader.GetString("BBranchAddress_Issue");
            oExportIncentive.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportIncentive.BBranchName_Advice = oReader.GetString("BBranchName_Advice");
            oExportIncentive.BBranchAddress_Advice = oReader.GetString("BBranchAddress_Advice");
            oExportIncentive.BankName_Advice = oReader.GetString("BankName_Advice");
            //ExportIncentive
            oExportIncentive.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportIncentive.PRCDate = oReader.GetDateTime("PRCDate");
            oExportIncentive.PRCCollectBy = oReader.GetInt32("PRCCollectBy");
            oExportIncentive.ApplicationBy = oReader.GetInt32("ApplicationBy");
            oExportIncentive.ApplicationDate = oReader.GetDateTime("ApplicationDate");
            oExportIncentive.BTMAIssueBy = oReader.GetInt32("BTMAIssueBy");
            oExportIncentive.BTMAIssueDate = oReader.GetDateTime("BTMAIssueDate");
            oExportIncentive.AuditCertBy = oReader.GetInt32("AuditCertBy");
            oExportIncentive.AuditCertDate = oReader.GetDateTime("AuditCertDate");
            oExportIncentive.RealizedBy = oReader.GetInt32("RealizedBy");
            oExportIncentive.BillRelizationDate = oReader.GetDateTime("BillRelizationDate");
            oExportIncentive.RealizedDate = oReader.GetDateTime("RealizedDate");
            oExportIncentive.Amount_Realized = oReader.GetDouble("Amount_Realized");
            oExportIncentive.Amount_BillReal = oReader.GetDouble("Amount_BillReal");
            oExportIncentive.Percentage_Incentive = oReader.GetDouble("Percentage_Incentive");

            //if()
            oExportIncentive.MasterLCNos = oReader.GetString("MasterLCNos");
            oExportIncentive.MasterLCDates = oReader.GetString("MasterLCDates");

            oExportIncentive.BankSubDate = oReader.GetDateTime("BankSubDate");
            oExportIncentive.BankSubByName = oReader.GetString("BankSubByName");
            oExportIncentive.BankSubBy = oReader.GetInt32("BankSubBy");
            oExportIncentive.Remarks_BankSub = oReader.GetString("Remarks_BankSub");

            oExportIncentive.PRCCollectByName = oReader.GetString("PRCCollectByName");
            oExportIncentive.ApplicationByName = oReader.GetString("ApplicationByName");
            oExportIncentive.BTMAIssueByName = oReader.GetString("BTMAIssueByName");
            oExportIncentive.AuditCertByName = oReader.GetString("AuditCertByName");
            oExportIncentive.RealizedByName = oReader.GetString("RealizedByName");
            oExportIncentive.CurrencyID_Real = oReader.GetInt32("CurrencyID_Real");
            oExportIncentive.Time_Lag = oReader.GetInt32("Time_Lag");
            oExportIncentive.SLNo = oReader.GetInt32("SLNo");
            oExportIncentive.IsCopyTo = oReader.GetBoolean("IsCopyTo");

            return oExportIncentive;
        }
        private ExportIncentive CreateObject(NullHandler oReader)
        {
            ExportIncentive oExportIncentive = new ExportIncentive();
            oExportIncentive = MapObject(oReader);
            return oExportIncentive;
        }
        private List<ExportIncentive> CreateObjects(IDataReader oReader)
        {
            List<ExportIncentive> oExportIncentive = new List<ExportIncentive>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportIncentive oItem = CreateObject(oHandler);
                oExportIncentive.Add(oItem);
            }
            return oExportIncentive;
        }
        #endregion

        #region Interface implementation
        public ExportIncentiveService() { }
        public ExportIncentive Save(ExportIncentive oExportIncentive, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportIncentive.ExportIncentiveID <= 0)
                {
                    reader = ExportIncentiveDA.InsertUpdate(tc, oExportIncentive, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportIncentiveDA.InsertUpdate(tc, oExportIncentive, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportIncentive = new ExportIncentive();
                    oExportIncentive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ExportIncentive. Because of " + e.Message, e);
                #endregion
            }
            return oExportIncentive;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportIncentive oExportIncentive = new ExportIncentive();
                oExportIncentive.ExportIncentiveID = id;
                ExportIncentiveDA.Delete(tc, oExportIncentive, EnumDBOperation.Delete, nUserId);
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
        public ExportIncentive Get(int id, Int64 nUserId)
        {
            ExportIncentive oExportIncentive = new ExportIncentive();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportIncentiveDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportIncentive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportIncentive", e);
                #endregion
            }
            return oExportIncentive;
        }
        public List<ExportIncentive> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportIncentive> oExportIncentives = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportIncentiveDA.Gets(tc, sSQL);
                oExportIncentives = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportIncentive", e);
                #endregion
            }
            return oExportIncentives;
        }
       
        //Update_PRCDate,Update_ApplicationDate,Update_BTMAIssueDate,Update_AuditCertDate,Update_RealizedDate
        public ExportIncentive Update_PRCDate(ExportIncentive oExportIncentive, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                oExportIncentive.PRCCollectBy = (int)nUserID;
                reader = ExportIncentiveDA.Update_PRCDate(tc, oExportIncentive);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportIncentive = new ExportIncentive();
                    oExportIncentive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update ExportIncentive. Because of " + e.Message, e);
                #endregion
            }
            return oExportIncentive;
        }
        public ExportIncentive Update_ApplicationDate(ExportIncentive oExportIncentive, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                oExportIncentive.ApplicationBy = (int)nUserID;
                reader = ExportIncentiveDA.InsertUpdate(tc, oExportIncentive, EnumDBOperation.Update, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportIncentive = new ExportIncentive();
                    oExportIncentive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update ExportIncentive. Because of " + e.Message, e);
                #endregion
            }
            return oExportIncentive;
        }
        public ExportIncentive Update_BTMAIssueDate(ExportIncentive oExportIncentive, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                oExportIncentive.BTMAIssueBy = (int)nUserID;
                reader = ExportIncentiveDA.Update_BTMAIssueDate(tc, oExportIncentive);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportIncentive = new ExportIncentive();
                    oExportIncentive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update ExportIncentive. Because of " + e.Message, e);
                #endregion
            }
            return oExportIncentive;
        }
        public ExportIncentive Update_BankSubDate(ExportIncentive oExportIncentive, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                oExportIncentive.BankSubBy = (int)nUserID;
                reader = ExportIncentiveDA.Update_BankSubDate(tc, oExportIncentive);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportIncentive = new ExportIncentive();
                    oExportIncentive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update ExportIncentive. Because of " + e.Message, e);
                #endregion
            }
            return oExportIncentive;
        }
        public ExportIncentive Update_AuditCertDate(ExportIncentive oExportIncentive, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                oExportIncentive.AuditCertBy = (int)nUserID;
                reader = ExportIncentiveDA.Update_AuditCertDate(tc, oExportIncentive);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportIncentive = new ExportIncentive();
                    oExportIncentive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update ExportIncentive. Because of " + e.Message, e);
                #endregion
            }
            return oExportIncentive;
        }
        public ExportIncentive Update_RealizedDate(ExportIncentive oExportIncentive, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                oExportIncentive.RealizedBy = (int)nUserID;
                reader = ExportIncentiveDA.Update_RealizedDate(tc, oExportIncentive);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportIncentive = new ExportIncentive();
                    oExportIncentive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update ExportIncentive. Because of " + e.Message, e);
                #endregion
            }
            return oExportIncentive;
        }
        #endregion
    }
}