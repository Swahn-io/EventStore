using EventStoreRepository.Common.Test.AutoData.Customizations;

namespace EventStoreRepository.Common.Test.AutoData
{
    public class EventStoreRepositoryAutoData : AutoNSubstituteData
    {
        public EventStoreRepositoryAutoData() : base(fixture =>
        {
            fixture.Customize(new PositionCustomization());

        })
        {
        }
    }
}