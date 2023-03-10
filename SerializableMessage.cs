using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AffenCode
{
    public abstract class SerializableMessage
    {
        private static readonly Dictionary<Type, int> SerializableTypeIndexByType = new Dictionary<Type, int>();
        private static readonly Dictionary<int, Type> SerializableTypeByTypeIndex = new Dictionary<int, Type>();
        
        private static readonly Dictionary<Type, ushort> EnumTypeIndexByType = new Dictionary<Type, ushort>();
        private static readonly Dictionary<ushort, Type> EnumTypeByTypeIndex = new Dictionary<ushort, Type>();

        private static readonly Dictionary<Type, FieldInfo[]> FieldInfos = new Dictionary<Type, FieldInfo[]>();

        static SerializableMessage()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(x => x.GetTypes());
            var serializableMessageTypes = types.Where(IsSerializableMessageType).OrderBy(x => x.FullName).ToArray();

            ushort enumIndex = 0;
            
            for (int i = 0; i < serializableMessageTypes.Length; i++)
            {
                var type = serializableMessageTypes[i];
                SerializableTypeIndexByType.Add(type, i);
                SerializableTypeByTypeIndex.Add(i, type);
                FieldInfos[type] = InitializeFieldInfos(type);

                foreach (var fieldInfo in FieldInfos[type])
                {
                    var fieldType = fieldInfo.FieldType;
                    if (fieldType.IsEnum)
                    {
                        EnumTypeByTypeIndex[enumIndex] = fieldType;
                        EnumTypeIndexByType[fieldType] = enumIndex;
                        enumIndex++;
                    }
                }
            }
        }
        
        public byte[] Serialize()
        {
            var fieldInfos = FieldInfos[GetType()];
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);

            bw.Write(BitConverter.GetBytes(GetSerializableMessageTypeIndex(this)));
            
            foreach (var fieldInfo in fieldInfos)
            {
                var value = fieldInfo.GetValue(this);
                var type = fieldInfo.FieldType;
                if (type == typeof(bool))
                {
                    bw.Write((bool) value);
                }
                else if (type == typeof(bool[]))
                {
                    var typedValue = (bool[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(typedValue[i]);
                    }
                }
                else if (type == typeof(byte))
                {
                    bw.Write((byte) value);
                }
                else if (type == typeof(byte[]))
                {
                    var typedValue = (byte[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(typedValue[i]);
                    }
                }
                else if (type == typeof(sbyte))
                {
                    bw.Write((sbyte) value);
                }
                else if (type == typeof(sbyte[]))
                {
                    var typedValue = (sbyte[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write((sbyte) typedValue[i]);
                    }
                }
                else if (type == typeof(char))
                {
                    bw.Write((char) value);
                }
                else if (type == typeof(char[]))
                {
                    var typedValue = (char[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(typedValue[i]);
                    }
                }
                else if (type == typeof(double))
                {
                    bw.Write(BitConverter.GetBytes((double) value));
                }
                else if (type == typeof(double[]))
                {
                    var typedValue = (double[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i]));
                    }
                }
                else if (type == typeof(short))
                {
                    bw.Write(BitConverter.GetBytes((short) value));
                }
                else if (type == typeof(short[]))
                {
                    var typedValue = (short[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i]));
                    }
                }
                else if (type == typeof(ushort))
                {
                    bw.Write(BitConverter.GetBytes((ushort) value));
                }
                else if (type == typeof(ushort[]))
                {
                    var typedValue = (ushort[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i]));
                    }
                }
                else if (type == typeof(int))
                {
                    bw.Write(BitConverter.GetBytes((int) value));
                }
                else if (type == typeof(int[]))
                {
                    var typedValue = (int[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i]));
                    }
                }
                else if (type == typeof(uint))
                {
                    bw.Write(BitConverter.GetBytes((uint) value));
                }
                else if (type == typeof(uint[]))
                {
                    var typedValue = (uint[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i]));
                    }
                }
                else if (type == typeof(long))
                {
                    bw.Write(BitConverter.GetBytes((long) value));
                }
                else if (type == typeof(long[]))
                {
                    var typedValue = (long[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i]));
                    }
                }
                else if (type == typeof(ulong))
                {
                    bw.Write(BitConverter.GetBytes((ulong) value));
                }
                else if (type == typeof(ulong[]))
                {
                    var typedValue = (ulong[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i]));
                    }
                }
                else if (type == typeof(float))
                {
                    bw.Write(BitConverter.GetBytes((float) value));
                }
                else if (type == typeof(float[]))
                {
                    var typedValue = (float[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i]));
                    }
                }
                else if (type == typeof(string))
                {
                    var chars = ((string) value).ToCharArray();
                    var length = chars.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(chars[i]);
                    }
                }
                else if (type == typeof(string[]))
                {
                    var typedValue = (string[]) value;

                    var arrayLength = typedValue.Length;
                    
                    bw.Write(BitConverter.GetBytes(arrayLength));

                    for (int j = 0; j < arrayLength; j++)
                    {
                        var chars = typedValue[j].ToCharArray();
                        var length = chars.Length;
                        bw.Write(BitConverter.GetBytes(length));
                        for (int i = 0; i < length; i++)
                        {
                            bw.Write(chars[i]);
                        }
                    }
                }
