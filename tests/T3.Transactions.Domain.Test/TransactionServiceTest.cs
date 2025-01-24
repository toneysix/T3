namespace T3.Transactions.Domain.Test;

using NSubstitute;
using NSubstitute.ReturnsExtensions;
using T3.Domain.Shared.Errors;
using T3.Domain.Shared.Specs;
using T3.Transactions.Core.Entities;
using T3.Transactions.Core.Repositories;
using T3.Transactions.Core.Services;
using DomainErrors = Core.Errors.Errors;

[TestFixture]
public class TransactionServiceTest
{
    private ITransactionService transactionService;

    private ITransactionsRepository repositoryMock;

    [SetUp]
    public void SetUp()
    {
        this.repositoryMock = Substitute.For<ITransactionsRepository>();
        this.transactionService = new TransactionService(repositoryMock);
    }

    [Test]
    public void GetBy_Should_Return_Transaction_When_Transaction_WithSpecificId_Exists()
    {
        var transactionInStore = this.GenerateTransaction();

        this.repositoryMock
            .Get(new EntityByGuidSpec<Transaction>() { Id = transactionInStore.Id })
            .Returns(transactionInStore);

        var transactionResult = this.transactionService.GetBy(transactionInStore.Id);

        Assert.True(transactionResult.IsSuccess);
        Assert.That(Error.None, Is.EqualTo(transactionResult.Error)); 
        Assert.That(transactionInStore, Is.EqualTo(transactionResult.Value));
        Assert.DoesNotThrow(() => new Func<Transaction>(() => transactionResult.Value).Invoke());
    }

    [Test]
    [TestCaseSource(nameof(Guids))]
    public void GetBy_Should_Return_ErrorNotFound_When_Transaction_WithSpecificId_DoesntExist(GuidTestCase testCase)
    {
        var id = testCase.DataProvider.Invoke();
        var transactionInStore = this.GenerateTransaction(id);

        this.repositoryMock
            .Get(new EntityByGuidSpec<Transaction>() { Id = id })
            .Returns(transactionInStore);

        var transactionResult = this.transactionService.GetBy(Guid.NewGuid());
        Assert.True(transactionResult.IsFailure);
        Assert.That(Errors.Entities.NotFound<Transaction>(), Is.EqualTo(transactionResult.Error));
        Assert.Throws<InvalidOperationException>(() => new Func<Transaction>(() => transactionResult.Value).Invoke());
    }

    [Test]
    [TestCase(100)]
    [TestCase(101)]
    public void Commit_Should_Return_ErrorMaxTransactionsExceeded_When_TransactionCount_GreaterOrEquals_100(long count)
    {
        this.repositoryMock
            .CountAll()
            .Returns(count);

        var transactionResult = this.transactionService.Commit(this.GenerateTransaction());

        Assert.True(transactionResult.IsFailure);
        Assert.That(DomainErrors.Transaction.MaxTransactionsExceeded, Is.EqualTo(transactionResult.Error));
    }

    [Test]
    [TestCase(0)]
    [TestCase(99)]
    [TestCase(50)]
    public void Commit_Should_Return_SuccessResult_When_TransactionCount_Less_100(long count)
    {
        this.repositoryMock
            .CountAll()
            .Returns(count);

        var transactionResult = this.transactionService.Commit(this.GenerateTransaction());

        Assert.True(transactionResult.IsSuccess);
        Assert.That(Error.None, Is.EqualTo(transactionResult.Error));
    }

    [Test]
    [TestCaseSource(nameof(DateTimesInFuture))]
    public void Commit_Should_Return_ErrorDateTimeIsInFuture_When_TransactionDateTime_IsInFuture(
        DateTimeTestCase testCase)
    {
        var transactionResult = this.transactionService.Commit(
            this.GenerateTransaction(dateTime: testCase.DataProvider.Invoke()));

        Assert.True(transactionResult.IsFailure);
        Assert.That(DomainErrors.Transaction.DateTimeIsInFuture, Is.EqualTo(transactionResult.Error));
    }

