using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApp.Helpers;

public class ConfigureModelBindingLocalization : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => String.Format(Base.Resources.Common.ValueIsInvalidAccessor, x));
        options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => String.Format(Base.Resources.Common.AttemptedValueIsInvalidAccessor, x, y));
        options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => String.Format(Base.Resources.Common.MissingBindRequiredValueAccessor, x));
        options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => String.Format(Base.Resources.Common.MissingKeyOrValueAccessor));
        options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => String.Format(Base.Resources.Common.UnknownValueIsInvalidAccessor, x));
        options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => String.Format(Base.Resources.Common.MissingRequestBodyRequiredValueAccessor));
        options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => String.Format(Base.Resources.Common.ValueMustBeANumberAccessor, x));
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => String.Format(Base.Resources.Common.ValueMustNotBeNullAccessor, x));
        options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => String.Format(Base.Resources.Common.NonPropertyAttemptedValueIsInvalidAccessor, x));
        options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => String.Format(Base.Resources.Common.NonPropertyUnknownValueIsInvalidAccessor));
        options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => String.Format(Base.Resources.Common.NonPropertyValueMustBeANumberAccessor));
    }
}