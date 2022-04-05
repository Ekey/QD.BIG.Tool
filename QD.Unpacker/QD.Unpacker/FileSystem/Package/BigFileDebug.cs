using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace QD.Unpacker
{
    class BigFileDebug
    {
        public static Boolean bDebugFileLoaded = false;
        public static Dictionary<String, String> m_DebugTable = new Dictionary<String, String>();

        public static void iLoad(String m_DebugFile)
        {
            using (FileStream TDebugStream = File.OpenRead(m_DebugFile))
            {
                var m_Header = new BigFileDebugHeader();
                m_Header.m_Magic = Encoding.ASCII.GetString(TDebugStream.ReadBytes(20));
                m_Header.dwVersion = TDebugStream.ReadInt32(true);
                m_Header.dwTotalFiles = TDebugStream.ReadInt32(true);

                if (m_Header.m_Magic != "QUANTICDREAMTABINDEX")
                {
                    throw new Exception("[ERROR]: Invalid magic of debug file!");
                }

                if (m_Header.dwVersion != 13 && m_Header.dwVersion != 17)
                {
                    throw new Exception("[ERROR]: Invalid version of debug file!");
                }

                if (m_Header.dwVersion == 17)
                {
                    m_Header.dwTotalFiles = TDebugStream.ReadInt32(true);
                }

                m_DebugTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    if (m_Header.dwVersion == 13)
                    {
                        Int32 dwResourceTypeID = TDebugStream.ReadInt32(true);
                        Int32 dwFlag = TDebugStream.ReadInt32(true);
                        Int32 dwFileID = TDebugStream.ReadInt32(true);
                        Int32 dwResourceNameLength = TDebugStream.ReadInt32(true);
                        String m_ResourceName = Encoding.ASCII.GetString(TDebugStream.ReadBytes(dwResourceNameLength));
                        UInt32 dwOffset = TDebugStream.ReadUInt32(true);
                        Int32 dwSize = TDebugStream.ReadInt32(true);
                        Int32 dwPaddedSize = TDebugStream.ReadInt32(true);
                        Int32 dwUnknown1 = TDebugStream.ReadInt32(true);
                        Int32 dwUnknown2 = TDebugStream.ReadInt32(true);
                        Int32 dwUnknown3 = TDebugStream.ReadInt32(true);

                        String m_ResourceType = BigFileTypes.iGetResourceType(dwResourceTypeID);

                        var TDebugEntry = new BigFileDebugEntryV13
                        {
                            dwResourceTypeID = dwResourceTypeID,
                            dwFlag = dwFlag,
                            dwFileID = dwFileID,
                            dwResourceNameLength = dwResourceNameLength,
                            dwOffset = dwOffset,
                            dwSize = dwSize,
                            dwPaddedSize = dwPaddedSize,
                            dwUnknown1 = dwUnknown1,
                            dwUnknown2 = dwUnknown2,
                            dwUnknown3 = dwUnknown3,
                            m_ResourceType = m_ResourceType,
                            m_ResourceName = m_ResourceName,
                        };

                        String m_UniqueID = TDebugEntry.dwResourceTypeID.ToString() + "_" + TDebugEntry.dwFileID.ToString() + "_" + TDebugEntry.dwSize.ToString();

                        if (TDebugEntry.dwPaddedSize != 0 || TDebugEntry.dwSize != 0)
                        {
                            m_DebugTable.Add(m_UniqueID, m_ResourceName);
                        }
                    }
                    else if (m_Header.dwVersion == 17)
                    {
                        Int32 dwResourceTypeID = TDebugStream.ReadInt32(true);
                        Int32 dwFlag = TDebugStream.ReadInt32(true);
                        Int32 dwFileID = TDebugStream.ReadInt32(true);
                        Int32 dwUnknown1 = TDebugStream.ReadInt32(true);
                        UInt32 dwUnknown2 = TDebugStream.ReadUInt32(true);
                        Int32 dwResourceNameLength = TDebugStream.ReadInt32(true);
                        String m_ResourceName = Encoding.ASCII.GetString(TDebugStream.ReadBytes(dwResourceNameLength));
                        UInt32 dwOffset = TDebugStream.ReadUInt32(true);
                        Int32 dwSize = TDebugStream.ReadInt32(true);
                        Int32 dwPaddedSize = TDebugStream.ReadInt32(true);
                        Int32 dwUnknown3 = TDebugStream.ReadInt32(true);
                        Int32 dwUnknown4 = TDebugStream.ReadInt32(true);
                        Int32 dwUnknown5 = TDebugStream.ReadInt32(true);

                        String m_ResourceType = BigFileTypes.iGetResourceType(dwResourceTypeID);

                        var TDebugEntry = new BigFileDebugEntryV17
                        {
                            dwResourceTypeID = dwResourceTypeID,
                            dwFlag = dwFlag,
                            dwFileID = dwFileID,
                            dwUnknown1 = dwUnknown1,
                            dwUnknown2 = dwUnknown2,
                            dwResourceNameLength = dwResourceNameLength,
                            dwOffset = dwOffset,
                            dwSize = dwSize,
                            dwPaddedSize = dwPaddedSize,
                            dwUnknown3 = dwUnknown3,
                            dwUnknown4 = dwUnknown4,
                            dwUnknown5 = dwUnknown5,
                            m_ResourceType = m_ResourceType,
                            m_ResourceName = m_ResourceName,
                        };

                        String m_UniqueID = TDebugEntry.dwResourceTypeID.ToString() + "_" + TDebugEntry.dwFileID.ToString() + "_" + TDebugEntry.dwSize.ToString();

                        if (TDebugEntry.dwPaddedSize != 0 || TDebugEntry.dwSize != 0)
                        {
                            m_DebugTable.Add(m_UniqueID, m_ResourceName);
                        }
                    }
                }

                TDebugStream.Dispose();

                bDebugFileLoaded = true;
            }
        }

        public static String iGetDebugResourceName(Int32 dwResourceTypeID, Int32 dwFileID, Int32 dwSize)
        {
            String m_ResourceName = null;
            String m_UniqueID = dwResourceTypeID.ToString() + "_" + dwFileID.ToString() + "_" + dwSize.ToString();
            if (m_DebugTable.ContainsKey(m_UniqueID))
            {
                m_DebugTable.TryGetValue(m_UniqueID, out m_ResourceName);
            }

            return m_ResourceName;
        }
    }
}
