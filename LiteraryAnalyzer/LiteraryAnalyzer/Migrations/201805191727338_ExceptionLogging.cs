namespace LiteraryAnalyzer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExceptionLogging : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExceptionLogs",
                c => new
                    {
                        ExceptionLogID = c.Int(nullable: false, identity: true),
                        StackTrace = c.String(),
                        Message = c.String(),
                        Source = c.String(),
                        ExToString = c.String(),
                        Commentary = c.String(),
                    })
                .PrimaryKey(t => t.ExceptionLogID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExceptionLogs");
        }
    }
}
