using Bc.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bc.Common.Helpers
{
    /// <summary>
    /// 通用常规文件操作
    /// </summary>
    public class FileCommonHelper
    {
        //使用BZIP解压文件的方法


        #region 读取指定目录下的文件(包含子文件夹及文件)
        /// <summary>
        /// 读取指定目录下的文件(包含子文件夹及文件)返回相对路径列表
        /// </summary>
        /// <param name="Dir">目录</param>
        /// <param name="IncludeNameReg">提取通过验证的文件(只读取通过验证的文件名)</param>
        public static List<string> ReadFiles(string Dir, Regex IncludeNameReg = null)
        {
            return ReadFiles(new DirectoryInfo(Dir), IncludeNameReg);
        }
        /// <summary>
        /// 读取指定目录下的文件(包含子文件夹及文件)返回相对路径列表
        /// </summary>
        /// <param name="Dir">目录</param>
        /// <param name="IncludeNameReg">提取通过验证的文件(只验证文件名)</param>
        public static List<string> ReadFiles(DirectoryInfo Dir, Regex IncludeNameReg = null)
        {
            List<string> files = new List<string>();
            Func<FileInfo, bool> fn;
            if (IncludeNameReg == null) { fn = (f) => true; }
            else { fn = (f) => IncludeNameReg.IsMatch(f.Name); }
            ReadFiles(Dir, ref files, fn, "");
            return files;
        }
        private static void ReadFiles(DirectoryInfo dir, ref List<string> files, Func<FileInfo, bool> filter, string relPath)
        {
            if (!dir.Exists) { return; }
            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ReadFiles(di, ref files, filter, relPath + di.Name + "\\");
            }
            foreach (FileInfo fi in dir.GetFiles())
            {
                if (filter(fi)) { files.Add(relPath + fi.Name); }
            }
        }
        /// <summary>
        /// 读取指定目录下的文件信息列表
        /// </summary>
        /// <param name="Dir">目录</param>
        /// <param name="FileEval">文件处理方法</param>
        public static void ReadFiles(string Dir, Action<DirectoryInfo, FileInfo> FileEval)
        {
            ReadFiles(new DirectoryInfo(Dir), FileEval, t => { });
        }
        /// <summary>
        /// 读取指定目录下的文件信息列表
        /// </summary>
        /// <param name="Dir">目录</param>
        /// <param name="FileEval">文件处理方法</param>
        /// <param name="DirEval">目录处理方法</param>
        public static void ReadFiles(DirectoryInfo Dir, Action<DirectoryInfo, FileInfo> FileEval, Action<DirectoryInfo> DirEval)
        {
            if (!Dir.Exists) { return; }
            foreach (DirectoryInfo d in Dir.GetDirectories())
            {
                ReadFiles(d, FileEval, DirEval);
            }
            foreach (FileInfo f in Dir.GetFiles())
            {
                FileEval(Dir, f);
            }
        }
        #endregion

        #region 复制文件夹(包含子文件夹及文件)
        public static void FileMove(string source, string destdir, List<string> excludedir, List<string> excludepath, string relativedir = "", bool isdelete = false)
        {
            try
            {
                var newsource = source + relativedir;
                var newdestdir = destdir + relativedir;
                if (Directory.Exists(newdestdir) && !isdelete)
                    Directory.Delete(newdestdir, true);
                Directory.CreateDirectory(newdestdir);
                var fs = Directory.GetFiles(newsource);
                for (var i = 0; i < fs.Length; i++)
                {
                    if (excludepath != null && excludepath.Contains(fs[i]))
                    {
                        continue;
                    }
                    var destpath = newdestdir + fs[i].Replace(newsource, "");
                    File.Copy(fs[i], destpath, true);
                    if (isdelete)
                    {
                        File.Delete(fs[i]);
                    }
                }
                var dirs = Directory.GetDirectories(newsource);
                for (var i = 0; i < dirs.Length; i++)
                {
                    if (excludedir != null && excludedir.Contains(dirs[i] + "\\"))
                    {
                        continue;
                    }
                    var relativedir2 = dirs[i].Replace(source, "");
                    FileMove(source, destdir, excludedir, excludepath, relativedir2, isdelete);


                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("FileMove=>" + ex.ToStr());
            }
        }

        /// <summary>
        /// 获取文件夹对象
        /// </summary>
        /// <param name="Dir">文件夹路径</param>
        /// <returns>文件夹对象</returns>
        public static DirectoryInfo GetDir(string Dir)
        {
            DirectoryInfo di;
            if (!Directory.Exists(Dir)) { di = Directory.CreateDirectory(Dir); }
            else { di = new DirectoryInfo(Dir); }
            return di;
        }
        /// <summary>
        /// 复制文件夹、子文件夹及文件(包含子文件夹及文件)
        /// </summary>
        /// <param name="SrcDir">源目录信息</param>
        /// <param name="TgtDir">目标目录信息</param>
        /// <param name="IsReplace">是否替换目标目录中已存在文件</param>
        /// <param name="IncludeFileReg">包含文件正则条件</param>
        /// <returns>文件个数</returns>
        public static int CopyDir(string SrcDir, string TgtDir, bool IsReplace, Regex IncludeFileReg = null)
        {
            return CopyDir(new DirectoryInfo(SrcDir), TgtDir, IsReplace, null, IncludeFileReg);
        }
        /// <summary>
        /// 复制文件夹、子文件夹及文件(包含子文件夹及文件)
        /// </summary>
        /// <param name="SrcDirInfo">源目录信息</param>
        /// <param name="TgtDir">目标目录信息</param>
        /// <param name="IsReplace">是否替换目标目录中已存在文件</param>
        /// <param name="IncludeDirReg">包含文件夹正则条件</param>
        /// <param name="IncludeFileReg">包含文件正则条件</param>
        /// <returns>文件个数</returns>
        public static int CopyDir(DirectoryInfo SrcDirInfo, string TgtDir, bool IsReplace, Regex IncludeDirReg = null, Regex IncludeFileReg = null)
        {
            Func<DirectoryInfo, bool> fnd;
            Func<FileInfo, bool> fnf;
            if (IncludeDirReg == null)
            {
                fnd = d => false;
            }
            else
            {
                fnd = d => !IncludeDirReg.IsMatch(d.Name);
            }
            if (IncludeFileReg == null)
            {
                fnf = d => false;
            }
            else
            {
                fnf = d => !IncludeDirReg.IsMatch(d.Name);
            }
            return CopyDir(SrcDirInfo, GetDir(TgtDir), IsReplace, fnd, fnf);
        }
        /// <summary>
        /// 移动文件夹、子文件夹及文件(包含子文件夹及文件)
        /// </summary>
        /// <param name="SrcDir">源目录</param>
        /// <param name="TgtDir">目标目录</param>
        /// <param name="IsReplace">是否替换</param>
        /// <param name="ExcludeDirFn">排除目录函数</param>
        /// <param name="ExcludeFileFn">排除文件函数</param>
        /// <param name="RenameFileNameFn">重写文件名函数(传入参数只包含文件名+扩展名，不包含目录)</param>
        /// <returns>数量</returns>
        public static int CopyDir(string SrcDir, string TgtDir, bool IsReplace,
            Func<DirectoryInfo, bool> ExcludeDirFn, Func<FileInfo, bool> ExcludeFileFn, Func<string, string> RenameFileNameFn = null)
        {
            return CopyDir(new DirectoryInfo(SrcDir), new DirectoryInfo(TgtDir), IsReplace, ExcludeDirFn, ExcludeFileFn, RenameFileNameFn);
        }
        /// <summary>
        /// 移动文件夹、子文件夹及文件(包含子文件夹及文件)
        /// </summary>
        /// <param name="SrcDirInfo">源目录信息</param>
        /// <param name="TgtDirInfo">目标目录信息</param>
        /// <param name="IsReplace">是否替换</param>
        /// <param name="ExcludeDirFn">排除目录函数</param>
        /// <param name="ExcludeFileFn">排除文件函数</param>
        /// <param name="RenameFileNameFn">重写文件名函数(传入参数只包含文件名+扩展名，不包含目录)</param>
        /// <returns>数量</returns>
        public static int CopyDir(DirectoryInfo SrcDirInfo, DirectoryInfo TgtDirInfo, bool IsReplace,
            Func<DirectoryInfo, bool> ExcludeDirFn, Func<FileInfo, bool> ExcludeFileFn, Func<string, string> RenameFileNameFn = null)
        {
            if (!SrcDirInfo.Exists) { return 0; }
            string relpath = "";
            if (RenameFileNameFn == null)
            {
                return CopyDir(SrcDirInfo, TgtDirInfo, IsReplace, ref relpath, ExcludeDirFn, ExcludeFileFn, s => s);
            }
            else
            {
                return CopyDir(SrcDirInfo, TgtDirInfo, IsReplace, ref relpath, ExcludeDirFn, ExcludeFileFn, RenameFileNameFn);
            }
        }
        /// <summary>
        /// 复制文件目录及子目录的文件
        /// </summary>
        /// <param name="SrcDirInfo">源目录</param>
        /// <param name="TgtDirInfo">目标目录</param>
        /// <param name="IsReplace">是否替换</param>
        /// <param name="RelPath">相对路径</param>
        /// <param name="ExcludeDirFn">排除目录函数</param>
        /// <param name="ExcludeFileFn">排除文件函数</param>
        /// <param name="RenameFileNameFn">重写文件名函数(传入参数只包含文件名+扩展名，不包含目录)</param>
        private static int CopyDir(DirectoryInfo SrcDirInfo, DirectoryInfo TgtDirInfo, bool IsReplace, ref string RelPath,
            Func<DirectoryInfo, bool> ExcludeDirFn, Func<FileInfo, bool> ExcludeFileFn, Func<string, string> RenameFileNameFn)
        {
            int intCount = 0;
            DirectoryInfo diS = SrcDirInfo;
            string dirt = TgtDirInfo.FullName + "\\";
            //递归执行
            foreach (DirectoryInfo dir in diS.GetDirectories())
            {
                if (ExcludeDirFn(dir)) { continue; }
                string strT = RelPath + dir + "\\", strT2 = dirt + dir + "\\";
                DirectoryInfo dit = new DirectoryInfo(strT2);
                if (!dit.Exists) { dit = Directory.CreateDirectory(strT2); }
                intCount += CopyDir(dir, dit, IsReplace, ref strT, ExcludeDirFn, ExcludeFileFn, RenameFileNameFn);
            }
            //替换文件
            foreach (FileInfo file in diS.GetFiles())
            {
                if (ExcludeFileFn(file)) { continue; }
                FileInfo tfile = new FileInfo(dirt + RenameFileNameFn(file.Name));
                if (tfile.Exists)
                {
                    if (!IsReplace) { continue; }
                    tfile.Attributes = FileAttributes.Normal;
                    tfile.Delete();
                }
                file.CopyTo(tfile.FullName).Attributes = FileAttributes.Normal;
                intCount++;
            }
            //不存文件则删除文件夹
            if (TgtDirInfo.GetFiles().Length == 0 && TgtDirInfo.GetDirectories().Length == 0) { TgtDirInfo.Delete(); }
            return intCount;
        }
        /// <summary>
        /// 复制文件到指定目录
        /// </summary>
        /// <param name="SrcDirInfo">源目录</param>
        /// <param name="TgtDirInfo">目标目录</param>
        /// <param name="IsReplace">是否目标文件替换</param>
        /// <param name="ExcludeDirFn">排除目录函数</param>
        /// <param name="ExcludeFileFn">排除文件函数</param>
        /// <param name="RenameFileNameFn">重写文件名函数(传入参数只包含文件名+扩展名，不包含目录)</param>
        /// <returns>总复制数</returns>
        public static int CopyDir(DirectoryInfo SrcDirInfo, DirectoryInfo TgtDirInfo, bool IsReplace,
            Func<DirectoryInfo, bool> ExcludeDirFn, Func<FileInfo, FileInfo, bool> ExcludeFileFn, Func<string, string> RenameFileNameFn = null)
        {
            if (!SrcDirInfo.Exists) { return 0; }
            string relpath = "";
            if (RenameFileNameFn == null)
            {
                return CopyDir(SrcDirInfo, TgtDirInfo, IsReplace, ref relpath, ExcludeDirFn, ExcludeFileFn, s => s);
            }
            else
            {
                return CopyDir(SrcDirInfo, TgtDirInfo, IsReplace, ref relpath, ExcludeDirFn, ExcludeFileFn, RenameFileNameFn);
            }
        }
        /// <summary>
        /// 复制文件目录及子目录的文件
        /// </summary>
        /// <param name="SrcDirInfo">源目录</param>
        /// <param name="TgtDirInfo">目标目录</param>
        /// <param name="IsReplace">是否替换</param>
        /// <param name="RelPath">相对路径</param>
        /// <param name="ExcludeDirFn">排除目录函数</param>
        /// <param name="ExcludeFileFn">排除文件函数</param>
        /// <param name="RenameFileNameFn">重写文件名函数(传入参数只包含文件名+扩展名，不包含目录)</param>
        /// <returns>总复制数</returns>
        private static int CopyDir(DirectoryInfo SrcDirInfo, DirectoryInfo TgtDirInfo, bool IsReplace, ref string RelPath,
            Func<DirectoryInfo, bool> ExcludeDirFn, Func<FileInfo, FileInfo, bool> ExcludeFileFn, Func<string, string> RenameFileNameFn)
        {
            int intCount = 0;
            DirectoryInfo diS = SrcDirInfo;
            //递归执行
            foreach (DirectoryInfo dir in diS.GetDirectories())
            {
                string dirt = TgtDirInfo.FullName + dir + "\\";
                if (ExcludeDirFn(dir)) { continue; }
                var dit = new DirectoryInfo(dirt);
                if (!dit.Exists) { dit = Directory.CreateDirectory(dirt); }
                string reldir = RelPath + dir + "\\";
                intCount += CopyDir(dir, dit, IsReplace, ref reldir, ExcludeDirFn, ExcludeFileFn, RenameFileNameFn);
            }
            //替换文件
            foreach (FileInfo file in diS.GetFiles())
            {
                FileInfo fit = new FileInfo(TgtDirInfo.FullName + RenameFileNameFn(file.Name));
                if (ExcludeFileFn(file, fit)) { continue; }
                if (fit.Exists)
                {
                    if (!IsReplace) { continue; }
                    fit.Attributes = FileAttributes.Normal;
                    fit.Delete();
                }
                file.CopyTo(fit.FullName).Attributes = FileAttributes.Normal;
                intCount++;
            }
            //不存文件则删除文件夹
            if (TgtDirInfo.GetFiles().Length == 0 && TgtDirInfo.GetDirectories().Length == 0) { TgtDirInfo.Delete(); }
            return intCount;
        }
        #endregion

        #region 清理文件夹(包含子文件夹及文件，保留当前文件夹)
        /// <summary>
        /// 清理文件夹(包含子文件夹及文件，保留当前文件夹)
        /// </summary>
        /// <param name="Dir">目录</param>
        /// <param name="ExcludeFiles">排除文件列表（文件名为小写且不包启路径）</param>
        /// <returns>文件数量</returns>
        public static int ClearDir(string Dir, HashSet<string> ExcludeFiles = null)
        {
            return ClearDir(new DirectoryInfo(Dir), ExcludeFiles);
        }
        /// <summary>
        /// 清理文件夹(包含子文件夹及文件，保留当前文件夹)
        /// </summary>
        /// <param name="DirInfo">目录信息</param>
        /// <param name="ExcludeFiles">排除文件列表（文件名为小写且不包启路径）</param>
        /// <returns>文件数量</returns>
        public static int ClearDir(DirectoryInfo DirInfo, HashSet<string> ExcludeFiles = null)
        {
            if (!DirInfo.Exists) { return 0; }
            int DirCount = 0, FileCount = 0;
            if (ExcludeFiles == null)
            {
                ClearDir(DirInfo, ref DirCount, ref FileCount);
            }
            else
            {
                ClearDir(DirInfo, ExcludeFiles, ref DirCount, ref FileCount);
            }
            return FileCount;
        }
        /// <summary>
        /// 清理目录
        /// </summary>
        /// <param name="DirInfo">目录信息</param>
        /// <param name="DirCount">清理目录数（用于返回）</param>
        /// <param name="FileCount">清理文件数（用于返回）</param>
        static void ClearDir(DirectoryInfo DirInfo, ref int DirCount, ref int FileCount)
        {
            foreach (FileInfo fi in DirInfo.GetFiles())
            {
                fi.Attributes = FileAttributes.Normal;
                fi.Delete(); FileCount++;
            }
            foreach (DirectoryInfo di in DirInfo.GetDirectories())
            {
                ClearDir(di, ref DirCount, ref FileCount);
                di.Attributes = FileAttributes.Normal;
                di.Delete(); DirCount++;
            }
        }
        /// <summary>
        /// 清理目录
        /// </summary>
        /// <param name="DirInfo">目录信息</param>
        /// <param name="ExcludeFiles">排除文件列表（文件名为小写且不包启路径）</param>
        /// <param name="DirCount">清理目录数（用于返回）</param>
        /// <param name="FileCount">清理文件数（用于返回）</param>
        static void ClearDir(DirectoryInfo DirInfo, HashSet<string> ExcludeFiles, ref int DirCount, ref int FileCount)
        {
            foreach (FileInfo fi in DirInfo.GetFiles())
            {
                if (ExcludeFiles.Contains(fi.Name.ToLower())) { continue; }
                fi.Attributes = FileAttributes.Normal;
                fi.Delete(); FileCount++;
            }
            foreach (DirectoryInfo di in DirInfo.GetDirectories())
            {
                ClearDir(di, ExcludeFiles, ref DirCount, ref FileCount);
                di.Attributes = FileAttributes.Normal;
                di.Delete(); DirCount++;
            }
        }

        #endregion

        #region 删除文件夹(包含子文件夹及文件)
        /// <summary>
        /// 删除文件夹(包含子文件夹及文件)
        /// </summary>
        /// <param name="Dir">目录</param>
        /// <param name="ExcludeFiles">排除文件列表（文件名为小写且不包启路径）</param>
        /// <returns>文件数量</returns>
        public static int DelDir(string Dir, HashSet<string> ExcludeFiles = null)
        {
            return DelDir(new DirectoryInfo(Dir), ExcludeFiles);
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="DirInfo">目录信息</param>
        /// <param name="ExcludeFiles">排除文件列表（文件名为小写且不包启路径）</param>
        /// <returns>文件数量</returns>
        public static int DelDir(DirectoryInfo DirInfo, HashSet<string> ExcludeFiles = null)
        {
            var count = ClearDir(DirInfo, ExcludeFiles);
            if (DirInfo.Exists) { DirInfo.Delete(); }
            return count;
        }
        /// <summary>
        /// 删除空文件夹
        /// </summary>
        /// <param name="DirInfo">目录信息</param>
        /// <returns>删除目录数</returns>
        public static int DelEmptyDir(DirectoryInfo DirInfo)
        {
            if (!DirInfo.Exists) { return 0; }
            int DirCount = 0;
            DelEmptyDir(DirInfo, ref DirCount);
            return DirCount;
        }
        /// <summary>
        /// 删除空文件夹
        /// </summary>
        /// <param name="DirInfo">目录信息</param>
        /// <param name="DirCount">删除目录数（用于返回）</param>
        static void DelEmptyDir(DirectoryInfo DirInfo, ref int DirCount)
        {
            foreach (DirectoryInfo di in DirInfo.GetDirectories())
            {
                DelEmptyDir(di, ref DirCount);
            }
            if ((DirInfo.GetDirectories().Count() == 0) && (DirInfo.GetFiles().Count() == 0))
            {
                DirInfo.Attributes = FileAttributes.Normal;
                DirInfo.Delete(); DirCount++;
            }
        }
        #endregion

        #region 文件Md5校验码
        /// <summary>
        /// 获取文件Md5校验码
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public static string GetFileMd5Value(string File)
        {
            if (!System.IO.File.Exists(File)) { return string.Empty; }
            Stream inputStream = System.IO.File.Open(File, FileMode.Open, FileAccess.Read, FileShare.Read);
            string md5 = GetFileMd5Value(inputStream, true);
            return md5;
        }
        /// <summary>
        /// 获取文件Md5校验码
        /// </summary>
        /// <param name="FileStream"></param>
        /// <param name="IsCloseStream"></param>
        /// <returns></returns>
        public static string GetFileMd5Value(Stream FileStream, bool IsCloseStream = true)
        {
            int bufferSize = 1024 * 16;//自定义缓冲区大小16K 
            byte[] buffer = new byte[bufferSize];
            HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
            int readLength = 0;//每次读取长度 
            var output = new byte[bufferSize];
            while ((readLength = FileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                //计算MD5 
                hashAlgorithm.TransformBlock(buffer, 0, readLength, output, 0);
            }
            //完成最后计算，必须调用(由于上一部循环已经完成所有运算，所以调用此方法时后面的两个参数都为0) 
            hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
            string md5 = BitConverter.ToString(hashAlgorithm.Hash);
            hashAlgorithm.Clear();
            if (IsCloseStream) { FileStream.Close(); }
            md5 = md5.Replace("-", "");
            return md5;
        }


        /// <summary>
        /// 获取文件MD5值
        /// add by sunyichao 2019-01-08
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
        #endregion
        #region 压缩文件


        /// <summary>    
        /// 获取所有文件    
        /// </summary>    
        /// <returns></returns>    
        private static Dictionary<string, DateTime> GetAllFies(string dir)
        {
            Dictionary<string, DateTime> FilesList = new Dictionary<string, DateTime>();
            DirectoryInfo fileDire = new DirectoryInfo(dir);
            if (!fileDire.Exists)
            {
                throw new System.IO.FileNotFoundException("目录:" + fileDire.FullName + "没有找到!");
            }
            GetAllDirFiles(fileDire, FilesList);
            GetAllDirsFiles(fileDire.GetDirectories(), FilesList);
            return FilesList;
        }
        /// <summary>    
        /// 获取一个文件夹下的所有文件夹里的文件    
        /// </summary>    
        /// <param name="dirs"></param>    
        /// <param name="filesList"></param>    
        private static void GetAllDirsFiles(DirectoryInfo[] dirs, Dictionary<string, DateTime> filesList)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                foreach (FileInfo file in dir.GetFiles("*.*"))
                {
                    filesList.Add(file.FullName, file.LastWriteTime);
                }
                GetAllDirsFiles(dir.GetDirectories(), filesList);
            }
        }
        /// <summary>    
        /// 获取一个文件夹下的文件    
        /// </summary>    
        /// <param name="dir">目录名称</param>    
        /// <param name="filesList">文件列表HastTable</param>    
        private static void GetAllDirFiles(DirectoryInfo dir, Dictionary<string, DateTime> filesList)
        {
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                filesList.Add(file.FullName, file.LastWriteTime);
            }
        }
        /// <summary>
        /// 大文件分割
        /// </summary>
        /// <param name="filepath">待分割文件路径:如：D:\x_20160407.txt</param>
        /// <param name="splitMinFileSize">当文件大于这个配置项时就执行文件分隔:单位M</param>
        /// <param name="splitFileSize">每个分隔出来的文件大小:单位M</param>
        /// <param name="splitFileFormat">新分割出的文件格式：如D:\x_20160407{0}.txt</param>
        /// <returns></returns>
        public static int SplitFile(string filepath, int splitMinFileSize, int splitFileSize, string splitFileFormat)
        {
            string file = filepath;

            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.Length > splitMinFileSize)
            {
                //  Console.WriteLine("判定结果：需要分隔文件！");
            }
            else
            {
                //Console.WriteLine("判定结果：不需要分隔文件！");
                //Console.ReadKey();
                return 0;
            }

            int steps = Math.Ceiling(fileInfo.Length / (splitFileSize * 1.0)).ToInt2();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    int couter = 1;
                    bool isReadingComplete = false;
                    while (!isReadingComplete)
                    {
                        string filePath = string.Format(splitFileFormat, couter);
                        //   Console.WriteLine("开始读取文件【{1}】：{0}", filePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                        byte[] input = br.ReadBytes(splitFileSize);
                        //能否读取到指定长度的字节
                        if (input.Length >= splitFileSize)
                        {
                            using (FileStream writeFs = new FileStream(filePath, FileMode.Create))
                            {
                                using (BinaryWriter bw = new BinaryWriter(writeFs))
                                {
                                    bw.Write(input);
                                }
                            }
                        }
                        else
                        {
                            isReadingComplete = (input.Length < splitFileSize);
                        }

                        if (!isReadingComplete)
                        {
                            couter += 1;
                        }
                        //   Console.WriteLine("完成读取文件【{1}】：{0}", filePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }
            }
            if (fileInfo.Length >= splitFileSize)
                fileInfo.Delete();

            return 0;


        }
        #endregion

        #region 递归文件方法
        /// <summary>
        /// 获取指定目录中的匹配项（文件或目录）
        /// </summary>
        /// <param name="dir">要搜索的目录</param>
        /// <param name="regexPattern">项名模式（正则）。null表示忽略模式匹配，返回所有项</param>
        /// <param name="recurse">是否搜索子目录</param>
        /// <param name="throwEx">是否抛异常</param>
        /// <returns></returns>
        public static List<string> GetFileSystemEntries(string dir, string regexPattern = null, bool recurse = false, bool throwEx = false)
        {
            List<string> lst = new List<string>();

            try
            {
                foreach (string item in Directory.GetFileSystemEntries(dir))
                {
                    try
                    {
                        if (regexPattern == null || Regex.IsMatch(Path.GetFileName(item), regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                        { lst.Add(item); }

                        //递归
                        if (recurse && (File.GetAttributes(item) & FileAttributes.Directory) == FileAttributes.Directory)
                        { lst.AddRange(GetFileSystemEntries(item, regexPattern, true)); }
                    }
                    catch { if (throwEx) { throw; } }
                }
            }
            catch { if (throwEx) { throw; } }

            return lst;
        }

        /// <summary>
        /// 获取指定目录中的匹配文件
        /// </summary>
        /// <param name="dir">要搜索的目录</param>
        /// <param name="regexPattern">文件名模式（正则）。null表示忽略模式匹配，返回所有文件</param>
        /// <param name="recurse">是否搜索子目录</param>
        /// <param name="throwEx">是否抛异常</param>
        /// <returns></returns>
        public static List<string> GetFiles(string dir, string regexPattern = null, bool recurse = false, bool throwEx = false)
        {
            List<string> lst = new List<string>();
            var ingorePath = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\ignore.txt";
            var expt = new List<string>();
            if (File.Exists(ingorePath)) expt = File.ReadAllLines(ingorePath).ToList();


            if (expt == null) expt = expt = new List<string>();
            try
            {
                foreach (string item in Directory.GetFileSystemEntries(dir))
                {
                    try
                    {
                        var cc = expt.Where(x => item.Contains(x)).Count();
                        if (cc > 0) continue;
                        bool isFile = (File.GetAttributes(item) & FileAttributes.Directory) != FileAttributes.Directory;

                        if (isFile && (regexPattern == null || Regex.IsMatch(Path.GetFileName(item), regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline)))
                        {
                            lst.Add(item);
                        }

                        //递归
                        if (recurse && !isFile)
                        {
                            lst.AddRange(GetFiles(item, regexPattern, true));
                        }
                    }
                    catch { if (throwEx) { throw; } }
                }
            }
            catch { if (throwEx) { throw; } }

            return lst;
        }

        /// <summary>
        /// 获取指定目录中的匹配目录
        /// </summary>
        /// <param name="dir">要搜索的目录</param>
        /// <param name="regexPattern">目录名模式（正则）。null表示忽略模式匹配，返回所有目录</param>
        /// <param name="recurse">是否搜索子目录</param>
        /// <param name="throwEx">是否抛异常</param>
        /// <returns></returns>
        public static List<string> GetDirectories(string dir, string regexPattern = null, bool recurse = false, bool throwEx = false)
        {
            List<string> lst = new List<string>();

            try
            {
                foreach (string item in Directory.GetDirectories(dir))
                {
                    try
                    {
                        if (regexPattern == null || Regex.IsMatch(Path.GetFileName(item), regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                        { lst.Add(item); }

                        //递归
                        if (recurse) { lst.AddRange(GetDirectories(item, regexPattern, true)); }
                    }
                    catch { if (throwEx) { throw; } }
                }
            }
            catch { if (throwEx) { throw; } }

            return lst;
        }

        #endregion


        #region 解压缩文件

        #endregion
        /// <summary>
        /// 将byte数组转换为文件并保存到指定地址
        /// </summary>
        /// <param name="buff">byte数组</param>
        /// <param name="savepath">保存地址</param>
        public static void Bytes2File(byte[] buff, string savepath)
        {
            if (File.Exists(savepath))
            {
                File.Delete(savepath);
            }
            if (!Directory.Exists(Path.GetDirectoryName(savepath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savepath));
            }

            FileStream fs = new FileStream(savepath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buff, 0, buff.Length);
            bw.Close();
            fs.Close();
        }

        #region 下载文件
        /// <summary>
        /// Http方式下载文件
        /// </summary>
        /// <param name="url">http地址</param>
        /// <param name="localfile">本地文件</param>
        /// <returns></returns>
        public static bool Download(string url, string localfile)
        {
            LogHelper.Info(url + "||" + localfile);
            // string localfile= ConfigUtils.FileUploadPath+filename;
            bool flag = false;
            long startPosition = 0; // 上次下载的文件起始位置
            FileStream writeStream; // 写入本地文件流对象

            long remoteFileLength = GetHttpLength(url);// 取得远程文件长度
            System.Console.WriteLine("remoteFileLength=" + remoteFileLength);
            if (remoteFileLength == 745)
            {
                System.Console.WriteLine("远程文件不存在.");
                return false;
            }

            // 判断要下载的文件夹是否存在
            if (File.Exists(localfile))
            {

                writeStream = File.OpenWrite(localfile);             // 存在则打开要下载的文件
                startPosition = writeStream.Length;                  // 获取已经下载的长度

                writeStream.Seek(0, SeekOrigin.Current); // 本地文件写入位置定位
            }
            else
            {

                //if (!Directory.Exists(localfile)) Directory.CreateDirectory(localfile);
                writeStream = new FileStream(localfile, FileMode.Create);// 文件不保存创建一个文件
                startPosition = 0;
            }


            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);// 打开网络连接

                if (startPosition > 0)
                {
                    myRequest.AddRange((int)startPosition);// 设置Range值,与上面的writeStream.Seek用意相同,是为了定义远程文件读取位置
                }


                Stream readStream = myRequest.GetResponse().GetResponseStream();// 向服务器请求,获得服务器的回应数据流


                byte[] btArray = new byte[512];// 定义一个字节数据,用来向readStream读取内容和向writeStream写入内容
                int contentSize = readStream.Read(btArray, 0, btArray.Length);// 向远程文件读第一次

                long currPostion = startPosition;

                while (contentSize > 0)// 如果读取长度大于零则继续读
                {
                    currPostion += contentSize;
                    int percent = (int)(currPostion * 100 / remoteFileLength);
                    System.Console.WriteLine("percent=" + percent + "%");

                    writeStream.Write(btArray, 0, contentSize);// 写入本地文件
                    contentSize = readStream.Read(btArray, 0, btArray.Length);// 继续向远程文件读取
                }

                //关闭流
                writeStream.Close();
                readStream.Close();

                flag = true;        //返回true下载成功
            }
            catch (Exception)
            {
                writeStream.Close();
                flag = false;       //返回false下载失败
            }

            return flag;
        }

        // 从文件头得到远程文件的长度
        private static long GetHttpLength(string url)
        {
            long length = 0;

            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);// 打开网络连接
                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

                if (rsp.StatusCode == HttpStatusCode.OK)
                {
                    length = rsp.ContentLength;// 从文件头得到远程文件的长度
                }

                rsp.Close();
                return length;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
                return length;
            }

        }
        #endregion



        #region 解压缩文件zip，rar
        /// <summary>
        /// 解压RAR和ZIP文件(需存在Winrar.exe(只要自己电脑上可以解压或压缩文件就存在Winrar.exe))
        /// </summary>
        /// <param name="UnPath">解压后文件保存目录</param>
        /// <param name="rarPathName">待解压文件存放绝对路径（包括文件名称）</param>
        /// <param name="IsCover">所解压的文件是否会覆盖已存在的文件(如果不覆盖,所解压出的文件和已存在的相同名称文件不会共同存在,只保留原已存在文件)</param>
        /// <param name="PassWord">解压密码(如果不需要密码则为空)</param>
        /// <returns>true(解压成功);false(解压失败)</returns>
        public static bool UnRarOrZip(string UnPath, string rarPathName, bool IsCover, string PassWord)
        {
            try
            {
                //string[] FileProperties = new string[2];
                //FileProperties[0] = rarPathName;//待解压的文件
                //FileProperties[1] = UnPath;//解压后放置的目标目录

                // UnZip(FileProperties);
                //return true;
                ////  System.IO.Compression.ZipFile.ExtractToDirectory(@rarPathName, UnPath); //解压
                //// return true;



                if (!Directory.Exists(UnPath))
                    Directory.CreateDirectory(UnPath);
                var rarPath = @"C:\Program Files\WinRAR\Winrar.exe";
                if (!File.Exists(rarPath)) rarPath = @"D:\Program Files\WinRAR\Winrar.exe";

                Process Process1 = new Process();
                Process1.StartInfo.FileName = rarPath;
                Process1.StartInfo.CreateNoWindow = true;
                string cmd = "";
                if (!string.IsNullOrEmpty(PassWord) && IsCover)
                    //解压加密文件且覆盖已存在文件( -p密码 )
                    cmd = string.Format(" x -p{0} -o+ {1} {2} -y", PassWord, rarPathName, UnPath);
                else if (!string.IsNullOrEmpty(PassWord) && !IsCover)
                    //解压加密文件且不覆盖已存在文件( -p密码 )
                    cmd = string.Format(" x -p{0} -o- {1} {2} -y", PassWord, rarPathName, UnPath);
                else if (IsCover)
                    //覆盖命令( x -o+ 代表覆盖已存在的文件)
                    cmd = string.Format(" x -o+ {0} {1} -y", rarPathName, UnPath);
                else
                    //不覆盖命令( x -o- 代表不覆盖已存在的文件)
                    cmd = string.Format(" x -o- {0} {1} -y", rarPathName, UnPath);
                LogHelper.Info(cmd);
                //命令
                Process1.StartInfo.Arguments = cmd;
                Process1.Start();
                Process1.WaitForExit();//无限期等待进程 winrar.exe 退出
                                       //Process1.ExitCode==0指正常执行，Process1.ExitCode==1则指不正常执行
                if (Process1.ExitCode == 0)
                {
                    Process1.Close();
                    return true;
                }
                else
                {
                    Process1.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 压缩文件成RAR或ZIP文件(需存在Winrar.exe(只要自己电脑上可以解压或压缩文件就存在Winrar.exe))
        /// </summary>
        /// <param name="filesPath">将要压缩的文件夹或文件的绝对路径</param>
        /// <param name="rarPathName">压缩后的压缩文件保存绝对路径（包括文件名称）</param>
        /// <param name="IsCover">所压缩文件是否会覆盖已有的压缩文件(如果不覆盖,所压缩文件和已存在的相同名称的压缩文件不会共同存在,只保留原已存在压缩文件)</param>
        /// <param name="PassWord">压缩密码(如果不需要密码则为空)</param>
        /// <returns>true(压缩成功);false(压缩失败)</returns>
        public static bool CondenseRarOrZip(string filesPath, string rarPathName, bool IsCover, string PassWord, List<string> excludedir, List<string> excludepath)
        {
            try
            {
                var tempsaverardir = "";
                if (File.Exists(filesPath))
                {
                    tempsaverardir = filesPath;
                }
                else
                {
                    //因为有要排除的路径或文件 所以用到临时文件
                    var root = @"D:\filetranstemprar\";
                    tempsaverardir = (root + "temprar\\");
                    FileMove(filesPath, tempsaverardir, excludedir, excludepath, "");
                }
                string rarPath = Path.GetDirectoryName(rarPathName);
                if (!Directory.Exists(rarPath))
                    Directory.CreateDirectory(rarPath);
                if (File.Exists(rarPathName))
                    File.Delete(rarPathName);
                Process Process1 = new Process();
                Process1.StartInfo.FileName = "Winrar.exe";
                Process1.StartInfo.CreateNoWindow = true;
                string cmd = "";
                if (!string.IsNullOrEmpty(PassWord) && IsCover)
                    //压缩加密文件且覆盖已存在压缩文件( -p密码 -o+覆盖 )
                    cmd = string.Format(" a -p{0} -o+ {1} {2} -y", PassWord, rarPathName, tempsaverardir);
                else if (!string.IsNullOrEmpty(PassWord) && !IsCover)
                    //压缩加密文件且不覆盖已存在压缩文件( -p密码 -o-不覆盖 )
                    cmd = string.Format(" a -p{0} -o- {1} {2} -y", PassWord, rarPathName, tempsaverardir);
                else if (string.IsNullOrEmpty(PassWord) && IsCover)
                    //压缩且覆盖已存在压缩文件( -o+覆盖 )
                    cmd = string.Format(" a -ep1 -o+ {0} {1} -r", rarPathName, tempsaverardir);
                else
                    //压缩且不覆盖已存在压缩文件( -o-不覆盖 )
                    cmd = string.Format(" a -o- {0} {1} -y", rarPathName, tempsaverardir);
                LogHelper.Info(cmd);
                //命令
                Process1.StartInfo.Arguments = cmd;
                Process1.Start();
                Process1.WaitForExit();//无限期等待进程 winrar.exe 退出
                                       //Process1.ExitCode==0指正常执行，Process1.ExitCode==1则指不正常执行
                if (Process1.ExitCode == 0)
                {
                    Process1.Close();
                    return true;
                }
                else
                {
                    Process1.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToStr());
                return false;
            }
        }


        #endregion

    }
}
