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
    public class RosterPlanEmployeeService : MarshalByRefObject, IRosterPlanEmployeeService
    {
        #region Private functions and declaration
        private RosterPlanEmployee MapObject(NullHandler oReader)
        {
            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            oRosterPlanEmployee.RPEID = oReader.GetInt32("RPEID");
            oRosterPlanEmployee.EmployeeID = oReader.GetInt32("EmployeeID");
            oRosterPlanEmployee.ShiftID = oReader.GetInt32("ShiftID");
            oRosterPlanEmployee.MaxOTInMin = oReader.GetInt32("MaxOTInMin");
            oRosterPlanEmployee.Remarks = oReader.GetString("Remarks");            
            oRosterPlanEmployee.IsDayOff = oReader.GetBoolean("IsDayOff");
            oRosterPlanEmployee.IsHoliday = oReader.GetBoolean("IsHoliday");
            oRosterPlanEmployee.InTime = oReader.GetDateTime("InTime");
            oRosterPlanEmployee.OutTime = oReader.GetDateTime("OutTime");
            oRosterPlanEmployee.AttendanceDate = oReader.GetDateTime("AttendanceDate");
            oRosterPlanEmployee.ShiftStartTime = oReader.GetDateTime("ShiftStartTime");
            oRosterPlanEmployee.ShiftEndTime = oReader.GetDateTime("ShiftEndTime");
            oRosterPlanEmployee.EmployeeCode = oReader.GetString("EmployeeCode");
            oRosterPlanEmployee.EmployeeName = oReader.GetString("EmployeeName");
            oRosterPlanEmployee.ShiftName = oReader.GetString("ShiftName");
            return oRosterPlanEmployee;

        }

        private RosterPlanEmployee CreateObject(NullHandler oReader)
        {
            RosterPlanEmployee oRosterPlanEmployee = MapObject(oReader);
            return oRosterPlanEmployee;
        }

        private List<RosterPlanEmployee> CreateObjects(IDataReader oReader)
        {
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RosterPlanEmployee oItem = CreateObject(oHandler);
                oRosterPlanEmployees.Add(oItem);
            }
            return oRosterPlanEmployees;
        }

        #endregion

        #region Interface implementation
        public RosterPlanEmployeeService() { }
        public RosterPlanEmployee IUD(RosterPlanEmployee oRosterPlanEmployee, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = RosterPlanEmployeeDA.IUD(tc, oRosterPlanEmployee, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRosterPlanEmployee = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oRosterPlanEmployee.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRosterPlanEmployee.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oRosterPlanEmployee;
        }

        public RosterPlanEmployee Get(int nRPEID, Int64 nUserId)
        {
            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RosterPlanEmployeeDA.Get(nRPEID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRosterPlanEmployee = CreateObject(oReader);
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

                oRosterPlanEmployee.ErrorMessage = e.Message;
                #endregion
            }

            return oRosterPlanEmployee;
        }

        public RosterPlanEmployee Get(string sSQL, Int64 nUserId)
        {
            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RosterPlanEmployeeDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRosterPlanEmployee = CreateObject(oReader);
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

                oRosterPlanEmployee.ErrorMessage = e.Message;
                #endregion
            }

            return oRosterPlanEmployee;
        }

        public List<RosterPlanEmployee> Gets(Int64 nUserID)
        {
            List<RosterPlanEmployee> oRosterPlanEmployee = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RosterPlanEmployeeDA.Gets(tc);
                oRosterPlanEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RosterPlanEmployee", e);
                #endregion
            }
            return oRosterPlanEmployee;
        }

        public List<RosterPlanEmployee> Gets(string sSQL, Int64 nUserID)
        {
            List<RosterPlanEmployee> oRosterPlanEmployee = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RosterPlanEmployeeDA.Gets(sSQL, tc);
                oRosterPlanEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RosterPlanEmployee", e);
                #endregion
            }
            return oRosterPlanEmployee;
        }

        public List<RosterPlanEmployee> Gets(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, Int64 nUserID)
        {
            List<RosterPlanEmployee> oRosterPlanEmployee = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RosterPlanEmployeeDA.Gets(EmployeeIDs, ShiftID, StartDate, EndDate, tc);
                oRosterPlanEmployee = CreateObjects(reader);
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
                #endregion
            }
            return oRosterPlanEmployee;
        }

        public List<RosterPlanEmployee> Transfer(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, int nOT_In_Minute, bool IsDayOff, int nDBOperation, Int64 nUserID)
        {
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>(); ;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RosterPlanEmployeeDA.Transfer(EmployeeIDs, ShiftID, StartDate, EndDate, nUserID, nOT_In_Minute, IsDayOff, nDBOperation, tc);
                oRosterPlanEmployees = CreateObjects(reader);
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
                #endregion
            }
            return oRosterPlanEmployees;
        }
        public List<RosterPlanEmployee> TransferDept(DateTime StartDate, DateTime EndDate, int BUID, string LocationIDs, string DepartmentIDs, int ShiftID, DateTime InTime, DateTime OutTime, bool IsGWD, bool IsDayOff, string Remarks, int MaxOTDateTime, string EmployeeIDs, string DesignationIDs, bool isOfficial, DateTime RosterDate, string GroupIDs, string BlockIDs, int TrsShiftID, int nDBOperation, Int64 nUserID)
        {
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>(); ;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RosterPlanEmployeeDA.TransferDept(StartDate, EndDate, BUID, LocationIDs, DepartmentIDs, ShiftID, InTime, OutTime, IsGWD, IsDayOff, Remarks, MaxOTDateTime, EmployeeIDs, DesignationIDs, isOfficial, RosterDate, GroupIDs, BlockIDs,TrsShiftID, nUserID, nDBOperation, tc);
                oRosterPlanEmployees = CreateObjects(reader);
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
                #endregion
            }
            return oRosterPlanEmployees;
        }

        public List<RosterPlanEmployee> Swap(int RosterPlanID, DateTime StartDate, DateTime EndDate, int nDBOperation, Int64 nUserID)
        {
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>(); ;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RosterPlanEmployeeDA.Swap(RosterPlanID, StartDate, EndDate, nUserID, nDBOperation, tc);
                oRosterPlanEmployees = CreateObjects(reader);
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
                #endregion
            }
            return oRosterPlanEmployees;
        }
        #endregion

        public List<RosterPlanEmployee> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID)
        {
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RosterPlanEmployeeDA.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks, nUserID, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    RosterPlanEmployee oItem = new RosterPlanEmployee();
                    oItem.EmployeeID = oreader.GetInt32("EmployeeID");
                    oItem.AttendanceDate = oreader.GetDateTime("AttendanceDate");
                    oItem.EmployeeName = oreader.GetString("EmployeeName");
                    oItem.EmployeeCode = oreader.GetString("EmployeeCode");
                    oItem.ShiftName = oreader.GetString("ShiftName");

                    oRosterPlanEmployees.Add(oItem);
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
                throw new ServiceException("Failed to Get RosterPlanEmployee", e);
                #endregion
            }
            return oRosterPlanEmployees;
        }

        #region UploadXL
        public List<RosterPlanEmployee> UploadRosterEmpXL(List<RosterPlanEmployee> oRosterPlanEmployees, Int64 nUserID)
        {
            List<RosterPlanEmployee> oTempRPEs = new List<RosterPlanEmployee>();
            List<RosterPlanEmployee> oRPEs = new List<RosterPlanEmployee>();
            TransactionContext tc = null;
            try
            {
                //bool IsAdd = true;
                foreach (RosterPlanEmployee oItem in oRosterPlanEmployees)
                {
                    tc = TransactionContext.Begin(true);
                    

                    oTempRPEs = new List<RosterPlanEmployee>();
                    IDataReader reader = null;
                    reader = RosterPlanEmployeeDA.UploadRosterEmpXL(tc, oItem, nUserID);
                    if (oRPEs.Count <= 0)
                    {
                        oTempRPEs = CreateObjects(reader);
                        oRPEs.AddRange(oTempRPEs);
                    }
                    //if (IsAdd)
                    //{
                        //NullHandler oReader = new NullHandler(reader);
                        //if (reader.Read())
                        //{
                        //    oTempRPEs = CreateObjects(oReader);
                        //    oRPEs.AddRange(oTempRPEs);
                        //}
                        
                    //}
                    reader.Close();
                    //IsAdd = false;
                    tc.End();
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                RosterPlanEmployee oRPE = new RosterPlanEmployee();
                oRPEs = new List<RosterPlanEmployee>();
                oRPEs.Add(oRPE);
                oRPEs[0].ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oRPEs;
        }
        #endregion UploadXl

    }
}
