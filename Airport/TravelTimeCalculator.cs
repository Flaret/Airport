using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Airport
{
    public class TravelTimeCalculator
    {
        private readonly ICommandLine commandLine;

        public TravelTimeCalculator(ICommandLine commandLine)
        {
            this.commandLine = commandLine;
        }

        public TimeSpan GetTravelTime(string cityOfDeparture, string cityOfArrival, double averageSpeedKmH)
        {
            var loacationElementDeparture = GetLocation(cityOfDeparture);

            if (loacationElementDeparture == null)
            {
                commandLine.Write("Returning a test travel time: 3 hours.");
                return TimeSpan.FromHours(3);
            }

            var loacationElementArrival = GetLocation(cityOfArrival);

            if (loacationElementArrival == null)
            {
                commandLine.Write("Returning a test travel time: 3 hours.");
                return TimeSpan.FromHours(3);
            }

            var distance = GetDistance(loacationElementDeparture, loacationElementArrival) / 1000;

            return TimeSpan.FromHours(Math.Round(distance / averageSpeedKmH, 2));
        }

        private double GetDistance(XElement locationElementDeparture, XElement locationElementArrival)
        {
            var latDeparture = (double)locationElementDeparture.Element("lat");
            var lngDeparture = (double)locationElementDeparture.Element("lng");
            var latArrival = (double)locationElementArrival.Element("lat");
            var lngArrival = (double)locationElementArrival.Element("lng");
            var fromCoord = new GeoCoordinate(latDeparture, lngDeparture);
            var toCoord = new GeoCoordinate(latArrival, lngArrival);
            return fromCoord.GetDistanceTo(toCoord);
        }

        private XElement GetLocation(string address)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));
            var request = WebRequest.Create(requestUri);

            if (!IsInternetAvailable())
            {
                commandLine.Write("Internet connection is not available.");
                return null;
            }

            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());
            var result = xdoc.Element("GeocodeResponse").Element("result");
            var locationElement = result.Element("geometry").Element("location");

            return locationElement;
        }

        private bool IsInternetAvailable()
        {
            try
            {
                Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch (SocketException ex)
            {
                return false;
            }
        }
    }
}
