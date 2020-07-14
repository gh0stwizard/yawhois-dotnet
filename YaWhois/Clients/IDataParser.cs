using System;

namespace YaWhois.Clients
{
    public interface IDataParser
    {
        /// <summary>
        /// Retrieves first referral from the server response.
        /// </summary>
        /// <param name="text">The server response as is.</param>
        /// <returns>The referral string or <see cref="null"/>.</returns>
        string GetReferral(in string text);
    }
}
