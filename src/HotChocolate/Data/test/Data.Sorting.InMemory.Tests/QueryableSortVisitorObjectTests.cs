using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.Execution;
using HotChocolate.Tests;
using Xunit;

namespace HotChocolate.Data.Sorting.Expressions;

public class QueryableSortVisitorObjectTests : IClassFixture<SchemaCache>
{
    private static readonly Bar[] _barEntities =
    {
        new()
        {
            Foo = new Foo
            {
                BarShort = 12,
                BarBool = true,
                BarEnum = BarEnum.BAR,
                BarString = "testatest",
                //ScalarArray = new[] { "c", "d", "a" },
                ObjectArray = new List<Bar>
                {
                    new()
                    {
                        Foo = new Foo
                        {
                            // ScalarArray = new[] { "c", "d", "a" }
                            BarShort = 12, BarString = "a"
                        }
                    }
                }
            }
        },
        new()
        {
            Foo = new Foo
            {
                BarShort = 14,
                BarBool = true,
                BarEnum = BarEnum.BAZ,
                BarString = "testbtest",
                //ScalarArray = new[] { "c", "d", "b" },
                ObjectArray = new List<Bar>
                {
                    new()
                    {
                        Foo = new Foo
                        {
                            //ScalarArray = new[] { "c", "d", "b" }
                            BarShort = 14, BarString = "d"
                        }
                    }
                }
            }
        },
        new()
        {
            Foo = new Foo
            {
                BarShort = 13,
                BarBool = false,
                BarEnum = BarEnum.FOO,
                BarString = "testctest",
                //ScalarArray = null,
                ObjectArray = null,
            }
        }
    };

    private static readonly BarNullable?[] _barNullableEntities =
    {
        new()
        {
            Foo = new FooNullable
            {
                BarShort = 12,
                BarBool = true,
                BarEnum = BarEnum.BAR,
                BarString = "testatest",
                //ScalarArray = new[] { "c", "d", "a" },
                ObjectArray = new List<BarNullable>
                {
                    new()
                    {
                        Foo = new FooNullable
                        {
                            //ScalarArray = new[] { "c", "d", "a" }
                            BarShort = 12,
                        }
                    }
                }
            }
        },
        new()
        {
            Foo = new FooNullable
            {
                BarShort = null,
                BarBool = null,
                BarEnum = BarEnum.BAZ,
                BarString = "testbtest",
                //ScalarArray = new[] { "c", "d", "b" },
                ObjectArray = new List<BarNullable>
                {
                    new()
                    {
                        Foo = new FooNullable
                        {
                            //ScalarArray = new[] { "c", "d", "b" }
                            BarShort = null,
                        }
                    }
                }
            }
        },
        new()
        {
            Foo = new FooNullable
            {
                BarShort = 14,
                BarBool = false,
                BarEnum = BarEnum.QUX,
                BarString = "testctest",
                //ScalarArray = null,
                ObjectArray = new List<BarNullable>
                {
                    new()
                    {
                        Foo = new FooNullable
                        {
                            //ScalarArray = new[] { "c", "d", "b" }
                            BarShort = 14,
                        }
                    }
                }
            }
        },
        new()
        {
            Foo = new FooNullable
            {
                BarShort = 13,
                BarBool = false,
                BarEnum = BarEnum.FOO,
                BarString = "testdtest",
                //ScalarArray = null,
                ObjectArray = null
            }
        },
        new()
        {
            Foo =null
        },
        null
    };

    private readonly SchemaCache _cache;

    public QueryableSortVisitorObjectTests(SchemaCache cache)
    {
        _cache = cache;
    }

