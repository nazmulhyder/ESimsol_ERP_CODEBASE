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
    public class EmployeeTINInformationService : MarshalByRefObject, IEmployeeTINInformationService
    {
        #region Private functions and declaration
        private EmployeeTINInformation MapObject(NullHandler oReader)
        {
            EmployeeTINInformation oEmployeeTINInformation = new EmployeeTINInformation();

            oEmployeeTINInformation.ETINID = oReader.GetInt32("ETINID");
            oEmployeeTINInformation.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeTINInformation.TIN = oReader.GetString("TIN");
            oEmployeeTINInformation.ETIN = oReader.GetString("ETIN");
            oEmployeeTINInformation.TaxArea = (EnumTaxArea)oReader.GetInt16("TaxArea");
            oEmployeeTINInformation.TaxAreaInint =(int)(EnumTaxArea)oReader.GetInt16("TaxArea");
            oEmployeeTINInformation.Circle = oReader.GetString("Circle");
            oEmployeeTINInformation.Zone = oReader.GetString("Zone");
            oEmployeeTINInformation.IsNonResident = oReader.GetBoolean("IsNonResident");

            return oEmployeeTINInformation;

        }

        private EmployeeTINInformation CreateObject(NullHandler oReader)
        {
            EmployeeTINInformation oEmployeeTINInformation = MapObject(oReader);
            return oEmployeeTINInformation;
        }

        private List<EmployeeTINInformation> CreateObjects(IDataReader oReader)
        {
            List<EmployeeTINInformation> oEmployeeTINInformations = new List<EmployeeTINInformation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeTINInformation oItem = CreateObject(oHandler);
                oEmployeeTINInformations.Add(oItem);
            }
            return oEmployeeTINInformations;
        }

        #endregion

        #region Interface implementation
        public EmployeeTINInformationService() { }

        public EmployeeTINInformation IUD(EmployeeTINInformation oEmployeeTINInformation, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeTINInformationDA.IUD(tc, oEmployeeTINInformation, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeTINInformation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeTINInformation.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                //oEmployeeTINInformation.ETINID = 0;
                #endregion
            }
            return oEmployeeTINInformation;
        }

        public string Upload(EmployeeTINInformation oEmployeeTINInformation,  Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
               EmployeeTINInformationDA.Upload(tc, oEmployeeTINInformation, nUserID, (int)EnumDBOperation.Upload);
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
            return "";
        }

        public EmployeeTINInformation Get(int nEMpID, Int64 nUserId)
        {
            EmployeeTINInformation oEmployeeTINInformation = new EmployeeTINInformation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeTINInformationDA.Get(nEMpID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeTINInformation = CreateObject(oReader);
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

                oEmployeeTINInformation.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeTINInformation;
        }

        public EmployeeTINInformation Get(string sSQL, Int64 nUserId)
        {
            EmployeeTINInformation oEmployeeTINInformation = new EmployeeTINInformation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeTINInformationDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeTINInformation = CreateObject(oReader);
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

                oEmployeeTINInformation.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeTINInformation;
        }
        #endregion


    }
}
