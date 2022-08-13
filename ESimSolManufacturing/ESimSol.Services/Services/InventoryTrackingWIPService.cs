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
    public class InventoryTrackingWIPService : MarshalByRefObject, IInventoryTrackingWIPService
    {
        #region Private functions and declaration

        private InventoryTrackingWIP MapObject(NullHandler oReader)
        {
            InventoryTrackingWIP oInventoryTrackingWIP = new InventoryTrackingWIP();
            oInventoryTrackingWIP.BUID = oReader.GetInt32("BUID");
            oInventoryTrackingWIP.StartDate = oReader.GetDateTime("StartDate");
            oInventoryTrackingWIP.EndDate = oReader.GetDateTime("EndDate");
            oInventoryTrackingWIP.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oInventoryTrackingWIP.ProductID = oReader.GetInt32("ProductID");
            oInventoryTrackingWIP.ProductName = oReader.GetString("ProductName");
            oInventoryTrackingWIP.ProductCode = oReader.GetString("ProductCode");
            oInventoryTrackingWIP.OpeningQty = oReader.GetDouble("OpeningQty");
            oInventoryTrackingWIP.ClosingQty = oReader.GetDouble("ClosingQty");
            oInventoryTrackingWIP.Qty = oReader.GetDouble("Qty");
            oInventoryTrackingWIP.InQty = oReader.GetDouble("InQty");
            oInventoryTrackingWIP.OutQty = oReader.GetDouble("OutQty");
            oInventoryTrackingWIP.PCategoryID = oReader.GetInt32("PCategoryID");
            oInventoryTrackingWIP.PCategoryName = oReader.GetString("PCategoryName");
            oInventoryTrackingWIP.WorkingUnitName = oReader.GetString("WorkingUnitName");
            oInventoryTrackingWIP.QtyPacking = oReader.GetDouble("QtyPacking");
            oInventoryTrackingWIP.Qty_RS = oReader.GetDouble("Qty_RS");
            oInventoryTrackingWIP.QtyRecycle = oReader.GetDouble("QtyRecycle");
            oInventoryTrackingWIP.QtyWastage = oReader.GetDouble("QtyWastage");
            oInventoryTrackingWIP.QtyShort = oReader.GetDouble("QtyShort");
            oInventoryTrackingWIP.InOutType = oReader.GetInt32("InOutType");
            oInventoryTrackingWIP.LotID = oReader.GetInt32("LotID");
            oInventoryTrackingWIP.MUnitID = oReader.GetInt32("MUnitID");
            oInventoryTrackingWIP.TransactionTime = oReader.GetDateTime("TransactionTime");
            oInventoryTrackingWIP.StoreName = oReader.GetString("StoreName");
            oInventoryTrackingWIP.LotNo = oReader.GetString("LotNo");
            oInventoryTrackingWIP.TriggerParentID = oReader.GetInt32("TriggerParentID");
            oInventoryTrackingWIP.RefNo = oReader.GetString("RefNo");
            oInventoryTrackingWIP.USymbol = oReader.GetString("USymbol");

            return oInventoryTrackingWIP;
        }

        private InventoryTrackingWIP CreateObject(NullHandler oReader)
        {
            InventoryTrackingWIP oInventoryTrackingWIP = new InventoryTrackingWIP();
            oInventoryTrackingWIP = MapObject(oReader);
            return oInventoryTrackingWIP;
        }

        private List<InventoryTrackingWIP> CreateObjects(IDataReader oReader)
        {
            List<InventoryTrackingWIP> oInventoryTrackingWIP = new List<InventoryTrackingWIP>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                InventoryTrackingWIP oItem = CreateObject(oHandler);
                oInventoryTrackingWIP.Add(oItem);
            }
            return oInventoryTrackingWIP;
        }

        #endregion

        #region Interface implementation
        public List<InventoryTrackingWIP> Gets(InventoryTrackingWIP oITWIWP, Int64 nUserID)
        {
            List<InventoryTrackingWIP> oInventoryTrackingWIPs = new List<InventoryTrackingWIP>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = InventoryTrackingWIPDA.Gets(tc, oITWIWP, nUserID);
                oInventoryTrackingWIPs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                InventoryTrackingWIP oInventoryTrackingWIP = new InventoryTrackingWIP();
                oInventoryTrackingWIP.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oInventoryTrackingWIPs;
        }

        public List<InventoryTrackingWIP> Gets(string sSQL, Int64 nUserID)
        {
            List<InventoryTrackingWIP> oInventoryTrackingWIPs = new List<InventoryTrackingWIP>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = InventoryTrackingWIPDA.Gets(tc, sSQL);
                oInventoryTrackingWIPs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InventoryTrackingWIP", e);
                #endregion
            }
            return oInventoryTrackingWIPs;
        }

        #endregion
    }

}
