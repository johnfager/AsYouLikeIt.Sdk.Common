
namespace AsYouLikeIt.Sdk.Common.Delegates
{

    /// <summary>
    /// Encrypt strings in a way that they are protected for your use and decrypte using <see cref="StringDecryptorDelegate"/>.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public delegate string StringEncryptorDelegate(string input);

    /// <summary>
    /// Decrypts strings using a delegate related to <see cref="StringEncryptorDelegate"/>.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public delegate string StringDecryptorDelegate(string input);

}
