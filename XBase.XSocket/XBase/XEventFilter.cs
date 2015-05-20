using System;
using System.Collections;

namespace XBase
{
    [Serializable]
    public class XEvent
    {
        public int EventID = 0;
        public object EventData = null;
    }

    public interface IXEventFilter
    {
        int GetFilterID();

        /// <param name="vEvent"></param>
        /// <returns></returns>
        bool EventProcess(object vEvent);
    }

    public class XEventFilter : IXEventFilter
    {
        private int m_EventID;
        private XEventFilterHandler m_EventHandler = null;

        public XEventFilter(int vEventID, XEventFilterHandler vEventHandler)
        {
            m_EventID = vEventID;
            m_EventHandler = vEventHandler;
        }

        public int GetFilterID()
        {
            return m_EventID;
        }

        public bool EventProcess(object vEvent)
        {
            return m_EventHandler(vEvent);
        }
    }

    public class XEventProcess
    {

        public int EventID = -1;
        public ArrayList EventFilters = new ArrayList();

        public XBeforeEventFilterHandler BeforeEventProcess = null;
        public XAfterEventFilterHandler AfterEventProcess = null;

        public XEventProcess()
        {
        }

        public XEventProcess(int vEventID)
        {
            EventID = vEventID;
        }

        public bool ProcessEvent(object vEvent)
        {
            if (EventFilters.Count <= 0) return true;

            bool blResult = true;

            object tEvent = vEvent;
            if (BeforeEventProcess != null)
            {
                tEvent = BeforeEventProcess(tEvent);
            }

            try
            {
                ArrayList tFilters = ArrayList.Synchronized(EventFilters);

                for (int j = 0; j < tFilters.Count; j++)
                {
                    if (!((IXEventFilter)tFilters[j]).EventProcess(tEvent))
                    {
                        blResult = false;
                        break;
                    }
                }
            }
#if DEBUG
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                blResult = false;
            }
#else
			catch
			{
			}
#endif

            if (AfterEventProcess != null)
            {
                AfterEventProcess(tEvent);
            }

            return blResult;
        }

        public void AddEventFilter(IXEventFilter vEventFilter)
        {
            try
            {
                System.Threading.Monitor.Enter(EventFilters.SyncRoot);
                EventFilters.Add(vEventFilter);
            }
#if DEBUG
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
#endif
            finally
            {
                System.Threading.Monitor.Exit(EventFilters.SyncRoot);
            }
        }

        public void RemoveEventFilter(IXEventFilter vEventFilter)
        {
            try
            {
                System.Threading.Monitor.Enter(EventFilters.SyncRoot);
                EventFilters.Remove(vEventFilter);
            }
#if DEBUG
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
#endif
            finally
            {
                System.Threading.Monitor.Exit(EventFilters.SyncRoot);
            }
        }
    }

    public class XEventProcessor
    {
        #region Member ---------------------------------------------------------------------------------

        private XEventProcess[] m_EventProcess = null;

        #endregion

        public XEventProcessor(int vMaxEventID)
        {
            m_EventProcess = new XEventProcess[vMaxEventID + 1];

            for (int i = 0; i <= vMaxEventID; i++)
            {
                m_EventProcess[i] = new XEventProcess(i);
            }
        }

        public XEventProcess this[int vEventID]
        {
            get
            {
                return this.m_EventProcess[vEventID];
            }
        }

        /// <param name="vEventFilter"></param>
        public void AddEventFilter(IXEventFilter vEventFilter)
        {
            int nEventID = vEventFilter.GetFilterID();
            m_EventProcess[nEventID].AddEventFilter(vEventFilter);
        }

        /// <param name="vEventFilter"></param>
        public void RemoveEventFilter(IXEventFilter vEventFilter)
        {
            int nEventID = vEventFilter.GetFilterID();
            m_EventProcess[nEventID].RemoveEventFilter(vEventFilter);
        }

        /// <param name="vEventID"></param>
        /// <param name="vEventArgs"></param>
        /// <returns></returns>
        public bool ProcessEvent(int vEventID, object vEventArgs)
        {
            return m_EventProcess[vEventID].ProcessEvent(vEventArgs);
        }
    }
}

