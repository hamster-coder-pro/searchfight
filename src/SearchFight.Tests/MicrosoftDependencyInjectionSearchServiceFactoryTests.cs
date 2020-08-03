using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Search.Common;
using SearchFight.Common.Interfaces;
using SearchFight.Common.Models;
using SearchFight.Common.Services;

namespace SearchFight.Tests
{
    public class MicrosoftDependencyInjectionSearchServiceFactoryTests
    {
        [Test]
        public void CreateReportProviderSucceed()
        {
            var serviceCollection = new ServiceCollection();
            var reporter1 = new Mock<ISearchReportProvider<ISearchReportModel>>();
            var reporter2 = new Mock<ISearchReportProvider<ISearchReportModel>>();
            serviceCollection.AddTransient(sp => reporter1.Object);
            serviceCollection.AddTransient(sp => reporter2.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var factory = new MicrosoftDependencyInjectionSearchServiceFactory(serviceProvider);
            var result = factory.CreateReportProvider<ISearchReportModel>();
            CollectionAssert.AreEquivalent(new[] { reporter1.Object, reporter2.Object }, result);
        }

        [Test]
        public void CreateReportProviderException()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ISearchReportProvider<ISearchReportModel>>(sp => throw new Exception());
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var factory = new MicrosoftDependencyInjectionSearchServiceFactory(serviceProvider);
            Assert.Throws(typeof(SearchException), () => factory.CreateReportProvider<ISearchReportModel>());
        }


        [Test]
        public void CreateSearchProviderSucceed()
        {
            var serviceCollection = new ServiceCollection();
            var expected1 = new Mock<ISearchProvider<ISearchRequestModel, ISearchResultModel>>();
            var expected2 = new Mock<ISearchProvider<ISearchRequestModel, ISearchResultModel>>();
            serviceCollection.AddTransient(sp => expected1.Object);
            serviceCollection.AddTransient(sp => expected2.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var factory = new MicrosoftDependencyInjectionSearchServiceFactory(serviceProvider);
            var result = factory.CreateSearchProvider<ISearchRequestModel, ISearchResultModel>();
            CollectionAssert.AreEquivalent(new [] { expected1.Object, expected2.Object }, result);
        }

        [Test]
        public void CreateSearchProviderException()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ISearchProvider<ISearchRequestModel, ISearchResultModel>>(sp => throw new Exception());
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var factory = new MicrosoftDependencyInjectionSearchServiceFactory(serviceProvider);
            Assert.Throws(typeof(SearchException), () => factory.CreateSearchProvider<ISearchRequestModel, ISearchResultModel>());
        }

        [Test]
        public void CreateReportBuilderSucceed()
        {
            var serviceCollection = new ServiceCollection();
            var expected1 = new Mock<ISearchReportBuilder<ISearchResultModel, ISearchReportModel>>();
            serviceCollection.AddTransient(sp => expected1.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var factory = new MicrosoftDependencyInjectionSearchServiceFactory(serviceProvider);
            var result = factory.CreateReportBuilder<ISearchResultModel, ISearchReportModel>();

            result.Should().Be(expected1.Object);
        }

        [Test]
        public void CreateReportBuilderException()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ISearchReportBuilder<ISearchResultModel, ISearchReportModel>>(sp => throw new Exception());
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var factory = new MicrosoftDependencyInjectionSearchServiceFactory(serviceProvider);
            Assert.Throws(typeof(SearchException), () => factory.CreateReportBuilder<ISearchResultModel, ISearchReportModel>());
        }
    }
}