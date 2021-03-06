namespace PracticeManagerApi.Services.Providers
{
    public interface IScoreProvider
    {
        string UserName { get; }
        ScoreV2Latest CreateScore(string owner, string scoreName, InitialScoreV2Property property);

        ScoreV2Latest GetScore(string owner, string scoreName);
        ScoreV2LatestSet GetScores(string owner);
        ScoreV2LatestSet GetScores();

        ScoreV2Latest UpdateProperty(string owner, string scoreName, string parentObjectHash,
            PatchScoreV2Property property);

        void DeleteScore(string owner, string scoreName);
        
        void InsertPages(string owner, string scoreName, string parentVersionHash, int index, NewScoreV2Page[] pages);
        void DeletePages(string owner, string scoreName, string parentVersionHash, string[] hashList);
        void UpdatePages(string owner, string scoreName, string parentVersionHash, PatchScoreV2Page[] pages);

        void AddComments(string owner, string scoreName, string parentVersionHash, NewScoreV2Comment[] comments);
        void DeleteComments(string owner, string scoreName, string parentVersionHash,
            string targetPage, string[] hashList);
        void UpdateComments(string owner, string scoreName, string parentVersionHash, PatchScoreV2Comment[] comments);

        string GetObject(string owner, string scoreName, string hash);

        ScoreV2Version CreateVersionRef(string owner, string scoreName);

        ScoreV2VersionSet GetVersions(string owner, string scoreName);

        ScoreV2Latest Commit(string owner, string scoreName, string parentVersionHash,
            CommitObject[] commitObjects);
    }
}
