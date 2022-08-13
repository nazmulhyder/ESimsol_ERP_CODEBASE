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
    public class DesignationService : MarshalByRefObject, IDesignationService
    {
        #region Private functions and declaration
        private Designation MapObject(NullHandler oReader)
        {
            Designation oDesignation = new Designation();
            oDesignation.DesignationID = oReader.GetInt32("DesignationID");
            oDesignation.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            oDesignation.Code = oReader.GetString("Code");
            oDesignation.Name = oReader.GetString("Name");
            oDesignation.NameInBangla = oReader.GetString("NameInBangla");
            oDesignation.HRResponsibilityID = oReader.GetInt32("HRResponsibilityID");
            oDesignation.Responsibility = oReader.GetString("Responsibility");
            oDesignation.ResponsibilityInBangla = oReader.GetString("ResponsibilityInBangla");
            oDesignation.Description = oReader.GetString("Description");
            oDesignation.ParentID = oReader.GetInt32("ParentID");
            oDesignation.Sequence = oReader.GetInt32("Sequence");
            oDesignation.RequiredPerson = oReader.GetInt32("RequiredPerson");
            oDesignation.IsActive = oReader.GetBoolean("IsActive");
            return oDesignation;
        }

        private Designation CreateObject(NullHandler oReader)
        {
            Designation oDesignation = new Designation();
            oDesignation = MapObject(oReader);
            return oDesignation;
        }

        private List<Designation> CreateObjects(IDataReader oReader)
        {
            List<Designation> oDesignation = new List<Designation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Designation oItem = CreateObject(oHandler);
                oDesignation.Add(oItem);
            }
            return oDesignation;
        }

        #endregion

        #region Interface implementation
        public DesignationService() { }

        public Designation Save(Designation oDesignation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDesignation.DesignationID <= 0)
                {
                    reader = DesignationDA.InsertUpdate(tc, oDesignation, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DesignationDA.InsertUpdate(tc, oDesignation, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDesignation = new Designation();
                    oDesignation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDesignation = new Designation();
                oDesignation.ErrorMessage = e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Designation. Because of " + e.Message, e);
                #endregion
            }
            return oDesignation;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Designation oDesignation = new Designation();
                oDesignation.DesignationID = id;
                DesignationDA.Delete(tc, oDesignation, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Designation. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public Designation Get(int id, Int64 nUserId)
        {
            Designation oAccountHead = new Designation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DesignationDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Designation", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<Designation> Gets(Int64 nUserId)
        {
            List<Designation> oDesignation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DesignationDA.Gets(tc);
                oDesignation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Designation", e);
                #endregion
            }

            return oDesignation;
        }

        public List<Designation> Gets(string sSQL, Int64 nUserId)
        {
            List<Designation> oDesignation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DesignationDA.Gets(tc, sSQL);
                oDesignation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Designation", e);
                #endregion
            }

            return oDesignation;
        }

        public List<Designation> GetsXL(string sSQL, Int64 nUserId)
        {
            List<Designation> oDesignations = new List<Designation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DesignationDA.GetsXL(tc, sSQL);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    Designation oItem = new Designation();
                    oItem.Code = oreader.GetString("Code");
                    oItem.Name = oreader.GetString("Name");
                    oItem.PCode = oreader.GetString("PCode");
                    oDesignations.Add(oItem);
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

            return oDesignations;
        }

        #endregion
    }
}
