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
    public class FabricYarnOrderService : MarshalByRefObject, IFabricYarnOrderService
    {
        #region Private functions and declaration
        private static FabricYarnOrder MapObject(NullHandler oReader)
        {
            FabricYarnOrder oFabricYarnOrder = new FabricYarnOrder();
            oFabricYarnOrder.FYOID = oReader.GetInt32("FYOID");
            oFabricYarnOrder.FEOID = oReader.GetInt32("FEOID");
            oFabricYarnOrder.ProductID = oReader.GetInt32("ProductID");
            oFabricYarnOrder.Qty = oReader.GetDouble("Qty");
            oFabricYarnOrder.RequestBy = oReader.GetInt32("RequestBy");
            oFabricYarnOrder.RequestDate = oReader.GetDateTime("RequestDate");
            oFabricYarnOrder.UnitPrice = oReader.GetDouble("UnitPrice");
            
            oFabricYarnOrder.FEONo = oReader.GetString("FEONo");
            oFabricYarnOrder.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFabricYarnOrder.OrderType =(EnumOrderType)oReader.GetInt16("OrderType");
            oFabricYarnOrder.FEONo = oReader.GetString("FEONo");
            oFabricYarnOrder.ProcessName = oReader.GetString("ProcessName");
            oFabricYarnOrder.IsYarnDyed = oReader.GetBoolean("IsYarnDyed");
            oFabricYarnOrder.ProductName = oReader.GetString("ProductName");
            oFabricYarnOrder.ContractorName = oReader.GetString("ContractorName");
            oFabricYarnOrder.AllocationQty = oReader.GetDouble("AllocationQty");
            oFabricYarnOrder.RequestByName = oReader.GetString("RequestByName");
            
            
            return oFabricYarnOrder;
        }

        public static FabricYarnOrder CreateObject(NullHandler oReader)
        {
            FabricYarnOrder oFabricYarnOrder = MapObject(oReader);
            return oFabricYarnOrder;
        }

        private List<FabricYarnOrder> CreateObjects(IDataReader oReader)
        {
            List<FabricYarnOrder> oFabricYarnOrder = new List<FabricYarnOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricYarnOrder oItem = CreateObject(oHandler);
                oFabricYarnOrder.Add(oItem);
            }
            return oFabricYarnOrder;
        }

        #endregion

        #region Interface implementation
        public FabricYarnOrderService() { }

        public FabricYarnOrder IUD(FabricYarnOrder oFabricYarnOrder, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval || nDBOperation == (int)EnumDBOperation.Revise)
                {
                    reader = FabricYarnOrderDA.IUD(tc, oFabricYarnOrder, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricYarnOrder = new FabricYarnOrder();
                        oFabricYarnOrder = CreateObject(oReader);
                    }
                    reader.Close();

                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FabricYarnOrderDA.IUD(tc, oFabricYarnOrder, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oFabricYarnOrder.ErrorMessage = Global.DeleteMessage;
                }
                else
                {
                    throw new Exception("Invalid Operation In Service");
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnOrder = new FabricYarnOrder();
                oFabricYarnOrder.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oFabricYarnOrder;
        }

        public FabricYarnOrder Get(int nFYOID, Int64 nUserId)
        {
            FabricYarnOrder oFabricYarnOrder = new FabricYarnOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricYarnOrderDA.Get(tc, nFYOID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricYarnOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnOrder = new FabricYarnOrder();
                oFabricYarnOrder.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFabricYarnOrder;
        }

        public List<FabricYarnOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricYarnOrder> oFabricYarnOrders = new List<FabricYarnOrder>();
            FabricYarnOrder oFabricYarnOrder = new FabricYarnOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricYarnOrderDA.Gets(tc, sSQL);
                oFabricYarnOrders = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnOrder.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oFabricYarnOrders.Add(oFabricYarnOrder);
                #endregion
            }

            return oFabricYarnOrders;
        }


        public FabricYarnOrder RequestAndSave(FabricYarnOrder oFabricYarnOrder, Int64 nUserID)
        {
            List<FabricYarnOrder> oFYOs = new List<FabricYarnOrder>();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                var newFYOs = oFabricYarnOrder.FYOs.Where(x => x.FYOID <= 0).ToList();
                oFabricYarnOrder.FYOs.RemoveAll(x => x.FYOID <= 0);

                //Newly Saved 
                if (newFYOs.Any())
                {
                    foreach (FabricYarnOrder oItem in newFYOs)
                    {
                        reader = FabricYarnOrderDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFabricYarnOrder = new FabricYarnOrder();
                            oFabricYarnOrder = CreateObject(oReader);
                            oFabricYarnOrder.FYOs.Add(oFabricYarnOrder);
                        }
                        reader.Close();
                    }
                }

                //  Send Request
                foreach (FabricYarnOrder oItem in oFabricYarnOrder.FYOs)
                {
                    reader = FabricYarnOrderDA.IUD(tc, oItem, (int)EnumDBOperation.Request, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricYarnOrder = new FabricYarnOrder();
                        oFabricYarnOrder = CreateObject(oReader);
                        oFYOs.Add(oFabricYarnOrder);
                    }
                    reader.Close();
                }

                // Get Item by FEO Which is not found in "oFYOs" List
                if(oFYOs.Any())
                {
                    string sSQl = "Select * from View_FabricYarnOrder Where FEOID=" + oFYOs.First().FEOID + " And FYOID NOT IN (" + string.Join(",", oFYOs.Select(x => x.FYOID).ToList()) + ")";
                    reader = FabricYarnOrderDA.Gets(tc, sSQl);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricYarnOrder = new FabricYarnOrder();
                        oFabricYarnOrder = CreateObject(oReader);
                        oFYOs.Add(oFabricYarnOrder);
                    }
                    reader.Close();
                    oFYOs = oFYOs.OrderBy(x => x.FYOID).ToList();
                    oFabricYarnOrder = new FabricYarnOrder();
                    oFabricYarnOrder.FYOs = oFYOs;
                }
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnOrder = new FabricYarnOrder();
                oFabricYarnOrder.FYOs = new List<FabricYarnOrder>();
                oFabricYarnOrder.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oFabricYarnOrder;
        }


        

        #endregion
    }
}
