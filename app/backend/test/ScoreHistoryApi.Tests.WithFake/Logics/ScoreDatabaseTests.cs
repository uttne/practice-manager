using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreHistoryApi.Factories;
using ScoreHistoryApi.Logics;
using ScoreHistoryApi.Models.Scores;
using Xunit;

namespace ScoreHistoryApi.Tests.WithFake.Logics
{
    public class ScoreDatabaseTests
    {
        [Fact]
        public async Task CreateAsyncTest()
        {
            var factory = new DynamoDbClientFactory().SetEndpointUrl(new Uri("http://localhost:18000"));
            var tableName = "ura-kata-score-history";
            var target = new ScoreDatabase(new ScoreQuota(), factory.Create(), tableName);

            var ownerId = Guid.Parse("f2240c15-0f2d-41ce-941d-6b173bae94c0");
            var scoreId = Guid.Parse("0d9fb491-43ae-44a1-9056-55bb25b21187");
            try
            {
                await target.InitializeAsync(ownerId);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }

            var title = "test score";
            var description = "楽譜の説明";

            await target.CreateAsync(ownerId, scoreId, title, description);
        }

        [Fact]
        public async Task UpdateTitleAsyncTest()
        {
            var factory = new DynamoDbClientFactory().SetEndpointUrl(new Uri("http://localhost:18000"));
            var tableName = "ura-kata-score-history";
            var target = new ScoreDatabase(new ScoreQuota(), factory.Create(), tableName);

            var ownerId = Guid.Parse("f2240c15-0f2d-41ce-941d-6b173bae94c0");
            var scoreId = Guid.Parse("0d9fb491-43ae-44a1-9056-55bb25b21187");

            var title = "test score";
            var description = "楽譜の説明";

            try
            {
                await target.InitializeAsync(ownerId);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }
            try
            {
                await target.CreateAsync(ownerId, scoreId, title, description);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }

            var newTitle = "new test score";
            await target.UpdateTitleAsync(ownerId, scoreId, newTitle);
        }

        [Fact]
        public async Task UpdateDescriptionAsyncTest()
        {
            var factory = new DynamoDbClientFactory().SetEndpointUrl(new Uri("http://localhost:18000"));
            var tableName = "ura-kata-score-history";
            var target = new ScoreDatabase(new ScoreQuota(), factory.Create(), tableName);

            var ownerId = Guid.Parse("f2240c15-0f2d-41ce-941d-6b173bae94c0");
            var scoreId = Guid.Parse("0d9fb491-43ae-44a1-9056-55bb25b21187");

            var title = "test score";
            var description = "楽譜の説明";

            try
            {
                await target.InitializeAsync(ownerId);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }
            try
            {
                await target.CreateAsync(ownerId, scoreId, title, description);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }

            var newDescription = "新しい楽譜の説明";
            await target.UpdateDescriptionAsync(ownerId, scoreId, newDescription);
        }

        [Fact]
        public async Task AddPagesAsyncTest()
        {
            var factory = new DynamoDbClientFactory().SetEndpointUrl(new Uri("http://localhost:18000"));
            var tableName = "ura-kata-score-history";
            var target = new ScoreDatabase(new ScoreQuota(), factory.Create(), tableName);

            var ownerId = Guid.Parse("f2240c15-0f2d-41ce-941d-6b173bae94c0");
            var scoreId = Guid.Parse("0d9fb491-43ae-44a1-9056-55bb25b21187");

            var title = "test score";
            var description = "楽譜の説明";

            try
            {
                await target.InitializeAsync(ownerId);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }
            try
            {
                await target.CreateAsync(ownerId, scoreId, title, description);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }

            var newPages = new List<NewScorePage>()
            {
                new NewScorePage()
                {
                    Page = "1",
                    ItemId = Guid.NewGuid(),
                },
                new NewScorePage()
                {
                    Page = "2",
                    ItemId = Guid.NewGuid(),
                }
            };
            await target.AddPagesAsync(ownerId, scoreId, newPages);
        }



