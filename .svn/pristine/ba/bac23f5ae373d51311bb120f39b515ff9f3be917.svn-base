using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class DeliveryChallanDetailService : MarshalByRefObject, IDeliveryChallanDetailService
    {
        #region Private functions and declaration

        private DeliveryChallanDetail MapObject(NullHandler oReader)
        {
            DeliveryChallanDetail oDeliveryChallanDetail = new DeliveryChallanDetail();
            oDeliveryChallanDetail.DeliveryChallanDetailID = oReader.GetInt32("DeliveryChallanDetailID");
            oDeliveryChallanDetail.DeliveryChallanID = oReader.GetInt32("DeliveryChallanID");
            oDeliveryChallanDetail.DODetailID = oReader.GetInt32("DODetailID");
            oDeliveryChallanDetail.PTUUnit2DistributionID = oReader.GetInt32("PTUUnit2DistributionID");
            oDeliveryChallanDetail.LotID = oReader.GetInt32("LotID");
            oDeliveryChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oDeliveryChallanDetail.MUnitID = oReader.GetInt32("MUnitID");
            oDeliveryChallanDetail.Qty = oReader.GetDouble("Qty");
            oDeliveryChallanDetail.BagQty = oReader.GetInt32("BagQty");
            oDeliveryChallanDetail.Note = oReader.GetString("Note");
            oDeliveryChallanDetail.ChallanNo = oReader.GetString("ChallanNo");
            oDeliveryChallanDetail.PINo = oReader.GetString("PINo");
            oDeliveryChallanDetail.DONo = oReader.GetString("DONo");
            oDeliveryChallanDetail.ProductName = oReader.GetString("ProductName");
            oDeliveryChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oDeliveryChallanDetail.MUnit = oReader.GetString("MUnit");
            oDeliveryChallanDetail.LotNo = oReader.GetString("LotNo");
            oDeliveryChallanDetail.ColorName = oReader.GetString("ColorName");
            oDeliveryChallanDetail.Measurement = oReader.GetString("Measurement");
            oDeliveryChallanDetail.SizeName = oReader.GetString("SizeName");
            oDeliveryChallanDetail.ProductDescription = oReader.GetString("ProductDescription");
            oDeliveryChallanDetail.ModelReferenceName = oReader.GetString("ModelReferenceName");
            oDeliveryChallanDetail.StyleNo = oReader.GetString("StyleNo");
            oDeliveryChallanDetail.YetToReturnQty = oReader.GetDouble("YetToReturnQty");
            oDeliveryChallanDetail.ReportingQty = oReader.GetDouble("ReportingQty");
            oDeliveryChallanDetail.ReportingUnit = oReader.GetString("ReportingUnit");
            oDeliveryChallanDetail.ReferenceCaption = oReader.GetString("ReferenceCaption");
            oDeliveryChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oDeliveryChallanDetail.QtyPerCarton = oReader.GetInt32("QtyPerCarton");
            oDeliveryChallanDetail.ChallanDate = oReader.GetDateTime("ChallanDate");
                        
            return oDeliveryChallanDetail;
        }

        private DeliveryChallanDetail CreateObject(NullHandler oReader)
        {
            DeliveryChallanDetail oDeliveryChallanDetail = new DeliveryChallanDetail();
            oDeliveryChallanDetail = MapObject(oReader);
            return oDeliveryChallanDetail;
        }

        private List<DeliveryChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<DeliveryChallanDetail> oDeliveryChallanDetail = new List<DeliveryChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DeliveryChallanDetail oItem = CreateObject(oHandler);
                oDeliveryChallanDetail.Add(oItem);
            }
            return oDeliveryChallanDetail;
        }

        #endregion

        #region Interface implementation

        public DeliveryChallanDetail Get(int id, Int64 nUserId)
        {
            DeliveryChallanDetail oDeliveryChallanDetail = new DeliveryChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DeliveryChallanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliveryChallanDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DeliveryChallanDetail", e);
                #endregion
            }
            return oDeliveryChallanDetail;
        }

        public List<DeliveryChallanDetail> Gets(int nDOID, Int64 nUserID)
        {
            List<DeliveryChallanDetail> oDeliveryChallanDetails = new List<DeliveryChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DeliveryChallanDetailDA.Gets(nDOID, tc);
                oDeliveryChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                DeliveryChallanDetail oDeliveryChallanDetail = new DeliveryChallanDetail();
                oDeliveryChallanDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oDeliveryChallanDetails;
        }

        public List<DeliveryChallanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<DeliveryChallanDetail> oDeliveryChallanDetails = new List<DeliveryChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DeliveryChallanDetailDA.Gets(tc, sSQL);
                oDeliveryChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DeliveryChallanDetail", e);
                #endregion
            }
            return oDeliveryChallanDetails;
        }

        #endregion
    }

}
