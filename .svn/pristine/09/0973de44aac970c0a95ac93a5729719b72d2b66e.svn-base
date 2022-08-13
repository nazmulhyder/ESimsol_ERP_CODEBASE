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
    public class EmployeeForIDCardService : MarshalByRefObject, IEmployeeForIDCardService
    {
        #region Private functions and declaration
        private EmployeeForIDCard MapObject(NullHandler oReader)
        {
            EmployeeForIDCard oEmployeeForIDCard = new EmployeeForIDCard();
            oEmployeeForIDCard.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeForIDCard.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oEmployeeForIDCard.DesignationID = oReader.GetInt32("DesignationID");
            oEmployeeForIDCard.DepartmentID = oReader.GetInt32("DepartmentID");
            oEmployeeForIDCard.LocationID = oReader.GetInt32("LocationID");
            oEmployeeForIDCard.DRPID = oReader.GetInt32("DRPID");



            oEmployeeForIDCard.Code = oReader.GetString("Code");
            oEmployeeForIDCard.CardNo = oReader.GetString("CardNo");
            oEmployeeForIDCard.DesignationNameInBangla = oReader.GetString("DesignationNameInBangla");
            oEmployeeForIDCard.DesignationName = oReader.GetString("DesignationName");
            oEmployeeForIDCard.BUFaxNo = oReader.GetString("BUFaxNo");
            oEmployeeForIDCard.BUPhone = oReader.GetString("BUPhone");
            oEmployeeForIDCard.BUAddress = oReader.GetString("BUAddress");
            oEmployeeForIDCard.BUAddressInBangla = oReader.GetString("BUAddressInBangla");
            oEmployeeForIDCard.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oEmployeeForIDCard.BusinessUnitNameInBangla = oReader.GetString("BusinessUnitNameInBangla");
            oEmployeeForIDCard.LocationNameInBangla = oReader.GetString("LocationNameInBangla");
            oEmployeeForIDCard.DepartmentNameInBangla = oReader.GetString("DepartmentNameInBangla");
            oEmployeeForIDCard.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeForIDCard.LocationName = oReader.GetString("LocationName");
            oEmployeeForIDCard.OtherPhoneNo = oReader.GetString("OtherPhoneNo");
            oEmployeeForIDCard.NameInBangla = oReader.GetString("NameInBangla");
            oEmployeeForIDCard.PermVillageBangla = oReader.GetString("PermVillageBangla");
            oEmployeeForIDCard.PermPostOfficeBangla = oReader.GetString("PermPostOfficeBangla");
            oEmployeeForIDCard.PermThanaBangla = oReader.GetString("PermThanaBangla");
            oEmployeeForIDCard.PermDistrictBangla = oReader.GetString("PermDistrictBangla");
            oEmployeeForIDCard.NationalIDBangla = oReader.GetString("NationalIDBangla");
            oEmployeeForIDCard.PostCode = oReader.GetString("PostCode");
            oEmployeeForIDCard.District = oReader.GetString("District");
            oEmployeeForIDCard.Thana = oReader.GetString("Thana");
            oEmployeeForIDCard.PostOffice = oReader.GetString("PostOffice");
            oEmployeeForIDCard.Village = oReader.GetString("Village");
            oEmployeeForIDCard.ChildrenInfo = oReader.GetString("ChildrenInfo");
            oEmployeeForIDCard.NationalID = oReader.GetString("NationalID");
            oEmployeeForIDCard.BloodGroup = oReader.GetString("BloodGroup");
            oEmployeeForIDCard.Email = oReader.GetString("Email");
            oEmployeeForIDCard.ContactNo = oReader.GetString("ContactNo");
            oEmployeeForIDCard.PresentAddress = oReader.GetString("PresentAddress");
            oEmployeeForIDCard.ParmanentAddress = oReader.GetString("ParmanentAddress");
            oEmployeeForIDCard.Name = oReader.GetString("Name");
            oEmployeeForIDCard.HRResp = oReader.GetString("HRResp");


            oEmployeeForIDCard.JoiningDate = oReader.GetDateTime("JoiningDate");
            oEmployeeForIDCard.ConfirmationDate = oReader.GetDateTime("ConfirmationDate");
            oEmployeeForIDCard.DateOfBirth = oReader.GetDateTime("DateOfBirth");
            oEmployeeForIDCard.Photo = oReader.GetBytes("Photo");
            oEmployeeForIDCard.Signature = oReader.GetBytes("Signature");

            oEmployeeForIDCard.DistrictBangla = oReader.GetString("DistrictBangla");
            oEmployeeForIDCard.ThanaBangla = oReader.GetString("ThanaBangla");
            oEmployeeForIDCard.PostOfficeBangla = oReader.GetString("PostOfficeBangla");
            oEmployeeForIDCard.VillageBangla = oReader.GetString("VillageBangla");
            oEmployeeForIDCard.CodeBangla = oReader.GetString("CodeBangla");
            oEmployeeForIDCard.BloodGroupBangla = oReader.GetString("BloodGroupBangla");
            return oEmployeeForIDCard;

        }

        private EmployeeForIDCard CreateObject(NullHandler oReader)
        {
            EmployeeForIDCard oEmployeeForIDCard = MapObject(oReader);
            return oEmployeeForIDCard;
        }

        private List<EmployeeForIDCard> CreateObjects(IDataReader oReader)
        {
            List<EmployeeForIDCard> oEmployeeForIDCard = new List<EmployeeForIDCard>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeForIDCard oItem = CreateObject(oHandler);
                oEmployeeForIDCard.Add(oItem);
            }
            return oEmployeeForIDCard;
        }


        #endregion

        #region Interface implementation
        public EmployeeForIDCardService() { }


        public EmployeeForIDCard Get(string sSQL, Int64 nUserId)
        {
            EmployeeForIDCard oEmployeeForIDCard  = new EmployeeForIDCard();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeForIDCardDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeForIDCard = CreateObject(oReader);
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

            return oEmployeeForIDCard;
        }

        public List<EmployeeForIDCard> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeForIDCard> oEmployeeForIDCard = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ApprovalHeadDA.Gets(sSQL, tc);
                oEmployeeForIDCard = CreateObjects(reader);
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
            return oEmployeeForIDCard;
        }
        #endregion
    }
}


