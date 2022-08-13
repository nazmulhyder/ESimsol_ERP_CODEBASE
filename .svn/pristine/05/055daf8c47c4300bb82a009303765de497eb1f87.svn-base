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
    public class ITaxBasicInformationService : MarshalByRefObject, IITaxBasicInformationService
    {
        #region Private functions and declaration
        private ITaxBasicInformation MapObject(NullHandler oReader)
        {
            ITaxBasicInformation oITaxBasicInformation = new ITaxBasicInformation();

            oITaxBasicInformation.ITaxBasicInformationID = oReader.GetInt32("ITaxBasicInformationID");
            oITaxBasicInformation.EmployeeID = oReader.GetInt32("EmployeeID");
            oITaxBasicInformation.TIN = oReader.GetString("TIN");
            oITaxBasicInformation.ETIN = oReader.GetString("ETIN");
            oITaxBasicInformation.NationalID = oReader.GetString("NationalID");
            oITaxBasicInformation.TaxArea = (EnumTaxArea)oReader.GetInt16("TaxArea");
            oITaxBasicInformation.Cercile = oReader.GetString("Cercile");
            oITaxBasicInformation.Zone = oReader.GetString("Zone");
            oITaxBasicInformation.IsNonResident = oReader.GetBoolean("IsNonResident");
            oITaxBasicInformation.IsSelf = oReader.GetBoolean("IsSelf");

            return oITaxBasicInformation;

        }

        private ITaxBasicInformation CreateObject(NullHandler oReader)
        {
            ITaxBasicInformation oITaxBasicInformation = MapObject(oReader);
            return oITaxBasicInformation;
        }

        private List<ITaxBasicInformation> CreateObjects(IDataReader oReader)
        {
            List<ITaxBasicInformation> oITaxBasicInformations = new List<ITaxBasicInformation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxBasicInformation oItem = CreateObject(oHandler);
                oITaxBasicInformations.Add(oItem);
            }
            return oITaxBasicInformations;
        }

        #endregion

        #region Interface implementation
        public ITaxBasicInformationService() { }

        public ITaxBasicInformation IUD(ITaxBasicInformation oITaxBasicInformation, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxBasicInformationDA.IUD(tc, oITaxBasicInformation, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oITaxBasicInformation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxBasicInformation.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oITaxBasicInformation.ITaxBasicInformationID = 0;
                #endregion
            }
            return oITaxBasicInformation;
        }

        public ITaxBasicInformation Get(int nITaxBasicInformationID, Int64 nUserId)
        {
            ITaxBasicInformation oITaxBasicInformation = new ITaxBasicInformation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxBasicInformationDA.Get(nITaxBasicInformationID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxBasicInformation = CreateObject(oReader);
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

                oITaxBasicInformation.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxBasicInformation;
        }

        public ITaxBasicInformation Get(string sSQL, Int64 nUserId)
        {
            ITaxBasicInformation oITaxBasicInformation = new ITaxBasicInformation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxBasicInformationDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxBasicInformation = CreateObject(oReader);
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

                oITaxBasicInformation.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxBasicInformation;
        }

        public List<ITaxBasicInformation> Gets(Int64 nUserID)
        {
            List<ITaxBasicInformation> oITaxBasicInformation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxBasicInformationDA.Gets(tc);
                oITaxBasicInformation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxBasicInformation", e);
                #endregion
            }
            return oITaxBasicInformation;
        }

        public List<ITaxBasicInformation> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxBasicInformation> oITaxBasicInformation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxBasicInformationDA.Gets(sSQL, tc);
                oITaxBasicInformation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxBasicInformation", e);
                #endregion
            }
            return oITaxBasicInformation;
        }

        #endregion

       
    }
}
