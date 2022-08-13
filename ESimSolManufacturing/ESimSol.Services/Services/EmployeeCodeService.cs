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
    public class EmployeeCodeService : MarshalByRefObject, IEmployeeCodeService
    {
        #region Private functions and declaration
        private EmployeeCode MapObject(NullHandler oReader)
        {
            EmployeeCode oEmployeeCode = new EmployeeCode();
            oEmployeeCode.EmployeeCodeID = oReader.GetInt32("EmployeeCodeID");
            oEmployeeCode.DRPID = oReader.GetInt32("DRPID");
            oEmployeeCode.DesignationID = oReader.GetInt32("DesignationID");
            oEmployeeCode.CompanyID = oReader.GetInt32("CompanyID");

            oEmployeeCode.EmpCode = oReader.GetString("EmployeeCode");
            oEmployeeCode.LocationID = oReader.GetInt32("LocationID");
            oEmployeeCode.LocationName = oReader.GetString("LocationName");
            oEmployeeCode.DepartmentID = oReader.GetInt32("DepartmentID");
            oEmployeeCode.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeCode.DesignationID = oReader.GetInt32("DesignationID");
            oEmployeeCode.DesignationName = oReader.GetString("DesignationName");
            oEmployeeCode.CompanyName = oReader.GetString("CompanyName");
            oEmployeeCode.EncryptEmployeeCodeID = Global.Encrypt(oEmployeeCode.EmployeeCodeID.ToString());
            
            return oEmployeeCode;

        }

        private EmployeeCode CreateObject(NullHandler oReader)
        {
            EmployeeCode oEmployeeCode = MapObject(oReader);
            return oEmployeeCode;
        }

        private List<EmployeeCode> CreateObjects(IDataReader oReader)
        {
            List<EmployeeCode> oEmployeeCode = new List<EmployeeCode>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeCode oItem = CreateObject(oHandler);
                oEmployeeCode.Add(oItem);
            }
            return oEmployeeCode;
        }

        #endregion

        #region Interface implementation
        public EmployeeCodeService() { }

        public EmployeeCode IUD(EmployeeCode oEmployeeCode,int nDBOperation, Int64 nUserID)
        {

            EmployeeCode oEmpCode = new EmployeeCode();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = EmployeeCodeDA.IUD(tc, oEmployeeCode, nUserID, nDBOperation);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmpCode = CreateObject(oReader);
                }
                reader.Close();
              
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmpCode.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmpCode.EmployeeCodeID = 0;
                #endregion
            }
            return oEmpCode;
        }

        public EmployeeCode Get(int nEmployeeCodeID, Int64 nUserId)
        {
            EmployeeCode oEmployeeCode = new EmployeeCode();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeCodeDA.Get(nEmployeeCodeID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCode = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeCode", e);
                oEmployeeCode.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeCode;
        }
        public List<EmployeeCode> Gets(Int64 nUserID)
        {
            List<EmployeeCode> oEmployeeCode = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeCodeDA.Gets(tc);
                oEmployeeCode = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeCode", e);
                #endregion
            }
            return oEmployeeCode;
        }

        public List<EmployeeCode> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeCode> oEmployeeCode = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeCodeDA.Gets(sSQL, tc);
                oEmployeeCode = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeCode", e);
                #endregion
            }
            return oEmployeeCode;
        }


        #endregion
     
    }
}
