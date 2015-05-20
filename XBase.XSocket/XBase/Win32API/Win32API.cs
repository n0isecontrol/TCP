/***********************************************************************
 * Packate: XBase.Win32API
 * Source:  Win32API.cs
 * 
 * Define Windows SDK API 
 *  
 * Date:		2006-12-8
 **********************************************************************/
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace XBase.Win32API
{
    /// <summary>
    /// Enumeration to be used for those Win32 function that return BOOL
    /// </summary>
    public enum Bool
    {
        False = 0,
        True
    };

    public struct _SIZE
    {
        public long cx;
        public long cy;
    };

    public struct NMHDR
    {
        public System.IntPtr hwndFrom;
        public uint idFrom;
        public uint code;         // NM_ code
    };

    public struct _POINT
    {
        public Int32 x;
        public Int32 y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public override string ToString()
        {
            return "{left=" + left.ToString() + ", " + "top=" + top.ToString() + ", " +
                "right=" + right.ToString() + ", " + "bottom=" + bottom.ToString() + "}";
        }
    }

    public class MESSAGE
    {

        /****************************************************************************
        *                                                                           *
        * winuser.h -- USER procedure declarations, constant definitions and macros *
        *                                                                           *
        * Copyright (c) Microsoft Corporation. All rights reserved.                 *
        *                                                                           *
        ****************************************************************************/


        public const int WM_SIZE = 0x0005;
        public const int WM_ACTIVATE = 0x0006;

        public const int WM_SETFOCUS = 0x0007;
        public const int WM_SETCURSOR = 0x0020;
        public const int WM_PAINT = 0x000F;
        public const int WM_ACTIVATEAPP = 0x001C;

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int WM_DEADCHAR = 0x0103;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_SYSCHAR = 0x0106;
        public const int WM_SYSDEADCHAR = 0x0107;

        public const int WM_COMMAND = 0x0111;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_TIMER = 0x0113;
        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;
        public const int WM_INITMENU = 0x0116;
        public const int WM_INITMENUPOPUP = 0x0117;
        public const int WM_MENUSELECT = 0x011F;
        public const int WM_MENUCHAR = 0x0120;
        public const int WM_ENTERIDLE = 0x0121;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_WINDOWPOSCHANGING = 0x0046;
        public const int WM_WINDOWPOSCHANGED = 0x0047;
        public const int WM_NCCALCSIZE = 0x0083;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_NCACTIVATE = 0x0086;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_CAPTURECHANGED = 0x0215;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_RBUTTONDBLCLK = 0x0206;

        public const int WM_PROCUSER = 0x8100;

        public const int WM_DEVMODECHANGE = 0x001B;
        public const int WM_FONTCHANGE = 0x001D;
        public const int WM_TIMECHANGE = 0x001E;
        public const int WM_CANCELMODE = 0x001F;
        public const int WM_MOUSEACTIVATE = 0x0021;
        public const int WM_CHILDACTIVATE = 0x0022;
        public const int WM_QUEUESYNC = 0x0023;

        public const int WM_GETMINMAXINFO = 0x0024;

        public const int WM_NOTIFY = 0x004E;
        public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;
        public const int WM_INPUTLANGCHANGE = 0x0051;
        public const int WM_TCARD = 0x0052;
        public const int WM_HELP = 0x0053;
        public const int WM_USERCHANGED = 0x0054;
        public const int WM_NOTIFYFORMAT = 0x0055;
        public const int WM_CONTEXTMENU = 0x007B;
        public const int WM_STYLECHANGING = 0x007C;
        public const int WM_STYLECHANGED = 0x007D;
        public const int WM_DISPLAYCHANGE = 0x007E;
        public const int WM_GETICON = 0x007F;
        public const int WM_SETICON = 0x0080;
        public const int WM_NCCREATE = 0x0081;
        public const int WM_NCDESTROY = 0x0082;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_GETDLGCODE = 0x0087;
        public const int WM_SYNCPAINT = 0x0088;
        public const int WM_NCMOUSEMOVE = 0x00A0;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_NCRBUTTONDOWN = 0x00A4;
        public const int WM_NCRBUTTONUP = 0x00A5;
        public const int WM_NCRBUTTONDBLCLK = 0x00A6;
        public const int WM_NCMBUTTONDOWN = 0x00A7;
        public const int WM_NCMBUTTONUP = 0x00A8;
        public const int WM_NCMBUTTONDBLCLK = 0x00A9;
        public const int WM_NCXBUTTONDOWN = 0x00AB;
        public const int WM_NCXBUTTONUP = 0x00AC;
        public const int WM_NCXBUTTONDBLCLK = 0x00AD;
        public const int WM_USER = 0x0400;

        // Process Messagge.
        public static int WM_MINMESSAGE = WM_PROCUSER + 200;
        public static int WM_SUCCESS = WM_PROCUSER + 200;
        public static int WM_ERROR = WM_PROCUSER + 201;
        public static int WM_MAXMESSAGE = WM_PROCUSER + 201;

        
        /*
         * Scroll Bar Constants
         */
        public const int SB_HORZ = 0;
        public const int SB_VERT = 1;
        public const int SB_CTL = 2;
        public const int SB_BOTH = 3;

        /*
         * Scroll Bar Commands
         */
        public const int SB_LINEUP = 0;
        public const int SB_LINELEFT = 0;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINERIGHT = 1;
        public const int SB_PAGEUP = 2;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGERIGHT = 3;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_LEFT = 6;
        public const int SB_BOTTOM = 7;
        public const int SB_RIGHT = 7;
        public const int SB_ENDSCROLL = 8;

        /*
         * Old ShowWindow() Commands
         */
        public const int HIDE_WINDOW = 0;
        public const int SHOW_OPENWINDOW = 1;
        public const int SHOW_ICONWINDOW = 2;
        public const int SHOW_FULLSCREEN = 3;
        public const int SHOW_OPENNOACTIVATE = 4;

        /*
         * Identifiers for the WM_SHOWWINDOW message
         */
        public const int SW_PARENTCLOSING = 1;
        public const int SW_OTHERZOOM = 2;
        public const int SW_PARENTOPENING = 3;
        public const int SW_OTHERUNZOOM = 4;

        /*
        * AnimateWindow() Commands
        */
        public const int AW_HOR_POSITIVE = 0x00000001;
        public const int AW_HOR_NEGATIVE = 0x00000002;
        public const int AW_VER_POSITIVE = 0x00000004;
        public const int AW_VER_NEGATIVE = 0x00000008;
        public const int AW_CENTER = 0x00000010;
        public const int AW_HIDE = 0x00010000;
        public const int AW_ACTIVATE = 0x00020000;
        public const int AW_SLIDE = 0x00040000;
        public const int AW_BLEND = 0x00080000;

        /*
         * WM_KEYUP/DOWN/CHAR HIWORD(lParam) flags
         */
        public const int KF_EXTENDED = 0x0100;
        public const int KF_DLGMODE = 0x0800;
        public const int KF_MENUMODE = 0x1000;
        public const int KF_ALTDOWN = 0x2000;
        public const int KF_REPEAT = 0x4000;
        public const int KF_UP = 0x8000;

        /*
         * SetWindowsHook() codes
         */
        public const int WH_MIN = (-1);
        public const int WH_MSGFILTER = (-1);
        public const int WH_JOURNALRECORD = 0;
        public const int WH_JOURNALPLAYBACK = 1;
        public const int WH_KEYBOARD = 2;
        public const int WH_GETMESSAGE = 3;
        public const int WH_CALLWNDPROC = 4;
        public const int WH_CBT = 5;
        public const int WH_SYSMSGFILTER = 6;
        public const int WH_MOUSE = 7;
        public const int WH_HARDWARE = 8;
        public const int WH_DEBUG = 9;
        public const int WH_SHELL = 10;
        public const int WH_FOREGROUNDIDLE = 11;
        public const int WH_CALLWNDPROCRET = 12;
        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE_LL = 14;
        /*
         * CBT Hook Codes
         */
        public const int HCBT_MOVESIZE = 0;
        public const int HCBT_MINMAX = 1;
        public const int HCBT_QS = 2;
        public const int HCBT_CREATEWND = 3;
        public const int HCBT_DESTROYWND = 4;
        public const int HCBT_ACTIVATE = 5;
        public const int HCBT_CLICKSKIPPED = 6;
        public const int HCBT_KEYSKIPPED = 7;
        public const int HCBT_SYSCOMMAND = 8;
        public const int HCBT_SETFOCUS = 9;

        /*
         * Status Bar 메시지
         */
        public const int SB_SETTEXTA = (WM_USER + 1);
        public const int SB_SETTEXTW = (WM_USER + 11);
        public const int SB_GETTEXTA = (WM_USER + 2);
        public const int SB_GETTEXTW = (WM_USER + 13);
        public const int SB_GETTEXTLENGTHA = (WM_USER + 3);
        public const int SB_GETTEXTLENGTHW = (WM_USER + 12);


        public const int SB_SETPARTS = (WM_USER + 4);
        public const int SB_GETPARTS = (WM_USER + 6);
        public const int SB_GETBORDERS = (WM_USER + 7);
        public const int SB_SETMINHEIGHT = (WM_USER + 8);
        public const int SB_SIMPLE = (WM_USER + 9);
        public const int SB_GETRECT = (WM_USER + 10);
        public const int SB_ISSIMPLE = (WM_USER + 14);
        public const int SB_SETICON = (WM_USER + 15);
        public const int SB_SETTIPTEXTA = (WM_USER + 16);
        public const int SB_SETTIPTEXTW = (WM_USER + 17);
        public const int SB_GETTIPTEXTA = (WM_USER + 18);
        public const int SB_GETTIPTEXTW = (WM_USER + 19);
        public const int SB_GETICON = (WM_USER + 20);
        public const int SBT_OWNERDRAW = 0x1000;
        public const int SBT_NOBORDERS = 0x0100;
        public const int SBT_POPOUT = 0x0200;
        public const int SBT_RTLREADING = 0x0400;
        public const int SBT_NOTABPARSING = 0x0800;
    }

    public enum NCRESULT
    {
        HTCLOSE = 20
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SystemTime
    {
        public ushort year;
        public ushort month;
        public ushort dayOfWeek;
        public ushort day;
        public ushort hour;
        public ushort minute;
        public ushort second;
        public ushort milliseconds;
    }

    /// <summary>
    /// WinGDI Class
    /// </summary>
    public class WinGDI
    {
        /// <summary>
        /// Win32 GDI Stroke Object 
        /// </summary>
        public const int NULL_BRUSH = 5;
        public const int BLACK_PEN = 7;

        /// <summary>
        /// Win32 GDI에서 Draw Mode
        /// This is used in  Win32 API SetROP method.
        /// </summary>
        public const int R2_XORPEN = 7;
        public const int R2_NOTXORPEN = 10;


        /// <summary>
        ///  Pen Style
        /// </summary>
        public enum PenStyles
        {
            PS_SOLID = 0,
            PS_DASH = 1,
            PS_DOT = 2,
            PS_DASHDOT = 3,
            PS_DASHDOTDOT = 4
        }

        /// <summary>
        /// Enumeration for the raster operations used in BitBlt.
        /// In C++ these are actually public const int. But to use these
        /// constants with C#, a new enumeration type is defined.
        /// </summary>
        public enum TernaryRasterOperations
        {
            SRCCOPY = 0x00CC0020, /* dest = source                   */
            SRCPAINT = 0x00EE0086, /* dest = source OR dest           */
            SRCAND = 0x008800C6, /* dest = source AND dest          */
            SRCINVERT = 0x00660046, /* dest = source XOR dest          */
            SRCERASE = 0x00440328, /* dest = source AND (NOT dest )   */
            NOTSRCCOPY = 0x00330008, /* dest = (NOT source)             */
            NOTSRCERASE = 0x001100A6, /* dest = (NOT src) AND (NOT dest) */
            MERGECOPY = 0x00C000CA, /* dest = (source AND pattern)     */
            MERGEPAINT = 0x00BB0226, /* dest = (NOT source) OR dest     */
            PATCOPY = 0x00F00021, /* dest = pattern                  */
            PATPAINT = 0x00FB0A09, /* dest = DPSnoo                   */
            PATINVERT = 0x005A0049, /* dest = pattern XOR dest         */
            DSTINVERT = 0x00550009, /* dest = (NOT dest)               */
            BLACKNESS = 0x00000042, /* dest = BLACK                    */
            WHITENESS = 0x00FF0062, /* dest = WHITE                    */
        };

        /// <summary>
        /// LOG BRUSH
        /// </summary>
        public struct LOGBRUSH
        {
            public uint lbStyle;
            public int lbColor;
            public long lbHatch;
        };

        /// <summary>
        /// Win32 GDI API 
        /// </summary>

        /// <summary>
        /// CreateCompatibleDC
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        /// <summary>
        /// DeleteDC
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// SelectObject
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        /// <summary>
        /// DeleteObject
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// CreateCompatibleBitmap
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hObject, int width, int height);


        /// <summary>
        /// BitBlt
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        /// <summary>
        /// StretchBlt
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest,
            int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, TernaryRasterOperations dwRop);

        /// <summary>
        /// PatBlt
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int PatBlt(IntPtr hDC, int x, int y, int width, int height,
            int dwROP);

        /// <summary>
        /// SetROP2
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int SetROP2(
            IntPtr hdc,		// Handle to a Win32 device context
            int enDrawMode	// Drawing mode
            );

        /// <summary>
        /// CreatePen
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreatePen(
            PenStyles enPenStyle,	// Pen style from enum PenStyles
            int nWidth,				// Width of pen
            int crColor				// Color of pen
            );

        /// <summary>
        /// Create Brush
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateBrushIndirect(ref LOGBRUSH lplb);


        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern void Rectangle(
            IntPtr hdc,			// Handle to a Win32 device context
            int X1,				// x-coordinate of top left corner
            int Y1,				// y-cordinate of top left corner
            int X2,				// x-coordinate of bottom right corner
            int Y2				// y-coordinate of bottm right corner
            );

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool LineTo(
            IntPtr hdc,			// Handle to a Win32 device context
            int endX,				// x-coordinate of top left corner
            int endY				// y-cordinate of top left corner
            );


        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool Ellipse(
            IntPtr hdc,			// Handle to a Win32 device context
            int nLeftRect,  // x-coord of upper-left corner of rectangle
            int nTopRect,   // y-coord of upper-left corner of rectangle
            int nRightRect, // x-coord of lower-right corner of rectangle
            int nBottomRect // y-coord of lower-right corner of rectangle
            );

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool MoveToEx(
            IntPtr hdc,			// Handle to a Win32 device context
            int toX,			// x-coordinate of top left corner
            int toY, 		    // y-cordinate of top left corner
            IntPtr lpOldPoint	// old point 
            );

        /// <summary>
        /// GetStockObject 
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetStockObject(
            int brStyle	// Selected from the WinGDI.h BrushStyles enum
            );

        /// <summary>
        /// GetDeviceCaps
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int GetDeviceCaps(
            IntPtr hDC,
            int nIndex	// Selected from the WinGDI.h BrushStyles enum
            );


        /* Mapping Modes */
        public const int MM_TEXT = 1;
        public const int MM_LOMETRIC = 2;
        public const int MM_HIMETRIC = 3;
        public const int MM_LOENGLISH = 4;
        public const int MM_HIENGLISH = 5;
        public const int MM_TWIPS = 6;
        public const int MM_ISOTROPIC = 7;
        public const int MM_ANISOTROPIC = 8;

        /* Min and Max Mapping Mode values */
        public const int MM_MIN = MM_TEXT;
        public const int MM_MAX = MM_ANISOTROPIC;
        public const int MM_MAX_FIXEDSCALE = MM_TWIPS;


        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool SetViewportExtEx(
            IntPtr hdc,       // handle to device context
            int nXExtent,  // new horizontal viewport extent
            int nYExtent,  // new vertical viewport extent
            ref _SIZE lpSize  // original viewport extent
            );

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool SetViewportOrgEx(
            IntPtr hdc,        // handle to device context
            int X,          // new x-coordinate of viewport origin
            int Y,          // new y-coordinate of viewport origin
            ref _POINT lpPoint // original viewport origin
            );

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int SetMapMode(
            IntPtr hdc,        // handle to device context
            int fnMapMode   // new mapping mode
            );

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool SetWindowExtEx(
            IntPtr hdc,       // handle to device context
            int nXExtent,  // new horizontal window extent
            int nYExtent,  // new vertical window extent
            ref _SIZE lpSize  // original window extent
            );

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int GetPixel(IntPtr hDC, int x, int y);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int SetPixel(IntPtr hDC, int x, int y, int color);

        /// <summary>
        /// C# version of Win32 RGB macro
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private static int RGB(int R, int G, int B)
        {
            return (R | (G << 8) | (B << 16));
        }


        public WinGDI()
        {
            
        }

    }

    public enum COMPUTER_NAME_FORMAT
    {
        ComputerNameNetBIOS,
        ComputerNameDnsHostname,
        ComputerNameDnsDomain,
        ComputerNameDnsFullyQualified,
        ComputerNamePhysicalNetBIOS,
        ComputerNamePhysicalDnsHostname,
        ComputerNamePhysicalDnsDomain,
        ComputerNamePhysicalDnsFullyQualified,
        ComputerNameMax
    } ;

    public class Kernel32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct _MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }
        /// <summary>
        /// GetTickCount
        /// </summary>
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern ulong GetTickCount();


        [DllImport("KERNEL32", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetDriveType(string lpRootPathName);
        public const int DRIVE_FIXED = 3;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int
            GetShortPathName(
            [MarshalAs(UnmanagedType.LPTStr)]    string path,
            [MarshalAs(UnmanagedType.LPTStr)]    StringBuilder shortPath,
            int shortPathLength);

        // VOID GetSystemTime(LPSYSTEMTIME lpSystemTime)
        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime([In, Out] SystemTime st);

        // VOID GetSystemTimeAsFileTime(LPFILETIME lpSystemTimeAsFileTime);
        //[DllImport("Kernel32.dll")]
        //public static extern void GetSystemTimeAsFileTime([In, Out] FILETIME ft);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern Bool GetComputerName(ref	String lpBuffer, ref int lpnSize);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern Bool GetComputerNameEx(
            int NameType,  // name type
            ref	String lpBuffer,                // name buffer
            ref int lpnSize                 // size of name buffer
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern Bool GetUserName(
            ref string lpBuffer,  // name buffer
            ref int nSize     // size of name buffer
            );

        [DllImport("Kernel32.dll")]
        public static extern int GetUserDefaultUILanguage();


        [DllImport("Kernel32.dll")]
        public static extern int GetLastError();

        [DllImport("Kernel32.dll")]
        public static extern void SetLastError(int vErrorCode);

        public Kernel32()
        {
        }
    }

    public class User32
    {
        // Window Pos Change Code
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOREDRAW = 0x0008;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_FRAMECHANGED = 0x0020; /* The frame changed: send WM_NCCALCSIZE */
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int SWP_HIDEWINDOW = 0x0080;
        public const int SWP_NOCOPYBITS = 0x0100;
        public const int SWP_NOOWNERZORDER = 0x0200; /* Don't do owner Z ordering */
        public const int SWP_NOSENDCHANGING = 0x0400;/* Don't send WM_WINDOWPOSCHANGING */

        public const int SWP_DRAWFRAME = SWP_FRAMECHANGED;
        public const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;

        public const int SWP_DEFERERASE = 0x2000;
        public const int SWP_ASYNCWINDOWPOS = 0x4000;


        public const int SB_HORZ = 0;
        public const int SB_VERT = 1;
        public const int SB_CTL = 2;
        public const int SB_BOTH = 3;

        /*
         * WM_SYSCOMMAND's WPARAM
         */
        public const int SC_SIZE = 0xF000;
        public const int SC_MOVE = 0xF010;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_NEXTWINDOW = 0xF040;
        public const int SC_PREVWINDOW = 0xF050;
        public const int SC_CLOSE = 0xF060;
        public const int SC_VSCROLL = 0xF070;
        public const int SC_HSCROLL = 0xF080;
        public const int SC_MOUSEMENU = 0xF090;
        public const int SC_KEYMENU = 0xF100;
        public const int SC_ARRANGE = 0xF110;
        public const int SC_RESTORE = 0xF120;
        public const int SC_TASKLIST = 0xF130;
        public const int SC_SCREENSAVE = 0xF140;
        public const int SC_HOTKEY = 0xF150;
        public const int SC_DEFAULT = 0xF160;
        public const int SC_MONITORPOWER = 0xF170;
        public const int SC_CONTEXTHELP = 0xF180;
        public const int SC_SEPARATOR = 0xF00F;
        public const int SC_ICON = SC_MINIMIZE;
        public const int SC_ZOOM = SC_MAXIMIZE;

        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        };

        // GetSystemMetrics's Index
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
        public const int SM_CXVSCROLL = 2;
        public const int SM_CYHSCROLL = 3;
        public const int SM_CYCAPTION = 4;
        public const int SM_CXBORDER = 5;
        public const int SM_CYBORDER = 6;
        public const int SM_CXDLGFRAME = 7;
        public const int SM_CYDLGFRAME = 8;

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int GetSystemMetrics(int nIndex);

        // Mouse Event Genrate Flag
        public const uint MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        public const uint MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        public const uint MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
        public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020; /* middle button down */
        public const uint MOUSEEVENTF_MIDDLEUP = 0x0040; /* middle button up */
        public const uint MOUSEEVENTF_WHEEL = 0x0800; /* wheel button rolled */
        public const uint MOUSEEVENTF_ABSOLUTE = 0x8000; /* absolute move */


        // Generate Mouse Event
        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern void mouse_event(
            uint dwFlags,         // motion and click options
            uint dx,              // horizontal position or change
            uint dy,              // vertical position or change
            uint dwData,          // wheel movement
            IntPtr dwExtraInfo  // application-defined information
            );


        public const int COLOR_SCROLLBAR = 0;
        public const int COLOR_BACKGROUND = 1;
        public const int COLOR_ACTIVECAPTION = 2;
        public const int COLOR_INACTIVECAPTION = 3;
        public const int COLOR_MENU = 4;
        public const int COLOR_WINDOW = 5;
        public const int COLOR_WINDOWFRAME = 6;
        public const int COLOR_MENUTEXT = 7;
        public const int COLOR_WINDOWTEXT = 8;
        public const int COLOR_CAPTIONTEXT = 9;
        public const int COLOR_ACTIVEBORDER = 10;
        public const int COLOR_INACTIVEBORDER = 11;
        public const int COLOR_APPWORKSPACE = 12;
        public const int COLOR_HIGHLIGHT = 13;
        public const int COLOR_HIGHLIGHTTEXT = 14;
        public const int COLOR_BTNFACE = 15;
        public const int COLOR_BTNSHADOW = 16;
        public const int COLOR_GRAYTEXT = 17;
        public const int COLOR_BTNTEXT = 18;
        public const int COLOR_INACTIVECAPTIONTEXT = 19;
        public const int COLOR_BTNHIGHLIGHT = 20;
        public const int COLOR_3DDKSHADOW = 21;
        public const int COLOR_3DLIGHT = 22;
        public const int COLOR_INFOTEXT = 23;
        public const int COLOR_INFOBK = 24;

        public const int COLOR_HOTLIGHT = 26;
        public const int COLOR_GRADIENTACTIVECAPTION = 27;
        public const int COLOR_GRADIENTINACTIVECAPTION = 28;

        public const int COLOR_DESKTOP = COLOR_BACKGROUND;
        public const int COLOR_3DFACE = COLOR_BTNFACE;
        public const int COLOR_3DSHADOW = COLOR_BTNSHADOW;
        public const int COLOR_3DHIGHLIGHT = COLOR_BTNHIGHLIGHT;
        public const int COLOR_3DHILIGHT = COLOR_BTNHIGHLIGHT;
        public const int COLOR_BTNHILIGHT = COLOR_BTNHIGHLIGHT;

        public const int WM_APP = 0x08000;
        public const int WM_SHOW_BUZZER = WM_APP + 1;


        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Int32 GetSysColor(
            int nIndex   // display element
            );

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetFocus();

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetActiveWindow();


        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int PeekMessage(
            ref Message msg,     // message information
            IntPtr hWnd,         // handle to window
            uint wMsgFilterMin,  // first message
            uint wMsgFilterMax,  // last message
            uint wRemoveMsg      // removal options
            );

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool UpdateWindow(
            IntPtr hWnd   // handle to window
            );

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("User32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool InvalidateRect(IntPtr hWnd, ref RECT rect, bool erase);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ValidateRect(IntPtr hWnd, ref RECT rect);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public const int VK_CAPITAL = 0x14;
        public const int VK_NUMLOCK = 0x090;
        public const int VK_SCROLL = 0x091;

        [DllImport("User32.dll")]
        public static extern int GetKeyState(
            int nVirtKey
            );

        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(
            int nVirtKey
            );

        [DllImport("User32.dll")]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern bool GetScrollRange(IntPtr hWnd, int nBar, ref int lpMinPos, ref int lpMaxPos);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetMessage(ref Message msg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool TranslateMessage(ref Message msg);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern long DispatchMessage(ref Message msg);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern Bool ClientToScreen(
            IntPtr hWnd,       // handle to device context
            ref _POINT point
            );

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern Bool ScreenToClient(
            IntPtr hWnd,       // handle to device context
            ref _POINT point
            );

        [DllImport("user32.dll")]
        public static extern Bool ShowWindow(IntPtr hWnd, Int32 nCmdShow);


        /*
         * Extended Window Styles
         */
        public const long WS_EX_DLGMODALFRAME = 0x00000001L;
        public const long WS_EX_NOPARENTNOTIFY = 0x00000004L;
        public const long WS_EX_TOPMOST = 0x00000008L;
        public const long WS_EX_ACCEPTFILES = 0x00000010L;
        public const long WS_EX_TRANSPARENT = 0x00000020L;

        public const long WS_EX_MDICHILD = 0x00000040L;
        public const long WS_EX_TOOLWINDOW = 0x00000080L;
        public const long WS_EX_WINDOWEDGE = 0x00000100L;
        public const long WS_EX_CLIENTEDGE = 0x00000200L;
        public const long WS_EX_CONTEXTHELP = 0x00000400L;

        public const long WS_EX_RIGHT = 0x00001000L;
        public const long WS_EX_LEFT = 0x00000000L;
        public const long WS_EX_RTLREADING = 0x00002000L;
        public const long WS_EX_LTRREADING = 0x00000000L;
        public const long WS_EX_LEFTSCROLLBAR = 0x00004000L;
        public const long WS_EX_RIGHTSCROLLBAR = 0x00000000L;

        public const long WS_EX_CONTROLPARENT = 0x00010000L;
        public const long WS_EX_STATICEDGE = 0x00020000L;
        public const long WS_EX_APPWINDOW = 0x00040000L;


        /*
         * Window field offsets for GetWindowLong()
         */
        public const int GWL_WNDPROC = (-4);
        public const int GWL_HINSTANCE = (-6);
        public const int GWL_HWNDPARENT = (-8);
        public const int GWL_STYLE = (-16);
        public const int GWL_EXSTYLE = (-20);
        public const int GWL_USERDATA = (-21);
        public const int GWL_ID = (-12);

        [DllImport("user32.dll")]
        public static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern uint GetDoubleClickTime();

        public User32()
        {
        }
    }

    public class Sound
    {
        /*
         *  flag values for fuSound and fdwSound arguments on [snd]PlaySound
         */
        public const int SND_SYNC = 0x0000;  /* play synchronously (default) */
        public const int SND_ASYNC = 0x0001;  /* play asynchronously */
        public const int SND_NODEFAULT = 0x0002;  /* silence (!default) if sound not found */
        public const int SND_MEMORY = 0x0004;  /* pszSound points to a memory file */
        public const int SND_LOOP = 0x0008;  /* loop the sound until next sndPlaySound */
        public const int SND_NOSTOP = 0x0010;  /* don't stop any currently playing sound */

        public const int SND_PURGE = 0x0040;  // purge non-static events for task 

        public const int SND_NOWAIT = 0x00002000; /* don't wait if the driver is busy */
        public const int SND_ALIAS = 0x00010000; /* name is a registry alias */
        public const int SND_ALIAS_ID = 0x00110000; /* alias is a predefined ID */
        public const int SND_FILENAME = 0x00020000; /* name is file name */
        public const int SND_RESOURCE = 0x00040004; /* name is resource name or atom */


        [DllImport("Winmm.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool sndPlaySound(
            string lpszSound,
            uint fuSound
            );

        [DllImport("Winmm.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool PlaySound(
            string pszSound,
            IntPtr hmod,
            uint fdwSound
            );

    }

}

