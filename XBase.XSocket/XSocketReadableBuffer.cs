using System;
using System.Collections.Generic;

using System.Text;


namespace XBase.XSocket
{
    
    public class XSocketReadableBuffer
    {
        public int mPreBufferSize = 0;
        public int mBufferSize = 0;

        public byte[] mPreBuffer = new byte[8192];
        public byte[] mBuffer = new byte[16384];

        public XSocketReadableBuffer()
        {
        }

        public void Store(byte[] vBuffer, int vBufferSize)
        {
            if (mPreBuffer == null)
            {
                if (mBuffer.Length < vBufferSize)
                {
                    mBuffer = new byte[vBufferSize];
                }

                Buffer.BlockCopy(vBuffer, 0, mBuffer, 0, vBufferSize);
                mBufferSize = vBuffer.Length;
            }
            else
            {
                if (mBuffer.Length < mPreBufferSize + vBufferSize)
                {
                    mBuffer = new byte[mPreBufferSize + vBufferSize];
                }

                Buffer.BlockCopy(mPreBuffer, 0, mBuffer, 0, mPreBufferSize);
                Buffer.BlockCopy(vBuffer, 0, mBuffer, mPreBufferSize, vBufferSize);
                mBufferSize = mPreBufferSize + vBufferSize;
            }
        }

        public bool IsReadable(int vOffset, int vBytes)
        {
            if (vOffset + vBytes < mBufferSize)
                return true;

            return false;
        }

        public byte[] GetBuffer()
        {
            return mBuffer;
        }

        public int ReadLength(int vOffset)
        {
            byte[] packetSize = new byte[4];
            packetSize[0] = mBuffer[vOffset];
            packetSize[1] = mBuffer[vOffset + 1];
            packetSize[2] = mBuffer[vOffset + 2];
            packetSize[3] = mBuffer[vOffset + 3];

            return BitConverter.ToInt32(packetSize, 0);
        }

        public void Rearrange(int vReadBytes)
        {
            if (mPreBuffer.Length < mBufferSize - vReadBytes)
            {
                mPreBuffer = new byte[mBufferSize - vReadBytes];
            }

            Buffer.BlockCopy(mBuffer, vReadBytes, mPreBuffer, 0, mBufferSize - vReadBytes);
            mPreBufferSize = mBufferSize - vReadBytes;
        }
    }
}
