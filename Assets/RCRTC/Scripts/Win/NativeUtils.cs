
#if UNITY_STANDALONE_WIN
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace cn_rongcloud_rtc_unity
{
    internal class NativeUtils
    {
        internal static string PtrToString(IntPtr p)
        {
            if (p == IntPtr.Zero)
                return null;
            return Marshal.PtrToStringAnsi(p);
        }

        // Convert from IntPtr to UTF-8 string
        internal static string PtrToStringUTF8(IntPtr p)
        {
            if (p == IntPtr.Zero)
            {
                return null;
            }
            List<byte> bytes = new List<byte>();
            for (int offset = 0; ; offset++)
            {
                byte b = Marshal.ReadByte(p, offset);
                if (b == 0) break;
                else bytes.Add(b);
            }
            return Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
        }

        internal static void GetStructListByPtr<StructType>(ref StructType[] list, IntPtr ptr, uint count)
        {
            for (int i = 0; i < count; ++i)
            {
                IntPtr item_ptr = new IntPtr(ptr.ToInt64() + Marshal.SizeOf(typeof(StructType)) * i);

                if ((list != null) && (item_ptr != null))
                {
                    list[i] = (StructType)Marshal.PtrToStructure(item_ptr, typeof(StructType));
                }
            }
        }
        internal static void GetStructListByPtr<StructType>(ref StructType[] list, IntPtr ptr, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                IntPtr item_ptr = new IntPtr(ptr.ToInt64() + Marshal.SizeOf(typeof(StructType)) * i);

                if ((list != null) && (item_ptr != null))
                {
                    list[i] = (StructType)Marshal.PtrToStructure(item_ptr, typeof(StructType));
                }
            }
        }

        internal static void GetStructListByPtr<StructType>(ref List<StructType> list, IntPtr ptr, uint count)
        {
            for (uint i = 0; i < count; ++i)
            {
                IntPtr item_ptr = new IntPtr(ptr.ToInt64() + Marshal.SizeOf(typeof(StructType)) * i);

                if ((list != null) && (item_ptr != null))
                {
                    list.Add((StructType)Marshal.PtrToStructure(item_ptr, typeof(StructType)));
                }
            }
        }
        internal static StructType GetStructByPtr<StructType>(IntPtr ptr)
        {
            return (StructType)Marshal.PtrToStructure(ptr, typeof(StructType));
        }

        internal static IntPtr GetStructPointer(ValueType a)
        {
            int nSize = Marshal.SizeOf(a);                 //定义指针长度
            IntPtr pointer = Marshal.AllocHGlobal(nSize);        //定义指针
            Marshal.StructureToPtr(a, pointer, true);                //将结构体a转为结构体指针
            return pointer;
        }
        internal static IntPtr GetObjPointer(System.Object obj)
        {
            GCHandle handle = GCHandle.Alloc(obj);
            IntPtr result = (IntPtr)handle;
            handle.Free();
            return result;
        }

        internal static void ReleaseAllStructPointers(ArrayList arrayList)
        {
            foreach (System.IntPtr item in arrayList)
            {

                Marshal.FreeHGlobal(item);//释放分配的非托管内存

            }
            arrayList.Clear();
        }

        internal static void ReleaseStructPointer(IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);//释放分配的非托管内存
        }
    }
}
#endif