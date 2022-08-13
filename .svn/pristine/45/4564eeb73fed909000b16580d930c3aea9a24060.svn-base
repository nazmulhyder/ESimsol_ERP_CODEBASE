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
    public class VacancyService : MarshalByRefObject, IVacancyService
    {
        #region Private functions and declaration
        private Vacancy MapObject(NullHandler oReader)
        {
            Vacancy oVacancy = new Vacancy();
            
            oVacancy.DepartmentID = oReader.GetInt32("DepartmentID");
            oVacancy.DepartmentName = oReader.GetString("DepartmentName");
            oVacancy.DesignationID = oReader.GetInt32("DesignationID");
            oVacancy.DesignationName = oReader.GetString("DesignationName");
            oVacancy.ExistPerson = oReader.GetInt32("ExistPerson");
            oVacancy.RequiredPerson = oReader.GetInt32("RequiredPerson");
            oVacancy.ShiftID = oReader.GetInt32("ShiftID");
            oVacancy.ShiftName = oReader.GetString("ShiftName");

            return oVacancy;

        }

        private Vacancy CreateObject(NullHandler oReader)
        {
            Vacancy oVacancy = MapObject(oReader);
            return oVacancy;
        }

        private List<Vacancy> CreateObjects(IDataReader oReader)
        {
            List<Vacancy> oVacancy = new List<Vacancy>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Vacancy oItem = CreateObject(oHandler);
                oVacancy.Add(oItem);
            }
            return oVacancy;
        }

        #endregion

        #region Interface implementation
        public VacancyService() { }

        public List<Vacancy> Gets(string sSQL, Int64 nUserID)
        {
            List<Vacancy> oVacancy = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VacancyDA.Gets(sSQL, tc);
                oVacancy = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_Vacancy", e);
                #endregion
            }
            return oVacancy;
        }

        #endregion

        
    }
}
