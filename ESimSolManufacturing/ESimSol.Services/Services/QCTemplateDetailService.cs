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

    public class QCTemplateDetailService : MarshalByRefObject, IQCTemplateDetailService
    {
        #region Private functions and declaration
        public static QCTemplateDetail MapObject(NullHandler oReader)
        {
            QCTemplateDetail oQCTemplateDetail = new QCTemplateDetail();
            oQCTemplateDetail.QCTemplateDetailID = oReader.GetInt32("QCTemplateDetailID");
            oQCTemplateDetail.QCTemplateID = oReader.GetInt32("QCTemplateID");
            oQCTemplateDetail.QCStepID = oReader.GetInt32("QCStepID");
            oQCTemplateDetail.QCStepName = oReader.GetString("QCStepName");
            oQCTemplateDetail.Sequence = oReader.GetInt32("Sequence");
            oQCTemplateDetail.QCStepParentID = oReader.GetInt32("QCStepParentID");
            oQCTemplateDetail.QCStepSequence = oReader.GetInt32("QCStepSequence");
            oQCTemplateDetail.TemplateName = oReader.GetString("TemplateName");
            return oQCTemplateDetail;
        }

        public static QCTemplateDetail CreateObject(NullHandler oReader)
        {
            QCTemplateDetail oQCTemplateDetail = new QCTemplateDetail();
            oQCTemplateDetail = MapObject(oReader);
            return oQCTemplateDetail;
        }

        public static List<QCTemplateDetail> CreateObjects(IDataReader oReader)
        {
            List<QCTemplateDetail> oQCTemplateDetail = new List<QCTemplateDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                QCTemplateDetail oItem = CreateObject(oHandler);
                oQCTemplateDetail.Add(oItem);
            }
            return oQCTemplateDetail;
        }

        #endregion

        #region Interface implementation
        public QCTemplateDetailService() { }

 
        public QCTemplateDetail Get(int QCTemplateDetailID, Int64 nUserId)
        {
            QCTemplateDetail oAccountHead = new QCTemplateDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader readerDetail = QCTemplateDetailDA.Get(tc, QCTemplateDetailID);
                NullHandler oReaderDetail = new NullHandler(readerDetail);
                if (readerDetail.Read())
                {
                    oAccountHead = CreateObject(oReaderDetail);
                }
                readerDetail.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QCTemplateDetail", e);
                #endregion
            }

            return oAccountHead;
        }




        public List<QCTemplateDetail> Gets(int QCTemplateID, Int64 nUserID)
        {
            List<QCTemplateDetail> oQCTemplateDetail = new List<QCTemplateDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = QCTemplateDetailDA.Gets(QCTemplateID, tc);
                oQCTemplateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QCTemplateDetail", e);
                #endregion
            }

            return oQCTemplateDetail;
        }

        public List<QCTemplateDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<QCTemplateDetail> oQCTemplateDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = QCTemplateDetailDA.Gets(tc, sSQL);
                oQCTemplateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QCTemplateDetail", e);
                #endregion
            }

            return oQCTemplateDetail;
        }

        public List<QCTemplateDetail> Gets(Int64 nUserID)
        {
            List<QCTemplateDetail> oQCTemplateDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = QCTemplateDetailDA.Gets(tc);
                oQCTemplateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QCTemplateDetail", e);
                #endregion
            }

            return oQCTemplateDetail;
        }

        #endregion
    }
    

}
