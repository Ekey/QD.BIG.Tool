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
                BigFileTypes.iInitResourceTypes();

                var m_Header = new BigFileHeader();
                m_Header.m_Magic = Encoding.ASCII.GetString(TIndexStream.ReadBytes(20));
                m_Header.dwVersion = TIndexStream.ReadInt32(true);
                m_Header.dwUnknown = TIndexStream.ReadInt32(true);

                switch (m_Header.dwVersion)
                {
                    case 13: m_Header.m_SubMagic = Encoding.ASCII.GetString(TIndexStream.ReadBytes(72)); break;
                    case 18: m_Header.m_SubMagic = Encoding.ASCII.GetString(TIndexStream.ReadBytes(72 + 1)).TrimEnd('\0'); break;
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
                    String m_ArchiveFile = null;

                    if (dwPackageID == 0)
                    {
                        switch (m_Header.dwVersion)
                        {
                            case 13: m_ArchiveFile = "BigFile_WIN.dat"; break;
                            case 18: m_ArchiveFile = "BigFile_PC.dat"; break;
                        }
                    }
                    else
                    {
                        switch (m_Header.dwVersion)
                        {
                            case 13: m_ArchiveFile = "BigFile_WIN.d" + dwPackageID.ToString("D2"); break;
                            case 18: m_ArchiveFile = "BigFile_PC.d" + dwPackageID.ToString("D2"); break;
                        }
                    }
                    
                    String m_ResourceType = BigFileTypes.iGetResourceType(dwResourceTypeID);


                    var TEntry = new BigFileEntry
                    {
                        dwResourceTypeID = dwResourceTypeID,
                        dwFlag = dwFlag,
                        dwFileID = dwFileID,
                        dwOffset = dwOffset,
                        dwSize = dwSize,
                        dwPaddedSize = dwPaddedSize,
                        dwPackageID = dwPackageID,
                        m_ArchiveFile = Path.GetDirectoryName(m_IndexFile) + @"\" + m_ArchiveFile,
                        m_ResourceType = m_ResourceType,
                    };

                    m_EntryTable.Add(TEntry);
                }

                TIndexStream.Dispose();

                foreach (var m_Entry in m_EntryTable)
                {
                    if (!File.Exists(m_Entry.m_ArchiveFile))
                    {
                        Utils.iSetError("[ERROR]: Input archive -> " + m_Entry.m_ArchiveFile + " <- does not exist");
                        return;
                    }

                    String m_FileName = m_Entry.m_ResourceType + @"\" + m_Entry.dwFileID.ToString();
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    using (FileStream TArchiveStream = File.OpenRead(m_Entry.m_ArchiveFile))
                    {
                        TArchiveStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                        if (m_Entry.dwPaddedSize != 0)
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
