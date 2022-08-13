using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSol.Services.Services
{
    public class BenefitOnAttendanceEmployeeService : MarshalByRefObject, IBenefitOnAttendanceEmployeeService
    {
        #region Private functions and declaration
        private BenefitOnAttendanceEmployee MapObject(NullHandler oReader)
        {
            BenefitOnAttendanceEmployee oBenefitOnAttendanceEmployee = new BenefitOnAttendanceEmployee();

            //oBenefitOnAttendanceEmployee.BOAESID = oReader.GetInt32("BOAESID");
            oBenefitOnAttendanceEmployee.BOAEmployeeID = oReader.GetInt32("BOAEmployeeID");
            oBenefitOnAttendanceEmployee.BOAID = oReader.GetInt32("BOAID");
            oBenefitOnAttendanceEmployee.EmployeeID = oReader.GetInt32("EmployeeID");
            oBenefitOnAttendanceEmployee.InactiveDate = oReader.GetDateTime("InactiveDate");
            oBenefitOnAttendanceEmployee.InactiveBy = oReader.GetInt32("InactiveBy");
            oBenefitOnAttendanceEmployee.IsTemporaryAssign = oReader.GetBoolean("IsTemporaryAssign");

            oBenefitOnAttendanceEmployee.Name = oReader.GetString("Name");
            //oBenefitOnAttendanceEmployee.BenefitOn = (EnumBenefitOnAttendance)oReader.GetInt16("BenefitOn");
            //oBenefitOnAttendanceEmployee.BenefitOnInt = (int)(EnumBenefitOnAttendance)oReader.GetInt16("BenefitOn");
            oBenefitOnAttendanceEmployee.StartDate = oReader.GetDateTime("StartDate");
            oBenefitOnAttendanceEmployee.EndDate = oReader.GetDateTime("EndDate");
            oBenefitOnAttendanceEmployee.BenefitOn = (EnumBenefitOnAttendance)oReader.GetInt16("BenefitOn");
            oBenefitOnAttendanceEmployee.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oBenefitOnAttendanceEmployee.LeaveHeadName = oReader.GetString("LeaveHeadName");
            oBenefitOnAttendanceEmployee.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oBenefitOnAttendanceEmployee.LeaveAmount = oReader.GetInt32("LeaveAmount");
            oBenefitOnAttendanceEmployee.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oBenefitOnAttendanceEmployee.LeaveHeadName = oReader.GetString("LeaveHeadName");
            oBenefitOnAttendanceEmployee.CurrencySymbol = oReader.GetString("CurrencySymbol");

            //oBenefitOnAttendanceEmployee.TolarenceInMinute = oReader.GetInt32("TolarenceInMinute");
            oBenefitOnAttendanceEmployee.OTInMinute = oReader.GetInt32("OTInMinute");
            oBenefitOnAttendanceEmployee.OTDistributePerPresence = oReader.GetInt32("OTDistributePerPresence");
            oBenefitOnAttendanceEmployee.IsFullWorkingHourOT = oReader.GetBoolean("IsFullWorkingHourOT");
            oBenefitOnAttendanceEmployee.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oBenefitOnAttendanceEmployee.AllowanceOn = (EnumPayrollApplyOn)oReader.GetInt32("AllowanceOn");
            oBenefitOnAttendanceEmployee.AllowanceOnInt = (int)(EnumPayrollApplyOn)oReader.GetInt16("AllowanceOn");
            oBenefitOnAttendanceEmployee.IsPercent = oReader.GetBoolean("IsPercent");
            oBenefitOnAttendanceEmployee.Value = oReader.GetDouble("Value");
            oBenefitOnAttendanceEmployee.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oBenefitOnAttendanceEmployee.LeaveAmount = oReader.GetInt32("LeaveAmount");

            oBenefitOnAttendanceEmployee.EncryptedID = Global.Encrypt(oBenefitOnAttendanceEmployee.BOAEmployeeID.ToString());

            oBenefitOnAttendanceEmployee.EmployeeName = oReader.GetString("EmployeeName");
            oBenefitOnAttendanceEmployee.EmployeeCode = oReader.GetString("EmployeeCode");
            oBenefitOnAttendanceEmployee.DepartmentName = oReader.GetString("DepartmentName");
            oBenefitOnAttendanceEmployee.DesignationName = oReader.GetString("DesignationName");

            return oBenefitOnAttendanceEmployee;
        }

        private BenefitOnAttendanceEmployee CreateObject(NullHandler oReader)
        {
            BenefitOnAttendanceEmployee oBenefitOnAttendanceEmployee = MapObject(oReader);
            return oBenefitOnAttendanceEmployee;
        }

        private List<BenefitOnAttendanceEmployee> CreateObjects(IDataReader oReader)
        {
            List<BenefitOnAttendanceEmployee> oBenefitOnAttendanceEmployees = new List<BenefitOnAttendanceEmployee>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BenefitOnAttendanceEmployee oItem = CreateObject(oHandler);
                oBenefitOnAttendanceEmployees.Add(oItem);
            }
            return oBenefitOnAttendanceEmployees;
        }

        #endregion

        #region Interface implementation
        public BenefitOnAttendanceEmployeeService() { }

        public BenefitOnAttendanceEmployee IUD(BenefitOnAttendanceEmployee oBenefitOnAttendanceEmployee, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<BenefitOnAttendanceEmployee> oBOAEmps = new List<BenefitOnAttendanceEmployee>();
            List<BenefitOnAttendanceEmployee> oBOAEs = new List<BenefitOnAttendanceEmployee>();
            BenefitOnAttendanceEmployee oBOAEmp = new BenefitOnAttendanceEmployee();
            try
            {
                string[] sEmpIDs;
                string[] sBOAIDs;
                sEmpIDs = oBenefitOnAttendanceEmployee.ErrorMessage.Split(',');
                sBOAIDs = oBenefitOnAttendanceEmployee.IDs.Split(',');

                oBOAEs = new List<BenefitOnAttendanceEmployee>();
                string sSql = "SELECT * FROM View_BenefitOnAttendanceEmployee WHERE EmployeeID IN(" + oBenefitOnAttendanceEmployee.ErrorMessage + ") AND BOAID IN(" + oBenefitOnAttendanceEmployee.IDs + ")";
                tc = TransactionContext.Begin();
                IDataReader Reader;
                Reader = BenefitOnAttendanceEmployeeDA.Gets(sSql, tc);
                oBOAEs = CreateObjects(Reader);
                Reader.Close();
                tc.End();

                tc = TransactionContext.Begin(true);
                foreach (string sEmpID in sEmpIDs)
                {
                    int nEmpID = Convert.ToInt32(sEmpID);
                    foreach (string sBOAID in sBOAIDs)
                    {
                        List<BenefitOnAttendanceEmployee> oExistItems = new List<BenefitOnAttendanceEmployee>();
                        oExistItems = oBOAEs.Where(x => x.EmployeeID == nEmpID && x.BOAID == Convert.ToInt32(sBOAID)).ToList();
                        if (oExistItems .Count<= 0)
                        {
                            oBOAEmp.BOAEmployeeID = 0;
                            oBOAEmp.BOAID = Convert.ToInt32(sBOAID);
                            oBOAEmp.EmployeeID = nEmpID;
                            IDataReader reader;
                            reader = BenefitOnAttendanceEmployeeDA.IUD(tc, oBOAEmp, nUserID, nDBOperation);
                            NullHandler oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                oBOAEmp = CreateObject(oReader);
                            }
                            reader.Close();
                            oBOAEmps.Add(oBOAEmp);
                        }
                    }
                }
                tc.End();
                if (nDBOperation == 3)
                {
                    oBOAEmp.ErrorMessage = Global.DeleteMessage;
                }
                if (oBOAEmps.Count<=0)
                {
                    oBOAEmp.ErrorMessage = "Assigned sucessfully";
                }

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBOAEmp.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oBOAEmp;
        }


        public List<BenefitOnAttendanceEmployee> MultiAssign(List<BenefitOnAttendanceEmployee> oBOAEs, DateTime dtStartFrom, DateTime dtEndTo, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<BenefitOnAttendanceEmployee> oBOAEmps = new List<BenefitOnAttendanceEmployee>();
            BenefitOnAttendanceEmployee oBOAEmp = new BenefitOnAttendanceEmployee();

            try
            {
                BenefitOnAttendanceEmployeeAssign oBOAEA = new BenefitOnAttendanceEmployeeAssign();
                tc = TransactionContext.Begin(true);
                NullHandler oReader;
                foreach (BenefitOnAttendanceEmployee oItem in oBOAEs)
                {
                    oBOAEmp = new BenefitOnAttendanceEmployee();
                    IDataReader reader;
                    if (oItem.BOAEmployeeID <= 0)
                    {
                        reader = BenefitOnAttendanceEmployeeDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oBOAEmp = CreateObject(oReader);
                            oBOAEmps.Add(oBOAEmp);
                        }
                        reader.Close();
                    }
                    else
                    {
                        reader = BenefitOnAttendanceEmployeeDA.Get(oItem.BOAEmployeeID, tc);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oBOAEmp = CreateObject(oReader);
                            if (oBOAEmp.BOAEmployeeID > 0 && oItem.IsTemporaryAssign == oBOAEmp.IsTemporaryAssign)
                            {
                                oBOAEmps.Add(oBOAEmp);
                            }
                        }
                        reader.Close();

                        if (oBOAEmp.BOAEmployeeID > 0 && oItem.IsTemporaryAssign != oBOAEmp.IsTemporaryAssign)
                        {
                            oBOAEmp.IsTemporaryAssign = oItem.IsTemporaryAssign;
                            reader = BenefitOnAttendanceEmployeeDA.IUD(tc, oBOAEmp, nUserID, (int)EnumDBOperation.Update);
                            oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                oBOAEmp = CreateObject(oReader);
                                oBOAEmps.Add(oBOAEmp);
                            }
                            reader.Close();
                        }
                    }
                    

                    if (oBOAEmp.BOAEmployeeID > 0 && oBOAEmp.IsTemporaryAssign)
                    {
                        oBOAEA = new BenefitOnAttendanceEmployeeAssign();
                        oBOAEA.BOAEmployeeID = oBOAEmp.BOAEmployeeID;
                        oBOAEA.StartDate = dtStartFrom;
                        oBOAEA.EndDate = dtEndTo;
                        reader = BenefitOnAttendanceEmployeeAssignDA.InsertUpdate(tc, oBOAEA, nUserID,(int)EnumDBOperation.Insert);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oBOAEA = BenefitOnAttendanceEmployeeAssignService.CreateObject(oReader);
                        }
                        reader.Close();
                    } 
                }
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBOAEmps = new List<BenefitOnAttendanceEmployee>();
                oBOAEmp = new BenefitOnAttendanceEmployee();
                oBOAEmp.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View  
                oBOAEmps.Add(oBOAEmp);
                #endregion
            }
            return oBOAEmps;
        }


        public BenefitOnAttendanceEmployee Get(int nBOAEmployeeID, Int64 nUserId)
        {
            BenefitOnAttendanceEmployee oBenefitOnAttendanceEmployee = new BenefitOnAttendanceEmployee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BenefitOnAttendanceEmployeeDA.Get(nBOAEmployeeID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBenefitOnAttendanceEmployee = CreateObject(oReader);
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

                oBenefitOnAttendanceEmployee.ErrorMessage = e.Message;
                #endregion
            }

            return oBenefitOnAttendanceEmployee;
        }

        public BenefitOnAttendanceEmployee Get(string sSQL, Int64 nUserId)
        {
            BenefitOnAttendanceEmployee oBenefitOnAttendanceEmployee = new BenefitOnAttendanceEmployee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BenefitOnAttendanceEmployeeDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBenefitOnAttendanceEmployee = CreateObject(oReader);
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

                oBenefitOnAttendanceEmployee.ErrorMessage = e.Message;
                #endregion
            }

            return oBenefitOnAttendanceEmployee;
        }

        public List<BenefitOnAttendanceEmployee> Gets(int nEmployeeID,Int64 nUserID)
        {
            List<BenefitOnAttendanceEmployee> oBenefitOnAttendanceEmployee = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BenefitOnAttendanceEmployeeDA.Gets(tc, nEmployeeID);
                oBenefitOnAttendanceEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BenefitOnAttendanceEmployee", e);
                #endregion
            }
            return oBenefitOnAttendanceEmployee;
        }

        public List<BenefitOnAttendanceEmployee> Gets(string sSQL, Int64 nUserID)
        {
            List<BenefitOnAttendanceEmployee> oBenefitOnAttendanceEmployee = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BenefitOnAttendanceEmployeeDA.Gets(sSQL, tc);
                oBenefitOnAttendanceEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BenefitOnAttendanceEmployee", e);
                #endregion
            }
            return oBenefitOnAttendanceEmployee;
        }

        #endregion

    }
}
