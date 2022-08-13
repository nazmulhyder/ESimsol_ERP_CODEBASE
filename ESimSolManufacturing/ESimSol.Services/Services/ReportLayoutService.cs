using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 


namespace ESimSol.Services.Services
{

    public class ReportLayoutService : MarshalByRefObject, IReportLayoutService
    {
        #region Private functions and declaration
        private ReportLayout MapObject(NullHandler oReader)
        {
            ReportLayout oReportLayout = new ReportLayout();
            oReportLayout.ReportLayoutID = oReader.GetInt32("ReportLayoutID");
            oReportLayout.ReportNo = oReader.GetString("ReportNo");
            oReportLayout.ReportName = oReader.GetString("ReportName");
            oReportLayout.ReportType = (EnumReportLayout)oReader.GetInt32("ReportType");
            oReportLayout.ReportTypeInInt = oReader.GetInt32("ReportType");
            oReportLayout.OperationType = (EnumModuleName)oReader.GetInt32("OperationType");

            return oReportLayout;
        }

        private ReportLayout CreateObject(NullHandler oReader)
        {
            ReportLayout oReportLayout = new ReportLayout();
            oReportLayout = MapObject(oReader);
            return oReportLayout;
        }

        private List<ReportLayout> CreateObjects(IDataReader oReader)
        {
            List<ReportLayout> oReportLayout = new List<ReportLayout>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ReportLayout oItem = CreateObject(oHandler);
                oReportLayout.Add(oItem);
            }
            return oReportLayout;
        }

        #endregion

        #region Interface implementation
        public ReportLayoutService() { }

        public ReportLayout Save(ReportLayout oReportLayout, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oReportLayout.ReportLayoutID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ReportLayout, EnumRoleOperationType.Add);
                    reader = ReportLayoutDA.InsertUpdate(tc, oReportLayout, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ReportLayout, EnumRoleOperationType.Edit);
                    reader = ReportLayoutDA.InsertUpdate(tc, oReportLayout, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oReportLayout = new ReportLayout();
                    oReportLayout = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oReportLayout = new ReportLayout();
                oReportLayout.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oReportLayout;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ReportLayout oReportLayout = new ReportLayout();
                oReportLayout.ReportLayoutID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ReportLayout, EnumRoleOperationType.Delete);
                ReportLayoutDA.Delete(tc, oReportLayout, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ReportLayout. Because of " + e.Message, e);
                #endregion
            }
            return "deleted";
        }

        public ReportLayout Get(int id, Int64 nUserId)
        {
            ReportLayout oAccountHead = new ReportLayout();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ReportLayoutDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ReportLayout", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ReportLayout> Gets(Int64 nUserID)
        {
            List<ReportLayout> oReportLayout = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReportLayoutDA.Gets(tc);
                oReportLayout = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReportLayout", e);
                #endregion
            }

            return oReportLayout;
        }

        public List<ReportLayout> Gets(int eEnumperationType, Int64 nUserID)
        {
            List<ReportLayout> oReportLayout = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReportLayoutDA.Gets(tc, eEnumperationType);
                oReportLayout = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReportLayout", e);
                #endregion
            }

            return oReportLayout;
        }

        public List<ReportLayout> Gets(string sSQL, Int64 nUserID)
        {
            List<ReportLayout> oReportLayout = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ReportLayoutDA.Gets(tc, sSQL);
                oReportLayout = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReportLayout", e);
                #endregion
            }

            return oReportLayout;
        }

        #endregion
    }   
    
  
}
