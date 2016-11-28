using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WenDuJianKong;



  public  class CanBus
{
    AdvCANIO Canbus = new AdvCANIO();
    uint pulNumberofWritten;
    AdvCan canbus = new AdvCan();

    public int CanSend(AdvCan.canmsg_t msg)
    {
        if (Canbus.acCanWrite(msg , 1, ref pulNumberofWritten) ==0)
        {
            return 0;
        }
        else
        {
            return -1;
        }
        
    }
}



