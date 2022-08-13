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

    public class QCService : MarshalByRefObject, IQCService
    {
        #region Private functions and declaration
        private QC MapObject(NullHandler oReader)
        {
            QC oQC = new QC();
            oQC.QCID = oReader.GetInt32("QCID");
            oQC.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oQC.ProductionSheetID = oReader.GetInt32("ProductionSheetID");
            oQC.PassQuantity = oReader.GetDouble("PassQuantity");
            oQC.RejectQuantity = oReader.GetDouble("RejectQuantity");
            oQC.ProductID = oReader.GetInt32("ProductID");
            oQC.QCPerson = oReader.GetInt32("QCPerson");
            oQC.OperationTime = oReader.GetDateTime("OperationTime");
            oQC.SheetNo = oReader.GetString("SheetNo");
            oQC.ProductCode = oReader.GetString("ProductCode");
            oQC.ProductName = oReader.GetString("ProductName");
            oQC.QCPersonName = oReader.GetString("QCPersonName");
            oQC.StoreName = oReader.GetString("StoreName");
            oQC.CartonQty = oReader.GetInt32("CartonQty");
            oQC.PerCartonFGQty = oReader.GetDouble("PerCartonFGQty");
            oQC.LotID = oReader.GetInt32("LotID");
            oQC.IsExist = oReader.GetBoolean("IsExist");
            oQC.LotNo = oReader.GetString("LotNo");
            oQC.MUName = oReader.GetString("MUName");
            return oQC;
        }

        private QC CreateObject(NullHandler oReader)
        {
            QC oQC = new QC();
            oQC = MapObject(oReader);
            return oQC;
        }

        private List<QC> CreateObjects(IDataReader oReader)
        {
            List<QC> oQC = new List<QC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                QC oItem = CreateObject(oHandler);
                oQC.Add(oItem);
            }
            return oQC;
        }

        #endregion

        #region Interface implementation
        public QCService() { }

        public QC Save(QC oQC, Int64 nUserId)
        {
            TransactionContext tc = null;
            List<PETransaction> oPETransactions = new List<PETransaction>();
            oPETransactions = oQC.PETransactions;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oQC.QCID <= 0)
                {
                    reader = QCDA.InsertUpdate(tc, oQC, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = QCDA.InsertUpdate(tc, oQC, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oQC = new QC();
                    oQC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oQC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oQC;
        }

        public QC FGCostProcess(QC oQC, Int64 nUserId)
        {
            TransactionContext tc = null;
            string[] QCID = new string[] {oQC.sParam};
            List<QC> oQCs = new List<QC>();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oQC.QCID <= 0)
                {
                    reader = QCDA.InsertUpdate(tc, oQC, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = QCDA.InsertUpdate(tc, oQC, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oQC = new QC();
                    oQC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oQC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oQC;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                QC oQC = new QC();
                oQC.QCID = id;
                QCDA.Delete(tc, oQC, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                QC oQC = new QC();
                oQC.ErrorMessage = e.Message;
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Delete sucessfully";
        }

        public QC Get(int id, Int64 nUserId)
        {
            QC oAccountHead = new QC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = QCDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get QC", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<QC> Gets(Int64 nUserId)
        {
            List<QC> oQCs = new List<QC>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = QCDA.Gets(tc);
                oQCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QC", e);
                #endregion
            }

            return oQCs;
        }
        public List<QC> Gets(int nPSID, Int64 nUserId)
        {
            List<QC> oQCs = new List<QC>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = QCDA.Gets(tc, nPSID);
                oQCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QC", e);
                #endregion
            }

            return oQCs;
        }
        public List<QC> Gets(string sSQL, Int64 nUserId)
        {
            List<QC> oQC = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = QCDA.Gets(tc, sSQL);
                oQC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QC", e);
                #endregion
            }

            return oQC;
        }
        #endregion
    }
    
    
   
}
