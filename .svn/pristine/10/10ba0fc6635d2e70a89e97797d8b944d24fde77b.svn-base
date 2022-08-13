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
    public class COAChartOfAccountNameAlternativeService : MarshalByRefObject, ICOAChartOfAccountNameAlternativeService
    {
        #region Private functions and declaration
        private COAChartOfAccountNameAlternative MapObject(NullHandler oReader)
        {
            COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
            oCOAChartOfAccountNameAlternative.AlternativeAccountHeadID = oReader.GetInt32("AlternativeAccountHeadID");
            oCOAChartOfAccountNameAlternative.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oCOAChartOfAccountNameAlternative.Name = oReader.GetString("Name");
            oCOAChartOfAccountNameAlternative.Description = oReader.GetString("Description");         

            return oCOAChartOfAccountNameAlternative;
        }

        private COAChartOfAccountNameAlternative CreateObject(NullHandler oReader)
        {
            COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
            oCOAChartOfAccountNameAlternative = MapObject(oReader);
            return oCOAChartOfAccountNameAlternative;
        }

        private List<COAChartOfAccountNameAlternative> CreateObjects(IDataReader oReader)
        {
            List<COAChartOfAccountNameAlternative> oCOAChartOfAccountNameAlternative = new List<COAChartOfAccountNameAlternative>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                COAChartOfAccountNameAlternative oItem = CreateObject(oHandler);
                oCOAChartOfAccountNameAlternative.Add(oItem);
            }
            return oCOAChartOfAccountNameAlternative;
        }

        #endregion

        #region Interface implementation
        public COAChartOfAccountNameAlternativeService() { }

        public List<COAChartOfAccountNameAlternative> Gets(int nUserID)
        {
            List<COAChartOfAccountNameAlternative> oCOAChartOfAccountNameAlternative = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = COAChartOfAccountNameAlternativeDA.Gets(tc);
                oCOAChartOfAccountNameAlternative = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Name", e);
                #endregion
            }

            return oCOAChartOfAccountNameAlternative;
        }

        public List<COAChartOfAccountNameAlternative> GetsByAccountHeadID(int nAccountHeadID, int nUserID)
        {
            List<COAChartOfAccountNameAlternative> oCOAChartOfAccountNameAlternative = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = COAChartOfAccountNameAlternativeDA.GetsByAccountHeadID(tc, nAccountHeadID);
                oCOAChartOfAccountNameAlternative = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Name", e);
                #endregion
            }

            return oCOAChartOfAccountNameAlternative;
        }

        public COAChartOfAccountNameAlternative Save(COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative, int nUserID)
        {
            TransactionContext tc = null;            
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCOAChartOfAccountNameAlternative.AlternativeAccountHeadID <= 0)
                {
                    reader = COAChartOfAccountNameAlternativeDA.InsertUpdate(tc, oCOAChartOfAccountNameAlternative, nUserID, EnumDBOperation.Insert);
                }
                else
                {
                    reader = COAChartOfAccountNameAlternativeDA.InsertUpdate(tc, oCOAChartOfAccountNameAlternative, nUserID, EnumDBOperation.Update);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
                    oCOAChartOfAccountNameAlternative = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();               
                oCOAChartOfAccountNameAlternative.ErrorMessage = e.Message; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oCOAChartOfAccountNameAlternative;
        }

        public COAChartOfAccountNameAlternative SaveForDocumentLeaf(COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative, int nUserID)
        {


            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCOAChartOfAccountNameAlternative.AlternativeAccountHeadID <= 0)
                {
                    reader = COAChartOfAccountNameAlternativeDA.InsertUpdate(tc, oCOAChartOfAccountNameAlternative, nUserID, EnumDBOperation.Insert);
                }
                else
                {
                    reader = COAChartOfAccountNameAlternativeDA.InsertUpdate(tc, oCOAChartOfAccountNameAlternative, nUserID, EnumDBOperation.Update);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
                    oCOAChartOfAccountNameAlternative = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCOAChartOfAccountNameAlternative.ErrorMessage = e.Message; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oCOAChartOfAccountNameAlternative;
        }
        public COAChartOfAccountNameAlternative Get(int AlternativeAccountHeadID, int nUserId)
        {
            COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = COAChartOfAccountNameAlternativeDA.Get(tc, AlternativeAccountHeadID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCOAChartOfAccountNameAlternative = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get S", e);
                #endregion
            }

            return oCOAChartOfAccountNameAlternative;
        }

        public List<COAChartOfAccountNameAlternative> SearchbyAlternativeName(string Search, int AccountHeadID, int nUserId)
        {
            List<COAChartOfAccountNameAlternative> oCOAChartOfAccountNameAlternative = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = COAChartOfAccountNameAlternativeDA.SearchbyAlternativeName(tc,Search,AccountHeadID,nUserId);
                oCOAChartOfAccountNameAlternative = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Alternative Name", e);
                #endregion
            }

            return oCOAChartOfAccountNameAlternative;
        }

        public List<COAChartOfAccountNameAlternative> Gets(string sSQL, int nUserId)
        {
            List<COAChartOfAccountNameAlternative> oCOAChartOfAccountNameAlternative = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = COAChartOfAccountNameAlternativeDA.Gets(tc, sSQL);
                oCOAChartOfAccountNameAlternative = CreateObjects(reader);
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

            return oCOAChartOfAccountNameAlternative;
        }

        public List<COAChartOfAccountNameAlternative> GetsbyParentID(int nParentID, int nUserId)
        {
            List<COAChartOfAccountNameAlternative> oCOAChartOfAccountNameAlternative = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = COAChartOfAccountNameAlternativeDA.GetsbyParentID(tc, nParentID, nUserId);
                oCOAChartOfAccountNameAlternative = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get User", e);
                #endregion
            }

            return oCOAChartOfAccountNameAlternative;
        }



        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
                oCOAChartOfAccountNameAlternative.AlternativeAccountHeadID = id;
                COAChartOfAccountNameAlternativeDA.Delete(tc, oCOAChartOfAccountNameAlternative, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }
        #endregion
    }
}
