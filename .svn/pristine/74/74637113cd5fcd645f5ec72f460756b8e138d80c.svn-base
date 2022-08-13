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

    public class DepartmentRequirementPolicyService : MarshalByRefObject, IDepartmentRequirementPolicyService
    {
        #region Private functions and declaration
        private DepartmentRequirementPolicy MapObject(NullHandler oReader)
        {
            DepartmentRequirementPolicy oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            oDepartmentRequirementPolicy.DepartmentRequirementPolicyID = oReader.GetInt32("DepartmentRequirementPolicyID");            
            oDepartmentRequirementPolicy.CompanyID = oReader.GetInt32("CompanyID");
            oDepartmentRequirementPolicy.DepartmentID = oReader.GetInt32("DepartmentID");
            oDepartmentRequirementPolicy.LocationID = oReader.GetInt32("LocationID");
            oDepartmentRequirementPolicy.Description = oReader.GetString("Description");
            oDepartmentRequirementPolicy.HeadCount = oReader.GetInt32("HeadCount");
            oDepartmentRequirementPolicy.Budget = oReader.GetDouble("Budget");
            oDepartmentRequirementPolicy.CompanyName = oReader.GetString("Company");
            oDepartmentRequirementPolicy.BUName = oReader.GetString("BUName");
            oDepartmentRequirementPolicy.LocationName = oReader.GetString("Location");
            oDepartmentRequirementPolicy.DepartmentName = oReader.GetString("Department");
            oDepartmentRequirementPolicy.EmployeeCount = oReader.GetInt32("EmployeeCount");
            oDepartmentRequirementPolicy.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            return oDepartmentRequirementPolicy;
        }

        private DepartmentRequirementPolicy CreateObject(NullHandler oReader)
        {
            DepartmentRequirementPolicy oDepartmentRequirementPolicy = MapObject(oReader);
            return oDepartmentRequirementPolicy;
        }

        private List<DepartmentRequirementPolicy> CreateObjects(IDataReader oReader)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicy = new List<DepartmentRequirementPolicy>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DepartmentRequirementPolicy oItem = CreateObject(oHandler);
                oDepartmentRequirementPolicy.Add(oItem);
            }
            return oDepartmentRequirementPolicy;
        }

        #endregion

        #region Interface implementation
        public DepartmentRequirementPolicyService() { }

        public DepartmentRequirementPolicy Save(DepartmentRequirementPolicy oDepartmentRequirementPolicy, Int64 nUserID)
        {
            TransactionContext tc = null;
            oDepartmentRequirementPolicy.ErrorMessage = "";


            try
            {
                tc = TransactionContext.Begin(true);

                string sDepartmentCloseDayIDs = "";
                DepartmentCloseDay oDepartmentCloseDay = new DepartmentCloseDay();
                List<DepartmentCloseDay> oDepartmentCloseDays = new List<DepartmentCloseDay>();
                oDepartmentCloseDays = oDepartmentRequirementPolicy.DepartmentCloseDays;

                string sDepartmentRequirementDesignationIDs = "";
                DepartmentRequirementDesignation oDepartmentRequirementDesignation = new DepartmentRequirementDesignation();
                List<DepartmentRequirementDesignation> oDepartmentRequirementDesignations = new List<DepartmentRequirementDesignation>();               
                oDepartmentRequirementDesignations = oDepartmentRequirementPolicy.DepartmentRequirementDesignations;

                #region Department Requirement Policy Part
                IDataReader reader;
                if (oDepartmentRequirementPolicy.DepartmentRequirementPolicyID <= 0)
                {
                    reader = DepartmentRequirementPolicyDA.InsertUpdate(tc, oDepartmentRequirementPolicy, nUserID, EnumDBOperation.Insert);
                }
                else
                {
                    reader = DepartmentRequirementPolicyDA.InsertUpdate(tc, oDepartmentRequirementPolicy, nUserID, EnumDBOperation.Update);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
                    oDepartmentRequirementPolicy = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Department Close Day Part

                foreach (DepartmentCloseDay oItem in oDepartmentCloseDays)
                {
                    IDataReader readerdetail;
                    oItem.DepartmentRequirementPolicyID = oDepartmentRequirementPolicy.DepartmentRequirementPolicyID;
                  
                    if (oItem.DepartmentCloseDayID <= 0)
                    {
                        readerdetail = DepartmentCloseDayDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "");
                    }
                    else
                    {
                        readerdetail = DepartmentCloseDayDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sDepartmentCloseDayIDs = sDepartmentCloseDayIDs + oReaderDetail.GetString("DepartmentCloseDayID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sDepartmentCloseDayIDs.Length > 0)
                {
                    sDepartmentCloseDayIDs = sDepartmentCloseDayIDs.Remove(sDepartmentCloseDayIDs.Length - 1, 1);
                }
                oDepartmentCloseDay = new DepartmentCloseDay();
                oDepartmentCloseDay.DepartmentRequirementPolicyID = oDepartmentRequirementPolicy.DepartmentRequirementPolicyID;
                DepartmentCloseDayDA.Delete(tc, oDepartmentCloseDay, EnumDBOperation.Delete, sDepartmentCloseDayIDs);

                #endregion

                #region Department Requirement Designation Part

                foreach (DepartmentRequirementDesignation oItem in oDepartmentRequirementDesignations)
                {
                    IDataReader readerdetail;
                    oItem.DepartmentRequirementPolicyID = oDepartmentRequirementPolicy.DepartmentRequirementPolicyID;
                    if (oItem.DepartmentRequirementDesignationID <= 0)
                    {
                        readerdetail = DepartmentRequirementDesignationDA.InsertUpdate(tc, oItem, nUserID, EnumDBOperation.Insert, "");
                    }
                    else
                    {
                        readerdetail = DepartmentRequirementDesignationDA.InsertUpdate(tc, oItem, nUserID, EnumDBOperation.Update, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sDepartmentRequirementDesignationIDs = sDepartmentRequirementDesignationIDs + oReaderDetail.GetString("DepartmentRequirementDesignationID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sDepartmentRequirementDesignationIDs.Length > 0)
                {
                    sDepartmentRequirementDesignationIDs = sDepartmentRequirementDesignationIDs.Remove(sDepartmentRequirementDesignationIDs.Length - 1, 1);
                }
                oDepartmentRequirementDesignation = new DepartmentRequirementDesignation();
                oDepartmentRequirementDesignation.DepartmentRequirementPolicyID = oDepartmentRequirementPolicy.DepartmentRequirementPolicyID;
                DepartmentRequirementDesignationDA.Delete(tc, oDepartmentRequirementDesignation, nUserID, EnumDBOperation.Delete, sDepartmentRequirementDesignationIDs);

                #endregion

                #region Get Department Requirement Policy

                reader = DepartmentRequirementPolicyDA.Get(tc, oDepartmentRequirementPolicy.DepartmentRequirementPolicyID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
                    oDepartmentRequirementPolicy = CreateObject(oReader);
                }
                reader.Close();

                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDepartmentRequirementPolicy.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oDepartmentRequirementPolicy.DepartmentRequirementPolicyID = 0;
                #endregion
            }
            return oDepartmentRequirementPolicy;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DepartmentRequirementPolicy oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
                oDepartmentRequirementPolicy.DepartmentRequirementPolicyID = id;
                DepartmentRequirementPolicyDA.Delete(tc, oDepartmentRequirementPolicy, nUserId, EnumDBOperation.Delete);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public DepartmentRequirementPolicy Get(int id, Int64 nUserId)
        {
            DepartmentRequirementPolicy aDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DepartmentRequirementPolicyDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aDepartmentRequirementPolicy = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DepartmentRequirementPolicy", e);
                #endregion
            }

            return aDepartmentRequirementPolicy;
        }

        public List<DepartmentRequirementPolicy> Gets(Int64 nUserID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicy = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentRequirementPolicyDA.Gets(tc);
                oDepartmentRequirementPolicy = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DepartmentRequirementPolicy", e);
                #endregion
            }
            return oDepartmentRequirementPolicy;
        }
        public List<DepartmentRequirementPolicy> Gets(string sSQL, Int64 nUserID)
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicy = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentRequirementPolicyDA.Gets(tc, sSQL);
                oDepartmentRequirementPolicy = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DepartmentRequirementPolicy", e);
                #endregion
            }
            return oDepartmentRequirementPolicy;
        }
        #endregion
    }

   
}
