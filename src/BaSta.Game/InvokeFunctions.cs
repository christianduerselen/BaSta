using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class InvokeFunctions
    {
        public void disposeFormAsync(Form target)
        {
            if (target == null)
                return;
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new InvokeFunctions.disposeFormAsyncDelegate(disposeFormAsync), (object)target);
            }
            else
            {
                target.Dispose();
                target = (Form)null;
            }
        }

        public void setCheckBoxCheckedAsync(CheckBox target, bool is_checked)
        {
            if (target == null)
                return;
            if (target.InvokeRequired)
                target.Invoke((Delegate)new InvokeFunctions.setCheckBoxCheckedAsyncDelegate(setCheckBoxCheckedAsync), (object)target, (object)is_checked);
            else
                target.Checked = is_checked;
        }

        public void setCheckBoxTextAsync(CheckBox target, string msg)
        {
            if (target == null)
                return;
            if (target.InvokeRequired)
                target.Invoke((Delegate)new InvokeFunctions.setCheckBoxTextAsyncDelegate(setCheckBoxTextAsync), (object)target, (object)msg);
            else
                target.Text = msg;
        }

        public void setControlEnabled(Control target, bool enabled)
        {
            if (target == null)
                return;
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new InvokeFunctions.setControlEnabledDelegate(setControlEnabled), (object)target, (object)enabled);
            }
            else
            {
                target.Enabled = enabled;
                if (enabled)
                    return;
                target.Text += "(nur Konsole)";
            }
        }

        public void setControlVisible(Control target, bool visible)
        {
            if (target == null)
                return;
            if (target.InvokeRequired)
                target.Invoke((Delegate)new InvokeFunctions.setControlVisibleDelegate(setControlVisible), (object)target, (object)visible);
            else
                target.Visible = visible;
        }

        public void setButtonTextAsync(Button target, string msg)
        {
            if (target == null)
                return;
            if (target.InvokeRequired)
                target.Invoke((Delegate)new InvokeFunctions.setButtonTextAsyncDelegate(setButtonTextAsync), (object)target, (object)msg);
            else
                target.Text = msg;
        }

        public void setLabelTextAsync(Label target, string msg)
        {
            if (target == null)
                return;
            if (target.InvokeRequired)
                target.Invoke((Delegate)new InvokeFunctions.setLabelTextAsyncDelegate(setLabelTextAsync), (object)target, (object)msg);
            else
                target.Text = msg;
        }

        public void setLabelColorAsync(Label target, Color color)
        {
            if (target == null)
                return;
            if (target.InvokeRequired)
                target.Invoke((Delegate)new InvokeFunctions.setLabelColorAsyncDelegate(setLabelColorAsync), (object)target, (object)color);
            else
                target.ForeColor = color;
        }

        private delegate void disposeFormAsyncDelegate(Form target);

        private delegate void setCheckBoxCheckedAsyncDelegate(CheckBox target, bool is_checked);

        private delegate void setCheckBoxTextAsyncDelegate(CheckBox target, string msg);

        private delegate void setControlEnabledDelegate(Control target, bool enabled);

        private delegate void setControlVisibleDelegate(Control target, bool visible);

        private delegate void setButtonTextAsyncDelegate(Button target, string msg);

        private delegate void setLabelTextAsyncDelegate(Label target, string msg);

        private delegate void setLabelColorAsyncDelegate(Label target, Color color);
    }
}