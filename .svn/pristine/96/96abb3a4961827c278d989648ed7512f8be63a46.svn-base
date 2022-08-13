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
    [Serializable]
    public class FabricPatternService : MarshalByRefObject, IFabricPatternService
    {
        #region Private functions and declaration
        private FabricPattern MapObject(NullHandler oReader)
        {
            FabricPattern oFP = new FabricPattern();
            oFP.FPID = oReader.GetInt32("FPID");
            oFP.PatternNo = oReader.GetString("PatternNo");
            oFP.FabricID = oReader.GetInt32("FabricID");
            oFP.Weave = oReader.GetInt32("Weave");
            oFP.Reed = oReader.GetInt32("Reed");
            oFP.Pick = oReader.GetInt32("Pick");
            oFP.GSM = oReader.GetDouble("GSM");
            oFP.Warp = oReader.GetInt32("Warp");
            oFP.Weft = oReader.GetInt32("Weft");
            oFP.Dent = oReader.GetDouble("Dent");
            oFP.Ratio = oReader.GetString("Ratio");
            oFP.RepeatSize = oReader.GetString("RepeatSize");
            oFP.ApproveBy = oReader.GetInt32("ApproveBy");
            oFP.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFP.IsActive = oReader.GetBoolean("IsActive");
            oFP.Note = oReader.GetString("Note");
            oFP.ApproveByName = oReader.GetString("ApproveByName");
            oFP.FabricNo = oReader.GetString("FabricNo");
            oFP.Construction = oReader.GetString("Construction");
            oFP.BuyerName = oReader.GetString("BuyerName");
            oFP.FabricWeave = oReader.GetString("FabricWeave");
            oFP.WeaveName = oReader.GetString("WeaveName");
            oFP.StyleNo = oReader.GetString("StyleNo");
            oFP.FabricDesignName = oReader.GetString("FabricDesignName");
            return oFP;
        }
        private FabricPattern CreateObject(NullHandler oReader)
        {
            FabricPattern oFabricPattern = new FabricPattern();
            oFabricPattern = MapObject(oReader);
            return oFabricPattern;
        }
        private List<FabricPattern> CreateObjects(IDataReader oReader)
        {
            List<FabricPattern> oFabricPatterns = new List<FabricPattern>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricPattern oItem = CreateObject(oHandler);
                oFabricPatterns.Add(oItem);
            }
            return oFabricPatterns;
        }
        #endregion

        #region Interface implementatio
        public FabricPatternService() { }

        public FabricPattern IUD(FabricPattern oFP, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = FabricPatternDA.IUD(tc, oFP, nDBOperation, nUserId);
              
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFP = new FabricPattern();
                    oFP = CreateObject(oReader);
                }
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFP = new FabricPattern();
                    oFP.ErrorMessage = Global.DeleteMessage;
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFP = new FabricPattern();
                oFP.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            return oFP;
        }
        public FabricPattern Save(FabricPattern oFP, Int64 nUserId)
        {
            List<FabricPatternDetail> oFabricPatternDetails = new List<FabricPatternDetail>();
            oFabricPatternDetails = oFP.FabricPatternDetails;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = FabricPatternDA.IUD(tc, oFP, (int)EnumDBOperation.Insert, nUserId);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFP = new FabricPattern();
                    oFP = CreateObject(oReader);
                }
               
                reader.Close();
                #region Details Part
                if (oFabricPatternDetails != null)
                {
                    foreach (FabricPatternDetail oItem in oFabricPatternDetails)
                    {
                        IDataReader readertnc;
                        oItem.FPID = oFP.FPID;
                        if (oItem.FPDID <= 0)
                        {
                            readertnc = FabricPatternDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserId);
                        }
                        else
                        {
                            readertnc = FabricPatternDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserId);
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        //if (readertnc.Read())
                        //{
                        //    sPaymentDetaillIDs = sPaymentDetaillIDs + oReaderTNC.GetString("PaymentDetailID") + ",";
                        //}
                        readertnc.Close();
                    }
             
                }


                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFP = new FabricPattern();
                oFP.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            return oFP;
        }
        public FabricPattern Get(int nFPID, Int64 nUserId)
        {
            FabricPattern oFP = new FabricPattern();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricPatternDA.Get(tc, nFPID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFP = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)  tc.HandleError();
                oFP = new FabricPattern();
                oFP.ErrorMessage = "Failed to get information." ;
                #endregion
            }

            return oFP;
        }
        public FabricPattern GetActiveFP(int nFabricId, Int64 nUserId)
        {
            FabricPattern oFP = new FabricPattern();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricPatternDA.GetActiveFP(tc, nFabricId, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFP = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)  tc.HandleError();
                oFP = new FabricPattern();
                oFP.ErrorMessage = "Failed to get information." ;
                #endregion
            }

            return oFP;
        }
        public List<FabricPattern> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricPattern> oFPs = new List<FabricPattern>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricPatternDA.Gets(tc, sSQL, nUserId);
                oFPs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFPs = new List<FabricPattern>();
                #endregion
            }

            return oFPs;
        }
        public FabricPattern Copy(FabricPattern oFP, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                List<FabricPatternDetail> oFPDetails = new List<FabricPatternDetail>();
                oFPDetails = oFP.FabricPatternDetails;

                #region Insert Fabric Pattern

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oFP.FPID = 0;
                reader = FabricPatternDA.IUD(tc, oFP, (int)EnumDBOperation.Insert, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFP = new FabricPattern();
                    oFP = CreateObject(oReader);
                }
                reader.Close();

                #endregion

                #region Insert Fabric Pattern Detail
                if (oFPDetails.Count > 0)
                {
                    FabricPatternDetail oFPD = new FabricPatternDetail();
                    IDataReader readerFPD;
                    NullHandler oReaderFPD;
                    foreach (FabricPatternDetail oItem in oFPDetails)
                    {
                        oItem.FPID = oFP.FPID;
                        readerFPD = FabricPatternDetailDA.IUD(tc, oItem, 999, nUserId); // 999 = Copy Pattern detail

                        oReaderFPD = new NullHandler(readerFPD);
                        if (readerFPD.Read())
                        {
                            FabricPatternDetailService oFPDS = new FabricPatternDetailService();
                            oFPD = new FabricPatternDetail();
                            oFPD = oFPDS.CreateObject(oReaderFPD);
                        }
                        readerFPD.Close();
                        oFP.FabricPatternDetails.Add(oFPD);
                    }
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFP = new FabricPattern();
                oFP.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFP;
        }
        public FabricPattern SaveSequence(FabricPattern oFP, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oFP.FabricPatternDetails != null)
                {
                    foreach (FabricPatternDetail oItem in oFP.FabricPatternDetails)
                    {
                        if (oItem.FPDID > 0 && oItem.SLNo > 0)
                        {
                            FabricPatternDetailDA.UpdateSequence(tc, oItem);
                        }
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFP = new FabricPattern();
                oFP.ErrorMessage = e.Message;
                #endregion
            }
            return oFP;
        }
        #endregion
    }
}