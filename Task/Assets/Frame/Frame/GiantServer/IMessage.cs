// -----------------------------------------------------------------
// File:    IMessage.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantFreeServer
{
    interface IMessage
    {
        byte[] CompressAndRC5Encrypt();
        byte[] CompressAndDESEncrypt();
    }
}
