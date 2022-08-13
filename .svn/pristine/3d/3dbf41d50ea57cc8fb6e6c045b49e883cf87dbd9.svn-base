using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class FabricYarnDeliveryOrderDetailService : MarshalByRefObject, IFabricYarnDeliveryOrderDetailService
    {
        #region Private functions and declaration
        private static FabricYarnDeliveryOrderDetail MapObject(NullHandler oReader)
        {
            FabricYarnDeliveryOrderDetail oFabricYarnDeliveryOrderDetail = new FabricYarnDeliveryOrderDetail();
            oFabricYarnDeliveryOrderDetail.FYDODetailID = oReader.GetInt32("FYDODetailID");
            oFabricYarnDeliveryOrderDetail.FYDOID = oReader.GetInt32("FYDOID");
            oFabricYarnDeliveryOrderDetail.ProductID = oReader.GetInt32("ProductID");
            oFabricYarnDeliveryOrderDetail.Qty = oReader.GetDouble("Qty");
            oFabricYarnDeliveryOrderDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oFabricYarnDeliveryOrderDetail.ProductCode = oReader.GetString("ProductCode");
            oFabricYarnDeliveryOrderDetail.ProductName = oReader.GetString("ProductName");
            oFabricYarnDeliveryOrderDetail.ChallanQty = oReader.GetDouble("ChallanQty");
            oFabricYarnDeliveryOrderDetail.FEOID = oReader.GetInt32("FEOID");
            oFabricYarnDeliveryOrderDetail.FEONO = oReader.GetString("FEONO");
            return oFabricYarnDeliveryOrderDetail;
       }

        public static FabricYarnDeliveryOrderDetail CreateObject(NullHandler oReader)
        {
            FabricYarnDeliveryOrderDetail oFabricYarnDeliveryOrderDetail = MapObject(oReader);
            return oFabricYarnDeliveryOrderDetail;
        }

        private List<FabricYarnDeliveryOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricYarnDeliveryOrderDetail> oFabricYarnDeliveryOrderDetails = new List<FabricYarnDeliveryOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricYarnDeliveryOrderDetail oItem = CreateObject(oHandler);
                oFabricYarnDeliveryOrderDetails.Add(oItem);
            }
            return oFabricYarnDeliveryOrderDetails;
        }

        #endregion

        #region Interface implementation
        public FabricYarnDeliveryOrderDetailService() { }

        public FabricYarnDeliveryOrderDetail IUD(FabricYarnDeliveryOrderDetail oFYDODetail, int nDBOperation, Int64 nUserID)
        {
            FabricYarnDeliveryOrder oFYDO = new FabricYarnDeliveryOrder();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    if (oFYDODetail.FYDOID <= 0 && oFYDODetail.FYDO!=null)
                    {
                        reader = FabricYarnDeliveryOrderDA.IUD(tc, oFYDODetail.FYDO, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFYDO = new FabricYarnDeliveryOrder();
                            oFYDO = FabricYarnDeliveryOrderService.CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    oFYDODetail.FYDOID = (oFYDODetail.FYDOID > 0) ? oFYDODetail.FYDOID : oFYDO.FYDOID;
                    reader = FabricYarnDeliveryOrderDetailDA.IUD(tc, oFYDODetail, nDBOperation, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFYDODetail = new FabricYarnDeliveryOrderDetail();
                        oFYDODetail = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FabricYarnDeliveryOrderDetailDA.IUD(tc, oFYDODetail, nDBOperation, nUserID);
                    oReader = new NullHandler(reader);
                    reader.Close();
                    oFYDODetail.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFYDODetail = new FabricYarnDeliveryOrderDetail();
                oFYDODetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            oFYDODetail.FYDO = oFYDO;
            return oFYDODetail;
        }

        public FabricYarnDeliveryOrderDetail Get(int nSULDDetailID, Int64 nUserId)
        {
            FabricYarnDeliveryOrderDetail oFabricYarnDeliveryOrderDetail = new FabricYarnDeliveryOrderDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricYarnDeliveryOrderDetailDA.Get(tc, nSULDDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricYarnDeliveryOrderDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryOrderDetail = new FabricYarnDeliveryOrderDetail();
                oFabricYarnDeliveryOrderDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFabricYarnDeliveryOrderDetail;
        }

        public List<FabricYarnDeliveryOrderDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricYarnDeliveryOrderDetail> oSULayDownDetails = new List<FabricYarnDeliveryOrderDetail>();
            FabricYarnDeliveryOrderDetail oFabricYarnDeliveryOrderDetail = new FabricYarnDeliveryOrderDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricYarnDeliveryOrderDetailDA.Gets(tc, sSQL);
                oSULayDownDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryOrderDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oSULayDownDetails.Add(oFabricYarnDeliveryOrderDetail);
                #endregion
            }

            return oSULayDownDetails;
        }

      
        
        #endregion
    }
}