    [Fact]
    public async Task Create_ObjectShort_OrderBy()
    {
        // arrange
        IRequestExecutor tester = _cache.CreateSchema<Bar, BarSortType>(_barEntities);

        // act
        // assert
        IExecutionResult res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barShort: ASC}}) " +
                    "{ foo{ barShort}}}")
                .Create());

        res1.MatchSnapshot("ASC");

        IExecutionResult res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barShort: DESC}}) " +
                    "{ foo{ barShort}}}")
                .Create());

        res2.MatchSnapshot("DESC");
    }

    [Fact]
    public async Task Create_ObjectNullableShort_OrderBy()
    {
        // arrange
        IRequestExecutor? tester =
            _cache.CreateSchema<BarNullable, BarNullableSortType>(_barNullableEntities);

        // act
        // assert
        IExecutionResult res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barShort: ASC}}) " +
                    "{ foo{ barShort}}}")
                .Create());

        res1.MatchSnapshot("ASC");

        IExecutionResult res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barShort: DESC}}) " +
                    "{ foo{ barShort}}}")
                .Create());

        res2.MatchSnapshot("13");
    }

    [Fact]
    public async Task Create_ObjectEnum_OrderBy()
    {
        // arrange
        IRequestExecutor tester = _cache.CreateSchema<Bar, BarSortType>(_barEntities);

        // act
        // assert
        IExecutionResult res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barEnum: ASC}}) " +
                    "{ foo{ barEnum}}}")
                .Create());

        res1.MatchSnapshot("ASC");

        IExecutionResult res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barEnum: DESC}}) " +
                    "{ foo{ barEnum}}}")
                .Create());

        res2.MatchSnapshot("DESC");
    }

    [Fact]
    public async Task Create_ObjectNullableEnum_OrderBy()
    {
        // arrange
        IRequestExecutor tester =
            _cache.CreateSchema<BarNullable, BarNullableSortType>(_barNullableEntities);

        // act
        // assert
        IExecutionResult res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barEnum: ASC}}) " +
                    "{ foo{ barEnum}}}")
                .Create());

        res1.MatchSnapshot("ASC");

        IExecutionResult res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barEnum: DESC}}) " +
                    "{ foo{ barEnum}}}")
                .Create());

        res2.MatchSnapshot("13");
    }

    [Fact]
    public async Task Create_ObjectString_OrderBy()
    {
        // arrange
        IRequestExecutor tester = _cache.CreateSchema<Bar, BarSortType>(_barEntities);

        // act
        // assert
        IExecutionResult res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barString: ASC}}) " +
                    "{ foo{ barString}}}")
                .Create());

        res1.MatchSnapshot("ASC");

        IExecutionResult res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barString: DESC}}) " +
                    "{ foo{ barString}}}")
                .Create());

        res2.MatchSnapshot("DESC");
    }

    [Fact]
    public async Task Create_ObjectNullableString_OrderBy()
    {
        // arrange
        IRequestExecutor tester =
            _cache.CreateSchema<BarNullable, BarNullableSortType>(_barNullableEntities);

        // act
        // assert
        IExecutionResult res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barString: ASC}}) " +
                    "{ foo{ barString}}}")
                .Create());

        res1.MatchSnapshot("ASC");

        IExecutionResult res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barString: DESC}}) " +
                    "{ foo{ barString}}}")
                .Create());

        res2.MatchSnapshot("13");
    }

    [Fact]
    public async Task Create_ObjectBool_OrderBy()
    {
        // arrange
        IRequestExecutor tester = _cache.CreateSchema<Bar, BarSortType>(_barEntities);

        // act
        // assert
        IExecutionResult res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barBool: ASC}}) " +
                    "{ foo{ barBool}}}")
                .Create());

        res1.MatchSnapshot("ASC");

        IExecutionResult res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barBool: DESC}}) " +
                    "{ foo{ barBool}}}")
                .Create());

        res2.MatchSnapshot("DESC");
    }

    [Fact]
    public async Task Create_ObjectNullableBool_OrderBy()
    {
        // arrange
        IRequestExecutor tester =
            _cache.CreateSchema<BarNullable, BarNullableSortType>(_barNullableEntities);

        // act
        // assert
        IExecutionResult res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barBool: ASC}}) " +
                    "{ foo{ barBool}}}")
                .Create());

        res1.MatchSnapshot("ASC");

        IExecutionResult res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barBool: DESC}}) " +
                    "{ foo{ barBool}}}")
                .Create());

        res2.MatchSnapshot("13");
    }

    [Fact]
    public async Task Create_ObjectString_OrderBy_TwoProperties()
    {
        // arrange
        IRequestExecutor tester = _cache.CreateSchema<Bar, BarSortType>(_barEntities);

        // act
        // assert
        IExecutionResult res1 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barBool: ASC, barShort: ASC }}) " +
                    "{ foo{ barBool barShort}}}")
                .Create());

        res1.MatchSnapshot("ASC");

        IExecutionResult res2 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    @"
                        {
                            root(order: [
                                { foo: { barBool: ASC } },
                                { foo: { barShort: ASC } }]) {
                                foo {
                                    barBool
                                    barShort
                                }
                            }
                        }
                        ")
                .Create());

        res2.MatchSnapshot("ASC");

        IExecutionResult res3 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    "{ root(order: { foo: { barBool: DESC, barShort: DESC}}) " +
                    "{ foo{ barBool barShort}}}")
                .Create());

        res3.MatchSnapshot("DESC");

        IExecutionResult res4 = await tester.ExecuteAsync(
            QueryRequestBuilder.New()
                .SetQuery(
                    @"{
                        root(order: [
                            { foo: { barBool: DESC } },
                            { foo: { barShort: DESC } }]) {
                            foo {
                                barBool
                                barShort
                            }
                        }
                    }")
                .Create());

        res4.MatchSnapshot("DESC");
    }

    public class Foo
    {
        public int Id { get; set; }

        public short BarShort { get; set; }

        public string BarString { get; set; } = "";

        public BarEnum BarEnum { get; set; }

        public bool BarBool { get; set; }

        //Not supported in SQL
        //public string[] ScalarArray { get; set; }

        public List<Bar>? ObjectArray { get; set; } = new();
    }

    public class FooNullable
    {
        public int Id { get; set; }

        public short? BarShort { get; set; }

        public string? BarString { get; set; }

        public BarEnum? BarEnum { get; set; }

        public bool? BarBool { get; set; }

        //Not supported in SQL
        //public string?[] ScalarArray { get; set; }

        public List<BarNullable>? ObjectArray { get; set; }
    }

    public class Bar
    {
        public int Id { get; set; }

        public Foo Foo { get; set; } = null!;
    }

    public class BarNullable
    {
        public int Id { get; set; }

        public FooNullable? Foo { get; set; }
    }

    public class BarSortType : SortInputType<Bar>
    {
    }

    public class BarNullableSortType : SortInputType<BarNullable>
    {
    }

    public enum BarEnum
    {
        FOO,
        BAR,
        BAZ,
        QUX
    }
}
