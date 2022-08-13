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
    public class EmployeeExperienceService : MarshalByRefObject, IEmployeeExperienceService
    {
        #region Private functions and declaration
        private EmployeeExperience MapObject(NullHandler oReader)
        {
            EmployeeExperience oEE = new EmployeeExperience();
            oEE.EmployeeExperienceID = oReader.GetInt32("EmployeeExperienceID");
            oEE.EmployeeID = oReader.GetInt32("EmployeeID");
            oEE.Organization = oReader.GetString("Organization");
            oEE.OrganizationType = oReader.GetString("OrganizationType");
            oEE.Address = oReader.GetString("Address");
            oEE.Designation = oReader.GetString("Designation");
            oEE.StartDate = oReader.GetDateTime("StartDate");
            oEE.StartDateExFormatType = (EnumCustomDateFormat)oReader.GetInt32("StartDateExFormatType");
            oEE.EndDate = oReader.GetDateTime("EndDate");
            oEE.EndDateExFormatType = (EnumCustomDateFormat)oReader.GetInt32("EndDateExFormatType");
            oEE.Duration = oReader.GetString("Duration");
            oEE.MajorResponsibility = oReader.GetString("MajorResponsibility");

            return oEE;
        }

        private EmployeeExperience CreateObject(NullHandler oReader)
        {
            EmployeeExperience oEmployeeExperience = new EmployeeExperience();
            oEmployeeExperience = MapObject(oReader);
            return oEmployeeExperience;
        }

        private List<EmployeeExperience> CreateObjects(IDataReader oReader)
        {
            List<EmployeeExperience> oEmployeeExperience = new List<EmployeeExperience>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeExperience oItem = CreateObject(oHandler);
                oEmployeeExperience.Add(oItem);
            }
            return oEmployeeExperience;
        }

        #endregion

        #region Interface implementation
        public EmployeeExperienceService() { }

        public EmployeeExperience IUD(EmployeeExperience oEE, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeExperienceDA.IUD(tc, oEE, nDBOperation, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEE = new EmployeeExperience();
                    oEE = CreateObject(oReader);
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
                oEE.ErrorMessage = e.Message;
                //throw new ServiceException("Failed to Save EmployeeExperience. Because of " + e.Message, e);
                #endregion
            }
            return oEE;
        }


        public EmployeeExperience Get(int id, Int64 nUserId) //EmployeeExperienceID
        {
            EmployeeExperience oEE = new EmployeeExperience();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeExperienceDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEE = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeExperience", e);
                #endregion
            }

            return oEE;
        }

        public List<EmployeeExperience> Gets(int nEmployeeID, Int64 nUserID)
        {
            List<EmployeeExperience> oEmployeeExperience = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeExperienceDA.Gets(tc, nEmployeeID);
                oEmployeeExperience = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeExperience", e);
                #endregion
            }

            return oEmployeeExperience;
        }

        #endregion
    }
}