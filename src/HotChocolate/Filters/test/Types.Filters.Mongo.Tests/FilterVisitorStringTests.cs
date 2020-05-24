using HotChocolate.Language;
using HotChocolate.Types.Filters.Mongo;
using HotChocolate.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;
using Squadron;
using Xunit;

namespace HotChocolate.Types.Filters
{
    public class FilterVisitorStringTests
        : TypeTestBase, IClassFixture<MongoResource>
    {
        private readonly MongoResource _mongoResource;

        public FilterVisitorStringTests(MongoResource mongoResource)
        {
            _mongoResource = mongoResource;
        }

        [Fact]
        public void Create_StringEqual_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar",
                    new StringValueNode("a")));

            FooFilterType fooType = CreateType(new FooFilterType());

            IMongoDatabase database = _mongoResource.CreateDatabase();
            IMongoCollection<Foo> collectionA = database.GetCollection<Foo>("a");
            IMongoCollection<Foo> collectionB = database.GetCollection<Foo>("b");

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default);

            FilterVisitor<FilterDefinition<BsonDocument>>.Default.Visit(value, filterContext);
            filterContext.TryCreateQuery(out BsonDocument query);

            // assert 
            collectionA.InsertOne(new Foo { Bar = "a" });
            Assert.True(collectionA.Find(query).Any());

            collectionB.InsertOne(new Foo { Bar = "b" });
            Assert.False(collectionB.Find(query).Any());
        }
        /*
        [Fact]
        public void Create_StringNotEqual_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar_not",
                    new StringValueNode("a")));

            FooFilterType fooType = CreateType(new FooFilterType());

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default,
                true);
            FilterVisitor<Expression>.Default.Visit(value, filterContext);
            Func<Foo, bool> func = filterContext.CreateOrAssert<Foo>().Compile();

            // assert
            var a = new Foo { Bar = "a" };
            Assert.False(func(a));

            var b = new Foo { Bar = "b" };
            Assert.True(func(b));
        }

        [Fact]
        public void Create_StringIn_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar_in",
                    new ListValueNode(new[]
                    {
                        new StringValueNode("a"),
                        new StringValueNode("c")
                    })));

            FooFilterType fooType = CreateType(new FooFilterType());

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default,
                true);
            FilterVisitor<Expression>.Default.Visit(value, filterContext);
            Func<Foo, bool> func = filterContext.CreateOrAssert<Foo>().Compile();

            // assert
            var a = new Foo { Bar = "a" };
            Assert.True(func(a));

            var b = new Foo { Bar = "b" };
            Assert.False(func(b));
        }

        [Fact]
        public void Create_StringIn_SingleValue_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar_in",
                    new StringValueNode("a")));

            FooFilterType fooType = CreateType(new FooFilterType());

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default,
                true);
            FilterVisitor<Expression>.Default.Visit(value, filterContext);
            Func<Foo, bool> func = filterContext.CreateOrAssert<Foo>().Compile();

            // assert
            var a = new Foo { Bar = "a" };
            Assert.True(func(a));

            var b = new Foo { Bar = "b" };
            Assert.False(func(b));
        }

        [Fact]
        public void Create_StringNotIn_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar_not_in",
                    new ListValueNode(new[]
                    {
                        new StringValueNode("a"),
                        new StringValueNode("c")
                    })));

            FooFilterType fooType = CreateType(new FooFilterType());

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default,
                true);
            FilterVisitor<Expression>.Default.Visit(value, filterContext);
            Func<Foo, bool> func = filterContext.CreateOrAssert<Foo>().Compile();

            // assert
            var a = new Foo { Bar = "a" };
            Assert.False(func(a));

            var b = new Foo { Bar = "b" };
            Assert.True(func(b));
        }

        [Fact]
        public void Create_StringContains_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar_contains",
                    new StringValueNode("a")));

            FooFilterType fooType = CreateType(new FooFilterType());

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default,
                true);
            FilterVisitor<Expression>.Default.Visit(value, filterContext);
            Func<Foo, bool> func = filterContext.CreateOrAssert<Foo>().Compile();

            // assert
            var a = new Foo { Bar = "testatest" };
            Assert.True(func(a));

            var b = new Foo { Bar = "testbtest" };
            Assert.False(func(b));
        }

        [Fact]
        public void Create_StringNoContains_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar_not_contains",
                    new StringValueNode("a")));

            FooFilterType fooType = CreateType(new FooFilterType());

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default,
                true);
            FilterVisitor<Expression>.Default.Visit(value, filterContext);
            Func<Foo, bool> func = filterContext.CreateOrAssert<Foo>().Compile();

            // assert
            var a = new Foo { Bar = "testatest" };
            Assert.False(func(a));

            var b = new Foo { Bar = "testbtest" };
            Assert.True(func(b));
        }

        [Fact]
        public void Create_StringStartsWith_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar_starts_with",
                    new StringValueNode("a")));

            FooFilterType fooType = CreateType(new FooFilterType());

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default,
                true);
            FilterVisitor<Expression>.Default.Visit(value, filterContext);
            Func<Foo, bool> func = filterContext.CreateOrAssert<Foo>().Compile();

            // assert
            var a = new Foo { Bar = "ab" };
            Assert.True(func(a));

            var b = new Foo { Bar = "ba" };
            Assert.False(func(b));
        }

        [Fact]
        public void Create_StringNotStartsWith_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar_not_starts_with",
                    new StringValueNode("a")));

            FooFilterType fooType = CreateType(new FooFilterType());

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default,
                true);
            FilterVisitor<Expression>.Default.Visit(value, filterContext);
            Func<Foo, bool> func = filterContext.CreateOrAssert<Foo>().Compile();

            // assert
            var a = new Foo { Bar = "ab" };
            Assert.False(func(a));

            var b = new Foo { Bar = "ba" };
            Assert.True(func(b));
        }

        [Fact]
        public void Create_StringEndsWith_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar_ends_with",
                    new StringValueNode("a")));

            FooFilterType fooType = CreateType(new FooFilterType());

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default,
                true);
            FilterVisitor<Expression>.Default.Visit(value, filterContext);
            Func<Foo, bool> func = filterContext.CreateOrAssert<Foo>().Compile();

            // assert
            var a = new Foo { Bar = "ab" };
            Assert.False(func(a));

            var b = new Foo { Bar = "ba" };
            Assert.True(func(b));
        }

        [Fact]
        public void Create_StringNotEndsWith_Expression()
        {
            // arrange
            var value = new ObjectValueNode(
                new ObjectFieldNode("bar_not_ends_with",
                    new StringValueNode("a")));

            FooFilterType fooType = CreateType(new FooFilterType());

            // act
            var filterContext = new MongoFilterVisitorContext(
                fooType,
                MockFilterConvention.Default.GetExpressionDefinition(),
                TypeConversion.Default,
                true);
            FilterVisitor<Expression>.Default.Visit(value, filterContext);
            Func<Foo, bool> func = filterContext.CreateOrAssert<Foo>().Compile();

            // assert
            var a = new Foo { Bar = "ab" };
            Assert.True(func(a));

            var b = new Foo { Bar = "ba" };
            Assert.False(func(b));
        }
        */

        public class Foo
        {
            public string Bar { get; set; }
        }

        public class FooFilterType
            : FilterInputType<Foo>
        {
            protected override void Configure(
                IFilterInputTypeDescriptor<Foo> descriptor)
            {
                descriptor.Filter(t => t.Bar)
                    .BindFiltersImplicitly();
            }
        }
    }
}