#if UNITY_5_3_OR_NEWER
                else if (type == typeof(Vector2))
                {
                    var typedValue = (Vector2) value;
                    bw.Write(BitConverter.GetBytes(typedValue.x));
                    bw.Write(BitConverter.GetBytes(typedValue.y));
                }
                else if (type == typeof(Vector2[]))
                {
                    var typedValue = (Vector2[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i].x));
                        bw.Write(BitConverter.GetBytes(typedValue[i].y));
                    }
                }
                else if (type == typeof(Vector3))
                {
                    var typedValue = (Vector3) value;
                    bw.Write(BitConverter.GetBytes(typedValue.x));
                    bw.Write(BitConverter.GetBytes(typedValue.y));
                    bw.Write(BitConverter.GetBytes(typedValue.z));
                }
                else if (type == typeof(Vector3[]))
                {
                    var typedValue = (Vector3[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i].x));
                        bw.Write(BitConverter.GetBytes(typedValue[i].y));
                        bw.Write(BitConverter.GetBytes(typedValue[i].z));
                    }
                }
                else if (type == typeof(Vector4))
                {
                    var typedValue = (Vector4) value;
                    bw.Write(BitConverter.GetBytes(typedValue.x));
                    bw.Write(BitConverter.GetBytes(typedValue.y));
                    bw.Write(BitConverter.GetBytes(typedValue.z));
                    bw.Write(BitConverter.GetBytes(typedValue.w));
                }
                else if (type == typeof(Vector4[]))
                {
                    var typedValue = (Vector4[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i].x));
                        bw.Write(BitConverter.GetBytes(typedValue[i].y));
                        bw.Write(BitConverter.GetBytes(typedValue[i].z));
                        bw.Write(BitConverter.GetBytes(typedValue[i].w));
                    }
                }
                else if (type == typeof(Vector2Int))
                {
                    var typedValue = (Vector2Int) value;
                    bw.Write(BitConverter.GetBytes(typedValue.x));
                    bw.Write(BitConverter.GetBytes(typedValue.y));
                }
                else if (type == typeof(Vector2Int[]))
                {
                    var typedValue = (Vector2Int[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i].x));
                        bw.Write(BitConverter.GetBytes(typedValue[i].y));
                    }
                }
                else if (type == typeof(Vector3Int))
                {
                    var typedValue = (Vector3Int) value;
                    bw.Write(BitConverter.GetBytes(typedValue.x));
                    bw.Write(BitConverter.GetBytes(typedValue.y));
                    bw.Write(BitConverter.GetBytes(typedValue.z));
                }
                else if (type == typeof(Vector3Int[]))
                {
                    var typedValue = (Vector3Int[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i].x));
                        bw.Write(BitConverter.GetBytes(typedValue[i].y));
                        bw.Write(BitConverter.GetBytes(typedValue[i].z));
                    }
                }
                else if (type == typeof(Rect))
                {
                    var typedValue = (Rect) value;
                    bw.Write(BitConverter.GetBytes(typedValue.x));
                    bw.Write(BitConverter.GetBytes(typedValue.y));
                    bw.Write(BitConverter.GetBytes(typedValue.width));
                    bw.Write(BitConverter.GetBytes(typedValue.height));
                }
                else if (type == typeof(Rect[]))
                {
                    var typedValue = (Rect[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i].x));
                        bw.Write(BitConverter.GetBytes(typedValue[i].y));
                        bw.Write(BitConverter.GetBytes(typedValue[i].width));
                        bw.Write(BitConverter.GetBytes(typedValue[i].height));
                    }
                }
                else if (type == typeof(RectInt))
                {
                    var typedValue = (RectInt) value;
                    bw.Write(BitConverter.GetBytes(typedValue.xMin));
                    bw.Write(BitConverter.GetBytes(typedValue.yMin));
                    bw.Write(BitConverter.GetBytes(typedValue.width));
                    bw.Write(BitConverter.GetBytes(typedValue.height));
                }
                else if (type == typeof(RectInt[]))
                {
                    var typedValue = (RectInt[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i].xMin));
                        bw.Write(BitConverter.GetBytes(typedValue[i].yMin));
                        bw.Write(BitConverter.GetBytes(typedValue[i].width));
                        bw.Write(BitConverter.GetBytes(typedValue[i].height));
                    }
                }
                else if (type == typeof(Quaternion))
                {
                    var typedValue = (Quaternion) value;
                    bw.Write(BitConverter.GetBytes(typedValue.x));
                    bw.Write(BitConverter.GetBytes(typedValue.y));
                    bw.Write(BitConverter.GetBytes(typedValue.z));
                    bw.Write(BitConverter.GetBytes(typedValue.w));
                }
                else if (type == typeof(Quaternion[]))
                {
                    var typedValue = (Quaternion[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i].x));
                        bw.Write(BitConverter.GetBytes(typedValue[i].y));
                        bw.Write(BitConverter.GetBytes(typedValue[i].z));
                        bw.Write(BitConverter.GetBytes(typedValue[i].w));
                    }
                }
                else if (type == typeof(Color))
                {
                    var typedValue = (Color) value;
                    bw.Write(BitConverter.GetBytes(typedValue.r));
                    bw.Write(BitConverter.GetBytes(typedValue.g));
                    bw.Write(BitConverter.GetBytes(typedValue.b));
                    bw.Write(BitConverter.GetBytes(typedValue.a));
                }
                else if (type == typeof(Color[]))
                {
                    var typedValue = (Color[])value;
                    var length = typedValue.Length;
                    bw.Write(BitConverter.GetBytes(length));
                    for (int i = 0; i < length; i++)
                    {
                        bw.Write(BitConverter.GetBytes(typedValue[i].r));
                        bw.Write(BitConverter.GetBytes(typedValue[i].g));
                        bw.Write(BitConverter.GetBytes(typedValue[i].b));
                        bw.Write(BitConverter.GetBytes(typedValue[i].a));
                    }
                }
