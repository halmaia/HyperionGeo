﻿// Written by Ákos Halmai, University of Pécs, Hungary.
//
// Copyright © Ákos Halmai, 2021. All rights reserved.
// Licensed under the GNU GPL 3.0. See LICENSE file in the project root for full license information.
//

namespace HyperionGeo
{
    public interface IProjection
    {
        public bool TryProjectForward(
            ref EllipsoidalCoordinate coordinateToProject,
            out ProjectedCoordinate projectedCoordinate);

        public EllipsoidalCoordinate ProjectInverse(ref ProjectedCoordinate coordinateToProject);
    }
}