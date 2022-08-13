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
    public class EmployeeSalaryDetailService : MarshalByRefObject, IEmployeeSalaryDetailService
    {
        #region Private functions and declaration
        private EmployeeSalaryDetail MapObject(NullHandler oReader)
        {
            EmployeeSalaryDetail oEmployeeSalaryDetail = new EmployeeSalaryDetail();
            oEmployeeSalaryDetail.ESDID = oReader.GetInt32("ESDSalarylID");
            oEmployeeSalaryDetail.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oEmployeeSalaryDetail.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeSalaryDetail.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oEmployeeSalaryDetail.Amount = oReader.GetDouble("Amount");
            oEmployeeSalaryDetail.CompAmount = oReader.GetDouble("CompAmount");
            //derive
            oEmployeeSalaryDetail.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oEmployeeSalaryDetail.SalaryHeadType = oReader.GetInt16("SalaryHeadType");

            return oEmployeeSalaryDetail;
        }

        private EmployeeSalaryDetail CreateObject(NullHandler oReader)
        {
            EmployeeSalaryDetail oEmployeeSalaryDetail = MapObject(oReader);
            return oEmployeeSalaryDetail;
        }

        private List<EmployeeSalaryDetail> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSalaryDetail> oEmployeeSalaryDetail = new List<EmployeeSalaryDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSalaryDetail oItem = CreateObject(oHandler);
                oEmployeeSalaryDetail.Add(oItem);
            }
            return oEmployeeSalaryDetail;
        }

        #endregion

        #region Interface implementation
        public EmployeeSalaryDetailService() { }

        public EmployeeSalaryDetail IUD(EmployeeSalaryDetail oEmployeeSalaryDetail, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSalaryDetailDA.IUD(tc, oEmployeeSalaryDetail, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeSalaryDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeSalaryDetail.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSalaryDetail.ESDID = 0;
                #endregion
            }
            return oEmployeeSalaryDetail;
        }


        public EmployeeSalaryDetail Get(int nESDID, Int64 nUserId)
        {
            EmployeeSalaryDetail oEmployeeSalaryDetail = new EmployeeSalaryDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryDetailDA.Get(nESDID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryDetail = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalaryDetail", e);
                oEmployeeSalaryDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryDetail;
        }

        public EmployeeSalaryDetail Get(string sSql, Int64 nUserId)
        {
            EmployeeSalaryDetail oEmployeeSalaryDetail = new EmployeeSalaryDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryDetailDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryDetail = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalaryDetail", e);
                oEmployeeSalaryDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryDetail;
        }

        public List<EmployeeSalaryDetail> Gets(Int64 nUserID)
        {
            List<EmployeeSalaryDetail> oEmployeeSalaryDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDetailDA.Gets(tc);
                oEmployeeSalaryDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeSalaryDetail", e);
                #endregion
            }
            return oEmployeeSalaryDetail;
        }

        public List<EmployeeSalaryDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSalaryDetail> oEmployeeSalaryDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDetailDA.Gets(sSQL, tc);
                oEmployeeSalaryDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeSalaryDetail", e);
                #endregion
            }
            return oEmployeeSalaryDetail;
        }

        public List<EmployeeSalaryDetail> GetsForPaySlip(string sEmployeeSalaryIDs, Int64 nUserID)
        {
            List<EmployeeSalaryDetail> oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryDetailDA.GetsForPaySlip(sEmployeeSalaryIDs, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    EmployeeSalaryDetail oItem = new EmployeeSalaryDetail();
                    oItem.ESDID = oreader.GetInt32("ESDSalarylID");
                    oItem.EmployeeSalaryID = oreader.GetInt32("EmployeeSalaryID");
                    oItem.Amount = oreader.GetDouble("Amount");
                    oItem.SalaryHeadID = oreader.GetInt32("SalaryHeadID");
                    oItem.SalaryHeadName = oreader.GetString("SalaryHeadName");
                    oItem.SalaryHeadType = oreader.GetInt16("SalaryHeadType");
                    oItem.Equation = oreader.GetString("Equation");
                 
                    oEmployeeSalaryDetails.Add(oItem);
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
                throw new ServiceException("Failed to Get View_EmployeeSalaryDetail", e);
                #endregion
            }
            return oEmployeeSalaryDetails;
        }

        #endregion

    }
}
