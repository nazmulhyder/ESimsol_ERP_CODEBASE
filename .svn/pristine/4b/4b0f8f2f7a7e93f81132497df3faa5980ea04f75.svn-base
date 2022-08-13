using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class CustomerPersonalInfoService : MarshalByRefObject, ICustomerPersonalInfoService
    {
        #region Private Function
        private CustomerPersonalInfo MapObject(NullHandler oReader)
        {
            CustomerPersonalInfo oCustomerPersonalInfo = new CustomerPersonalInfo();
            oCustomerPersonalInfo.CustomerPersonalInfoID = oReader.GetInt32("CustomerPersonalInfoID");
            oCustomerPersonalInfo.CustomerID = oReader.GetInt32("CustomerID");
            oCustomerPersonalInfo.CustomerName = oReader.GetString("CustomerName");
            oCustomerPersonalInfo.EmployeerName = oReader.GetString("EmployeerName");
            oCustomerPersonalInfo.Designation = oReader.GetString("Designation");
            oCustomerPersonalInfo.Address = oReader.GetString("Address");
            oCustomerPersonalInfo.ContactNumber = oReader.GetString("ContactNumber");
            oCustomerPersonalInfo.EmailAddress = oReader.GetString("EmailAddress");
            oCustomerPersonalInfo.DateOfBirth = oReader.GetDateTime("DateOfBirth");
            oCustomerPersonalInfo.MarriedStatus = (EnumMarriedStatus)oReader.GetInt32("MarriedStatus");
            oCustomerPersonalInfo.SpouseName = oReader.GetString("SpouseName");
            oCustomerPersonalInfo.SpouseDateOfBirth = oReader.GetDateTime("SpouseDateOfBirth");
            oCustomerPersonalInfo.AnniversaryDate = oReader.GetDateTime("AnniversaryDate");
            oCustomerPersonalInfo.Remarks = oReader.GetString("Remarks");
            oCustomerPersonalInfo.ContractorName = oReader.GetString("ContractorName");

            return oCustomerPersonalInfo;
        }
        private CustomerPersonalInfo CreateObject(NullHandler oReader){
            CustomerPersonalInfo oCustomerPersonalInfo=new CustomerPersonalInfo();
            oCustomerPersonalInfo=MapObject(oReader);
            return oCustomerPersonalInfo;
        }
        private List<CustomerPersonalInfo> CreateObjects(IDataReader oReader)
        {
            List<CustomerPersonalInfo> oCustomerPersonalInfos = new List<CustomerPersonalInfo>();
            NullHandler ohandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CustomerPersonalInfo oItem = CreateObject(ohandler);
                    oCustomerPersonalInfos.Add(oItem); 
            }
            return oCustomerPersonalInfos;
        }
        #endregion
        #region Interface Implementation
        public CustomerPersonalInfoService() { }

        public CustomerPersonalInfo Save(CustomerPersonalInfo oCustomerPersonalInfo, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCustomerPersonalInfo.CustomerPersonalInfoID <= 0)
                {
                    
                    reader = CustomerPersonalInfoDA.InsertUpdate(tc, oCustomerPersonalInfo, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    
                    reader = CustomerPersonalInfoDA.InsertUpdate(tc, oCustomerPersonalInfo, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCustomerPersonalInfo = new CustomerPersonalInfo();
                    oCustomerPersonalInfo = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oCustomerPersonalInfo = new CustomerPersonalInfo();
                oCustomerPersonalInfo.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oCustomerPersonalInfo;
        }

        public string Delete(int id, long nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CustomerPersonalInfo oCustomerPersonalInfo = new CustomerPersonalInfo();
                oCustomerPersonalInfo.CustomerPersonalInfoID = id;
                CustomerPersonalInfoDA.Delete(tc, oCustomerPersonalInfo, EnumDBOperation.Delete, nUserId);
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

        public CustomerPersonalInfo Get(int id, long nUserId)
        {
            CustomerPersonalInfo oCustomerPersonalInfo = new CustomerPersonalInfo();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = CustomerPersonalInfoDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCustomerPersonalInfo = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CustomerPersonalInfo", e);
                #endregion
            }
            return oCustomerPersonalInfo;
        }
        public List<CustomerPersonalInfo> Gets(long nUserID)
        {
            List<CustomerPersonalInfo> oCustomerPersonalInfos = new List<CustomerPersonalInfo>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CustomerPersonalInfoDA.Gets(tc);
                oCustomerPersonalInfos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CustomerPersonalInfo", e);
                #endregion
            }
            return oCustomerPersonalInfos;
        }

        public List<CustomerPersonalInfo> Gets(string sSQL, long nUserID)
        {
            List<CustomerPersonalInfo> oCustomerPersonalInfos = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CustomerPersonalInfoDA.Gets(tc, sSQL);
                oCustomerPersonalInfos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CustomerPersonalInfo", e);
                #endregion
            }
            return oCustomerPersonalInfos;
        }
        #endregion
    }
}
