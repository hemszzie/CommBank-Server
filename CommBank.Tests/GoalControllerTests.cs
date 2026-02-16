using CommBank.Controllers;
using CommBank.Services;
using CommBank.Models;
using CommBank.Tests.Fake;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CommBank.Tests
{
    public class GoalControllerTests
    {
        private readonly FakeCollections collections;

        public GoalControllerTests()
        {
            collections = new();
        }

        [Fact]
        public async Task GetForUser()
        {
            // Arrange
            var goals = collections.GetGoals();
            var users = collections.GetUsers();

            IGoalsService goalsService = new FakeGoalsService(goals, goals[0]);
            IUsersService usersService = new FakeUsersService(users, users[0]);

            GoalController controller = new(goalsService, usersService);

            var httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            ActionResult<IEnumerable<Goal>> result =
                await controller.GetForUser(goals[0].UserId!);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);

            foreach (var goal in result.Value!)
            {
                Assert.IsAssignableFrom<Goal>(goal);
                Assert.Equal(goals[0].UserId, goal.UserId);
            }
        }
    }
}
