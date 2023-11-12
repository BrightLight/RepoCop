namespace Silverseed.RepoCop.Tests
{
  using Moq;
  using NUnit.Framework;

  /// <summary>
  /// Unit tests for the <see cref="CapabilityCondition"/> class.
  /// </summary>
  [TestFixture(TestOf = typeof(CapabilityCondition))]
  public class CapabilityConditionTests
  {
    /// <summary>
    /// Tests that a capability condition is correctly initialized.
    /// </summary>
    /// <param name="capability">The capability to test.</param>
    [TestCase("MergeInfo")]
    public void InstanceIsCorrectlyInitialized(string capability)
    {
      var capabilityCondition = new CapabilityCondition { Capability = capability };
      Assert.That(capabilityCondition.Capability, Is.EqualTo(capability));
      Assert.That(capabilityCondition.State, Is.False);
    }

    /// <summary>
    /// Tests that the state of a capability condition is true if the capability is present.
    /// </summary>
    [Test]
    public void StateIsTrueIfCapabilityIsPresent()
    {
      // arrange
      var capabilityCondition = new CapabilityCondition { Capability = "MergeInfo" };
      var repoChangeInfo = Mock.Of<IRepoChangeInfo>(x => x.ClientCapabilities == new[] { "MergeInfo", "SuperFreeze" });
      Assert.That(capabilityCondition.State, Is.False);
      
      // act (setting RepoChangeInfo triggers all conditions to update their state)
      RepositoryInfoHub.Instance.RepoChangeInfo = repoChangeInfo;
      
      // assert
      Assert.That(capabilityCondition.State, Is.True);
    }
  }
}