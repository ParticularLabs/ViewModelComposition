using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PerformanceTests
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Warming up...");
            var tests = new Tests();
            for (int i = 0; i < 10; i++)
            {
                await tests.Warmup();
            }

            Console.WriteLine("Warm-up completed.");

            var baselineTimes = new List<long>();
            var uiTimes = new List<long>();
            var gatewayTimes = new List<long>();

            Console.WriteLine("Starting tests...");
            Console.WriteLine();

            var sw = Stopwatch.StartNew();
            sw.Reset();

            for (var i = 0; i < 10000; i++)
            {
                sw.Start();
                await tests.EShopUIBaseline();
                TrackTimeAndReset(baselineTimes, sw);

                sw.Start();
                await tests.EShopUI();
                TrackTimeAndReset(uiTimes, sw);

                sw.Start();
                await tests.EShopCompositionGateway();
                TrackTimeAndReset(gatewayTimes, sw);

                Console.Write(".");
            }

            var baselineAverageTime = baselineTimes.Average();
            var uiAverageTime = uiTimes.Average();
            var gatewayAverageTime = gatewayTimes.Average();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"MVC Baseline application:\t\t\t\t {baselineAverageTime} (ticks, Average Time)");
            Console.WriteLine($"UI Composition on top of MVC:\t\t\t\t {uiAverageTime} (ticks, Average Time)");
            Console.WriteLine($"Gateway Composition as stand alone reverse proxy:\t {gatewayAverageTime} (ticks, Average Time)");
            Console.WriteLine();
            Console.WriteLine("Completed.");
            Console.Read();
        }

        static void TrackTimeAndReset(List<long> bag, Stopwatch sw)
        {
            bag.Add(sw.ElapsedTicks);
            sw.Reset();
        }
    }

    public class Tests
    {
        HttpClient baselineClient = null;
        HttpClient uiClient = null;
        HttpClient gatewayClient = null;

        public Tests()
        {
            baselineClient = new WebApplicationFactory<EShop.UI.Baseline.Startup>()
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            uiClient = new WebApplicationFactory<EShop.UI.Startup>()
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            gatewayClient = new WebApplicationFactory<EShop.CompositionGateway.Startup>()
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }

        public async Task Warmup()
        {
            await EShopUIBaseline();
            await EShopUI();
            await EShopCompositionGateway();
        }

        public async Task EShopUIBaseline()
        {
            await baselineClient.GetAsync("/api/products/1");
        }

        public async Task EShopUI()
        {
            await uiClient.GetAsync("/api/products/1");
        }

        public async Task EShopCompositionGateway()
        {
            await gatewayClient.GetAsync("/products/1?foo=foo");
        }
    }
}