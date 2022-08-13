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
    public class EmployeeAdvanceSalaryService : MarshalByRefObject, IEmployeeAdvanceSalaryService
    {
        #region Private functions and declaration
        private EmployeeAdvanceSalary MapObject(NullHandler oReader)
        {
            EmployeeAdvanceSalary oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();

            oEmployeeAdvanceSalary.EASID = oReader.GetInt32("EASID");
            oEmployeeAdvanceSalary.EASPID = oReader.GetInt32("EASPID");
            oEmployeeAdvanceSalary.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oEmployeeAdvanceSalary.LocationID = oReader.GetInt32("LocationID");
            oEmployeeAdvanceSalary.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeAdvanceSalary.DepartmentID = oReader.GetInt32("DepartmentID");
            oEmployeeAdvanceSalary.Code = oReader.GetString("Code");
            oEmployeeAdvanceSalary.Name = oReader.GetString("Name");
            oEmployeeAdvanceSalary.BlockName = oReader.GetString("BlockName");
            oEmployeeAdvanceSalary.LocationName = oReader.GetString("LocationName");
            oEmployeeAdvanceSalary.BUName = oReader.GetString("BUName");
            oEmployeeAdvanceSalary.DepartmentIDs = oReader.GetString("DepartmentIDs");
            oEmployeeAdvanceSalary.LocationIDs = oReader.GetString("LocationIDs");
            oEmployeeAdvanceSalary.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeAdvanceSalary.DesignationID = oReader.GetInt32("DesignationID");
            oEmployeeAdvanceSalary.DesignationName = oReader.GetString("DesignationName");
            oEmployeeAdvanceSalary.TotalPresent = oReader.GetInt32("TotalPresent");
            oEmployeeAdvanceSalary.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oEmployeeAdvanceSalary.TotalPaidLeave = oReader.GetInt32("TotalPaidLeave");
            oEmployeeAdvanceSalary.TotalUPLeave = oReader.GetInt32("TotalUPLeave");
            oEmployeeAdvanceSalary.TotalLateInDays = oReader.GetInt32("TotalLateInDays");
            oEmployeeAdvanceSalary.TotalEarlyInDays = oReader.GetInt32("TotalEarlyInDays");
            oEmployeeAdvanceSalary.TotalLateInMin = oReader.GetInt32("TotalLateInMin");
            oEmployeeAdvanceSalary.TotalEarlyInMin = oReader.GetInt32("TotalEarlyInMin");
            oEmployeeAdvanceSalary.TotalHoliday = oReader.GetInt32("TotalHoliday");
            oEmployeeAdvanceSalary.TotalDayOff = oReader.GetInt32("TotalDayOff");
            oEmployeeAdvanceSalary.GrossSalary = oReader.GetDouble("GrossSalary");
            oEmployeeAdvanceSalary.GrossEarnings = oReader.GetDouble("GrossEarnings");
            oEmployeeAdvanceSalary.TotalDeductions = oReader.GetDouble("TotalDeductions");
            oEmployeeAdvanceSalary.NetAmount = oReader.GetDouble("NetAmount");
            oEmployeeAdvanceSalary.JoiningDate = oReader.GetDateTime("JoiningDate");

            return oEmployeeAdvanceSalary;
        }
        private EmployeeAdvanceSalary CreateObject(NullHandler oReader)
        {
            EmployeeAdvanceSalary oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            oEmployeeAdvanceSalary = MapObject(oReader);
            return oEmployeeAdvanceSalary;
        }
        private List<EmployeeAdvanceSalary> CreateObjects(IDataReader oReader)
        {
            List<EmployeeAdvanceSalary> oEmployeeAdvanceSalary = new List<EmployeeAdvanceSalary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeAdvanceSalary oItem = CreateObject(oHandler);
                oEmployeeAdvanceSalary.Add(oItem);
            }
            return oEmployeeAdvanceSalary;
        }
        #endregion

        #region Interface implementation


        public EmployeeAdvanceSalary Save(EmployeeAdvanceSalary oEmployeeAdvanceSalary, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                //tc = TransactionContext.Begin(true);
                //IDataReader reader;
                //reader = EmployeeAdvanceSalaryDA.InsertUpdate(tc, oEmployeeAdvanceSalary, EnumDBOperation.Insert, nUserId);
                //NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
                //    oEmployeeAdvanceSalary = CreateObject(oReader);
                //}
                //reader.Close();
                //tc.End();


                //tc = TransactionContext.Begin(true);
                //IDataReader reader;
                //if (oEmployeeAdvanceSalary.EASID <= 0)
                //{
                //    reader = EmployeeAdvanceSalaryDA.InsertUpdate(tc, oEmployeeAdvanceSalary, EnumDBOperation.Insert, nUserId);
                //}
                //else
                //{
                //    reader = EmployeeAdvanceSalaryDA.InsertUpdate(tc, oEmployeeAdvanceSalary, EnumDBOperation.Update, nUserId);
                //}
                //NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{
                //    oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
                //    oEmployeeAdvanceSalary = CreateObject(oReader);
                //}
                //reader.Close();
                //tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to save EmployeeAdvanceSalary " + e.Message, e);
                #endregion
            }
            return oEmployeeAdvanceSalary;
        }


        public int EmployeeAdvanceSalarySave(int nIndex, int EASPID,  Int64 nUserID)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = EmployeeAdvanceSalaryDA.EmployeeAdvanceSalarySave(tc, nIndex, EASPID, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                nNewIndex = 0;
                throw new ServiceException(e.Message);
                #endregion
            }
            return nNewIndex;
        }


        public string Delete(int id, int nUserId)
        {
            string message = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeAdvanceSalary oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
                oEmployeeAdvanceSalary.EASID = id;
                EmployeeAdvanceSalaryDA.Delete(tc, oEmployeeAdvanceSalary, EnumDBOperation.Delete, nUserId);
                tc.End();
                message = Global.DeleteMessage;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                message = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }
            return message;
        }


        public EmployeeAdvanceSalary Get(string sSQL, Int64 nUserId)
        {
            EmployeeAdvanceSalary oEmployeeAdvanceSalary = new EmployeeAdvanceSalary();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ELSetupDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeAdvanceSalary = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeAdvanceSalary;
        }


        public List<EmployeeAdvanceSalary> Gets(string sSQL, int nUserId)
        {
            List<EmployeeAdvanceSalary> oEmployeeAdvanceSalary = new List<EmployeeAdvanceSalary>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeAdvanceSalaryDA.Gets(tc, sSQL);
                oEmployeeAdvanceSalary = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeAdvanceSalary", e);
                #endregion
            }

            return oEmployeeAdvanceSalary;
        }


        public List<EmployeeAdvanceSalary> Gets(int nUserId)
        {
            List<EmployeeAdvanceSalary> oEmployeeAdvanceSalary = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeAdvanceSalaryDA.Gets(tc);
                oEmployeeAdvanceSalary = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeAdvanceSalary ", e);
                #endregion
            }

            return oEmployeeAdvanceSalary;
        }


        #endregion

       
    }
}

