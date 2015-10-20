using System;
namespace fxpa
{
    public interface IFileWorker
    {
        bool LoadCdlFile(string fpath, int startingRow, int rowsLimit, out System.Collections.Generic.List<BarData> datas, bool isDecrypt, bool isLocal);
        bool LoadSigFile(string fpath, int startingRow, int rowsLimit, out System.Collections.Generic.List<Signal> datas, bool isDecrypt, bool isLocal);
        bool LoadFile(string fpath, int startingRow, int rowsLimit, out System.Collections.Generic.List<RealTimeData> datas, bool isDecrypt);
        bool SaveCdlFile(string fpath, BarData[] barDatas, bool isEncrypt);
        bool SaveSigFile(string fpath, Signal[] signals, bool isEncrypt);
        bool ZipFile(string file, string outPath, string password);
        bool ZipFiles(string file, string outPath, string password);
        bool UnZipFile(string zipPathAndFile, string outputFolder, string password, bool deleteZipFile);
        bool UnZipFiles(string zipPathAndFile, string outputFolder, string password, bool deleteZipFile);

    }
}
