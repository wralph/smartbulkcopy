using Microsoft.SqlServer.Server;

namespace Microsoft.SqlServer.Types
{
    [SqlUserDefinedType(Format.UserDefined, IsByteOrdered = false, MaxByteSize = -1, IsFixedLength = false)]
    public class SqlGeometry : SerializableBase
    { }
}