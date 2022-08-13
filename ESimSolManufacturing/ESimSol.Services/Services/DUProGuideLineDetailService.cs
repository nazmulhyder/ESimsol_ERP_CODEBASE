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
    public class DUProGuideLineDetailService : MarshalByRefObject, IDUProGuideLineDetailService
    {
        #region Private functions and declaration
        private DUProGuideLineDetail MapObject(NullHandler oReader)
        {
            DUProGuideLineDetail oDUProGuideLineDetail = new DUProGuideLineDetail();
            oDUProGuideLineDetail.DUProGuideLineDetailID = oReader.GetInt32("DUProGuideLineDetailID");
            oDUProGuideLineDetail.DUProGuideLineID = oReader.GetInt32("DUProGuideLineID");
            oDUProGuideLineDetail.MUnitID = oReader.GetInt32("MUnitID");
            oDUProGuideLineDetail.LotID = oReader.GetInt32("LotID");
            oDUProGuideLineDetail.LotNo = oReader.GetString("LotNo");
            oDUProGuideLineDetail.Brand = oReader.GetString("Brand");
            oDUProGuideLineDetail.LotParentID = oReader.GetInt32("LotParentID");
            oDUProGuideLineDetail.ProductID = oReader.GetInt32("ProductID");
            oDUProGuideLineDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oDUProGuideLineDetail.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUProGuideLineDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oDUProGuideLineDetail.ProductName = oReader.GetString("ProductName");
            oDUProGuideLineDetail.LotProductName = oReader.GetString("LotProductName");
            oDUProGuideLineDetail.ChallanNo = oReader.GetString("ChallanNo");


            oDUProGuideLineDetail.ReceiveDate = oReader.GetDateTime("ReceiveDate");

            oDUProGuideLineDetail.MUnit = oReader.GetString("MUnit");
            oDUProGuideLineDetail.Qty = oReader.GetDouble("Qty");
            oDUProGuideLineDetail.Qty_Order = oReader.GetDouble("Qty_Order");

            oDUProGuideLineDetail.Qty_LotParent = oReader.GetDouble("Qty_LotParent");
            oDUProGuideLineDetail.LotQty = oReader.GetDouble("LotQty");
            oDUProGuideLineDetail.BagNo = oReader.GetDouble("BagNo");
            oDUProGuideLineDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oDUProGuideLineDetail.Note = oReader.GetString("Note");
            oDUProGuideLineDetail.SLNo = oReader.GetString("SLNo");

            return oDUProGuideLineDetail;
        }

        private DUProGuideLineDetail CreateObject(NullHandler oReader)
        {
            DUProGuideLineDetail oDUProGuideLineDetail = new DUProGuideLineDetail();
            oDUProGuideLineDetail = MapObject(oReader);
            return oDUProGuideLineDetail;
        }

        private List<DUProGuideLineDetail> CreateObjects(IDataReader oReader)
        {
            List<DUProGuideLineDetail> oDUProGuideLineDetails = new List<DUProGuideLineDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUProGuideLineDetail oItem = CreateObject(oHandler);
                oDUProGuideLineDetails.Add(oItem);
            }
            return oDUProGuideLineDetails;
        }

        #endregion

        #region Interface implementation
        public DUProGuideLineDetailService() { }


        public DUProGuideLineDetail Save(DUProGuideLineDetail oDUProGuideLineDetail, Int64 nUserId)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                #region DUProGuideLineDetail
                IDataReader reader;
                if (oDUProGuideLineDetail.DUProGuideLineDetailID <= 0)
                {
                    reader = DUProGuideLineDetailDA.InsertUpdate(tc, oDUProGuideLineDetail, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DUProGuideLineDetailDA.InsertUpdate(tc, oDUProGuideLineDetail, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUProGuideLineDetail = new DUProGuideLineDetail();
                    oDUProGuideLineDetail = CreateObject(oReader);
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
                oDUProGuideLineDetail = new DUProGuideLineDetail();
                oDUProGuideLineDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUProGuideLineDetail;
        }

        public String Delete(DUProGuideLineDetail oDUProGuideLineDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DUProGuideLineDetailDA.Delete(tc, oDUProGuideLineDetail, EnumDBOperation.Delete, nUserID, "");
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
        public DUProGuideLineDetail Get(int id, Int64 nUserId)
        {
            DUProGuideLineDetail oDUProGuideLineDetail = new DUProGuideLineDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUProGuideLineDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUProGuideLineDetail = CreateObject(oReader);
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

            return oDUProGuideLineDetail;
        }

        public List<DUProGuideLineDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<DUProGuideLineDetail> oDUProGuideLineDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUProGuideLineDetailDA.Gets(sSQL, tc);
                oDUProGuideLineDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUProGuideLineDetail", e);
                #endregion
            }
            return oDUProGuideLineDetail;
        }

        public List<DUProGuideLineDetail> Gets(Int64 nUserId)
        {
            List<DUProGuideLineDetail> oDUProGuideLineDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUProGuideLineDetailDA.Gets(tc);
                oDUProGuideLineDetails = CreateObjects(reader);
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

            return oDUProGuideLineDetails;
        }

        public List<DUProGuideLineDetail> Gets(int nImportInvoiceID, Int64 nUserId)
        {
            List<DUProGuideLineDetail> oDUProGuideLineDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUProGuideLineDetailDA.Gets(nImportInvoiceID, tc);
                oDUProGuideLineDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Production Details", e);
                #endregion
            }
            return oDUProGuideLineDetails;
        }

        #endregion
    }
}