using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Helper.IO
{
    public partial class FileHelper
    {
        private class AsyncState
        {
            public FileStream FS { get; set; }
            public byte[] Buffer { get; set; }
            public ManualResetEvent WaitHandle { get; set; }
            public List<byte> Rs;
            public int Offset { get; set; }
            public int WriteCount { get; set; }

        }

        static int bufferSize = 256;

        public static string AsyncRead(string filePath)
        {
            // string filePath = "c:\\test.txt";
            //以只读方式打开文件流
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous))
            {
                var buffer = new byte[bufferSize];

                //构造BeginRead需要传递的状态
                AsyncState asyncState = new AsyncState() { FS = fileStream, Buffer = buffer, WaitHandle = new ManualResetEvent(false) };
                asyncState.Rs = new List<byte>();

                //异步读取
                IAsyncResult asyncResult = fileStream.BeginRead(buffer, 0, bufferSize, new AsyncCallback(AsyncReadCallback), asyncState);
                asyncState.WaitHandle.WaitOne();
                //Console.WriteLine("read complete");
                return Encoding.UTF8.GetString(asyncState.Rs.ToArray());
            }
        }

        //异步读取回调处理方法
        static void AsyncReadCallback(IAsyncResult asyncResult)
        {
            var asyncState = (AsyncState)asyncResult.AsyncState;
            int readCn = asyncState.FS.EndRead(asyncResult);
            //判断是否读到内容
            if (readCn > 0)
            {
                byte[] buffer;
                if (readCn == bufferSize) buffer = asyncState.Buffer;
                else
                {
                    buffer = new byte[readCn];
                    Array.Copy(asyncState.Buffer, 0, buffer, 0, readCn);
                }
                asyncState.Rs.AddRange(buffer);
            }

            if (readCn < bufferSize)
            {
                asyncState.WaitHandle.Set();
            }
            else
            {
                Array.Clear(asyncState.Buffer, 0, bufferSize);
                //再次执行异步读取操作
                asyncState.FS.BeginRead(asyncState.Buffer, 0, bufferSize, new AsyncCallback(AsyncReadCallback), asyncState);
            }
        }

        public static void AsyncWrite(string content, string path)
        {
            byte[] toWriteBytes = System.Text.Encoding.GetEncoding("utf-8").GetBytes(content);
            string filePath = path;
            AsyncState state = new AsyncState
            {
                Offset = 0,
                Buffer = toWriteBytes,
                WaitHandle = new ManualResetEvent(false),
            };
            state.WriteCount = 26;// toWriteBytes.Length;

            //FileStream实例
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, bufferSize, FileOptions.Asynchronous))
            {
                state.FS = fileStream;

                fileStream.BeginWrite(toWriteBytes, 0, state.WriteCount, WriteCallback, state);
                //等待写完毕或者出错发出的继续信号
                state.WaitHandle.WaitOne();
            }
            //Console.WriteLine("Done");
            //Console.Read();
        }

        static void WriteCallback(IAsyncResult asyncResult)
        {
            AsyncState asyncState = (AsyncState)asyncResult.AsyncState;
            try
            {
                asyncState.FS.EndWrite(asyncResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EndWrite Error:" + ex.Message);
                asyncState.WaitHandle.Set();
                return;
            }

            Console.WriteLine("write to " + asyncState.FS.Position);
            //判断是否写完，未写完继续异步写
            if (asyncState.Offset + asyncState.WriteCount < asyncState.Buffer.Length)
            {
                asyncState.Offset += asyncState.WriteCount;
                int writeSize = asyncState.WriteCount;
                if (asyncState.FS.Position + asyncState.WriteCount > asyncState.Buffer.Length)
                {
                    writeSize = (int)((long)asyncState.Buffer.Length - asyncState.FS.Position);
                }
                asyncState.FS.BeginWrite(asyncState.Buffer, asyncState.Offset, writeSize, WriteCallback, asyncState);
            }
            else
            {                //写完发出完成信号
                asyncState.WaitHandle.Set();
            }

        }







    }
}
