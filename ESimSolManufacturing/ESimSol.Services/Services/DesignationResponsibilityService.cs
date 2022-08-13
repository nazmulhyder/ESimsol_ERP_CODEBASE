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
    public class DesignationResponsibilityService : MarshalByRefObject, IDesignationResponsibilityService
    {
        #region Private functions and declaration
        private DesignationResponsibility MapObject(NullHandler oReader)
        {
            DesignationResponsibility oDesignationResponsibility = new DesignationResponsibility();
            oDesignationResponsibility.DesignationResponsibilityID = oReader.GetInt32("DesignationResponsibilityID");
            oDesignationResponsibility.DRPID = oReader.GetInt32("DRPID");
            oDesignationResponsibility.HRResponsibilityID = oReader.GetInt32("HRResponsibilityID");
            oDesignationResponsibility.HRResponsibilityCode = oReader.GetString("HRResponsibilityCode");
            oDesignationResponsibility.HRResponsibilityText = oReader.GetString("HRResponsibilityText");
            oDesignationResponsibility.HRResponsibilityTextInBangla = oReader.GetString("HRResponsibilityTextInBangla");
            oDesignationResponsibility.DesignationID = oReader.GetInt32("DesignationID");
            return oDesignationResponsibility;
        }

        private DesignationResponsibility CreateObject(NullHandler oReader)
        {
            DesignationResponsibility oDesignationResponsibility = MapObject(oReader);
            return oDesignationResponsibility;
        }

        private List<DesignationResponsibility> CreateObjects(IDataReader oReader)
        {
            List<DesignationResponsibility> oDesignationResponsibility = new List<DesignationResponsibility>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DesignationResponsibility oItem = CreateObject(oHandler);
                oDesignationResponsibility.Add(oItem);
            }
            return oDesignationResponsibility;
        }

        #endregion

        #region Interface implementation
        public DesignationResponsibilityService() { }

        public List<DesignationResponsibility> Gets(Int64 nUserID)
        {
            List<DesignationResponsibility> oDesignationResponsibility = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DesignationResponsibilityDA.Gets(tc);
                oDesignationResponsibility = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DesignationResponsibility", e);
                #endregion
            }

            return oDesignationResponsibility;
        }

        public List<DesignationResponsibility> GetsByPolicy(int nDepartmentRequirementPolicyID, Int64 nUserID)
        {
            List<DesignationResponsibility> oDesignationResponsibility = new List<DesignationResponsibility>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DesignationResponsibilityDA.GetsByPolicy(tc, nDepartmentRequirementPolicyID);
                oDesignationResponsibility = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DesignationResponsibility", e);
                #endregion
            }

            return oDesignationResponsibility;
        }

        public List<DesignationResponsibility> Gets(int nDepartmentRequirementDesignationID, Int64 nUserID)
        {
            List<DesignationResponsibility> oDesignationResponsibility = new List<DesignationResponsibility>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DesignationResponsibilityDA.Gets(tc, nDepartmentRequirementDesignationID);
                oDesignationResponsibility = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DesignationResponsibility", e);
                #endregion
            }

            return oDesignationResponsibility;
        }
        public List<DesignationResponsibility> Gets(string sSQL, Int64 nUserID)
        {
            List<DesignationResponsibility> oDesignationResponsibility = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DesignationResponsibilityDA.Gets(tc, sSQL);
                oDesignationResponsibility = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DesignationResponsibility", e);
                #endregion
            }

            return oDesignationResponsibility;
        }
        public DesignationResponsibility Save(DesignationResponsibility oDesignationResponsibility, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<DesignationResponsibility> oDRs = new List<DesignationResponsibility>();
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (DesignationResponsibility oitem in oDesignationResponsibility.DesignationResponsibilitys)
                {
                    IDataReader reader;
                    reader = DesignationResponsibilityDA.InsertUpdate(tc, oitem, nUserID, EnumDBOperation.Insert);

                    if (reader.Read())
                    {
                        NullHandler oReader = new NullHandler(reader);
                        var oDR = new DesignationResponsibility();
                        oDR = CreateObject(oReader);
                        oDRs.Add(oDR);
                        
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDesignationResponsibility = new DesignationResponsibility();
                oDesignationResponsibility.ErrorMessage = e.Message.Split('!')[0]; 
                #endregion
            }
            oDesignationResponsibility.DesignationResponsibilitys = oDRs;

            return oDesignationResponsibility;
        }

        #endregion
    }
}
