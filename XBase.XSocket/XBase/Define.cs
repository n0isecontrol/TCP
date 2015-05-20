/****************************************************
 * Pacakge: XBase
 * Source:  Define.cs
 * 
 * Define delegation, that is used common
 *
 ****************************************************/

using System;
using System.Collections;

namespace XBase
{
    /// <summary>
    /// Define enable handler
    /// </summary>
    public delegate void EnableHandler(bool isEnable);

    /// <summary>
    /// Define void event handler
    /// </summary>
    public delegate void VoidEventHandler();

    /// <summary>
    /// Define xbase enable handler 
    /// </summary>
    public delegate int XEnableHandler(Hashtable hs);

    /// <summary>
    /// Define xbase event handler
    /// </summary>
    public delegate object XEventHandler(Hashtable hs);

    /// <summary>
    /// Define set object handler
    /// </summary>
    public delegate void SetObjectHandler(object vValue);

    /// <summary>
    /// Define XEventFilter Handler
    /// 
    /// if return true then next event filter is processed,
    /// else next event filter is canceled
    /// </summary>
    public delegate bool XEventFilterHandler(object vEvent);

    /// <summary>
    /// Define before event handler.
    /// </summary>
    public delegate object XBeforeEventFilterHandler(object vEvent);

    /// <summary>
    /// Define after event handler
    /// </summary>
    public delegate void XAfterEventFilterHandler(object vEvent);

    /// <summary>
    /// Define object compare handler
    /// 
    /// return value: 
    /// vObject1 > vObject2 then biger than 0
    /// vObject1 = vObject2 then equal  0
    /// vObject1 < vObject2 then small than 0 
    /// </summary>
    public delegate int ItemCompareHandler(object vObject1, object vObject2);
}

