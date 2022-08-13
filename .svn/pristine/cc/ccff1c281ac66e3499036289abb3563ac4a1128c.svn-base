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
    public class BenefitOnAttendanceEmployeeLedgerService : MarshalByRefObject, IBenefitOnAttendanceEmployeeLedgerService
    {
        #region Private functions and declaration
        private BenefitOnAttendanceEmployeeLedger MapObject(NullHandler oReader)
        {
            BenefitOnAttendanceEmployeeLedger oBenefitOnAttendanceEmployeeLedger = new BenefitOnAttendanceEmployeeLedger();

            oBenefitOnAttendanceEmployeeLedger.BOAELID = oReader.GetInt32("BOAELID");
            oBenefitOnAttendanceEmployeeLedger.BOAEmployeeID = oReader.GetInt32("BOAEmployeeID");
            oBenefitOnAttendanceEmployeeLedger.AttendanceDate = oReader.GetDateTime("AttendanceDate");
            oBenefitOnAttendanceEmployeeLedger.BOAName = oReader.GetString("BOAName");
            oBenefitOnAttendanceEmployeeLedger.BenefitOn = (EnumBenefitOnAttendance)oReader.GetInt16("BenifitOn");
            oBenefitOnAttendanceEmployeeLedger.BenefitOnInt = (int)(EnumBenefitOnAttendance)oReader.GetInt16("BenifitOn");
            oBenefitOnAttendanceEmployeeLedger.StartTime = oReader.GetDateTime("StartTime");
            oBenefitOnAttendanceEmployeeLedger.JoiningDate = oReader.GetDateTime("JoiningDate");
            oBenefitOnAttendanceEmployeeLedger.EndTime = oReader.GetDateTime("EndTime");
            oBenefitOnAttendanceEmployeeLedger.EmployeeID = oReader.GetInt32("EmployeeID");
            oBenefitOnAttendanceEmployeeLedger.EmployeeCode = oReader.GetString("EmployeeCode");
            oBenefitOnAttendanceEmployeeLedger.EmployeeName = oReader.GetString("EmployeeName");
            oBenefitOnAttendanceEmployeeLedger.DepartmentName = oReader.GetString("DepartmentName");
            oBenefitOnAttendanceEmployeeLedger.DesignationName = oReader.GetString("DesignationName");
            
            return oBenefitOnAttendanceEmployeeLedger;
        }

        private BenefitOnAttendanceEmployeeLedger CreateObject(NullHandler oReader)
        {
            BenefitOnAttendanceEmployeeLedger oBenefitOnAttendanceEmployeeLedger = MapObject(oReader);
            return oBenefitOnAttendanceEmployeeLedger;
        }

        private List<BenefitOnAttendanceEmployeeLedger> CreateObjects(IDataReader oReader)
        {
            List<BenefitOnAttendanceEmployeeLedger> oBenefitOnAttendanceEmployeeLedgers = new List<BenefitOnAttendanceEmployeeLedger>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BenefitOnAttendanceEmployeeLedger oItem = CreateObject(oHandler);
                oBenefitOnAttendanceEmployeeLedgers.Add(oItem);
            }
            return oBenefitOnAttendanceEmployeeLedgers;
        }
        #endregion

        #region Interface implementation
        public BenefitOnAttendanceEmployeeLedgerService() { }
        public List<BenefitOnAttendanceEmployeeLedger> Gets(string sSQL, Int64 nUserID)
        {
            List<BenefitOnAttendanceEmployeeLedger> oBenefitOnAttendanceEmployeeLedger = new List<BenefitOnAttendanceEmployeeLedger>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BenefitOnAttendanceEmployeeLedgerDA.Gets(sSQL, tc);
                oBenefitOnAttendanceEmployeeLedger = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BenefitOnAttendanceEmployeeLedger", e);
                #endregion
            }
            return oBenefitOnAttendanceEmployeeLedger;
        }

        public List<BenefitOnAttendanceEmployeeLedger> BOA_ReportGets(string sSQL, Int64 nUserID)
        {
            List<BenefitOnAttendanceEmployeeLedger> oBenefitOnAttendanceEmployeeLedgers = new List<BenefitOnAttendanceEmployeeLedger>();
            BenefitOnAttendanceEmployeeLedger oBenefitOnAttendanceEmployeeLedger = new BenefitOnAttendanceEmployeeLedger();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BenefitOnAttendanceEmployeeLedgerDA.Gets(sSQL, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    oBenefitOnAttendanceEmployeeLedger = new BenefitOnAttendanceEmployeeLedger();
                    oBenefitOnAttendanceEmployeeLedger.EmployeeCode = oreader.GetString("EmployeeCode");
                    oBenefitOnAttendanceEmployeeLedger.EmployeeName = oreader.GetString("Name");
                    oBenefitOnAttendanceEmployeeLedger.BOAName = oreader.GetString("BOAName");
                    oBenefitOnAttendanceEmployeeLedger.TotalDay = oreader.GetInt32("TotalDay");
                    oBenefitOnAttendanceEmployeeLedgers.Add(oBenefitOnAttendanceEmployeeLedger);
                }
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                oBenefitOnAttendanceEmployeeLedgers = new List<BenefitOnAttendanceEmployeeLedger>();
                oBenefitOnAttendanceEmployeeLedger = new BenefitOnAttendanceEmployeeLedger();
                oBenefitOnAttendanceEmployeeLedger.ErrorMessage = e.Message;
                oBenefitOnAttendanceEmployeeLedgers.Add(oBenefitOnAttendanceEmployeeLedger);
                #endregion
            }
            return oBenefitOnAttendanceEmployeeLedgers;
        }
        #endregion

    }
}
