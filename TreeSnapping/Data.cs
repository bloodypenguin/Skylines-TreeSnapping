﻿using ColossalFramework.IO;

namespace TreeSnapping
{
    public class Data : IDataContainer
    {
        public void Serialize(DataSerializer s)
        {
            var instance = TreeManager.instance;
            var items = instance.m_trees.m_buffer;
            s.WriteInt32(items.Length);
            var @ushort = EncodedArray.UShort.BeginWrite(s);
            for (var index = 0; index < items.Length; index++)
                @ushort.Write(items[index].m_posY);
            @ushort.EndWrite();
        }

        public void Deserialize(DataSerializer s)
        {
            var instance = TreeManager.instance;
            var arraySize = s.ReadInt32();
            var @ushort = EncodedArray.UShort.BeginRead(s);
            for (var index = 0; index < arraySize; ++index)
            {
                var item = @ushort.Read();
                var prop = instance.m_trees.m_buffer[index];
                prop.m_posY = item;
                instance.m_trees.m_buffer[index] = prop;
            }
            @ushort.EndRead();
        }

        public void AfterDeserialize(DataSerializer s)
        {
        }
    }
}