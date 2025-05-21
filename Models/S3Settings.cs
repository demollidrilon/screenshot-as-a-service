namespace ScreenshotService.Models;

public class S3Settings
{
    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int LinkExpiryMinutes { get; set; } = 60;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(BucketName) ||
            string.IsNullOrWhiteSpace(Region) ||
            string.IsNullOrWhiteSpace(AccessKey) ||
            string.IsNullOrWhiteSpace(SecretKey))
        {
            throw new ApplicationException("S3 configuration is invalid or incomplete.");
        }
    }
}