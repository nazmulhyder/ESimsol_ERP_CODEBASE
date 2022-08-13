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
    public class EmployeeReferenceService : MarshalByRefObject, IEmployeeReferenceService
    {
        #region Private functions and declaration
        private EmployeeReference MapObject(NullHandler oReader)
        {
            EmployeeReference oER = new EmployeeReference();
            oER.EmployeeReferenceID = oReader.GetInt32("EmployeeReferenceID");
            oER.EmployeeID = oReader.GetInt32("EmployeeID");
            oER.Name = oReader.GetString("Name");
            oER.Designation = oReader.GetString("Designation");
            oER.Organization = oReader.GetString("Organization");
            oER.ContactNo = oReader.GetString("ContactNo");
            oER.Address = oReader.GetString("Address");
            oER.Relation = oReader.GetString("Relation");
            oER.Description = oReader.GetString("Description");
            return oER;
        }

        private EmployeeReference CreateObject(NullHandler oReader)
        {
            EmployeeReference oEmployeeReference = new EmployeeReference();
            oEmployeeReference = MapObject(oReader);
            return oEmployeeReference;
        }

        private List<EmployeeReference> CreateObjects(IDataReader oReader)
        {
            List<EmployeeReference> oEmployeeReference = new List<EmployeeReference>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeReference oItem = CreateObject(oHandler);
                oEmployeeReference.Add(oItem);
            }
            return oEmployeeReference;
        }

        #endregion

        #region Interface implementation
        public EmployeeReferenceService() { }

        public EmployeeReference IUD(EmployeeReference oER, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeReferenceDA.IUD(tc, oER, nDBOperation, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oER = new EmployeeReference();
                    oER = CreateObject(oReader);
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
                oER.ErrorMessage = e.Message;
                //throw new ServiceException("Failed to Save EmployeeReference. Because of " + e.Message, e);
                #endregion
            }
            return oER;
        }


        public EmployeeReference Get(int id, Int64 nUserId) //EmployeeReferenceID
        {
            EmployeeReference oER = new EmployeeReference();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeReferenceDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oER = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeReference", e);
                #endregion
            }

            return oER;
        }

        public List<EmployeeReference> Gets(int nEmployeeID, Int64 nUserID)
        {
            List<EmployeeReference> oEmployeeReference = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeReferenceDA.Gets(tc, nEmployeeID);
                oEmployeeReference = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeReference", e);
                #endregion
            }

            return oEmployeeReference;
        }

        #endregion
    }
}