using System;
using System.Collections.Generic;

using System.Text;


namespace XBase
{
    /// <summary>
    /// IXApplication
    /// </summary>
    public interface IXApp
    {
        void WriteLog(String vString);
    }

    public class XApp
    {
        private static IXApp mApp;

        public static void SetApp(IXApp vApp)
        {
            mApp = vApp;
        }

        public static void WriteLog(String vString)
        {
            mApp.WriteLog(vString);
        }
    }
}
