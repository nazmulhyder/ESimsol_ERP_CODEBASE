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
    public class ITaxHeadConfigurationService : MarshalByRefObject, IITaxHeadConfigurationService
    {
        #region Private functions and declaration
        private ITaxHeadConfiguration MapObject(NullHandler oReader)
        {
            ITaxHeadConfiguration oITaxHeadConfiguration = new ITaxHeadConfiguration();

            oITaxHeadConfiguration.ITaxHeadConfigurationID = oReader.GetInt32("ITaxHeadConfigurationID");
            oITaxHeadConfiguration.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oITaxHeadConfiguration.IsExempted = oReader.GetBoolean("IsExempted");
            oITaxHeadConfiguration.MaxExemptAmount = oReader.GetDouble("MaxExemptAmount");
            oITaxHeadConfiguration.CalculationOn = (EnumSalaryCalculationOn)oReader.GetInt16("CalculationOn");
            oITaxHeadConfiguration.CalculationSalaryHeadID = oReader.GetInt32("CalculationSalaryHeadID");
            oITaxHeadConfiguration.PercentOfCalculation = oReader.GetDouble("PercentOfCalculation");
            oITaxHeadConfiguration.InactiveDate = oReader.GetDateTime("InactiveDate");
            oITaxHeadConfiguration.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oITaxHeadConfiguration.CalculationHead = oReader.GetString("CalculationHead");

            return oITaxHeadConfiguration;

        }

        private ITaxHeadConfiguration CreateObject(NullHandler oReader)
        {
            ITaxHeadConfiguration oITaxHeadConfiguration = MapObject(oReader);
            return oITaxHeadConfiguration;
        }

        private List<ITaxHeadConfiguration> CreateObjects(IDataReader oReader)
        {
            List<ITaxHeadConfiguration> oITaxHeadConfigurations = new List<ITaxHeadConfiguration>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxHeadConfiguration oItem = CreateObject(oHandler);
                oITaxHeadConfigurations.Add(oItem);
            }
            return oITaxHeadConfigurations;
        }

        #endregion

        #region Interface implementation
        public ITaxHeadConfigurationService() { }

        public ITaxHeadConfiguration IUD(ITaxHeadConfiguration oITaxHeadConfiguration, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxHeadConfigurationDA.IUD(tc, oITaxHeadConfiguration, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxHeadConfiguration = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oITaxHeadConfiguration = new ITaxHeadConfiguration();
                    oITaxHeadConfiguration.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxHeadConfiguration = new ITaxHeadConfiguration();
                oITaxHeadConfiguration.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oITaxHeadConfiguration;
        }

        public ITaxHeadConfiguration Get(int nITaxHeadConfigurationID, Int64 nUserId)
        {
            ITaxHeadConfiguration oITaxHeadConfiguration = new ITaxHeadConfiguration();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxHeadConfigurationDA.Get(nITaxHeadConfigurationID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxHeadConfiguration = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxHeadConfiguration.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxHeadConfiguration;
        }

        public List<ITaxHeadConfiguration> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxHeadConfiguration> oITaxHeadConfigurations = new List<ITaxHeadConfiguration>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxHeadConfigurationDA.Gets(sSQL, tc);
                oITaxHeadConfigurations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ITaxHeadConfiguration oITaxHeadConfiguration = new ITaxHeadConfiguration();
                oITaxHeadConfiguration.ErrorMessage = e.Message;
                oITaxHeadConfigurations.Add(oITaxHeadConfiguration);
                #endregion
            }
            return oITaxHeadConfigurations;
        }

        #endregion

    }
}
