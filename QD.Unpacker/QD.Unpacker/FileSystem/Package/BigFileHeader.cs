using System;

namespace QD.Unpacker
{
    class BigFileHeader
    {
        public String m_Magic { get; set; } // QUANTICDREAMTABINDEX
        public Int32 dwVersion { get; set; } // 13 - Heavy Rain, 17 - BEYOND Two Souls, 18 - Detroit: Become Human
        public Int32 dwUnknown { get; set; } // 13 = 65535, 17 = 0, 18 = 11548767
        public String m_SubMagic { get; set; } // ZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEP
        public Int32 dwTotalFiles { get; set; }
    }

    class BigFileDebugHeader
    {
        public String m_Magic { get; set; } // QUANTICDREAMTABINDEX
        public Int32 dwVersion { get; set; } // 13 - Heavy Rain, 17 - BEYOND Two Souls
        public Int32 dwTotalFiles { get; set; }
    }
}
