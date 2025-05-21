# Screenshot-as-a-Service

A production-ready, extensible API service that captures website screenshots and returns them as downloadable PNG, PDF, or WebP files. Built with **.NET 9**, **PuppeteerSharp**, and **Amazon S3**.

![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)
![AWS S3](https://img.shields.io/badge/AWS-S3-orange)
![License](https://img.shields.io/badge/license-MIT-brightgreen)

---

## Features

- Capture full-page screenshots from any URL
- Supports PNG, PDF, and WebP formats
- Mobile, desktop, and tablet viewport options
- Screenshots uploaded directly to Amazon S3
- Pre-signed download URL response
- FluentValidation for input validation
- Serilog logging to rolling log files
- Prometheus metrics endpoint (`/metrics`)
- Clean architecture with Strategy Pattern (format + viewport)

---

## API Usage

### Endpoint

```
POST /api/screenshot
```

### Request Body (JSON)

```
{
  "url": "https://twitter.com",
  "format": "Png",        // or "Pdf", "WebP"
  "viewport": "Mobile"    // or "Desktop", "Tablet"
}
```

### Response

```
{
  "success": true,
  "data": {
    "fileName": "abc1234.png",
    "screenshotUrl": "https://your-s3-url.com/abc1234.png"
  },
  "error": ""
}
```

---

## Local Setup

1. Clone the repo:

```bash
git clone https://github.com/your-username/screenshot-as-a-service.git
cd screenshot-as-a-service
```

2. Configure your `appsettings.json`:

```json
"S3": {
  "BucketName": "your-bucket",
  "Region": "your-region",
  "AccessKey": "your-access-key",
  "SecretKey": "your-secret-key",
  "LinkExpiryMinutes": 60
}
```

3. Run the project:

```bash
dotnet run
```

---

## Metrics

Prometheus metrics exposed at:

```
/metrics
```

Includes:

- HTTP duration buckets
- Screenshot render counter
- Exception counters

---

## Tech Stack

- .NET 9
- PuppeteerSharp
- Amazon S3 SDK
- Serilog
- FluentValidation
- Prometheus-net

---

## 📄 License

MIT - free for personal and commercial use.

---
