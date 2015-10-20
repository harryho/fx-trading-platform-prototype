using System;
namespace fxpa
{
  public  interface ICrypta
    {
        string BfKey { get; set; }
        string decrypt(string source);
        string encrypt(string source);
        string localDecrypt(string source);
        string localEncrypt(string source);
        string specialEncrypt(string source);
    }
}
