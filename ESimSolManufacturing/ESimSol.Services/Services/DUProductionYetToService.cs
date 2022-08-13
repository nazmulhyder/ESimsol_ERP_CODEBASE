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
    public class DUProductionYetToService : MarshalByRefObject, IDUProductionYetToService
    {
    
        #region Private functions and declaration
        private static DUProductionYetTo MapObject(NullHandler oReader)
        {
            DUProductionYetTo oDUProductionYetTo = new DUProductionYetTo();
            oDUProductionYetTo.OrderNo = oReader.GetString("OrderNo");
            oDUProductionYetTo.OrderDate = oReader.GetDateTime("OrderDate");
            oDUProductionYetTo.PINo = oReader.GetString("PINo");
            oDUProductionYetTo.ContractorID = oReader.GetInt32("ContractorID");
            oDUProductionYetTo.ContractorName = oReader.GetString("ContractorName");
            oDUProductionYetTo.EndBuyer = oReader.GetString("EndBuyer");
            oDUProductionYetTo.BuyerConcern = oReader.GetString("BuyerConcern");
            oDUProductionYetTo.CategoryName = oReader.GetString("CategoryName");
            oDUProductionYetTo.ProductCode = oReader.GetInt32("ProductCode");
            oDUProductionYetTo.ProductName = oReader.GetString("ProductName");
            oDUProductionYetTo.ProductID = oReader.GetInt32("ProductID");
            oDUProductionYetTo.DyeingOrderType = oReader.GetInt32("DyeingOrderType");
            oDUProductionYetTo.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUProductionYetTo.OrderType = oReader.GetString("OrderType");
            oDUProductionYetTo.Qty = oReader.GetDouble("Qty");
            oDUProductionYetTo.Qty_Prod = oReader.GetDouble("Qty_Prod");
            oDUProductionYetTo.Qty_DC = oReader.GetDouble("Qty_DC");
            oDUProductionYetTo.Qty_Req = oReader.GetDouble("Qty_Req");
            oDUProductionYetTo.StockInHand = oReader.GetDouble("StockInHand");
            oDUProductionYetTo.Qty_Unit = oReader.GetDouble("Qty_Unit");
            oDUProductionYetTo.Qty_DCToDay = oReader.GetDouble("Qty_DCToDay");
            oDUProductionYetTo.Qty_QCToDay = oReader.GetDouble("Qty_QCToDay");
            oDUProductionYetTo.IsInHouse = oReader.GetBoolean("IsInHouse");

            oDUProductionYetTo.MKTPName = oReader.GetString("MKTPName");
            oDUProductionYetTo.MKTPNickName = oReader.GetString("MKTPNickName");
            oDUProductionYetTo.MUName = oReader.GetString("MUName");
            oDUProductionYetTo.ColorCount = oReader.GetInt32("ColorCount");
            oDUProductionYetTo.HankorCone = (EumDyeingType)oReader.GetInt16("HankorCone");
            if ((oDUProductionYetTo.Qty - oDUProductionYetTo.Qty_DC) < oDUProductionYetTo.StockInHand)
            {
                oDUProductionYetTo.StockInHand=Math.Round((oDUProductionYetTo.Qty - oDUProductionYetTo.Qty_DC),2);
            }
            if ((oDUProductionYetTo.Qty) < oDUProductionYetTo.Qty_Prod)
            {
                oDUProductionYetTo.Qty_Prod = Math.Round(oDUProductionYetTo.Qty, 2);
            }

            return oDUProductionYetTo;

         
        }

        public static DUProductionYetTo CreateObject(NullHandler oReader)
        {
            DUProductionYetTo oDUProductionYetTo = new DUProductionYetTo();
            oDUProductionYetTo = MapObject(oReader);            
            return oDUProductionYetTo;
        }

        private List<DUProductionYetTo> CreateObjects(IDataReader oReader)
        {
            List<DUProductionYetTo> oDUProductionYetTos = new List<DUProductionYetTo>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUProductionYetTo oItem = CreateObject(oHandler);
                oDUProductionYetTos.Add(oItem);
            }
            return oDUProductionYetTos;
        }

        #endregion

        #region Interface implementation
        public DUProductionYetToService() { }
        public DUProductionYetTo Get(int id, Int64 nUserId)
        {
            DUProductionYetTo oDUProductionYetTo = new DUProductionYetTo();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUProductionYetToDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUProductionYetTo = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DUProductionYetTo", e);
                #endregion
            }

            return oDUProductionYetTo;
        }
        public List<DUProductionYetTo> Gets(string sSQL, Int64 nUserId)
        {
            List<DUProductionYetTo> oDUProductionYetTos = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUProductionYetToDA.Gets(tc, sSQL);
                oDUProductionYetTos = CreateObjects(reader);
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUProductionYetTo", e);
                #endregion
            }

            return oDUProductionYetTos;
        }
     
     
     
        #endregion
    }
}

