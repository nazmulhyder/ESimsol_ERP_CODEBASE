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
    public class ShipmentDetailService : MarshalByRefObject, IShipmentDetailService
    {
        #region Private functions and declaration

        private ShipmentDetail MapObject(NullHandler oReader)
        {
            ShipmentDetail oShipmentDetail = new ShipmentDetail();
            oShipmentDetail.ShipmentDetailID = oReader.GetInt32("ShipmentDetailID");
            oShipmentDetail.ShipmentID = oReader.GetInt32("ShipmentID");
            oShipmentDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oShipmentDetail.LotID = oReader.GetInt32("LotID");
            oShipmentDetail.CountryID = oReader.GetInt32("CountryID");
            oShipmentDetail.ShipmentQty = oReader.GetInt32("ShipmentQty");
            oShipmentDetail.CTNQty = oReader.GetInt32("CTNQty");
            oShipmentDetail.Remarks = oReader.GetString("Remarks");

            oShipmentDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oShipmentDetail.CartonQty = oReader.GetInt32("CartonQty");
            oShipmentDetail.StyleNo = oReader.GetString("StyleNo");
            oShipmentDetail.TotalQuantity = oReader.GetInt32("TotalQuantity");
            oShipmentDetail.CountryName = oReader.GetString("CountryName");
            oShipmentDetail.CountryShortName = oReader.GetString("CountryShortName");
            oShipmentDetail.AlreadyShipmentQty = oReader.GetInt32("AlreadyShipmentQty");
            oShipmentDetail.YetToShipmentQty = oReader.GetInt32("YetToShipmentQty");
            oShipmentDetail.Balance = oReader.GetInt32("Balance");
            oShipmentDetail.LotNo = oReader.GetString("LotNo");

            return oShipmentDetail;
        }

        private ShipmentDetail CreateObject(NullHandler oReader)
        {
            ShipmentDetail oShipmentDetail = new ShipmentDetail();
            oShipmentDetail = MapObject(oReader);
            return oShipmentDetail;
        }

        private List<ShipmentDetail> CreateObjects(IDataReader oReader)
        {
            List<ShipmentDetail> oShipmentDetail = new List<ShipmentDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ShipmentDetail oItem = CreateObject(oHandler);
                oShipmentDetail.Add(oItem);
            }
            return oShipmentDetail;
        }

        #endregion

        #region Interface implementation


        public ShipmentDetail Get(int id, Int64 nUserId)
        {
            ShipmentDetail oShipmentDetail = new ShipmentDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ShipmentDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShipmentDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ShipmentDetail", e);
                #endregion
            }
            return oShipmentDetail;
        }

        public List<ShipmentDetail> Gets(int nShipmentID, Int64 nUserID)
        {
            List<ShipmentDetail> oShipmentDetails = new List<ShipmentDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ShipmentDetailDA.Gets(tc, nShipmentID);
                oShipmentDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ShipmentDetail oShipmentDetail = new ShipmentDetail();
                oShipmentDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oShipmentDetails;
        }

        public List<ShipmentDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ShipmentDetail> oShipmentDetails = new List<ShipmentDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ShipmentDetailDA.Gets(tc, sSQL);
                oShipmentDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShipmentDetail", e);
                #endregion
            }
            return oShipmentDetails;
        }

        #endregion
    }

}
