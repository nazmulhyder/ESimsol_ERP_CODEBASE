using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class LB_LocationService : MarshalByRefObject, ILB_LocationService
    {
        #region Private functions and declaration
        private LB_Location MapObject(NullHandler oReader)
        {
            LB_Location oLocation = new LB_Location();
            oLocation.LB_LocationID = oReader.GetInt32("LB_LocationID");
            oLocation.LB_IPV4 = oReader.GetString("LB_IPV4");
            oLocation.LB_KnownName = oReader.GetString("LB_KnownName");
            oLocation.LB_LocationNote = oReader.GetString("LB_LocationNote");
            oLocation.LB_Is_Classified = oReader.GetBoolean("LB_Is_Classified");
            oLocation.LB_ClassificationDate = oReader.GetDateTime("LB_ClassificationDate");
            oLocation.LB_ClasifiedBy = oReader.GetInt32("LB_ClasifiedBy");
            oLocation.LB_FirstHitDate = oReader.GetDateTime("LB_FirstHitDate");
            oLocation.LB_FirstHitBy = oReader.GetInt32("LB_FirstHitBy");
            return oLocation;
        }

        private LB_Location CreateObject(NullHandler oReader)
        {
            LB_Location oLocation = new LB_Location();
            oLocation = MapObject(oReader);
            return oLocation;
        }

        private List<LB_Location> CreateObjects(IDataReader oReader)
        {
            List<LB_Location> oLocation = new List<LB_Location>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LB_Location oItem = CreateObject(oHandler);
                oLocation.Add(oItem);
            }
            return oLocation;
        }

        #endregion

        #region Interface implementation
        public LB_LocationService() { }

        public LB_Location IUD(LB_Location oLB_Location, short nDBOperation, int nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = LB_LocationDA.IUD(tc, oLB_Location, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oLB_Location = new LB_Location();
                        oLB_Location = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = LB_LocationDA.IUD(tc, oLB_Location, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oLB_Location.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLB_Location = new LB_Location();
                oLB_Location.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oLB_Location;
        }

        public LB_Location Get(int nID, int nUserId)
        {
            LB_Location oLB_Location = new LB_Location(); ;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LB_LocationDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLB_Location = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLB_Location = new LB_Location();
                oLB_Location.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oLB_Location;
        }
        public List<LB_Location> Gets(string sSQL, int nUserId)
        {
            List<LB_Location> oLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LB_LocationDA.Gets(tc, sSQL);
                oLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Location", e);
                #endregion
            }

            return oLocation;
        }
       
        
        #endregion
    }

    public class LB_UserLocationMapService : MarshalByRefObject, ILB_UserLocationMapService
    {
        #region Private functions and declaration
        private LB_UserLocationMap MapObject(NullHandler oReader)
        {
            LB_UserLocationMap oLBULM = new LB_UserLocationMap();
            oLBULM.LB_UserLocationMapID = oReader.GetInt32("LB_UserLocationMapID");
            oLBULM.LB_UserID = oReader.GetInt32("LB_UserID");
            oLBULM.LB_LB_LocationID = oReader.GetInt32("LB_LB_LocationID");
            oLBULM.LB_ExpireDateTime = oReader.GetDateTime("LB_ExpireDateTime");
            oLBULM.LB_IPV4 = oReader.GetString("LB_IPV4");
            oLBULM.LB_KnownName = oReader.GetString("LB_KnownName");
            oLBULM.LogInID = oReader.GetString("LoginID");
            oLBULM.UserName = oReader.GetString("UserName");
            return oLBULM;
        }

        private LB_UserLocationMap CreateObject(NullHandler oReader)
        {
            LB_UserLocationMap oLocation = new LB_UserLocationMap();
            oLocation = MapObject(oReader);
            return oLocation;
        }

        private List<LB_UserLocationMap> CreateObjects(IDataReader oReader)
        {
            List<LB_UserLocationMap> oLocation = new List<LB_UserLocationMap>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LB_UserLocationMap oItem = CreateObject(oHandler);
                oLocation.Add(oItem);
            }
            return oLocation;
        }

        #endregion

        #region Interface implementation
        public LB_UserLocationMapService() { }

         public LB_UserLocationMap Save(LB_UserLocationMap oLB_UserLocationMap, int nUserID)
        {
            TransactionContext tc = null;
            oLB_UserLocationMap.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = LB_UserLocationMapDA.IUD(tc, oLB_UserLocationMap, (int)EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLB_UserLocationMap = new LB_UserLocationMap();
                    oLB_UserLocationMap = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLB_UserLocationMap.ErrorMessage = e.Message;
                #endregion
            }
            return oLB_UserLocationMap;
        }
         public string Delete(LB_UserLocationMap oLB_UserLocationMap, int nUserID)
         {
             TransactionContext tc = null;
             try
             {
                 tc = TransactionContext.Begin(true);

                 LB_UserLocationMapDA.DeleteRow(tc, oLB_UserLocationMap, (int)EnumDBOperation.Delete, nUserID);
                 tc.End();
             }
             catch (Exception e)
             {
                 #region Handle Exception
                 if (tc != null)
                     tc.HandleError();
                 return e.Message.Split('~')[0];
                 #endregion
             }
             return Global.DeleteMessage;
         }
        public LB_UserLocationMap IUD(LB_UserLocationMap oLB_UserLocationMap, short nDBOperation, int nUserID)
        {
            List<LB_UserLocationMap> oLBULMs = new List<LB_UserLocationMap>();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                if (!oLB_UserLocationMap.LB_UserLocationMaps.Any())
                {
                    oLB_UserLocationMap.LB_UserLocationMaps.Add(oLB_UserLocationMap);                   
          
                }

                tc = TransactionContext.Begin(true);
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    foreach (LB_UserLocationMap oItem in oLB_UserLocationMap.LB_UserLocationMaps)
                    {
                        reader = LB_UserLocationMapDA.IUD(tc, oItem, nDBOperation, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            var oLBULM = new LB_UserLocationMap();
                            oLBULM = CreateObject(oReader);
                            oLBULMs.Add(oLBULM);
                        }
                        reader.Close();
                    }

                    //if (nDBOperation == (int)EnumDBOperation.Insert && oLBULMs.Any())
                    //{
                    //    string sSQL = "";
                    //    if (oLB_UserLocationMap.HasMultiLocation)
                    //    {
                    //        sSQL = "Delete from LB_UserLocationMap Where LB_UserID=" + oLBULMs.FirstOrDefault().LB_UserID + " And LB_LB_LocationID NOT IN(" + string.Join(",", oLBULMs.Select(x => x.LB_LB_LocationID).ToList()) + ")";
                    //    }
                    //    else
                    //    {
                    //        sSQL = "Delete from LB_UserLocationMap Where LB_LB_LocationID=" + oLBULMs.FirstOrDefault().LB_LB_LocationID + " And LB_UserID NOT IN(" + string.Join(",", oLBULMs.Select(x => x.LB_UserID).ToList()) + ")";
                    //    }
                    //    LB_UserLocationMapDA.Delete(tc, sSQL);
                    //}

                    oLB_UserLocationMap = new LB_UserLocationMap();
                    oLB_UserLocationMap.LB_UserLocationMaps = oLBULMs;

                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    foreach (LB_UserLocationMap oItem in oLB_UserLocationMap.LB_UserLocationMaps)
                    {
                        reader = LB_UserLocationMapDA.IUD(tc, oItem, nDBOperation, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        reader.Close();
                    }
                    oLB_UserLocationMap.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLBULMs = new List<LB_UserLocationMap>();
                oLB_UserLocationMap = new LB_UserLocationMap();
                oLB_UserLocationMap.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oLB_UserLocationMap;
        }
        public LB_UserLocationMap Get(int nID, int nUserId)
        {
            LB_UserLocationMap oLB_UserLocationMap = new LB_UserLocationMap(); ;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LB_UserLocationMapDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLB_UserLocationMap = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLB_UserLocationMap = new LB_UserLocationMap();
                oLB_UserLocationMap.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oLB_UserLocationMap;
        }
        public List<LB_UserLocationMap> Gets(string sSQL, int nUserId)
        {
            List<LB_UserLocationMap> oLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LB_UserLocationMapDA.Gets(tc, sSQL);
                oLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Location", e);
                #endregion
            }

            return oLocation;
        }

       

        #endregion
    }
}