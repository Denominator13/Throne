using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Throne.Framework.Cryptography;
using Throne.Framework.Utilities;

namespace Throne.World.Database.Client
{
    public static class ClientDatabaseReader
    {
        private const String ClientDbPath = "system/database/";

        public static void Read<TRecord>(String file, out Dictionary<Int32, TRecord> dict)
            where TRecord : IClientDbRecord, new()
        {
            string filePath = Path.Combine(ClientDbPath, file);

            using (var reader = new FileReader(filePath, new DatCryptography()))
            {
                dict = new Dictionary<Int32, TRecord>();

                FieldInfo[] fields = typeof (TRecord).GetFields();
                foreach (TRecord record in reader.Select(line => CreateRecord<TRecord>(line, fields)))
                    dict[record.Id] = record;
            }
        }

        public static TRecord CreateRecord<TRecord>(String info, FieldInfo[] fields, String separator = "@@")
            where TRecord : IClientDbRecord, new()
        {
            var record = new TRecord();
            IEnumerator data = Regex.Split(info, separator).GetEnumerator();
            if (data.MoveNext())
                record.Id = Convert.ToInt32(data.Current);

            foreach (FieldInfo field in fields.TakeWhile(field => data.MoveNext()))
                if (field.FieldType.IsEnum)
                    field.SetValue(record, Convert.ToInt32(data.Current));
                else
                    switch (Type.GetTypeCode(field.FieldType))
                    {
                        case TypeCode.String:
                            field.SetValue(record, data.Current);
                            break;
                        case TypeCode.Byte:
                            field.SetValue(record, Convert.ToByte(data.Current));
                            break;
                        case TypeCode.UInt16:
                            field.SetValue(record, Convert.ToUInt16(data.Current));
                            break;
                        case TypeCode.Int32:
                            field.SetValue(record, Convert.ToInt32(data.Current));
                            break;
                        case TypeCode.Boolean:
                            field.SetValue(record, ReferenceEquals(data.Current, "1") || ReferenceEquals(data.Current, "true"));
                            break;
                    }
            return record;
        }
    }
}