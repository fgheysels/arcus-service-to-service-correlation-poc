﻿using System;
using Arcus.POC.WebApi.Logging;
using Arcus.POC.WebApi.Logging.Correlation;
using GuardNet;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder 
{
    /// <summary>
    /// Extra extensions on the <see cref="IApplicationBuilder"/> for logging.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="CustomExceptionHandlingMiddleware"/> type to the application's request pipeline.
        /// </summary>
        /// <param name="app">The builder to configure the application's request pipeline.</param>
        public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder app)
        {
            Guard.NotNull(app, nameof(app));

            return app.UseMiddleware<CustomExceptionHandlingMiddleware>();
        }

        /// <summary>
        /// Adds the <see cref="CustomRequestTrackingMiddleware"/> type to the application's request pipeline.
        /// </summary>
        /// <param name="app">The builder to configure the application's request pipeline.</param>
        /// <param name="configureOptions">The optional options to configure the behavior of the request tracking.</param>
        public static IApplicationBuilder UseCustomRequestTracking(
            this IApplicationBuilder app,
            Action<RequestTrackingOptions> configureOptions = null)
        {
            Guard.NotNull(app, nameof(app));

            return UseCustomRequestTracking<CustomRequestTrackingMiddleware>(app, configureOptions);
        }

        /// <summary>
        /// Adds the <see cref="CustomRequestTrackingMiddleware"/> type to the application's request pipeline.
        /// </summary>
        /// <param name="app">The builder to configure the application's request pipeline.</param>
        /// <param name="configureOptions">The optional options to configure the behavior of the request tracking.</param>
        public static IApplicationBuilder UseCustomRequestTracking<TMiddleware>(
            this IApplicationBuilder app,
            Action<RequestTrackingOptions> configureOptions = null)
            where TMiddleware : CustomRequestTrackingMiddleware
        {
            Guard.NotNull(app, nameof(app));

            var options = new RequestTrackingOptions();
            configureOptions?.Invoke(options);

            return app.UseMiddleware<TMiddleware>(options);
        }

        /// <summary>
        /// Adds operation and transaction correlation to the application by using the <see cref="CustomCorrelationMiddleware"/> in the request pipeline.
        /// </summary>
        /// <param name="app">The builder to configure the application's request pipeline.</param>
        public static IApplicationBuilder UseCustomHttpCorrelation(this IApplicationBuilder app)
        {
            Guard.NotNull(app, nameof(app));

            return app.UseMiddleware<CustomCorrelationMiddleware>();
        }

        /// <summary>
        /// Adds the <see cref="CustomVersionTrackingMiddleware"/> component to the application request's pipeline to automatically include the application version to the response.
        /// </summary>
        /// <param name="app">The builder to configure the application's request pipeline.</param>
        /// <param name="configureOptions">
        ///     The optional function to configure the version tracking options that will influence the behavior of the version tracking functionality.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="app"/> is <c>null</c>.</exception>
        /// <remarks>
        ///     WARNING: Only use the version tracking for non-public endpoints otherwise the version information is leaked and it can be used for unintended malicious purposes.
        /// </remarks>
        public static IApplicationBuilder UseCustomVersionTracking(this IApplicationBuilder app, Action<VersionTrackingOptions> configureOptions = null)
        {
            Guard.NotNull(app, nameof(app), "Requires an application builder to add the version tracking middleware");

            var options = new VersionTrackingOptions();
            configureOptions?.Invoke(options);

            return app.UseMiddleware<CustomVersionTrackingMiddleware>(options);
        }
    }
}
