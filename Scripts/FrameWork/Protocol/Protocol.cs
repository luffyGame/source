using System;
using System.Collections.Generic;

namespace FrameWork
{
    public abstract class Protocol : Message
    {
        public abstract ByteBuf Serialize();
        public abstract void Deserialize(ByteBuf buf, int start, int count);
    }
}
