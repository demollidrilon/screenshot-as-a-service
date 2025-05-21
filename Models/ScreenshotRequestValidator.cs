using FluentValidation;

namespace ScreenshotService.Models;
public class ScreenshotRequestValidator : AbstractValidator<ScreenshotRequest>
{
    public ScreenshotRequestValidator()
    {
        RuleFor(x => x.Url).NotEmpty().Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("Invalid URL");
    }
}