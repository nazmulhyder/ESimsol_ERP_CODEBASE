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
    public class SalarySchemeService : MarshalByRefObject, ISalarySchemeService
    {
        #region Private functions and declaration
        private SalaryScheme MapObject(NullHandler oReader)
        {
            SalaryScheme oSalaryScheme = new SalaryScheme();
            oSalaryScheme.SalarySchemeID = oReader.GetInt32("SalarySchemeID");
            oSalaryScheme.Name = oReader.GetString("Name");
            oSalaryScheme.NatureOfEmployee = (EnumEmployeeNature)oReader.GetInt16("NatureOfEmployee");
            oSalaryScheme.PaymentCycle = (EnumPaymentCycle)oReader.GetInt16("PaymentCycle");
            oSalaryScheme.PaymentCycleInt = oReader.GetInt16("PaymentCycle");
            oSalaryScheme.Description = oReader.GetString("Description");
            oSalaryScheme.IsAllowBankAccount = oReader.GetBoolean("IsAllowBankAccount");
            oSalaryScheme.IsAllowOverTime = oReader.GetBoolean("IsAllowOverTime");
            oSalaryScheme.OverTimeON = (EnumOverTimeON)oReader.GetInt16("OverTimeON");
            oSalaryScheme.OverTimeONInt =(int)(EnumOverTimeON)oReader.GetInt16("OverTimeON");
            oSalaryScheme.DividedBy = oReader.GetDouble("DividedBy");
            oSalaryScheme.MultiplicationBy = oReader.GetDouble("MultiplicationBy");
            oSalaryScheme.FixedOTRatePerHour = oReader.GetDouble("FixedOTRatePerHour");

            oSalaryScheme.CompOverTimeON = (EnumOverTimeON)oReader.GetInt16("CompOverTimeON");
            oSalaryScheme.CompOverTimeONInt = (int)(EnumOverTimeON)oReader.GetInt16("CompOverTimeON");
            oSalaryScheme.CompDividedBy = oReader.GetDouble("CompDividedBy");
            oSalaryScheme.CompMultiplicationBy = oReader.GetDouble("CompMultiplicationBy");
            oSalaryScheme.CompFixedOTRatePerHour = oReader.GetDouble("CompFixedOTRatePerHour");

            oSalaryScheme.IsActive = oReader.GetBoolean("IsActive");
            oSalaryScheme.IsAttendanceDependant = oReader.GetBoolean("IsAttendanceDependant");
            oSalaryScheme.LateCount = oReader.GetInt32("LateCount");
            oSalaryScheme.EarlyLeavingCount = oReader.GetInt32("EarlyLeavingCount");
            oSalaryScheme.FixedLatePenalty = oReader.GetDouble("FixedLatePenalty");
            oSalaryScheme.FixedEarlyLeavePenalty = oReader.GetDouble("FixedEarlyLeavePenalty");
            oSalaryScheme.IsProductionBase = oReader.GetBoolean("IsProductionBase");
            oSalaryScheme.StartDay = oReader.GetInt32("StartDay");
            oSalaryScheme.IsGratuity = oReader.GetBoolean("IsGratuity");
            oSalaryScheme.GraturityMaturedInYear = oReader.GetInt32("GraturityMaturedInYear");
            oSalaryScheme.NoOfMonthCountOneYear = oReader.GetInt32("NoOfMonthCountOneYear");
            oSalaryScheme.GratuityApplyOn = (EnumPayrollApplyOn)oReader.GetInt16("GratuityApplyOn");
            oSalaryScheme.EncryptSalarySchemeID = Global.Encrypt(oSalaryScheme.SalarySchemeID.ToString());
            return oSalaryScheme;

        }

        private SalaryScheme CreateObject(NullHandler oReader)
        {
            SalaryScheme oSalaryScheme = MapObject(oReader);
            return oSalaryScheme;
        }

        private List<SalaryScheme> CreateObjects(IDataReader oReader)
        {
            List<SalaryScheme> oSalaryScheme = new List<SalaryScheme>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalaryScheme oItem = CreateObject(oHandler);
                oSalaryScheme.Add(oItem);
            }
            return oSalaryScheme;
        }

        #endregion

        #region Interface implementation
        public SalarySchemeService() { }

        public SalaryScheme IUD(SalaryScheme oSalaryScheme, int nDBOperation, Int64 nUserID)
        {
            List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();
            oSalarySchemeDetails = oSalaryScheme.SalarySchemeDetails;
            TransactionContext tc = null;

            try
            {
                if (nDBOperation == 2 || nDBOperation == 3)
                {
                    string sSql = "SELECT TOP(1)ESSID FROM EmployeeSalaryStructure WHERE SalarySchemeID =" + oSalaryScheme.SalarySchemeID;
                    tc = TransactionContext.Begin(true);
                    bool IsAssigned = SalarySchemeDA.IsAssigned(sSql, tc);
                    tc.End();

                    if (IsAssigned == true)
                    {
                        throw new Exception("This Salary Scheme is already assigned to employee. So edit or delete is not possible !");
                    }
                }

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SalarySchemeDA.IUD(tc, oSalaryScheme, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalaryScheme = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSalaryScheme.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oSalaryScheme.SalarySchemeID = 0;
                #endregion
            }
            return oSalaryScheme;
        }

        public SalaryScheme SalarySchemeSave(SalaryScheme oSalaryScheme, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SalarySchemeDA.IUD(tc, oSalaryScheme, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oSalaryScheme = CreateObject(oReader);
                }
                reader.Close();
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSalaryScheme.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oSalaryScheme.SalarySchemeID = 0;
                #endregion
            }
            return oSalaryScheme;
        }
       
        public SalaryScheme Get(int nSalarySchemeID, Int64 nUserId)
        {
            SalaryScheme oSalaryScheme = new SalaryScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalarySchemeDA.Get(nSalarySchemeID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalaryScheme = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get SalaryScheme", e);
                oSalaryScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oSalaryScheme;
        }
        public List<SalaryScheme> Gets(Int64 nUserID)
        {
            List<SalaryScheme> oSalaryScheme = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalarySchemeDA.Gets(tc);
                oSalaryScheme = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalaryScheme", e);
                #endregion
            }
            return oSalaryScheme;
        }

        public List<SalaryScheme> Gets(string sSQL, Int64 nUserID)
        {
            List<SalaryScheme> oSalaryScheme = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalarySchemeDA.Gets(sSQL, tc);
                oSalaryScheme = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalaryScheme", e);
                #endregion
            }
            return oSalaryScheme;
        }

        #region Activity
        public SalaryScheme Activite(int nSalarySchemeID, bool Active, Int64 nUserId)
        {
            SalaryScheme oSalaryScheme = new SalaryScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                if (!Active)
                {
                    string sSql = "SELECT TOP(1)ESSID FROM EmployeeSalaryStructure WHERE SalarySchemeID =" + nSalarySchemeID + " AND IsActive=1";
                    tc = TransactionContext.Begin(true);
                    bool IsAssigned = SalarySchemeDA.IsAssigned(sSql, tc);

                    if (IsAssigned == true)
                    {
                        throw new Exception("This Salary Scheme is already assigned to employee. So Inactive is not possible !");
                    }
                }
                IDataReader reader = SalarySchemeDA.Activity(nSalarySchemeID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalaryScheme = CreateObject(oReader);
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
                oSalaryScheme = new SalaryScheme();
                oSalaryScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oSalaryScheme;
        }

        #endregion
        #endregion
    }
}
