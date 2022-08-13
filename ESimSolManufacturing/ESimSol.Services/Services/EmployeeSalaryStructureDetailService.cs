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
    public class EmployeeSalaryStructureDetailService : MarshalByRefObject, IEmployeeSalaryStructureDetailService
    {
        #region Private functions and declaration
        private EmployeeSalaryStructureDetail MapObject(NullHandler oReader)
        {
            EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail = new EmployeeSalaryStructureDetail();
            oEmployeeSalaryStructureDetail.ESSSID = oReader.GetInt32("ESSSID");
            oEmployeeSalaryStructureDetail.ESSID = oReader.GetInt32("ESSID");
            oEmployeeSalaryStructureDetail.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oEmployeeSalaryStructureDetail.Amount = oReader.GetDouble("Amount");
            oEmployeeSalaryStructureDetail.CompAmount = oReader.GetDouble("CompAmount");
            //start derive            
            oEmployeeSalaryStructureDetail.Condition = (EnumAllowanceCondition)oReader.GetInt16("Condition");
            oEmployeeSalaryStructureDetail.Period = (EnumPeriod)oReader.GetInt16("Period");
            oEmployeeSalaryStructureDetail.Times = oReader.GetInt32("Times");
            oEmployeeSalaryStructureDetail.DeferredDay = oReader.GetInt32("DeferredDay");
            oEmployeeSalaryStructureDetail.ActivationAfter = (EnumRecruitmentEvent)oReader.GetInt16("ActivationAfter");
            oEmployeeSalaryStructureDetail.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oEmployeeSalaryStructureDetail.SalaryHeadNameInBangla = oReader.GetString("SalaryHeadNameInBangla");
            oEmployeeSalaryStructureDetail.SalaryHeadType = (EnumSalaryHeadType)oReader.GetInt16("SalaryHeadType");

            oEmployeeSalaryStructureDetail.AddDate = oReader.GetDateTime("AddDate");
            oEmployeeSalaryStructureDetail.UserNameCode = oReader.GetString("UserNameCode");
            oEmployeeSalaryStructureDetail.SalaryType = (EnumSalaryType)oReader.GetInt16("SalaryType");
            oEmployeeSalaryStructureDetail.SalaryTypeInt = (int)(EnumSalaryType)oReader.GetInt16("SalaryType");
            //end derive
            return oEmployeeSalaryStructureDetail;
        }

        private EmployeeSalaryStructureDetail CreateObject(NullHandler oReader)
        {
            EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail = MapObject(oReader);
            return oEmployeeSalaryStructureDetail;
        }

        private List<EmployeeSalaryStructureDetail> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSalaryStructureDetail oItem = CreateObject(oHandler);
                oEmployeeSalaryStructureDetails.Add(oItem);
            }
            return oEmployeeSalaryStructureDetails;
        }

        #endregion

        #region Interface implementation
        public EmployeeSalaryStructureDetailService() { }

        public EmployeeSalaryStructureDetail IUD(EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSalaryStructureDetailDA.IUD(tc, oEmployeeSalaryStructureDetail, nUserID, nDBOperation, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeSalaryStructureDetail = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeSalaryStructureDetail.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oEmployeeSalaryStructureDetail;
        }

        public string DeleteSingleSalaryStructureDetail(EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail, Int64 nUserID)
        {
            string sFeedbackMessage = Global.DeleteMessage;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSalaryStructureDetailDA.DeleteSingleSalaryStructureDetail(tc, oEmployeeSalaryStructureDetail, nUserID, EnumDBOperation.SingleDelete, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryStructureDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                sFeedbackMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return sFeedbackMessage;
        }
        public EmployeeSalaryStructureDetail Get(int nESSID, Int64 nUserId)
        {
            EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail = new EmployeeSalaryStructureDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSalaryStructureDetailDA.Get(nESSID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryStructureDetail = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalaryStructureDetail", e);
                oEmployeeSalaryStructureDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryStructureDetail;
        }
        public List<EmployeeSalaryStructureDetail> Gets(Int64 nUserID)
        {
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetail = new List<EmployeeSalaryStructureDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryStructureDetailDA.Gets(tc);
                oEmployeeSalaryStructureDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSalaryStructureDetail", e);
                #endregion
            }
            return oEmployeeSalaryStructureDetail;
        }

        public List<EmployeeSalaryStructureDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSalaryStructureDetailDA.Gets(sSQL, tc);
                oEmployeeSalaryStructureDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSalaryStructureDetail", e);
                #endregion
            }

            return oEmployeeSalaryStructureDetail;
        }

        public List<EmployeeSalaryStructureDetail> GetHistorys(string sSQL, Int64 nUserID)
        {
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeSalaryStructureDA.Gets(sSQL, tc);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    EmployeeSalaryStructureDetail oItem = new EmployeeSalaryStructureDetail();
                    oItem.ESSSID = oreader.GetInt32("ESSSHistoryID");
                    oItem.ESSID = oreader.GetInt32("ESSHistoryID");
                    oItem.SalaryHeadID = oreader.GetInt32("SalaryHeadID");
                    oItem.Amount = oreader.GetDouble("Amount");

                    oItem.SalaryHeadName = oreader.GetString("SalaryHeadName");
                    oItem.SalaryHeadNameInBangla = oreader.GetString("SalaryHeadNameInBangla");
                    oItem.SalaryHeadType = (EnumSalaryHeadType)oreader.GetInt16("SalaryHeadType");
                    oEmployeeSalaryStructureDetails.Add(oItem);
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
                #endregion
            }
            return oEmployeeSalaryStructureDetails;
        }
        #endregion
    }
}
