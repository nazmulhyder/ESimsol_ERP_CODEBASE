using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class BillOfMaterialService : MarshalByRefObject, IBillOfMaterialService
    {
        #region Private functions and declaration
        private BillOfMaterial MapObject(NullHandler oReader)
        {
            BillOfMaterial oBillOfMaterial = new BillOfMaterial();
            oBillOfMaterial.BillOfMaterialID = oReader.GetInt32("BillOfMaterialID");
            oBillOfMaterial.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oBillOfMaterial.ProductID = oReader.GetInt32("ProductID");
            oBillOfMaterial.ColorID = oReader.GetInt32("ColorID");
            oBillOfMaterial.SizeID = oReader.GetInt32("SizeID");
            oBillOfMaterial.ItemDescription = oReader.GetString("ItemDescription");
            oBillOfMaterial.Reference = oReader.GetString("Reference");
            oBillOfMaterial.Construction = oReader.GetString("Construction");
            oBillOfMaterial.Sequence = oReader.GetInt32("Sequence");
            oBillOfMaterial.MUnitID = oReader.GetInt32("MUnitID");
            oBillOfMaterial.ReqQty = oReader.GetDouble("ReqQty");
            oBillOfMaterial.CuttingQty = oReader.GetDouble("CuttingQty");
            oBillOfMaterial.ConsumptionQty = oReader.GetDouble("ConsumptionQty");
            oBillOfMaterial.ProductCode = oReader.GetString("ProductCode");
            oBillOfMaterial.ProductName = oReader.GetString("ProductName");
            oBillOfMaterial.ColorName = oReader.GetString("ColorName");
            oBillOfMaterial.SizeName = oReader.GetString("SizeName");
            oBillOfMaterial.Symbol = oReader.GetString("Symbol");
            oBillOfMaterial.UnitName = oReader.GetString("UnitName");
            oBillOfMaterial.POCode = oReader.GetString("POCode");
            oBillOfMaterial.Remarks = oReader.GetString("Remarks");
            
            oBillOfMaterial.StyleNo = oReader.GetString("StyleNo");
            oBillOfMaterial.AttachFile = oReader.GetBytes("AttachFile");
            oBillOfMaterial.IsAttachmentExist = oReader.GetBoolean("IsAttachmentExist");
            oBillOfMaterial.OrderQty = oReader.GetDouble("OrderQty");
            oBillOfMaterial.BookingQty = oReader.GetDouble("BookingQty");
            oBillOfMaterial.BookingConsumption = oReader.GetDouble("BookingConsumption");
            oBillOfMaterial.BookingConsumptionInPercent = oReader.GetDouble("BookingConsumptionInPercent");

            return oBillOfMaterial;
        }

        private BillOfMaterial CreateObject(NullHandler oReader)
        {
            BillOfMaterial oBillOfMaterial = new BillOfMaterial();
            oBillOfMaterial = MapObject(oReader);
            return oBillOfMaterial;
        }

        private List<BillOfMaterial> CreateObjects(IDataReader oReader)
        {
            List<BillOfMaterial> oBillOfMaterial = new List<BillOfMaterial>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BillOfMaterial oItem = CreateObject(oHandler);
                oBillOfMaterial.Add(oItem);
            }
            return oBillOfMaterial;
        }

        #endregion

        #region Interface implementation
        public BillOfMaterialService() { }
        public BillOfMaterial Save(BillOfMaterial oBillOfMaterial, Int64 nUserID)
        {
            int nTechnicalSheetID = 0;
            string sBillOfMaterialIDs = ""; 
            TransactionContext tc = null;            
            List<BillOfMaterial> oBillOfMaterials = new List<BillOfMaterial>();
            oBillOfMaterials = oBillOfMaterial.BillOfMaterials;
            try
            {                
                tc = TransactionContext.Begin(true);
                foreach (BillOfMaterial oItem in oBillOfMaterials)
                {
                    nTechnicalSheetID = oItem.TechnicalSheetID;
                    IDataReader reader;
                    if (oItem.BillOfMaterialID <= 0)
                    {
                        reader = BillOfMaterialDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        reader = BillOfMaterialDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oBillOfMaterial = new BillOfMaterial();
                        oBillOfMaterial = CreateObject(oReader);
                        sBillOfMaterialIDs = sBillOfMaterialIDs + oBillOfMaterial.BillOfMaterialID.ToString() + ",";
                    }
                    reader.Close();                    
                }
                if (sBillOfMaterialIDs.Length > 0)
                {
                    sBillOfMaterialIDs = sBillOfMaterialIDs.Remove(sBillOfMaterialIDs.Length - 1, 1);
                }
                oBillOfMaterial = new BillOfMaterial();
                oBillOfMaterial.TechnicalSheetID = nTechnicalSheetID;
                BillOfMaterialDA.Delete(tc, oBillOfMaterial, EnumDBOperation.Delete, nUserID, sBillOfMaterialIDs);

                oBillOfMaterials = new List<BillOfMaterial>();
                IDataReader readers = null;
                readers = BillOfMaterialDA.Gets(tc, nTechnicalSheetID);
                oBillOfMaterials = CreateObjects(readers);
                readers.Close();

                oBillOfMaterial = new BillOfMaterial();
                oBillOfMaterial.BillOfMaterials = oBillOfMaterials;
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBillOfMaterial = new BillOfMaterial();
                oBillOfMaterial.ErrorMessage = e.Message;
                #endregion
            }
            return oBillOfMaterial;
        }
        public BillOfMaterial UpdateImage(BillOfMaterial oBillOfMaterial, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                byte[] attachfile = oBillOfMaterial.AttachFile;
                tc = TransactionContext.Begin(true);
                if (oBillOfMaterial.BillOfMaterialID > 0 && oBillOfMaterial.AttachFile != null)
                {
                    BillOfMaterialDA.UpdateImage(tc, oBillOfMaterial.AttachFile, oBillOfMaterial.BillOfMaterialID, nUserID);
                    IDataReader reader = BillOfMaterialDA.Get(tc, oBillOfMaterial.BillOfMaterialID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oBillOfMaterial = CreateObject(oReader);
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

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save BillOfMaterial. Because of " + e.Message, e);
                #endregion
            }
            return oBillOfMaterial;
        }
        public BillOfMaterial GetWithAttachFile(int id, Int64 nUserId)
        {
            BillOfMaterial oBillOfMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BillOfMaterialDA.GetWithImage(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBillOfMaterial = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oBillOfMaterial;
        }
        public BillOfMaterial DeleteImage(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            BillOfMaterial oBillOfMaterial = new BillOfMaterial();
            try
            {
                tc = TransactionContext.Begin(true);
                BillOfMaterialDA.DeleteImage(tc, id);
                IDataReader reader = BillOfMaterialDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBillOfMaterial = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBillOfMaterial.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oBillOfMaterial;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BillOfMaterial oBillOfMaterial = new BillOfMaterial();
                oBillOfMaterial.BillOfMaterialID = id;
                BillOfMaterialDA.Delete(tc, oBillOfMaterial, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BillOfMaterial. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }
        public BillOfMaterial Get(int id, Int64 nUserId)
        {
            BillOfMaterial oAccountHead = new BillOfMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BillOfMaterialDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BillOfMaterial", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<BillOfMaterial> Gets(Int64 nUserID)
        {
            List<BillOfMaterial> oBillOfMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BillOfMaterialDA.Gets(tc);
                oBillOfMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BillOfMaterial", e);
                #endregion
            }

            return oBillOfMaterial;
        }

        public List<BillOfMaterial> Gets(int nTechnicalSheetID, Int64 nUserID)
        {
            List<BillOfMaterial> oBillOfMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BillOfMaterialDA.Gets(tc, nTechnicalSheetID);
                oBillOfMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BillOfMaterial", e);
                #endregion
            }

            return oBillOfMaterial;
        }

        public List<BillOfMaterial> PasteBillOfMaterials(int nOldTSID, int nNewTSID, Int64 nUserID)
        {
            List<BillOfMaterial> oTempBillOfMaterials = new List<BillOfMaterial>();
            List<BillOfMaterial> oNewBillOfMaterials = new List<BillOfMaterial>();
            BillOfMaterial oTempBillOfMaterial = new BillOfMaterial();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BillOfMaterialDA.Gets(tc, nOldTSID);
                oTempBillOfMaterials = CreateObjects(reader);
                reader.Close();
                foreach(BillOfMaterial oItem in oTempBillOfMaterials)
                {
                    int nOldBMID = oItem.BillOfMaterialID;
                    oItem.BillOfMaterialID = 0;
                    oItem.TechnicalSheetID = nNewTSID;
                    reader = BillOfMaterialDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempBillOfMaterial = new BillOfMaterial();
                        oTempBillOfMaterial = CreateObject(oReader);
                        oNewBillOfMaterials.Add(oTempBillOfMaterial);
                    }
                    reader.Close();
                    if (oTempBillOfMaterial.BillOfMaterialID>0)
                    {
                        BillOfMaterialDA.UpdateImageFromBM(tc, nOldBMID, oTempBillOfMaterial.BillOfMaterialID);
                    }
                    
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BillOfMaterial", e);
                #endregion
            }

            return oNewBillOfMaterials;
        }
        public List<BillOfMaterial> GetsWithImage(int nTechnicalSheetID, Int64 nUserID)
        {
            List<BillOfMaterial> oBillOfMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BillOfMaterialDA.GetsWithImage(tc, nTechnicalSheetID);
                oBillOfMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BillOfMaterial", e);
                #endregion
            }

            return oBillOfMaterial;
        }

        public List<BillOfMaterial> ResetSequence(List<BillOfMaterial> oBillOfMaterials, Int64 nUserID)
        {
            List<BillOfMaterial> oTempBillOfMaterials = new List<BillOfMaterial> ();

            TransactionContext tc = null;
            int nTempTechnicalSheetID = oBillOfMaterials[0].TechnicalSheetID;
            try
            {
                tc = TransactionContext.Begin();                
                foreach(BillOfMaterial oItem in oBillOfMaterials)
                {                    
                    BillOfMaterialDA.UpDateSequence(tc, oItem.BillOfMaterialID, oItem.Sequence);
                }
                IDataReader reader;
                reader = BillOfMaterialDA.Gets(tc, nTempTechnicalSheetID);
                oTempBillOfMaterials = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTempBillOfMaterials = new List<BillOfMaterial>();
                BillOfMaterial oBillOfMaterial = new BillOfMaterial();
                oBillOfMaterial.ErrorMessage = e.Message;
                oTempBillOfMaterials.Add(oBillOfMaterial);
                #endregion
            }

            return oTempBillOfMaterials;
        }

        
        public List<BillOfMaterial> Gets_Report(int id, Int64 nUserID)
        {
            List<BillOfMaterial> oBillOfMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BillOfMaterialDA.Gets_Report(tc, id);
                oBillOfMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheet", e);
                #endregion
            }

            return oBillOfMaterial;
        }

        #endregion
    }
}

