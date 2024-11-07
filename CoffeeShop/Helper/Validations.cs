namespace CoffeeShop.Helper
{
    public static class Validations
    {
        public static bool IsString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            return input.Any(char.IsLetter);
        }

        public static bool IsImageFile(IFormFile file)
        {
            var validTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/jpeg" };

            if (file == null && file.Length < 0)
            {
                return false;
            }
            return validTypes.Contains(file.ContentType);
        }

    }

}