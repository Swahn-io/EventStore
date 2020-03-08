using AutoFixture;
using EventStore.ClientAPI;

namespace EventStoreRepository.Common.Test.AutoData.Customizations
{
    public class PositionCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var position = new Position();
            fixture.Inject(position);
        }
    }
}