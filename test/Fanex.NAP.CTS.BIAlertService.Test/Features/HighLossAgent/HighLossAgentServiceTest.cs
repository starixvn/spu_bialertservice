namespace Fanex.NAP.CTS.BIAlertService.Tests.Features.HighLossAgent
{
    using Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent;
    using Fanex.NAP.CTS.BIAlertService.Shares;
    using Microsoft.AspNetCore.Routing;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class HighLossAgentServiceTest
    {
        private readonly IHighLossAgentDataAccess _dataAccess;
        private readonly IHighLossAgentService _highLossAgentService;
        

        public HighLossAgentServiceTest() 
        {
            _dataAccess = Substitute.For<IHighLossAgentDataAccess>();
            _highLossAgentService = new HighLossAgentService(_dataAccess);
        }

        #region Constructor Tests
        [Fact]
        public void Constructor_WithValidDataAccess_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var service = new HighLossAgentService(_dataAccess);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithNullDataAccess_ShouldNotThrowException()
        {
            var exception = Record.Exception(() => new HighLossAgentService(null));

            Assert.Null(exception);
        }

        #endregion Constructor Tests

        #region IsReadyAlertHighLossAgent Tests
        [Fact]
        public void IsReadyAlertHighLossAgent_WithValidParameters_ShouldReturnTrue()
        {
            // Arrange
            var rule = RuleHighLossAgent.RuleID_HighLossAgent3Days;
            var checkDate = DateTime.Now;
            _dataAccess.IsReadyAlertHighLossAgent(rule, checkDate).Returns(true);

            // Act
            var result = _highLossAgentService.IsReadyAlertHighLossAgent(rule, checkDate);

            // Assert
            Assert.True(result);
            _dataAccess.Received(1).IsReadyAlertHighLossAgent(rule, checkDate);
        }

        [Fact]
        public void IsReadyAlertHighLossAgent_WithValidParameters_ShouldReturnFalse()
        {
            // Arrange
            var rule = RuleHighLossAgent.RuleID_HighLossAgent7Days;
            var checkDate = DateTime.Now;
            _dataAccess.IsReadyAlertHighLossAgent(rule, checkDate).Returns(false);

            // Act
            var result = _highLossAgentService.IsReadyAlertHighLossAgent(rule, checkDate);

            // Assert
            Assert.False(result);
            _dataAccess.Received(1).IsReadyAlertHighLossAgent(rule, checkDate);
        }

        [Theory]
        [InlineData(RuleHighLossAgent.RuleID_HighLossAgent3Days)]
        [InlineData(RuleHighLossAgent.RuleID_HighLossAgent7Days)]
        public void IsReadyAlertHighLossAgent_WithDifferentRules_ShouldCallDataAccess(RuleHighLossAgent rule)
        {
            // Arrange
            var checkDate = DateTime.Now;
            _dataAccess.IsReadyAlertHighLossAgent(rule, checkDate).Returns(true);

            // Act
            _highLossAgentService.IsReadyAlertHighLossAgent(rule, checkDate);

            // Assert
            _dataAccess.Received(1).IsReadyAlertHighLossAgent(rule, checkDate);
        }
        #endregion IsReadyAlertHighLossAgent Tests

        #region GetAlertHighLossAgent Tests
        [Fact]
        public void GetAlertHighLossAgent_WithValidParameters_ShouldReturnCorrectModel()
        {
            // Arrange
            var rule = RuleHighLossAgent.RuleID_HighLossAgent3Days;
            var captureDate = DateTime.Now;
            var batchSize = 5;
            var expectedModel = new HighLossAgentAlertModel
            {
                TopCust = new List<HighLossAgentTopCustEntity>
                {
                    new HighLossAgentTopCustEntity { CustID = 1, UserName = "User1" },
                    new HighLossAgentTopCustEntity { CustID = 2, UserName = "User2" }
                },
                FromDate = DateTime.Now.AddDays(-3),
                ToDate = DateTime.Now,
                TotalCust = 10
            };

            _dataAccess.GetAlertHighLossAgent(rule, captureDate, batchSize).Returns(expectedModel);

            // Act
            var result = _highLossAgentService.GetAlertHighLossAgent(rule, captureDate, batchSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedModel.TotalCust, result.TotalCust);
            Assert.Equal(2, result.TopCust.Count());
            Assert.Equal(expectedModel.FromDate, result.FromDate);
            Assert.Equal(expectedModel.ToDate, result.ToDate);
            _dataAccess.Received(1).GetAlertHighLossAgent(rule, captureDate, batchSize);
        }

        [Fact]
        public void GetAlertHighLossAgent_WithEmptyTopCust_ShouldReturnModelWithEmptyCollection()
        {
            // Arrange
            var rule = RuleHighLossAgent.RuleID_HighLossAgent7Days;
            var captureDate = DateTime.Now;
            var batchSize = 5;
            var expectedModel = new HighLossAgentAlertModel
            {
                TopCust = Enumerable.Empty<HighLossAgentTopCustEntity>(),
                FromDate = DateTime.Now.AddDays(-7),
                ToDate = DateTime.Now,
                TotalCust = 0
            };

            _dataAccess.GetAlertHighLossAgent(rule, captureDate, batchSize).Returns(expectedModel);

            // Act
            var result = _highLossAgentService.GetAlertHighLossAgent(rule, captureDate, batchSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalCust);
            Assert.Empty(result.TopCust);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void GetAlertHighLossAgent_WithDifferentBatchSizes_ShouldCallDataAccess(int batchSize)
        {
            // Arrange
            var rule = RuleHighLossAgent.RuleID_HighLossAgent3Days;
            var captureDate = DateTime.Now;
            var expectedModel = new HighLossAgentAlertModel();

            _dataAccess.GetAlertHighLossAgent(rule, captureDate, batchSize).Returns(expectedModel);

            // Act
            _highLossAgentService.GetAlertHighLossAgent(rule, captureDate, batchSize);

            // Assert
            _dataAccess.Received(1).GetAlertHighLossAgent(rule, captureDate, batchSize);
        }
        #endregion GetAlertHighLossAgent Tests

        #region Edge Case Tests
        [Fact]
        public void GetAlertHighLossAgent_WhenDataAccessReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var rule = RuleHighLossAgent.RuleID_HighLossAgent3Days;
            var captureDate = DateTime.Now;
            var batchSize = 5;

            _dataAccess.GetAlertHighLossAgent(rule, captureDate, batchSize).Returns((HighLossAgentAlertModel)null);

            // Act
            var result = _highLossAgentService.GetAlertHighLossAgent(rule, captureDate, batchSize);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void IsReadyAlertHighLossAgent_WithMinDateTime_ShouldCallDataAccess()
        {
            // Arrange
            var rule = RuleHighLossAgent.RuleID_HighLossAgent3Days;
            var checkDate = DateTime.MinValue;

            _dataAccess.IsReadyAlertHighLossAgent(rule, checkDate).Returns(false);

            // Act
            var result = _highLossAgentService.IsReadyAlertHighLossAgent(rule, checkDate);

            // Assert
            Assert.False(result);
            _dataAccess.Received(1).IsReadyAlertHighLossAgent(rule, checkDate);
        }

        [Fact]
        public void IsReadyAlertHighLossAgent_WithMaxDateTime_ShouldCallDataAccess()
        {
            // Arrange
            var rule = RuleHighLossAgent.RuleID_HighLossAgent7Days;
            var checkDate = DateTime.MaxValue;

            _dataAccess.IsReadyAlertHighLossAgent(rule, checkDate).Returns(true);

            // Act
            var result = _highLossAgentService.IsReadyAlertHighLossAgent(rule, checkDate);

            // Assert
            Assert.True(result);
            _dataAccess.Received(1).IsReadyAlertHighLossAgent(rule, checkDate);
        }

        #endregion Edge Case Tests
    }
}
