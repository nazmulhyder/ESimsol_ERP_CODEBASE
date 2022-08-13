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
    public class EmployeeDatabaseService : MarshalByRefObject, IEmployeeDatabaseService
    {
        #region Private functions and declaration
        private EmployeeDatabase MapObject(NullHandler oReader)
        {
            EmployeeDatabase oE = new EmployeeDatabase();
            oE.EmployeeID = oReader.GetInt32("EmployeeID");
            oE.EmployeeStructureID = oReader.GetInt32("EmployeeStructureID");
            oE.EmployeeCode = oReader.GetString("EmployeeCode");
            oE.Thana = oReader.GetString("Thana");
            oE.District = oReader.GetString("District");
            oE.Village = oReader.GetString("Village");
            oE.PostOffice = oReader.GetString("PostOffice");
            oE.PostCode = oReader.GetString("PostCode");
            oE.Email = oReader.GetString("Email");
            oE.GroupName = oReader.GetString("GroupName");
            oE.BlockName = oReader.GetString("BlockName");
            oE.NationalID = oReader.GetString("NationalID");
            oE.BirthID = oReader.GetString("BirthID");
            oE.EmployeeName = oReader.GetString("EmployeeName");
            oE.FatherName = oReader.GetString("FatherName");
            oE.MotherName = oReader.GetString("MotherName");
            oE.PresentAddress = oReader.GetString("PresentAddress");
            oE.PermanentAddress = oReader.GetString("PermanentAddress");
            oE.DateOfBirth = oReader.GetDateTime("DateOfBirth");
            oE.Gender = oReader.GetString("Gender");
            oE.BloodGroup = oReader.GetString("BloodGroup");
            oE.Religion = oReader.GetString("Religion");
            oE.DepartmentName = oReader.GetString("Department");
            oE.DesignationName = oReader.GetString("Designation");
            oE.LocationName = oReader.GetString("LocationName");
            oE.ContactNo = oReader.GetString("ContactNo");
            oE.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oE.DateOfConf = oReader.GetDateTime("DateOfConf");
            oE.AttendanceScheme = oReader.GetString("AttendanceScheme");
            oE.ShiftName = oReader.GetString("ShiftName");
            oE.CardNo = oReader.GetString("CardNo");
            oE.salaryScheme = oReader.GetString("salaryScheme");
            oE.Gross = oReader.GetDouble("Gross");
            oE.CompGross = oReader.GetDouble("CompGross");
            oE.BankName = oReader.GetString("BankName");
            oE.AccountNo = oReader.GetString("AccountNo");
            oE.PFScheme = oReader.GetString("PFScheme");
            oE.PFMemberDate = oReader.GetDateTime("PFMemberDate");
            oE.PIScheme = oReader.GetString("PIScheme");
            oE.TAXPerMonth = oReader.GetDouble("TAXPerMonth");
            oE.LoanBalance = oReader.GetDouble("LoanBalance");

            oE.NomineeName = oReader.GetString("NomineeName");
            oE.NomineeContact = oReader.GetString("NomineeContact");
            oE.NomineeRelation = oReader.GetString("NomineeRelation");
            oE.EmployeeCategory = (EnumEmployeeCategory)oReader.GetInt16("EmployeeCategory");
            oE.EmployeeType = oReader.GetString("EmployeeType");
            oE.LastWorkingDate = oReader.GetDateTime("LastWorkingDate");
            oE.LastEducationDegree = oReader.GetString("LastEducationDegree");
            oE.ReportingPerson = oReader.GetString("ReportingPerson");

            oE.BankAmount = oReader.GetDouble("BankAmount");
            oE.CashAmount = oReader.GetDouble("CashAmount");
            oE.ETINNumber = oReader.GetString("ETINNumber"); ;
            oE.BUName = oReader.GetString("BUName"); ;
            return oE;
        }

        private EmployeeDatabase CreateObject(NullHandler oReader)
        {
            EmployeeDatabase oEmployeeDatabase_HRM = new EmployeeDatabase();
            oEmployeeDatabase_HRM = MapObject(oReader);
            return oEmployeeDatabase_HRM;
        }

        private List<EmployeeDatabase> CreateObjects(IDataReader oReader)
        {
            List<EmployeeDatabase> oEmployeeDatabase_HRMs = new List<EmployeeDatabase>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeDatabase oItem = CreateObject(oHandler);
                oEmployeeDatabase_HRMs.Add(oItem);
            }
            return oEmployeeDatabase_HRMs;
        }

        #endregion

        #region Interface implementation
        public EmployeeDatabaseService() { }
        public List<EmployeeDatabase> Gets(string sParam, Int64 nUserId)
        {
            List<EmployeeDatabase> oEmployeeDatabases = new List<EmployeeDatabase>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = EmployeeDatabaseDA.Gets(tc, sParam, nUserId);
                NullHandler oReader = new NullHandler(reader);
                oEmployeeDatabases = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeDatabase ", e);
                #endregion
            }
            return oEmployeeDatabases;
        }
        #endregion
    }
}
