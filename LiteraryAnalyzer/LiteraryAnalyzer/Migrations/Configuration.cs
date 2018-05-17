namespace LiteraryAnalyzer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LiteraryAnalyzer.LiteraryAnalyzerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "LiteraryAnalyzer.LiteraryAnalyzerContext";
        }

        protected override void Seed(LiteraryAnalyzer.LiteraryAnalyzerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
