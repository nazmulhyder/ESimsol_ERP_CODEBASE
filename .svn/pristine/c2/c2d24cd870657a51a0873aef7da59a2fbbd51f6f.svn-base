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
    public class ITaxRebateSchemeSlabService : MarshalByRefObject, IITaxRebateSchemeSlabService
    {
        #region Private functions and declaration
        private ITaxRebateSchemeSlab MapObject(NullHandler oReader)
        {
            ITaxRebateSchemeSlab oITaxRebateSchemeSlab = new ITaxRebateSchemeSlab();
            oITaxRebateSchemeSlab.ITaxRSSID = oReader.GetInt32("ITaxRSSID");
            oITaxRebateSchemeSlab.ITaxRebateSchemeID = oReader.GetInt32("ITaxRebateSchemeID");
            oITaxRebateSchemeSlab.MinAmount = oReader.GetDouble("MinAmount");
            oITaxRebateSchemeSlab.MaxAmount = oReader.GetDouble("MaxAmount");

            return oITaxRebateSchemeSlab;
        }

        private ITaxRebateSchemeSlab CreateObject(NullHandler oReader)
        {
            ITaxRebateSchemeSlab oITaxRebateSchemeSlab = MapObject(oReader);
            return oITaxRebateSchemeSlab;
        }

        private List<ITaxRebateSchemeSlab> CreateObjects(IDataReader oReader)
        {
            List<ITaxRebateSchemeSlab> oITaxRebateSchemeSlabs = new List<ITaxRebateSchemeSlab>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxRebateSchemeSlab oItem = CreateObject(oHandler);
                oITaxRebateSchemeSlabs.Add(oItem);
            }
            return oITaxRebateSchemeSlabs;
        }

        #endregion

        #region Interface implementation
        public ITaxRebateSchemeSlabService() { }
        public ITaxRebateSchemeSlab IUD(ITaxRebateSchemeSlab oITaxRebateSchemeSlab, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxRebateSchemeSlabDA.IUD(tc, oITaxRebateSchemeSlab, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebateSchemeSlab = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oITaxRebateSchemeSlab = new ITaxRebateSchemeSlab();
                    oITaxRebateSchemeSlab.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRebateSchemeSlab = new ITaxRebateSchemeSlab();
                oITaxRebateSchemeSlab.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oITaxRebateSchemeSlab;
        }

        public ITaxRebateSchemeSlab Get(string sSQL, Int64 nUserId)
        {
            ITaxRebateSchemeSlab oITaxRebateSchemeSlab = new ITaxRebateSchemeSlab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRebateSchemeSlabDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebateSchemeSlab = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRebateSchemeSlab.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRebateSchemeSlab;
        }

        public List<ITaxRebateSchemeSlab> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxRebateSchemeSlab> oITaxRebateSchemeSlabs = new List<ITaxRebateSchemeSlab>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRebateSchemeSlabDA.Gets(sSQL, tc);
                oITaxRebateSchemeSlabs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ITaxRebateSchemeSlab oITaxRebateSchemeSlab = new ITaxRebateSchemeSlab();
                oITaxRebateSchemeSlab.ErrorMessage = e.Message;
                oITaxRebateSchemeSlabs.Add(oITaxRebateSchemeSlab);
                #endregion
            }
            return oITaxRebateSchemeSlabs;
        }
        #endregion
    }
}
