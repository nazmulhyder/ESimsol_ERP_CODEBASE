using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;
using System.Data.SqlClient;


namespace ESimSol.Services.DataAccess
{
    public class EmployeeDA
    {
        public EmployeeDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, Employee oE, int nEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Employee]"
                                    + "%n,%n,%s,%s,%s,%s,%s,%s,%s,%s,"
                                    + "%s,%s,%s,%s,%d,%s,%s,%s,"
                                    + "%s,%s,%n,%b,%s,%s,%s,"
                                    + "%s,%s,%s,%s,%s,%s,%s,"
                                    + "%n,%n, %s, %s,%s,%s,%s,"
                                    // bangla //
                                    + "%u,%u,%u,%u,%u,%u,"
                                    + "%u,%u,%u,%u,%u,"
                                    + "%u,%u,%u,%u,%u,"
                                    + "%u,%u,%u,%u,%u,%u,%u",
                                    oE.EmployeeID, oE.CompanyID, oE.Code, oE.Name, oE.NickName, oE.Gender, oE.MaritalStatus, oE.FatherName,oE.SpouseName, oE.MotherName,
                                    oE.ParmanentAddress, oE.PresentAddress, oE.ContactNo, oE.Email, oE.DateOfBirth, oE.BloodGroup, oE.Height, oE.Weight,
                                    oE.IdentificationMart, oE.Note, (int)oE.EmployeeDesignationType, oE.IsFather, oE.BirthID, oE.NationalID, oE.Religious,
                                    oE.Nationalism, oE.ChildrenInfo, oE.Village, oE.PostOffice, oE.Thana, oE.District, oE.PostCode, 
                                    nUserId, (int)nEnumDBOperation, oE.OtherPhoneNo, oE.PermVillage, oE.PermPostOffice, oE.PermThana, oE.PermDistrict,
                                    // bangla //
                                    oE.CodeBangla, oE.NameInBangla, oE.FatherNameBangla, oE.MotherNameBangla, oE.NationalityBangla, oE.NationalIDBangla,
                                    oE.BloodGroupBangla , oE.HeightBangla , oE.WeightBangla , oE.DistrictBangla , oE.ThanaBangla ,
                                    oE.PostOfficeBangla , oE.VillageBangla , oE.PresentAddressBangla , oE.PermDistrictBangla , oE.PermThanaBangla ,
                                    oE.PermPostOfficeBangla , oE.PermVillageBangla , oE.PermanentAddressBangla , oE.ReligionBangla , oE.MaritalStatusBangla,
                                    oE.NomineeBangla, oE.AuthenticationBangla
                                    );
        }

        public static void UpdatePhoto(TransactionContext tc, Employee oE, Int64 nUserID)
        {

            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oE.Photo;

            string sSQL = SQLParser.MakeSQL("UPDATE Employee SET Photo=%q"
                + " WHERE EmployeeID=%n", "@Photopic", oE.EmployeeID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }

        public static IDataReader Activity(int nEmployeeID, Int64 nUserId, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeActiveInactiveHistory]"
                                   + "%n,%n",
                                   nEmployeeID, nUserId);

            //return tc.ExecuteReader("UPDATE Employee SET IsActive=~IsActive WHERE EmployeeID=%n;SELECT * FROM View_Employee WHERE EmployeeID=%n", nEmployeeID, nEmployeeID);
        }

        public static IDataReader EmployeeWorkingStatusChange(int EmployeeID, Int64 nUserId, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_EmployeeWorkingStatus]"
                                    + "%n,%n",
                                    EmployeeID, nUserId);
        }

        public static IDataReader ContinuedEmployee(int EmployeeID, Int64 nUserId, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_ContinuedEmployee]"
                                    + "%n,%n",
                                    EmployeeID, nUserId);
        }

        public static IDataReader UploadXL(TransactionContext tc, Employee_UploadXL oEXL, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_EmployeeBasicInformation] %s,%s,%s,%s,%s,%d,%s,%s,%s,%s,%s,%d,%d,%s,%s,%n,%s,%s,%s,%n,%s,%s,%n,%b,%n",
                   oEXL.Code,
                   oEXL.Name,
                   oEXL.FathersName,
                   oEXL.MothersName,
                   oEXL.Address,
                   oEXL.DateOfBirth,
                   oEXL.Gender,
                   oEXL.LocationCode,
                   oEXL.AttScheme,
                   oEXL.DepartmentCode,
                   oEXL.DesignationCode,
                   oEXL.DateOfJoin,
                   oEXL.DateOfConfirmation,
                   oEXL.ShiftCode,
                   oEXL.SalaryScheme,
                   oEXL.GrossSalary,
                   oEXL.ProximityCardNo,
                   oEXL.BankCode,
                   oEXL.ACNo,
                   oEXL.IsUser,
                   oEXL.Category,
                   oEXL.BUCode,
                   nUserID,
                   oEXL.IsCashFixed,
                   oEXL.CashAmount);
        }

        public static IDataReader UploadEmpBasicXL(TransactionContext tc, Employee_UploadXL oEXL, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Upload_EmployeeUpdateInformation] %s,%u,%s,%s,%u,%u,%u,%s,%u,%u,%u,%d,%u,%u,%u,%u,%u,%s,%s",
                   oEXL.Code,
                   oEXL.NameInBangla,
                   oEXL.ContactNo,
                   oEXL.Email,
                   oEXL.FathersName,
                   oEXL.HusbandName,
                   oEXL.MothersName,
                   oEXL.BloodGroup,
                   oEXL.NationalID,
                   oEXL.PresentAdd,
                   oEXL.PermanentAdd,
                   oEXL.DateOfBirth,
                   oEXL.Village,
                   oEXL.PostOffice,
                   oEXL.Thana,
                   oEXL.District,
                   oEXL.PostCode,
                   oEXL.Grade,
                   oEXL.ProximityCardNo);
        }

        public static IDataReader UploadEmpBasicXLWithConfig(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader EditDateOfJoin(TransactionContext tc, Employee oE, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_Update_DateOfJoin]"
                                    + "%n,%d,%d,%n",
                                    oE.EmployeeID, oE.DateOfJoin,oE.ConfirmationDate, nUserId);

        }
        public static IDataReader GetsManPower(TransactionContext tc, string sBUIDs, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_GetsManpower]"
                                    + "%s,%n",
                                    sBUIDs, nUserId);

        }

        #endregion

        #region Employee Signature
        public static void SaveSignature(TransactionContext tc, int nEmployeeID, byte[] imgSingnature, Int64 nUserID)
        {
            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = imgSingnature;

            string sSQL = SQLParser.MakeSQL("UPDATE Employee SET [Signature]=%q"
                + " WHERE EmployeeID=%n", "@Photopic", nEmployeeID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }
        public static IDataReader EmployeeRecontract(int EmployeeID, DateTime StartDate, DateTime EndDate, string sNewCode, int nCategory, Int64 nUserID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeRecontact] %n,%d,%d,%s,%n,%n", EmployeeID, StartDate, EndDate, sNewCode, nCategory, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_Employee_WithImage WHERE EmployeeID=%n", nEmployeeID);
        }
        public static IDataReader GetByCode(TransactionContext tc, string sEmpCode)
        {
            return tc.ExecuteReader("SELECT * FROM View_Employee AS HH WHERE HH.Code = %s", sEmpCode);
        }
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Employee Order By EmployeeID DESC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int eEmployeeType)
        {
            return tc.ExecuteReader("SELECT * FROM View_Employee WHERE EmployeeDesignationType=%n", (int)eEmployeeType);
        }

        public static IDataReader BUGets(TransactionContext tc, int eEmployeeType, int BUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Employee WHERE EmployeeDesignationType=%n AND BusinessUnitID = %n AND ISNULL(IsActive,1)=%b", (int)eEmployeeType, BUID, true);
        }
        public static IDataReader Gets(TransactionContext tc, int eEmployeeType, int nLocationID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Employee WHERE EmployeeDesignationType=%n AND EmployeeID IN (SELECT EmployeeID FROM View_EmployeeOfficial WHERE LocationID=%n)", (int)eEmployeeType, nLocationID);
        }
        public static IDataReader GetsByOperationEvent(TransactionContext tc, int nLocationID, string sObjectName, string sOperaationEvent)
        {
            return tc.ExecuteReader(" Select * from View_Employee where EmployeeID in (Select EmployeeID from Users where Users.LocationID=%n and UserID In "
      + " ( SELECT UserID FROM View_AuthorizationUserOEDO as UA WHERE UA.IsActive=1 and IsMTRApply=0 and  DBObjectName=%s  AND OEValue in (%q)))", nLocationID, sObjectName, sOperaationEvent);
        }
        public static IDataReader GetsforPOP(TransactionContext tc, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_Employee WHERE  EmployeeID IN ("
                                + " SELECT EmployeeID FROM View_EmployeeOfficial WHERE LocationID IN ("
                                + " SELECT LocationID FROM Users WHERE UserID=%n)) order by Name", nUserId);
        }
        #endregion

        #region Shift transfer
        public static IDataReader TransferShift(TransactionContext tc, string sEmployeeIDs, int nCurrentShiftID, DateTime dDate)
        {
            return tc.ExecuteReader("EXEC [SP_ShiftTransfer]" + "%s, %n,%d", sEmployeeIDs, nCurrentShiftID, dDate);
        }
        #endregion
     
        #region Generated EmpCode
        public static string GetGeneratedEmpCode(TransactionContext tc, int nDRPId, int nDesignationId, DateTime JoinningDate, int nCompanyId)
        {
            string sEmpCode = "";
            object obj = tc.ExecuteScalar("SELECT [dbo].[FN_EmployeeCode] (%n,%n,%d,%n)", nDRPId, nDesignationId, JoinningDate, nCompanyId);
            if (obj == null)
            {
                return "";
            }
            else
            {
                 sEmpCode = Convert.ToString(obj);
              
            }
            return sEmpCode;
        }
        #endregion Generated EmpCode

        #region SwapShift
        public static IDataReader SwapShift(TransactionContext tc, int nRosterPlanID,DateTime dDate)
        {
            return tc.ExecuteReader("EXEC [SP_ShiftSwap]" + "%n,%d", nRosterPlanID, dDate);
        }
        #endregion SwapShift
    }

}
