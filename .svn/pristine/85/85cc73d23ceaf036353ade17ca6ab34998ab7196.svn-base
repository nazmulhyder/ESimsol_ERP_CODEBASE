using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class FabricYarnDeliveryOrderService : MarshalByRefObject, IFabricYarnDeliveryOrderService
    {
       #region Private functions and declaration
        private static FabricYarnDeliveryOrder MapObject(NullHandler oReader)
        {
            FabricYarnDeliveryOrder oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
            oFabricYarnDeliveryOrder.FYDOID = oReader.GetInt32("FYDOID");
            oFabricYarnDeliveryOrder.FEOID = oReader.GetInt32("FEOID");
            oFabricYarnDeliveryOrder.FYDNo =oReader.GetString("FYDNo");
            oFabricYarnDeliveryOrder.DeliveryUnit = (EnumTextileUnit)oReader.GetInt16("DeliveryUnit");
            oFabricYarnDeliveryOrder.ApproveBy = oReader.GetInt32("ApproveBy");
            oFabricYarnDeliveryOrder.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFabricYarnDeliveryOrder.ExpectedDeliveryDate = oReader.GetDateTime("ExpectedDeliveryDate");
            oFabricYarnDeliveryOrder.FEONo =oReader.GetString("FEONo");
            oFabricYarnDeliveryOrder.ApproveByName =oReader.GetString("ApproveByName");
            oFabricYarnDeliveryOrder.OrderQty =oReader.GetDouble("OrderQty");
            oFabricYarnDeliveryOrder.ChallanQty =oReader.GetDouble("ChallanQty");
            oFabricYarnDeliveryOrder.ReviseNo = oReader.GetInt32("ReviseNo");
            oFabricYarnDeliveryOrder.ReviseDate = oReader.GetDateTime("ReviseDate");
            oFabricYarnDeliveryOrder.LCNo = oReader.GetString("LCNo");
            oFabricYarnDeliveryOrder.BuyerName = oReader.GetString("BuyerName");
            oFabricYarnDeliveryOrder.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oFabricYarnDeliveryOrder.Construction = oReader.GetString("Construction");
            oFabricYarnDeliveryOrder.Remark = oReader.GetString("Remark");
            return oFabricYarnDeliveryOrder;
       }

        public static FabricYarnDeliveryOrder CreateObject(NullHandler oReader)
        {
            FabricYarnDeliveryOrder oFabricYarnDeliveryOrder = MapObject(oReader);
            return oFabricYarnDeliveryOrder;
        }

        private List<FabricYarnDeliveryOrder> CreateObjects(IDataReader oReader)
        {
            List<FabricYarnDeliveryOrder> oFabricYarnDeliveryOrders = new List<FabricYarnDeliveryOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricYarnDeliveryOrder oItem = CreateObject(oHandler);
                oFabricYarnDeliveryOrders.Add(oItem);
            }
            return oFabricYarnDeliveryOrders;
        }

        #endregion

       #region Interface implementation
        public FabricYarnDeliveryOrderService() { }

        public FabricYarnDeliveryOrder IUD(FabricYarnDeliveryOrder oFabricYarnDeliveryOrder, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            var oFYDODetails = oFabricYarnDeliveryOrder.FYDODetails.Where(x=>x.FYDODetailID==0).ToList();
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval || nDBOperation == (int)EnumDBOperation.Revise)
                {
                    reader = FabricYarnDeliveryOrderDA.IUD(tc, oFabricYarnDeliveryOrder, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                        oFabricYarnDeliveryOrder = CreateObject(oReader);
                    }
                    reader.Close();


                    oFYDODetails.ForEach(x => x.FYDOID = oFabricYarnDeliveryOrder.FYDOID);

                    foreach (FabricYarnDeliveryOrderDetail oItem in oFYDODetails)
                    {
                        oItem.FYDOID = oFabricYarnDeliveryOrder.FYDOID;
                        reader = FabricYarnDeliveryOrderDetailDA.IUD(tc, oItem, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFabricYarnDeliveryOrder.FYDODetails.Add(FabricYarnDeliveryOrderDetailService.CreateObject(oReader));
                        }
                        reader.Close();
                    }


                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FabricYarnDeliveryOrderDA.IUD(tc, oFabricYarnDeliveryOrder, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oFabricYarnDeliveryOrder.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                oFabricYarnDeliveryOrder.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oFabricYarnDeliveryOrder;
        }

        public FabricYarnDeliveryOrder Get(int nFYDOID, Int64 nUserId)
        {
            FabricYarnDeliveryOrder oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricYarnDeliveryOrderDA.Get(tc, nFYDOID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricYarnDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
                oFabricYarnDeliveryOrder.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oFabricYarnDeliveryOrder;
        }

        public List<FabricYarnDeliveryOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricYarnDeliveryOrder> oFabricYarnDeliveryOrders = new List<FabricYarnDeliveryOrder>();
            FabricYarnDeliveryOrder oFabricYarnDeliveryOrder = new FabricYarnDeliveryOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricYarnDeliveryOrderDA.Gets(tc, sSQL);
                oFabricYarnDeliveryOrders = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryOrder.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                oFabricYarnDeliveryOrders.Add(oFabricYarnDeliveryOrder);
                #endregion
            }

            return oFabricYarnDeliveryOrders;
        }

        #endregion
    }
}
