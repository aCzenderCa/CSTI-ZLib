using System;

namespace CSTI_ZLib.Utils;

public static class ObjectTransformUtils
{
    public static object? TransformObject(this object obj, TypeCode from, TypeCode to)
    {
        switch (from)
        {
            case TypeCode.Int64:
                var l = (long)obj;
                switch (to)
                {
                    case TypeCode.SByte:
                        return (sbyte)l;
                    case TypeCode.Byte:
                        return (byte)l;
                    case TypeCode.Int16:
                        return (short)l;
                    case TypeCode.UInt16:
                        return (ushort)l;
                    case TypeCode.Int32:
                        return (int)l;
                    case TypeCode.UInt32:
                        return (uint)l;
                    case TypeCode.Int64:
                        return l;
                    case TypeCode.UInt64:
                        return (ulong)l;
                    case TypeCode.Single:
                        return (float)l;
                    case TypeCode.Double:
                        return (double)l;
                    case TypeCode.Decimal:
                        return (decimal)l;
                }

                return obj;
            case TypeCode.Double:
                var d = (double)obj;
                switch (to)
                {
                    case TypeCode.SByte:
                        return (sbyte)d;
                    case TypeCode.Byte:
                        return (byte)d;
                    case TypeCode.Int16:
                        return (short)d;
                    case TypeCode.UInt16:
                        return (ushort)d;
                    case TypeCode.Int32:
                        return (int)d;
                    case TypeCode.UInt32:
                        return (uint)d;
                    case TypeCode.Int64:
                        return (long)d;
                    case TypeCode.UInt64:
                        return (ulong)d;
                    case TypeCode.Single:
                        return (float)d;
                    case TypeCode.Double:
                        return d;
                    case TypeCode.Decimal:
                        return (decimal)d;
                }

                return obj;
            case TypeCode.Decimal:
            case TypeCode.DateTime:
            case TypeCode.Single:
            case TypeCode.UInt64:
            case TypeCode.UInt32:
            case TypeCode.Int32:
            case TypeCode.UInt16:
            case TypeCode.Int16:
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.Char:
            case TypeCode.Boolean:
            case TypeCode.Object:
            case TypeCode.String:
                return obj;
            case TypeCode.DBNull:
            case TypeCode.Empty:
            default:
                return null;
        }
    }
}