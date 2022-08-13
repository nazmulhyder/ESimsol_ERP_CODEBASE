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

    public class InspectionCertificateDetailService : MarshalByRefObject, IInspectionCertificateDetailService
    {
        #region Private functions and declaration
        private InspectionCertificateDetail MapObject(NullHandler oReader)
        {
            InspectionCertificateDetail oInspectionCertificateDetail = new InspectionCertificateDetail();
            oInspectionCertificateDetail.InspectionCertificateDetailID = oReader.GetInt32("InspectionCertificateDetailID");
            oInspectionCertificateDetail.InspectionCertificateID = oReader.GetInt32("InspectionCertificateID");
            oInspectionCertificateDetail.CommercialInvoiceDetailID = oReader.GetInt32("CommercialInvoiceDetailID");
            oInspectionCertificateDetail.OrderQty = oReader.GetDouble("OrderQty");
            oInspectionCertificateDetail.ShipedQty = oReader.GetDouble("ShipedQty");
            oInspectionCertificateDetail.CartonQty = oReader.GetDouble("CartonQty");
            oInspectionCertificateDetail.StyleNo = oReader.GetString("StyleNo");
            oInspectionCertificateDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oInspectionCertificateDetail.OrderNo = oReader.GetString("OrderNo");

            return oInspectionCertificateDetail;
        }

        private InspectionCertificateDetail CreateObject(NullHandler oReader)
        {
            InspectionCertificateDetail oInspectionCertificateDetail = new InspectionCertificateDetail();
            oInspectionCertificateDetail = MapObject(oReader);
            return oInspectionCertificateDetail;
        }

        private List<InspectionCertificateDetail> CreateObjects(IDataReader oReader)
        {
            List<InspectionCertificateDetail> oInspectionCertificateDetail = new List<InspectionCertificateDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                InspectionCertificateDetail oItem = CreateObject(oHandler);
                oInspectionCertificateDetail.Add(oItem);
            }
            return oInspectionCertificateDetail;
        }

        #endregion

        #region Interface implementation
        public InspectionCertificateDetailService() { }

        public InspectionCertificateDetail Save(InspectionCertificateDetail oInspectionCertificateDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<InspectionCertificateDetail> _oInspectionCertificateDetails = new List<InspectionCertificateDetail>();
            oInspectionCertificateDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = InspectionCertificateDetailDA.InsertUpdate(tc, oInspectionCertificateDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oInspectionCertificateDetail = new InspectionCertificateDetail();
                    oInspectionCertificateDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oInspectionCertificateDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oInspectionCertificateDetail;
        }

        public InspectionCertificateDetail Get(int InspectionCertificateDetailID, Int64 nUserId)
        {
            InspectionCertificateDetail oAccountHead = new InspectionCertificateDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = InspectionCertificateDetailDA.Get(tc, InspectionCertificateDetailID);
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
                throw new ServiceException("Failed to Get InspectionCertificateDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<InspectionCertificateDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<InspectionCertificateDetail> oInspectionCertificateDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InspectionCertificateDetailDA.Gets(LabDipOrderID, tc);
                oInspectionCertificateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InspectionCertificateDetail", e);
                #endregion
            }

            return oInspectionCertificateDetail;
        }

        public List<InspectionCertificateDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<InspectionCertificateDetail> oInspectionCertificateDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InspectionCertificateDetailDA.Gets(tc, sSQL);
                oInspectionCertificateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InspectionCertificateDetail", e);
                #endregion
            }

            return oInspectionCertificateDetail;
        }
        #endregion
    }
    
  
}
