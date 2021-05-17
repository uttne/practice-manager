namespace ScoreHistoryApi.Logics.ScoreDatabases
{
    public static class ScoreDatabasePropertyNames
    {
        public const string OwnerId = "owner";
        public const string ScoreId = "score";
        public const string DataHash = "d_hash";
        public const string CreateAt = "create_at";
        public const string UpdateAt = "update_at";
        public const string Data = "data";
        public const string Title = "title";
        public const string Description = "desc";
        public const string DataVersion = "v";
        public const string Pages = "page";
        public const string PagesId = "i";
        public const string PagesItemId = "it";
        public const string PagesPage = "p";
        public const string Annotations = "anno";
        public const string AnnotationsId = "i";
        public const string AnnotationsContentHash = "h";
        public const string ScoreCount = "score_count";
        public const string Scores = "scores";
        public const string Access = "access";
        public const string SnapshotName = "snapname";
        public const string SnapshotCount = "s_count";

        public static class DataPropertyNames
        {
            public const string PageCount = "p_count";
            public const string AnnotationCount = "a_count";
        }
    }
}