#endif
                else if (type.IsEnum)
                {
                    var enumTypeIndex = GetEnumTypeIndex(type);
                    var enumValueIndex = (int)value;
                    bw.Write(BitConverter.GetBytes(enumTypeIndex));
                    bw.Write(BitConverter.GetBytes(enumValueIndex));
                }
            }

            var array = ms.ToArray();
            
            ms.Dispose();
            bw.Dispose();

            return array;
        }

        public static object Deserialize(byte[] bytes)
        {
            var ms = new MemoryStream(bytes);
            var br = new BinaryReader(ms);

            var serializableMessageType = GetSerializableMessageType(br.ReadInt32());
            
            var serializableMessage = Activator.CreateInstance(serializableMessageType);
            Deserialize(ms, br, serializableMessage, serializableMessageType);
            return serializableMessage;
        }

        public static T Deserialize<T>(byte[] bytes) where T : SerializableMessage, new()
        {
            var ms = new MemoryStream(bytes);
            var br = new BinaryReader(ms);

            var serializableMessageType = GetSerializableMessageType(br.ReadInt32());
            if (serializableMessageType != typeof(T))
            {
                throw new ArgumentException("Invalid SerializableMessage Type: target is " + typeof(T) + ", but binary message type is " + serializableMessageType);
            }

            var serializableMessage = new T();
            Deserialize(ms, br, serializableMessage, typeof(T));
            return serializableMessage;
        }

        private static void Deserialize(MemoryStream ms, BinaryReader br, object serializableMessage, Type serializableMessageType)
        {
            var fieldInfos = FieldInfos[serializableMessageType];
            foreach (var fieldInfo in fieldInfos)
            {
                object value = null;
                var type = fieldInfo.FieldType;
                if (type == typeof(bool))
                {
                    value = br.ReadBoolean();
                }
                else if (type == typeof(bool[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new bool[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadBoolean();
                    }
                    value = typedValue;
                }
                else if (type == typeof(byte))
                {
                    value = br.ReadByte();
                }
                else if (type == typeof(byte[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new byte[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadByte();
                    }
                    value = typedValue;
                }
                else if (type == typeof(sbyte))
                {
                    value = br.ReadSByte();
                }
                else if (type == typeof(sbyte[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new sbyte[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadSByte();
                    }
                    value = typedValue;
                }
                else if (type == typeof(char))
                {
                    value = br.ReadChar();
                }
                else if (type == typeof(char[]))
                {
                    var length = br.ReadInt32();
                    value = br.ReadChars(length);
                }
                else if (type == typeof(double))
                {
                    value = br.ReadDouble();
                }
                else if (type == typeof(double[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new double[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadDouble();
                    }
                    value = typedValue;
                }
                else if (type == typeof(short))
                {
                    value = br.ReadInt16();
                }
                else if (type == typeof(short[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new short[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadInt16();
                    }
                    value = typedValue;
                }
                else if (type == typeof(ushort))
                {
                    value = br.ReadUInt16();
                }
                else if (type == typeof(ushort[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new ushort[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadUInt16();
                    }
                    value = typedValue;
                }
                else if (type == typeof(int))
                {
                    value = br.ReadInt32();
                }
                else if (type == typeof(int[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new int[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadInt32();
                    }
                    value = typedValue;
                }
                else if (type == typeof(uint))
                {
                    value = br.ReadUInt32();
                }
                else if (type == typeof(uint[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new uint[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadUInt32();
                    }
                    value = typedValue;
                }
                else if (type == typeof(long))
                {
                    value = br.ReadInt64();
                }
                else if (type == typeof(long[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new long[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadInt64();
                    }
                    value = typedValue;
                }
                else if (type == typeof(ulong))
                {
                    value = br.ReadUInt64();
                }
                else if (type == typeof(ulong[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new ulong[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadUInt64();
                    }
                    value = typedValue;
                }
                else if (type == typeof(float))
                {
                    value = br.ReadSingle();
                }
                else if (type == typeof(float[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new float[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = br.ReadSingle();
                    }
                    value = typedValue;
                }
                else if (type == typeof(string))
                {
                    var length = br.ReadInt32();
                    value = new string(br.ReadChars(length));
                }
                else if (type == typeof(string[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new string[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        var stringLength = br.ReadInt32();
                        typedValue[i] = new string(br.ReadChars(stringLength));
                    }
                    value = typedValue;
                }
#if UNITY_5_3_OR_NEWER
                else if (type == typeof(Vector2))
                {
                    value = new Vector2(br.ReadSingle(), br.ReadSingle());
                }
                else if (type == typeof(Vector2[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new Vector2[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = new Vector2(br.ReadSingle(), br.ReadSingle());
                    }
                    value = typedValue;
                }
                else if (type == typeof(Vector3))
                {
                    value = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                }
                else if (type == typeof(Vector3[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new Vector3[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    }
                    value = typedValue;
                }
                else if (type == typeof(Vector4))
                {
                    value = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                }
                else if (type == typeof(Vector4[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new Vector4[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    }
                    value = typedValue;
                }
                else if (type == typeof(Vector2Int))
                {
                    value = new Vector2Int(br.ReadInt32(), br.ReadInt32());
                }
                else if (type == typeof(Vector2Int[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new Vector2Int[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = new Vector2Int(br.ReadInt32(), br.ReadInt32());
                    }
                    value = typedValue;
                }
                else if (type == typeof(Vector3Int))
                {
                    value = new Vector3Int(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                }
                else if (type == typeof(Vector3Int[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new Vector3Int[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = new Vector3Int(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                    }
                    value = typedValue;
                }
                else if (type == typeof(Rect))
                {
                    value = new Rect(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                }
                else if (type == typeof(Rect[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new Rect[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = new Rect(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    }
                    value = typedValue;
                }
                else if (type == typeof(RectInt))
                {
                    value = new RectInt(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                }
                else if (type == typeof(RectInt[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new RectInt[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = new RectInt(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                    }
                    value = typedValue;
                }
                else if (type == typeof(Quaternion))
                {
                    value = new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                }
                else if (type == typeof(Quaternion[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new Quaternion[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    }
                    value = typedValue;
                }
                else if (type == typeof(Color))
                {
                    value = new Color(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                }
                else if (type == typeof(Color[]))
                {
                    var arrayLength = br.ReadInt32();
                    var typedValue = new Color[arrayLength];
                    for (int i = 0; i < arrayLength; i++)
                    {
                        typedValue[i] = new Color(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    }
                    value = typedValue;
                }
#endif
                else if (type.IsEnum)
                {
                    var enumTypeIndex = br.ReadUInt16();
                    var enumValue = br.ReadInt32();
                    value = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(GetEnumType(enumTypeIndex)));
                }
                
                fieldInfo.SetValue(serializableMessage, value);
            }
            
            ms.Dispose();
            br.Dispose();
        }
        
        private static int GetSerializableMessageTypeIndex(object serializableMessage)
        {
            return SerializableTypeIndexByType[serializableMessage.GetType()];
        }

        private static Type GetSerializableMessageType(int serializableMessageTypeIndex)
        {
            return SerializableTypeByTypeIndex[serializableMessageTypeIndex];
        }
        
        private static bool IsSerializableMessageType(Type type) => typeof(SerializableMessage).IsAssignableFrom(type);
        
        private static FieldInfo[] InitializeFieldInfos(Type serializableMessageType)
        {
            if (!FieldInfos.TryGetValue(serializableMessageType, out var fieldInfos))
            {
                FieldInfos[serializableMessageType] = serializableMessageType.GetFields();
                fieldInfos = FieldInfos[serializableMessageType];
            }
            return fieldInfos;
        }

        public static ushort GetEnumTypeIndex(Type enumType)
        {
            return EnumTypeIndexByType[enumType];
        }

        public static Type GetEnumType(ushort enumTypeIndex)
        {
            return EnumTypeByTypeIndex[enumTypeIndex];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(GetType());
            sb.Append("\n");
            foreach (var fieldInfo in FieldInfos[GetType()])
            {
                sb.Append($"    {fieldInfo.Name} = {fieldInfo.GetValue(this).ToString()}\n");
            }
            return sb.ToString();
        }
    }
}