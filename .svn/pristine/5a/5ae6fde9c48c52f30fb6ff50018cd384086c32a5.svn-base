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
    public class KnittingOrderDetailService : MarshalByRefObject, IKnittingOrderDetailService
    {
        #region Private functions and declaration

        private KnittingOrderDetail MapObject(NullHandler oReader)
        {
            KnittingOrderDetail oKnittingOrderDetail = new KnittingOrderDetail();
            oKnittingOrderDetail.KnittingOrderDetailID = oReader.GetInt32("KnittingOrderDetailID");
            oKnittingOrderDetail.KnittingOrderID = oReader.GetInt32("KnittingOrderID");
            oKnittingOrderDetail.KnitDyeingProgramDetailID = oReader.GetInt32("KnitDyeingProgramDetailID");
            oKnittingOrderDetail.StyleID = oReader.GetInt32("StyleID");
            oKnittingOrderDetail.OrderQty = oReader.GetInt32("OrderQty");
            oKnittingOrderDetail.OrderUnitID = oReader.GetInt32("OrderUnitID");
            oKnittingOrderDetail.PAM = oReader.GetInt32("PAM");
            oKnittingOrderDetail.FabricID = oReader.GetInt32("FabricID");
            oKnittingOrderDetail.GSM = oReader.GetString("GSM");
            oKnittingOrderDetail.MICDia = oReader.GetString("MICDia");
            oKnittingOrderDetail.FinishDia = oReader.GetString("FinishDia");
            oKnittingOrderDetail.ColorID = oReader.GetInt32("ColorID");
            oKnittingOrderDetail.StratchLength = oReader.GetString("StratchLength");
            oKnittingOrderDetail.MUnitID = oReader.GetInt32("MUnitID");
            oKnittingOrderDetail.Qty = oReader.GetDouble("Qty");
            oKnittingOrderDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oKnittingOrderDetail.Amount = oReader.GetDouble("Amount");
            oKnittingOrderDetail.Remarks = oReader.GetString("Remarks");
            oKnittingOrderDetail.StyleNo = oReader.GetString("StyleNo");
            oKnittingOrderDetail.FabricName = oReader.GetString("FabricName");
            oKnittingOrderDetail.FabricCode = oReader.GetString("FabricCode");
            oKnittingOrderDetail.ColorName = oReader.GetString("ColorName");
            oKnittingOrderDetail.MUnitName = oReader.GetString("MUnitName");
            oKnittingOrderDetail.OrderUnitName = oReader.GetString("OrderUnitName");
            oKnittingOrderDetail.BuyerName = oReader.GetString("BuyerName");
            oKnittingOrderDetail.BrandName = oReader.GetString("BrandName");
            oKnittingOrderDetail.MUSymbol = oReader.GetString("MUSymbol");
            oKnittingOrderDetail.LotNo = oReader.GetString("LotNo");
            oKnittingOrderDetail.YetToChallanQty = oReader.GetDouble("YetToChallanQty");
            oKnittingOrderDetail.GSMID = oReader.GetInt32("GSMID");
            oKnittingOrderDetail.MICDiaID = oReader.GetInt32("MICDiaID");
            oKnittingOrderDetail.FinishDiaID = oReader.GetInt32("FinishDiaID");
            oKnittingOrderDetail.YetReqFabricQty = oReader.GetDouble("YetReqFabricQty");
            oKnittingOrderDetail.StylePcsQty = oReader.GetInt32("StylePcsQty");
            oKnittingOrderDetail.CompositionID = oReader.GetInt32("CompositionID");
            return oKnittingOrderDetail; 
        }

        private KnittingOrderDetail CreateObject(NullHandler oReader)
        {
            KnittingOrderDetail oKnittingOrderDetail = new KnittingOrderDetail();
            oKnittingOrderDetail = MapObject(oReader);
            return oKnittingOrderDetail;
        }

        private List<KnittingOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<KnittingOrderDetail> oKnittingOrderDetail = new List<KnittingOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnittingOrderDetail oItem = CreateObject(oHandler);
                oKnittingOrderDetail.Add(oItem);
            }
            return oKnittingOrderDetail;
        }

        #endregion

        #region Interface implementation
        public KnittingOrderDetail Save(KnittingOrderDetail oKnittingOrderDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnittingOrderDetail.KnittingOrderDetailID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingOrderDetail", EnumRoleOperationType.Add);
                    reader = KnittingOrderDetailDA.InsertUpdate(tc, oKnittingOrderDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingOrderDetail", EnumRoleOperationType.Edit);
                    reader = KnittingOrderDetailDA.InsertUpdate(tc, oKnittingOrderDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrderDetail = new KnittingOrderDetail();
                    oKnittingOrderDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnittingOrderDetail = new KnittingOrderDetail();
                    oKnittingOrderDetail.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingOrderDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnittingOrderDetail oKnittingOrderDetail = new KnittingOrderDetail();
                oKnittingOrderDetail.KnittingOrderDetailID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "KnittingOrderDetail", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "KnittingOrderDetail", id);
                KnittingOrderDetailDA.Delete(tc, oKnittingOrderDetail, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public KnittingOrderDetail Get(int id, Int64 nUserId)
        {
            KnittingOrderDetail oKnittingOrderDetail = new KnittingOrderDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnittingOrderDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrderDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnittingOrderDetail", e);
                #endregion
            }
            return oKnittingOrderDetail;
        }

        public List<KnittingOrderDetail> Gets(Int64 nUserID)
        {
            List<KnittingOrderDetail> oKnittingOrderDetails = new List<KnittingOrderDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderDetailDA.Gets(tc);
                oKnittingOrderDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingOrderDetail oKnittingOrderDetail = new KnittingOrderDetail();
                oKnittingOrderDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingOrderDetails;
        }

        public List<KnittingOrderDetail> Gets(Int64 id, Int64 nUserID)
        {
            List<KnittingOrderDetail> oKnittingOrderDetails = new List<KnittingOrderDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderDetailDA.Gets(tc,id);
                oKnittingOrderDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingOrderDetail oKnittingOrderDetail = new KnittingOrderDetail();
                oKnittingOrderDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingOrderDetails;
        }

        public List<KnittingOrderDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<KnittingOrderDetail> oKnittingOrderDetails = new List<KnittingOrderDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderDetailDA.Gets(tc, sSQL);
                oKnittingOrderDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingOrderDetail", e);
                #endregion
            }
            return oKnittingOrderDetails;
        }

        #endregion
    }

}
