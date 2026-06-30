using SchoolAccount.Tests.Common.Fakes;

namespace SchoolAccount.Tests.Common.Factories;

public partial class SchoolAccountWebApplicationFactory
{
    public sealed class Builder
    {
        internal TestQueryHandlerRegistry? HandlerRegistry { get; private set; }
        internal StubFallbackProviderResolver? FallbackProviderResolver { get; private set; }
        internal bool UseAuthentication { get; private set; }
        internal bool UseNoAuthentication { get; private set; }
        internal bool UseDisabledAntiforgery { get; private set; }
        internal bool UseInMemoryDatabase { get; private set; }

        public Builder WithTestDoubles(
            TestQueryHandlerRegistry handlerRegistry,
            StubFallbackProviderResolver fallbackProviderResolver
        )
        {
            HandlerRegistry = handlerRegistry;
            FallbackProviderResolver = fallbackProviderResolver;
            return this;
        }

        public Builder WithAuthentication()
        {
            UseAuthentication = true;
            return this;
        }

        public Builder WithoutAuthentication()
        {
            UseNoAuthentication = true;
            return this;
        }

        public Builder WithInMemoryDatabase()
        {
            UseInMemoryDatabase = true;
            return this;
        }

        public Builder WithDisabledAntiforgery()
        {
            UseDisabledAntiforgery = true;
            return this;
        }

        public SchoolAccountWebApplicationFactory Build()
        {
            return new SchoolAccountWebApplicationFactory(this);
        }
    }
}
