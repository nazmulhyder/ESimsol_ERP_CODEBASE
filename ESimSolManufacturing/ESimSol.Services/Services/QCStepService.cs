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


    public class QCStepService : MarshalByRefObject, IQCStepService
    {
        #region Private functions and declaration
        private QCStep MapObject(NullHandler oReader)
        {
            QCStep oQCStep = new QCStep();
            oQCStep.QCStepID = oReader.GetInt32("QCStepID");
            oQCStep.ParentID = oReader.GetInt32("ParentID");
            oQCStep.QCStepName = oReader.GetString("QCStepName");
            oQCStep.QCDataType = (EnumQCDataType)oReader.GetInt32("QCDataType");
            oQCStep.Sequence = oReader.GetInt32("Sequence");
            oQCStep.ProductionStepID = oReader.GetInt32("ProductionStepID");
            oQCStep.ProductionStepName = oReader.GetString("ProductionStepName");
            return oQCStep;
        }

        private QCStep CreateObject(NullHandler oReader)
        {
            QCStep oQCStep = new QCStep();
            oQCStep = MapObject(oReader);
            return oQCStep;
        }

        private List<QCStep> CreateObjects(IDataReader oReader)
        {
            List<QCStep> oQCStep = new List<QCStep>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                QCStep oItem = CreateObject(oHandler);
                oQCStep.Add(oItem);
            }
            return oQCStep;
        }

        #endregion

        #region Interface implementation
        public QCStepService() { }

        public QCStep Save(QCStep oQCStep, Int64 nUserID)
        {
            List<QCStep> oChildQCSteps = new List<QCStep>();
            oChildQCSteps = oQCStep.ChildQCSteps;
            string sChildQCStepIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Child QC Step Part
                if (oChildQCSteps != null)
                {
                    foreach (QCStep oItem in oChildQCSteps)
                    {
                        IDataReader ChildQCStepReader;
                        oItem.ParentID = oQCStep.QCStepID;
                        if (oItem.QCStepID <= 0)
                        {
                            ChildQCStepReader = QCStepDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            ChildQCStepReader = QCStepDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(ChildQCStepReader);
                        if (ChildQCStepReader.Read())
                        {
                            sChildQCStepIDs = sChildQCStepIDs + oReaderDetail.GetString("QCStepID") + ",";
                        }
                        ChildQCStepReader.Close();
                    }
                    if (sChildQCStepIDs.Length > 0)
                    {
                        sChildQCStepIDs = sChildQCStepIDs.Remove(sChildQCStepIDs.Length - 1, 1);
                    }
                    QCStep oTempQCStep = new QCStep();
                    oTempQCStep.ParentID = oQCStep.QCStepID;
                    QCStepDA.Delete(tc, oTempQCStep, EnumDBOperation.Delete, nUserID, sChildQCStepIDs,false);
                }

                #endregion
               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oQCStep = new QCStep();
                oQCStep.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oQCStep;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                QCStep oQCStep = new QCStep();
                oQCStep.ParentID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.QCStep, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "QCStep", id);
                QCStepDA.Delete(tc, oQCStep, EnumDBOperation.Delete, nUserId,"",true);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete QCStep. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public QCStep Get(int id, Int64 nUserId)
        {
            QCStep oAccountHead = new QCStep();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = QCStepDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get QCStep", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<QCStep> Gets(Int64 nUserID)
        {
            List<QCStep> oQCStep = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = QCStepDA.Gets(tc);
                oQCStep = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QCStep", e);
                #endregion
            }

            return oQCStep;
        }

        public List<QCStep> Gets(string sSQL, Int64 nUserID)
        {
            List<QCStep> oQCStep = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = QCStepDA.Gets(tc, sSQL);
                oQCStep = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QCStep", e);
                #endregion
            }

            return oQCStep;
        }

        #endregion
    }   

}
