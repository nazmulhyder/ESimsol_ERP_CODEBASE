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
    public class EmployeeOfficialService : MarshalByRefObject, IEmployeeOfficialService
    {
        #region Private functions and declaration
        private EmployeeOfficial MapObject(NullHandler oReader)
        {
            EmployeeOfficial oEO = new EmployeeOfficial();
            oEO.EmployeeOfficialID = oReader.GetInt32("EmployeeOfficialID");
            oEO.EmployeeID = oReader.GetInt32("EmployeeID");
            oEO.AttendanceSchemeID = oReader.GetInt32("AttendanceSchemeID");
            oEO.DRPID = oReader.GetInt32("DRPID");
            oEO.DesignationID = oReader.GetInt32("DesignationID");
            oEO.CurrentShiftID = oReader.GetInt32("CurrentShiftID");
            oEO.WorkingStatus = (EnumEmployeeWorkigStatus)oReader.GetInt16("WorkingStatus");
            oEO.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oEO.DateOfConfirmation = oReader.GetDateTime("DateOfConfirmation");
            oEO.IsActive = oReader.GetBoolean("IsActive");
            oEO.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            

            //Derive
            oEO.DesignationName = oReader.GetString("DesignationName");
            oEO.CurrentShiftName = oReader.GetString("CuurentShift");
            oEO.AttendanceSchemeName = oReader.GetString("AttendanceSchemeName");
            oEO.LocationID = oReader.GetInt32("LocationID");
            oEO.LocationName = oReader.GetString("LocationName");
            oEO.DepartmentName = oReader.GetString("DepartmentName");
            oEO.DepartmentID = oReader.GetInt32("DepartmentID");
            oEO.RosterPlanID = oReader.GetInt32("RosterPlanID");
            oEO.RosterPlanDescription = oReader.GetString("RosterPlanDescription");
            oEO.EmployeeName = oReader.GetString("Name");
            oEO.DRPName = oReader.GetString("DRPName");
            oEO.Code = oReader.GetString("Code");
            oEO.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            try { oEO.RowNum = oReader.GetInt32("RowNum"); } catch (Exception ex) { }
            return oEO;
        }

        private EmployeeOfficial CreateObject(NullHandler oReader)
        {
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            oEmployeeOfficial = MapObject(oReader);
            return oEmployeeOfficial;
        }

        private List<EmployeeOfficial> CreateObjects(IDataReader oReader)
        {
            List<EmployeeOfficial> oEmployeeOfficial = new List<EmployeeOfficial>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeOfficial oItem = CreateObject(oHandler);
                oEmployeeOfficial.Add(oItem);
            }
            return oEmployeeOfficial;
        }

        #endregion

        #region Interface implementation
        public EmployeeOfficialService() { }

        public EmployeeOfficial IUD(EmployeeOfficial oEO, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<EmployeeOfficial> oEmployeeOfficials= new List<EmployeeOfficial>();
            oEmployeeOfficials = oEO.EmployeeOfficials;
            EmployeeConfirmation oEmployeeConfirmation = new EmployeeConfirmation();
            if (oEmployeeOfficials.Count > 0) { oEmployeeConfirmation = oEmployeeOfficials[0].EmployeeConfirmation; };
            try
            {
                string sSql = "SELECT ISNULL( COUNT(*),0) as TotalCount FROM View_EmployeeConfirmation WHERE EmployeeID=" + oEmployeeConfirmation.EmployeeID;
                
                tc = TransactionContext.Begin(true);
                
                bool IsConfirmation = EmployeeOfficialDA.GetEmpConf(sSql, tc);
                
                tc.End();

                tc = TransactionContext.Begin(true);
                for (int i = 0; i < oEmployeeOfficials.Count; i++)
                {
                    oEmployeeOfficials[i].WorkingStatus = (int)(EnumEmployeeWorkigStatus).1;
                    IDataReader reader;
                    reader = EmployeeOfficialDA.IUD(tc, oEmployeeOfficials[i], nDBOperation, nUserID);

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {

                        oEmployeeOfficials[i] = new EmployeeOfficial();
                        oEmployeeOfficials[i] = CreateObject(oReader);

                    }
                    reader.Close();

                    if (!IsConfirmation)
                    {

                        oEmployeeConfirmation.EmployeeCategory = (EnumEmployeeCategory)oEmployeeConfirmation.EmployeeCategoryInt;
                        reader = EmployeeConfirmationDA.IUD(tc, oEmployeeConfirmation, nUserID,1);
                        reader.Close();

                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oEO.ErrorMessage = Message;
                oEO.EmployeeOfficials = null;
                //throw new ServiceException("Failed to Save EmployeeOfficial. Because of " + e.Message, e);

                #endregion
            }
            return oEO;
        }
        public EmployeeOfficial GetByEmployee(int nEmployeeID, long nUserID) //EmployeeID
        {
            EmployeeOfficial oEO = new EmployeeOfficial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeOfficialDA.GetByEmployee(tc, nEmployeeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEO = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeOfficial", e);
                #endregion
            }

            return oEO;
        }
        public EmployeeOfficial Get(int id, Int64 nUserId) //EmployeeOfficialID
        {
            EmployeeOfficial oEO = new EmployeeOfficial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeOfficialDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEO = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeOfficial", e);
                #endregion
            }

            return oEO;
        }

        public List<EmployeeOfficial> Gets(int nEmployeeID, Int64 nUserID)
        {
            List<EmployeeOfficial> oEmployeeOfficial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeOfficialDA.Gets(tc, nEmployeeID);
                oEmployeeOfficial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeOfficial", e);
                #endregion
            }

            return oEmployeeOfficial;
        }
        public List<EmployeeOfficial> Gets(string sSql, Int64 nUserID)
        {
            List<EmployeeOfficial> oEmployeeOfficial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeOfficialDA.Gets(tc, sSql);
                oEmployeeOfficial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeOfficial", e);
                #endregion
            }

            return oEmployeeOfficial;
        }
        public int TotalRecordCount(string sSql, long nUserID)
        {
            int nTotalRecordCount =0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                 IDataReader reader = EmployeeOfficialDA.Gets(tc, sSql);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    nTotalRecordCount = oReader.GetInt32("TotalRecordCount");
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
                throw new ServiceException("Failed to Get EmployeeOfficial", e);
                #endregion
            }

            return nTotalRecordCount;
        }
        public EmployeeOfficial Get(string sSql, Int64 nUserID)
        {
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeOfficialDA.Get(tc, sSql);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeOfficial = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeOfficial", e);
                oEmployeeOfficial.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeOfficial;
        }

        #endregion
    }
}