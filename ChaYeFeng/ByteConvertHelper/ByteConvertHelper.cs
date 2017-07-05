using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

namespace ChaYeFeng
{
    /// <summary>
    /// C#下，类TcpClient，UdpClient传输信息时，都需要将信息转换为byte类型的数组进行发送，
    /// 文本实现了两种object与byte数组的转换和一种文件与byte数组转换的方式
    /// </summary>
    public class ByteConvertHelper
    {
        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换的对象</param>
        /// <returns>转换后byte数组</returns>
        public static byte[] ObjectToBytes(object obj)
        {
            try
            {
                if (obj.GetType().IsSerializable)
                {
                    byte[] buff;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        IFormatter iFormatter = new BinaryFormatter();
                        iFormatter.Serialize(ms, obj);
                        buff = ms.GetBuffer();
                    }
                    return buff;
                }
                else
                {
                    byte[] buff = new byte[Marshal.SizeOf(obj)];
                    IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
                    Marshal.StructureToPtr(obj, ptr, true);
                    return buff;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="buff">被转换byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static object BytesToObject(byte[] buff)
        {
            try
            {
                object obj;
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(buff, 0, buff.Length);
                    ms.Position = 0;
                    IFormatter iFormatter = new BinaryFormatter();
                    obj = iFormatter.Deserialize(ms);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
