using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using System.Text;


using XBase.Database;
using XBase.App.Model;

namespace XBase.App.Controller
{
    class LoginController
    {
        public static bool Login(string vUser, string vPassword, string IpAddress)
        {
            ArrayList users = MUser.GetUser(new DataEngine(), vUser);

            if( users.Count == 1 )
            {
                MUser tUser = (MUser)users[0];

                if( tUser.Password == vPassword )
                {
                    return true;
                }

            }

            return false;
        }
    }
}
