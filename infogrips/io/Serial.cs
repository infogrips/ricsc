using System;

namespace infogrips.IO
{
   interface Serial
   {
      //public abstract void writeTo(SerialOutputStream out);
      Object ReadFrom(SerialInputStream sis);
   }
}