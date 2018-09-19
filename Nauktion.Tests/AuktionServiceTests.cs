using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var service = new AuktionService(mockRepo.Object, null);

            var model = new AuktionModel();
            mockRepo.Setup(t => t.GetAuktionAsync(It.IsAny<int>()))
                .ReturnsAsync(model)
                .Verifiable();

            // Act
            AuktionModel result = service.GetAuktionAsync(1).Result;

            // Assert
            Assert.AreSame(model, result);
            mockRepo.Verify();
        }

        [TestMethod]
        public void ListAuktions_FilterOutClosed()
        {
            // Arrange
            var mockRepo = new Mock<IAuktionRepository>();
            var service = new AuktionService(mockRepo.Object, null);

            List<AuktionModel> modelList = GetMeSomeAuktions();
            List<AuktionModel> expected = modelList
                .Where(a => !a.IsClosed())
                .ToList();

            mockRepo.Setup(t => t.ListAuktionsAsync())
                .ReturnsAsync(modelList)
                .Verifiable();

            // Act
            List<AuktionModel> result = service.ListAuktionsAsync(includeClosed: false).Result;

            // Assert
            mockRepo.Verify();
            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void ListAuktions_OrderByDate()
        {
            // Arrange
            var mockRepo = new Mock<IAuktionRepository>();
            var service = new AuktionService(mockRepo.Object, null);

            List<AuktionModel> modelList = GetMeSomeAuktions();
            List<AuktionModel> expected = modelList
                .OrderBy(a => a.StartDatum)
                .ToList();

            mockRepo.Setup(t => t.ListAuktionsAsync())
                .ReturnsAsync(modelList)
                .Verifiable();

            // Act
            List<AuktionModel> result = service.ListAuktionsAsync(includeClosed: true).Result;

            // Assert
            mockRepo.Verify();
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ListBuds_OrderByBid()
        {
            // Arrange
            var mockRepo = new Mock<IAuktionRepository>();
            var service = new AuktionService(mockRepo.Object, null);

            List<BudModel> modelList = GetMeSomeBuds(1);
            List<BudModel> expected = modelList.OrderByDescending(b => b.Summa).ToList();

            mockRepo.Setup(t => t.ListBudsAsync(1))
                .ReturnsAsync(modelList)
                .Verifiable();

            // Act
            List<BudModel> result = service.ListBudsAsync(1).Result;

            // Assert
            mockRepo.Verify();
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CreateBud_CallsRepositoryMethod()
        {
            // Arrange
            var mockRepo = new Mock<IAuktionRepository>();
            var service = new AuktionService(mockRepo.Object, null);
            BudModel obj = null;
            var bud = new BiddingViewModel {AuktionID = 1, Summa = 2};
            var user = new NauktionUser {Id = "abcdef"};

            mockRepo.Setup(t => t.CreateBudAsync(It.IsAny<BudModel>()))
                .Callback((BudModel o) => obj = o)
                .Returns(Task.CompletedTask).Verifiable();
            
            // Act
            service.CreateBudAsync(bud, user).GetAwaiter().GetResult();

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.AuktionID);
            Assert.AreEqual(2, obj.Summa);
            Assert.AreEqual(user.Id, obj.Budgivare);
            mockRepo.Verify();
        }

        private static List<BudModel> GetMeSomeBuds(int auktionId) => new List<BudModel>
        {
            new BudModel {AuktionID = auktionId, BudID = auktionId * 100 + 1, Budgivare = "Entera boss", Summa = 350},
            new BudModel {AuktionID = auktionId, BudID = auktionId * 100 + 2, Budgivare = "Smith Phill", Summa = 300},
            new BudModel {AuktionID = auktionId, BudID = auktionId * 100 + 3, Budgivare = "John doe", Summa = 250},
            new BudModel {AuktionID = auktionId, BudID = auktionId * 100 + 4, Budgivare = "Tom von Bom", Summa = 455},
        };

        private static List<AuktionModel> GetMeSomeAuktions() => new List<AuktionModel>
        {
            new AuktionModel {AuktionID = 1, Titel = "First", Beskrivning = "First desc", Gruppkod = 1, StartDatum = DateTime.Today, SlutDatum = DateTime.Today.AddMonths(1), SkapadAv = "me", Utropspris = 100},
            new AuktionModel {AuktionID = 2, Titel = "Second", Beskrivning = "Second desc", Gruppkod = 1, StartDatum = DateTime.Today.AddDays(20), SlutDatum = DateTime.Today.AddDays(100), SkapadAv = "me2", Utropspris = 120},
            new AuktionModel {AuktionID = 3, Titel = "Third", Beskrivning = "Third desc", Gruppkod = 1, StartDatum = DateTime.Today.AddMonths(-1), SlutDatum = DateTime.Today.AddMonths(2), SkapadAv = "me3", Utropspris = 200},
            new AuktionModel {AuktionID = 4, Titel = "Forth", Beskrivning = "Forth desc", Gruppkod = 1, StartDatum = DateTime.Today.AddDays(1), SlutDatum = DateTime.Today.AddMonths(1), SkapadAv = "me4", Utropspris = 80},
            new AuktionModel {AuktionID = 5, Titel = "Fifth", Beskrivning = "This one has ended", Gruppkod = 1, StartDatum = DateTime.Today.AddMonths(-3), SlutDatum = DateTime.Today.AddMonths(-1), SkapadAv = "me4", Utropspris = 25},
        };
    }
}