using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class DURequisitionDetailService : MarshalByRefObject, IDURequisitionDetailService
    {
        #region Private functions and declaration
        private DURequisitionDetail MapObject(NullHandler oReader)
        {
            DURequisitionDetail oDURequisitionDetail = new DURequisitionDetail();
            oDURequisitionDetail.DURequisitionDetailID = oReader.GetInt32("DURequisitionDetailID");
            oDURequisitionDetail.DURequisitionID = oReader.GetInt32("DURequisitionID");
            oDURequisitionDetail.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDURequisitionDetail.DyeingOrderNo = oReader.GetString("DyeingOrderNo");
            oDURequisitionDetail.MUnitID = oReader.GetInt32("MUnitID");
            oDURequisitionDetail.LotID = oReader.GetInt32("LotID");
            oDURequisitionDetail.OrderType = oReader.GetInt32("DyeingOrderType");
            oDURequisitionDetail.LotNo = oReader.GetString("LotNo");
            oDURequisitionDetail.DestinationLotID = oReader.GetInt32("DestinationLotID");
            oDURequisitionDetail.ContractorID = oReader.GetInt32("ContractorID");
            oDURequisitionDetail.DestinationLotNo = oReader.GetString("DesignationLotNo");
            oDURequisitionDetail.ProductID = oReader.GetInt32("ProductID");
            oDURequisitionDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oDURequisitionDetail.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oDURequisitionDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oDURequisitionDetail.ProductName = oReader.GetString("ProductName");
            oDURequisitionDetail.LotProductName = oReader.GetString("LotProductName");
            oDURequisitionDetail.MUnit = oReader.GetString("MUnit");
            oDURequisitionDetail.BagNo = oReader.GetDouble("BagNo");
            oDURequisitionDetail.Qty = oReader.GetDouble("Qty");
            oDURequisitionDetail.Qty_Order = oReader.GetDouble("Qty_Order");
            oDURequisitionDetail.LotQty = oReader.GetDouble("LotQty");
            oDURequisitionDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oDURequisitionDetail.Note = oReader.GetString("Note");
            oDURequisitionDetail.BuyerName = oReader.GetString("BuyerName");
            oDURequisitionDetail.WUName = oReader.GetString("WUName");
            oDURequisitionDetail.IsInHouse = oReader.GetBoolean("IsInHouse");
            oDURequisitionDetail.RequisitionNo = oReader.GetString("RequisitionNo");
            oDURequisitionDetail.ReqDate = oReader.GetDateTime("ReqDate");
            oDURequisitionDetail.ReceiveDate = oReader.GetDateTime("ReceiveDate");

            oDURequisitionDetail.RequisitionType = (EnumInOutType)oReader.GetInt32("RequisitionType");
            return oDURequisitionDetail;
        }

        private DURequisitionDetail CreateObject(NullHandler oReader)
        {
            DURequisitionDetail oDURequisitionDetail = new DURequisitionDetail();
            oDURequisitionDetail = MapObject(oReader);
            return oDURequisitionDetail;
        }

        private List<DURequisitionDetail> CreateObjects(IDataReader oReader)
        {
            List<DURequisitionDetail> oDURequisitionDetails = new List<DURequisitionDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DURequisitionDetail oItem = CreateObject(oHandler);
                oDURequisitionDetails.Add(oItem);
            }
            return oDURequisitionDetails;
        }

        #endregion

        #region Interface implementation
        public DURequisitionDetailService() { }


        public DURequisitionDetail Save(DURequisitionDetail oDURequisitionDetail, Int64 nUserId)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                #region DURequisitionDetail
                IDataReader reader;
                if (oDURequisitionDetail.DURequisitionDetailID <= 0)
                {
                    reader = DURequisitionDetailDA.InsertUpdate(tc, oDURequisitionDetail, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DURequisitionDetailDA.InsertUpdate(tc, oDURequisitionDetail, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDURequisitionDetail = new DURequisitionDetail();
                    oDURequisitionDetail = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDURequisitionDetail = new DURequisitionDetail();
                oDURequisitionDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDURequisitionDetail;
        }

        public String Delete(DURequisitionDetail oDURequisitionDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DURequisitionDetailDA.Delete(tc, oDURequisitionDetail, EnumDBOperation.Delete, nUserID,"");
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public DURequisitionDetail Get(int id, Int64 nUserId)
        {
            DURequisitionDetail oDURequisitionDetail = new DURequisitionDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DURequisitionDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDURequisitionDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDURequisitionDetail;
        }

        public List<DURequisitionDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<DURequisitionDetail> oDURequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DURequisitionDetailDA.Gets(sSQL, tc);
                oDURequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DURequisitionDetail", e);
                #endregion
            }
            return oDURequisitionDetail;
        }

        public List<DURequisitionDetail> Gets(Int64 nUserId)
        {
            List<DURequisitionDetail> oDURequisitionDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DURequisitionDetailDA.Gets(tc);
                oDURequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Claim Details", e);
                #endregion
            }

            return oDURequisitionDetails;
        }

        public List<DURequisitionDetail> Gets(int nDURequisitionID, Int64 nUserId)
        {
            List<DURequisitionDetail> oDURequisitionDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DURequisitionDetailDA.Gets(nDURequisitionID, tc);
                oDURequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Import Invoice Products", e);
                #endregion
            }
            return oDURequisitionDetails;
        }

        #endregion
    }
}