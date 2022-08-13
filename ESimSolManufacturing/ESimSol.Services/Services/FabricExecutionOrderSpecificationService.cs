using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
using System.Linq;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class FabricExecutionOrderSpecificationService : MarshalByRefObject, IFabricExecutionOrderSpecificationService
    {
        #region Private functions and declaration
        private FabricExecutionOrderSpecification MapObject(NullHandler oReader)
        {
            FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();

            oFEOS.FEOSID = oReader.GetInt32("FEOSID");
            oFEOS.FSCDID = oReader.GetInt32("FSCDID");
            oFEOS.ReferenceFSCDID = oReader.GetInt32("ReferenceFSCDID");
            oFEOS.FESONo = oReader.GetString("FESONo");

            oFEOS.Reed = oReader.GetDouble("Reed");
            oFEOS.ReedWidth = oReader.GetDouble("ReedWidth");
            oFEOS.Crimp = oReader.GetDouble("Crimp");
           
            oFEOS.GreigeFabricWidth = oReader.GetDouble("GreigeFabricWidth");
            oFEOS.Ends = oReader.GetDouble("Ends");
            oFEOS.Picks = oReader.GetDouble("Picks");
            oFEOS.FinishPick = oReader.GetDouble("FinishPick");
            oFEOS.FinishEnd = oReader.GetDouble("FinishEnd");

            oFEOS.GreigeDemand = oReader.GetDouble("GreigeDemand");
            oFEOS.RequiredWarpLength = oReader.GetDouble("RequiredWarpLength");
            oFEOS.GroundEnds = oReader.GetDouble("GroundEnds");
            oFEOS.WarpSet = oReader.GetInt32("WarpSet");
            oFEOS.SetLength = oReader.GetDouble("SetLength");
            oFEOS.EndsRepeat = oReader.GetDouble("EndsRepeat");
            oFEOS.RepeatSection = oReader.GetDouble("RepeatSection");
            oFEOS.SectionEnds = oReader.GetDouble("SectionEnds");
            oFEOS.NoOfSection = oReader.GetDouble("NoOfSection");
            oFEOS.PerConeLength = oReader.GetDouble("PerConeLength");
            oFEOS.PerConeDia = oReader.GetDouble("PerConeDia");
            oFEOS.ApproveBy = oReader.GetInt32("ApproveBy");
            oFEOS.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFEOS.ForwardDODate = oReader.GetDateTime("ForwardDODate");
            oFEOS.LengthSpecification = oReader.GetString("LengthSpecification");
            oFEOS.RefNote = oReader.GetString("RefNote");
            oFEOS.GrayTargetInPercent = oReader.GetDouble("GrayTargetInPercent");
            oFEOS.GrayWarpInPercent = oReader.GetDouble("GrayWarpInPercent");
            oFEOS.WeftColor = oReader.GetString("WeftColor");
            oFEOS.WarpColor = oReader.GetString("WarpColor");
            oFEOS.SelvedgeEnds = oReader.GetDouble("SelvedgeEnds");
            oFEOS.ReqLoomProduction = oReader.GetDouble("ReqLoomProduction");
            oFEOS.PrepareByName = oReader.GetString("PrepareByName");
            
            oFEOS.ApproveByName = oReader.GetString("ApproveByName");
            oFEOS.FabricSource = oReader.GetString("FabricSource");
            oFEOS.SCNoFull = oReader.GetString("SCNoFull");
            oFEOS.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFEOS.PINo = oReader.GetString("PINo");
            oFEOS.Construction = oReader.GetString("Construction");
            oFEOS.Composition = oReader.GetString("Composition");
            oFEOS.FinishWidth = oReader.GetDouble("FinishWidth");
            oFEOS.FinishWidthFS = oReader.GetString("FinishWidthFS");
            oFEOS.BuyerName = oReader.GetString("BuyerName");
            oFEOS.Weave = oReader.GetString("Weave");
            oFEOS.HLReference = oReader.GetString("HLReference");
            //oFEOS.FinishType = ((EnumFinishType)oReader.GetInt16("FinishType")).ToString();
            oFEOS.FinishType = oReader.GetString("FinishType");
            oFEOS.Qty = oReader.GetDouble("Qty");
            //oFEOS.RefSCNo = oReader.GetString("RefSCNo");
            oFEOS.FabricNo = oReader.GetString("FabricNo");
            oFEOS.FabricID = oReader.GetInt32("FabricID");
            oFEOS.FinishedDate = oReader.GetDateTime("FinishedDate");
            oFEOS.Dent = oReader.GetDouble("Dent");
            oFEOS.ReviseNo = oReader.GetInt32("ReviseNo");
            oFEOS.TotalEnds = oReader.GetDouble("TotalEnds");
            oFEOS.TotalEndsAdd = oReader.GetDouble("TotalEndsAdd");
            oFEOS.IsTEndsAdd = oReader.GetBoolean("IsTEndsAdd");
            //oFEOS.ReqLoomPP = oReader.GetDouble("ReqLoomPP");
            oFEOS.NoOfFrame = oReader.GetString("NoOfFrame");
            oFEOS.SelvedgeEndTwo = oReader.GetDouble("SelvedgeEndTwo");
            oFEOS.WarpLenAdd = oReader.GetDouble("WarpLenAdd");
            oFEOS.LoomPPAdd = oReader.GetDouble("LoomPPAdd");
            oFEOS.QtyExtraMet = oReader.GetDouble("QtyExtraMet");
            oFEOS.WarpCount = oReader.GetString("WarpCount");
            oFEOS.WeftCount = oReader.GetString("WeftCount");
            oFEOS.ExeNo = oReader.GetString("ExeNo");
            oFEOS.IsYD = oReader.GetBoolean("IsYD");
            oFEOS.ProdtionType = (EnumDispoProType)oReader.GetInt32("ProdtionType");
            oFEOS.ProdtionTypeInt = oReader.GetInt32("ProdtionType");
            oFEOS.IsSepBeam = oReader.GetBoolean("IsSepBeam");
            oFEOS.SEBeamType = (EnumBeamType)oReader.GetInt16("SEBeamType");
            oFEOS.FSpcType = (EnumFabricSpeType)oReader.GetInt16("FSpcType");
            oFEOS.TotalEndsUB = oReader.GetDouble("TotalEndsUB");
            oFEOS.TotalEndsLB = oReader.GetDouble("TotalEndsLB");
            oFEOS.ReqWarpLenLB = oReader.GetDouble("ReqWarpLenLB");
            oFEOS.IsOutSide = oReader.GetBoolean("IsOutSide");
            oFEOS.ContractorID = oReader.GetInt32("ContractorID");
            oFEOS.ForwardToDOby = oReader.GetInt32("ForwardToDOby");
            oFEOS.IssueDate = oReader.GetDateTime("IssueDate");
            //oFEOS.QtySub = oReader.GetDouble("QtySub");
            oFEOS.QtyOrder = oReader.GetDouble("QtyOrder");
            oFEOS.WarpWeftType = (EnumWarpWeft)oReader.GetInt16("WarpWeftType");
            oFEOS.RefNo = oReader.GetString("RefNo");
            
            return oFEOS;
        }
        public static FabricExecutionOrderSpecification CreateObject(NullHandler oReader)
        {
            FabricExecutionOrderSpecification oFabricExecutionOrderSpecification = new FabricExecutionOrderSpecification();
            FabricExecutionOrderSpecificationService oFEOSService = new FabricExecutionOrderSpecificationService();
            oFabricExecutionOrderSpecification = oFEOSService.MapObject(oReader);
            return oFabricExecutionOrderSpecification;
        }

        private List<FabricExecutionOrderSpecification> CreateObjects(IDataReader oReader)
        {
            List<FabricExecutionOrderSpecification> oFabricExecutionOrderSpecifications = new List<FabricExecutionOrderSpecification>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricExecutionOrderSpecification oItem = CreateObject(oHandler);
                oFabricExecutionOrderSpecifications.Add(oItem);
            }
            return oFabricExecutionOrderSpecifications;
        }
        #endregion

        #region Interface implementatio
        public FabricExecutionOrderSpecificationService() { }

        public FabricExecutionOrderSpecification IUD(FabricExecutionOrderSpecification oFEOS, int nDBOperation, Int64 nUserId)
        {
            List<FabricExecutionOrderSpecificationDetail> oFEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
            List<FabricQtyAllow> oFabricQtyAllows = new List<FabricQtyAllow>();
            List<FabricSpecificationNote> oFabricSpecificationNotes = new List<FabricSpecificationNote>();

            List<FabricExecutionOrderSpecificationDetail> oTemp = new List<FabricExecutionOrderSpecificationDetail>();
            oFabricQtyAllows = oFEOS.FabricQtyAllows;
            oFEOSDetails = oFEOS.FEOSDetails;
            oFabricSpecificationNotes = oFEOS.FabricSpecificationNotes; 
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (nDBOperation == (int)EnumDBOperation.Revise)
                {
                    reader = FabricExecutionOrderSpecificationDA.IUD_Log(tc, oFEOS, nDBOperation, nUserId);
                }
                else
                {
                    reader = FabricExecutionOrderSpecificationDA.IUD(tc, oFEOS, nDBOperation, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEOS = new FabricExecutionOrderSpecification();
                    oFEOS = CreateObject(oReader);
                }
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFEOS = new FabricExecutionOrderSpecification();
                    oFEOS.ErrorMessage = Global.DeleteMessage;
                }
                reader.Close();
                #region Detail Part
                if (nDBOperation != (int)EnumDBOperation.Delete && nDBOperation != (int)EnumDBOperation.Revise && nDBOperation != (int)EnumDBOperation.Approval)
                {
                    if (oFEOSDetails != null)
                    {
                        int nCount = 0;
                        double nNoOfFractionRem = 0;
                        int nWarpEnds =  oFEOSDetails.Where(x => (x.IsWarp == true && x.TwistedGroup<=0 )).Sum(x => x.EndsCount);

                      //  nWarpEnds = nWarpEnds + oFEOSDetails.Where(x => (x.IsWarp == true && x.TwistedGroup > 0)).ToList().Select(k => k.EndsCount).Distinct().Sum();

                        oTemp = oFEOSDetails.Where(x => x.IsWarp == true && x.TwistedGroup > 0).ToList();
                        oTemp = oTemp.GroupBy(x => new { x.TwistedGroup, x.EndsCount }, (key, grp) =>  new FabricExecutionOrderSpecificationDetail  {  TwistedGroup = key.TwistedGroup,    EndsCount = key.EndsCount  }).ToList();
                        if (oTemp.Count > 0) {  nWarpEnds = nWarpEnds + oTemp.Sum(x => x.EndsCount); }

                        oFEOS.SectionEnds = nWarpEnds * oFEOS.RepeatSection;
                        double nTotalEnds =0;
                        if (oFEOS.IsTEndsAdd)
                        { nTotalEnds = oFEOS.TotalEnds + oFEOS.TotalEndsAdd;}
                        else { nTotalEnds = (oFEOS.TotalEnds - oFEOS.TotalEndsAdd); }
                        nNoOfFractionRem = nTotalEnds % oFEOS.SectionEnds;

                        int nTotalend = oFEOSDetails.Where(x => (x.IsWarp == false && x.TwistedGroup <= 0)).Sum(x => x.EndsCount);
                       // nTotalend = nTotalend + oFEOSDetails.Where(x => (x.IsWarp == false && x.TwistedGroup > 0)).ToList().Select(k => k.TwistedGroup ).Distinct().ToList();

                        oTemp = new List<FabricExecutionOrderSpecificationDetail>();
                        oTemp = oFEOSDetails.Where(x => x.IsWarp == false && x.TwistedGroup>0).ToList();
                         oTemp = oTemp.GroupBy(x => new { x.TwistedGroup, x.EndsCount }, (key, grp) =>
                            new FabricExecutionOrderSpecificationDetail
                            {
                                TwistedGroup = key.TwistedGroup,
                                EndsCount = key.EndsCount

                            }).ToList();
                         if (oTemp.Count>0)    {      nTotalend = nTotalend + oTemp.Sum(x => x.EndsCount);     }

                        double nRequiredWarpLength = 0;
                        nRequiredWarpLength = oFEOS.RequiredWarpLength;
                        foreach (FabricExecutionOrderSpecificationDetail oItem in oFEOSDetails)
                        {
                            //nSLNo++;
                            if (oFEOS.FSpcType == EnumFabricSpeType.SeerSucker)
                            {
                                if(oItem.BeamType==EnumBeamType.LowerBeam)
                                {
                                    nRequiredWarpLength = oFEOS.ReqWarpLenLB;
                                }
                                else
                                {
                                    nRequiredWarpLength = oFEOS.RequiredWarpLength;
                                }
                            }

                            if (oItem.Value < 30 && oItem.ValueMin <= 0)
                            { oItem.ValueMin =0.5; }


                            //oItem.SLNo = nSLNo;
                            if (oItem.ProductID > 0)
                            {
                                IDataReader readerdetail;
                                oItem.FEOSID = oFEOS.FEOSID;
                                if (nNoOfFractionRem > 0 || oFEOS.FSpcType == EnumFabricSpeType.SeerSucker)
                                {
                                    if (oItem.TotalEndActual <= 0)
                                    {
                                        oItem.TotalEndActual = (oFEOS.RepeatSection * oItem.EndsCount * oFEOS.NoOfSection);
                                    }
                                }
                                else
                                {
                                    oItem.TotalEndActual = oItem.TotalEnd;
                                }
                                if (oFEOS.ProdtionType != EnumDispoProType.ExcessDyeingQty)
                                {
                                    if (oItem.IsWarp == true)
                                    {
                                        if (oFEOS.RepeatSection > 0 && oItem.EndsCount > 0 && oItem.Value > 0)
                                        {
                                            if (oItem.FEOSDID <= 0 && oFabricQtyAllows.Count > 0)
                                            {
                                                oItem.AllowanceWarp = 0;
                                                nCount = oFEOSDetails.Where(x => (x.ColorName == oItem.ColorName && x.LabdipDetailID == oItem.LabdipDetailID && x.Value == oItem.Value && x.ProductID == oItem.ProductID)).ToList().Count;
                                                oItem.Qty = ((oItem.TotalEndActual * Math.Round(Math.Ceiling(nRequiredWarpLength)) / (oItem.Value - oItem.ValueMin)) / (((100 - oItem.AllowanceWarp) / 100))) * 0.0005905;
                                                if (nCount < 2)
                                                {
                                                    if (oFabricQtyAllows.FirstOrDefault() != null && oFabricQtyAllows.FirstOrDefault().Qty_From > 0 && oFabricQtyAllows.Where(b => b.Qty_From <=oItem.Qty && b.Qty_To > oItem.Qty && b.AllowType == EnumFabricQtyAllowType.WarpnWeft && b.WarpWeftType == EnumWarpWeft.Warp).Count() > 0)
                                                    {
                                                        oItem.AllowanceWarp = oFabricQtyAllows.Where(x => (x.Qty_From <= oItem.Qty && x.Qty_To > oItem.Qty && x.AllowType == EnumFabricQtyAllowType.WarpnWeft && x.WarpWeftType == EnumWarpWeft.Warp)).First().Percentage;
                                                    }
                                                }
                                                else
                                                {
                                                    if (oFabricQtyAllows.FirstOrDefault() != null && oFabricQtyAllows.FirstOrDefault().Qty_From > 0 && oFabricQtyAllows.Where(b => b.Qty_From <= oItem.Qty && b.Qty_To > oItem.Qty && b.AllowType == EnumFabricQtyAllowType.WarpnWeft && b.WarpWeftType == EnumWarpWeft.WarpnWeft).Count() > 0)
                                                    {
                                                        oItem.AllowanceWarp = oFabricQtyAllows.Where(x => (x.Qty_From <= oItem.Qty && x.Qty_To > oItem.Qty && x.AllowType == EnumFabricQtyAllowType.WarpnWeft && x.WarpWeftType == EnumWarpWeft.WarpnWeft)).First().Percentage;
                                                    }
                                                }
                                            }

                                            oItem.Qty = ((oItem.TotalEndActual * Math.Round(Math.Ceiling(nRequiredWarpLength)) / (oItem.Value - oItem.ValueMin)) / (((100 - oItem.AllowanceWarp) / 100))) * 0.0005905;
                                            if (oItem.Qty > 0)
                                            {
                                                oItem.Length = (Math.Round(oItem.Qty, 2) * 2.2046 * 840 * oItem.Value * 0.9144) / (oFEOS.RepeatSection * oItem.EndsCount);
                                            }
                                            oItem.Qty = Math.Round(oItem.Qty, 2);
                                            oItem.Length = Math.Round(oItem.Length, 2);
                                        }
                                    }
                                    else
                                    {
                                        oItem.TotalEndActual = oItem.TotalEnd;
                                        if (oFEOS.RepeatSection > 0 && oItem.EndsCount > 0)
                                        {
                                            oItem.Qty = (((Math.Round(oFEOS.ReedWidth, 2) + oFEOS.Crimp) * oFEOS.Picks * Math.Round(Math.Ceiling(oFEOS.ReqLoomProduction))) / (oItem.Value - oItem.ValueMin)) * 0.0005905;
                                            if (oItem.AllowanceWarp > 0)
                                            { oItem.Qty = ((oItem.Qty * oItem.EndsCount) / nTotalend) / ((100 - oItem.AllowanceWarp) / 100); }
                                            else
                                            {
                                                oItem.Qty = (oItem.Qty * oItem.EndsCount) / nTotalend;
                                            }
                                            oItem.Qty = Math.Round(oItem.Qty, 2);
                                            oItem.Length = 0;
                                        }
                                    }
                                }
                                if (oItem.FEOSDID <= 0)
                                {
                                    readerdetail = FabricExecutionOrderSpecificationDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserId);
                                }
                                else
                                {
                                    readerdetail = FabricExecutionOrderSpecificationDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserId);
                                }
                                NullHandler oReaderDetail = new NullHandler(readerdetail);
                                //if (readerdetail.Read())
                                //{
                                //    sDetailIDs = sDetailIDs + oReaderDetail.GetString("ExportPIDetailID") + ",";
                                //}
                                readerdetail.Close();
                            }
                        }
                    }
                    if (oFabricSpecificationNotes != null)
                    {
                        if (oFabricSpecificationNotes.Count > 0)
                        {
                            string sFabricSpecificationNoteID = "";
                            foreach (FabricSpecificationNote oItem in oFabricSpecificationNotes)
                            {
                                IDataReader readertnc;
                                oItem.FEOSID = oFEOS.FEOSID;
                                if (oItem.FabricSpecificationNoteID <= 0)
                                {
                                    readertnc = FabricSpecificationNoteDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                                }
                                else
                                {
                                    readertnc = FabricSpecificationNoteDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                                }
                                NullHandler oReaderTNC = new NullHandler(readertnc);
                                if (readertnc.Read())
                                {
                                    sFabricSpecificationNoteID = sFabricSpecificationNoteID + oReaderTNC.GetString("FabricSpecificationNoteID") + ",";
                                }
                                readertnc.Close();
                            }
                            if (sFabricSpecificationNoteID.Length > 0)
                            {
                                sFabricSpecificationNoteID = sFabricSpecificationNoteID.Remove(sFabricSpecificationNoteID.Length - 1, 1);
                            }
                            if (!string.IsNullOrEmpty(sFabricSpecificationNoteID))
                            {
                                FabricSpecificationNoteDA.DeleteAll(tc, oFEOS.FEOSID, sFabricSpecificationNoteID);
                            }
                        }
                    }
                }
                //if (sDetailIDs.Length > 0)
                //{
                //    sDetailIDs = sDetailIDs.Remove(sDetailIDs.Length - 1, 1);
                //}
                //oExportPIDetail = new ExportPIDetail();
                //oExportPIDetail.ExportPIID = oExportPI.ExportPIID;
                //ExportPIDetailDA.Delete(tc, oExportPIDetail, EnumDBOperation.Delete, nUserID, sDetailIDs);
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            return oFEOS;
        }
        public FabricExecutionOrderSpecification UpdateOutSide(FabricExecutionOrderSpecification oFESDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricExecutionOrderSpecificationDA.UpdateOutSide(tc, oFESDetail);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFESDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFESDetail = new FabricExecutionOrderSpecification();
                oFESDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFESDetail;
        }
        public FabricExecutionOrderSpecification IUD_DO(FabricExecutionOrderSpecification oFEOS,  Int64 nUserId)
        {
            List<FabricExecutionOrderSpecificationDetail> oFEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
            oFEOSDetails = oFEOS.FEOSDetails;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                
                //foreach (FabricExecutionOrderSpecificationDetail oItem in oFEOSDetails)
                //{
                //    if (oItem.IsWarp == true)
                //    {
                //        oItem.Qty = (oFEOS.RepeatSection * oItem.EndsCount * oFEOS.NoOfSection * oFEOS.RequiredWarpLength * 0.0005905 / (oItem.Value - oItem.ValueMin)) / (((100 - oFEOS.AllowancePercent) / 100));
                //        oItem.Length = (Math.Round(oItem.Qty, 2) * 2.2046 * 840 * oItem.Value * 0.9144) / (oFEOS.RepeatSection * oItem.EndsCount);
                //        oItem.Qty = Math.Round(oItem.Qty, 2);
                //        oItem.Length = Math.Round(oItem.Length, 2);
                //    }
                //    else
                //    {
                //        oItem.Qty = (((Math.Round(oFEOS.ReedWidth) + 3.5) * oFEOS.Picks * Math.Round(oFEOS.ReqLoomProduction, 2) * 0.0005905) / (oItem.Value - oItem.ValueMin));
                //        oItem.Qty = (oItem.Qty * oItem.EndsCount) / nTotalend;
                //        oItem.Qty = Math.Round(oItem.Qty, 2);
                //        oItem.Length = 0;
                //    }
                //    FabricExecutionOrderSpecificationDetailDA.UpdateQty(tc, oItem);
                //}

                //foreach (FabricExecutionOrderSpecificationDetail oItem in oFEOSDetails)
                //{
                //    FabricExecutionOrderSpecificationDetailDA.UpdateQty(tc, oItem);
                //}

                reader = FabricExecutionOrderSpecificationDA.IUD_DO(tc, oFEOS, nUserId);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEOS = new FabricExecutionOrderSpecification();
                    oFEOS = CreateObject(oReader);
                }
              
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            return oFEOS;
        }
        public FabricExecutionOrderSpecification Get(int nFSCDID, Int64 nUserId)
        {
            FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricExecutionOrderSpecificationDA.Get(tc, nFSCDID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEOS = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = "Failed to get information.";
                #endregion
            }

            return oFEOS;
        }
        public List<FabricExecutionOrderSpecification> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricExecutionOrderSpecification> oFEOSs = new List<FabricExecutionOrderSpecification>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderSpecificationDA.Gets(tc, sSQL, nUserId);
                oFEOSs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEOSs = new List<FabricExecutionOrderSpecification>();
                #endregion
            }

            return oFEOSs;
        }
        #endregion
    }
}