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
    public class SuperAdminService : MarshalByRefObject, ISuperAdminService
    {
        #region Private functions and declaration
        private SuperAdmin MapObject(NullHandler oReader)
        {
            SuperAdmin oSuperAdmin = new SuperAdmin();
            oSuperAdmin.StartDate = oReader.GetDateTime("StartDate");
            oSuperAdmin.EndDate = oReader.GetDateTime("StartDate");
            oSuperAdmin.IsComp = oReader.GetBoolean("IsComp");
            return oSuperAdmin;

        }

        private SuperAdmin CreateObject(NullHandler oReader)
        {
            SuperAdmin oSuperAdmin = MapObject(oReader);
            return oSuperAdmin;
        }

        private List<SuperAdmin> CreateObjects(IDataReader oReader)
        {
            List<SuperAdmin> oSuperAdmin = new List<SuperAdmin>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SuperAdmin oItem = CreateObject(oHandler);
                oSuperAdmin.Add(oItem);
            }
            return oSuperAdmin;
        }


        #endregion

        #region Interface implementation
        public SuperAdminService() { }

        public SuperAdmin MakeDayoffHoliday(string sDate, string eDate, bool isComp, Int64 nUserID)
        {
            SuperAdmin oSuperAdmin = new SuperAdmin();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SuperAdminDA.MakeDayoffHoliday(tc, sDate, eDate, isComp, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSuperAdmin = CreateObject(oReader);
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
                #endregion
            }
            return oSuperAdmin;
        }

        #endregion
    }
}

