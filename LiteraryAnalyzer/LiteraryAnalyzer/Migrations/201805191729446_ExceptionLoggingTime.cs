namespace LiteraryAnalyzer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExceptionLoggingTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExceptionLogs", "OccuranceTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExceptionLogs", "OccuranceTime");
        }
    }
}
