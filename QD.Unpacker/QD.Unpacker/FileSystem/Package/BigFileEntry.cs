using System;

namespace QD.Unpacker
{
    class BigFileEntry
    {
        public Int32 dwResourceTypeID { get; set; }
        public Int32 dwFlag { get; set; } // 1 (???)
        public Int32 dwFileID { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
        public Int32 dwPaddedSize { get; set; } // ???
        public Int32 dwPackageID { get; set; }
        public String m_ArchiveFile { get; set; }
        public String m_ResourceType { get; set; }
        public String m_ResourceName { get; set; } // Only if debug file are present
    }

    class BigFileDebugEntryV13
    {
        public Int32 dwResourceTypeID { get; set; }
        public Int32 dwFlag { get; set; } // 1 (???)
        public Int32 dwFileID { get; set; }
        public Int32 dwResourceNameLength { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
        public Int32 dwPaddedSize { get; set; }
        public Int32 dwUnknown1 { get; set; } // (???)
        public Int32 dwUnknown2 { get; set; } // (???)
        public Int32 dwUnknown3 { get; set; } // (???)
        public String m_ArchiveFile { get; set; }
        public String m_ResourceType { get; set; }
        public String m_ResourceName { get; set; }
    }

    class BigFileDebugEntryV17
    {
        public Int32 dwResourceTypeID { get; set; }
        public Int32 dwFlag { get; set; } // 1 (???)
        public Int32 dwFileID { get; set; }
        public Int32 dwUnknown1 { get; set; }
        public UInt32 dwUnknown2 { get; set; } // Hash ???
        public Int32 dwResourceNameLength { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
        public Int32 dwPaddedSize { get; set; }
        public Int32 dwUnknown3 { get; set; }
        public Int32 dwUnknown4 { get; set; }
        public Int32 dwUnknown5 { get; set; }
        public String m_ArchiveFile { get; set; }
        public String m_ResourceType { get; set; }
        public String m_ResourceName { get; set; }
    }
}
