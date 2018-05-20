namespace LiteraryAnalyzer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarkdownOptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MarkdownOptions",
                c => new
                    {
                        MarkdownOptionID = c.Int(nullable: false, identity: true),
                        URIOption = c.Int(nullable: false),
                        ContentsOption = c.Int(nullable: false),
                        ParserOption = c.Int(nullable: false),
                        BaseDir = c.String(),
                        Filename = c.String(),
                        Prefix = c.String(),
                    })
                .PrimaryKey(t => t.MarkdownOptionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MarkdownOptions");
        }
    }
}
