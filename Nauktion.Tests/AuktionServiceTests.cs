using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nauktion.Models;
using Nauktion.Repositories;
using Nauktion.Services;

namespace Nauktion.Tests
{
    [TestClass]
    public class AuktionServiceTests
    {
        [TestMethod]
        public void GetAuktion_GoesThroughService()
        {
            // Arrange
            var mockRepo = new Mock<IAuktionRepository>();
            var service = new AuktionService(mockRepo.Object);

            var model = new AuktionModel();
            mockRepo.Setup(t => t.GetAuktion(It.IsAny<int>()))
                .ReturnsAsync(model)
                .Verifiable();

            // Act
            AuktionModel result = service.GetAuktion(1).Result;

            // Assert
            Assert.AreSame(model, result);
            mockRepo.Verify();
        }

        [TestMethod]
        public void ListAuktions_GoesThroughService()
        {
            // Arrange
            var mockRepo = new Mock<IAuktionRepository>();
            var service = new AuktionService(mockRepo.Object);

            List<AuktionModel> modelList = GetMeSomeAuktions();
            mockRepo.Setup(t => t.ListAuktions())
                .ReturnsAsync(modelList)
                .Verifiable();

            // Act
            List<AuktionModel> result = service.ListAuktions().Result;

            // Assert
            CollectionAssert.AreEqual(modelList, result);
            mockRepo.Verify();
        }

        private static List<AuktionModel> GetMeSomeAuktions() => new List<AuktionModel>
        {
            new AuktionModel {AuktionID = 1, Titel = "First", Beskrivning = "First desc", Gruppkod = 1, StartDatum = DateTime.Today, SlutDatum = DateTime.Today.AddMonths(1), SkapadAv = "me", Utropspris = 100},
            new AuktionModel {AuktionID = 2, Titel = "Second", Beskrivning = "Second desc", Gruppkod = 1, StartDatum = DateTime.Today.AddDays(20), SlutDatum = DateTime.Today.AddDays(100), SkapadAv = "me2", Utropspris = 120},
            new AuktionModel {AuktionID = 3, Titel = "Third", Beskrivning = "Third desc", Gruppkod = 1, StartDatum = DateTime.Today.AddMonths(-1), SlutDatum = DateTime.Today.AddMonths(2), SkapadAv = "me3", Utropspris = 200},
            new AuktionModel {AuktionID = 4, Titel = "Forth", Beskrivning = "Forth desc", Gruppkod = 1, StartDatum = DateTime.Today.AddDays(1), SlutDatum = DateTime.Today.AddMonths(1), SkapadAv = "me4", Utropspris = 80},
        };
    }
}