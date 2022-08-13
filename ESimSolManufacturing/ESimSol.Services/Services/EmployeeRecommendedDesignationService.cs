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
    public class EmployeeRecommendedDesignationService : MarshalByRefObject, IEmployeeRecommendedDesignationService
    {
        #region Private functions and declaration
        private EmployeeRecommendedDesignation MapObject(NullHandler oReader)
        {
            EmployeeRecommendedDesignation oEmployeeRecommendedDesignation = new EmployeeRecommendedDesignation();

            oEmployeeRecommendedDesignation.ARDID = oReader.GetInt32("ARDID");
            oEmployeeRecommendedDesignation.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeRecommendedDesignation.DepartmentID = oReader.GetInt32("DepartmentID");
            oEmployeeRecommendedDesignation.DesignationID = oReader.GetInt32("DesignationID");
            
            //derive
            oEmployeeRecommendedDesignation.EmployeeNameCode = oReader.GetString("EmployeeNameCode");
            oEmployeeRecommendedDesignation.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeRecommendedDesignation.DesignationName = oReader.GetString("DesignationName");

            return oEmployeeRecommendedDesignation;

        }

        private EmployeeRecommendedDesignation CreateObject(NullHandler oReader)
        {
            EmployeeRecommendedDesignation oEmployeeRecommendedDesignation = MapObject(oReader);
            return oEmployeeRecommendedDesignation;
        }

        private List<EmployeeRecommendedDesignation> CreateObjects(IDataReader oReader)
        {
            List<EmployeeRecommendedDesignation> oEmployeeRecommendedDesignations = new List<EmployeeRecommendedDesignation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeRecommendedDesignation oItem = CreateObject(oHandler);
                oEmployeeRecommendedDesignations.Add(oItem);
            }
            return oEmployeeRecommendedDesignations;
        }

        #endregion

        #region Interface implementation
        public EmployeeRecommendedDesignationService() { }

        public EmployeeRecommendedDesignation IUD(EmployeeRecommendedDesignation oEmployeeRecommendedDesignation, int nDBOperation, Int64 nUserID)
        {


            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeRecommendedDesignationDA.IUD(tc, oEmployeeRecommendedDesignation, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeRecommendedDesignation = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeRecommendedDesignation.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeRecommendedDesignation.ARDID = 0;
                #endregion
            }
            return oEmployeeRecommendedDesignation;
        }


        public EmployeeRecommendedDesignation Get(int nARDID, Int64 nUserId)
        {
            EmployeeRecommendedDesignation oEmployeeRecommendedDesignation = new EmployeeRecommendedDesignation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeRecommendedDesignationDA.Get(nARDID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeRecommendedDesignation = CreateObject(oReader);
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

                oEmployeeRecommendedDesignation.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeRecommendedDesignation;
        }

        public EmployeeRecommendedDesignation Get(string sSQL, Int64 nUserId)
        {
            EmployeeRecommendedDesignation oEmployeeRecommendedDesignation = new EmployeeRecommendedDesignation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeRecommendedDesignationDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeRecommendedDesignation = CreateObject(oReader);
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

                oEmployeeRecommendedDesignation.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeRecommendedDesignation;
        }

        public List<EmployeeRecommendedDesignation> Gets(int nID, Int64 nUserID)
        {
            List<EmployeeRecommendedDesignation> oEmployeeRecommendedDesignation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeRecommendedDesignationDA.Gets(nID,tc);
                oEmployeeRecommendedDesignation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeRecommendedDesignation", e);
                #endregion
            }
            return oEmployeeRecommendedDesignation;
        }

        public List<EmployeeRecommendedDesignation> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeRecommendedDesignation> oEmployeeRecommendedDesignation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeRecommendedDesignationDA.Gets(sSQL, tc);
                oEmployeeRecommendedDesignation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeRecommendedDesignation", e);
                #endregion
            }
            return oEmployeeRecommendedDesignation;
        }

        #endregion


    }
}
