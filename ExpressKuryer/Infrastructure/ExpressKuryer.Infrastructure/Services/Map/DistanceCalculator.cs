using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.Operation.Distance;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

public  class FindDistance
{
    public static double FindDist(double latitude1, double longitude1, double latitude2, double longitude2)
    {
        const double EarthRadiusKm = 6371; // Değer düzeltmesi: EarthRadiusKm değeri 4326 yerine 6371 olmalıdır.

        double lat1Rad = Math.PI * latitude1 / 180;
        double lon1Rad = Math.PI * longitude1 / 180;
        double lat2Rad = Math.PI * latitude2 / 180;
        double lon2Rad = Math.PI * longitude2 / 180;
        double dLon = lon2Rad - lon1Rad;
        double dLat = lat2Rad - lat1Rad;
        double a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(dLon / 2), 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distanceInKilometers = EarthRadiusKm * c;
        return distanceInKilometers;
    }

}

