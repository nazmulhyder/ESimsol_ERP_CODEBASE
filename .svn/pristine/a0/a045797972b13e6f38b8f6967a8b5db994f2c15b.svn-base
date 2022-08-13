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
    public class EmployeeService : MarshalByRefObject, IEmployeeService
    {
        #region Private functions and declaration
        private Employee MapObject(NullHandler oReader)
        {
            Employee oE = new Employee();
            oE.EmployeeID = oReader.GetInt32("EmployeeID");
            oE.Male = oReader.GetInt32("Male");
            oE.Female = oReader.GetInt32("Female");
            oE.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oE.DesignationID = oReader.GetInt32("DesignationID");
            oE.DRPID = oReader.GetInt32("DRPID");
            oE.CompanyID = oReader.GetInt32("CompanyID");
            oE.Code = oReader.GetString("Code");
            oE.OtherPhoneNo = oReader.GetString("OtherPhoneNo");
            oE.Name = oReader.GetString("Name");
            oE.NickName = oReader.GetString("NickName");
            oE.Gender = oReader.GetString("Gender");
            oE.MaritalStatus = oReader.GetString("MaritalStatus");
            oE.FatherName = oReader.GetString("FatherName");
            oE.SpouseName = oReader.GetString("SpouseName");
            oE.MotherName = oReader.GetString("MotherName");
            oE.ParmanentAddress = oReader.GetString("ParmanentAddress");
            oE.PresentAddress = oReader.GetString("PresentAddress");
            oE.ContactNo = oReader.GetString("ContactNo");
            oE.Email = oReader.GetString("Email");
            oE.DateOfBirth = oReader.GetDateTime("DateOfBirth");
            oE.BloodGroup = oReader.GetString("BloodGroup");
            oE.Height = oReader.GetString("Height");
            oE.Weight = oReader.GetString("Weight");
            oE.IdentificationMart = oReader.GetString("IdentificationMart");
            oE.Photo = oReader.GetBytes("Photo");
            oE.Signature = oReader.GetBytes("Signature");
            oE.Note = oReader.GetString("Note");
            oE.Attachment = oReader.GetBytes("Attachment");
            oE.IsActive = oReader.GetBoolean("IsActive");
            oE.EmployeeDesignationType = (EnumEmployeeDesignationType)oReader.GetInt32("EmployeeDesignationType");
            oE.EmployeeDesignationTypeInt = oReader.GetInt32("EmployeeDesignationType");
            //new
            oE.IsFather = oReader.GetBoolean("IsFather");
            oE.BirthID = oReader.GetString("BirthID");
            oE.NationalID = oReader.GetString("NationalID");
            oE.Religious = oReader.GetString("Religious");
            oE.Nationalism = oReader.GetString("Nationalism");
            oE.ChildrenInfo = oReader.GetString("ChildrenInfo");
            oE.Village = oReader.GetString("Village");
            oE.PostOffice = oReader.GetString("PostOffice");
            oE.Thana = oReader.GetString("Thana");
            oE.District = oReader.GetString("District");
            oE.PostCode = oReader.GetString("PostCode");
            //Derive Property
            oE.BUName = oReader.GetString("BUName");
            oE.LocationID = oReader.GetInt32("LocationID");
            oE.LocationName = oReader.GetString("LocationName");
            oE.DepartmentID = oReader.GetInt32("DepartmentID");
            oE.DepartmentName = oReader.GetString("DepartmentName");
            oE.DesignationName = oReader.GetString("DesignationName");
            oE.WorkingStatus = (EnumEmployeeWorkigStatus)oReader.GetInt32("WorkingStatus");
            oE.RosterPlanID = oReader.GetInt32("RosterPlanID");
            oE.RosterPlanDescription = oReader.GetString("RosterPlanDescription");
            oE.CurrentShift = oReader.GetString("CurrentShift");
            oE.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            oE.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            oE.EmployeeCardStatus = (EnumEmployeeCardStatus)oReader.GetInt32("EmployeeCardStatus");
            oE.DateOfJoin = oReader.GetDateTime("JoiningDate");
            oE.ConfirmationDate = oReader.GetDateTime("ConfirmationDate");
            oE.EmployeeCategory = (EnumEmployeeCategory)oReader.GetInt16("EmployeeCategory");
            oE.EncryptEmpID = Global.Encrypt(oE.EmployeeID.ToString());
            oE.CardNo = oReader.GetString("CardNo");
            oE.DepartmentNameInBangla = oReader.GetString("DepartmentNameInBangla");
            oE.LocationNameInBangla = oReader.GetString("LocationNameInBangla");
            oE.BusinessUnitNameInBangla = oReader.GetString("BusinessUnitNameInBangla");
            oE.BusinessUnitName = oReader.GetString("BusinessUnitName");            
            oE.DesignationName = oReader.GetString("DesignationName");
            oE.DesignationNameInBangla = oReader.GetString("DesignationNameInBangla");
            oE.BUAddressInBangla = oReader.GetString("BUAddressInBangla");
            oE.BUAddress = oReader.GetString("BUAddress");
            oE.BUPhone = oReader.GetString("BUPhone");
            oE.BUFaxNo = oReader.GetString("BUFaxNo");
            oE.ErrorMessage = oReader.GetString("ErrorMessage");
            oE.PermVillage = oReader.GetString("PermVillage");
            oE.PermPostOffice = oReader.GetString("PermPostOffice");
            oE.PermThana = oReader.GetString("PermThana");
            oE.PermDistrict = oReader.GetString("PermDistrict");
            oE.NameInBangla = oReader.GetString("NameInBangla");
            oE.CodeBangla = oReader.GetString("CodeBangla");
            oE.FatherNameBangla = oReader.GetString("FatherNameBangla");
            oE.MotherNameBangla = oReader.GetString("MotherNameBangla");
            oE.NationalityBangla = oReader.GetString("NationalityBangla");
            oE.NationalIDBangla = oReader.GetString("NationalIDBangla");
            oE.BloodGroupBangla = oReader.GetString("BloodGroupBangla");
            oE.HeightBangla = oReader.GetString("HeightBangla");
            oE.WeightBangla = oReader.GetString("WeightBangla");
            oE.DistrictBangla = oReader.GetString("DistrictBangla");
            oE.ThanaBangla = oReader.GetString("ThanaBangla");
            oE.VillageBangla = oReader.GetString("VillageBangla");
            oE.PresentAddressBangla = oReader.GetString("PresentAddressBangla");
            oE.PermDistrictBangla = oReader.GetString("PermDistrictBangla");
            oE.PermThanaBangla = oReader.GetString("PermThanaBangla");
            oE.PostOfficeBangla = oReader.GetString("PostOfficeBangla");
            oE.PermPostOfficeBangla = oReader.GetString("PermPostOfficeBangla");
            oE.PermVillageBangla = oReader.GetString("PermVillageBangla");
            oE.PermanentAddressBangla = oReader.GetString("PermanentAddressBangla");
            oE.ReligionBangla = oReader.GetString("ReligionBangla");
            oE.MaritalStatusBangla = oReader.GetString("MaritalStatusBangla");
            oE.NomineeBangla = oReader.GetString("NomineeBangla");
            oE.AuthenticationBangla = oReader.GetString("AuthenticationBangla");
            return oE;

        }

        private Employee CreateObject(NullHandler oReader)
        {
            Employee oEmployee_HRM = new Employee();
            oEmployee_HRM = MapObject(oReader);
            return oEmployee_HRM;
        }

        private List<Employee> CreateObjects(IDataReader oReader)
        {
            List<Employee> oEmployee_HRMs = new List<Employee>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Employee oItem = CreateObject(oHandler);
                oEmployee_HRMs.Add(oItem);
            }
            return oEmployee_HRMs;
        }

        #endregion

        #region Interface implementation
        public EmployeeService() { }

        public Employee IUD(Employee oEmployee_HRM, int nEnumDBOperation, Int64 nUserID)
        {
            Employee oTempCP = new Employee();
            oTempCP.Photo = oEmployee_HRM.Photo;
            oTempCP.Signature = oEmployee_HRM.Signature;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeDA.IUD(tc, oEmployee_HRM, nEnumDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployee_HRM = new Employee();
                    oEmployee_HRM = CreateObject(oReader);
                }
                reader.Close();
                oTempCP.EmployeeID = oEmployee_HRM.EmployeeID;
                if (oTempCP.Photo != null)
                {
                    EmployeeDA.UpdatePhoto(tc, oTempCP, nUserID);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Employee. Because of " + e.Message, e);
                oEmployee_HRM.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployee_HRM;
        }

        public string DeleteImage(Employee oE, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeDA.UpdatePhoto(tc, oE, nUserId);

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ContractorPersonal. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public string EmployeeImageIU(Employee oE, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeDA.UpdatePhoto(tc, oE, nUserId);

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ContractorPersonal. Because of " + e.Message, e);
                #endregion
            }
            return "Image Saved Successfully.";
        }

        public Employee Get(int nEmployeeid, Int64 nUserId)
        {
            Employee oEmployee_HRM = new Employee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeDA.Get(tc, nEmployeeid);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployee_HRM = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Employee ", e);
                #endregion
            }

            return oEmployee_HRM;
        }

        public Employee GetByCode(string sEmpCode, Int64 nUserId)
        {
            Employee oEmployee_HRM = new Employee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeDA.GetByCode(tc, sEmpCode);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployee_HRM = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Employee ", e);
                #endregion
            }

            return oEmployee_HRM;
        }

        public Employee Get(string sSQL, Int64 nUserId)
        {
            Employee oEmployee_HRM = new Employee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployee_HRM = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Employee ", e);
                #endregion
            }

            return oEmployee_HRM;
        }

        public List<Employee> Gets(Int64 nUserId)
        {
            List<Employee> oEmployee_HRM = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDA.Gets(tc);
                oEmployee_HRM = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Employees ", e);
                #endregion
            }

            return oEmployee_HRM;
        }

        public List<Employee> Gets(string sSQL, Int64 nUserId)
        {
            List<Employee> oEmployee_HRM = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDA.Get(tc, sSQL);
                oEmployee_HRM = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Employees ", e);
                #endregion
            }

            return oEmployee_HRM;
        }


        public List<Employee> GetsManPower(string sBUIDs, Int64 nUserId)
        {
            List<Employee> oEmployees = new List<Employee>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDA.GetsManPower(tc, sBUIDs, nUserId);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    Employee oItem = new Employee();
                    oItem.BusinessUnitName = oreader.GetString("BUName");
                    oItem.LocationName = oreader.GetString("LocName");
                    oItem.DepartmentName = oreader.GetString("DptName");
                    oItem.Male = oreader.GetInt32("Male");
                    oItem.Female = oreader.GetInt32("Female");
                    oEmployees.Add(oItem);
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
                throw new ServiceException("Failed to Get Employees ", e);
                #endregion
            }

            return oEmployees;
        }

        public List<Employee> GetsforPOP(Int64 nUserId)
        {
            List<Employee> oEmployee = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDA.GetsforPOP(tc, nUserId);
                oEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Employee", e);
                #endregion
            }

            return oEmployee;
        }

        public List<Employee> Gets(int eEmployeeType, Int64 nUserId)
        {
            List<Employee> oEmployee = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDA.Gets(tc, eEmployeeType);
                oEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Employee", e);
                #endregion
            }

            return oEmployee;
        }


        public List<Employee> BUGets(int eEmployeeType, int BUID, Int64 nUserId)
        {
            List<Employee> oEmployee = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDA.BUGets(tc, eEmployeeType, BUID);
                oEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Employee", e);
                #endregion
            }

            return oEmployee;
        }

        public List<Employee> Gets(int eEmployeeType, int nLocationID, Int64 nUserId)
        {
            List<Employee> oEmployee = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDA.Gets(tc, eEmployeeType, nLocationID);
                oEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Employee", e);
                #endregion
            }

            return oEmployee;
        }

        public List<Employee> GetsByOperationEvent(int nLocationID, string sObjectName, string sOperaationEvent, Int64 nUserId)
        {
            List<Employee> oEmployee = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDA.GetsByOperationEvent(tc, nLocationID, sObjectName, sOperaationEvent);
                oEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Employee", e);
                #endregion
            }

            return oEmployee;
        }
        #region Transfer Shift

        public List<Employee> TransferShift(string sEmployeeIDs, int nCurrentShiftID,DateTime dDate, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<Employee> oEmps = new List<Employee>();
            Employee oEmp = new Employee();
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeDA.TransferShift(tc, sEmployeeIDs, nCurrentShiftID, dDate);
                oEmps = CreateObjects(reader);
                reader.Close();
                tc.End();
            }

            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Transfer Shift", e);
                oEmp = new Employee();
                oEmps = new List<Employee>();
                oEmp.ErrorMessage = e.Message;
                oEmps.Add(oEmp);
                #endregion
            }
            return oEmps;
        }
        #endregion

        #region Activity
        public List<Employee> Activite(string sEmpIDs, Int64 nUserId)
        {
            Employee oEmployee = new Employee();
            List<Employee> oEmployees = new List<Employee>();
            string[] nEmpIDs;
            nEmpIDs = sEmpIDs.Split(',');

            TransactionContext tc = null;
            try
            {
                foreach (string NEmpID in nEmpIDs)
                {
                    tc = TransactionContext.Begin();
                    int NID = Convert.ToInt32(NEmpID);
                    IDataReader reader = EmployeeDA.Activity(NID, nUserId, tc);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oEmployee = CreateObject(oReader);
                    }
                    reader.Close();
                    oEmployees.Add(oEmployee);
                    tc.End();
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //oEmployee.ErrorMessage = e.Message;
                throw new ServiceException(e.Message.Split('!')[0]);
                #endregion
            }

            return oEmployees;
        }


        #endregion

        #region EmployeeWorkingStatusChange
        public List<Employee> EmployeeWorkingStatusChange(string sEmpIDs, Int64 nUserId)
        {
            Employee oEmployee = new Employee();
            List<Employee> oEmployees = new List<Employee>();
            string[] nEmpIDs;
            nEmpIDs = sEmpIDs.Split(',');

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                foreach (string NEmpID in nEmpIDs)
                {
                    int NID = Convert.ToInt32(NEmpID);
                    IDataReader reader = EmployeeDA.EmployeeWorkingStatusChange(NID, nUserId, tc);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oEmployee = CreateObject(oReader);
                    }
                    reader.Close();
                    oEmployees.Add(oEmployee);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //oEmployee.ErrorMessage = e.Message;
                throw new ServiceException(e.Message.Split('!')[0]);
                #endregion
            }

            return oEmployees;
        }

        public List<Employee> ContinuedEmployee(string sEmpIDs, Int64 nUserId)
        {
            Employee oEmployee = new Employee();
            List<Employee> oEmployees = new List<Employee>();
            string[] nEmpIDs;
            nEmpIDs = sEmpIDs.Split(',');
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                foreach (string NEmpID in nEmpIDs)
                {
                    int NID = Convert.ToInt32(NEmpID);
                    IDataReader reader = EmployeeDA.ContinuedEmployee(NID, nUserId, tc);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oEmployee = CreateObject(oReader);
                    }
                    reader.Close();
                    oEmployees.Add(oEmployee);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                oEmployee.ErrorMessage = e.Message;
                oEmployees.Add(oEmployee);
                #endregion
            }

            return oEmployees;
        }


        #endregion

        public Employee EmployeeBasicInformation_IUD(Employee oEmployee, int nDBOperation, Int64 nUserID)
        {
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            oEmployeeOfficial = oEmployee.EmployeeOfficial;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeDA.IUD(tc, oEmployee, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployee = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployee.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployee.EmployeeID = 0;
                #endregion
            }

            return oEmployee;
        }

        #region Generated EmpCode

        public string GetGeneratedEmpCode(int nDRPId, int nDesignationId, DateTime JoinningDate, int nCompanyId, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sEmpCode = "";
            try
            {
                tc = TransactionContext.Begin();
                sEmpCode = EmployeeDA.GetGeneratedEmpCode(tc, nDRPId, nDesignationId, JoinningDate, nCompanyId);
                tc.End();
            }

            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Transfer Shift", e);
                sEmpCode = e.Message;

                #endregion
            }
            return sEmpCode;
        }
        #endregion Generated EmpCode

        public Employee SaveSignature(int nEmployeeID, byte[] imgSingnature, Int64 nUserID)
        {
            Employee oEmployee = new Employee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                EmployeeDA.SaveSignature(tc, nEmployeeID, imgSingnature, nUserID);

                IDataReader reader = EmployeeDA.Get(tc, nEmployeeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployee = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }

            catch (Exception ex)
            {
                #region Handle Exception
                oEmployee.ErrorMessage = ex.Message;
                #endregion
            }
            return oEmployee;
        }

        public Employee RemoveSignature(int nEmployeeID, Int64 nUserID)
        {
            Employee oEmployee = new Employee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                string sSQL = "Update Employee Set [Signature]=null where EmployeeID=" + nEmployeeID + "  Select * from View_Employee Where EmployeeID=" + nEmployeeID + "";
                IDataReader reader = EmployeeDA.Gets(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployee = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (oEmployee.EmployeeID <= 0) { throw new Exception("No valid employee found."); }
            }

            catch (Exception ex)
            {
                #region Handle Exception
                oEmployee.ErrorMessage = ex.Message;
                #endregion
            }
            return oEmployee;
        }

        #region Swap Shift
        public Employee SwapShift( int nRosterPlanID,DateTime dDate, Int64 nUserID)
        {
            TransactionContext tc = null;
            Employee oEmp = new Employee();
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeDA.SwapShift(tc, nRosterPlanID, dDate);
                reader.Close();
                tc.End();
                oEmp.ErrorMessage = "Swaped Successfully!";
            }

            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Transfer Shift", e);
                oEmp = new Employee();
                oEmp.ErrorMessage = e.Message;
                #endregion
            }
            return oEmp;
        }
        #endregion Swap Shift

        #region UploadXL
        public List<Employee> UploadXL(List<Employee_UploadXL> oEXLs, Int64 nUserID)
        {
            Employee oTempEmp = new Employee();
            List<Employee> oTempEmps = new List<Employee>();
            List<Employee> oTempList = new List<Employee>();
            TransactionContext tc = null;

            foreach (Employee_UploadXL oItem in oEXLs)
            {
                try
                {
                    tc = TransactionContext.Begin(true);

                    oTempEmps = new List<Employee>();
                    IDataReader reader = null;
                    reader = EmployeeDA.UploadXL(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    //if (reader.RecordsAffected <= 0)
                    //{
                    //    oTempList.Add(oItem);
                    //}
                    if (reader.Read())
                    {
                        oTempEmp = CreateObject(oReader);
                    }

                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    if (tc != null)
                        tc.HandleError();
                    Employee oEmp = new Employee();
                    oEmp.Code = oItem.Code;
                    oEmp.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                    oTempList.Add(oEmp);
                }
            }
            return oTempList;


            //try
            //{
            //    int nCount = 0;
            //    foreach (Employee_UploadXL oItem in oEXLs)
            //    {
            //        tc = TransactionContext.Begin(true);
            //        IDataReader reader;
            //        oTempEmp = new Employee();
            //        reader = EmployeeDA.UploadXL(tc, oItem, nUserID);
            //        if (nCount < 100)
            //        {
            //            NullHandler oReader = new NullHandler(reader);
            //            if (reader.Read())
            //            {
            //                oTempEmp = CreateObject(oReader);
            //            }

            //            reader.Close();
            //            tc.End();
            //            //if (reader.Read())
            //            //{
            //            //    oTempEmp = CreateObject(oReader);
            //            //}
            //            //if (oTempEmp.EmployeeID > 0)
            //            //{
            //            //    oTempEmps.Add(oTempEmp);
            //            //}
            //        }
            //        nCount++;
            //        reader.Close();
            //        tc.End();
            //    }
            //}
            //catch (Exception e)
            //{
            //    #region Handle Exception
            //    if (tc != null)
            //        tc.HandleError();
            //    throw new ServiceException(e.Message.Split('!')[0]);
            //    #endregion
            //}
            //return oTempEmps;
        }


        public List<Employee> UploadEmpBasicXL(List<Employee_UploadXL> oEXLs, Int64 nUserID)
        {
            Employee oTempEmp = new Employee();
            List<Employee> oTempEmps = new List<Employee>();
            TransactionContext tc = null;

            foreach (Employee_UploadXL oItem in oEXLs)
            {
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    oTempEmp = new Employee();
                    reader = EmployeeDA.UploadEmpBasicXL(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempEmp = CreateObject(oReader);
                        oTempEmp.Code = oItem.Code;
                    }
                    if (oTempEmp.ErrorMessage != "")
                    {
                        oTempEmps.Add(oTempEmp);
                    }
                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    //if (tc != null)
                    //    tc.HandleError();
                    oTempEmp = new Employee();
                    oTempEmp.Code = oItem.Code;
                    oTempEmp.ErrorMessage = e.Message.Split('!')[0];
                    oTempEmps.Add(oTempEmp);
                    //throw new ServiceException(e.Message.Split('!')[0]);
                    #endregion
                }
            }
            return oTempEmps;
        }
        public Employee UploadEmpBasicXLWithConfig(string sSql, Int64 nUserID)
        {
            Employee oTempEmp = new Employee();
            List<Employee> oTempEmps = new List<Employee>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oTempEmp = new Employee();
                reader = EmployeeDA.UploadEmpBasicXLWithConfig(tc, sSql);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTempEmp.EmployeeID = oReader.GetInt32("EmployeeID");
                    oTempEmp.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
                    oTempEmp.BankID = oReader.GetInt32("BankID");
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oTempEmp.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oTempEmp;
        }

        #endregion UploadXL

        public Employee EditDateOfJoin(Employee oEmployee, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeDA.EditDateOfJoin(tc, oEmployee, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployee = new Employee();
                    oEmployee = CreateObject(oReader);
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
                oEmployee.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oEmployee;
        }

        public Employee EmployeeRecontract(int EmployeeID, DateTime StartDate, DateTime EndDate, string sNewCode, int nCategory, Int64 nUserId)
        {
            Employee oEmployee = new Employee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = EmployeeDA.EmployeeRecontract(EmployeeID, StartDate, EndDate, sNewCode,nCategory, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployee = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployee.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployee;
        }

        #endregion
    }
}
