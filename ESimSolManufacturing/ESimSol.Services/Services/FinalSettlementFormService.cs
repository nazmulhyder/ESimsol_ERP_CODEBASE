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
    public class FinalSettlementFormService : MarshalByRefObject, IFinalSettlementFormService
    {
        #region Private functions and declaration
        private FinalSettlementForm MapObject(NullHandler oReader)
        {
            FinalSettlementForm oFSF = new FinalSettlementForm();
            oFSF.EmployeeID = oReader.GetInt32("EmployeeID");
            oFSF.EmployeeName = oReader.GetString("EmployeeName");
            oFSF.EmployeeCode = oReader.GetString("EmployeeCode");
            oFSF.DepartmentName = oReader.GetString("DepartmentName");
            oFSF.DesignationName = oReader.GetString("DesignationName");
            oFSF.DateOfBirth = oReader.GetDateTime("DateOfBirth");
            oFSF.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oFSF.DateOfConfirmation = oReader.GetDateTime("DateOfConfirmation");
            oFSF.SettlementType = (EnumSettleMentType)oReader.GetInt16("SettlementType");
            oFSF.DateOfSubmission = oReader.GetDateTime("DateOfSubmission");
            oFSF.DateOfEffect = oReader.GetDateTime("DateOfEffect");
            oFSF.SalaryStartDate = oReader.GetDateTime("SalaryStartDate");
            oFSF.SalaryMonth = oReader.GetString("SalaryMonth");
            oFSF.SettMonth = oReader.GetString("SettMonth");
            oFSF.TotalAbsent = oReader.GetInt32("TotalAbsent");
            //oFSF.SickLeave = oReader.GetDouble("SickLeave");
            //oFSF.CasualLeave = oReader.GetDouble("CasualLeave");
            //oFSF.EarnLeave = oReader.GetDouble("EarnLeave");
            oFSF.LeaveStatus = oReader.GetString("LeaveStatus");
            oFSF.OT_HHR = oReader.GetInt32("OT_HHR");
            oFSF.OT_NHR = oReader.GetInt32("OT_NHR");
            oFSF.TotalNW = oReader.GetInt32("TotalNW");
            oFSF.TotalEL = oReader.GetInt32("TotalEL");
            oFSF.EnjoyedEl = oReader.GetInt32("EnjoyedEl");
            oFSF.RefCode = oReader.GetString("RefCode");
            oFSF.IsNoticePay = oReader.GetBoolean("IsNoticePay");
            oFSF.TotalBenefitedDays = oReader.GetInt32("TotalBenefitedDays");

            return oFSF;
        }

        private FinalSettlementForm CreateObject(NullHandler oReader)
        {
            FinalSettlementForm oFinalSettlementForm = new FinalSettlementForm();
            oFinalSettlementForm = MapObject(oReader);
            return oFinalSettlementForm;
        }

        private List<FinalSettlementForm> CreateObjects(IDataReader oReader)
        {
            List<FinalSettlementForm> oFinalSettlementForm = new List<FinalSettlementForm>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FinalSettlementForm oItem = CreateObject(oHandler);
                oFinalSettlementForm.Add(oItem);
            }
            return oFinalSettlementForm;
        }

        #endregion

        #region Interface implementation
        public FinalSettlementFormService() { }
        public List<FinalSettlementForm> Gets(int nEmployeeID, Int64 nUserID)
        {
            List<FinalSettlementForm> oFinalSettlementForm = new List<FinalSettlementForm>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FinalSettlementFormDA.Gets(tc, nEmployeeID);
                oFinalSettlementForm = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException((e.Message.Contains("!"))? e.Message.Split('!')[0]:e.Message);
                #endregion
            }

            return oFinalSettlementForm;
        }

        #endregion
    }
}