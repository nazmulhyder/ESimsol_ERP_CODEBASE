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
    public class CircularService : MarshalByRefObject, ICircularService
    {
        #region Private functions and declaration
        private Circular MapObject(NullHandler oReader)
        {
            Circular oCircular = new Circular();
            oCircular.CircularID = oReader.GetInt32("CircularID");
            oCircular.DepartmentID = oReader.GetInt32("DepartmentID");
            oCircular.DesignationID = oReader.GetInt32("DesignationID");
            oCircular.NoOfPosition = oReader.GetInt32("NoOfPosition");
            oCircular.Description = oReader.GetString("Description");
            oCircular.StartDate = oReader.GetDateTime("StartDate");
            oCircular.EndDate = oReader.GetDateTime("EndDate");
            oCircular.ApproveBy = oReader.GetInt32("ApproveBy");
            oCircular.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oCircular.IsActive = oReader.GetBoolean("IsActive");

            //derive
            oCircular.DepartmentName = oReader.GetString("DepartmentName");
            oCircular.DesignationName = oReader.GetString("DesignationName");
            oCircular.ApproveByName = oReader.GetString("ApproveByName");

            return oCircular;

        }

        private Circular CreateObject(NullHandler oReader)
        {
            Circular oCircular = MapObject(oReader);
            return oCircular;
        }

        private List<Circular> CreateObjects(IDataReader oReader)
        {
            List<Circular> oCircular = new List<Circular>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Circular oItem = CreateObject(oHandler);
                oCircular.Add(oItem);
            }
            return oCircular;
        }

        #endregion

        #region Interface implementation
        public CircularService() { }

        public Circular IUD(Circular oCircular, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CircularDA.IUD(tc, oCircular, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oCircular = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCircular.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCircular.CircularID = 0;
                #endregion
            }
            return oCircular;
        }


        public Circular Get(int nCircularID, Int64 nUserId)
        {
            Circular oCircular = new Circular();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CircularDA.Get(nCircularID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCircular = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get Circular", e);
                oCircular.ErrorMessage = e.Message;
                #endregion
            }

            return oCircular;
        }

        public Circular Get(string sSql, Int64 nUserId)
        {
            Circular oCircular = new Circular();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CircularDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCircular = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get Circular", e);
                oCircular.ErrorMessage = e.Message;
                #endregion
            }

            return oCircular;
        }

        public List<Circular> Gets(Int64 nUserID)
        {
            List<Circular> oCircular = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CircularDA.Gets(tc);
                oCircular = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_Circular", e);
                #endregion
            }
            return oCircular;
        }

        public List<Circular> Gets(string sSQL, Int64 nUserID)
        {
            List<Circular> oCircular = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CircularDA.Gets(sSQL, tc);
                oCircular = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_Circular", e);
                #endregion
            }
            return oCircular;
        }

        public List<Circular> GetNewCirculars(string sSQL, Int64 nUserID)
        {
            List<Circular> oCircular = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CircularDA.GetNewCirculars(sSQL, tc);
                oCircular = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_Circular", e);
                #endregion
            }
            return oCircular;
        }

        #endregion

        #region Activity
        public Circular Activity(int nCircularID,  Int64 nUserId)
        {
            Circular oCircular = new Circular();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CircularDA.Activity(nCircularID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCircular = CreateObject(oReader);
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
                oCircular.ErrorMessage = e.Message;
                #endregion
            }

            return oCircular;
        }


        #endregion
    }
}
