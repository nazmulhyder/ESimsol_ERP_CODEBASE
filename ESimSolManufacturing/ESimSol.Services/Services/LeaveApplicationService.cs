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
    public class LeaveApplicationService : MarshalByRefObject, ILeaveApplicationService
    {
        #region Private functions and declaration
        private LeaveApplication MapObject(NullHandler oReader)
        {
            LeaveApplication oLeaveApplication = new LeaveApplication();
            oLeaveApplication.LeaveApplicationID = oReader.GetInt32("LeaveApplicationID");
            oLeaveApplication.EmployeeID = oReader.GetInt32("EmployeeID");
            oLeaveApplication.EmpLeaveLedgerID = oReader.GetInt32("EmpLeaveLedgerID");
            oLeaveApplication.ApplicationNature = (EnumLeaveApplication)oReader.GetInt16("ApplicationNature");
            oLeaveApplication.ApplicationNatureInt = oReader.GetInt16("ApplicationNature");
            oLeaveApplication.LeaveType = (EnumLeaveType)oReader.GetInt16("LeaveType");
            oLeaveApplication.LeaveTypeInt = oReader.GetInt16("LeaveType");
            oLeaveApplication.StartDateTime = oReader.GetDateTime("StartDateTime");
            oLeaveApplication.EndDateTime = oReader.GetDateTime("EndDateTime");
            oLeaveApplication.Location = oReader.GetString("Location");
            oLeaveApplication.Reason = oReader.GetString("Reason");
            oLeaveApplication.RequestForRecommendation = oReader.GetInt32("RequestForRecommendation");
            oLeaveApplication.RecommendedBy = oReader.GetInt32("RecommendedBy");
            oLeaveApplication.RecommendedByDate = oReader.GetDateTime("RecommendedByDate");
            oLeaveApplication.ApproveBy = oReader.GetInt32("ApproveBy");
            oLeaveApplication.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oLeaveApplication.IsUnPaid = oReader.GetBoolean("IsUnPaid");
            oLeaveApplication.RecommendationNote = oReader.GetString("RecommendationNote");
            oLeaveApplication.ApprovalNote = oReader.GetString("ApprovalNote");
            oLeaveApplication.LeaveStatus = (EnumLeaveStatus)oReader.GetInt16("LeaveStatus");
            oLeaveApplication.LeaveStatusInt = oReader.GetInt16("LeaveStatus");
            oLeaveApplication.LeaveHeadName = oReader.GetString("LeaveHeadName");
            oLeaveApplication.RecommendedByName = oReader.GetString("RecommendedByName");
            oLeaveApplication.ApproveByName = oReader.GetString("ApprovedByName");
            oLeaveApplication.EmployeeName = oReader.GetString("EmployeeName");
            oLeaveApplication.EmployeeCode = oReader.GetString("EmployeeCode");
            oLeaveApplication.RecommendedByName = oReader.GetString("RecommendedByName");
            oLeaveApplication.RequestedForRecommendation = oReader.GetString("RequestedForRecommendation");
            oLeaveApplication.CancelledBy = oReader.GetInt32("CancelledBy");
            oLeaveApplication.CancelledByDate = oReader.GetDateTime("CancelledByDate");
            oLeaveApplication.CancelledNote = oReader.GetString("CancelledNote");
            oLeaveApplication.RequestForAproval = oReader.GetInt32("RequestForAproval");
            oLeaveApplication.RequestForAprovalName = oReader.GetString("RequestForAprovalName");
            oLeaveApplication.ResponsiblePersonID = oReader.GetInt32("ResponsiblePersonID");
            oLeaveApplication.TotalDay = oReader.GetDouble("TotalDay");
            oLeaveApplication.ResponsiblePersonName = oReader.GetString("ResponsiblePersonName");
            oLeaveApplication.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oLeaveApplication.LeaveDuration = oReader.GetInt32("LeaveDuration");
            oLeaveApplication.LDays = oReader.GetInt32("LDays");
            oLeaveApplication.DepartmentName = oReader.GetString("DepartmentName");
            oLeaveApplication.DesignationName = oReader.GetString("DesignationName");
            oLeaveApplication.HRApproveBYName = oReader.GetString("HRApproveBYName");
            oLeaveApplication.IsApprove = oReader.GetBoolean("IsApprove");
            oLeaveApplication.DBUserName = oReader.GetString("DBUserName");
            oLeaveApplication.JoiningDate = oReader.GetDateTime("JoiningDate");
            oLeaveApplication.LocationName = oReader.GetString("LocationName");
            oLeaveApplication.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oLeaveApplication.Gender = oReader.GetString("Gender");
            oLeaveApplication.IsActive = oReader.GetBoolean("IsActive");
            oLeaveApplication.ApprovedByDepartment = oReader.GetString("ApprovedByDepartment");
            oLeaveApplication.ApprovedByDesignation = oReader.GetString("ApprovedByDesignation");
            oLeaveApplication.LeaveHeadShortName = oReader.GetString("LeaveHeadShortName");
            return oLeaveApplication;
        }

        private LeaveApplication CreateObject(NullHandler oReader)
        {
            LeaveApplication oLeaveApplication = MapObject(oReader);
            return oLeaveApplication;
        }

        private List<LeaveApplication> CreateObjects(IDataReader oReader)
        {
            List<LeaveApplication> oLeaveApplication = new List<LeaveApplication>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LeaveApplication oItem = CreateObject(oHandler);
                oLeaveApplication.Add(oItem);
            }
            return oLeaveApplication;
        }

        #endregion

        #region Interface implementation
        public LeaveApplicationService() { }

        public LeaveApplication IUD(LeaveApplication oLeaveApplication, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            LeaveApplication oNewLeaveApplication = new LeaveApplication();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = LeaveApplicationDA.IUD(tc, oLeaveApplication, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oNewLeaveApplication = CreateObject(oReader);
                }
                reader.Close();
                
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oNewLeaveApplication.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNewLeaveApplication.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oNewLeaveApplication;
        }

        public LeaveApplication Approved(LeaveApplication oLeaveApplication, Int64 nUserID)
        {
            LeaveApplication aLeaveApplication = new LeaveApplication();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LeaveApplicationDA.Approve(tc, oLeaveApplication, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aLeaveApplication = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                aLeaveApplication.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }

            return aLeaveApplication;
        }

        public List<LeaveApplication> MultipleApprove(List<LeaveApplication> oLeaveApplications, Int64 nUserID)
        {
            List<LeaveApplication> oLApplications = new List<LeaveApplication>();
            LeaveApplication oLApplication = new LeaveApplication();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                foreach (LeaveApplication oLeaveApplication in oLeaveApplications)
                {
                    IDataReader reader = LeaveApplicationDA.Approve(tc, oLeaveApplication, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oLApplication = CreateObject(oReader);
                    }
                    reader.Close();
                    oLApplications.Add(oLApplication);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLApplications = new List<LeaveApplication>();
                oLApplication = new LeaveApplication();
                oLApplication.ErrorMessage = e.Message.Split('!')[0];
                oLApplications.Add(oLApplication);
                #endregion
            }

            return oLApplications;
        }

        public LeaveApplication Cancel(LeaveApplication oLeaveApplication, Int64 nUserID)
        {
            LeaveApplication oLApp = new LeaveApplication();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LeaveApplicationDA.Cancel(tc, oLeaveApplication, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLApp = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oLApp = new LeaveApplication();
                oLApp.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oLApp;
        }

        public LeaveApplication Get(int id, Int64 nUserId)
        {
            LeaveApplication aLeaveApplication = new LeaveApplication();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LeaveApplicationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aLeaveApplication = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LeaveApplication", e);
                #endregion
            }

            return aLeaveApplication;
        }

        public List<LeaveApplication> Gets(Int64 nUserID)
        {
            List<LeaveApplication> oLeaveApplication = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LeaveApplicationDA.Gets(tc);
                oLeaveApplication = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LeaveApplication", e);
                #endregion
            }
            return oLeaveApplication;
        }
        public List<LeaveApplication> Gets(string sSQL, Int64 nUserID)
        {
            List<LeaveApplication> oLeaveApplication = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LeaveApplicationDA.Gets(tc, sSQL);
                oLeaveApplication = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LeaveApplication", e);
                #endregion
            }
            return oLeaveApplication;
        }

        public List<LeaveApplication> GetsEmployeeLeaveLedger(string sSQL, int nACSID, Int64 nUserID)
        {
            List<LeaveApplication> oLeaveApplication = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LeaveApplicationDA.GetsEmployeeLeaveLedger(tc, sSQL, nACSID);
                oLeaveApplication = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LeaveApplication", e);
                #endregion
            }
            return oLeaveApplication;
        }

        public LeaveApplication Get(string sSQL, Int64 nUserId)
        {
            LeaveApplication aLeaveApplication = new LeaveApplication();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LeaveApplicationDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aLeaveApplication = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LeaveApplication", e);
                #endregion
            }

            return aLeaveApplication;
        }

        public LeaveApplication LeaveAdjustment(int LeaveApplicationID, DateTime EndDate, Int64 nUserID)
        {
            LeaveApplication oLApp = new LeaveApplication();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LeaveApplicationDA.LeaveAdjustment(tc,LeaveApplicationID, EndDate, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLApp = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oLApp = new LeaveApplication();
                oLApp.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oLApp;
        }

        #endregion
    }
}
