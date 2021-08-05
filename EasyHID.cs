using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MecaniqueUK
{
    class EasyHID
    {
        // HID specific...
        public const UInt32 VENDOR_ID = 0X04D8;
        public const UInt32 PRODUCT_ID = 0X000B;
        public const int BUFFER_IN_SIZE = 32;
        public const int BUFFER_OUT_SIZE = 32;

        // HID events...
        private const int WM_APP = 0x8000;
        public const int WM_HID_EVENT = WM_APP + 200;
        public const int NOTIFY_PLUGGED = 0x0001;
        public const int NOTIFY_UNPLUGGED = 0x0002;
        public const int NOTIFY_CHANGED = 0x0003;
        public const int NOTIFY_READ = 0x0004;

        // HID interface...
        [DllImport("mcHID.dll")]
        public static extern bool Connect(IntPtr pHostWin);
        [DllImport("mcHID.dll")]
        public static extern bool Disconnect();
        [DllImport("mcHID.dll")]
        public static extern UInt32 GetItem(UInt32 pIndex);
        [DllImport("mcHID.dll")]
        public static extern UInt32 GetItemCount();
        [DllImport("mcHID.dll")]
        public static extern bool Read(UInt32 pHandle, IntPtr pData);
        [DllImport("mcHID.dll")]
        private static extern bool Write(UInt32 pHandle, IntPtr pData);
        [DllImport("mcHID.dll")]
        private static extern bool ReadEx(UInt32 pVendorId, UInt32 pProductId, IntPtr pData);
        [DllImport("mcHID.dll")]
        private static extern bool WriteEx(UInt32 pVendorId, UInt32 pProductId, IntPtr pData);
        [DllImport("mcHID.dll")]
        public static extern UInt32 GetHandle(UInt32 pVendorID, UInt32 pProductId);
        [DllImport("mcHID.dll")]
        public static extern UInt32 GetVendorID(UInt32 pHandle);
        [DllImport("mcHID.dll")]
        public static extern UInt32 GetProductID(UInt32 pHandle);
        [DllImport("mcHID.dll")]
        public static extern UInt32 GetVersionID(UInt32 pHandle);
        [DllImport("mcHID.dll")]
        public static extern UInt32 GetInputReportLength(UInt32 pHandle);
        [DllImport("mcHID.dll")]
        public static extern UInt32 GetOutputReportLength(UInt32 pHandle);
        [DllImport("mcHID.dll")]
        public static extern void SetReadNotify(UInt32 pHandle, bool pValue);
        [DllImport("mcHID.dll")]
        public static extern bool IsReadNotifyEnabled(UInt32 pHandle);
        [DllImport("mcHID.dll")]
        public static extern bool IsAvailable(UInt32 pVendorId, UInt32 pProductId);

        // Managed version of the read/write functions.
        public static bool Read(UInt32 pHandle, out byte[] pData)
        {
            IntPtr unmanagedBuffer = Marshal.AllocHGlobal(BUFFER_IN_SIZE);
            bool result = Read(pHandle, unmanagedBuffer);

            try { pData = new byte[BUFFER_IN_SIZE]; Marshal.Copy(unmanagedBuffer, pData, 0, BUFFER_IN_SIZE); }
            finally { Marshal.FreeHGlobal(unmanagedBuffer); }

            return result;
        }

        public static bool Write(UInt32 pHandle, byte[] pData)
        {
            IntPtr unmanagedBuffer = Marshal.AllocHGlobal(BUFFER_OUT_SIZE);
            bool result;

            try { Marshal.Copy(pData, 0, unmanagedBuffer, BUFFER_OUT_SIZE); result = Write(pHandle, unmanagedBuffer); }
            finally { Marshal.FreeHGlobal(unmanagedBuffer); }

            return result;
        }

        }
    }
