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
    public class ReturnChallanDetailService : MarshalByRefObject, IReturnChallanDetailService
    {
        #region Private functions and declaration
        private ReturnChallanDetail MapObject(NullHandler oReader)
        {
            ReturnChallanDetail oReturnChallanDetail = new ReturnChallanDetail();
            oReturnChallanDetail.ReturnChallanDetailID = oReader.GetInt32("ReturnChallanDetailID");
            oReturnChallanDetail.ReturnChallanID = oReader.GetInt32("ReturnChallanID");
            oReturnChallanDetail.DeliveryChallanDetailID = oReader.GetInt32("DeliveryChallanDetailID");
            oReturnChallanDetail.DeliveryChallanID = oReader.GetInt32("DeliveryChallanID");
            oReturnChallanDetail.LotID = oReader.GetInt32("LotID");
            oReturnChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oReturnChallanDetail.MUnitID = oReader.GetInt32("MUnitID");
            oReturnChallanDetail.Qty = oReader.GetDouble("Qty");
            oReturnChallanDetail.Note = oReader.GetString("Note");
            oReturnChallanDetail.DeliveryChallanNo = oReader.GetString("DeliveryChallanNo");
            oReturnChallanDetail.PINo = oReader.GetString("PINo");
            oReturnChallanDetail.ProductName = oReader.GetString("ProductName");
            oReturnChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oReturnChallanDetail.MUnit = oReader.GetString("MUnit");
            oReturnChallanDetail.LotNo = oReader.GetString("LotNo");
            oReturnChallanDetail.DONo = oReader.GetString("DONo");
            oReturnChallanDetail.DeliveryChallanQty = oReader.GetDouble("DeliveryChallanQty");
            oReturnChallanDetail.YetToReturnQty = oReader.GetDouble("YetToReturnQty");
            oReturnChallanDetail.ReturnChallanNo = oReader.GetString("ReturnChallanNo");
            oReturnChallanDetail.Symbol = oReader.GetString("Symbol");
            oReturnChallanDetail.ReturnDate = oReader.GetDateTime("ReturnDate");
            
            return oReturnChallanDetail;
        }

        private ReturnChallanDetail CreateObject(NullHandler oReader)
        {
            ReturnChallanDetail oReturnChallanDetail = new ReturnChallanDetail();
            oReturnChallanDetail = MapObject(oReader);
            return oReturnChallanDetail;
        }

        private List<ReturnChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<ReturnChallanDetail> oReturnChallanDetail = new List<ReturnChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ReturnChallanDetail oItem = CreateObject(oHandler);
                oReturnChallanDetail.Add(oItem);
            }
            return oReturnChallanDetail;
        }

        #endregion

        #region Interface implementation

        public ReturnChallanDetail Get(int id, Int64 nUserId)
        {
            ReturnChallanDetail oReturnChallanDetail = new ReturnChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ReturnChallanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oReturnChallanDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ReturnChallanDetail", e);
                #endregion
            }
            return oReturnChallanDetail;
        }

        public List<ReturnChallanDetail> Gets(int nDOID, Int64 nUserID)
        {
            List<ReturnChallanDetail> oReturnChallanDetails = new List<ReturnChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ReturnChallanDetailDA.Gets(nDOID, tc);
                oReturnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ReturnChallanDetail oReturnChallanDetail = new ReturnChallanDetail();
                oReturnChallanDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oReturnChallanDetails;
        }

        public List<ReturnChallanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ReturnChallanDetail> oReturnChallanDetails = new List<ReturnChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ReturnChallanDetailDA.Gets(tc, sSQL);
                oReturnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReturnChallanDetail", e);
                #endregion
            }
            return oReturnChallanDetails;
        }

        #endregion
    }

}
