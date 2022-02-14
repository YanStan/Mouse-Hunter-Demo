
using System.Windows.Forms;

namespace Mouse_Hunter.Extensions
{
    public static class WinFormsControlExtensions
    {
        public static void InvokeIfRequired(this Control c, MethodInvoker action)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
