﻿// XAML Map Control - https://github.com/ClemensFischer/XAML-Map-Control
// © 2020 Clemens Fischer
// Licensed under the Microsoft Public License (Ms-PL)

using System;
#if !WINDOWS_UWP
using System.Windows;
#endif

namespace MapControl
{
    /// <summary>
    /// Spherical Gnomonic Projection.
    /// </summary>
    public class GnomonicProjection : AzimuthalProjection
    {
        public GnomonicProjection()
        {
            CrsId = "AUTO2:97001"; // GeoServer non-standard CRS ID
        }

        public override Point LocationToMap(Location location)
        {
            if (location.Equals(Center))
            {
                return new Point();
            }

            double azimuth, distance;

            GetAzimuthDistance(Center, location, out azimuth, out distance);

            var mapDistance = distance < Math.PI / 2d
                ? Math.Tan(distance) * UnitsPerDegree * 180d / Math.PI
                : double.PositiveInfinity;

            return new Point(mapDistance * Math.Sin(azimuth), mapDistance * Math.Cos(azimuth));
        }

        public override Location MapToLocation(Point point)
        {
            if (point.X == 0d && point.Y == 0d)
            {
                return new Location(Center.Latitude, Center.Longitude);
            }

            var azimuth = Math.Atan2(point.X, point.Y);
            var mapDistance = Math.Sqrt(point.X * point.X + point.Y * point.Y);

            var distance = Math.Atan(mapDistance / (UnitsPerDegree * 180d / Math.PI));

            return GetLocation(Center, azimuth, distance);
        }
    }
}
