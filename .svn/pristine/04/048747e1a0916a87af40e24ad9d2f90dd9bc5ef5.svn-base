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
    public class AttendanceMonitoringService : MarshalByRefObject, IAttendanceMonitoringService
    {
        #region Private functions and declaration
        private AttendanceMonitoring MapObject(NullHandler oReader)
        {
            AttendanceMonitoring oAttendanceMonitoring = new AttendanceMonitoring();

            oAttendanceMonitoring.DepartmentID = oReader.GetInt32("DepartmentID");
            oAttendanceMonitoring.DepartmentName = oReader.GetString("Department");
            oAttendanceMonitoring.DesignationID = oReader.GetInt32("DesignationID");
            oAttendanceMonitoring.DesignationName = oReader.GetString("Designation");
            oAttendanceMonitoring.ShiftName = oReader.GetString("Shift");
            oAttendanceMonitoring.Required = oReader.GetInt32("RequiredPerson");
            oAttendanceMonitoring.Exists = oReader.GetInt32("ExistPerson");
            oAttendanceMonitoring.MaleExistPerson = oReader.GetInt32("MaleExistPerson");
            oAttendanceMonitoring.FemaleExistPerson = oReader.GetInt32("FemaleExistPerson");
            oAttendanceMonitoring.Present = oReader.GetInt32("Present");
            oAttendanceMonitoring.MalePresent = oReader.GetInt32("MalePresent");
            oAttendanceMonitoring.FemalePresent = oReader.GetInt32("FemalePresent");
            oAttendanceMonitoring.Absent = oReader.GetInt32("Absent");
            oAttendanceMonitoring.MaleAbsent = oReader.GetInt32("MaleAbsent");
            oAttendanceMonitoring.FemaleAbsent = oReader.GetInt32("FemaleAbsent");
            oAttendanceMonitoring.DayOff = oReader.GetInt32("DayOff");
            oAttendanceMonitoring.MaleDayOff = oReader.GetInt32("MaleDayOff");
            oAttendanceMonitoring.FemaleDayOff = oReader.GetInt32("FeMaleDayOff");
            oAttendanceMonitoring.Leave = oReader.GetInt32("Leave");
            oAttendanceMonitoring.MaleLeave = oReader.GetInt32("MaleLeave");
            oAttendanceMonitoring.FemaleLeave = oReader.GetInt32("FemaleLeave");
            oAttendanceMonitoring.BUID = oReader.GetInt32("BUID");
            oAttendanceMonitoring.BUName = oReader.GetString("BUName");
            oAttendanceMonitoring.LocationName = oReader.GetString("LocationName");
            oAttendanceMonitoring.MaleLate = oReader.GetInt32("MaleLate");
            oAttendanceMonitoring.FemaleLate = oReader.GetInt32("FemaleLate");
            oAttendanceMonitoring.MaleEarlyLeave = oReader.GetInt32("MaleEarlyLeave");
            oAttendanceMonitoring.FemaleEarlyLeave = oReader.GetInt32("FemaleEarlyLeave");


            return oAttendanceMonitoring;

        }

      

        private AttendanceMonitoring CreateObject(NullHandler oReader)
        {
            AttendanceMonitoring oAttendanceMonitoring = MapObject(oReader);
            return oAttendanceMonitoring;
        }

        private List<AttendanceMonitoring> CreateObjects(IDataReader oReader)
        {
            List<AttendanceMonitoring> oAttendanceMonitoring = new List<AttendanceMonitoring>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceMonitoring oItem = CreateObject(oHandler);
                oAttendanceMonitoring.Add(oItem);
            }
            return oAttendanceMonitoring;
        }

        
        
        #endregion

        #region Interface implementation
        public AttendanceMonitoringService() { }

      

        public AttendanceMonitoring Get(string sSQL, Int64 nUserId)
        {
            AttendanceMonitoring oAttendanceMonitoring = new AttendanceMonitoring();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceMonitoringDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceMonitoring = CreateObject(oReader);
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
                //oAttendanceMonitoring.ErrorMessage = e.Message;
                #endregion
            }

            return oAttendanceMonitoring;
        }

        public List<AttendanceMonitoring> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceMonitoring> oAttendanceMonitoring = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceMonitoringDA.Gets(sSQL, tc);
                oAttendanceMonitoring = CreateObjects(reader);
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
            return oAttendanceMonitoring;
        }

        public List<AttendanceMonitoring> Gets(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBlockIDs, DateTime dDate, string sGroupIDs, Int64 nUserID)
        {
            List<AttendanceMonitoring> oAttendanceMonitorings = new List<AttendanceMonitoring>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceMonitoringDA.Gets(sBUnit, sLocationID, sDepartmentIDs, sDesignationIDs, sShiftIds, sBlockIDs, dDate, sGroupIDs, nUserID, tc);
                oAttendanceMonitorings = CreateObjects(reader);
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
            return oAttendanceMonitorings;
        }
        public List<AttendanceMonitoring> GetsComp(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate,string sGroupIDs,string sBlockIDs, Int64 nUserID)
        {
            List<AttendanceMonitoring> oAttendanceMonitorings = new List<AttendanceMonitoring>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceMonitoringDA.GetsComp(sBUnit, sLocationID, sDepartmentIDs, sDesignationIDs, sShiftIds, sBMMIDs, dDate, sGroupIDs, sBlockIDs, nUserID, tc);
                oAttendanceMonitorings = CreateObjects(reader);
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
            return oAttendanceMonitorings;
        }

        public List<AttendanceMonitoring> Gets_LINE(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, Int64 nUserID)
        {
            List<AttendanceMonitoring> oAttendanceMonitorings = new List<AttendanceMonitoring>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceMonitoringDA.Gets_LINE(sBUnit, sLocationID,sDepartmentIDs, sDesignationIDs, sShiftIds, sBMMIDs, dDate,nUserID, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceMonitoring oItem = new AttendanceMonitoring();
                    oItem.DepartmentID = oreader.GetInt32("DepartmentID");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.DesignationID = oreader.GetInt32("DesignationID");
                    oItem.DesignationName = oreader.GetString("DesignationName");
                    oItem.BlockID = oreader.GetInt32("BlockID");
                    oItem.BlockName = oreader.GetString("BlockName");
                    oItem.Present = oreader.GetInt32("Present");
                    oItem.Absent = oreader.GetInt32("Absent");
                    oItem.Leave = oreader.GetInt32("Leave");
                    oAttendanceMonitorings.Add(oItem);
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
                #endregion
            }
            return oAttendanceMonitorings;
        }

        public List<AttendanceMonitoring> Gets_DeptSecWise(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, Int64 nUserID)
        {
            List<AttendanceMonitoring> oAttendanceMonitorings = new List<AttendanceMonitoring>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceMonitoringDA.Gets_DeptSecWise(sBUnit, sLocationID,sDepartmentIDs, sDesignationIDs, sShiftIds, sBMMIDs, dDate,nUserID, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    AttendanceMonitoring oItem = new AttendanceMonitoring();
                    oItem.DepartmentID = oreader.GetInt32("DepartmentID");
                    oItem.DepartmentName = oreader.GetString("DepartmentName");
                    oItem.BlockID = oreader.GetInt32("SectionID");
                    oItem.BlockName = oreader.GetString("SectionName");
                    oItem.Count = oreader.GetInt32("Count");
                    oItem.Status = oreader.GetString("Status");

                    oAttendanceMonitorings.Add(oItem);
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
                #endregion
            }
            return oAttendanceMonitorings;
        }

        public DataSet GetsManPower(string SqlQuery, DateTime AttendanceDate, int ReportLayout, Int64 nUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AttendanceMonitoringDA.GetsManPower(tc, SqlQuery, AttendanceDate, ReportLayout);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DataSet", e);
                #endregion
            }
            return oDataSet;
        }

        #endregion
    }
}
