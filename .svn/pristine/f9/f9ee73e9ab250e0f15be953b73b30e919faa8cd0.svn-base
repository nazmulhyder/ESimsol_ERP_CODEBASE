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
    public class GUQCRegisterService : MarshalByRefObject, IGUQCRegisterService
    {
        #region Private functions and declaration

        private GUQCRegister MapObject(NullHandler oReader)
        {
            GUQCRegister oGUQCRegister = new GUQCRegister();
            oGUQCRegister.GUQCDetailID = oReader.GetInt32("GUQCDetailID");
            oGUQCRegister.GUQCID = oReader.GetInt32("GUQCID");
            oGUQCRegister.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oGUQCRegister.QCPassQty = oReader.GetInt32("QCPassQty");
            oGUQCRegister.RejectQty = oReader.GetInt32("RejectQty");
            oGUQCRegister.Remarks = oReader.GetString("Remarks");
            oGUQCRegister.StyleNo = oReader.GetString("StyleNo");
            oGUQCRegister.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oGUQCRegister.TotalQuantity = oReader.GetInt32("TotalQuantity");
            oGUQCRegister.AlredyQCQty = oReader.GetInt32("AlredyQCQty");
            oGUQCRegister.YetToQCQty = oReader.GetInt32("YetToQCQty");
            oGUQCRegister.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");

            oGUQCRegister.QCBy = oReader.GetInt32("QCBy");
            oGUQCRegister.QCDate = oReader.GetDateTime("QCDate");
            oGUQCRegister.ApproveBy = oReader.GetInt32("ApproveBy");
            oGUQCRegister.BUID = oReader.GetInt32("BUID");
            oGUQCRegister.QCNo = oReader.GetString("QCNo");
            oGUQCRegister.BuyerID = oReader.GetInt32("BuyerID");
            oGUQCRegister.StoreID = oReader.GetInt32("StoreID");
            oGUQCRegister.ApproveDate = oReader.GetDateTime("ApproveDate");
            oGUQCRegister.QCByName = oReader.GetString("QCByName");
            oGUQCRegister.ApproveByName = oReader.GetString("ApproveByName");
            oGUQCRegister.StoreName = oReader.GetString("StoreName");
            oGUQCRegister.BuyerName = oReader.GetString("BuyerName");
            oGUQCRegister.POWiseRemarks = oReader.GetString("POWiseRemarks");
            oGUQCRegister.ReportLayout = (EnumReportLayout)oReader.GetInt32("ReportLayout");
            oGUQCRegister.ShipmentDate = oReader.GetDateTime("ShipmentDate");

            return oGUQCRegister;
        }

        private GUQCRegister CreateObject(NullHandler oReader)
        {
            GUQCRegister oGUQCRegister = new GUQCRegister();
            oGUQCRegister = MapObject(oReader);
            return oGUQCRegister;
        }

        private List<GUQCRegister> CreateObjects(IDataReader oReader)
        {
            List<GUQCRegister> oGUQCRegister = new List<GUQCRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GUQCRegister oItem = CreateObject(oHandler);
                oGUQCRegister.Add(oItem);
            }
            return oGUQCRegister;
        }

        #endregion

        #region Interface implementation


        public GUQCRegister Get(int id, Int64 nUserId)
        {
            GUQCRegister oGUQCRegister = new GUQCRegister();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = GUQCRegisterDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUQCRegister = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get GUQCRegister", e);
                #endregion
            }
            return oGUQCRegister;
        }

        public List<GUQCRegister> Gets(int nGUQCID, Int64 nUserID)
        {
            List<GUQCRegister> oGUQCRegisters = new List<GUQCRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUQCRegisterDA.Gets(tc, nGUQCID);
                oGUQCRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                GUQCRegister oGUQCRegister = new GUQCRegister();
                oGUQCRegister.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oGUQCRegisters;
        }

        public List<GUQCRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<GUQCRegister> oGUQCRegisters = new List<GUQCRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUQCRegisterDA.Gets(tc, sSQL);
                oGUQCRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUQCRegister", e);
                #endregion
            }
            return oGUQCRegisters;
        }

        #endregion
    }

}
