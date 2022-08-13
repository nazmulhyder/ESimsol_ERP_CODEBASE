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
    public class FabricService : MarshalByRefObject, IFabricService
    {
        #region Private functions and declaration
        private Fabric MapObject(NullHandler oReader)
        {
            Fabric oFabric = new Fabric();
            oFabric.FabricID = oReader.GetInt32("FabricID");
            oFabric.FabricNo = oReader.GetString("FabricNo");
            oFabric.FabricNum = oReader.GetString("FabricNum");
            
            oFabric.BuyerReference = oReader.GetString("BuyerReference");
            oFabric.IssueDate = oReader.GetDateTime("IssueDate");
            oFabric.BuyerID = oReader.GetInt32("BuyerID");
            oFabric.ProductID = oReader.GetInt32("ProductID");
            oFabric.MKTPersonID = oReader.GetInt32("MKTPersonID");
            oFabric.Construction = oReader.GetString("Construction");
            oFabric.ConstructionPI = oReader.GetString("ConstructionPI");
            oFabric.StyleNo = oReader.GetString("StyleNo");
            oFabric.ColorInfo = oReader.GetString("ColorInfo");
            oFabric.FabricWidth = oReader.GetString("FabricWidth");
            oFabric.IsWash = oReader.GetBoolean("IsWash");
            oFabric.IsFinish = oReader.GetBoolean("IsFinish");
            oFabric.IsDyeing = oReader.GetBoolean("IsDyeing");
            oFabric.IsPrint = oReader.GetBoolean("IsPrint");
            oFabric.FinishType = oReader.GetInt32("FinishType");
            oFabric.FinishTypeSugg = oReader.GetInt32("FinishTypeSugg");
            oFabric.FinishTypeName = oReader.GetString("FinishTypeName");
            oFabric.FinishTypeNameSugg = oReader.GetString("FinishTypeNameSugg");
            oFabric.FabricDesignID = oReader.GetInt32("FabricDesignID");

            oFabric.HandLoomNo = oReader.GetString("HandLoomNo");
            oFabric.PrimaryLightSourceID = oReader.GetInt32("PrimaryLightSourceID");
            oFabric.SecondaryLightSourceID = oReader.GetInt32("SecondaryLightSourceID");
            oFabric.EndUse = oReader.GetString("EndUse");
            oFabric.OptionNo = oReader.GetString("OptionNo");
            oFabric.NoOfFrame = oReader.GetInt32("NoOfFrame");
            oFabric.WeftColor = oReader.GetString("WeftColor");
            oFabric.ActualConstruction = oReader.GetString("ActualConstruction");
            oFabric.WarpCount = oReader.GetString("WarpCount");
            oFabric.WeftCount = oReader.GetString("WeftCount");
            oFabric.EPI = oReader.GetString("EPI");
            oFabric.PPI = oReader.GetString("PPI");

            oFabric.FabricDesignName = oReader.GetString("FabricDesignName");
            oFabric.Remarks = oReader.GetString("Remarks");
            oFabric.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oFabric.BuyerName = oReader.GetString("BuyerName");
            oFabric.ProductName = oReader.GetString("ProductName");
            oFabric.ProductNameWeft = oReader.GetString("ProductNameWeft");
            oFabric.MKTPersonName = oReader.GetString("MKTPersonName");
            oFabric.ApprovedByName = oReader.GetString("ApprovedByName");
            oFabric.PriorityLevel = (EnumPriorityLevel)oReader.GetInt32("PriorityLevel");
            oFabric.PriorityLevelInInt = oReader.GetInt32("PriorityLevel");
            oFabric.SeekingSubmissionDate = oReader.GetDateTime("SeekingSubmissionDate");
            oFabric.SubmissionDate = oReader.GetDateTime("SubmissionDate");
            oFabric.MUID = oReader.GetInt32("MUID");
            //oFabric.MUName = oReader.GetString("MUName");
            //oFabric.TotalPattern = oReader.GetInt32("TotalPattern");
            //oFabric.TotalLabDipRequest = oReader.GetInt32("TotalLabDipRequest");
            //oFabric.TotalLabDipDone = oReader.GetInt32("TotalLabDipDone");
            oFabric.ApprovedByDate = oReader.GetDateTime("ApprovedByDate");
            oFabric.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oFabric.ProcessType = oReader.GetInt32("ProcessType");
            oFabric.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFabric.FabricWeave = oReader.GetInt32("FabricWeave");
            oFabric.FabricWeaveName = oReader.GetString("FabricWeaveName");
            //oFabric.FabricAttachmentCount = oReader.GetInt32("FabricAttachmentCount");
            oFabric.PrepareByName = oReader.GetString("PrepareByName");
            oFabric.AttCount =( oReader.GetInt32("AttCount")).ToString();
            oFabric.PrimaryLightSource = oReader.GetString("PrimaryLightSource");
            oFabric.SecondaryLightSource = oReader.GetString("SecondaryLightSource");
            oFabric.FabricOrderType = (EnumFabricRequestType)oReader.GetInt32("FabricOrderType");
            oFabric.FabricOrderTypeInt = oReader.GetInt32("FabricOrderType");
            oFabric.ProductIDWeft = oReader.GetInt32("ProductIDWeft");
            oFabric.WeightAct = oReader.GetDouble("WeightAct");
            oFabric.WeightCal = oReader.GetDouble("WeightCal");
            oFabric.WeightDec = oReader.GetDouble("WeightDec");
            oFabric.NoteRnD = oReader.GetString("NoteRnD");
            oFabric.Code = oReader.GetString("Code");
            oFabric.CurrencyID = oReader.GetInt32("CurrencyID");
            oFabric.CurrencyName = oReader.GetString("CurrencyName");
            oFabric.Price = oReader.GetDouble("Price");
            oFabric.Symbol = oReader.GetString("Symbol");


            return oFabric;
        }
        private Fabric CreateObject(NullHandler oReader)
        {
            Fabric oFabric = new Fabric();
            oFabric = MapObject(oReader);
            return oFabric;
        }
        private List<Fabric> CreateObjects(IDataReader oReader)
        {
            List<Fabric> oFabric = new List<Fabric>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Fabric oItem = CreateObject(oHandler);
                oFabric.Add(oItem);
            }
            return oFabric;
        }

        #endregion

        #region Interface implementation 
        public Fabric Save(Fabric oFabric, Int64 nUserID)
        {
            List<FabricSeekingDate> oFabricSeekingDates = oFabric.FabricSeekingDates; 

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabric.FabricID <= 0)
                {
                    reader = FabricDA.InsertUpdate(tc, oFabric, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricDA.InsertUpdate(tc, oFabric, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabric = new Fabric();
                    oFabric = CreateObject(oReader);
                }
                reader.Close();
                #region FabricSeekingDates
                if(oFabricSeekingDates!=null)
                {
                    FabricSeekingDate oFabricSeekingDate = new FabricSeekingDate();
                    oFabricSeekingDate.FabricID = oFabric.FabricID;
                    FabricSeekingDateDA.Delete(tc, oFabricSeekingDate, EnumDBOperation.Delete, nUserID);
                    foreach (FabricSeekingDate oItem in oFabricSeekingDates)
                    {
                        IDataReader readerdetail;
                        oItem.FabricID = oFabric.FabricID;
                    
                        readerdetail = FabricSeekingDateDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                  
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        readerdetail.Close();
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
                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabric;
        }
        public List<Fabric> SaveAll(List<Fabric> oFabrics, Int64 nUserID)
        {

            Fabric oFabric = new Fabric();
            List<Fabric> oFabrics_Return = new List<Fabric>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (Fabric oItem in oFabrics)
                {

                    if (oItem.FabricID <= 0)
                    {
                        reader = FabricDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FabricDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabric = new Fabric();
                        oFabric = CreateObject(oReader);
                        oFabrics_Return.Add(oFabric);
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

                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('~')[0];
                oFabrics_Return.Add(oFabric);

                #endregion
            }
            return oFabrics_Return;
        }
        public Fabric SaveReceiveDate(Fabric oFabric, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if(string.IsNullOrEmpty(oFabric.HandLoomNo))
                    throw new Exception("Hand loom no required.");
                if(FabricDA.HasHandLoomNo(tc, oFabric.FabricID, oFabric.HandLoomNo))
                    throw new Exception("Already this hand loom no exists.");

                IDataReader reader;
                FabricDA.SaveReceiveDate(tc, oFabric, nUserID);
                reader = FabricDA.Get(tc, oFabric.FabricID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabric = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabric;
        }
        public Fabric SaveSubmissionDate(Fabric oFabric, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                FabricDA.SaveSubmissionDate(tc, oFabric, nUserID);
                reader = FabricDA.Get(tc, oFabric.FabricID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabric = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabric;
        }
        public Fabric SaveSubmission(Fabric oFabric, Int64 nUserID)
        {
            List<FabricSeekingDate> oFabricSeekingDates = oFabric.FabricSeekingDates;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                FabricDA.SaveSubmission(tc, oFabric, nUserID);
                reader = FabricDA.Get(tc, oFabric.FabricID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabric = new Fabric();
                    oFabric = CreateObject(oReader);
                }
                reader.Close();
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabric;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Fabric oFabric = new Fabric();
                oFabric.FabricID = id;
                DBTableReferenceDA.HasReference(tc, "Fabric", id);
                FabricDA.Delete(tc, oFabric, EnumDBOperation.Delete, nUserId);
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
        public Fabric ReFabricSubmission(Fabric oFabric, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                var oFabricSeekingDates = oFabric.FabricSeekingDates;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricDA.ReFabricSubmission(tc, oFabric, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabric = new Fabric();
                    oFabric = CreateObject(oReader);
                }
                reader.Close();

                #region FabricSeekingDates
                if (oFabricSeekingDates != null)
                {
                    FabricSeekingDate oFabricSeekingDate = new FabricSeekingDate();
                    oFabricSeekingDate.FabricID = oFabric.FabricID;
                    FabricSeekingDateDA.Delete(tc, oFabricSeekingDate, EnumDBOperation.Delete, nUserID);
                    foreach (FabricSeekingDate oItem in oFabricSeekingDates)
                    {
                        IDataReader readerdetail;
                        oItem.FabricID = oFabric.FabricID;

                        readerdetail = FabricSeekingDateDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);

                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        readerdetail.Close();
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
                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabric;
        }
        public Fabric Get(int id, Int64 nUserId)
        {
            Fabric oFabric = new Fabric();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabric = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabric;
        }
        public List<Fabric> Gets(Int64 nUserID)
        {
            List<Fabric> oFabrics = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDA.Gets(tc);
                oFabrics = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                Fabric oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('!')[0];
                oFabrics.Add(oFabric);
                #endregion
            }
            return oFabrics;
        }
        public List<Fabric> Gets(string sSQL, Int64 nUserID)
        {
            List<Fabric> oFabrics = new List<Fabric>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricDA.Gets(tc, sSQL);
                oFabrics = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                Fabric oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('!')[0];
                oFabrics.Add(oFabric);
                #endregion
            }
            return oFabrics;
        }

        public Fabric StatusChange(int nFabricId, Int64 nUserId)
        {
            Fabric oFabric = new Fabric();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                FabricDA.StatusChange(tc, nFabricId, nUserId);
                IDataReader reader = FabricDA.Get(tc, nFabricId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabric = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabric;
        }
        public List<Fabric> Approve(List<Fabric> oFabrics, Int64 nUserID)
        {

            Fabric oFabric = new Fabric();
            List<Fabric> oFabrics_Return = new List<Fabric>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (Fabric oItem in oFabrics)
                {
                    FabricDA.StatusChange(tc, oItem.FabricID, nUserID);
                    IDataReader reader = FabricDA.Get(tc, oItem.FabricID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabric = new Fabric();
                        oFabric = CreateObject(oReader);
                        oFabrics_Return.Add(oFabric);
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

                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('~')[0];
                oFabrics_Return.Add(oFabric);

                #endregion
            }
            return oFabrics_Return;
        }
        public List<Fabric> Received(List<Fabric> oFabrics, Int64 nUserID)
        {
            Fabric oFabric = new Fabric();
            List<Fabric> oFabrics_Return = new List<Fabric>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (Fabric oItem in oFabrics)
                {
                    //if (string.IsNullOrEmpty(oItem.HandLoomNo)) { oItem.HandLoomNo = ""; }
                    FNLabDipDetailDA.Save_LDNo_FromFabric(tc, oItem, nUserID);
                    //IDataReader reader = FabricDA.Received(tc, oItem, nUserID);
                    IDataReader reader = FabricDA.Get(tc, oItem.FabricID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabric = new Fabric();
                        oFabric = CreateObject(oReader);
                        oFabrics_Return.Add(oFabric);
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
                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('~')[0];
                oFabrics_Return.Add(oFabric);

                #endregion
            }
            return oFabrics_Return;
        }
        public List<Fabric> Submission(List<Fabric> oFabrics, Int64 nUserID)
        {
            Fabric oFabric = new Fabric();
            List<Fabric> oFabrics_Return = new List<Fabric>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (Fabric oItem in oFabrics)
                {
                    FabricDA.SaveSubmissionDate(tc, oItem, nUserID);
                    //IDataReader reader = FabricDA.Received(tc, oItem, nUserID);
                    IDataReader reader = FabricDA.Get(tc, oItem.FabricID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabric = new Fabric();
                        oFabric = CreateObject(oReader);
                        oFabrics_Return.Add(oFabric);
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
                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('~')[0];
                oFabrics_Return.Add(oFabric);

                #endregion
            }
            return oFabrics_Return;
        }
        public Fabric SaveReceived(Fabric oFabric, Int64 nUserID)
        {
            List<FabricSeekingDate> oFabricSeekingDates = oFabric.FabricSeekingDates;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
               reader = FabricDA.InsertUpdate(tc, oFabric, EnumDBOperation.Receive, nUserID);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabric = new Fabric();
                    oFabric = CreateObject(oReader);
                }
                reader.Close();
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabric = new Fabric();
                oFabric.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabric;
        }
        #endregion
    }   
}