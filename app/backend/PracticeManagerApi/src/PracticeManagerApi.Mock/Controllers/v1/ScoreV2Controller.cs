using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PracticeManagerApi.Services.Models;
using PracticeManagerApi.Services.Providers;
using PracticeManagerApi.Services.Storage;

namespace PracticeManagerApi.Mock.Controllers.v1
{
    [Route("api/v1/score_v2")]
    public class ScoreV2Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ScoreProvider _scoreProvider;

        public ScoreV2Controller(IConfiguration configuration, ILogger<ScoreV2Controller> logger, IScoreStorage scoreStorage)
        {
            _configuration = configuration;
            this._logger = logger;

            RequestUserName = "test";
            _scoreProvider = new ScoreProvider(DateTimeOffset.Now, scoreStorage, RequestUserName);
        }

        public string RequestUserName { get; }

        [HttpGet]
        public ScoreV2LatestSet GetScoreSet()
        {
            try
            {
                return _scoreProvider.GetScores();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new InvalidOperationException("楽譜を取得できませんでした", ex);
            }
        }

        [HttpGet]
        [Route("{owner}")]
        public ScoreV2LatestSet GetScoreNameListWithOwner(
            [FromRoute(Name = "owner")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string owner,
            [FromQuery(Name = "q")]
            string q)
        {
            try
            {
                return _scoreProvider.GetScores(owner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new InvalidOperationException("楽譜を取得できませんでした", ex);
            }
        }

        [HttpPost]
        [Route("{owner}/{score_name}")]
        public ActionResult <ScoreV2Latest> CreateScoreWithOwner(
            [FromRoute(Name = "owner")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string owner,
            [FromRoute(Name = "score_name")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string scoreName,
            [FromBody]
            [Required]
            InitialScoreV2Property property)
        {
            if (null != property.Title && !( property.Title.Length <= 128))
            {
                return BadRequest();
            }
            if (property.Description != null && !(property.Description.Length <= 1024))
            {
                return BadRequest();
            }
            
            try
            {
                return _scoreProvider.CreateScore(owner, scoreName,property);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("作成に失敗しました");
            }
        }

        [HttpDelete]
        [Route("{owner}/{score_name}")]
        public IActionResult DeleteScoreWithOwner(
            [FromRoute(Name = "owner")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string owner,
            [FromRoute(Name = "score_name")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string scoreName)
        {
            try
            {
                _scoreProvider.DeleteScore(owner, scoreName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("削除に失敗しました");
            }

            return Ok();
        }

        [HttpPatch]
        [Route("{owner}/{score_name}")]
        public ActionResult<ScoreV2Latest> UpdateScoreWithOwner(
            [FromRoute(Name = "owner")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string owner,
            [FromRoute(Name = "score_name")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string scoreName,
            [FromBody]
            [Required]
            UpdateScoreWithOwner body)
        {
            try
            {
                return _scoreProvider.UpdateProperty(owner, scoreName, body.Parent, body.Property);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidComObjectException("プロパティの更新に失敗しました", ex);
            }
        }

        [HttpGet]
        [Route("{owner}/{score_name}")]
        public ScoreV2Latest GetScoreWithOwner(
            [FromRoute(Name = "owner")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string owner,
            [FromRoute(Name = "score_name")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string scoreName)
        {
            try
            {
                return _scoreProvider.GetScore(owner, scoreName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("楽譜の取得に失敗しました", ex);
            }
        }



        [HttpGet]
        [Route("{owner}/{score_name}/object/{hash}")]
        public string GetScoreWithOwnerObjects(
            [FromRoute(Name = "owner")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string owner,
            [FromRoute(Name = "score_name")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string scoreName,
            [FromRoute(Name = "hash")]
            [MaxLength(40, ErrorMessage = "{0} は 40 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-z0-9]+$",ErrorMessage = "{0} は 半角英数字の小文字が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string hash)
        {

            try
            {
                return _scoreProvider.GetObject(owner, scoreName, hash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("Hash Object の取得に失敗しました", ex);
            }
        }



        [HttpPost]
        [Route("{owner}/{score_name}/commit")]
        public ScoreV2Latest CommitScoreWithOwner(
            [FromRoute(Name = "owner")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string owner,
            [FromRoute(Name = "score_name")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string scoreName,
            [FromBody]
            CommitRequest request)
        {

            try
            {
                var response = _scoreProvider.Commit(owner, scoreName, request.Parent, request.Commits);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("Hash Object の取得に失敗しました", ex);
            }
        }



        [HttpPost]
        [Route("{owner}/{score_name}/version")]
        public ScoreV2Version CreateVersionWithOwner(
            [FromRoute(Name = "owner")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string owner,
            [FromRoute(Name = "score_name")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string scoreName)
        {

            try
            {
                var response = _scoreProvider.CreateVersionRef(owner, scoreName);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("Version の作成に失敗しました", ex);
            }
        }





        [HttpGet]
        [Route("{owner}/{score_name}/version")]
        public ScoreV2VersionSet GetVersionsWithOwner(
            [FromRoute(Name = "owner")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string owner,
            [FromRoute(Name = "score_name")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string scoreName)
        {

            try
            {
                var response = _scoreProvider.GetVersions(owner, scoreName);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("Version のリストの取得に失敗しました", ex);
            }
        }


        [HttpPatch]
        [Route("{owner}/{score_name}/property")]
        public ScoreV2Latest PatchPropertyWithOwner(
            [FromRoute(Name = "owner")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string owner,
            [FromRoute(Name = "score_name")]
            [MaxLength(128, ErrorMessage = "{0} は 128 文字以内です")]
            [MinLength(1, ErrorMessage = "{0} は 1 文字以上です")]
            [RegularExpression(@"^[a-zA-Z0-9\-_]+$",ErrorMessage = "{0} は 半角英数字 , - , _ が使用できます", MatchTimeoutInMilliseconds = 1000)]
            string scoreName,
            [FromBody]
            UpdatePropertyRequest request)
        {

            try
            {
                var response = _scoreProvider.UpdateProperty(owner, scoreName, request?.Parent, request?.Property);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new InvalidOperationException("Version の作成に失敗しました", ex);
            }
        }
    }
}
