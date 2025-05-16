using Microsoft.Data.SqlClient.Server;
using Microsoft.SqlServer.Server;

namespace Microsoft.SqlServer.Types
{
    [SqlUserDefinedType(Format.UserDefined, IsByteOrdered = true, MaxByteSize = 892)]
    public class SqlHierarchyId : SerializableBase
    { }
}