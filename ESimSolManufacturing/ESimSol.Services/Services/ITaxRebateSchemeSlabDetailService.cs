using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ITaxRebateSchemeSlabDetailService : MarshalByRefObject, IITaxRebateSchemeSlabDetailService
    {
        #region Private functions and declaration
        private ITaxRebateSchemeSlabDetail MapObject(NullHandler oReader)
        {
            ITaxRebateSchemeSlabDetail oITaxRebateSchemeSlabDetail = new ITaxRebateSchemeSlabDetail();
            oITaxRebateSchemeSlabDetail.ITaxRSSDID = oReader.GetInt32("ITaxRSSDID");
            oITaxRebateSchemeSlabDetail.ITaxRSSID = oReader.GetInt32("ITaxRSSID");
            oITaxRebateSchemeSlabDetail.UptoAmount = oReader.GetDouble("UptoAmount");
            oITaxRebateSchemeSlabDetail.SlabRebateInPercent = oReader.GetDouble("SlabRebateInPercent");

            return oITaxRebateSchemeSlabDetail;
        }

        private ITaxRebateSchemeSlabDetail CreateObject(NullHandler oReader)
        {
            ITaxRebateSchemeSlabDetail oITaxRebateSchemeSlabDetail = MapObject(oReader);
            return oITaxRebateSchemeSlabDetail;
        }

        private List<ITaxRebateSchemeSlabDetail> CreateObjects(IDataReader oReader)
        {
            List<ITaxRebateSchemeSlabDetail> oITaxRebateSchemeSlabDetails = new List<ITaxRebateSchemeSlabDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxRebateSchemeSlabDetail oItem = CreateObject(oHandler);
                oITaxRebateSchemeSlabDetails.Add(oItem);
            }
            return oITaxRebateSchemeSlabDetails;
        }

        #endregion

        #region Interface implementation
        public ITaxRebateSchemeSlabDetailService() { }
        public ITaxRebateSchemeSlabDetail IUD(ITaxRebateSchemeSlabDetail oITaxRebateSchemeSlabDetail, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxRebateSchemeSlabDetailDA.IUD(tc, oITaxRebateSchemeSlabDetail, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebateSchemeSlabDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oITaxRebateSchemeSlabDetail = new ITaxRebateSchemeSlabDetail();
                    oITaxRebateSchemeSlabDetail.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRebateSchemeSlabDetail = new ITaxRebateSchemeSlabDetail();
                oITaxRebateSchemeSlabDetail.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oITaxRebateSchemeSlabDetail;
        }

        public ITaxRebateSchemeSlabDetail Get(string sSQL, Int64 nUserId)
        {
            ITaxRebateSchemeSlabDetail oITaxRebateSchemeSlabDetail = new ITaxRebateSchemeSlabDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRebateSchemeSlabDetailDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebateSchemeSlabDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRebateSchemeSlabDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRebateSchemeSlabDetail;
        }

        public List<ITaxRebateSchemeSlabDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxRebateSchemeSlabDetail> oITaxRebateSchemeSlabDetails = new List<ITaxRebateSchemeSlabDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRebateSchemeSlabDetailDA.Gets(sSQL, tc);
                oITaxRebateSchemeSlabDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ITaxRebateSchemeSlabDetail oITaxRebateSchemeSlabDetail = new ITaxRebateSchemeSlabDetail();
                oITaxRebateSchemeSlabDetail.ErrorMessage = e.Message;
                oITaxRebateSchemeSlabDetails.Add(oITaxRebateSchemeSlabDetail);
                #endregion
            }
            return oITaxRebateSchemeSlabDetails;
        }
        #endregion
    }
}
