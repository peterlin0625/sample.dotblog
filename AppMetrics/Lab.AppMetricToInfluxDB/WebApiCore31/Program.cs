using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Filtering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebApiCore31
{
    public class Program
    {
        private const string InfluxDbUrl  = "http://127.0.0.1:8086";
        private const string InfluxDbName = "appmetricsreservoirs";

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var filter = new MetricsFilter();

            //filter.WhereContext(c => c == MetricsRegistry.Context);

            return Host.CreateDefaultBuilder(args)
                       .ConfigureMetricsWithDefaults(builder =>
                                                     {
                                                         builder.Filter.With(filter);
                                                         builder.Report.ToInfluxDb(InfluxDbUrl, InfluxDbName,
                                                                                   TimeSpan.FromSeconds(1));
                                                     })
                       .UseMetrics()
                       .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}