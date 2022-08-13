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
    public class EmployeeAuthenticationService : MarshalByRefObject, IEmployeeAuthenticationService
    {
        #region Private functions and declaration
        private EmployeeAuthentication MapObject(NullHandler oReader)
        {
            EmployeeAuthentication oEmployeeAuthentication = new EmployeeAuthentication();
            oEmployeeAuthentication.EmployeeAuthenticationID = oReader.GetInt32("EmployeeAuthenticationID");
            oEmployeeAuthentication.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeAuthentication.FingerPrint = oReader.GetBytes("FingerPrint");
            oEmployeeAuthentication.CardNo = oReader.GetString("CardNo");
            oEmployeeAuthentication.Password = oReader.GetString("Password");
            oEmployeeAuthentication.IsActive = oReader.GetBoolean("IsActive");
            return oEmployeeAuthentication;
            
        }

        private EmployeeAuthentication CreateObject(NullHandler oReader)
        {
            EmployeeAuthentication oEmployeeAuthentication = MapObject(oReader);
            return oEmployeeAuthentication;
        }

        private List<EmployeeAuthentication> CreateObjects(IDataReader oReader)
        {
            List<EmployeeAuthentication> oEmployeeAuthentication = new List<EmployeeAuthentication>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeAuthentication oItem = CreateObject(oHandler);
                oEmployeeAuthentication.Add(oItem);
            }
            return oEmployeeAuthentication;
        }

        #endregion

        #region Interface implementation
        public EmployeeAuthenticationService() { }

        public EmployeeAuthentication IUD(EmployeeAuthentication oEmployeeAuthentication, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeAuthenticationDA.IUD(tc, oEmployeeAuthentication, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeAuthentication = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeAuthentication.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oEmployeeAuthentication;
        }


        public EmployeeAuthentication Get(int nEmployeeAuthenticationID, Int64 nUserId)
        {
            EmployeeAuthentication oEmployeeAuthentication = new EmployeeAuthentication();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeAuthenticationDA.Get(tc, nEmployeeAuthenticationID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeAuthentication = CreateObject(oReader);
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
                oEmployeeAuthentication.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeAuthentication;
        }
        public List<EmployeeAuthentication> Gets(int nEmployeeID, Int64 nUserID)
        {
            List<EmployeeAuthentication> oEmployeeAuthentication = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeAuthenticationDA.Gets(tc,nEmployeeID);
                oEmployeeAuthentication = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeAuthentication", e);
                #endregion
            }
            return oEmployeeAuthentication;
        }

        public List<EmployeeAuthentication> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeAuthentication> oEmployeeAuthentication = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeAuthenticationDA.Gets(sSQL, tc);
                oEmployeeAuthentication = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeAuthentication", e);
                #endregion
            }
            return oEmployeeAuthentication;
        }


        #endregion
    }
}
