using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace EventStoreRepository.Common.Test.AutoData
{
    public class AutoNSubstituteData : AutoDataAttribute
    {
        public AutoNSubstituteData(Action<IFixture> initialize) : base(() =>
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoNSubstituteCustomization { GenerateDelegates = true, ConfigureMembers = true });
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
           
            initialize(fixture);

            return fixture;
        }) { }

        public AutoNSubstituteData() : this(fixture => { }) { }
    }
}