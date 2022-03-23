using System.Threading.Tasks;
using Prism.Events;
using Shiny.Locations;
using ShinyPOCDryIoc.ViewModels;

namespace ShinyPOCDryIoc
{
    public class GpsDelegate : IGpsDelegate
    {
        private readonly IEventAggregator _eventAggregator;

        public GpsDelegate(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public Task OnReading(IGpsReading reading)
        {
            _eventAggregator.GetEvent<LocationUpdateEvent>().Publish($"{reading.Position.Latitude} / {reading.Position.Longitude} - H: {reading.Heading}");
            return Task.CompletedTask;
        }
    }
}