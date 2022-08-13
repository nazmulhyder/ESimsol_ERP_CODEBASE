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
    public class FabricRnDService : MarshalByRefObject, IFabricRnDService
    {
        #region Private functions and declaration
        private FabricRnD MapObject(NullHandler oReader)
        {
            FabricRnD oFabricRnD = new FabricRnD();
            oFabricRnD.FabricRnDID = oReader.GetInt32("FabricRnDID");
            oFabricRnD.FabricID = oReader.GetInt32("FabricID");
            oFabricRnD.FSCDID = oReader.GetInt32("FSCDID");
            oFabricRnD.FabricNo = oReader.GetString("FabricNo");
            //oFabricRnD.FabricRnDNum = oReader.GetString("FabricRnDNum");
            oFabricRnD.IssueDate = oReader.GetDateTime("IssueDate");
            oFabricRnD.ProductIDWarp = oReader.GetInt32("ProductIDWarp");
            oFabricRnD.FabricWidth = oReader.GetString("FabricWidth");
            oFabricRnD.FinishType = oReader.GetInt32("FinishType");
            oFabricRnD.FinishTypeName = oReader.GetString("FinishTypeName");
            oFabricRnD.FabricDesignID = oReader.GetInt32("FabricDesignID");
            oFabricRnD.WeftColor = oReader.GetString("WeftColor");
            oFabricRnD.ConstructionSuggest = oReader.GetString("ConstructionSuggest");
            oFabricRnD.WarpCount = oReader.GetString("WarpCount");
            oFabricRnD.WeftCount = oReader.GetString("WeftCount");
            oFabricRnD.EPI = oReader.GetString("EPI");
            oFabricRnD.PPI = oReader.GetString("PPI");
            oFabricRnD.FabricDesignName = oReader.GetString("FabricDesignName");
            oFabricRnD.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oFabricRnD.ProcessType = oReader.GetInt32("ProcessType");
            oFabricRnD.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFabricRnD.FabricWeave = oReader.GetInt32("FabricWeave");
            oFabricRnD.FabricWeaveName = oReader.GetString("FabricWeaveName");
            //oFabricRnD.FabricRnDAttachmentCount = oReader.GetInt32("FabricRnDAttachmentCount");
            oFabricRnD.PrepareByName = oReader.GetString("PrepareByName");
            oFabricRnD.ProductIDWeft = oReader.GetInt32("ProductIDWeft");
            oFabricRnD.WeightAct = oReader.GetDouble("WeightAct");
            oFabricRnD.WeightCal = oReader.GetDouble("WeightCal");
            oFabricRnD.WeightDec = oReader.GetDouble("WeightDec");
            oFabricRnD.YarnFly = oReader.GetString("YarnFly");
            oFabricRnD.YarnQuality = oReader.GetString("YarnQuality");
            oFabricRnD.Note = oReader.GetString("Note");
            oFabricRnD.ForwardBy = oReader.GetInt16("ForwardBy");
            oFabricRnD.ForwardByName = oReader.GetString("ForwardByName");
            oFabricRnD.ForwardDate = oReader.GetDateTime("ForwardDate");
            oFabricRnD.ProductNameWarp = oReader.GetString("ProductNameWarp");
            oFabricRnD.ProductNameWeft = oReader.GetString("ProductNameWeft");
            //oFabricRnD.ProductIDWarpSuggest = oReader.GetInt32("ProductIDWarpSuggest");
            //oFabricRnD.ProductIDWeftSuggest = oReader.GetInt32("ProductIDWeftSuggest");
            oFabricRnD.ProductWarpRnd_Suggest = oReader.GetString("ProductWarpRnd_Suggest");
            //oFabricRnD.ProductNameWarpSuggest = oReader.GetString("ProductNameWarpSuggest");
            //oFabricRnD.ProductNameWeftSuggest = oReader.GetString("ProductNameWeftSuggest");
            oFabricRnD.FabricWeaveSuggest = oReader.GetInt32("FabricWeaveSuggest");
            oFabricRnD.FabricWeaveNameSuggest = oReader.GetString("FabricWeaveNameSuggest");

            oFabricRnD.WashType = oReader.GetInt32("WashType");
            oFabricRnD.ShadeType = (EnumFabricRndShade)oReader.GetInt32("ShadeType");
            oFabricRnD.CrimpWP = oReader.GetString("CrimpWP");
            oFabricRnD.CrimpWF = oReader.GetString("CrimpWF");
            oFabricRnD.Growth = oReader.GetString("Growth");
            oFabricRnD.Recovy = oReader.GetString("Recovy");
            oFabricRnD.Elongation = oReader.GetString("Elongation");
            oFabricRnD.SlubLengthWP = oReader.GetString("SlubLengthWP");
            oFabricRnD.PauseLengthWP = oReader.GetString("PauseLengthWP");
            oFabricRnD.SlubDiaWP = oReader.GetString("SlubDiaWP");
            oFabricRnD.SlubLengthWF = oReader.GetString("SlubLengthWF");
            oFabricRnD.PauseLengthWF = oReader.GetString("PauseLengthWF");
            oFabricRnD.SlubDiaWF = oReader.GetString("SlubDiaWF");
            oFabricRnD.WidthGrey = oReader.GetString("WidthGrey");

            return oFabricRnD;
        }
        private FabricRnD CreateObject(NullHandler oReader)
        {
            FabricRnD oFabricRnD = new FabricRnD();
            oFabricRnD = MapObject(oReader);
            return oFabricRnD;
        }
        private List<FabricRnD> CreateObjects(IDataReader oReader)
        {
            List<FabricRnD> oFabricRnD = new List<FabricRnD>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricRnD oItem = CreateObject(oHandler);
                oFabricRnD.Add(oItem);
            }
            return oFabricRnD;
        }

        #endregion

        #region Interface implementation 
        public FabricRnD Save(FabricRnD oFabricRnD, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricRnD.FabricRnDID <= 0)
                {
                    reader = FabricRnDDA.InsertUpdate(tc, oFabricRnD, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricRnDDA.InsertUpdate(tc, oFabricRnD, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRnD = new FabricRnD();
                    oFabricRnD = CreateObject(oReader);
                }
                reader.Close();
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricRnD = new FabricRnD();
                oFabricRnD.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricRnD;
        }
        public List<FabricRnD> SaveAll(List<FabricRnD> oFabricRnDs, Int64 nUserID)
        {

            FabricRnD oFabricRnD = new FabricRnD();
            List<FabricRnD> oFabricRnDs_Return = new List<FabricRnD>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (FabricRnD oItem in oFabricRnDs)
                {

                    if (oItem.FabricRnDID <= 0)
                    {
                        reader = FabricRnDDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FabricRnDDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricRnD = new FabricRnD();
                        oFabricRnD = CreateObject(oReader);
                        oFabricRnDs_Return.Add(oFabricRnD);
                    }
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricRnD = new FabricRnD();
                oFabricRnD.ErrorMessage = e.Message.Split('~')[0];
                oFabricRnDs_Return.Add(oFabricRnD);

                #endregion
            }
            return oFabricRnDs_Return;
        }
  

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricRnD oFabricRnD = new FabricRnD();
                oFabricRnD.FabricRnDID = id;
                DBTableReferenceDA.HasReference(tc, "FabricRnD", id);
                FabricRnDDA.Delete(tc, oFabricRnD, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
 
        public FabricRnD Get(int id, Int64 nUserId)
        {
            FabricRnD oFabricRnD = new FabricRnD();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricRnDDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRnD = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricRnD = new FabricRnD();
                oFabricRnD.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricRnD;
        }
        public FabricRnD GetBy(int nFSCDID, int nFabricID, Int64 nUserId)
        {
            FabricRnD oFabricRnD = new FabricRnD();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricRnDDA.GetBy(tc,  nFSCDID,  nFabricID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRnD = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricRnD = new FabricRnD();
                oFabricRnD.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricRnD;
        }
        public List<FabricRnD> Gets(Int64 nUserID)
        {
            List<FabricRnD> oFabricRnDs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricRnDDA.Gets(tc);
                oFabricRnDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricRnD oFabricRnD = new FabricRnD();
                oFabricRnD.ErrorMessage = e.Message.Split('!')[0];
                oFabricRnDs.Add(oFabricRnD);
                #endregion
            }
            return oFabricRnDs;
        }
        public List<FabricRnD> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricRnD> oFabricRnDs = new List<FabricRnD>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricRnDDA.Gets(tc, sSQL);
                oFabricRnDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricRnD oFabricRnD = new FabricRnD();
                oFabricRnD.ErrorMessage = e.Message.Split('!')[0];
                oFabricRnDs.Add(oFabricRnD);
                #endregion
            }
            return oFabricRnDs;
        }

        public FabricRnD StatusChange(int nFabricRnDId, Int64 nUserId)
        {
            FabricRnD oFabricRnD = new FabricRnD();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                FabricRnDDA.StatusChange(tc, nFabricRnDId, nUserId);
                IDataReader reader = FabricRnDDA.Get(tc, nFabricRnDId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRnD = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricRnD = new FabricRnD();
                oFabricRnD.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricRnD;
        }
        public List<FabricRnD> Approve(List<FabricRnD> oFabricRnDs, Int64 nUserID)
        {

            FabricRnD oFabricRnD = new FabricRnD();
            List<FabricRnD> oFabricRnDs_Return = new List<FabricRnD>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (FabricRnD oItem in oFabricRnDs)
                {
                    FabricRnDDA.StatusChange(tc, oItem.FabricRnDID, nUserID);
                    IDataReader reader = FabricRnDDA.Get(tc, oItem.FabricRnDID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricRnD = new FabricRnD();
                        oFabricRnD = CreateObject(oReader);
                        oFabricRnDs_Return.Add(oFabricRnD);
                    }
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricRnD = new FabricRnD();
                oFabricRnD.ErrorMessage = e.Message.Split('~')[0];
                oFabricRnDs_Return.Add(oFabricRnD);

                #endregion
            }
            return oFabricRnDs_Return;
        }
   
        #endregion
    }   
}