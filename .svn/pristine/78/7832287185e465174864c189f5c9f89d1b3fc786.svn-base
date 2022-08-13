using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class FabricSuggestionHistoryService : MarshalByRefObject, IFabricSuggestionHistoryService
    {
        #region Private functions and declaration
        private FabricSuggestionHistory MapObject(NullHandler oReader)
        {
            FabricSuggestionHistory oFSH = new FabricSuggestionHistory();
            oFSH.FabricSuggestionID = oReader.GetInt32("FabricSuggestionID");
            oFSH.FabricID = oReader.GetInt32("FabricID");
            oFSH.ProductID = oReader.GetInt32("ProductID");
            oFSH.Construction = oReader.GetString("Construction");
            oFSH.ColorInfo = oReader.GetString("ColorInfo");
            oFSH.FabricWidth = oReader.GetString("FabricWidth");
            oFSH.FinishType = oReader.GetInt32("FinishType");
            oFSH.Remarks = oReader.GetString("Remarks");
            oFSH.ProcessType = oReader.GetInt32("ProcessType");
            oFSH.FabricWeave = oReader.GetInt32("FabricWeave");
            oFSH.FabricDesignID = oReader.GetInt32("FabricDesignID");
            oFSH.WeftColor = oReader.GetString("WeftColor");
            oFSH.ActualConstruction = oReader.GetString("ActualConstruction");
            oFSH.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");

            oFSH.FabricNo = oReader.GetString("FabricNo");
            oFSH.ProductCode = oReader.GetString("ProductCode");
            oFSH.ProductName = oReader.GetString("ProductName");
            oFSH.FinishTypeName = oReader.GetString("FinishTypeName");
            oFSH.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFSH.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFSH.CreatedByName = oReader.GetString("CreatedByName");
            oFSH.ModifiedByName = oReader.GetString("ModifiedByName");
            return oFSH;
        }
        private FabricSuggestionHistory CreateObject(NullHandler oReader)
        {
            FabricSuggestionHistory oFabricSuggestionHistory = new FabricSuggestionHistory();
            oFabricSuggestionHistory = MapObject(oReader);
            return oFabricSuggestionHistory;
        }
        private List<FabricSuggestionHistory> CreateObjects(IDataReader oReader)
        {
            List<FabricSuggestionHistory> oFabricSuggestionHistory = new List<FabricSuggestionHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSuggestionHistory oItem = CreateObject(oHandler);
                oFabricSuggestionHistory.Add(oItem);
            }
            return oFabricSuggestionHistory;
        }

        #endregion

        #region Interface implementation
        public List<FabricSuggestionHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricSuggestionHistory> oFabricSuggestionHistorys = new List<FabricSuggestionHistory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSuggestionHistoryDA.Gets(tc, sSQL);
                oFabricSuggestionHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricSuggestionHistorys = new List<FabricSuggestionHistory>();
                FabricSuggestionHistory oFabricSuggestionHistory = new FabricSuggestionHistory();
                oFabricSuggestionHistory.ErrorMessage = e.Message.Split('~')[0];
                oFabricSuggestionHistorys.Add(oFabricSuggestionHistory);
                #endregion
            }
            return oFabricSuggestionHistorys;
        }

        #endregion
    }
}
