﻿// This software is part of the LittleBlocks framework
// Copyright (C) 2019 LittleBlocks
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace LittleBlocks.Azure.AspNetCore.AppInsights
{
    public static class ServiceCollectionExtensions
    {
        public static ITelemetryConfigure AddTelemetry(this IServiceCollection services,
            Action<ApplicationInsightsServiceOptions> configure = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddApplicationInsightsTelemetry(options => configure?.Invoke(options));
            services.AddSingleton<ITelemetryInitializer, ApplicationInfoTelemetryInitializer>();
            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
            {
                module.EnableSqlCommandTextInstrumentation = true;
            });

            return new TelemetryConfigure(services);
        }
    }
}