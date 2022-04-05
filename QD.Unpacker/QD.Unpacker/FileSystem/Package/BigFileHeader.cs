using System;

namespace QD.Unpacker
{
    class BigFileHeader
    {
        public String m_Magic { get; set; } // QUANTICDREAMTABINDEX
        public Int32 dwVersion { get; set; } // 13 - Heavy Rain, 18 - Detroit: Become Human
        public Int32 dwUnknown { get; set; } // Memory size ???? > 13 > 65535, v18 > 11548767
        public String m_SubMagic { get; set; } // ZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEP (if version > 18 = null terminated string)
        public Int32 dwTotalFiles { get; set; }
    }
}
