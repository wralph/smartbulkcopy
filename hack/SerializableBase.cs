using Microsoft.Data.SqlClient.Server;
using System;
using System.Data.SqlTypes;
using System.IO;

// Define the SqlMethodAttribute as it's not available in .NET 8
namespace Microsoft.Data.SqlClient.Server
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SqlMethodAttribute : Attribute
    {
        public bool IsDeterministic { get; set; }
        public bool IsPrecise { get; set; }
    }
}

// Define the IBinarySerialize interface as it's no longer available in .NET 8
namespace Microsoft.SqlServer.Types
{
    public interface IBinarySerialize
    {
        void Write(BinaryWriter w);
        void Read(BinaryReader r);
    }
}

namespace Microsoft.SqlServer.Types
{
    public class SerializableBase : IBinarySerialize, INullable
    {
        private MemoryStream _ms;
        private bool _null;

        public bool IsNull => _null; 

        public SerializableBase()
        {
            this._null = true;
            this._ms = new MemoryStream();
        }

        [SqlMethod(IsDeterministic = true, IsPrecise = true)]
        public void Write(BinaryWriter w)
        {
            if (w is null)
                throw new ArgumentException(nameof(w));

            w.Write(_ms.ToArray());
        }

        [SqlMethod(IsDeterministic = true, IsPrecise = true)]
        public void Read(BinaryReader r)
        {
            if (r is null)
                throw new ArgumentException(nameof(r));

            const int bufferSize = 1024;
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = r.Read(buffer, 0, buffer.Length)) != 0)
                _ms.Write(buffer, 0, count);            

            this._null = false;
        }
    }
}