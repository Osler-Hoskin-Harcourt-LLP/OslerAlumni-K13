using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;

namespace ECA.Core.Definitions
{
    public enum KenticoClassType
    {
        [EnumStringRepresentation(DataClassInfo.OBJECT_TYPE)]
        Any = 0,
        [EnumStringRepresentation(DocumentTypeInfo.OBJECT_TYPE_DOCUMENTTYPE)]
        PageType = 1,
        [EnumStringRepresentation(CustomTableInfo.OBJECT_TYPE_CUSTOMTABLE)]
        CustomTable = 2
    }
}
