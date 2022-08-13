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
    public class DiagramIdentificationService : MarshalByRefObject, IDiagramIdentificationService
    {
        #region Private functions and declaration
        private DiagramIdentification MapObject(NullHandler oReader)
        {


            DiagramIdentification oDiagramIdentification = new DiagramIdentification();
            oDiagramIdentification.DiagramIdentificationID = oReader.GetInt32("DiagramIdentificationID");
            oDiagramIdentification.MesurementPoint = oReader.GetString("MesurementPoint");
            oDiagramIdentification.PointName = oReader.GetString("PointName");
            oDiagramIdentification.Note = oReader.GetString("Note");
            
            
            return oDiagramIdentification;
        }

        private DiagramIdentification CreateObject(NullHandler oReader)
        {
            DiagramIdentification oDiagramIdentification = new DiagramIdentification();
            oDiagramIdentification = MapObject(oReader);
            return oDiagramIdentification;
        }

        private List<DiagramIdentification> CreateObjects(IDataReader oReader)
        {
            List<DiagramIdentification> oDiagramIdentifications = new List<DiagramIdentification>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DiagramIdentification oItem = CreateObject(oHandler);
                oDiagramIdentifications.Add(oItem);
            }
            return oDiagramIdentifications;
        }

        #endregion

        #region Interface implementation
        public DiagramIdentificationService() { }

        public DiagramIdentification Save(DiagramIdentification oDiagramIdentification, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDiagramIdentification.DiagramIdentificationID <= 0)
                {
                    reader = DiagramIdentificationDA.InsertUpdate(tc, oDiagramIdentification, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DiagramIdentificationDA.InsertUpdate(tc, oDiagramIdentification, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDiagramIdentification = new DiagramIdentification();
                    oDiagramIdentification = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDiagramIdentification = new DiagramIdentification();
                oDiagramIdentification.ErrorMessage = e.Message;
                #endregion
            }
            return oDiagramIdentification;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DiagramIdentification oDiagramIdentification = new DiagramIdentification();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.DiagramIdentification, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "DiagramIdentification", id);
                oDiagramIdentification.DiagramIdentificationID = id;
                DiagramIdentificationDA.Delete(tc, oDiagramIdentification, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion
                return e.Message.Split('!')[0];
            }
            return "Data delete successfully";
        }

        public DiagramIdentification Get(int id, Int64 nUserId)
        {
            DiagramIdentification oDiagramIdentification = new DiagramIdentification();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DiagramIdentificationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDiagramIdentification = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDiagramIdentification = new DiagramIdentification();
                oDiagramIdentification.ErrorMessage = e.Message;
                #endregion
            }

            return oDiagramIdentification;
        }

        public List<DiagramIdentification> Gets(Int64 nUserID)
        {
            List<DiagramIdentification> oDiagramIdentifications = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DiagramIdentificationDA.Gets(tc);
                oDiagramIdentifications = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                DiagramIdentification oDiagramIdentification = new DiagramIdentification();
                oDiagramIdentification.ErrorMessage = e.Message;
                oDiagramIdentifications.Add(oDiagramIdentification);
                #endregion
            }

            return oDiagramIdentifications;
        }

        public List<DiagramIdentification> Gets_print(string sSQL, Int64 nUserId)
        {
            List<DiagramIdentification> oDiagramIdentification = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DiagramIdentificationDA.Gets_print(tc, sSQL);
                oDiagramIdentification = CreateObjects(reader);
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

            return oDiagramIdentification;
        }

        public List<DiagramIdentification> GetsByName(string sName, Int64 nUserId)
        {
            List<DiagramIdentification> oDiagramIdentifications = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DiagramIdentificationDA.GetsByName(tc, sName);
                oDiagramIdentifications = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DiagramIdentifications", e);
                #endregion
            }

            return oDiagramIdentifications;
        }

        #endregion
    }
}
