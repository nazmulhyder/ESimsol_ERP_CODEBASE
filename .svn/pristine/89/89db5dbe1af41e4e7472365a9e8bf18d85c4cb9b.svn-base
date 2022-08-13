using System;
using System.Configuration;
using System.Collections;
using System.Windows.Forms;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region Delegate: Collection change Event
    public delegate void CollectionChanged();
    #endregion
    public static class HelperBusinessObject
    {
        public static void RegisterDGVForSortAndFilter(CollectionBase oCollectionBase, ref DataGridView dgv, ref System.ComponentModel.IContainer components)// Add by Faruk Here sObjectID=Individual object PK filed Name
        {
            IndexedBusinessObjects oBOs = oCollectionBase as IndexedBusinessObjects;

            String PropertyList = "ObjectID";
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                if (String.IsNullOrEmpty(item.DataPropertyName)) continue;
                PropertyList = PropertyList + "," + item.DataPropertyName;
            }

            components = new System.ComponentModel.Container();
            System.Windows.Forms.BindingSource bs = new BindingSource(components);
            System.Data.DataSet ds = oBOs.ToDataSet(PropertyList);

            ((System.ComponentModel.ISupportInitialize)(bs)).BeginInit();

            bs.DataMember = "DataTable";
            bs.DataSource = ds;
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = bs;
            ((System.ComponentModel.ISupportInitialize)(bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(ds)).EndInit();
        }
    }
}