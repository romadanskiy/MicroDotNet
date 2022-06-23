namespace WhiteRabbit_WebApi.Infrastructure
{
    public class ImageConvertion
    {
        public static byte[] ConvertToByteArray(IFormFile file)
        {
            byte[] fileInByte;

            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                fileInByte = binaryReader.ReadBytes((int)file.Length);
            }

            return fileInByte;
        }
    }
}
