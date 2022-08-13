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
    public class DepartmentCloseDayService : MarshalByRefObject, IDepartmentCloseDayService
    {
        #region Private functions and declaration
        private DepartmentCloseDay MapObject(NullHandler oReader)
        {
            DepartmentCloseDay oDepartmentCloseDay = new DepartmentCloseDay();
            oDepartmentCloseDay.DepartmentCloseDayID = oReader.GetInt32("DepartmentCloseDayID");
            oDepartmentCloseDay.DepartmentRequirementPolicyID = oReader.GetInt32("DepartmentRequirementPolicyID");
            oDepartmentCloseDay.WeekDay = oReader.GetString("WeekDay");
   
            return oDepartmentCloseDay;
        }

        private DepartmentCloseDay CreateObject(NullHandler oReader)
        {
            DepartmentCloseDay oDepartmentCloseDay = MapObject(oReader);
            return oDepartmentCloseDay;
        }

        private List<DepartmentCloseDay> CreateObjects(IDataReader oReader)
        {
            List<DepartmentCloseDay> oDepartmentCloseDay = new List<DepartmentCloseDay>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DepartmentCloseDay oItem = CreateObject(oHandler);
                oDepartmentCloseDay.Add(oItem);
            }
            return oDepartmentCloseDay;
        }

        #endregion

        #region Interface implementation
        public DepartmentCloseDayService() { }

        public List<DepartmentCloseDay> Gets(Int64 nUserID)
        {
            List<DepartmentCloseDay> oDepartmentCloseDay = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentCloseDayDA.Gets(tc);
                oDepartmentCloseDay = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DepartmentCloseDay", e);
                #endregion
            }

            return oDepartmentCloseDay;
        }

        public List<DepartmentCloseDay> Gets(int nDepartmentRequirementPolicyID, Int64 nUserID)
        {
            List<DepartmentCloseDay> oDepartmentCloseDay = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentCloseDayDA.Gets(tc, nDepartmentRequirementPolicyID);
                oDepartmentCloseDay = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DepartmentCloseDay", e);
                #endregion
            }

            return oDepartmentCloseDay;
        }
        public List<DepartmentCloseDay> Gets(string sSQL, Int64 nUserID)
        {
            List<DepartmentCloseDay> oDepartmentCloseDay = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentCloseDayDA.Gets(tc, sSQL);
                oDepartmentCloseDay = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DepartmentCloseDay", e);
                #endregion
            }

            return oDepartmentCloseDay;
        }
        public DepartmentCloseDay IUD(DepartmentCloseDay oDepartmentCloseDay, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            DepartmentCloseDay oNewCCCD = new DepartmentCloseDay();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DepartmentCloseDayDA.InsertUpdate(tc, oDepartmentCloseDay, EnumDBOperation.Insert, "");

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

        #endregion
    }
}
