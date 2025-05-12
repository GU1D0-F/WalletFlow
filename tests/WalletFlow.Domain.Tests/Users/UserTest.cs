using WalletFlow.Domain.Entities.Users;

namespace WalletFlow.Domain.Tests.Users;

public class UserTest
{
    [Fact]
        public void Create_ShouldInitializeProperties()
        {
            // Arrange
            var firstName = "John";
            var lastName  = "Doe";
            var email     = "john.doe@example.com";
            var before    = DateTime.UtcNow;

            // Act
            var user = User.Create(firstName, lastName, email);
            var after = DateTime.UtcNow;

            // Assert
            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName,  user.LastName);
            Assert.Equal(email,     user.Email);
            Assert.Equal(email,     user.UserName);

            // CreatedAt foi atribuído e está entre before e after
            Assert.True(user.CreatedAt >= before, 
                "CreatedAt deve ser maior ou igual ao instante antes da criação");
            Assert.True(user.CreatedAt <= after,  
                "CreatedAt deve ser menor ou igual ao instante após a criação");

            // LastLogin começa null
            Assert.Null(user.LastLogin);
        }

        [Fact]
        public void UpdateLastLogin_ShouldSetLastLoginBetweenBeforeAndAfter()
        {
            // Arrange
            var user = User.Create("Jane", "Smith", "jane.smith@example.com");
            Assert.Null(user.LastLogin);

            var beforeLogin = DateTime.UtcNow;

            // Act
            user.UpdateLastLogin();
            var afterLogin = DateTime.UtcNow;

            // Assert
            Assert.NotNull(user.LastLogin);

            // LastLogin está entre antes e depois da chamada
            Assert.True(user.LastLogin.Value >= beforeLogin, 
                "LastLogin deve ser maior ou igual ao instante antes da atualização");
            Assert.True(user.LastLogin.Value <= afterLogin,  
                "LastLogin deve ser menor ou igual ao instante após a atualização");

            // E é posterior ou igual ao CreatedAt
            Assert.True(user.LastLogin.Value >= user.CreatedAt, 
                "LastLogin deve ser posterior ou igual a CreatedAt");
        }
}