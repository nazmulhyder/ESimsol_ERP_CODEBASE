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
    public class SalarySchemeDetailService : MarshalByRefObject, ISalarySchemeDetailService
    {
        #region Private functions and declaration
        private SalarySchemeDetail MapObject(NullHandler oReader)
        {
            SalarySchemeDetail oSalarySchemeDetail = new SalarySchemeDetail();
            oSalarySchemeDetail.SalarySchemeDetailID = oReader.GetInt32("SalarySchemeDetailID");
            oSalarySchemeDetail.SalarySchemeID = oReader.GetInt32("SalarySchemeID");
            oSalarySchemeDetail.SalaryHeadID = oReader.GetInt32("SalaryHeadID");            
            oSalarySchemeDetail.Condition = (EnumAllowanceCondition)oReader.GetInt16("Condition");
            oSalarySchemeDetail.ConditionInt = (int)(EnumAllowanceCondition)oReader.GetInt16("Condition");
            oSalarySchemeDetail.Period = (EnumPeriod)oReader.GetInt16("Period");
            oSalarySchemeDetail.PeriodInt = (int)(EnumPeriod)oReader.GetInt16("Period");
            oSalarySchemeDetail.Times = oReader.GetInt32("Times");
            oSalarySchemeDetail.DeferredDay = oReader.GetInt32("DeferredDay");
            oSalarySchemeDetail.ActivationAfter = (EnumRecruitmentEvent)oReader.GetInt16("ActivationAfter");
            oSalarySchemeDetail.ActivationAfterInt = (int)(EnumRecruitmentEvent)oReader.GetInt16("ActivationAfter");
           //derive
            oSalarySchemeDetail.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oSalarySchemeDetail.SalaryHeadType = (EnumSalaryHeadType)oReader.GetInt16("SalaryHeadType");
            oSalarySchemeDetail.SalaryHeadTypeInt = (int)(EnumSalaryHeadType)oReader.GetInt16("SalaryHeadType");
            oSalarySchemeDetail.SalaryType = (EnumSalaryType)oReader.GetInt16("SalaryType");
            oSalarySchemeDetail.SalaryTypeInt = (int)(EnumSalaryType)oReader.GetInt16("SalaryType");
            return oSalarySchemeDetail;
           
        }

        private SalarySchemeDetail CreateObject(NullHandler oReader)
        {
            SalarySchemeDetail oSalarySchemeDetail = MapObject(oReader);
            return oSalarySchemeDetail;
        }

        private List<SalarySchemeDetail> CreateObjects(IDataReader oReader)
        {
            List<SalarySchemeDetail> oSalarySchemeDetail = new List<SalarySchemeDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalarySchemeDetail oItem = CreateObject(oHandler);
                oSalarySchemeDetail.Add(oItem);
            }
            return oSalarySchemeDetail;
        }

        #endregion

        #region Interface implementation
        public SalarySchemeDetailService() { }

        public SalarySchemeDetail IUD(SalarySchemeDetail oSalarySchemeDetail, Int64 nUserID)
        {
            List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
            oSalarySchemeDetailCalculations = oSalarySchemeDetail.SalarySchemeDetailCalculations;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSalarySchemeDetail.SalarySchemeDetailID <= 0)
                {
                    reader = SalarySchemeDetailDA.IUD(tc, oSalarySchemeDetail, nUserID, (int)EnumDBOperation.Insert);
                }
                else
                {
                    reader = SalarySchemeDetailDA.IUD(tc, oSalarySchemeDetail, nUserID, (int)EnumDBOperation.Update);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oSalarySchemeDetail = CreateObject(oReader);
                }
                reader.Close();


                if (oSalarySchemeDetail.SalarySchemeDetailID <= 0)
                {
                    throw new ServiceException("Invalid SalarySchemeDetail!");
                }

                #region SalarySchemeDetailCalculation
                SalarySchemeDetailCalculationDA.Delete(tc, oSalarySchemeDetail.SalarySchemeDetailID);
                if (oSalarySchemeDetailCalculations != null)
                {
                    foreach (SalarySchemeDetailCalculation oSalarySchemeDetailCalculation in oSalarySchemeDetailCalculations)
                    {
                        IDataReader calculationreader;
                        oSalarySchemeDetailCalculation.SalarySchemeDetailID = oSalarySchemeDetail.SalarySchemeDetailID;
                        calculationreader = SalarySchemeDetailCalculationDA.IUD(tc, oSalarySchemeDetailCalculation, nUserID, (int)EnumDBOperation.Insert);
                        NullHandler oCalculationReader = new NullHandler(calculationreader);
                        calculationreader.Close();
                    }
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSalarySchemeDetail.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oSalarySchemeDetail;
        }


        public SalarySchemeDetail IUDGross(SalarySchemeDetail oSalarySchemeDetail, Int64 nUserID)
        {
            List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
            oSalarySchemeDetailCalculations = oSalarySchemeDetail.SalarySchemeDetailCalculations;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region SalarySchemeDetailCalculation
                SalarySchemeDetailCalculationDA.DeleteGross(tc, oSalarySchemeDetail.SalarySchemeID);
                if (oSalarySchemeDetailCalculations != null)
                {
                    foreach (SalarySchemeDetailCalculation oSalarySchemeDetailCalculation in oSalarySchemeDetailCalculations)
                    {
                        IDataReader calculationreader;
                        oSalarySchemeDetailCalculation.SalarySchemeID = oSalarySchemeDetail.SalarySchemeID;
                        calculationreader = SalarySchemeDetailCalculationDA.IUDGross(tc, oSalarySchemeDetailCalculation, nUserID, (int)EnumDBOperation.Insert);
                        NullHandler oCalculationReader = new NullHandler(calculationreader);
                        calculationreader.Close();
                    }
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSalarySchemeDetail.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oSalarySchemeDetail;
        }


        public string Delete(SalarySchemeDetail oSalarySchemeDetail, Int64 nUserID)
        {            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SalarySchemeDetailDA.IUD(tc, oSalarySchemeDetail, nUserID, (int)EnumDBOperation.Delete);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oSalarySchemeDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return Global.DeleteMessage;
        }
        public SalarySchemeDetail Get(int nSalarySchemeDetailID, Int64 nUserId)
        {
            SalarySchemeDetail oSalarySchemeDetail = new SalarySchemeDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalarySchemeDetailDA.Get(nSalarySchemeDetailID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalarySchemeDetail = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get SalarySchemeDetail", e);
                oSalarySchemeDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oSalarySchemeDetail;
        }
        public List<SalarySchemeDetail> Gets(int nSID, Int64 nUserID)
        {
            List<SalarySchemeDetail> oSalarySchemeDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalarySchemeDetailDA.Gets(tc, nSID);
                oSalarySchemeDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalarySchemeDetail", e);
                #endregion
            }
            return oSalarySchemeDetail;
        }

        public List<SalarySchemeDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<SalarySchemeDetail> oSalarySchemeDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalarySchemeDetailDA.Gets(sSQL, tc);
                oSalarySchemeDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalarySchemeDetail", e);
                #endregion
            }
            return oSalarySchemeDetail;
        }


        #endregion
    }
}
