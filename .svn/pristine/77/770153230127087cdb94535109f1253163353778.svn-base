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
    public class EmployeeConfirmationService : MarshalByRefObject, IEmployeeConfirmationService
    {
        #region Private functions and declaration
        private EmployeeConfirmation MapObject(NullHandler oReader)
        {
            EmployeeConfirmation oEmployeeConfirmation = new EmployeeConfirmation();
            oEmployeeConfirmation.ECID = oReader.GetInt32("ECID");
            oEmployeeConfirmation.EmployeeCategory = (EnumEmployeeCategory)oReader.GetInt16("EmployeeCategory");
            oEmployeeConfirmation.EmployeeCategoryInt = (int)(EnumEmployeeCategory)oReader.GetInt16("EmployeeCategory");
            oEmployeeConfirmation.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeConfirmation.EndDate = oReader.GetDateTime("EndDate");
            oEmployeeConfirmation.StartDate = oReader.GetDateTime("StartDate");
            oEmployeeConfirmation.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oEmployeeConfirmation.Note = oReader.GetString("Note");
            oEmployeeConfirmation.MotherEmployeeID = oReader.GetInt32("MotherEmployeeID");
            oEmployeeConfirmation.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeConfirmation.EmployeeName = oReader.GetString("EmployeeName");

            return oEmployeeConfirmation;
        }

        private EmployeeConfirmation CreateObject(NullHandler oReader)
        {
            EmployeeConfirmation oEmployeeConfirmation = MapObject(oReader);
            return oEmployeeConfirmation;
        }

        private List<EmployeeConfirmation> CreateObjects(IDataReader oReader)
        {
            List<EmployeeConfirmation> oEmployeeConfirmations = new List<EmployeeConfirmation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeConfirmation oItem = CreateObject(oHandler);
                oEmployeeConfirmations.Add(oItem);
            }
            return oEmployeeConfirmations;
        }

        #endregion

        #region Interface implementation
        public EmployeeConfirmationService() { }

        public EmployeeConfirmation IUD(EmployeeConfirmation oEmployeeConfirmation, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeConfirmationDA.IUD(tc, oEmployeeConfirmation, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeConfirmation = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oEmployeeConfirmation = new EmployeeConfirmation();
                    oEmployeeConfirmation.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeConfirmation = new EmployeeConfirmation();
                oEmployeeConfirmation.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oEmployeeConfirmation;
        }

        public EmployeeConfirmation Get(int nECID, Int64 nUserId)
        {
            EmployeeConfirmation oEmployeeConfirmation = new EmployeeConfirmation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = EmployeeConfirmationDA.Get(nECID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeConfirmation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeConfirmation.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployeeConfirmation;
        }

        public EmployeeConfirmation Get(string sSQL, Int64 nUserId)
        {
            EmployeeConfirmation oEmployeeConfirmation = new EmployeeConfirmation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeConfirmationDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeConfirmation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeConfirmation.ErrorMessage = e.Message;
                #endregion
            }
            return oEmployeeConfirmation;
        }

        public List<EmployeeConfirmation> Gets(Int64 nUserID)
        {
            List<EmployeeConfirmation> oEmployeeConfirmations = new List<EmployeeConfirmation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeConfirmationDA.Gets(tc);
                oEmployeeConfirmations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                EmployeeConfirmation oEmployeeConfirmation = new EmployeeConfirmation();
                oEmployeeConfirmation.ErrorMessage = e.Message;
                oEmployeeConfirmations.Add(oEmployeeConfirmation);
                #endregion
            }
            return oEmployeeConfirmations;
        }

        public List<EmployeeConfirmation> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeConfirmation> oEmployeeConfirmations = new List<EmployeeConfirmation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeConfirmationDA.Gets(sSQL, tc);
                oEmployeeConfirmations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                EmployeeConfirmation oEmployeeConfirmation = new EmployeeConfirmation();
                oEmployeeConfirmation.ErrorMessage = e.Message;
                oEmployeeConfirmations.Add(oEmployeeConfirmation);
                #endregion
            }
            return oEmployeeConfirmations;
        }

        #endregion
    }
}
