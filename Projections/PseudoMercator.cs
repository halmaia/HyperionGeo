﻿using System;
using static System.Math;
using System.Runtime.CompilerServices;

namespace HyperionGeo
{
    public record PseudoMercator : IProjection
    {
        private const string K0NotFinite = "k0 must be a finite, floating point number!";
        public PseudoMercator(double k0)
        {
            if (FiniteChecks.IsNonFinite(k0))
                throw new NotFiniteNumberException(K0NotFinite, k0);

            this.k0 = k0;
        }

        public double k0 { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; init; }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        EllipsoidalCoordinate IProjection.ProjectInverse(ref ProjectedCoordinate coordinateToProject)
        {
            double k0 = this.k0;
            coordinateToProject.QueryXYZ(out double x, out double y, out double z);
            return new(
                lon: x / k0,
                lat: Atan(Sinh(y / k0)),
                height: z,
                untrusted: false,
                radianLonAndLat: true);              
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        bool IProjection.TryProjectForward(
            ref EllipsoidalCoordinate coordinateToProject,
            out ProjectedCoordinate projectedCoordinate)
        {
            const double PIp4 = 0.78539816339744830961566084581988;
            double k0 = this.k0;
            coordinateToProject.QueryLatLonHeight(out double lon_radians, out double lat_radians, out double height_meters);

            projectedCoordinate = new(k0 * lon_radians,
                                      k0 * Log(Tan(PIp4 + ScaleB(lat_radians, -1))),
                                      height_meters,
                                      false);
            return true;
        }
    }
}