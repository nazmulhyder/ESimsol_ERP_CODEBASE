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
    public class GUQCDetailService : MarshalByRefObject, IGUQCDetailService
    {
        #region Private functions and declaration

        private GUQCDetail MapObject(NullHandler oReader)
        {
            GUQCDetail oGUQCDetail = new GUQCDetail();
            oGUQCDetail.GUQCDetailID = oReader.GetInt32("GUQCDetailID");
            oGUQCDetail.GUQCID = oReader.GetInt32("GUQCID");
            oGUQCDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oGUQCDetail.QCPassQty = oReader.GetInt32("QCPassQty");
            oGUQCDetail.RejectQty = oReader.GetInt32("RejectQty");
            oGUQCDetail.Remarks = oReader.GetString("Remarks");
            oGUQCDetail.StyleNo = oReader.GetString("StyleNo");
            oGUQCDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oGUQCDetail.TotalQuantity = oReader.GetInt32("TotalQuantity");
            oGUQCDetail.AlredyQCQty = oReader.GetInt32("AlredyQCQty");
            oGUQCDetail.YetToQCQty = oReader.GetInt32("YetToQCQty");
            oGUQCDetail.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            return oGUQCDetail;
        }

        private GUQCDetail CreateObject(NullHandler oReader)
        {
            GUQCDetail oGUQCDetail = new GUQCDetail();
            oGUQCDetail = MapObject(oReader);
            return oGUQCDetail;
        }

        private List<GUQCDetail> CreateObjects(IDataReader oReader)
        {
            List<GUQCDetail> oGUQCDetail = new List<GUQCDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GUQCDetail oItem = CreateObject(oHandler);
                oGUQCDetail.Add(oItem);
            }
            return oGUQCDetail;
        }

        #endregion

        #region Interface implementation


        public GUQCDetail Get(int id, Int64 nUserId)
        {
            GUQCDetail oGUQCDetail = new GUQCDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = GUQCDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUQCDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get GUQCDetail", e);
                #endregion
            }
            return oGUQCDetail;
        }

        public List<GUQCDetail> Gets(int nGUQCID, Int64 nUserID)
        {
            List<GUQCDetail> oGUQCDetails = new List<GUQCDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUQCDetailDA.Gets(tc, nGUQCID);
                oGUQCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                GUQCDetail oGUQCDetail = new GUQCDetail();
                oGUQCDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oGUQCDetails;
        }

        public List<GUQCDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<GUQCDetail> oGUQCDetails = new List<GUQCDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GUQCDetailDA.Gets(tc, sSQL);
                oGUQCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUQCDetail", e);
                #endregion
            }
            return oGUQCDetails;
        }

        #endregion
    }

}
