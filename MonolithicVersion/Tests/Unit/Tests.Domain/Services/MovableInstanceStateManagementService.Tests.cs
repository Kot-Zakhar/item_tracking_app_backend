using DataFactories;
using Domain.Aggregates.MovableInstances;
using Domain.Aggregates.Users;
using Domain.Enums;
using Domain.Services;

namespace Tests.Domain.Services;

// TODO: Check the history population

public class MovableInstanceStateManagementServiceTests
{
    private readonly MovableInstanceFactory _movableInstanceFactory = new();
    private readonly MovableItemFactory _movableItemFactory = new();
    private readonly UserDataFactory _userDataFactory = new();
    private readonly LocationDataFactory _locationDataFactory = new();
    private readonly CategoryDataFactory _categoryDataFactory = new();

    #region BookInstance Tests

    [Fact(DisplayName = "BookInstance - Valid Data - No Exception")]
    public void BookInstance_ValidData_NoException()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.CreateMovableInstance(_movableItemFactory.GetRandomMovableItem());
        var user = _userDataFactory.GetRandomUser();

        // Act
        MovableInstanceStateManagementService.BookInstance(movableInstance, user);

        // Assert
        Assert.Equal(MovableInstanceStatus.Booked, movableInstance.Status);
        Assert.Contains(movableInstance, user.MovableInstances);
    }

    [Fact(DisplayName = "BookInstance - Force Take Of Booked Instance - Instance is Booked, associated with new User")]
    public void BookInstance_ForceTakeOfBooked_InstanceIsTaken()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Booked;
        var location = _locationDataFactory.CreateLocation();
        var user1 = _userDataFactory.CreateUser();
        var user2 = _userDataFactory.CreateUser();
        movableInstance.User = user1;
        user1.MovableInstances.Add(movableInstance);
        movableInstance.Location = location;
        location.Instances.Add(movableInstance);

        // Act
        MovableInstanceStateManagementService.BookInstance(movableInstance, user2, force: true);

        // Assert
        Assert.Equal(MovableInstanceStatus.Booked, movableInstance.Status);
        Assert.Equal(user2.Id, movableInstance.User?.Id);
        Assert.Equal(location.Id, movableInstance.Location?.Id);
        Assert.Contains(movableInstance, location.Instances);
        Assert.Contains(movableInstance, user2.MovableInstances);
        Assert.DoesNotContain(movableInstance, user1.MovableInstances);
    }


    [Fact(DisplayName = "BookInstance - Force Take Of Taken Instance - Instance is Booked, associated with new User")]
    public void BookInstance_ForceTakeOfTaken_InstanceIsTaken()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Taken;
        var user1 = _userDataFactory.CreateUser();
        var user2 = _userDataFactory.CreateUser();
        movableInstance.User = user1;
        user1.MovableInstances.Add(movableInstance);

        // Act
        MovableInstanceStateManagementService.BookInstance(movableInstance, user2, force: true);

        // Assert
        Assert.Equal(MovableInstanceStatus.Booked, movableInstance.Status);
        Assert.Equal(user2.Id, movableInstance.User?.Id);
        Assert.Contains(movableInstance, user2.MovableInstances);
        Assert.DoesNotContain(movableInstance, user1.MovableInstances);
    }

    [Fact(DisplayName = "BookInstance - User Already Has Instance - Throws InvalidOperationException")]
    public void BookInstance_UserAlreadyHasInstance_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.CreateMovableInstance(_movableItemFactory.GetRandomMovableItem());
        var user = _userDataFactory.GetRandomUser();
        MovableInstanceStateManagementService.BookInstance(movableInstance, user);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.BookInstance(movableInstance, user));
    }

    [Fact(DisplayName = "BookInstance - Item Is Not Available - Throws InvalidOperationException")]
    public void BookInstance_ItemIsNotAvailable_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        var user = _userDataFactory.GetRandomUser();
        movableInstance.Status = MovableInstanceStatus.Booked; // Set status to not available

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.BookInstance(movableInstance, user));
    }

    [Fact(DisplayName = "BookInstance - Null Movable Instance - Throws InvalidOperationException")]
    public void BookInstance_NullMovableInstance_ThrowsArgumentNullException()
    {
        // Arrange
        User user = _userDataFactory.GetRandomUser();
        MovableInstance? movableInstance = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => MovableInstanceStateManagementService.BookInstance(movableInstance!, user));
    }

    [Fact(DisplayName = "BookInstance - Null User - Throws InvalidOperationException")]
    public void BookInstance_NullUser_ThrowsArgumentNullException()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        User? user = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => MovableInstanceStateManagementService.BookInstance(movableInstance, user!));
    }

    #endregion

    #region BookAnyInstanceInLocation Tests

    [Fact(DisplayName = "BookAnyInstanceInLocation - Valid Data - No Exception")]
    public void BookAnyInstanceInLocation_ValidData_NoException()
    {
        // Arrange
        var movableItem = _movableItemFactory.CreateMovableItem(_categoryDataFactory.CreateCategory());
        var location = _locationDataFactory.CreateLocation();
        var instances = _movableInstanceFactory.GetMovableInstancesOfItem(movableItem, 5);
        var user = _userDataFactory.CreateUser();

        var targetInstance = instances[3];
        targetInstance.Location = location;
        location.Instances.Add(targetInstance);

        // Act
        var bookedInstance = MovableInstanceStateManagementService.BookAnyInstanceInLocation(movableItem, location, user);

        // Assert
        Assert.Equal(targetInstance.Id, bookedInstance.Id);
        Assert.Equal(MovableInstanceStatus.Booked, bookedInstance.Status);
        Assert.Contains(bookedInstance, user.MovableInstances);
        Assert.Contains(bookedInstance, location.Instances);
    }

    [Fact(DisplayName = "BookAnyInstanceInLocation - No Available Instances - Throws InvalidOperationException")]
    public void BookAnyInstanceInLocation_NoAvailableInstance_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableItem = _movableItemFactory.CreateMovableItem(_categoryDataFactory.CreateCategory());
        var location = _locationDataFactory.CreateLocation();
        var instances = _movableInstanceFactory.GetMovableInstancesOfItem(movableItem, 5, location);
        var user = _userDataFactory.CreateUser();

        // Set all instances to not available
        foreach (var instance in instances)
        {
            instance.Status = MovableInstanceStatus.Booked;
            instance.User = user;
            user.MovableInstances.Add(instance);
        }

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.BookAnyInstanceInLocation(movableItem, location, user));
    }

    [Fact(DisplayName = "BookAnyInstanceInLocation - Empty Location - Throws InvalidOperationException")]
    public void BookAnyInstanceInLocation_EmptyLocation_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableItem = _movableItemFactory.CreateMovableItem(_categoryDataFactory.CreateCategory());
        var location = _locationDataFactory.CreateLocation();
        var instances = _movableInstanceFactory.GetMovableInstancesOfItem(movableItem, 5);
        var user = _userDataFactory.CreateUser();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.BookAnyInstanceInLocation(movableItem, location, user));
    }

    #endregion

    #region TakeInstance Tests

    [Fact(DisplayName = "TakeInstance - Valid Data - Instance is Taken, associated with User and detached from Location")]
    public void TakeInstance_ValidData_InstanceIsTaken()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        var location = _locationDataFactory.CreateLocation();
        movableInstance.Location = location;
        location.Instances.Add(movableInstance);
        var user = _userDataFactory.CreateUser();

        // Act
        MovableInstanceStateManagementService.TakeInstance(movableInstance, user);

        // Assert
        Assert.Equal(MovableInstanceStatus.Taken, movableInstance.Status);
        Assert.Equal(user.Id, movableInstance.User?.Id);
        Assert.Null(movableInstance.Location);
        Assert.DoesNotContain(movableInstance, location.Instances);
        Assert.Contains(movableInstance, user.MovableInstances);
    }

    [Fact(DisplayName = "TakeInstance - Instance Already Taken By Same User - Throws InvalidOperationException")]
    public void TakeInstance_InstanceAlreadyTakenBySameUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Taken;
        var user = _userDataFactory.CreateUser();
        movableInstance.User = user;
        user.MovableInstances.Add(movableInstance);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.TakeInstance(movableInstance, user));
    }

    [Fact(DisplayName = "TakeInstance - Instance Already Taken By Another User - Throws InvalidOperationException")]
    public void TakeInstance_InstanceAlreadyTakenByAnotherUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Taken;
        var user1 = _userDataFactory.CreateUser();
        var user2 = _userDataFactory.CreateUser();
        movableInstance.User = user1;
        user1.MovableInstances.Add(movableInstance);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.TakeInstance(movableInstance, user2));
    }

    [Fact(DisplayName = "TakeInstance - User Has Max Instances - Throws InvalidOperationException")]
    public void TakeInstance_UserHasMaxInstances_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableItem = _movableItemFactory.CreateMovableItem(_categoryDataFactory.CreateCategory());
        var movableInstances = _movableInstanceFactory.GetMovableInstancesOfItem(movableItem, User.MaxMovableInstances);
        var user = _userDataFactory.CreateUser();
        foreach (var movableInstance in movableInstances)
        {
            movableInstance.Status = MovableInstanceStatus.Taken;
            movableInstance.User = user;
            user.MovableInstances.Add(movableInstance);
        }
        var newMovableInstance = _movableInstanceFactory.CreateMovableInstance(movableItem);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.TakeInstance(newMovableInstance, user));
    }

    [Fact(DisplayName = "TakeInstance - Force Take Of Booked Instance - Instance is Taken, associated with new User")]
    public void TakeInstance_ForceTakeOfBooked_InstanceIsTaken()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Booked;
        var location = _locationDataFactory.CreateLocation();
        var user1 = _userDataFactory.CreateUser();
        var user2 = _userDataFactory.CreateUser();
        movableInstance.User = user1;
        user1.MovableInstances.Add(movableInstance);
        movableInstance.Location = location;
        location.Instances.Add(movableInstance);

        // Act
        MovableInstanceStateManagementService.TakeInstance(movableInstance, user2, force: true);

        // Assert
        Assert.Equal(MovableInstanceStatus.Taken, movableInstance.Status);
        Assert.Equal(user2.Id, movableInstance.User?.Id);
        Assert.Null(movableInstance.Location);
        Assert.DoesNotContain(movableInstance, location.Instances);
        Assert.Contains(movableInstance, user2.MovableInstances);
        Assert.DoesNotContain(movableInstance, user1.MovableInstances);
    }


    [Fact(DisplayName = "TakeInstance - Force Take Of Taken Instance - Instance is Taken, associated with new User")]
    public void TakeInstance_ForceTakeOfTaken_InstanceIsTaken()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Taken;
        var location = _locationDataFactory.CreateLocation();
        var user1 = _userDataFactory.CreateUser();
        var user2 = _userDataFactory.CreateUser();
        movableInstance.User = user1;
        user1.MovableInstances.Add(movableInstance);

        // Act
        MovableInstanceStateManagementService.TakeInstance(movableInstance, user2, force: true);

        // Assert
        Assert.Equal(MovableInstanceStatus.Taken, movableInstance.Status);
        Assert.Equal(user2.Id, movableInstance.User?.Id);
        Assert.Null(movableInstance.Location);
        Assert.DoesNotContain(movableInstance, location.Instances);
        Assert.Contains(movableInstance, user2.MovableInstances);
        Assert.DoesNotContain(movableInstance, user1.MovableInstances);
    }

    #endregion

    #region CancelBooking Tests

    [Fact(DisplayName = "CancelBooking - Valid Data - Instance is Available and detached from User")]
    public void CancelBooking_ValidData_InstanceIsAvailable()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Booked;
        var user = _userDataFactory.CreateUser();
        movableInstance.User = user;
        user.MovableInstances.Add(movableInstance);
        var location = _locationDataFactory.CreateLocation();
        movableInstance.Location = location;
        location.Instances.Add(movableInstance);

        // Act
        MovableInstanceStateManagementService.CancelBooking(movableInstance, user);

        // Assert
        Assert.Equal(MovableInstanceStatus.Available, movableInstance.Status);
        Assert.Null(movableInstance.User);
        Assert.DoesNotContain(movableInstance, user.MovableInstances);
        Assert.Contains(movableInstance, location.Instances);
        Assert.Equal(location.Id, movableInstance.Location?.Id);
    }

    [Fact(DisplayName = "CancelBooking - Instance Not Booked - Throws InvalidOperationException")]
    public void CancelBooking_InstanceNotBooked_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        var user = _userDataFactory.CreateUser();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.CancelBooking(movableInstance, user));
    }

    [Fact(DisplayName = "CancelBooking - User Not Issuer - Throws InvalidOperationException")]
    public void CancelBooking_UserNotIssuer_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Booked;
        var user1 = _userDataFactory.CreateUser();
        var user2 = _userDataFactory.CreateUser();
        movableInstance.User = user1;
        user1.MovableInstances.Add(movableInstance);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.CancelBooking(movableInstance, user2));
    }

    [Fact(DisplayName = "CancelBooking - Force Cancel Booking - Instance is Available and detached from User")]
    public void CancelBooking_ForceCancelBooking_InstanceIsAvailable()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Booked;
        var user1 = _userDataFactory.CreateUser();
        var user2 = _userDataFactory.CreateUser();
        movableInstance.User = user1;
        user1.MovableInstances.Add(movableInstance);

        // Act
        MovableInstanceStateManagementService.CancelBooking(movableInstance, user2, force: true);
        // Assert
        Assert.Equal(MovableInstanceStatus.Available, movableInstance.Status);
        Assert.Null(movableInstance.User);
        Assert.DoesNotContain(movableInstance, user1.MovableInstances);
        Assert.DoesNotContain(movableInstance, user2.MovableInstances);
    }

    #endregion

    #region ReleaseInstance Tests

    //only for Taken instances
    //can be release by another user if force
    //item should be moved to the location provided

    [Fact(DisplayName = "ReleaseInstance - Valid Data - Instance is Released to New Location")]
    public void ReleaseInstance_ValidData_InstanceIsReleased()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Taken;
        var user = _userDataFactory.CreateUser();
        movableInstance.User = user;
        user.MovableInstances.Add(movableInstance);
        var newLocation = _locationDataFactory.CreateLocation();

        // Act
        MovableInstanceStateManagementService.ReleaseInstance(movableInstance, user, newLocation);

        // Assert
        Assert.Equal(MovableInstanceStatus.Available, movableInstance.Status);
        Assert.Contains(movableInstance, newLocation.Instances);
        Assert.Equal(newLocation.Id, movableInstance.Location?.Id);
        Assert.DoesNotContain(movableInstance, user.MovableInstances);
        Assert.Null(movableInstance.User);
    }

    [Fact(DisplayName = "ReleaseInstance - Instance Not Taken - Throws InvalidOperationException")]
    public void ReleaseInstance_InstanceNotTaken_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        var user = _userDataFactory.CreateUser();
        var newLocation = _locationDataFactory.CreateLocation();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.ReleaseInstance(movableInstance, user, newLocation));
    }

    [Fact(DisplayName = "ReleaseInstance - User Not Issuer - Throws InvalidOperationException")]
    public void ReleaseInstance_UserNotIssuer_ThrowsInvalidOperationException()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Taken;
        var user1 = _userDataFactory.CreateUser();
        var user2 = _userDataFactory.CreateUser();
        movableInstance.User = user1;
        user1.MovableInstances.Add(movableInstance);
        var newLocation = _locationDataFactory.CreateLocation();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => MovableInstanceStateManagementService.ReleaseInstance(movableInstance, user2, newLocation));
    }

    [Fact(DisplayName = "ReleaseInstance - Force Release Taken Instance - Instance is Released to New Location")]
    public void ReleaseInstance_ForceReleaseTaken_InstanceIsReleased()
    {
        // Arrange
        var movableInstance = _movableInstanceFactory.GetRandomMovableInstance();
        movableInstance.Status = MovableInstanceStatus.Taken;
        var user1 = _userDataFactory.CreateUser();
        var user2 = _userDataFactory.CreateUser();
        movableInstance.User = user1;
        user1.MovableInstances.Add(movableInstance);
        var newLocation = _locationDataFactory.CreateLocation();

        // Act
        MovableInstanceStateManagementService.ReleaseInstance(movableInstance, user2, newLocation, force: true);

        // Assert
        Assert.Equal(MovableInstanceStatus.Available, movableInstance.Status);
        Assert.Contains(movableInstance, newLocation.Instances);
        Assert.Equal(newLocation.Id, movableInstance.Location?.Id);
        Assert.DoesNotContain(movableInstance, user1.MovableInstances);
        Assert.DoesNotContain(movableInstance, user2.MovableInstances);
        Assert.Null(movableInstance.User);
    }

    #endregion
}