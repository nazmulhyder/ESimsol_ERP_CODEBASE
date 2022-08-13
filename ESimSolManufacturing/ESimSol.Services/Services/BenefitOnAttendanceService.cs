using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class BenefitOnAttendanceService : MarshalByRefObject, IBenefitOnAttendanceService
    {
        #region Private functions and declaration
        private BenefitOnAttendance MapObject(NullHandler oReader)
        {
            BenefitOnAttendance oBenefitOnAttendance = new BenefitOnAttendance();

            oBenefitOnAttendance.BOAID = oReader.GetInt32("BOAID");
            oBenefitOnAttendance.Name = oReader.GetString("Name");
            oBenefitOnAttendance.BenefitOn = (EnumBenefitOnAttendance)oReader.GetInt16("BenefitOn");
            oBenefitOnAttendance.BenefitOnInt =(int)(EnumBenefitOnAttendance)oReader.GetInt16("BenefitOn");
            oBenefitOnAttendance.StartTime = oReader.GetDateTime("StartTime");
            oBenefitOnAttendance.EndTime = oReader.GetDateTime("EndTime");
            oBenefitOnAttendance.TolarenceInMinute = oReader.GetInt32("TolarenceInMinute");
            oBenefitOnAttendance.OTInMinute = oReader.GetInt32("OTInMinute");
            oBenefitOnAttendance.OTDistributePerPresence = oReader.GetInt32("OTDistributePerPresence");
            oBenefitOnAttendance.IsFullWorkingHourOT = oReader.GetBoolean("IsFullWorkingHourOT");
            oBenefitOnAttendance.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oBenefitOnAttendance.AllowanceOn = (EnumPayrollApplyOn)oReader.GetInt32("AllowanceOn");
            oBenefitOnAttendance.AllowanceOnInt =(int)(EnumPayrollApplyOn)oReader.GetInt16("AllowanceOn");
            oBenefitOnAttendance.IsPercent = oReader.GetBoolean("IsPercent");
            oBenefitOnAttendance.Value = oReader.GetDouble("Value");
            oBenefitOnAttendance.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oBenefitOnAttendance.LeaveAmount = oReader.GetInt32("LeaveAmount");
            oBenefitOnAttendance.HolidayID = oReader.GetInt32("HolidayID");
            oBenefitOnAttendance.IsContinous = oReader.GetBoolean("IsContinous");
            oBenefitOnAttendance.BenefitStartDate = oReader.GetDateTime("BenefitStartDate");
            oBenefitOnAttendance.BenefitEndDate = oReader.GetDateTime("BenefitEndDate");
            oBenefitOnAttendance.ApproveBy = oReader.GetInt32("ApproveBy");
            oBenefitOnAttendance.ApproveByName = oReader.GetString("ApproveByName");
            oBenefitOnAttendance.ApproveDate = oReader.GetDateTime("ApproveDate");
            oBenefitOnAttendance.InactiveBy = oReader.GetInt32("InactiveBy");
            oBenefitOnAttendance.InactiveDate = oReader.GetDateTime("InactiveDate");
            oBenefitOnAttendance.CreateDate = oReader.GetDateTime("DBServerDateTime");
            oBenefitOnAttendance.EncryptedID = Global.Encrypt(oBenefitOnAttendance.BOAID.ToString());
            oBenefitOnAttendance.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oBenefitOnAttendance.LeaveHeadName = oReader.GetString("LeaveHeadName");
            oBenefitOnAttendance.CurrencySymbol = oReader.GetString("CurrencySymbol");
            return oBenefitOnAttendance;
        }

        private BenefitOnAttendance CreateObject(NullHandler oReader)
        {
            BenefitOnAttendance oBenefitOnAttendance = MapObject(oReader);
            return oBenefitOnAttendance;
        }

        private List<BenefitOnAttendance> CreateObjects(IDataReader oReader)
        {
            List<BenefitOnAttendance> oBenefitOnAttendances = new List<BenefitOnAttendance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BenefitOnAttendance oItem = CreateObject(oHandler);
                oBenefitOnAttendances.Add(oItem);
            }
            return oBenefitOnAttendances;
        }

        #endregion

        #region Interface implementation
        public BenefitOnAttendanceService() { }

        public BenefitOnAttendance IUD(BenefitOnAttendance oBenefitOnAttendance, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = BenefitOnAttendanceDA.IUD(tc, oBenefitOnAttendance, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBenefitOnAttendance = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oBenefitOnAttendance.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBenefitOnAttendance.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                //oBenefitOnAttendance.BOAID = 0;//it makes problem during edit
                #endregion
            }
            return oBenefitOnAttendance;
        }

        public BenefitOnAttendance Get(int nBOAID, Int64 nUserId)
        {
            BenefitOnAttendance oBenefitOnAttendance = new BenefitOnAttendance();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BenefitOnAttendanceDA.Get(nBOAID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBenefitOnAttendance = CreateObject(oReader);
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

                oBenefitOnAttendance.ErrorMessage = e.Message;
                #endregion
            }

            return oBenefitOnAttendance;
        }

        public BenefitOnAttendance Get(string sSQL, Int64 nUserId)
        {
            BenefitOnAttendance oBenefitOnAttendance = new BenefitOnAttendance();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BenefitOnAttendanceDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBenefitOnAttendance = CreateObject(oReader);
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

                oBenefitOnAttendance.ErrorMessage = e.Message;
                #endregion
            }

            return oBenefitOnAttendance;
        }

        public List<BenefitOnAttendance> Gets(Int64 nUserID)
        {
            List<BenefitOnAttendance> oBenefitOnAttendance = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BenefitOnAttendanceDA.Gets(tc);
                oBenefitOnAttendance = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BenefitOnAttendance", e);
                #endregion
            }
            return oBenefitOnAttendance;
        }

        public List<BenefitOnAttendance> Gets(string sSQL, Int64 nUserID)
        {
            List<BenefitOnAttendance> oBenefitOnAttendance = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BenefitOnAttendanceDA.Gets(sSQL, tc);
                oBenefitOnAttendance = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BenefitOnAttendance", e);
                #endregion
            }
            return oBenefitOnAttendance;
        }

        #endregion

    }
}
