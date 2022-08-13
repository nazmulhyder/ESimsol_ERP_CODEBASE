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
    public class DepartmentRequirementDesignationService : MarshalByRefObject, IDepartmentRequirementDesignationService
    {
        #region Private functions and declaration
        private DepartmentRequirementDesignation MapObject(NullHandler oReader)
        {
            DepartmentRequirementDesignation oDepartmentRequirementDesignation = new DepartmentRequirementDesignation();
            oDepartmentRequirementDesignation.DepartmentRequirementDesignationID = oReader.GetInt32("DepartmentRequirementDesignationID");
            oDepartmentRequirementDesignation.DepartmentRequirementPolicyID = oReader.GetInt32("DepartmentRequirementPolicyID");
            oDepartmentRequirementDesignation.DesignationID = oReader.GetInt32("DesignationID");
            oDepartmentRequirementDesignation.HRResponsibilityID = oReader.GetInt32("HRResponsibilityID");
            oDepartmentRequirementDesignation.RequiredPerson = oReader.GetInt32("RequiredPerson");
            oDepartmentRequirementDesignation.DesignationName = oReader.GetString("DesignationName");
            oDepartmentRequirementDesignation.Responsibility = oReader.GetString("Responsibility");
            oDepartmentRequirementDesignation.ResponsibilityInBangla = oReader.GetString("ResponsibilityInBangla");
            oDepartmentRequirementDesignation.ShiftName = oReader.GetString("Shift");
            oDepartmentRequirementDesignation.StartTime = oReader.GetString("StartTime");
            oDepartmentRequirementDesignation.EndTime = oReader.GetString("EndTime");
            oDepartmentRequirementDesignation.NameOfShift = oReader.GetString("NameOfShift");
            //oDepartmentRequirementDesignation.Designation = oReader.GetString("Designation");
            //oDepartmentRequirementDesignation.ShiftID = oReader.GetInt32("ShiftID");
            //oDepartmentRequirementDesignation.DesignationSequence = oReader.GetInt32("DesignationSequence");
            //oDepartmentRequirementDesignation.ShiftSequence = oReader.GetInt32("ShiftSequence");
            return oDepartmentRequirementDesignation;
        }

        private DepartmentRequirementDesignation CreateObject(NullHandler oReader)
        {
            DepartmentRequirementDesignation oDepartmentRequirementDesignation = MapObject(oReader);
            return oDepartmentRequirementDesignation;
        }

        private List<DepartmentRequirementDesignation> CreateObjects(IDataReader oReader)
        {
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignation = new List<DepartmentRequirementDesignation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DepartmentRequirementDesignation oItem = CreateObject(oHandler);
                oDepartmentRequirementDesignation.Add(oItem);
            }
            return oDepartmentRequirementDesignation;
        }

        #endregion

        #region Interface implementation
        public DepartmentRequirementDesignationService() { }

        public List<DepartmentRequirementDesignation> Gets(Int64 nUserID)
        {
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentRequirementDesignationDA.Gets(tc);
                oDepartmentRequirementDesignation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DepartmentRequirementDesignation", e);
                #endregion
            }

            return oDepartmentRequirementDesignation;
        }
        public List<DepartmentRequirementDesignation> Gets(int nDepartmentRequirementPolicyID, bool bIsShiftOrder, Int64 nUserID)
        {
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignation = new List<DepartmentRequirementDesignation>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentRequirementDesignationDA.Gets(tc, nDepartmentRequirementPolicyID, bIsShiftOrder);
                oDepartmentRequirementDesignation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DepartmentRequirementDesignation", e);
                #endregion
            }

            return oDepartmentRequirementDesignation;
        }

        public List<DepartmentRequirementDesignation> GetsPolicy(int nDepartmentRequirementPolicyID, Int64 nUserID)
        {
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignation = new List<DepartmentRequirementDesignation>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentRequirementDesignationDA.GetsPolicy(tc, nDepartmentRequirementPolicyID);
                oDepartmentRequirementDesignation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DepartmentRequirementDesignation", e);
                #endregion
            }

            return oDepartmentRequirementDesignation;
        }
        public List<DepartmentRequirementDesignation> Gets(string sSQL, Int64 nUserID)
        {
            List<DepartmentRequirementDesignation> oDepartmentRequirementDesignation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentRequirementDesignationDA.Gets(tc, sSQL);
                oDepartmentRequirementDesignation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DepartmentRequirementDesignation", e);
                #endregion
            }

            return oDepartmentRequirementDesignation;
        }
        public DepartmentRequirementDesignation IUD(DepartmentRequirementDesignation oDepartmentRequirementDesignation, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            DepartmentRequirementDesignation oNewCCCD = new DepartmentRequirementDesignation();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DepartmentRequirementDesignationDA.InsertUpdate(tc, oDepartmentRequirementDesignation, nUserID, EnumDBOperation.Insert, "");

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNewCCCD = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNewCCCD.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oNewCCCD;
        }

        public string SaveDRPDesignations(List<DepartmentRequirementDesignation> oDepartmentRequirementDesignations, Int64 nUserID)
        {
            TransactionContext tc = null;
            DepartmentRequirementDesignation oNewCCCD = new DepartmentRequirementDesignation();
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (DepartmentRequirementDesignation oItem in oDepartmentRequirementDesignations)
                {
                    IDataReader reader;
                    reader = DepartmentRequirementDesignationDA.InsertUpdate(tc, oItem, nUserID, EnumDBOperation.Insert, "");
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNewCCCD.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oNewCCCD.ErrorMessage;
        }

        #endregion
    }
}
