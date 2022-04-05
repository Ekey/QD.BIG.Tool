using System;

namespace QD.Unpacker
{
    class BigFileEntry
    {
        public Int32 dwResourceTypeID { get; set; }
        public Int32 dwFlag { get; set; } // always 1 (compressed???)
        public Int32 dwFileID { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
        public Int32 dwPaddedSize { get; set; } // ???
        public Int32 dwPackageID { get; set; }
        public String m_ArchiveFile { get; set; }
        public String m_ResourceType { get; set; }
    }
}
