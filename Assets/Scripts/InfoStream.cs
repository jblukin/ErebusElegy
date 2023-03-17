using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoStream
{

    private List<byte> _stream;

    public InfoStream() {

        _stream = new List<byte>();

    }

    public InfoStream(byte[] source) {

        _stream = new List<byte>();
        _stream.AddRange(source);

    }

    public void addString(string str) { 
    
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);

        addByteArray(buffer);
    
    }

    public string getString() {

        byte[] buffer = getByteArray();

        string str = System.Text.Encoding.UTF8.GetString(buffer);

        return str;

    }

    public void addByteArray(byte[] buffer) {

        int length = buffer.Length;

        addInteger(length);

        for (int i = 0; i < length; i++) {

            addByte(buffer[i]);

        }

    }

    public byte[] getByteArray() {

        int length = getInteger();

        byte[] buffer = new byte[length];

        for (int i = 0; i < length; i++) {

            buffer[i] = getByte();

        }

        return buffer;

    }

    public void addInteger(int n) {

        byte[] buffer = System.BitConverter.GetBytes(n);

        for (int i = 0; i < 4; i++) {

            addByte(buffer[i]);

        }

    }


    public int getInteger() {

        byte[] buffer = new byte[4];

        for (int i = 0; i < 4; i++) {

            buffer[i] = getByte();

        }

        return System.BitConverter.ToInt32(buffer);

    }

    public void addByte(byte b) {

        _stream.Add(b);

    }

    public byte getByte() {

        byte b = _stream[0];

        _stream.RemoveAt(0);

        return b;

    }

    public byte[] getStream() {

        return _stream.ToArray();

    }

}