        [Fact]
        public async Task RemovePagesAsyncTest()
        {
            var factory = new DynamoDbClientFactory().SetEndpointUrl(new Uri("http://localhost:18000"));
            var tableName = "ura-kata-score-history";
            var target = new ScoreDatabase(new ScoreQuota(), factory.Create(), tableName);

            var ownerId = Guid.Parse("f2240c15-0f2d-41ce-941d-6b173bae94c0");
            var scoreId = Guid.Parse("727679a2-c1eb-4089-9817-9a9bfb7a23b1");

            var title = "test score";
            var description = "楽譜の説明";

            try
            {
                await target.InitializeAsync(ownerId);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }
            try
            {
                await target.CreateAsync(ownerId, scoreId, title, description);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }

            var newPages = new List<NewScorePage>()
            {
                new NewScorePage()
                {
                    Page = "1",
                    ItemId = Guid.NewGuid(),
                },
                new NewScorePage()
                {
                    Page = "2",
                    ItemId = Guid.NewGuid(),
                },
                new NewScorePage()
                {
                    Page = "3",
                    ItemId = Guid.NewGuid(),
                },
                new NewScorePage()
                {
                    Page = "4",
                    ItemId = Guid.NewGuid(),
                },
                new NewScorePage()
                {
                    Page = "5",
                    ItemId = Guid.NewGuid(),
                }
            };

            try
            {
                await target.AddPagesAsync(ownerId, scoreId, newPages);
            }
            catch (Exception)
            {
                // 握りつぶす
            }

            var pageIds = new List<long>()
            {
                1,3
            };
            await target.RemovePagesAsync(ownerId, scoreId, pageIds);

        }


        [Fact]
        public async Task ReplacePagesAsyncTest()
        {
            var factory = new DynamoDbClientFactory().SetEndpointUrl(new Uri("http://localhost:18000"));
            var tableName = "ura-kata-score-history";
            var target = new ScoreDatabase(new ScoreQuota(), factory.Create(), tableName);

            var ownerId = Guid.Parse("f2240c15-0f2d-41ce-941d-6b173bae94c0");
            var scoreId = Guid.Parse("9db807b1-42fe-4e1a-b85c-61905b176616");

            var title = "test score";
            var description = "楽譜の説明";

            try
            {
                await target.InitializeAsync(ownerId);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }
            try
            {
                await target.CreateAsync(ownerId, scoreId, title, description);
            }
            catch
            {
                // 初期化のエラーは握りつぶす
            }

            var newPages = new List<NewScorePage>()
            {
                new NewScorePage()
                {
                    Page = "1",
                    ItemId = Guid.NewGuid(),
                },
                new NewScorePage()
                {
                    Page = "2",
                    ItemId = Guid.NewGuid(),
                },
                new NewScorePage()
                {
                    Page = "3",
                    ItemId = Guid.NewGuid(),
                },
                new NewScorePage()
                {
                    Page = "4",
                    ItemId = Guid.NewGuid(),
                },
                new NewScorePage()
                {
                    Page = "5",
                    ItemId = Guid.NewGuid(),
                }
            };

            try
            {
                await target.AddPagesAsync(ownerId, scoreId, newPages);
            }
            catch (Exception)
            {
                // 握りつぶす
            }

            var pages = new List<PatchScorePage>()
            {
                new PatchScorePage()
                {
                    TargetPageId = 1,
                    Page = "replaced " + Guid.NewGuid(),
                    ItemId = Guid.NewGuid()
                },
                new PatchScorePage()
                {
                    TargetPageId = 3,
                    Page = "replaced " + Guid.NewGuid(),
                    ItemId = Guid.NewGuid()
                },
            };
            await target.ReplacePagesAsync(ownerId, scoreId, pages);

        }
    }
}