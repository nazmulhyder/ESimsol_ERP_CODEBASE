using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class DUDeliveryOrderDCService : MarshalByRefObject, IDUDeliveryOrderDCService
    {
        #region Private functions and declaration
        private DUDeliveryOrderDC MapObject(NullHandler oReader)
        {
            DUDeliveryOrderDC oDUDeliveryOrderDC = new DUDeliveryOrderDC();
            oDUDeliveryOrderDC.DUDeliveryOrderID = oReader.GetInt32("DUDeliveryOrderID");
            oDUDeliveryOrderDC.DONo = oReader.GetString("DONo");
            oDUDeliveryOrderDC.DODate = oReader.GetDateTime("DODate");
            oDUDeliveryOrderDC.ContractorID = oReader.GetInt32("ContractorID");
            //oDUDeliveryOrderDC.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oDUDeliveryOrderDC.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oDUDeliveryOrderDC.DeliveryPoint = oReader.GetString("DeliveryPoint");
            oDUDeliveryOrderDC.OrderType = oReader.GetInt32("OrderType");
            oDUDeliveryOrderDC.OrderID = oReader.GetInt32("OrderID");
            oDUDeliveryOrderDC.ExportPIID = oReader.GetInt32("ExportPIID");
            oDUDeliveryOrderDC.ApproveBy = oReader.GetInt32("ApproveBy");
            oDUDeliveryOrderDC.Note = oReader.GetString("Note");
            //oDUDeliveryOrderDC.ApproveDate = oReader.GetDateTime("ApproveDate");
            ////derive
            //oDUDeliveryOrderDC.ContractorName = oReader.GetString("ContractorName");
            oDUDeliveryOrderDC.DeliveryToName = oReader.GetString("DeliveryToName");
            //oDUDeliveryOrderDC.PreaperByName = oReader.GetString("PreaperByName");
          
            oDUDeliveryOrderDC.OrderNo = oReader.GetString("OrderNo");
            oDUDeliveryOrderDC.Qty = oReader.GetDouble("Qty");
            oDUDeliveryOrderDC.Qty_DC = oReader.GetDouble("Qty_DC");
            oDUDeliveryOrderDC.DOStatus = oReader.GetInt32("DOStatus");
            return oDUDeliveryOrderDC;

        }


        private DUDeliveryOrderDC CreateObject(NullHandler oReader)
        {
            DUDeliveryOrderDC oDUDeliveryOrderDC = MapObject(oReader);
            return oDUDeliveryOrderDC;
        }

        private List<DUDeliveryOrderDC> CreateObjects(IDataReader oReader)
        {
            List<DUDeliveryOrderDC> oDUDeliveryOrderDCs = new List<DUDeliveryOrderDC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryOrderDC oItem = CreateObject(oHandler);
                oDUDeliveryOrderDCs.Add(oItem);
            }
            return oDUDeliveryOrderDCs;
        }

        #endregion

        #region Interface implementation
        public DUDeliveryOrderDCService() { }

      
        public DUDeliveryOrderDC Get(int nDOID, Int64 nUserId)
        {
            DUDeliveryOrderDC oDUDeliveryOrderDC = new DUDeliveryOrderDC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDeliveryOrderDCDA.Get(nDOID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryOrderDC = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUDeliveryOrderDC", e);
                oDUDeliveryOrderDC.ErrorMessage = e.Message;
                #endregion
            }

            return oDUDeliveryOrderDC;
        }
     
        public List<DUDeliveryOrderDC> GetsBy(string sContractorID, Int64 nUserID)
        {
            List<DUDeliveryOrderDC> oDUDeliveryOrderDC = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDCDA.GetsBy(tc, sContractorID);
                oDUDeliveryOrderDC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrderDC", e);
                #endregion
            }
            return oDUDeliveryOrderDC;
        }
        public List<DUDeliveryOrderDC> GetsByPI(int nExportPIID, Int64 nUserID)
        {
            List<DUDeliveryOrderDC> oDUDeliveryOrderDC = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDCDA.GetsByPI(tc, nExportPIID);
                oDUDeliveryOrderDC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrderDC", e);
                #endregion
            }
            return oDUDeliveryOrderDC;
        }
        public List<DUDeliveryOrderDC> GetsByNo(string sOrderNo,Int64 nUserID)
        {
            List<DUDeliveryOrderDC> oDUDeliveryOrderDC = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDCDA.GetsByNo(tc, sOrderNo);
                oDUDeliveryOrderDC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrderDC", e);
                #endregion
            }
            return oDUDeliveryOrderDC;
        }
   
        public List<DUDeliveryOrderDC> Gets(string sSQL, Int64 nUserID)
        {
            List<DUDeliveryOrderDC> oDUDeliveryOrderDC = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryOrderDCDA.Gets(sSQL, tc);
                oDUDeliveryOrderDC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryOrderDC", e);
                #endregion
            }
            return oDUDeliveryOrderDC;
        }


        
        #endregion
       
    }
}
