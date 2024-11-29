﻿using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace Core.Comm
{
    public class ZipClass
    {
        /// <summary>
        /// 递归压缩文件夹方法
        /// </summary>
        /// <param name="FolderToZip"></param>
        /// <param name="s"></param>
        /// <param name="ParentFolderName"></param>
        private static bool ZipFileDictory(string FolderToZip, ZipOutputStream s, string ParentFolderName)
        {
            bool res = true;
            string[] folders, filenames;
            FileStream fs = null;
            Crc32 crc = new Crc32();

            try
            {
                //创建当前文件夹
                ZipEntry entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/"));
                s.PutNextEntry(entry);
                s.Flush();

                //先压缩文件，再递归压缩文件夹 
                filenames = Directory.GetFiles(FolderToZip);
                foreach (string file in filenames)
                {
                    //打开压缩文件
                    fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    _ = fs.Read(buffer, 0, buffer.Length);
                    entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/" + Path.GetFileName(file)));

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    entry.Crc = crc.Value;

                    s.PutNextEntry(entry);
                    s.Write(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                res = false;
            }
            finally
            {
                fs?.Close();
                //if (entry != null) { }
                GC.Collect();
                GC.Collect(1);
            }

            folders = Directory.GetDirectories(FolderToZip);
            foreach (string folder in folders)
                if (!ZipFileDictory(folder, s, Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip))))
                    return false;

            return res;
        }

        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="FolderToZip">待压缩的文件夹，全路径格式</param>
        /// <param name="ZipedFile">压缩后的文件名，全路径格式</param>
        /// <returns></returns>
        public static bool ZipFileDictory(string FolderToZip, string ZipedFile, string Password)
        {
            bool res;
            if (!Directory.Exists(FolderToZip))
                return false;

            ZipOutputStream s = new ZipOutputStream(File.Create(ZipedFile));
            s.SetLevel(6);
            s.Password = Password;

            res = ZipFileDictory(FolderToZip, s, "");

            s.Finish();
            s.Close();

            return res;
        }

        /// <summary>
        /// 解压功能(解压压缩文件到指定目录)
        /// </summary>
        /// <param name="FileToUpZip">待解压的文件</param>
        /// <param name="ZipedFolder">指定解压目标目录</param>
        public static bool UnZip(string FileToUpZip, string ZipedFolder, string Password)
        {
            if (!File.Exists(FileToUpZip))
                return true;

            if (!Directory.Exists(ZipedFolder))
                _ = Directory.CreateDirectory(ZipedFolder);

            ZipInputStream s = null;
            string fileName;
            FileStream streamWriter = null;
            try
            {
                s = new ZipInputStream(File.OpenRead(FileToUpZip));
                s.Password = Password;
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.Name != String.Empty)
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        ///判断文件路径是否是文件夹
                        if (fileName.EndsWith("/") || fileName.EndsWith("//"))
                        {
                            _ = Directory.CreateDirectory(fileName);
                            continue;
                        }

                        streamWriter = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                                streamWriter.Write(data, 0, size);
                            else break;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                streamWriter?.Close();
                // if (theEntry != null) { }
                s?.Close();
                GC.Collect();
                GC.Collect(1);
            }
        }
    }
}
