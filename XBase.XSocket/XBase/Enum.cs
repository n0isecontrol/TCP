using System;

namespace XBase
{
    public enum E_RESULT
    {
        Error = -1,

        NoError = 0,
        Success = 0,

        Fail_CreateCommand = 100,

        Fail_HISTORY_REGISTERCMD,

        Fail_HISTORY_NOCREATEUNDOITEM,

        Fail_SetState,

        ERROR_MOVE_BIGSTEP = 200,

        ERROR_UPVERSION = 300,

        ERROR_DOWNVERSION = 301,

        ERROR_MATCHHEADER = 302,

        ERROR_MATCHVERSION = 303
    }


}

