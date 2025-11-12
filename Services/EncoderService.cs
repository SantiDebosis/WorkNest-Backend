using BCrypt.Net;

namespace WorkNest.Services
{
    public interface IEncoderServices
    {
        string Encode(string value);
        bool Verify(string value, string hash);
    }

    public class EncoderServices : IEncoderServices
    {
        public string Encode(string value)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(13);
            string encoded = BCrypt.Net.BCrypt.HashPassword(value, salt);
            return encoded;
        }

        public bool Verify(string value, string hash)
        {
            bool matched = BCrypt.Net.BCrypt.Verify(value, hash);
            return matched;
        }
    }
}