﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.EntityFrameworkCore.Query;

public class JsonQueryAdHocSqlServerTest : JsonQueryAdHocTestBase
{
    public JsonQueryAdHocSqlServerTest(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    protected override ITestStoreFactory TestStoreFactory
        => SqlServerTestStoreFactory.Instance;

    protected override void Seed29219(MyContext29219 ctx)
    {
        var entity1 = new MyEntity29219
        {
            Id = 1,
            Reference = new MyJsonEntity29219 { NonNullableScalar = 10, NullableScalar = 11 },
            Collection = new List<MyJsonEntity29219>
            {
                new MyJsonEntity29219 { NonNullableScalar = 100, NullableScalar = 101 },
                new MyJsonEntity29219 { NonNullableScalar = 200, NullableScalar = 201 },
                new MyJsonEntity29219 { NonNullableScalar = 300, NullableScalar = null },
            }
        };

        var entity2 = new MyEntity29219
        {
            Id = 2,
            Reference = new MyJsonEntity29219 { NonNullableScalar = 20, NullableScalar = null },
            Collection = new List<MyJsonEntity29219>
            {
                new MyJsonEntity29219 { NonNullableScalar = 1001, NullableScalar = null },
            }
        };

        ctx.Entities.AddRange(entity1, entity2);
        ctx.SaveChanges();

        ctx.Database.ExecuteSqlRaw(@"INSERT INTO [Entities] ([Id], [Reference], [Collection])
VALUES(3, N'{{ ""NonNullableScalar"" : 30 }}', N'[{{ ""NonNullableScalar"" : 10001 }}]')");
    }

    protected override void Seed30028(MyContext30028 ctx)
    {
        // complete
        ctx.Database.ExecuteSqlRaw(@"INSERT INTO [Entities] ([Id], [Json])
VALUES(
1,
N'{{""RootName"":""e1"",""Collection"":[{{""BranchName"":""e1 c1"",""Nested"":{{""LeafName"":""e1 c1 l""}}}},{{""BranchName"":""e1 c2"",""Nested"":{{""LeafName"":""e1 c2 l""}}}}],""OptionalReference"":{{""BranchName"":""e1 or"",""Nested"":{{""LeafName"":""e1 or l""}}}},""RequiredReference"":{{""BranchName"":""e1 rr"",""Nested"":{{""LeafName"":""e1 rr l""}}}}}}')");

        // missing collection
        ctx.Database.ExecuteSqlRaw(@"INSERT INTO [Entities] ([Id], [Json])
VALUES(
2,
N'{{""RootName"":""e2"",""OptionalReference"":{{""BranchName"":""e2 or"",""Nested"":{{""LeafName"":""e2 or l""}}}},""RequiredReference"":{{""BranchName"":""e2 rr"",""Nested"":{{""LeafName"":""e2 rr l""}}}}}}')");

        // missing optional reference
        ctx.Database.ExecuteSqlRaw(@"INSERT INTO [Entities] ([Id], [Json])
VALUES(
3,
N'{{""RootName"":""e3"",""Collection"":[{{""BranchName"":""e3 c1"",""Nested"":{{""LeafName"":""e3 c1 l""}}}},{{""BranchName"":""e3 c2"",""Nested"":{{""LeafName"":""e3 c2 l""}}}}],""RequiredReference"":{{""BranchName"":""e3 rr"",""Nested"":{{""LeafName"":""e3 rr l""}}}}}}')");

        // missing required reference
        ctx.Database.ExecuteSqlRaw(@"INSERT INTO [Entities] ([Id], [Json])
VALUES(
4,
N'{{""RootName"":""e4"",""Collection"":[{{""BranchName"":""e4 c1"",""Nested"":{{""LeafName"":""e4 c1 l""}}}},{{""BranchName"":""e4 c2"",""Nested"":{{""LeafName"":""e4 c2 l""}}}}],""OptionalReference"":{{""BranchName"":""e4 or"",""Nested"":{{""LeafName"":""e4 or l""}}}}}}')");
    }

    protected override void SeedArrayOfPrimitives(MyContextArrayOfPrimitives ctx)
    {
        var entity1 = new MyEntityArrayOfPrimitives
        {
            Id = 1,
            Reference = new MyJsonEntityArrayOfPrimitives
            {
                IntArray = new int[] { 1, 2, 3 },
                ListOfString = new List<string> { "Foo", "Bar", "Baz" }
            },
            Collection = new List<MyJsonEntityArrayOfPrimitives>
            {
                new MyJsonEntityArrayOfPrimitives
                {
                    IntArray = new int[] { 111, 112, 113 },
                    ListOfString = new List<string> { "Foo11", "Bar11" }
                },
                new MyJsonEntityArrayOfPrimitives
                {
                    IntArray = new int[] { 211, 212, 213 },
                    ListOfString = new List<string> { "Foo12", "Bar12" }
                },
            }
        };

        var entity2 = new MyEntityArrayOfPrimitives
        {
            Id = 2,
            Reference = new MyJsonEntityArrayOfPrimitives
            {
                IntArray = new int[] { 10, 20, 30 },
                ListOfString = new List<string> { "A", "B", "C" }
            },
            Collection = new List<MyJsonEntityArrayOfPrimitives>
            {
                new MyJsonEntityArrayOfPrimitives
                {
                    IntArray = new int[] { 110, 120, 130 },
                    ListOfString = new List<string> { "A1", "Z1" }
                },
                new MyJsonEntityArrayOfPrimitives
                {
                    IntArray = new int[] { 210, 220, 230 },
                    ListOfString = new List<string> { "A2", "Z2" }
                },
            }
        };

        ctx.Entities.AddRange(entity1, entity2);
        ctx.SaveChanges();
    }

    protected override void SeedJunkInJson(MyContextJunkInJson ctx)
    {



        ctx.Database.ExecuteSqlRaw(@"INSERT INTO [Entities] ([Collection], [CollectionWithCtor], [Reference], [ReferenceWithCtor], [Id])
VALUES(
N'[{{""Name"":""c11"",""JunkProperty1"":50,""Number"":11.5,""JunkCollection1"":[],""JunkCollection2"":[{{""Foo"":""junk value""}}],""NestedCollection"":[{{""DoB"":""2002-04-01T00:00:00""}},{{""DoB"":""2002-04-02T00:00:00""}}],""NestedReference"":{{""DoB"":""2002-03-01T00:00:00""}}}},{{""Name"":""c12"",""Number"":12.5,""NestedCollection"":[{{""DoB"":""2002-06-01T00:00:00""}},{{""DoB"":""2002-06-02T00:00:00""}}],""NestedReference"":{{""DoB"":""2002-05-01T00:00:00""}}}}]',
N'[{{""MyBool"":true,""Name"":""c11 ctor"",""NestedCollection"":[{{""DoB"":""2002-08-01T00:00:00""}},{{""DoB"":""2002-08-02T00:00:00""}}],""NestedReference"":{{""DoB"":""2002-07-01T00:00:00""}}}},{{""MyBool"":false,""Name"":""c12 ctor"",""NestedCollection"":[{{""DoB"":""2002-10-01T00:00:00""}},{{""DoB"":""2002-10-02T00:00:00""}}],""NestedReference"":{{""DoB"":""2002-09-01T00:00:00""}}}}]',
N'{{""Name"":""r1"",""Number"":1.5,""NestedCollection"":[{{""DoB"":""2000-02-01T00:00:00""}},{{""DoB"":""2000-02-02T00:00:00""}}],""NestedReference"":{{""DoB"":""2000-01-01T00:00:00""}}}}',
N'{{""MyBool"":true,""Name"":""r1 ctor"",""NestedCollection"":[{{""DoB"":""2001-02-01T00:00:00""}},{{""DoB"":""2001-02-02T00:00:00""}}],""NestedReference"":{{""DoB"":""2001-01-01T00:00:00""}}}}',
1)");

        //var entity1 = new MyEntityJunkInJson
        //{
        //    Id = 1,

        //    Reference = new MyJsonEntityJunkInJson
        //    {
        //        Name = "r1",
        //        Number = 1.5,
        //        NestedReference = new MyJsonEntityJunkInJsonNested { DoB = new DateTime(2000, 1, 1) },
        //        NestedCollection = new List<MyJsonEntityJunkInJsonNested>
        //        {
        //            new MyJsonEntityJunkInJsonNested { DoB = new DateTime(2000, 2, 1) },
        //            new MyJsonEntityJunkInJsonNested { DoB = new DateTime(2000, 2, 2) },
        //        }
        //    },

        //    ReferenceWithCtor = new MyJsonEntityJunkInJsonWithCtor(true, "r1 ctor")
        //    {
        //        NestedReference = new MyJsonEntityJunkInJsonWithCtorNested(new DateTime(2001, 1, 1)),
        //        NestedCollection = new List<MyJsonEntityJunkInJsonWithCtorNested>
        //        {
        //            new MyJsonEntityJunkInJsonWithCtorNested(new DateTime(2001, 2, 1)),
        //            new MyJsonEntityJunkInJsonWithCtorNested(new DateTime(2001, 2, 2)),
        //        }
        //    },

        //    Collection = new List<MyJsonEntityJunkInJson>
        //    {
        //        new MyJsonEntityJunkInJson
        //        {
        //            Name = "c11",
        //            Number = 11.5,
        //            NestedReference = new MyJsonEntityJunkInJsonNested { DoB = new DateTime(2002, 3, 1) },
        //            NestedCollection = new List<MyJsonEntityJunkInJsonNested>
        //            {
        //                new MyJsonEntityJunkInJsonNested { DoB = new DateTime(2002, 4, 1) },
        //                new MyJsonEntityJunkInJsonNested { DoB = new DateTime(2002, 4, 2) },
        //            }
        //        },
        //        new MyJsonEntityJunkInJson
        //        {
        //            Name = "c12",
        //            Number = 12.5,
        //            NestedReference = new MyJsonEntityJunkInJsonNested { DoB = new DateTime(2002, 5, 1) },
        //            NestedCollection = new List<MyJsonEntityJunkInJsonNested>
        //            {
        //                new MyJsonEntityJunkInJsonNested { DoB = new DateTime(2002, 6, 1) },
        //                new MyJsonEntityJunkInJsonNested { DoB = new DateTime(2002, 6, 2) },
        //            }
        //        },
        //    },

        //    CollectionWithCtor = new List<MyJsonEntityJunkInJsonWithCtor>
        //    {
        //        new MyJsonEntityJunkInJsonWithCtor(true, "c11 ctor")
        //        {
        //            NestedReference = new MyJsonEntityJunkInJsonWithCtorNested(new DateTime(2002, 7, 1)),
        //            NestedCollection = new List<MyJsonEntityJunkInJsonWithCtorNested>
        //            {
        //                new MyJsonEntityJunkInJsonWithCtorNested(new DateTime(2002, 8, 1)),
        //                new MyJsonEntityJunkInJsonWithCtorNested(new DateTime(2002, 8, 2)),
        //            }
        //        },
        //        new MyJsonEntityJunkInJsonWithCtor(false, "c12 ctor")
        //        {
        //            NestedReference = new MyJsonEntityJunkInJsonWithCtorNested(new DateTime(2002, 9, 1)),
        //            NestedCollection = new List<MyJsonEntityJunkInJsonWithCtorNested>
        //            {
        //                new MyJsonEntityJunkInJsonWithCtorNested(new DateTime(2002, 10, 1)),
        //                new MyJsonEntityJunkInJsonWithCtorNested(new DateTime(2002, 10, 2)),
        //            }
        //        },
        //    },
        //};

        //ctx.Entities.AddRange(entity1);
        //ctx.SaveChanges();
    }

    public override Task Junk_in_json_basic_tracking(bool async)
    {
        return base.Junk_in_json_basic_tracking(async);
    }
}
