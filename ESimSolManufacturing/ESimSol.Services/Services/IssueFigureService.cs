using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    public class IssueFigureService : MarshalByRefObject, IIssueFigureService
    {
        #region Private functions and declaration
        private IssueFigure MapObject(NullHandler oReader)
        {
            IssueFigure oIssueFigure = new IssueFigure();
            oIssueFigure.IssueFigureID = oReader.GetInt32("IssueFigureID");
            oIssueFigure.ContractorID = oReader.GetInt32("ContractorID"); ;
            oIssueFigure.ChequeIssueTo = oReader.GetString("ChequeIssueTo"); ;
            oIssueFigure.SecondLineIssueTo = oReader.GetString("SecondLineIssueTo"); ;
            oIssueFigure.DetailNote = oReader.GetString("DetailNote"); ;
            oIssueFigure.IsActive = oReader.GetBoolean("IsActive"); ;
            return oIssueFigure;
        }

        private IssueFigure CreateObject(NullHandler oReader)
        {
            IssueFigure oIssueFigure = new IssueFigure();
            oIssueFigure = MapObject(oReader);
            return oIssueFigure;
        }

        private List<IssueFigure> CreateObjects(IDataReader oReader)
        {
            List<IssueFigure> oIssueFigures = new List<IssueFigure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                IssueFigure oItem = CreateObject(oHandler);
                oIssueFigures.Add(oItem);
            }
            return oIssueFigures;
        }

        #endregion

        #region Interface implementation
        public IssueFigureService() { }

        public IssueFigure Save(IssueFigure oIssueFigure, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {                
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                if (oIssueFigure.IssueFigureID <= 0)
                {
                    reader = IssueFigureDA.InsertUpdate(tc, oIssueFigure, EnumDBOperation.Insert, nCurrentUserID);
                }
                else
                {
                    reader = IssueFigureDA.InsertUpdate(tc, oIssueFigure, EnumDBOperation.Update, nCurrentUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oIssueFigure = new IssueFigure();
                    oIssueFigure = CreateObject(oReader);
                }
                reader.Close();              
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oIssueFigure.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save IssueFigure. Because of " + e.Message, e);
                #endregion
            }
            
            return oIssueFigure;
        }
        public string Delete(int nIssueFigureID, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IssueFigure oIssueFigure = new IssueFigure();
                oIssueFigure.IssueFigureID = nIssueFigureID;
                AuthorizationRoleDA.CheckUserPermission(tc, nCurrentUserID, EnumModuleName.IssueFigure, EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "IssueFigure", nIssueFigureID);
                IssueFigureDA.Delete(tc, oIssueFigure, EnumDBOperation.Delete, nCurrentUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public IssueFigure Get(int id, int nCurrentUserID)
        {
            IssueFigure oIssueFigure = new IssueFigure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = IssueFigureDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oIssueFigure = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get IssueFigure", e);
                #endregion
            }

            return oIssueFigure;
        }
        public List<IssueFigure> Gets(int nCurrentUserID)
        {
            List<IssueFigure> oIssueFigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IssueFigureDA.Gets(tc);
                oIssueFigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IssueFigure", e);
                #endregion
            }

            return oIssueFigure;
        }
        public List<IssueFigure> Gets(int nContractorID, int nCurrentUserID)
        {
            List<IssueFigure> oIssueFigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IssueFigureDA.Gets(tc, nContractorID);
                oIssueFigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IssueFigure", e);
                #endregion
            }

            return oIssueFigure;
        }

        public List<IssueFigure> Gets(int nContractorID, bool bIsActive, int nCurrentUserID)
        {
            List<IssueFigure> oIssueFigures = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IssueFigureDA.Gets(tc, nContractorID, bIsActive);
                oIssueFigures = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IssueFigure", e);
                #endregion
            }

            return oIssueFigures;
        }

        

        public List<IssueFigure> GetsByName(int nContractorID, string sName, int nCurrentUserID)
        {
            List<IssueFigure> oIssueFigures = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = IssueFigureDA.GetsByName(tc, nContractorID, sName);
                oIssueFigures = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IssueFigure", e);
                #endregion
            }

            return oIssueFigures;
        }

        public List<IssueFigure> Gets(string sSQL, int nCurrentUserID)
        {
            List<IssueFigure> oIssueFigures = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IssueFigureDA.Gets(tc, sSQL);
                oIssueFigures = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IssueFigure", e);
                #endregion
            }

            return oIssueFigures;
        }        
        #endregion
    }
}
