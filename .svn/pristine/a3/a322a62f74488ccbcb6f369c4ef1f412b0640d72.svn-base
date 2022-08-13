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
    public class EmployeeDocumentAcceptanceService : MarshalByRefObject, IEmployeeDocumentAcceptanceService
    {
        #region Private functions and declaration
        private EmployeeDocumentAcceptance MapObject(NullHandler oReader)
        {
            EmployeeDocumentAcceptance oEmployeeDocumentAcceptance = new EmployeeDocumentAcceptance();
            oEmployeeDocumentAcceptance.EDAID = oReader.GetInt32("EDAID");
            oEmployeeDocumentAcceptance.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeDocumentAcceptance.EmployeeName = oReader.GetString("EmployeeName");
            return oEmployeeDocumentAcceptance;

        }

        private EmployeeDocumentAcceptance CreateObject(NullHandler oReader)
        {
            EmployeeDocumentAcceptance oEmployeeDocumentAcceptance = MapObject(oReader);
            return oEmployeeDocumentAcceptance;
        }

        private List<EmployeeDocumentAcceptance> CreateObjects(IDataReader oReader)
        {
            List<EmployeeDocumentAcceptance> oEmployeeDocumentAcceptance = new List<EmployeeDocumentAcceptance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeDocumentAcceptance oItem = CreateObject(oHandler);
                oEmployeeDocumentAcceptance.Add(oItem);
            }
            return oEmployeeDocumentAcceptance;
        }


        #endregion

        #region Interface implementation
        public EmployeeDocumentAcceptanceService() { }
        public EmployeeDocumentAcceptance IUD(EmployeeDocumentAcceptance oEmployeeDocumentAcceptance, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    reader = EmployeeDocumentAcceptanceDA.IUD(tc, oEmployeeDocumentAcceptance, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oEmployeeDocumentAcceptance = new EmployeeDocumentAcceptance();
                        oEmployeeDocumentAcceptance = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = EmployeeDocumentAcceptanceDA.IUD(tc, oEmployeeDocumentAcceptance, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oEmployeeDocumentAcceptance.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oEmployeeDocumentAcceptance = new EmployeeDocumentAcceptance();
                oEmployeeDocumentAcceptance.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oEmployeeDocumentAcceptance;
        }



        public EmployeeDocumentAcceptance Get(string sSQL, Int64 nUserId)
        {
            EmployeeDocumentAcceptance oEmployeeDocumentAcceptance = new EmployeeDocumentAcceptance();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeDocumentAcceptanceDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeDocumentAcceptance = CreateObject(oReader);
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
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeDocumentAcceptance;
        }

        public List<EmployeeDocumentAcceptance> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeDocumentAcceptance> oEmployeeDocumentAcceptance = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeDocumentAcceptanceDA.Gets(sSQL, tc);
                oEmployeeDocumentAcceptance = CreateObjects(reader);
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
            return oEmployeeDocumentAcceptance;
        }
        #endregion
    }
}


