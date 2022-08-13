using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ICS.Core.Utility
{
    public partial class ProgressWindow : Form, IProgressCallback
    {
        #region Declarations
        public delegate void SetTextInvoker(String text);
        public delegate void SetImageInvoker(byte[] chobi);
        public delegate void IncrementInvoker(int val);
        public delegate void StepToInvoker(int val);
        public delegate void RangeInvoker(int Min, int Max);

        private String titleRoot = "";
        private ManualResetEvent initEvent = new ManualResetEvent(false);
        private ManualResetEvent abortEvent = new ManualResetEvent(false);
        private bool requiresClose = true;        

        #endregion
        public ProgressWindow()
        {
            InitializeComponent();
            //this.btnCancel.DialogResult = DialogResult.Cancel;
        }
        #region Properties
        #region StartTime
        private DateTime st = DateTime.Now;//start Time
        public DateTime StartTime
        {
            get { return st; }
            set { st = value; }
        }
        #endregion
        #endregion

        #region Implementation of IProgressCallback
        /// <summary>
        /// Call this method from the worker thread to initialize
        /// the progress meter.
        /// </summary>
        /// <param name="Min">The minimum value in the progress range (e.g. 0)</param>
        /// <param name="Max">The maximum value in the progress range (e.g. 100)</param>
        public void Begin(int Min, int Max)
        {
            this.initEvent.WaitOne();
            this.Invoke(new RangeInvoker(DoBegin), new object[] { Min, Max });
        }

        /// <summary>
        /// Call this method from the worker thread to initialize
        /// the progress callback, without setting the range
        /// </summary>
        public void Begin()
        {
            this.initEvent.WaitOne();
            this.Invoke(new MethodInvoker(DoBegin));
        }

        /// <summary>
        /// Call this method from the worker thread to reset the range in the progress callback
        /// </summary>
        /// <param name="Min">The minimum value in the progress range (e.g. 0)</param>
        /// <param name="Max">The maximum value in the progress range (e.g. 100)</param>
        /// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
        public void SetRange(int Min, int Max)
        {
            this.initEvent.WaitOne();
            this.Invoke(new RangeInvoker(DoSetRange), new object[] { Min, Max });
        }

        /// <summary>
        /// Call this method from the worker thread to update the progress text.
        /// </summary>
        /// <param name="text">The progress text to display</param>
        public void SetText(String text)
        {
            this.Invoke(new SetTextInvoker(DoSetText), new object[] { text });
        }

        /// <summary>
        /// Call this method from the worker thread to update the progress image.
        /// </summary>
        /// <param name="chobi">The progress text to display</param>
        public void SetImage(byte[] chobi)
        {
            this.Invoke(new SetImageInvoker(DoSetImage), new object[] { chobi });
        }        

        /// <summary>
        /// Call this method from the worker thread to increase the progress counter by a specified value.
        /// </summary>
        /// <param name="val">The amount by which to increment the progress indicator</param>
        public void Increment(int val)
        {
            this.Invoke(new IncrementInvoker(DoIncrement), new object[] { val });
        }

        /// <summary>
        /// Call this method from the worker thread to step the progress meter to a particular value.
        /// </summary>
        /// <param name="val"></param>
        public void StepTo(int val)
        {
            this.Invoke(new StepToInvoker(DoStepTo), new object[] { val });
        }


        /// <summary>
        /// If this property is true, then you should abort work
        /// </summary>
        public bool IsAborting
        {
            get
            {
                return this.abortEvent.WaitOne(0, false);
            }
        }

        /// <summary>
        /// Call this method from the worker thread to finalize the progress meter
        /// </summary>
        public void End()
        {
            if (this.requiresClose)
            {
                this.Invoke(new MethodInvoker(DoEnd));
            }
        }
        #endregion

        #region Implementation members invoked on the owner thread
        private void DoSetText(String text)
        {
            this.label.Text = text;
        }
        private void DoSetImage(byte[] chobi)
        {
            if (chobi == null) { picBoxPhoto.Image = null; return; }
            MemoryStream ms = new MemoryStream(chobi.Length);
            ms.Write(chobi, 0, chobi.Length);
            Image pic = Image.FromStream(ms);
            picBoxPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
            picBoxPhoto.Image = pic;
        }

        private void DoIncrement(int val)
        {
            this.progressBar.Increment(val);
            this.UpdateStatusText();
        }

        private void DoStepTo(int val)
        {
            this.progressBar.Value = val;
            this.UpdateStatusText();
        }

        private void DoBegin(int Min, int Max)
        {
            this.DoBegin();
            this.DoSetRange(Min, Max);
        }

        private void DoBegin()
        {
            //this.btnCancel.Enabled = true;
            this.ControlBox = true;
        }

        private void DoSetRange(int Min, int Max)
        {
            this.progressBar.Minimum = Min;
            this.progressBar.Maximum = Max;
            this.progressBar.Value = Min;
            this.titleRoot = Text;
        }

        private void DoEnd()
        {
            this.Close();
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Handles the form load, and sets an event to ensure that
        /// intialization is synchronized with the appearance of the form.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.ControlBox = false;
            this.initEvent.Set();
        }

        /// <summary>
        /// Handler for 'Close' clicking
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.requiresClose = false;
            this.AbortWork();
            base.OnClosing(e);
        }
        #endregion

        #region Implementation Utilities
        /// <summary>
        /// Utility function that formats and updates the title bar text
        /// </summary>
        private void UpdateStatusText()
        {
            this.Text = titleRoot + String.Format(" -> {0}% completed", (progressBar.Value * 100) / (progressBar.Maximum - progressBar.Minimum));
            this.lblPercent.Text = this.Text;
            TimeSpan ts = Global.TimeDifference(DateTime.Now, st);
            this.lblTime.Text = ts.Minutes.ToString() + ":" + ts.Seconds.ToString() + "|" + (ts.Milliseconds / 100).ToString();
        }

        /// <summary>
        /// Utility function to terminate the thread
        /// </summary>
        private void AbortWork()
        {
            this.abortEvent.Set();
        }
        #endregion
    }
}