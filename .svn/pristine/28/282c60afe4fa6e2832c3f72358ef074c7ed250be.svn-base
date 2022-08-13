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
    public class ELEncashComplianceDetailService : MarshalByRefObject, IELEncashComplianceDetailService
    {
        #region Private functions and declaration
        private ELEncashComplianceDetail MapObject(NullHandler oReader)
        {
            ELEncashComplianceDetail oELEncashComplianceDetail = new ELEncashComplianceDetail();
            oELEncashComplianceDetail.ELECDID = oReader.GetInt32("ELECDID");
            oELEncashComplianceDetail.BUID = oReader.GetInt32("BUID");
            oELEncashComplianceDetail.ELEncashCompID = oReader.GetInt32("ELEncashCompID");
            oELEncashComplianceDetail.EmployeeID = oReader.GetInt32("EmployeeID");
            oELEncashComplianceDetail.DepartmentID = oReader.GetInt32("DepartmentID");
            oELEncashComplianceDetail.DesignationID = oReader.GetInt32("DesignationID");
            oELEncashComplianceDetail.BlockID = oReader.GetInt32("BlockID");
            oELEncashComplianceDetail.AttendanceSchemeID = oReader.GetInt32("AttendanceSchemeID");
            oELEncashComplianceDetail.TotalPresent = oReader.GetInt32("TotalPresent");
            oELEncashComplianceDetail.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oELEncashComplianceDetail.TotalLeave = oReader.GetInt32("TotalLeave");
            oELEncashComplianceDetail.TotalDayOff = oReader.GetInt32("TotalDayOff");
            oELEncashComplianceDetail.TotalHoliday = oReader.GetInt32("TotalHoliday");
            oELEncashComplianceDetail.TotalEarnLeave = oReader.GetDouble("TotalEarnLeave");
            oELEncashComplianceDetail.EncashELCount = oReader.GetDouble("EncashELCount");
            oELEncashComplianceDetail.CompGrossSalary = oReader.GetDouble("CompGrossSalary");
            oELEncashComplianceDetail.CompBasicAmount = oReader.GetDouble("CompBasicAmount");
            oELEncashComplianceDetail.EncashAmount = oReader.GetDouble("EncashAmount");
            oELEncashComplianceDetail.Stamp = oReader.GetDouble("Stamp");
            oELEncashComplianceDetail.ApproveBy = oReader.GetInt32("ApproveBy");
            oELEncashComplianceDetail.ApproveDate = oReader.GetDateTime("ApproveDate");
            oELEncashComplianceDetail.PresencePerLeave = oReader.GetInt32("PresencePerLeave");
            oELEncashComplianceDetail.EmployeeName = oReader.GetString("EmployeeName");
            oELEncashComplianceDetail.LocationName = oReader.GetString("LocationName");
            oELEncashComplianceDetail.DepartmentName = oReader.GetString("DepartmentName");
            oELEncashComplianceDetail.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oELEncashComplianceDetail.DesignationName = oReader.GetString("DesignationName");
            oELEncashComplianceDetail.BusinessUnitAddress = oReader.GetString("BusinessUnitAddress");
            oELEncashComplianceDetail.EmployeeCode = oReader.GetString("EmployeeCode");
            oELEncashComplianceDetail.DeclarationDate = oReader.GetDateTime("DeclarationDate");
            oELEncashComplianceDetail.StartDate = oReader.GetDateTime("StartDate");
            oELEncashComplianceDetail.EndDate = oReader.GetDateTime("EndDate");
            oELEncashComplianceDetail.JoiningDate = oReader.GetDateTime("JoiningDate");
            return oELEncashComplianceDetail;

        }

        private ELEncashComplianceDetail CreateObject(NullHandler oReader)
        {
            ELEncashComplianceDetail oELEncashComplianceDetail = MapObject(oReader);
            return oELEncashComplianceDetail;
        }

        private List<ELEncashComplianceDetail> CreateObjects(IDataReader oReader)
        {
            List<ELEncashComplianceDetail> oELEncashComplianceDetail = new List<ELEncashComplianceDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ELEncashComplianceDetail oItem = CreateObject(oHandler);
                oELEncashComplianceDetail.Add(oItem);
            }
            return oELEncashComplianceDetail;
        }


        #endregion

        #region Interface implementation
        public ELEncashComplianceDetailService() { }
        public ELEncashComplianceDetail Get(string sSQL, Int64 nUserId)
        {
            ELEncashComplianceDetail oELEncashComplianceDetail = new ELEncashComplianceDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ELEncashComplianceDetailDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELEncashComplianceDetail = CreateObject(oReader);
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

            return oELEncashComplianceDetail;
        }

        public List<ELEncashComplianceDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ELEncashComplianceDetail> oELEncashComplianceDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ELEncashComplianceDetailDA.Gets(sSQL, tc);
                oELEncashComplianceDetail = CreateObjects(reader);
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
            return oELEncashComplianceDetail;
        }
        #endregion
    }
}

