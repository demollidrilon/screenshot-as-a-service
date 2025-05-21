using Microsoft.Extensions.Options;
using Amazon.S3;
using Amazon.S3.Model;
using Prometheus;
using Metrics = Prometheus.Metrics;
using ScreenshotService.Helpers;
using ScreenshotService.Models;

namespace ScreenshotService.Services;

public class ScreenshotGenerator
{
    private readonly ILogger<ScreenshotGenerator> _logger;
    private readonly S3Settings _s3Settings;
    private readonly IAmazonS3 _s3;
    private readonly PuppeteerHelper _helper;

    public ScreenshotGenerator(ILogger<ScreenshotGenerator> logger, IOptions<S3Settings> s3Options, IAmazonS3 s3, PuppeteerHelper helper)
    {
        _logger = logger;
        _s3Settings = s3Options.Value;
        _s3 = s3;
        _helper = helper;
    }

    private static readonly Counter ScreenshotCounter = Metrics.CreateCounter("screenshots_taken_total", "Total number of screenshots generated");

    public async Task<ScreenshotResponse> GenerateAsync(ScreenshotRequest request)
    {
        var format = request.Format.ToString().ToLower();
        var viewPort = request.Viewport.ToString().ToLower();
        var fileName = $"{Guid.NewGuid()}.{format}";
        var fileBytes = await _helper.CaptureScreenshotAsync(request.Url, format, viewPort);

        var uploadRequest = new PutObjectRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = fileName,
            InputStream = new MemoryStream(fileBytes),
            ContentType = format == "pdf" ? "application/pdf" : "image/png"
        };

        try
        {
            await _s3.PutObjectAsync(uploadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload screenshot to S3: {FileName}", fileName);
            throw new ApplicationException("S3 upload failed");
        }

        var preSignedUrl = _s3.GetPreSignedURL(new GetPreSignedUrlRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = fileName,
            Expires = DateTime.UtcNow.AddMinutes(_s3Settings.LinkExpiryMinutes),
            Protocol = Protocol.HTTPS
        });

        ScreenshotCounter.Inc();

        return new ScreenshotResponse
        {
            FileName = fileName,
            ScreenshotUrl = preSignedUrl
        };
    }
}