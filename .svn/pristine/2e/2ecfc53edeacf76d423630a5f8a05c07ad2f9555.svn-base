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
    public class SalarySchemeDetailCalculationService : MarshalByRefObject, ISalarySchemeDetailCalculationService
    {
        #region Private functions and declaration
        private SalarySchemeDetailCalculation MapObject(NullHandler oReader)
        {
            SalarySchemeDetailCalculation oSalarySchemeDetailCalculation = new SalarySchemeDetailCalculation();

            oSalarySchemeDetailCalculation.GSCID = oReader.GetInt32("GSCID");
            oSalarySchemeDetailCalculation.SalarySchemeID = oReader.GetInt32("SalarySchemeID");

            oSalarySchemeDetailCalculation.SSDCID = oReader.GetInt32("SSDCID");
            oSalarySchemeDetailCalculation.SalarySchemeDetailID = oReader.GetInt32("SalarySchemeDetailID");
            oSalarySchemeDetailCalculation.ValueOperator = (EnumValueOperator)oReader.GetInt16("ValueOperator");
            oSalarySchemeDetailCalculation.ValueOperatorInt = oReader.GetInt16("ValueOperator");
            oSalarySchemeDetailCalculation.CalculationOn = (EnumSalaryCalculationOn)oReader.GetInt32("CalculationOn");
            oSalarySchemeDetailCalculation.CalculationOnInt = oReader.GetInt32("CalculationOn");
            oSalarySchemeDetailCalculation.FixedValue = oReader.GetDouble("FixedValue");
            oSalarySchemeDetailCalculation.Operator = (EnumOperator)oReader.GetInt16("Operator");
            oSalarySchemeDetailCalculation.OperatorInt = oReader.GetInt16("Operator");
            oSalarySchemeDetailCalculation.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oSalarySchemeDetailCalculation.PercentVelue = oReader.GetDouble("PercentVelue");
            oSalarySchemeDetailCalculation.SalaryHeadName = oReader.GetString("SalaryHeadName");

            return oSalarySchemeDetailCalculation;

        }

        private SalarySchemeDetailCalculation CreateObject(NullHandler oReader)
        {
            SalarySchemeDetailCalculation oSalarySchemeDetailCalculation = MapObject(oReader);
            return oSalarySchemeDetailCalculation;
        }

        private List<SalarySchemeDetailCalculation> CreateObjects(IDataReader oReader)
        {
            List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculation = new List<SalarySchemeDetailCalculation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalarySchemeDetailCalculation oItem = CreateObject(oHandler);
                oSalarySchemeDetailCalculation.Add(oItem);
            }
            return oSalarySchemeDetailCalculation;
        }

        #endregion

        #region Interface implementation
        public SalarySchemeDetailCalculationService() { }

        public SalarySchemeDetailCalculation IUD(SalarySchemeDetailCalculation oSalarySchemeDetailCalculation, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SalarySchemeDetailCalculationDA.IUD(tc, oSalarySchemeDetailCalculation, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oSalarySchemeDetailCalculation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSalarySchemeDetailCalculation.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oSalarySchemeDetailCalculation.SSDCID = 0;
                #endregion
            }
            return oSalarySchemeDetailCalculation;
        }

        public SalarySchemeDetailCalculation IUDGross(SalarySchemeDetailCalculation oSalarySchemeDetailCalculation, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SalarySchemeDetailCalculationDA.IUD(tc, oSalarySchemeDetailCalculation, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oSalarySchemeDetailCalculation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSalarySchemeDetailCalculation.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oSalarySchemeDetailCalculation.SSDCID = 0;
                #endregion
            }
            return oSalarySchemeDetailCalculation;
        }

        public SalarySchemeDetailCalculation Get(int nSSDCID, Int64 nUserId)
        {
            SalarySchemeDetailCalculation oSalarySchemeDetailCalculation = new SalarySchemeDetailCalculation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalarySchemeDetailCalculationDA.Get(nSSDCID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalarySchemeDetailCalculation = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get SalarySchemeDetailCalculation", e);
                oSalarySchemeDetailCalculation.ErrorMessage = e.Message;
                #endregion
            }

            return oSalarySchemeDetailCalculation;
        }
        public List<SalarySchemeDetailCalculation> Gets(Int64 nUserID)
        {
            List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalarySchemeDetailCalculationDA.Gets(tc);
                oSalarySchemeDetailCalculation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_SalarySchemeDetailCalculation", e);
                #endregion
            }
            return oSalarySchemeDetailCalculation;
        }

        public List<SalarySchemeDetailCalculation> GetsBySalarySchemeDetail(int nSchemeDetailID, Int64 nUserID)
        {
            List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculation = new List<SalarySchemeDetailCalculation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SalarySchemeDetailCalculationDA.GetsBySalarySchemeDetail(tc, nSchemeDetailID);
                oSalarySchemeDetailCalculation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalarySchemeDetailCalculation", e);
                #endregion
            }
            return oSalarySchemeDetailCalculation;
        }
        public List<SalarySchemeDetailCalculation> GetsBySalarySchemeGross(int nSchemeDetailID, Int64 nUserID)
        {
            List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculation = new List<SalarySchemeDetailCalculation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SalarySchemeDetailCalculationDA.GetsBySalarySchemeGross(tc, nSchemeDetailID);
                oSalarySchemeDetailCalculation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Gross Salary Calculation", e);
                #endregion
            }
            return oSalarySchemeDetailCalculation;
        }
        public List<SalarySchemeDetailCalculation> Gets(string sSQL, Int64 nUserID)
        {
            List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculation = new List<SalarySchemeDetailCalculation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SalarySchemeDetailCalculationDA.Gets(sSQL, tc);
                oSalarySchemeDetailCalculation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_SalarySchemeDetailCalculation", e);
                #endregion
            }
            return oSalarySchemeDetailCalculation;
        }


        #endregion
        
    }
}
