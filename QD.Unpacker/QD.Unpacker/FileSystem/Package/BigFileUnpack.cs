using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace QD.Unpacker
{
    class BigFileUnpack
    {
        static List<BigFileEntry> m_EntryTable = new List<BigFileEntry>();

        public static void iDoIt(String m_IndexFile, String m_DstFolder)
        {
            using (FileStream TIndexStream = File.OpenRead(m_IndexFile))
            {
                var m_Header = new BigFileHeader();
                m_Header.m_Magic = Encoding.ASCII.GetString(TIndexStream.ReadBytes(20));
                m_Header.dwVersion = TIndexStream.ReadInt32(true);
                m_Header.dwUnknown = TIndexStream.ReadInt32(true);

                switch (m_Header.dwVersion)
                {
                    case 13:
                    case 17: m_Header.m_SubMagic = Encoding.ASCII.GetString(TIndexStream.ReadBytes(72)); break;
                    case 18: m_Header.m_SubMagic = Encoding.ASCII.GetString(TIndexStream.ReadBytes(72)); TIndexStream.Seek(1, SeekOrigin.Current); break;
                    default: throw new Exception("[ERROR]: Invalid version of index file!");
                }

                m_Header.dwTotalFiles = TIndexStream.ReadInt32(true);

                if (m_Header.m_Magic != "QUANTICDREAMTABINDEX")
                {
                    throw new Exception("[ERROR]: Invalid magic of index file!");
                }

                if (m_Header.m_SubMagic != "ZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEPZEP")
                {
                    throw new Exception("[ERROR]: Invalid sub magic of index file!");
                }

                String m_BaseFile = Path.GetDirectoryName(m_IndexFile) + @"\" + Path.GetFileNameWithoutExtension(m_IndexFile);

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    Int32 dwResourceTypeID = TIndexStream.ReadInt32(true);
                    Int32 dwFlag = TIndexStream.ReadInt32(true);
                    Int32 dwFileID = TIndexStream.ReadInt32(true);
                    UInt32 dwOffset = TIndexStream.ReadUInt32(true);
                    Int32 dwSize = TIndexStream.ReadInt32(true);
                    Int32 dwPaddedSize = TIndexStream.ReadInt32(true);
                    Int32 dwPackageID = TIndexStream.ReadInt32(true);
                    String m_ResourceName = "";
                    String m_ArchiveFile = null;

                    if (dwPackageID == 0)
                    {
                        m_ArchiveFile += m_BaseFile + ".dat";
                    }
                    else
                    {
                        m_ArchiveFile += m_BaseFile + ".d" + dwPackageID.ToString("D2");
                    }
                    
                    String m_ResourceType = BigFileTypes.iGetResourceType(dwResourceTypeID);

                    if (BigFileDebug.bDebugFileLoaded)
                    {
                        m_ResourceName = m_ResourceType + @"\" + BigFileDebug.iGetDebugResourceName(dwResourceTypeID, dwFileID, dwSize);
                    }
                    else
                    {
                        m_ResourceName = m_ResourceType + @"\" + dwFileID.ToString();
                    }

                    var TEntry = new BigFileEntry
                    {
                        dwResourceTypeID = dwResourceTypeID,
                        dwFlag = dwFlag,
                        dwFileID = dwFileID,
                        dwOffset = dwOffset,
                        dwSize = dwSize,
                        dwPaddedSize = dwPaddedSize,
                        dwPackageID = dwPackageID,
                        m_ArchiveFile = m_ArchiveFile,
                        m_ResourceType = m_ResourceType,
                        m_ResourceName = m_ResourceName,
                    };

                    if (TEntry.dwPaddedSize != 0 || TEntry.dwSize != 0)
                    {
                        m_EntryTable.Add(TEntry);
                    }
                }

                TIndexStream.Dispose();

                foreach (var m_Entry in m_EntryTable)
                {
                    if (!File.Exists(m_Entry.m_ArchiveFile))
                    {
                        Utils.iSetError("[ERROR]: Input archive -> " + m_Entry.m_ArchiveFile + " <- does not exist");
                        return;
                    }

                    String m_FileName = m_Entry.m_ResourceName;
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    using (FileStream TArchiveStream = File.OpenRead(m_Entry.m_ArchiveFile))
                    {
                        TArchiveStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                        if (m_Entry.dwPaddedSize > m_Entry.dwSize)
                        {
                            var lpBuffer = TArchiveStream.ReadBytes(m_Entry.dwPaddedSize);
                            File.WriteAllBytes(m_FullPath, lpBuffer);
                        }
                        else
                        {
                            var lpBuffer = TArchiveStream.ReadBytes(m_Entry.dwSize);
                            File.WriteAllBytes(m_FullPath, lpBuffer);
                        }

                        TArchiveStream.Dispose();
                    }
                }
            }
        }
    }
}
