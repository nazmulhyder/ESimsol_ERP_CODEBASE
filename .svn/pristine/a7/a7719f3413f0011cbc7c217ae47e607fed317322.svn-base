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


    public class PackageTemplateDetailService : MarshalByRefObject, IPackageTemplateDetailService
    {
        #region Private functions and declaration
        private PackageTemplateDetail MapObject(NullHandler oReader)
        {
            PackageTemplateDetail oPackageTemplateDetail = new PackageTemplateDetail();

            oPackageTemplateDetail.PackageTemplateDetailID = oReader.GetInt32("PackageTemplateDetailID");
            oPackageTemplateDetail.PackageTemplateID = oReader.GetInt32("PackageTemplateID");
            oPackageTemplateDetail.ProductID = oReader.GetInt32("ProductID");
            oPackageTemplateDetail.ProductCode = oReader.GetString("ProductCode");
            oPackageTemplateDetail.ProductName = oReader.GetString("ProductName");
            oPackageTemplateDetail.Quantity = oReader.GetString("Quantity");
            return oPackageTemplateDetail;
        }

        private PackageTemplateDetail CreateObject(NullHandler oReader)
        {
            PackageTemplateDetail oPackageTemplateDetail = new PackageTemplateDetail();
            oPackageTemplateDetail = MapObject(oReader);
            return oPackageTemplateDetail;
        }

        private List<PackageTemplateDetail> CreateObjects(IDataReader oReader)
        {
            List<PackageTemplateDetail> oPackageTemplateDetail = new List<PackageTemplateDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PackageTemplateDetail oItem = CreateObject(oHandler);
                oPackageTemplateDetail.Add(oItem);
            }
            return oPackageTemplateDetail;
        }

        #endregion

        #region Interface implementation
        public PackageTemplateDetailService() { }



        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PackageTemplateDetail oPackageTemplateDetail = new PackageTemplateDetail();
                oPackageTemplateDetail.PackageTemplateDetailID = id;
                PackageTemplateDetailDA.Delete(tc, oPackageTemplateDetail, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete PackageTemplateDetail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public PackageTemplateDetail Get(int id, Int64 nUserId)
        {
            PackageTemplateDetail oAccountHead = new PackageTemplateDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PackageTemplateDetailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get PackageTemplateDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PackageTemplateDetail> Gets(Int64 nUserID)
        {
            List<PackageTemplateDetail> oPackageTemplateDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PackageTemplateDetailDA.Gets(tc);
                oPackageTemplateDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PackageTemplateDetail", e);
                #endregion
            }

            return oPackageTemplateDetail;
        }

        public List<PackageTemplateDetail> Gets(int nONSID, Int64 nUserID)
        {
            List<PackageTemplateDetail> oPackageTemplateDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PackageTemplateDetailDA.Gets(nONSID, tc);
                oPackageTemplateDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Order Negotiation Detail", e);
                #endregion
            }

            return oPackageTemplateDetails;
        }


        #endregion
    }   

}
