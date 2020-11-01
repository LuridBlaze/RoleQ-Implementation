using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RoleQueue
{
    //Literally the objects that find little use, Just wanted to try those out and it isn't like there's much place for them anywhere else really.
    //From this Enum we'll be taking the roles for our players
    public enum Role
    {
        Tank,
        DamageDealer,
        Support
    }

    public delegate void SessionDone();

}