    [TestCaseSource(nameof(DateTimesNotInFuture))]
    public void Commit_Should_Return_SuccessResult_When_TransactionDateTime_IsNotInFuture(DateTimeTestCase testCase)
    {
        var transactionResult = this.transactionService.Commit(
            this.GenerateTransaction(dateTime: testCase.DataProvider.Invoke()));

        Assert.True(transactionResult.IsSuccess);
        Assert.That(Error.None, Is.EqualTo(transactionResult.Error));
    }

    [Test]
    [TestCase(0d)]
    [TestCase(-1d)]
    [TestCase(-10d)]
    public void Commit_Should_Return_ErrorAmountIsNotPositive_When_TransactionAmount_IsNotPositive(decimal amount)
    {
        var transactionResult = this.transactionService.Commit(
            this.GenerateTransaction(amount: amount));

        Assert.True(transactionResult.IsFailure);
        Assert.That(DomainErrors.Transaction.AmountIsNotPositive, Is.EqualTo(transactionResult.Error));
    }

    [Test]
    [TestCase(1d)]
    [TestCase(5d)]
    [TestCase(10d)]
    public void Commit_Should_Return_SuccessResult_When_TransactionAmount_IsPositive(decimal amount)
    {
        var transactionResult = this.transactionService.Commit(
            this.GenerateTransaction(amount: amount));

        Assert.True(transactionResult.IsSuccess);
        Assert.That(Error.None, Is.EqualTo(transactionResult.Error));
    }

    [Test]
    public void Commit_Should_Return_ErrorAlreadyExists_When_Transaction_AlreadyExists()
    {
        var transactionInStore = this.GenerateTransaction();

        this.repositoryMock
            .Get(new EntityByGuidSpec<Transaction>() { Id = transactionInStore.Id })
            .Returns(transactionInStore);

        var transactionResult = this.transactionService.Commit(transactionInStore);

        Assert.True(transactionResult.IsFailure);
        Assert.That(Errors.Entities.AlreadyExists<Transaction>(), Is.EqualTo(transactionResult.Error));
    }

    [Test]
    public void Commit_Should_Return_SuccessResult_When_Transaction_DoesNotExists()
    {
        var newTransaction = this.GenerateTransaction();

        this.repositoryMock
            .Get(Arg.Any<EntityByGuidSpec<Transaction>>())
            .ReturnsNull();

        var transactionResult = this.transactionService.Commit(newTransaction);

        Assert.True(transactionResult.IsSuccess);
        Assert.That(Error.None, Is.EqualTo(transactionResult.Error));
    }

    private Transaction GenerateTransaction(
        Guid? id = null, 
        DateTime? dateTime = null,
        decimal? amount = null)
    {
        return new Transaction()
        {
            Id = id ?? Guid.NewGuid(),
            Date = dateTime ?? DateTime.Now,
            Amount = amount ?? 1,
        };
    }

    public class DateTimeTestCase
    {
        public required Func<DateTime> DataProvider { get; init; }
    }

    public class GuidTestCase
    {
        public required Func<Guid> DataProvider { get; init; }
    }

    private static IEnumerable<GuidTestCase> Guids()
    {
        yield return new GuidTestCase() { DataProvider = () => Guid.NewGuid() };
        yield return new GuidTestCase() { DataProvider = () => Guid.Empty };
    }

    private static IEnumerable<DateTimeTestCase> DateTimesInFuture()
    {
        yield return new DateTimeTestCase() { DataProvider = () => DateTime.Now.AddDays(1) };
        yield return new DateTimeTestCase() { DataProvider = () => DateTime.Now.AddHours(1) };
    }

    private static IEnumerable<DateTimeTestCase> DateTimesNotInFuture()
    {
        yield return new DateTimeTestCase() { DataProvider = () => DateTime.Now.AddDays(-1) };
        yield return new DateTimeTestCase() { DataProvider = () => DateTime.Now.AddHours(-1) };
        yield return new DateTimeTestCase() { DataProvider = () => DateTime.Now };
    }
}
