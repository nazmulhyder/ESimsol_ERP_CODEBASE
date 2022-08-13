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
    public class ITaxHeadEquationService : MarshalByRefObject, IITaxHeadEquationService
    {
        #region Private functions and declaration
        private ITaxHeadEquation MapObject(NullHandler oReader)
        {
            ITaxHeadEquation oITaxHeadEquation = new ITaxHeadEquation();

            oITaxHeadEquation.ITaxHeadEquationID = oReader.GetInt32("ITaxHeadEquationID");
            oITaxHeadEquation.ITaxHeadConfigurationID = oReader.GetInt32("ITaxHeadConfigurationID");
            oITaxHeadEquation.CalculateOn = (EnumSalaryCalculationOn)oReader.GetInt16("CalculateOn");
            oITaxHeadEquation.Value = oReader.GetDouble("Value");
            oITaxHeadEquation.SalaryHeadID = oReader.GetInt16("SalaryHeadID");
            //derive
            oITaxHeadEquation.SalaryHeadName = oReader.GetString("SalaryHeadName");

            return oITaxHeadEquation;

        }

        private ITaxHeadEquation CreateObject(NullHandler oReader)
        {
            ITaxHeadEquation oITaxHeadEquation = MapObject(oReader);
            return oITaxHeadEquation;
        }

        private List<ITaxHeadEquation> CreateObjects(IDataReader oReader)
        {
            List<ITaxHeadEquation> oITaxHeadEquations = new List<ITaxHeadEquation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxHeadEquation oItem = CreateObject(oHandler);
                oITaxHeadEquations.Add(oItem);
            }
            return oITaxHeadEquations;
        }

        #endregion

        #region Interface implementation
        public ITaxHeadEquationService() { }

        public ITaxHeadEquation IUD(ITaxHeadEquation oITaxHeadEquation, int nDBOperation, Int64 nUserID)
        {


            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxHeadEquationDA.IUD(tc, oITaxHeadEquation, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oITaxHeadEquation = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxHeadEquation.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oITaxHeadEquation.ITaxHeadEquationID = 0;
                #endregion
            }
            return oITaxHeadEquation;
        }


        public ITaxHeadEquation Get(int nITaxHeadEquationID, Int64 nUserId)
        {
            ITaxHeadEquation oITaxHeadEquation = new ITaxHeadEquation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxHeadEquationDA.Get(nITaxHeadEquationID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxHeadEquation = CreateObject(oReader);
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

                oITaxHeadEquation.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxHeadEquation;
        }

        public ITaxHeadEquation Get(string sSQL, Int64 nUserId)
        {
            ITaxHeadEquation oITaxHeadEquation = new ITaxHeadEquation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxHeadEquationDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxHeadEquation = CreateObject(oReader);
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

                oITaxHeadEquation.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxHeadEquation;
        }

        public List<ITaxHeadEquation> Gets(Int64 nUserID)
        {
            List<ITaxHeadEquation> oITaxHeadEquation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxHeadEquationDA.Gets(tc);
                oITaxHeadEquation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxHeadEquation", e);
                #endregion
            }
            return oITaxHeadEquation;
        }

        public List<ITaxHeadEquation> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxHeadEquation> oITaxHeadEquation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxHeadEquationDA.Gets(sSQL, tc);
                oITaxHeadEquation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxHeadEquation", e);
                #endregion
            }
            return oITaxHeadEquation;
        }

        #endregion


    }
}
