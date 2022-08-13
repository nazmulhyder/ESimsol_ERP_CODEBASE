using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 
namespace ESimSol.Services.Services
{


    public class BuyerWiseBrandService : MarshalByRefObject, IBuyerWiseBrandService
    {
        #region Private functions and declaration
        private BuyerWiseBrand MapObject(NullHandler oReader)
        {
            BuyerWiseBrand oBuyerWiseBrand = new BuyerWiseBrand();
            oBuyerWiseBrand.BuyerWiseBrandID = oReader.GetInt32("BuyerWiseBrandID");
            oBuyerWiseBrand.BrandID = oReader.GetInt32("BrandID");
            oBuyerWiseBrand.BuyerID = oReader.GetInt32("BuyerID");
            oBuyerWiseBrand.BuyerShortName = oReader.GetString("BuyerShortName");
            oBuyerWiseBrand.BuyerName = oReader.GetString("BuyerName");
            oBuyerWiseBrand.BrandCode = oReader.GetString("BrandCode");
            oBuyerWiseBrand.BrandName = oReader.GetString("BrandName");

            return oBuyerWiseBrand;
        }

        private BuyerWiseBrand CreateObject(NullHandler oReader)
        {
            BuyerWiseBrand oBuyerWiseBrand = new BuyerWiseBrand();
            oBuyerWiseBrand = MapObject(oReader);
            return oBuyerWiseBrand;
        }

        private List<BuyerWiseBrand> CreateObjects(IDataReader oReader)
        {
            List<BuyerWiseBrand> oBuyerWiseBrand = new List<BuyerWiseBrand>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BuyerWiseBrand oItem = CreateObject(oHandler);
                oBuyerWiseBrand.Add(oItem);
            }
            return oBuyerWiseBrand;
        }

        #endregion

        #region Interface implementation
        public string Save(BuyerWiseBrand oBuyerWiseBrand, bool IsShortList, bool IsBuyerBased, Int64 nUserID)
        {
            TransactionContext tc = null;
            BuyerWiseBrand oNewBuyerWiseBrand = new BuyerWiseBrand();
            List<BuyerWiseBrand> oBuyerWiseBrands = new List<BuyerWiseBrand>();
            oBuyerWiseBrands = oBuyerWiseBrand.BuyerWiseBrands;
            string sIds = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                if (IsShortList == true)
                {

                    foreach (BuyerWiseBrand oItem in oBuyerWiseBrands)
                    {
                        if (oItem.BuyerWiseBrandID <= 0)
                        {
                            reader = BuyerWiseBrandDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = BuyerWiseBrandDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        reader.Close();
                    }

                }
                else
                {
                    if (oBuyerWiseBrands.Count > 0)
                    {
                        foreach (BuyerWiseBrand oItem in oBuyerWiseBrands)
                        {

                            if (oItem.BuyerWiseBrandID <= 0)
                            {
                                reader = BuyerWiseBrandDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                reader = BuyerWiseBrandDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            }
                            NullHandler oReader = new NullHandler(reader);

                            if (reader.Read())
                            {
                                oNewBuyerWiseBrand = new BuyerWiseBrand();
                                oNewBuyerWiseBrand = CreateObject(oReader);
                            }
                            reader.Close();
                            if (IsBuyerBased == true)
                            {
                                sIds = sIds + oNewBuyerWiseBrand.BrandID + ",";
                            }
                            else
                            {
                                sIds = sIds + oNewBuyerWiseBrand.BuyerID + ",";
                            }

                        }
                    }
                    else
                    {
                      //The region will be set for Empty List, if not Assign any User or any Role.  
                        oNewBuyerWiseBrand = new BuyerWiseBrand();
                        oNewBuyerWiseBrand.BrandID = oBuyerWiseBrand.BrandID;//for Role Based
                        oNewBuyerWiseBrand.BuyerID = oBuyerWiseBrand.BuyerID;//for user based
                    }

                    if (sIds.Length > 0)
                    {
                        sIds = sIds.Remove(sIds.Length - 1, 1);
                    }

                    BuyerWiseBrandDA.Delete(tc, oNewBuyerWiseBrand, EnumDBOperation.Delete, nUserID, IsBuyerBased, sIds);
                }

               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save BuyerWiseBrand. Because of " + e.Message, e);
                #endregion
            }
            return "Succefully Saved";
        }
        
        public BuyerWiseBrand Get(int id, Int64 nUserId)
        {
            BuyerWiseBrand oAccountHead = new BuyerWiseBrand();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BuyerWiseBrandDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BuyerWiseBrand", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BuyerWiseBrand> GetsByBrand(int id, Int64 nUserID)
        {
            List<BuyerWiseBrand> oBuyerWiseBrands = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BuyerWiseBrandDA.GetsByBrand(tc, id);
                oBuyerWiseBrands = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BuyerWiseBrand", e);
                #endregion
            }

            return oBuyerWiseBrands;
        }

        public List<BuyerWiseBrand> GetsByBuyer(int id, Int64 nUserID)
        {
            List<BuyerWiseBrand> oBuyerWiseBrands = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BuyerWiseBrandDA.GetsByBuyer(tc, id);
                oBuyerWiseBrands = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BuyerWiseBrand", e);
                #endregion
            }

            return oBuyerWiseBrands;
        }

        #endregion
    }   
    
    
}
